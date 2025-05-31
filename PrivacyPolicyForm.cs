using System;
using System.Windows.Forms;

namespace Home_assistant
{
    public partial class PrivacyPolicyForm : Form
    {
        public bool IsAccepted { get; private set; }

        public PrivacyPolicyForm()
        {
            InitializeComponent();
            SetupUI();


        }

        private void SetupUI()
        {
            Text = "Privacy Policy for Luna";
            Width = 600;
            Height = 550;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            TextBox txtPolicy = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Width = 560,
                Height = 400,
                Left = 10,
                Top = 10,
                Font = new System.Drawing.Font("Segoe UI", 10),
                Text =
@"AI Services and Data Processing for Voice Assistant Luna

To enable intelligent responses and real-time environmental feedback, Luna makes use of several cloud-based AI services combined with local data logging and forecasting models.

Used AI Services:
• Google Speech Recognition (STT): Converts your voice into text using Google's cloud platform. Voice data is temporarily processed on their servers and may be stored briefly to improve service quality.
  → https://cloud.google.com/terms/cloud-privacy-notice?hl=en

• ElevenLabs (TTS): Transforms Luna’s generated text into speech and streams it back to your device. Both text and audio may be stored temporarily on ElevenLabs servers.
  → https://elevenlabs.io/privacy-policy

• OpenAI GPT (NLP): Interprets and generates natural language responses based on user input. Messages are processed via OpenAI servers and may be stored depending on privacy configurations.
  → https://openai.com/policies/privacy-policy

Local Data Collection:
Sensor data (CO₂, temperature, humidity, presence) is stored hourly (24×7) in a local `.db` file and exposed via the internal API:
→ http://pi.local:8000/data

Forecasting & Weekly Prediction:
Each Sunday at 23:30, Luna generates a 7-day forecast:
• Temperature: via Prophet model
• CO₂ levels: via SARIMAX model

Forecast results are saved to `forecast_result.json` and made available at:
→ http://pi.local:8001/forecast

Forecast History:
The last four weekly forecasts are stored in `forecast_history.json` and can be accessed at:
→ http://pi.local:8001/forecast/history

Advice:
For each day, Luna provides tailored advice. If CO₂ ≥ 1000 ppm or temperature ≥ 25°C is predicted, the assistant will proactively suggest actions to improve comfort.

By clicking 'I Accept', you acknowledge and agree to the usage of these AI services and data storage methods."
            };

            Button btnAccept = new Button
            {
                Text = "I Accept and Continue",
                Width = 200,
                Height = 40,
                Top = 420,
                Left = (ClientSize.Width - 200) / 2
            };
            btnAccept.Click += (s, e) =>
            {
                IsAccepted = true;
                Close();
            };

            Controls.Add(txtPolicy);
            Controls.Add(btnAccept);


        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(600, 550);
            this.Name = "PrivacyPolicyForm";
            this.ResumeLayout(false);
        }
    }
}
