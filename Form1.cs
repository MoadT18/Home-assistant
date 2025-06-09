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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;


namespace Home_assistant
{
    public partial class Form1 : Form
    {
        private readonly ClimateDataService climateService; 
        private readonly AIResponseService aiService;
        private readonly Timer thresholdTimer;
        private readonly NotifyIcon notifyIcon;
        private bool hasAlerted = false;
        private const int CO2_THRESHOLD = 900;
        private Panel panelAlertDropdown1;
        private ListBox lstAlerts1;
        private readonly Timer forecastTimer;


        private int lastDataId = -1;

        private readonly List<string> alertHistory = new List<string>();


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
            SetupPdfFonts();

            climateService = new ClimateDataService();
            aiService = new AIResponseService();

            ShowLoadingChart();
            chartClimate.Refresh();
            labelLoading.Visible = true;
            chartClimate.Visible = false;
            this.Load += async (s, e) => await OnFormLoadAsync();







            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Warning,
                Visible = true,
                BalloonTipTitle = "CO₂ Warning"
            };

            // Set up a 5s Timer to poll the API
            thresholdTimer = new Timer { Interval = 5000 };
            thresholdTimer.Tick += ThresholdTimer_Tick;
            thresholdTimer.Start();


            // Forecast waarschuwing bij opstart + om de 2 uur
            forecastTimer = new Timer { Interval = 2 * 60 * 60 * 1000 }; // 2 uur in ms
            forecastTimer.Tick += async (s, e) => await CheckForecastWarnings();
            forecastTimer.Start();

            // Run onmiddellijk bij opstart
            _ = CheckForecastWarnings();

            UpdateAlertIcon();

        }

        private void ShowLoadingChart()
        {
            chartClimate.Series = new SeriesCollection
    {
        new LineSeries
        {
            Title = "Loading...",
            Values = new ChartValues<double> { 0, 0, 0, 0, 0 },
            StrokeThickness = 2,
            PointGeometry = null,
            Stroke = System.Windows.Media.Brushes.Gray,
            Fill = System.Windows.Media.Brushes.Transparent
        }
    };

            chartClimate.AxisX.Clear();
            chartClimate.AxisX.Add(new Axis
            {
                Labels = new[] { "", "", "", "", "" },
                Foreground = System.Windows.Media.Brushes.Gray
            });

            chartClimate.AxisY.Clear();
            chartClimate.AxisY.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 100,
                Foreground = System.Windows.Media.Brushes.Gray
            });
        }

        private async Task OnFormLoadAsync()
        {
            await FetchClimateData();

            labelLoading.Visible = false;
            chartClimate.Visible = true;
        }

        private async Task FetchClimateData()
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
                    co2Values.Add(0);
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

            // Advisory line for temperature (25°C)
            chartClimate.AxisY[0].Sections = new SectionsCollection
{
    new AxisSection
    {
        Value = 25,
        Stroke = System.Windows.Media.Brushes.Blue,
        StrokeThickness = 2,
        StrokeDashArray = new System.Windows.Media.DoubleCollection { 4 },
        SectionWidth = 0,
        Label = "Comfort Max Temp"
    }
};

            // Advisory line for CO2 (1000 ppm)
            chartClimate.AxisY[1].Sections = new SectionsCollection
{
    new AxisSection
    {
        Value = 1000,
        Stroke = System.Windows.Media.Brushes.Red,
        StrokeThickness = 2,
        StrokeDashArray = new System.Windows.Media.DoubleCollection { 4 },
        SectionWidth = 0,
        Label = "CO₂ Limit"
    }
};

        }

        private async void btnTemperature_Click(object sender, EventArgs e)
        {
            try
            {

                loadingOverlay1.ShowOverlay();


                var (id, temperature, co2, humidity, timestamp)
                    = await climateService.FetchLatestClimateData();
                string formattedTime
                    = DateTime.Parse(timestamp).ToString("dd-MM-yyyy HH:mm");



                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + "What is my temperature right now?" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + $"📍 Last Measured at: {formattedTime}\n🌡 Temperature: {temperature}°C" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;


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



                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + "What is my co2 right now?" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;


                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + $"📍 Last Measured at: {formattedTime}\n🌿 CO₂ Level: {co2} ppm" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                txtChatHistory.ScrollToCaret();

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


                // Your prompt in blue, "You:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + "What is my humidity right now?" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;


                // Luna’s response in green, "Luna:" bold
                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + $"📍 Last Measured at: {formattedTime}\n💧 Humidity: {humidity}%" + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                txtChatHistory.ScrollToCaret();

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
            string formattedTime = "";
            double co2 = 0, temperature = 0, humidity = 0;
            string response = "";

            try
            {
                loadingOverlay1.ShowOverlay();

                var (id, temp, co2Val, humVal, timestamp) = await climateService.FetchLatestClimateData();
                temperature = temp;
                co2 = co2Val;
                humidity = humVal;
                formattedTime = DateTime.Parse(timestamp).ToString("dd-MM-yyyy HH:mm");

                string prompt = "Analyze today’s indoor air quality using all measurements. For temperature, humidity, and CO₂, summarize the trend or average, and note if values were mostly ideal, too high, or too low. Use exact times (hh:mm), not vague terms like 'midday'. Provide clear, personalized tips to improve comfort and air quality. Do not use any bold formatting!";

                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + prompt + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                response = await aiService.GetAIResponse(prompt);

                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + response + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                txtChatHistory.ScrollToCaret();

                string formattedMessage = $"🧠 Luna's Climate Analysis\n\n" +
                                          $"📍 Last Measured at: {formattedTime}\n\n" +
                                          $"🌿 CO₂ Level: {co2} ppm\n" +
                                          $"🌡 Temperature: {temperature}°C\n" +
                                          $"💧 Humidity: {humidity}%\n\n" +
                                          $"📋 AI Advice:\n{response}";
                MessageBox.Show(formattedMessage, "General Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // PDF Opslaan
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveDialog.FileName = "Daily_Climate_Analysis_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        GenerateGeneralAnalysisPdf(
                            formattedTime,
                            co2,
                            temperature,
                            humidity,
                            response,
                            saveDialog.FileName
                        );
                        MessageBox.Show("PDF successfully saved!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
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






        private void SetupPdfFonts()
        {
            // Enable Windows fonts for PdfSharp Core build
            try
            {
                GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Could not set Windows fonts: {ex.Message}");
            }
        }

        private async void btnForecastAdvice_Click(object sender, EventArgs e)
        {
            try
            {
                loadingOverlay1.ShowOverlay();

                string forecastPrompt = "What’s the forecast advice for the coming days?";

                string response = await aiService.GetAIResponse(forecastPrompt);

                txtChatHistory.SelectionStart = txtChatHistory.TextLength;
                txtChatHistory.SelectionColor = Color.Blue;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("You:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + forecastPrompt + Environment.NewLine);

                txtChatHistory.SelectionColor = Color.Green;
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Bold);
                txtChatHistory.AppendText("Luna:");
                txtChatHistory.SelectionFont = new Font(txtChatHistory.Font, FontStyle.Regular);
                txtChatHistory.AppendText(" " + response + Environment.NewLine);
                txtChatHistory.SelectionColor = txtChatHistory.ForeColor;

                txtChatHistory.ScrollToCaret();

                MessageBox.Show(response, "7-Day Forecast Advice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Download als PDF
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveDialog.FileName = "IndoorForecast_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        GenerateForecastPdf(response, saveDialog.FileName);
                        MessageBox.Show("PDF successfully saved!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
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




        private void GenerateForecastPdf(string content, string filePath)
        {
            try
            {
                SetupPdfFonts();

                PdfDocument document = new PdfDocument();
                document.Info.Title = "7-Day Indoor Forecast";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
                XFont bodyFont = new XFont("Arial", 12, XFontStyleEx.Regular);
                XFont boldFont = new XFont("Arial", 12, XFontStyleEx.Bold);

                double margin = 40;
                double y = margin;
                double lineHeight = 18;

                gfx.DrawString("7-Day Forecast Advice", titleFont, XBrushes.DarkBlue,
                    new XRect(margin, y, page.Width - 2 * margin, 40), XStringFormats.TopLeft);
                y += 50;

                gfx.DrawString($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm}", bodyFont, XBrushes.Gray,
                    new XRect(margin, y, page.Width - 2 * margin, 20), XStringFormats.TopLeft);
                y += 30;

                string[] dayBlocks = content.Split(new[] { "Date: " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var block in dayBlocks)
                {
                    if (y > page.Height - margin - 100)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = margin;
                    }

                    string trimmed = block.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed)) continue;

                    string[] lines = trimmed.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    string dateLine = lines[0].Trim();
                    string forecastLine = "";
                    string adviceLine = "";

                    foreach (string l in lines.Skip(1))
                    {
                        if (l.StartsWith("Forecast:"))
                            forecastLine = l.Replace("Forecast:", "").Trim();
                        else if (l.StartsWith("Advice:"))
                            adviceLine += l.Replace("Advice:", "").Trim() + " ";
                        else
                            adviceLine += l.Trim() + " ";
                    }

                    y = DrawWrappedLabelValue(gfx, page, "Date:", dateLine, boldFont, bodyFont, margin, y);
                    y = DrawWrappedLabelValue(gfx, page, "Forecast:", forecastLine, boldFont, bodyFont, margin, y);
                    y = DrawWrappedLabelValue(gfx, page, "Advice:", adviceLine.Trim(), boldFont, bodyFont, margin, y);
                    y += 20;
                }

                document.Save(filePath);
                document.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF Generation failed: {ex.Message}", ex);
            }
        }



        // Helper method for text wrapping
        private List<string> WrapText(string text, int maxLength)
        {
            var lines = new List<string>();
            var words = text.Split(' ');
            var currentLine = "";

            foreach (var word in words)
            {
                if ((currentLine + " " + word).Trim().Length <= maxLength)
                {
                    currentLine += (currentLine.Length > 0 ? " " : "") + word;
                }
                else
                {
                    lines.Add(currentLine);
                    currentLine = word;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine);

            return lines;
        }


        private double DrawWrappedLabelValue(XGraphics gfx, PdfPage page, string label, string value, XFont labelFont, XFont valueFont, double margin, double y)
        {
            double lineHeight = 18;
            double availableWidth = page.Width - 2 * margin;

            gfx.DrawString(label, labelFont, XBrushes.Black,
                new XRect(margin, y, availableWidth, lineHeight), XStringFormats.TopLeft);
            y += lineHeight;

            var wrappedLines = WrapText(value, 100);
            foreach (var line in wrappedLines)
            {
                gfx.DrawString(line, valueFont, XBrushes.Black,
                    new XRect(margin + 10, y, availableWidth - 10, lineHeight), XStringFormats.TopLeft);
                y += lineHeight;
            }

            return y + 5;
        }

        private void GenerateGeneralAnalysisPdf(string timestamp, double co2, double temp, double humidity, string aiAdvice, string filePath)
        {
            try
            {
                SetupPdfFonts();

                PdfDocument document = new PdfDocument();
                document.Info.Title = "Luna's Daily Climate Analysis";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
                XFont labelFont = new XFont("Arial", 12, XFontStyleEx.Bold);
                XFont valueFont = new XFont("Arial", 12, XFontStyleEx.Regular);

                double margin = 40;
                double y = margin;
                double lineHeight = 18;
                double availableWidth = page.Width - 2 * margin;

                gfx.DrawString("Luna’s Climate Analysis", titleFont, XBrushes.DarkBlue,
                    new XRect(margin, y, availableWidth, lineHeight), XStringFormats.TopLeft);
                y += 40;

                y = DrawWrappedLabelValue(gfx, page, "Last Measured at:", timestamp, labelFont, valueFont, margin, y);
                y = DrawWrappedLabelValue(gfx, page, "CO₂ Level:", co2 + " ppm", labelFont, valueFont, margin, y);
                y = DrawWrappedLabelValue(gfx, page, "Temperature:", temp.ToString("0.00") + "°C", labelFont, valueFont, margin, y);
                y = DrawWrappedLabelValue(gfx, page, "Humidity:", humidity.ToString("0.00") + "%", labelFont, valueFont, margin, y);

                y += 10;
                y = DrawWrappedLabelValue(gfx, page, "AI Advice:", "", labelFont, valueFont, margin, y);

                var adviceLines = aiAdvice.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in adviceLines)
                {
                    string trimmed = line.Trim();

                    if (trimmed.ToLower().Contains("tips"))
                    {
                        y += 10;
                    }

                    bool isSectionTitle =
                        (trimmed.EndsWith(":") && trimmed.Length < 40 && !trimmed.StartsWith("-") &&
                         !trimmed.Contains(".") && !trimmed.Contains("•") && char.IsUpper(trimmed[0]))
                        || trimmed.StartsWith("1.") || trimmed.StartsWith("2.") || trimmed.StartsWith("3.") || trimmed.StartsWith("4.");

                    if (isSectionTitle)
                    {
                        y += 10;

                        if (y > page.Height - margin - lineHeight)
                        {
                            page = document.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            y = margin;
                        }

                        if (trimmed.Contains(":") && trimmed.Length > 3 && trimmed[1] == '.' && char.IsDigit(trimmed[0]))
                        {
                            int colonIndex = trimmed.IndexOf(":");
                            string boldPart = trimmed.Substring(0, colonIndex + 1); // incl. :
                            string normalPart = trimmed.Substring(colonIndex + 1).Trim();

                            gfx.DrawString(boldPart, labelFont, XBrushes.Black,
                                new XRect(margin, y, availableWidth, lineHeight), XStringFormats.TopLeft);
                            y += lineHeight;

                            var wrappedLines = WrapText(normalPart, 100);
                            foreach (var linePart in wrappedLines)
                            {
                                gfx.DrawString(linePart, valueFont, XBrushes.Black,
                                    new XRect(margin + 10, y, availableWidth - 10, lineHeight), XStringFormats.TopLeft);
                                y += lineHeight;
                            }

                            y += 6;
                        }
                        else
                        {
                            gfx.DrawString(trimmed, labelFont, XBrushes.Black,
                                new XRect(margin, y, availableWidth, lineHeight), XStringFormats.TopLeft);
                            y += lineHeight;
                        }
                    }
                    else
                    {
                        var wrappedLines = WrapText(trimmed, 100);
                        foreach (var wrappedLine in wrappedLines)
                        {
                            if (y > page.Height - margin - lineHeight)
                            {
                                page = document.AddPage();
                                gfx = XGraphics.FromPdfPage(page);
                                y = margin;
                            }

                            gfx.DrawString(wrappedLine, valueFont, XBrushes.Black,
                                new XRect(margin + 10, y, availableWidth - 10, lineHeight), XStringFormats.TopLeft);
                            y += lineHeight;
                        }

                        y += 6;
                    }
                }

                document.Save(filePath);
                document.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF generation failed: " + ex.Message);
            }
        }






        // ThresholdTimer_Tick
        private async void ThresholdTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var (id, temp, co2, hum, ts) = await climateService.FetchLatestClimateData();

                if (id != lastDataId)
                {
                    lastDataId = id;

                    // Update de grafiek bij nieuwe data
                    await FetchClimateData();

                    if (co2 >= CO2_THRESHOLD)
                    {
                        string alertMessage = $"[{ts}] ⚠️ CO₂ level is {co2} ppm – please ventilate!";
                        notifyIcon.BalloonTipText = alertMessage;
                        notifyIcon.ShowBalloonTip(5000);

                        alertHistory.Add(alertMessage);
                        hasUnreadAlerts = true;
                        UpdateAlertIcon();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Realtime update error: " + ex.Message);
            }
        }


        // btnViewAlerts_Click
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

                txtChatHistory.ScrollToCaret();

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

                txtChatHistory.ScrollToCaret();

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

        private async Task CheckForecastWarnings()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("http://pi.local:8001/forecast");
                    dynamic forecastData = JsonConvert.DeserializeObject(response);

                    // [🔧] Normaal: gebruik vandaag
                    //DateTime today = DateTime.Today;

                    // [🧪] Voor testen met specifieke datum, de-comment:
                     DateTime today = new DateTime(2025, 5, 29);

                    foreach (var entry in forecastData)
                    {
                        DateTime forecastDate = DateTime.Parse(entry.ds.ToString()).Date;
                        if (forecastDate == today)
                        {
                            double predCo2 = (double)entry.pred_co2;
                            double predTemp = (double)entry.pred_temp;

                            List<string> warnings = new List<string>();

                            if (predCo2 >= 1000)
                                warnings.Add($"🔴 Forecast: CO₂ may reach {Math.Round(predCo2)} ppm");

                            if (predTemp >= 25)
                                warnings.Add($"🔥 Forecast: Temperature may rise to {Math.Round(predTemp)}°C");

                            if (warnings.Any())
                            {
                                string timestamp = DateTime.Now.ToString("HH:mm");
                                string alertText = $"[{timestamp}] {string.Join(" | ", warnings)}";

                                // Toon melding
                                notifyIcon.BalloonTipTitle = "📊 Forecast Warning";
                                notifyIcon.BalloonTipText = alertText + "\n👉 Prepare to ventilate or cool down.";
                                notifyIcon.ShowBalloonTip(10000);

                                // Voeg toe aan geschiedenis
                                alertHistory.Add(alertText);
                                hasUnreadAlerts = true;
                                UpdateAlertIcon();

                                // Als het alertpaneel open is, direct updaten
                                if (panelAlertDropdown.Visible)
                                {
                                    lstAlerts.Items.Insert(0, alertText);
                                }
                            }

                            break; // enkel eerste match vandaag
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error during forecast check: " + ex.Message);
            }
        }



        private void loadingOverlay1_Load(object sender, EventArgs e)
        {

        }

        private void labelLoading_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabClimate_Click(object sender, EventArgs e)
        {

        }

        private void txtChatHistory_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
