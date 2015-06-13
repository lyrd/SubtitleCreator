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
            this.btnStart = new System.Windows.Forms.Button();
            this.tBInputVideo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.timerStatusChecker = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tSL_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSL_Settings = new System.Windows.Forms.ToolStripDropDownButton();
            this.formatsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standartFiltersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extendedFiltersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSL_Base = new System.Windows.Forms.ToolStripDropDownButton();
            this.inspectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSL_About = new System.Windows.Forms.ToolStripButton();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnTest = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(651, 45);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tBInputVideo
            // 
            this.tBInputVideo.BackColor = System.Drawing.SystemColors.Window;
            this.tBInputVideo.Location = new System.Drawing.Point(6, 19);
            this.tBInputVideo.Name = "tBInputVideo";
            this.tBInputVideo.ReadOnly = true;
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
            this.tSL_Base,
            this.tSL_About});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(737, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tSL_File
            // 
            this.tSL_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.tSL_File.Name = "tSL_File";
            this.tSL_File.Size = new System.Drawing.Size(49, 22);
            this.tSL_File.Text = "Файл";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.openFileToolStripMenuItem.Text = "Открыть файл";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
            // tSL_Base
            // 
            this.tSL_Base.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSL_Base.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inspectToolStripMenuItem,
            this.addToolStripMenuItem});
            this.tSL_Base.Image = ((System.Drawing.Image)(resources.GetObject("tSL_Base.Image")));
            this.tSL_Base.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSL_Base.Name = "tSL_Base";
            this.tSL_Base.Size = new System.Drawing.Size(73, 22);
            this.tSL_Base.Text = "База слов";
            // 
            // inspectToolStripMenuItem
            // 
            this.inspectToolStripMenuItem.Name = "inspectToolStripMenuItem";
            this.inspectToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.inspectToolStripMenuItem.Text = "Просмотреть";
            this.inspectToolStripMenuItem.Click += new System.EventHandler(this.inspectToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.addToolStripMenuItem.Text = "Добавить";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
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
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 84);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(714, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 119);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(737, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(650, 16);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(13, 350);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(713, 56);
            this.chart1.TabIndex = 7;
            this.chart1.Text = "chart1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 141);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subtitle Creator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
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
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton tSL_Base;
        private System.Windows.Forms.ToolStripMenuItem inspectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

