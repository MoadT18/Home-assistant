using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.WinForms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Drawing;

using System.IO;

namespace Home_assistant
{
    public partial class Form1 : Form
    {
        private readonly ClimateDataService climateService;
        private readonly AIResponseService aiService;
        private readonly Timer thresholdTimer;
        private readonly NotifyIcon notifyIcon;
        private bool hasAlerted = false;
        private const int CO2_THRESHOLD = 1000;
        private Panel panelAlertDropdown1;
        private ListBox lstAlerts1;

        private int lastDataId = -1;

        private readonly List<string> alertHistory = new List<string>();

        // 1) Class‐level veld en helper–methode
        private bool hasUnreadAlerts = false;

        private void UpdateAlertIcon()
        {
            btnViewAlerts.Image = hasUnreadAlerts
                ? Properties.Resources.notificationAlert
                : Properties.Resources.notification;
        }
        public Form1()
        {
            InitializeComponent();
            climateService = new ClimateDataService();
            aiService = new AIResponseService();
            FetchClimateData();

            // Initialize chart
            chartClimate.Series = new SeriesCollection();
            chartClimate.AxisX.Add(new Axis { Title = "Time", Labels = new List<string>() });
            chartClimate.AxisY.Add(new Axis { Title = "Measurements", MinValue = 0 });

            chartClimate.AxisX.Add(new Axis { Title = "Measurement Time" });
            chartClimate.AxisY.Add(new Axis { Title = "Values" });



            // 1) Set up NotifyIcon (you’ll need an Icon resource)
            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Warning,  // or your custom .ico
                Visible = true,
                BalloonTipTitle = "CO₂ Warning"
            };

            // 2) Set up a 5s Timer to poll the API
            thresholdTimer = new Timer { Interval = 5000 };
            thresholdTimer.Tick += ThresholdTimer_Tick;
            thresholdTimer.Start();

            // 2) In Form1() na InitializeComponent():
            UpdateAlertIcon();

        }

        private async void FetchClimateData()
        {
            var dataDict = await climateService.FetchClimateData();
            if (dataDict == null || dataDict.Count == 0)
            {
                MessageBox.Show("⚠️ Failed to fetch climate data. Check your API connection!", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var labels = new List<string>();
            var temperatureValues = new ChartValues<double>();
            var co2Values = new ChartValues<double>();
            var humidityValues = new ChartValues<double>();

            for (int hour = 0; hour <= 23; hour++)
            {
                labels.Add(hour.ToString("00") + ":00");
                if (dataDict.ContainsKey(hour))
                {
                    temperatureValues.Add(dataDict[hour].temperature);
                    co2Values.Add(dataDict[hour].co2);
                    humidityValues.Add(dataDict[hour].humidity);
                }
                else
                {
                    temperatureValues.Add(0);
                    co2Values.Add(0); // Baseline CO₂
                    humidityValues.Add(0);
                }
            }

            chartClimate.Series.Clear();
            chartClimate.Series.Add(new LineSeries
            {
                Title = "🌡 Temp (°C)",
                Values = temperatureValues,
                StrokeThickness = 3,
                PointGeometrySize = 10,
                Stroke = System.Windows.Media.Brushes.SteelBlue,
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 70, 130, 180))
            });
            chartClimate.Series.Add(new LineSeries
            {
                Title = "💨 CO₂ (ppm)",
                Values = co2Values,
                StrokeThickness = 3,
                PointGeometrySize = 10,
                ScalesYAt = 1,
                Stroke = System.Windows.Media.Brushes.IndianRed,
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 205, 92, 92))
            });
            chartClimate.Series.Add(new LineSeries
            {
                Title = "💧 Humidity (%)",
                Values = humidityValues,
                StrokeThickness = 3,
                PointGeometrySize = 10,
                Stroke = System.Windows.Media.Brushes.SeaGreen,
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 60, 179, 113))
            });

            chartClimate.AxisX.Clear();
            chartClimate.AxisX.Add(new Axis
            {
                Title = "Hour of Day",
                Labels = labels,
                Separator = new Separator { Step = 2, IsEnabled = false },
                Foreground = System.Windows.Media.Brushes.Gray,
                FontSize = 12
            });

            chartClimate.AxisY.Clear();
            chartClimate.AxisY.Add(new Axis
            {
                Title = "Temperature (°C) / Humidity (%)",
                MinValue = 0,
                MaxValue = 100,
                Separator = new Separator { Stroke = System.Windows.Media.Brushes.LightGray },
                Foreground = System.Windows.Media.Brushes.Gray,
                FontSize = 12
            });

            chartClimate.AxisY.Add(new Axis
            {
                Title = "CO₂ (ppm)",
                Position = AxisPosition.RightTop,
                MinValue = 0,
                MaxValue = Math.Ceiling(co2Values.Max() * 1.2),
                Separator = new Separator { Stroke = System.Windows.Media.Brushes.LightGray },
                Foreground = System.Windows.Media.Brushes.Red,
                FontSize = 12
            });
        }

        private async void btnTemperature_Click(object sender, EventArgs e)
        {
            try
            {
                // Laat de spinner zien
                loadingOverlay1.ShowOverlay();

                // Haal data op
                var (id, temperature, co2, humidity, timestamp)
                    = await climateService.FetchLatestClimateData();
                string formattedTime
                    = DateTime.Parse(timestamp).ToString("dd-MM-yyyy HH:mm");

              /*  string prompt
                    = $"Provide only the current temperature and the exact time it was measured. " +
                      $"The last measurement was at {formattedTime}, with temperature: {temperature}°C.";*/

                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + "What is my temperature right now?" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Get AI response
/*                string response = await aiService.GetAIResponse(prompt);
*/
                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + $"📍 Last Measured at: {formattedTime}\n🌡 Temperature: {temperature}°C" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Scroll naar beneden
                txtChatHistory.ScrollToCaret();

                // Toon resultaat in MessageBox
                MessageBox.Show(
                    $"📍 Last Measured at: {formattedTime}\n🌡 Temperature: {temperature}°C",
                    "Temperature Data", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Fout opgetreden: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }
            finally
            {
                // Verberg de spinner
                loadingOverlay1.HideOverlay();
            }
        }


        private async void btnAirQuality_Click(object sender, EventArgs e)
        {
            try
            {
                loadingOverlay1.ShowOverlay();

                var (id, temperature, co2, humidity, timestamp)
                    = await climateService.FetchLatestClimateData();
                string formattedTime = DateTime.Parse(timestamp)
                                          .ToString("dd-MM-yyyy HH:mm");

                //string prompt = $"Provide only the current CO₂ level and the exact time it was measured. " +
                               // $"The last measurement was at {formattedTime}, with CO₂ level: {co2} ppm.";

                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + "What is my co2 right now?" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Get AI response
                //string response = await aiService.GetAIResponse(prompt);

                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + $"📍 Last Measured at: {formattedTime}\n🌿 CO₂ Level: {co2} ppm" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Scroll to bottom
                txtChatHistory.ScrollToCaret();

                // Show message box as before
                MessageBox.Show(
                    $"📍 Last Measured at: {formattedTime}\n🌿 CO₂ Level: {co2} ppm",
                    "Indoor Air Quality", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Fout opgetreden: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        }




        private async void btnHumidity_Click(object sender, EventArgs e)
        {
            try
            {
                loadingOverlay1.ShowOverlay();

                var (id, temperature, co2, humidity, timestamp)
                    = await climateService.FetchLatestClimateData();
                string formattedTime = DateTime.Parse(timestamp)
                                          .ToString("dd-MM-yyyy HH:mm");

               /* string prompt = $"Provide only the current humidity percentage and the exact time it was measured. " +
                                $"The last measurement was at {formattedTime}, with humidity: {humidity}%.";
*/
                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + "What is my humidity right now?" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Get AI response
/*                string response = await aiService.GetAIResponse(prompt);
*/
                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + $"📍 Last Measured at: {formattedTime}\n💧 Humidity: {humidity}%" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Scroll to bottom
                txtChatHistory.ScrollToCaret();

                // Show message box as before
                MessageBox.Show(
                    $"📍 Last Measured at: {formattedTime}\n💧 Humidity: {humidity}%",
                    "Humidity Data", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Fout opgetreden: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        }




        private async void btnGeneralAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                loadingOverlay1.ShowOverlay();

                var (id, temperature, co2, humidity, timestamp)
                    = await climateService.FetchLatestClimateData();
                string formattedTime = DateTime.Parse(timestamp)
                                          .ToString("dd-MM-yyyy HH:mm");

                string prompt = "Please analyze today’s indoor air quality based on all measurements collected throughout the day. For each parameter (temperature, humidity, and CO₂), summarize the trend or average, and explain whether the values were mostly ideal, too high, or too low. Always refer to specific times using the hh:mm format (e.g., 14:00 or 09:30), not general terms like 'midday' or 'early morning'. Then provide clear, personalized recommendations to improve air quality and comfort.";

                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + prompt + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Get AI response
                string response = await aiService.GetAIResponse(prompt);

                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + response + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Scroll to bottom
                txtChatHistory.ScrollToCaret();

                // Show detailed analysis
                string formattedMessage = $"🧠 **Luna's Climate Analysis**\n\n" +
                                          $"📍 Measured at: {formattedTime}\n\n" +
                                          $"🌿 CO₂ Level: {co2} ppm\n" +
                                          $"🌡 Temperature: {temperature}°C\n" +
                                          $"💧 Humidity: {humidity}%\n\n" +
                                          $"📋 **AI Advice:**\n{response}";
                MessageBox.Show(formattedMessage, "General Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout opgetreden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        }






        private async void btnSendChat_Click(object sender, EventArgs e)
        {
            string userMessage = txtChatInput.Text.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;

            bool requestLatestData = userMessage.ToLower().Contains("time") ||
                                     userMessage.ToLower().Contains("when") ||
                                     userMessage.ToLower().Contains("last measure");

            // Your prompt in blue, "You:" bold
            txtChatHistory.SelectionStart = txtChatHistory.TextLength;
            txtChatHistory.SelectionColor = Color.Blue;
            txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
            txtChatHistory.AppendText("You:");
            txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
            txtChatHistory.AppendText(" " + userMessage + Environment.NewLine);
            txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

            // Get AI response
            string response = await aiService.GetAIResponse(userMessage, requestLatestData);

            // Luna’s response in green, "Luna:" bold
            txtChatHistory.SelectionStart = txtChatHistory.TextLength;
            txtChatHistory.SelectionColor = Color.Green;
            txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
            txtChatHistory.AppendText("Luna:");
            txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
            txtChatHistory.AppendText(" " + response + Environment.NewLine);
            txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

            txtChatHistory.ScrollToCaret();
            txtChatInput.Clear();
        }


        public async Task<string> GetAIResponse(string prompt, bool includeLatestData = false)
        {
            try
            {
                string finalPrompt = prompt;
                if (includeLatestData)
                {
                    var (id, temperature, co2, humidity, timestamp) = await climateService.FetchLatestClimateData();
                    if (timestamp != "Unknown")
                    {
                        finalPrompt += $" The latest measurement was recorded at {timestamp}. " +
                                       $"CO₂: {co2} ppm, Temp: {temperature}°C, Humidity: {humidity}%." +
                                       $"Ensure your response includes this timestamp.";
                    }
                }

                return await aiService.GetAIResponse(finalPrompt);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            // This method is required but can be left empty if no functionality is needed.
        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            // This method is required but can be left empty if no functionality is needed.
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }



        private async void btnForecastAdvice_Click(object sender, EventArgs e)
        {
            try
            {
                loadingOverlay1.ShowOverlay();

                string forecastPrompt = "What’s the forecast advice for the coming days?";

                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + forecastPrompt + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Get AI response
                string response = await aiService.GetAIResponse(forecastPrompt);

                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + response + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Scroll to bottom
                txtChatHistory.ScrollToCaret();

                // Show forecast advice
                MessageBox.Show(response, "7-Day Forecast Advice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout opgetreden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        }




        // 3) ThresholdTimer_Tick
        private async void ThresholdTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var (id, temp, co2, hum, ts) = await climateService.FetchLatestClimateData();

                if (id != lastDataId && co2 >= CO2_THRESHOLD)
                {
                    lastDataId = id;
                    string alertMessage = $"[{ts}] ⚠️ CO₂ level is {co2} ppm – please ventilate!";
                    notifyIcon.BalloonTipText = alertMessage;
                    notifyIcon.ShowBalloonTip(5000);

                    alertHistory.Add(alertMessage);
                    hasUnreadAlerts = true;
                    UpdateAlertIcon();
                }
                else if (id != lastDataId)
                {
                    lastDataId = id;
                }
            }
            catch { /* ignore */ }
        }

        // 4) btnViewAlerts_Click
        private void btnViewAlerts_Click(object sender, EventArgs e)
        {
            panelAlertDropdown.Visible = !panelAlertDropdown.Visible;

            if (panelAlertDropdown.Visible)
            {
                lstAlerts.Items.Clear();
                if (alertHistory.Count == 0)
                    lstAlerts.Items.Add("✅ No CO₂ alerts recorded.");
                else
                    alertHistory.ForEach(a => lstAlerts.Items.Add(a));

                hasUnreadAlerts = false;
                UpdateAlertIcon();
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (panelAlertDropdown.Visible && !panelAlertDropdown.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                panelAlertDropdown.Visible = false;
            }
        }


        private void lstAlerts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void measure_Click(object sender, EventArgs e)
        {
            // Maak textbox schoon voor nieuwe run
            cronTextBox.Clear();

            try
            {
                loadingOverlay1.ShowOverlay();

                string forecastPrompt = "measure now";
                cronTextBox.AppendText("🟦 You: " + forecastPrompt + Environment.NewLine);

                string response = await aiService.GetAIResponse(forecastPrompt);

                cronTextBox.AppendText("🟩 Luna: " + response.Trim() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                cronTextBox.AppendText("❌ EXCEPTION: " + ex.Message + Environment.NewLine);
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        }

        private async void forecast_Click(object sender, EventArgs e)
        {
            // Maak textbox schoon voor nieuwe run
            cronTextBox.Clear();

            try
            {
                loadingOverlay1.ShowOverlay();

                string forecastPrompt = "run forecast";
                cronTextBox.AppendText("🟦 You: " + forecastPrompt + Environment.NewLine);

                string response = await aiService.GetAIResponse(forecastPrompt);

                cronTextBox.AppendText("🟩 Luna: " + response.Trim() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                cronTextBox.AppendText("❌ EXCEPTION: " + ex.Message + Environment.NewLine);
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        }

        private void loadingOverlay1_Load(object sender, EventArgs e)
        {

        }

        private void tabClimate_Click(object sender, EventArgs e)
        {

        }

        private void txtChatHistory_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChatHistory_TextChanged_1(object sender, EventArgs e)
        {

        }

        private async void bttnweatherToday_Click(object sender, EventArgs e)
        {
            try
            {
                loadingOverlay1.ShowOverlay();

                string prompt = "What is the weather today?";

                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + prompt + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Get AI response
                string response = await aiService.GetAIResponse(prompt);

                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + response + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Scroll to bottom
                txtChatHistory.ScrollToCaret();

                // Show forecast advice
                MessageBox.Show(response, "Weather update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout opgetreden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        
    }

        private async void bttnweatherTomorrow_Click(object sender, EventArgs e)
        {
            try
            {
                loadingOverlay1.ShowOverlay();

                string prompt = "What is the weather tomorrow?";

                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + prompt + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Get AI response
                string response = await aiService.GetAIResponse(prompt);

                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + response + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Scroll to bottom
                txtChatHistory.ScrollToCaret();

                // Show forecast advice
                MessageBox.Show(response, "Weather update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout opgetreden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadingOverlay1.HideOverlay();
            }
        }

        private void layoutButtons_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
