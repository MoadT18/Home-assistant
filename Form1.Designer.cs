using System;
using System.Drawing;
using System.Windows.Forms;

namespace Home_assistant
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.panelAlertDropdown = new System.Windows.Forms.Panel();
            this.lstAlerts = new System.Windows.Forms.ListBox();
            this.tabCron = new System.Windows.Forms.TabPage();
            this.cronTextBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.measure = new System.Windows.Forms.Button();
            this.forecast = new System.Windows.Forms.Button();
            this.tabChat = new System.Windows.Forms.TabPage();
            this.layoutChatMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelChatHistory = new System.Windows.Forms.Panel();
            this.txtChatHistory = new System.Windows.Forms.RichTextBox();
            this.panelChatInput = new System.Windows.Forms.Panel();
            this.layoutChatInput = new System.Windows.Forms.TableLayoutPanel();
            this.txtChatInput = new System.Windows.Forms.TextBox();
            this.btnSendChat = new System.Windows.Forms.Button();
            this.tabClimate = new System.Windows.Forms.TabPage();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnTemperature = new Home_assistant.RoundButton();
            this.btnGeneralAnalysis = new Home_assistant.RoundButton();
            this.btnForecastAdvice = new Home_assistant.RoundButton();
            this.bttnweatherTomorrow = new Home_assistant.RoundButton();
            this.bttnweatherToday = new Home_assistant.RoundButton();
            this.btnHumidity = new Home_assistant.RoundButton();
            this.btnAirQuality = new Home_assistant.RoundButton();
            this.panelChartContainer = new System.Windows.Forms.Panel();
            this.labelLoading = new System.Windows.Forms.Label();
            this.chartClimate = new LiveCharts.WinForms.CartesianChart();
            this.chartTitle = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.btnViewAlerts = new System.Windows.Forms.Button();
            this.loadingOverlay1 = new Home_assistant.LoadingOverlay();
            this.panelAlertDropdown.SuspendLayout();
            this.tabCron.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabChat.SuspendLayout();
            this.layoutChatMain.SuspendLayout();
            this.panelChatHistory.SuspendLayout();
            this.panelChatInput.SuspendLayout();
            this.layoutChatInput.SuspendLayout();
            this.tabClimate.SuspendLayout();
            this.layoutMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.layoutButtons.SuspendLayout();
            this.panelChartContainer.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // panelAlertDropdown
            // 
            this.panelAlertDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAlertDropdown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAlertDropdown.Controls.Add(this.lstAlerts);
            this.panelAlertDropdown.Location = new System.Drawing.Point(560, 56);
            this.panelAlertDropdown.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panelAlertDropdown.Name = "panelAlertDropdown";
            this.panelAlertDropdown.Size = new System.Drawing.Size(486, 217);
            this.panelAlertDropdown.TabIndex = 1;
            this.panelAlertDropdown.Visible = false;
            // 
            // lstAlerts
            // 
            this.lstAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAlerts.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstAlerts.FormattingEnabled = true;
            this.lstAlerts.IntegralHeight = false;
            this.lstAlerts.ItemHeight = 21;
            this.lstAlerts.Location = new System.Drawing.Point(0, 0);
            this.lstAlerts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstAlerts.Name = "lstAlerts";
            this.lstAlerts.ScrollAlwaysVisible = true;
            this.lstAlerts.Size = new System.Drawing.Size(484, 215);
            this.lstAlerts.TabIndex = 0;
            this.lstAlerts.SelectedIndexChanged += new System.EventHandler(this.lstAlerts_SelectedIndexChanged);
            // 
            // tabCron
            // 
            this.tabCron.Controls.Add(this.cronTextBox);
            this.tabCron.Controls.Add(this.panel2);
            this.tabCron.Location = new System.Drawing.Point(4, 37);
            this.tabCron.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabCron.Name = "tabCron";
            this.tabCron.Size = new System.Drawing.Size(1054, 512);
            this.tabCron.TabIndex = 3;
            this.tabCron.Text = "Cronjobs";
            this.tabCron.UseVisualStyleBackColor = true;
            // 
            // cronTextBox
            // 
            this.cronTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cronTextBox.Location = new System.Drawing.Point(249, 0);
            this.cronTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cronTextBox.Multiline = true;
            this.cronTextBox.Name = "cronTextBox";
            this.cronTextBox.Size = new System.Drawing.Size(805, 512);
            this.cronTextBox.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.measure);
            this.panel2.Controls.Add(this.forecast);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.panel2.Size = new System.Drawing.Size(249, 512);
            this.panel2.TabIndex = 14;
            // 
            // measure
            // 
            this.measure.BackColor = System.Drawing.Color.WhiteSmoke;
            this.measure.Dock = System.Windows.Forms.DockStyle.Top;
            this.measure.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.measure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.measure.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.measure.Location = new System.Drawing.Point(11, 60);
            this.measure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.measure.Name = "measure";
            this.measure.Padding = new System.Windows.Forms.Padding(4);
            this.measure.Size = new System.Drawing.Size(227, 50);
            this.measure.TabIndex = 12;
            this.measure.Text = "Measure now";
            this.measure.UseVisualStyleBackColor = false;
            this.measure.Click += new System.EventHandler(this.measure_Click);
            // 
            // forecast
            // 
            this.forecast.BackColor = System.Drawing.Color.WhiteSmoke;
            this.forecast.Dock = System.Windows.Forms.DockStyle.Top;
            this.forecast.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.forecast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.forecast.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.forecast.Location = new System.Drawing.Point(11, 10);
            this.forecast.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.forecast.Name = "forecast";
            this.forecast.Padding = new System.Windows.Forms.Padding(4);
            this.forecast.Size = new System.Drawing.Size(227, 50);
            this.forecast.TabIndex = 13;
            this.forecast.Text = "Run Forecast";
            this.forecast.UseVisualStyleBackColor = false;
            this.forecast.Click += new System.EventHandler(this.forecast_Click);
            // 
            // tabChat
            // 
            this.tabChat.Controls.Add(this.layoutChatMain);
            this.tabChat.Location = new System.Drawing.Point(4, 37);
            this.tabChat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabChat.Name = "tabChat";
            this.tabChat.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabChat.Size = new System.Drawing.Size(1054, 512);
            this.tabChat.TabIndex = 1;
            this.tabChat.Text = "Chat";
            this.tabChat.UseVisualStyleBackColor = true;
            // 
            // layoutChatMain
            // 
            this.layoutChatMain.ColumnCount = 1;
            this.layoutChatMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutChatMain.Controls.Add(this.panelChatHistory, 0, 0);
            this.layoutChatMain.Controls.Add(this.panelChatInput, 0, 1);
            this.layoutChatMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutChatMain.Location = new System.Drawing.Point(3, 2);
            this.layoutChatMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutChatMain.Name = "layoutChatMain";
            this.layoutChatMain.RowCount = 2;
            this.layoutChatMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutChatMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutChatMain.Size = new System.Drawing.Size(1048, 508);
            this.layoutChatMain.TabIndex = 1;
            // 
            // panelChatHistory
            // 
            this.panelChatHistory.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelChatHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChatHistory.Controls.Add(this.txtChatHistory);
            this.panelChatHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChatHistory.Location = new System.Drawing.Point(0, 0);
            this.panelChatHistory.Margin = new System.Windows.Forms.Padding(0);
            this.panelChatHistory.Name = "panelChatHistory";
            this.panelChatHistory.Padding = new System.Windows.Forms.Padding(10);
            this.panelChatHistory.Size = new System.Drawing.Size(1048, 438);
            this.panelChatHistory.TabIndex = 0;
            // 
            // txtChatHistory
            // 
            this.txtChatHistory.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtChatHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChatHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChatHistory.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.txtChatHistory.Location = new System.Drawing.Point(10, 10);
            this.txtChatHistory.Margin = new System.Windows.Forms.Padding(0);
            this.txtChatHistory.Name = "txtChatHistory";
            this.txtChatHistory.ReadOnly = true;
            this.txtChatHistory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtChatHistory.Size = new System.Drawing.Size(1026, 416);
            this.txtChatHistory.TabIndex = 0;
            this.txtChatHistory.Text = "";
            this.txtChatHistory.TextChanged += new System.EventHandler(this.txtChatHistory_TextChanged_1);
            // 
            // panelChatInput
            // 
            this.panelChatInput.BackColor = System.Drawing.Color.White;
            this.panelChatInput.Controls.Add(this.layoutChatInput);
            this.panelChatInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChatInput.Location = new System.Drawing.Point(0, 438);
            this.panelChatInput.Margin = new System.Windows.Forms.Padding(0);
            this.panelChatInput.Name = "panelChatInput";
            this.panelChatInput.Padding = new System.Windows.Forms.Padding(10);
            this.panelChatInput.Size = new System.Drawing.Size(1048, 70);
            this.panelChatInput.TabIndex = 1;
            // 
            // layoutChatInput
            // 
            this.layoutChatInput.ColumnCount = 2;
            this.layoutChatInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutChatInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.layoutChatInput.Controls.Add(this.txtChatInput, 0, 0);
            this.layoutChatInput.Controls.Add(this.btnSendChat, 1, 0);
            this.layoutChatInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutChatInput.Location = new System.Drawing.Point(10, 10);
            this.layoutChatInput.Margin = new System.Windows.Forms.Padding(0);
            this.layoutChatInput.Name = "layoutChatInput";
            this.layoutChatInput.RowCount = 1;
            this.layoutChatInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutChatInput.Size = new System.Drawing.Size(1028, 50);
            this.layoutChatInput.TabIndex = 0;
            // 
            // txtChatInput
            // 
            this.txtChatInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChatInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChatInput.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChatInput.Location = new System.Drawing.Point(3, 2);
            this.txtChatInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtChatInput.Multiline = true;
            this.txtChatInput.Name = "txtChatInput";
            this.txtChatInput.Size = new System.Drawing.Size(932, 46);
            this.txtChatInput.TabIndex = 8;
            // 
            // btnSendChat
            // 
            this.btnSendChat.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSendChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendChat.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnSendChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendChat.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendChat.ForeColor = System.Drawing.Color.White;
            this.btnSendChat.Location = new System.Drawing.Point(941, 2);
            this.btnSendChat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSendChat.Name = "btnSendChat";
            this.btnSendChat.Size = new System.Drawing.Size(84, 46);
            this.btnSendChat.TabIndex = 9;
            this.btnSendChat.Text = "➡";
            this.btnSendChat.UseVisualStyleBackColor = false;
            this.btnSendChat.Click += new System.EventHandler(this.btnSendChat_Click);
            // 
            // tabClimate
            // 
            this.tabClimate.Controls.Add(this.layoutMain);
            this.tabClimate.Controls.Add(this.chartTitle);
            this.tabClimate.Location = new System.Drawing.Point(4, 37);
            this.tabClimate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabClimate.Name = "tabClimate";
            this.tabClimate.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabClimate.Size = new System.Drawing.Size(1054, 512);
            this.tabClimate.TabIndex = 0;
            this.tabClimate.Text = "Dashboard";
            this.tabClimate.UseVisualStyleBackColor = true;
            this.tabClimate.Click += new System.EventHandler(this.tabClimate_Click);
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.panel1, 0, 0);
            this.layoutMain.Controls.Add(this.panelChartContainer, 1, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(3, 34);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 1;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 476F));
            this.layoutMain.Size = new System.Drawing.Size(1048, 476);
            this.layoutMain.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.layoutButtons);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(8, 7);
            this.panel1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(204, 462);
            this.panel1.TabIndex = 12;
            // 
            // layoutButtons
            // 
            this.layoutButtons.BackColor = System.Drawing.Color.Transparent;
            this.layoutButtons.ColumnCount = 1;
            this.layoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutButtons.Controls.Add(this.btnTemperature, 0, 1);
            this.layoutButtons.Controls.Add(this.btnGeneralAnalysis, 0, 3);
            this.layoutButtons.Controls.Add(this.btnForecastAdvice, 0, 4);
            this.layoutButtons.Controls.Add(this.bttnweatherTomorrow, 0, 6);
            this.layoutButtons.Controls.Add(this.bttnweatherToday, 0, 5);
            this.layoutButtons.Controls.Add(this.btnHumidity, 0, 2);
            this.layoutButtons.Controls.Add(this.btnAirQuality, 0, 0);
            this.layoutButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutButtons.Location = new System.Drawing.Point(10, 10);
            this.layoutButtons.Name = "layoutButtons";
            this.layoutButtons.RowCount = 7;
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutButtons.Size = new System.Drawing.Size(184, 442);
            this.layoutButtons.TabIndex = 11;
            // 
            // btnTemperature
            // 
            this.btnTemperature.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnTemperature.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnTemperature.BorderRadius = 18;
            this.btnTemperature.BorderSize = 1;
            this.btnTemperature.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTemperature.DefaultBackColor = this.btnTemperature.BackColor;
            this.btnTemperature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTemperature.FlatAppearance.BorderSize = 0;
            this.btnTemperature.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTemperature.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTemperature.ForeColor = System.Drawing.Color.Black;
            this.btnTemperature.HoverBackColor = System.Drawing.Color.AliceBlue;
            this.btnTemperature.Location = new System.Drawing.Point(5, 68);
            this.btnTemperature.Margin = new System.Windows.Forms.Padding(5);
            this.btnTemperature.Name = "btnTemperature";
            this.btnTemperature.Padding = new System.Windows.Forms.Padding(5);
            this.btnTemperature.Size = new System.Drawing.Size(174, 53);
            this.btnTemperature.TabIndex = 2;
            this.btnTemperature.Text = "🌡 Check Temp";
            this.btnTemperature.UseVisualStyleBackColor = false;
            this.btnTemperature.Click += new System.EventHandler(this.btnTemperature_Click);
            // 
            // btnGeneralAnalysis
            // 
            this.btnGeneralAnalysis.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnGeneralAnalysis.BorderColor = System.Drawing.Color.MediumBlue;
            this.btnGeneralAnalysis.BorderRadius = 18;
            this.btnGeneralAnalysis.BorderSize = 1;
            this.btnGeneralAnalysis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGeneralAnalysis.DefaultBackColor = this.btnGeneralAnalysis.BackColor;
            this.btnGeneralAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGeneralAnalysis.FlatAppearance.BorderSize = 0;
            this.btnGeneralAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGeneralAnalysis.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGeneralAnalysis.ForeColor = System.Drawing.Color.White;
            this.btnGeneralAnalysis.HoverBackColor = System.Drawing.Color.MediumBlue;
            this.btnGeneralAnalysis.Location = new System.Drawing.Point(5, 194);
            this.btnGeneralAnalysis.Margin = new System.Windows.Forms.Padding(5);
            this.btnGeneralAnalysis.Name = "btnGeneralAnalysis";
            this.btnGeneralAnalysis.Padding = new System.Windows.Forms.Padding(5);
            this.btnGeneralAnalysis.Size = new System.Drawing.Size(174, 53);
            this.btnGeneralAnalysis.TabIndex = 8;
            this.btnGeneralAnalysis.Text = "🧠 Today\'s Advice";
            this.btnGeneralAnalysis.UseVisualStyleBackColor = false;
            this.btnGeneralAnalysis.Click += new System.EventHandler(this.btnGeneralAnalysis_Click);
            // 
            // btnForecastAdvice
            // 
            this.btnForecastAdvice.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnForecastAdvice.BorderColor = System.Drawing.Color.MediumBlue;
            this.btnForecastAdvice.BorderRadius = 18;
            this.btnForecastAdvice.BorderSize = 1;
            this.btnForecastAdvice.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnForecastAdvice.DefaultBackColor = this.btnForecastAdvice.BackColor;
            this.btnForecastAdvice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnForecastAdvice.FlatAppearance.BorderSize = 0;
            this.btnForecastAdvice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnForecastAdvice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnForecastAdvice.ForeColor = System.Drawing.Color.White;
            this.btnForecastAdvice.HoverBackColor = System.Drawing.Color.MediumBlue;
            this.btnForecastAdvice.Location = new System.Drawing.Point(5, 257);
            this.btnForecastAdvice.Margin = new System.Windows.Forms.Padding(5);
            this.btnForecastAdvice.Name = "btnForecastAdvice";
            this.btnForecastAdvice.Padding = new System.Windows.Forms.Padding(5);
            this.btnForecastAdvice.Size = new System.Drawing.Size(174, 53);
            this.btnForecastAdvice.TabIndex = 7;
            this.btnForecastAdvice.Text = "📈 Forecast";
            this.btnForecastAdvice.UseVisualStyleBackColor = false;
            this.btnForecastAdvice.Click += new System.EventHandler(this.btnForecastAdvice_Click);
            // 
            // bttnweatherTomorrow
            // 
            this.bttnweatherTomorrow.BackColor = System.Drawing.Color.Goldenrod;
            this.bttnweatherTomorrow.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.bttnweatherTomorrow.BorderRadius = 18;
            this.bttnweatherTomorrow.BorderSize = 1;
            this.bttnweatherTomorrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bttnweatherTomorrow.DefaultBackColor = this.bttnweatherTomorrow.BackColor;
            this.bttnweatherTomorrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bttnweatherTomorrow.FlatAppearance.BorderSize = 0;
            this.bttnweatherTomorrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttnweatherTomorrow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.bttnweatherTomorrow.ForeColor = System.Drawing.Color.Black;
            this.bttnweatherTomorrow.HoverBackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.bttnweatherTomorrow.Location = new System.Drawing.Point(5, 383);
            this.bttnweatherTomorrow.Margin = new System.Windows.Forms.Padding(5);
            this.bttnweatherTomorrow.Name = "bttnweatherTomorrow";
            this.bttnweatherTomorrow.Padding = new System.Windows.Forms.Padding(5);
            this.bttnweatherTomorrow.Size = new System.Drawing.Size(174, 54);
            this.bttnweatherTomorrow.TabIndex = 10;
            this.bttnweatherTomorrow.Text = "🌤 Weather Tom";
            this.bttnweatherTomorrow.UseVisualStyleBackColor = false;
            this.bttnweatherTomorrow.Click += new System.EventHandler(this.bttnweatherTomorrow_Click);
            // 
            // bttnweatherToday
            // 
            this.bttnweatherToday.BackColor = System.Drawing.Color.Goldenrod;
            this.bttnweatherToday.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.bttnweatherToday.BorderRadius = 18;
            this.bttnweatherToday.BorderSize = 1;
            this.bttnweatherToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bttnweatherToday.DefaultBackColor = this.bttnweatherToday.BackColor;
            this.bttnweatherToday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bttnweatherToday.FlatAppearance.BorderSize = 0;
            this.bttnweatherToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttnweatherToday.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.bttnweatherToday.ForeColor = System.Drawing.Color.Black;
            this.bttnweatherToday.HoverBackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.bttnweatherToday.Location = new System.Drawing.Point(5, 320);
            this.bttnweatherToday.Margin = new System.Windows.Forms.Padding(5);
            this.bttnweatherToday.Name = "bttnweatherToday";
            this.bttnweatherToday.Padding = new System.Windows.Forms.Padding(5);
            this.bttnweatherToday.Size = new System.Drawing.Size(174, 53);
            this.bttnweatherToday.TabIndex = 9;
            this.bttnweatherToday.Text = "☀️ Weather Tod";
            this.bttnweatherToday.UseVisualStyleBackColor = false;
            this.bttnweatherToday.Click += new System.EventHandler(this.bttnweatherToday_Click);
            // 
            // btnHumidity
            // 
            this.btnHumidity.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnHumidity.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnHumidity.BorderRadius = 18;
            this.btnHumidity.BorderSize = 1;
            this.btnHumidity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHumidity.DefaultBackColor = this.btnHumidity.BackColor;
            this.btnHumidity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHumidity.FlatAppearance.BorderSize = 0;
            this.btnHumidity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHumidity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnHumidity.ForeColor = System.Drawing.Color.Black;
            this.btnHumidity.HoverBackColor = System.Drawing.Color.AliceBlue;
            this.btnHumidity.Location = new System.Drawing.Point(5, 131);
            this.btnHumidity.Margin = new System.Windows.Forms.Padding(5);
            this.btnHumidity.Name = "btnHumidity";
            this.btnHumidity.Padding = new System.Windows.Forms.Padding(5);
            this.btnHumidity.Size = new System.Drawing.Size(174, 53);
            this.btnHumidity.TabIndex = 4;
            this.btnHumidity.Text = "💧 Check Hum";
            this.btnHumidity.UseVisualStyleBackColor = false;
            this.btnHumidity.Click += new System.EventHandler(this.btnHumidity_Click);
            // 
            // btnAirQuality
            // 
            this.btnAirQuality.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnAirQuality.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnAirQuality.BorderRadius = 18;
            this.btnAirQuality.BorderSize = 1;
            this.btnAirQuality.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAirQuality.DefaultBackColor = this.btnAirQuality.BackColor;
            this.btnAirQuality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAirQuality.FlatAppearance.BorderSize = 0;
            this.btnAirQuality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAirQuality.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAirQuality.ForeColor = System.Drawing.Color.Black;
            this.btnAirQuality.HoverBackColor = System.Drawing.Color.AliceBlue;
            this.btnAirQuality.Location = new System.Drawing.Point(5, 5);
            this.btnAirQuality.Margin = new System.Windows.Forms.Padding(5);
            this.btnAirQuality.Name = "btnAirQuality";
            this.btnAirQuality.Padding = new System.Windows.Forms.Padding(5);
            this.btnAirQuality.Size = new System.Drawing.Size(174, 53);
            this.btnAirQuality.TabIndex = 15;
            this.btnAirQuality.Text = "🌬 Check CO₂";
            this.btnAirQuality.UseVisualStyleBackColor = false;
            this.btnAirQuality.Click += new System.EventHandler(this.btnAirQuality_Click);
            // 
            // panelChartContainer
            // 
            this.panelChartContainer.BackColor = System.Drawing.Color.White;
            this.panelChartContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChartContainer.Controls.Add(this.labelLoading);
            this.panelChartContainer.Controls.Add(this.chartClimate);
            this.panelChartContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChartContainer.Location = new System.Drawing.Point(223, 3);
            this.panelChartContainer.Name = "panelChartContainer";
            this.panelChartContainer.Padding = new System.Windows.Forms.Padding(15);
            this.panelChartContainer.Size = new System.Drawing.Size(822, 470);
            this.panelChartContainer.TabIndex = 0;
            // 
            // labelLoading
            // 
            this.labelLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLoading.AutoSize = true;
            this.labelLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoading.Location = new System.Drawing.Point(324, 402);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(151, 32);
            this.labelLoading.TabIndex = 14;
            this.labelLoading.Text = "Loading...";
            this.labelLoading.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.labelLoading.Click += new System.EventHandler(this.labelLoading_Click);
            // 
            // chartClimate
            // 
            this.chartClimate.BackColor = System.Drawing.Color.White;
            this.chartClimate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartClimate.Location = new System.Drawing.Point(15, 15);
            this.chartClimate.Margin = new System.Windows.Forms.Padding(0);
            this.chartClimate.Name = "chartClimate";
            this.chartClimate.Size = new System.Drawing.Size(790, 438);
            this.chartClimate.TabIndex = 13;
            // 
            // chartTitle
            // 
            this.chartTitle.AutoSize = true;
            this.chartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartTitle.Location = new System.Drawing.Point(3, 2);
            this.chartTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chartTitle.Name = "chartTitle";
            this.chartTitle.Size = new System.Drawing.Size(340, 32);
            this.chartTitle.TabIndex = 0;
            this.chartTitle.Text = "📊 Indoor Climate Overview";
            this.chartTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chartTitle.Click += new System.EventHandler(this.label1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabClimate);
            this.tabControl1.Controls.Add(this.tabChat);
            this.tabControl1.Controls.Add(this.tabCron);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1062, 553);
            this.tabControl1.TabIndex = 15;
            // 
            // btnViewAlerts
            // 
            this.btnViewAlerts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewAlerts.BackColor = System.Drawing.Color.White;
            this.btnViewAlerts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnViewAlerts.FlatAppearance.BorderSize = 0;
            this.btnViewAlerts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAlerts.Image = ((System.Drawing.Image)(resources.GetObject("btnViewAlerts.Image")));
            this.btnViewAlerts.Location = new System.Drawing.Point(1014, 0);
            this.btnViewAlerts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnViewAlerts.Name = "btnViewAlerts";
            this.btnViewAlerts.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnViewAlerts.Size = new System.Drawing.Size(44, 47);
            this.btnViewAlerts.TabIndex = 11;
            this.btnViewAlerts.UseVisualStyleBackColor = false;
            this.btnViewAlerts.Click += new System.EventHandler(this.btnViewAlerts_Click);
            // 
            // loadingOverlay1
            // 
            this.loadingOverlay1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.loadingOverlay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadingOverlay1.Location = new System.Drawing.Point(0, 0);
            this.loadingOverlay1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadingOverlay1.Name = "loadingOverlay1";
            this.loadingOverlay1.Size = new System.Drawing.Size(1062, 553);
            this.loadingOverlay1.TabIndex = 14;
            this.loadingOverlay1.Visible = false;
            this.loadingOverlay1.Load += new System.EventHandler(this.loadingOverlay1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 553);
            this.Controls.Add(this.btnViewAlerts);
            this.Controls.Add(this.panelAlertDropdown);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.loadingOverlay1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelAlertDropdown.ResumeLayout(false);
            this.tabCron.ResumeLayout(false);
            this.tabCron.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabChat.ResumeLayout(false);
            this.layoutChatMain.ResumeLayout(false);
            this.panelChatHistory.ResumeLayout(false);
            this.panelChatInput.ResumeLayout(false);
            this.layoutChatInput.ResumeLayout(false);
            this.layoutChatInput.PerformLayout();
            this.tabClimate.ResumeLayout(false);
            this.tabClimate.PerformLayout();
            this.layoutMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.layoutButtons.ResumeLayout(false);
            this.panelChartContainer.ResumeLayout(false);
            this.panelChartContainer.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

     

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Panel panelAlertDropdown;
        private System.Windows.Forms.ListBox lstAlerts;
        private LoadingOverlay loadingOverlay1;
        private System.Windows.Forms.TabPage tabCron;
        private System.Windows.Forms.Button measure;
        private System.Windows.Forms.Button forecast;
        private System.Windows.Forms.TabPage tabChat;
        private System.Windows.Forms.TextBox txtChatInput;
        private System.Windows.Forms.Button btnSendChat;
        private System.Windows.Forms.TabPage tabClimate;
        private System.Windows.Forms.Button btnViewAlerts;
        private LiveCharts.WinForms.CartesianChart chartClimate;
        private System.Windows.Forms.Label chartTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelChatHistory;
        private System.Windows.Forms.Panel panelChatInput;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox cronTextBox;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutButtons;

        //private System.Windows.Forms.Button btnHumidity;
        private RoundButton btnHumidity;

        //private System.Windows.Forms.Button btnGeneralAnalysis;
        private RoundButton btnGeneralAnalysis;

        //private System.Windows.Forms.Button btnForecastAdvice;
        private RoundButton btnForecastAdvice;

        //private System.Windows.Forms.Button bttnweatherToday;
        private RoundButton bttnweatherToday;

        //private System.Windows.Forms.Button btnAirQuality;
        private RoundButton btnAirQuality;

        //private System.Windows.Forms.Button btnTemperature;
        private RoundButton btnTemperature;

        //private System.Windows.Forms.Button bttnweatherTomorrow;
        private RoundButton bttnweatherTomorrow;
        private Panel panelChartContainer;
        private TableLayoutPanel layoutChatMain;
        private TableLayoutPanel layoutChatInput;
        private RichTextBox txtChatHistory;
        private Label labelLoading;
    }
}

