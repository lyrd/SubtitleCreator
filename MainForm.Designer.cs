namespace SubtitleCreator
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnTest = new System.Windows.Forms.Button();
            this.tBInputVideo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.timerStatusChecker = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tSL_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.tSL_Settings = new System.Windows.Forms.ToolStripDropDownButton();
            this.formatsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standartFiltersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extendedFiltersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSL_About = new System.Windows.Forms.ToolStripButton();
            this.testTB = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.testChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.testCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testChart)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(651, 45);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(103, 23);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tBInputVideo
            // 
            this.tBInputVideo.Location = new System.Drawing.Point(6, 19);
            this.tBInputVideo.Name = "tBInputVideo";
            this.tBInputVideo.Size = new System.Drawing.Size(535, 20);
            this.tBInputVideo.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpen);
            this.groupBox1.Controls.Add(this.tBInputVideo);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(633, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Исходный видеофайл";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(547, 17);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "Обзор";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // timerStatusChecker
            // 
            this.timerStatusChecker.Tick += new System.EventHandler(this.timerStatusChecker_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSL_File,
            this.tSL_Settings,
            this.tSL_About});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(846, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tSL_File
            // 
            this.tSL_File.Name = "tSL_File";
            this.tSL_File.Size = new System.Drawing.Size(49, 22);
            this.tSL_File.Text = "Файл";
            // 
            // tSL_Settings
            // 
            this.tSL_Settings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatsMenuItem});
            this.tSL_Settings.Name = "tSL_Settings";
            this.tSL_Settings.Size = new System.Drawing.Size(80, 22);
            this.tSL_Settings.Text = "Настройки";
            // 
            // formatsMenuItem
            // 
            this.formatsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standartFiltersMenuItem,
            this.extendedFiltersMenuItem});
            this.formatsMenuItem.Name = "formatsMenuItem";
            this.formatsMenuItem.Size = new System.Drawing.Size(126, 22);
            this.formatsMenuItem.Text = "Форматы";
            // 
            // standartFiltersMenuItem
            // 
            this.standartFiltersMenuItem.Checked = true;
            this.standartFiltersMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.standartFiltersMenuItem.Name = "standartFiltersMenuItem";
            this.standartFiltersMenuItem.Size = new System.Drawing.Size(153, 22);
            this.standartFiltersMenuItem.Text = "Стандартные";
            this.standartFiltersMenuItem.Click += new System.EventHandler(this.standartFiltersMenuItem_Click);
            // 
            // extendedFiltersMenuItem
            // 
            this.extendedFiltersMenuItem.Name = "extendedFiltersMenuItem";
            this.extendedFiltersMenuItem.Size = new System.Drawing.Size(153, 22);
            this.extendedFiltersMenuItem.Text = "Расширенные";
            this.extendedFiltersMenuItem.Click += new System.EventHandler(this.extendedFiltersMenuItem_Click);
            // 
            // tSL_About
            // 
            this.tSL_About.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSL_About.Image = ((System.Drawing.Image)(resources.GetObject("tSL_About.Image")));
            this.tSL_About.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSL_About.Name = "tSL_About";
            this.tSL_About.Size = new System.Drawing.Size(86, 22);
            this.tSL_About.Text = "О программе";
            // 
            // testTB
            // 
            this.testTB.Location = new System.Drawing.Point(83, 84);
            this.testTB.Multiline = true;
            this.testTB.Name = "testTB";
            this.testTB.Size = new System.Drawing.Size(751, 228);
            this.testTB.TabIndex = 5;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 318);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(822, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // testChart
            // 
            chartArea1.Name = "ChartArea1";
            this.testChart.ChartAreas.Add(chartArea1);
            this.testChart.Location = new System.Drawing.Point(18, 84);
            this.testChart.Name = "testChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Blue;
            series1.Name = "Series1";
            this.testChart.Series.Add(series1);
            this.testChart.Size = new System.Drawing.Size(59, 228);
            this.testChart.TabIndex = 6;
            this.testChart.Text = "chart1";
            // 
            // testCheckBox
            // 
            this.testCheckBox.AutoSize = true;
            this.testCheckBox.Location = new System.Drawing.Point(760, 47);
            this.testCheckBox.Name = "testCheckBox";
            this.testCheckBox.Size = new System.Drawing.Size(58, 17);
            this.testCheckBox.TabIndex = 7;
            this.testCheckBox.Text = "isNorm";
            this.testCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 353);
            this.Controls.Add(this.testCheckBox);
            this.Controls.Add(this.testChart);
            this.Controls.Add(this.testTB);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnTest);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subtitle Creator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox tBInputVideo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Timer timerStatusChecker;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton tSL_File;
        private System.Windows.Forms.ToolStripDropDownButton tSL_Settings;
        private System.Windows.Forms.ToolStripButton tSL_About;
        private System.Windows.Forms.ToolStripMenuItem formatsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem standartFiltersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extendedFiltersMenuItem;
        private System.Windows.Forms.TextBox testTB;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataVisualization.Charting.Chart testChart;
        private System.Windows.Forms.CheckBox testCheckBox;
    }
}

