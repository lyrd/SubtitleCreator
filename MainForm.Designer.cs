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
            this.btnTest = new System.Windows.Forms.Button();
            this.tBInputVideo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timerStatusChecker = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tSL_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.tSL_Settings = new System.Windows.Forms.ToolStripDropDownButton();
            this.форматыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.стандартныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.расширенныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSL_About = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
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
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 318);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(822, 23);
            this.progressBar1.TabIndex = 3;
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
            this.форматыToolStripMenuItem});
            this.tSL_Settings.Name = "tSL_Settings";
            this.tSL_Settings.Size = new System.Drawing.Size(80, 22);
            this.tSL_Settings.Text = "Настройки";
            // 
            // форматыToolStripMenuItem
            // 
            this.форматыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.стандартныеToolStripMenuItem,
            this.расширенныеToolStripMenuItem});
            this.форматыToolStripMenuItem.Name = "форматыToolStripMenuItem";
            this.форматыToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.форматыToolStripMenuItem.Text = "Форматы";
            // 
            // стандартныеToolStripMenuItem
            // 
            this.стандартныеToolStripMenuItem.Checked = true;
            this.стандартныеToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.стандартныеToolStripMenuItem.Name = "стандартныеToolStripMenuItem";
            this.стандартныеToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.стандартныеToolStripMenuItem.Text = "Стандартные";
            this.стандартныеToolStripMenuItem.Click += new System.EventHandler(this.стандартныеToolStripMenuItem_Click);
            // 
            // расширенныеToolStripMenuItem
            // 
            this.расширенныеToolStripMenuItem.Name = "расширенныеToolStripMenuItem";
            this.расширенныеToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.расширенныеToolStripMenuItem.Text = "Расширенные";
            this.расширенныеToolStripMenuItem.Click += new System.EventHandler(this.расширенныеToolStripMenuItem_Click);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 353);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox tBInputVideo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timerStatusChecker;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton tSL_File;
        private System.Windows.Forms.ToolStripDropDownButton tSL_Settings;
        private System.Windows.Forms.ToolStripButton tSL_About;
        private System.Windows.Forms.ToolStripMenuItem форматыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem стандартныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem расширенныеToolStripMenuItem;
    }
}

