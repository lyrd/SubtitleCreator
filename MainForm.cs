﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OperationWithFiles;
using System.Drawing;
using System.Threading;

namespace SubtitleCreator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();           
        }

        /*
         * TODO:
         * block form while cmd                      †  
         * фильтр                                    †   
         *progressBar1.Width = this.Width - 40;
         *progressBar1.Top = this.Height - 73;
         *возвращение фокуса                         †
         *кол-во фильтов (stndrt/ extended)          †
         *toolStrip                                  †
         *нормировка
         *фильтры сохр. в файле                      †
         *exceptions
         *switch
         *class
         */

        /*
         * ffmpeg keys
         * ffmpeg -i source_video.avi -vn -ar 44100 -ac 2 -ab 192k -f mp3 sound.mp3
         * --------------------------------------------------------------------------------------------------------------------------------
         * Ключ 	      |  Пример      |	Описание 
         * --------------------------------------------------------------------------------------------------------------------------------
         * -i <filename>  |	-i movie.avi |	Путь/имя входного файла. Без установки параметров обработки означает информацию о входном файле 
         * -vn 	          | -vn 	     |  Не кодировать видео 
         * -ar <freq> 	  | -ar 48000    |  Частота дискредитации. По умочанию 44100Hz 
         * -ac <channels> | -aс 6        |  Количество каналов. По умочанию 2
         * -ab <bitrate>  | -ab 256k 	 |  Битрейт аудио. По умочанию 64kbit/s 
         * -f <format> 	  | -f mkv       |	Формат входного/выходного файла 
         */

        private string inputVideoFile = "";
        private string outputAudioFile = "";
        private string srtFile = "";
        private string name = "";
        private readonly string format = "wav";

        private string filter1 = "";
        private string filter2 = "";

        private readonly string iniFilePath = Directory.GetCurrentDirectory() + "\\FileFilters.ini";
        private readonly string formText = "Subtitle Creator v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private enum Status { idle, in_work };
        private Status status = Status.idle;

        Stopwatch stopwatch = new Stopwatch();

        delegate void SetCallBackrogressBarSpeed(int speed);

        public void setProgressBarSpeed(int speed)
        {
            if(this.progressBar1.InvokeRequired)
            {
                SetCallBackrogressBarSpeed d = new SetCallBackrogressBarSpeed(setProgressBarSpeed);
                this.Invoke(d, speed);
            }
            else
            {
                this.progressBar1.MarqueeAnimationSpeed = 1;
            }
        }

        #region Methods

        private string Transliteration(string source)
        {
            Dictionary<string, string> letters = new Dictionary<string, string>();

            string[,] setOfLetters ={ { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я", "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" },
                                    { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "j", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "h", "c", "ch", "sh", "sch", "j", "i", "j", "e", "yu", "ya", "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "H", "C", "Ch", "Sh", "Sch", "J", "I", "J", "E", "Yu", "Ya" }};

            for (byte i = 0; i < setOfLetters.GetLength(1); i++)
                letters.Add(setOfLetters[0, i], setOfLetters[1, i]);

            foreach (KeyValuePair<string, string> pair in letters)
            {
                source = source.Replace(pair.Key, pair.Value);
            }

            return source;
        }

        private void SetFilters(string type)
        {
            //type
            //"StandardFilters"
            //"ExtendedFilters"
            using (IniFile ini = new IniFile(iniFilePath))
            {
                filter1 = ini.IniReadValue(type, "filter1");
                filter2 = ini.IniReadValue(type, "filter2");
            }
        }

        private void TEST_GetMfcc(string file)
        {
            try
            {
                WavData.ReadWavDataChunk("samples\\" + file + ".wav");//female1\\2.wav female1\\3.wav samples\\"My_Edited_Video.wav"
            }
            catch (WavException ex)
            {
                MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (inputVideoFile != "err" & tBInputVideo.Text != "")
            {
                try
                {
                    Process.Start(Directory.GetCurrentDirectory() + "\\ffmpeg.exe", String.Format(" -i \"{0}\" -vn -ar 44100 -ac 2 -ab 192k -f {1} {2}", inputVideoFile, format, outputAudioFile));
                    stopwatch.Start();
                    timerStatusChecker.Start();
                }
                catch (Win32Exception ex)
                {
                    MessageBox.Show(String.Format("ffmpeg.exe\n{0}", ex.Message),
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    timerStatusChecker.Stop();
                }
            }
            else
            {
                MessageBox.Show("Выберите файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = String.Format("Video Files {0}|{1}", filter1, filter2);

            tBInputVideo.Text =
                inputVideoFile =
                dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "err";

            name = Transliteration(Path.GetFileNameWithoutExtension(inputVideoFile)).Replace(' ', '_');

            outputAudioFile = String.Format("{0}.{1}", name, format);

            srtFile = Path.GetFileNameWithoutExtension(inputVideoFile);
        }

        #region "Event handlers"
        private void timerStatusChecker_Tick(object sender, EventArgs e)
        {
            status = Process.GetProcessesByName("ffmpeg").Any() ? Status.in_work : Status.idle;

            this.Text = status == Status.in_work ? formText + " - IN WORK" : formText;
            this.Enabled = status == Status.in_work ? false : true;

            if (!this.Focused && status == Status.idle)
            {
                try
                {
                    WavData.ReadWavDataChunk(outputAudioFile);
                }
                catch (WavException ex)
                {
                    MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                backgroundWorker1.RunWorkerAsync();

                this.Activate();
                timerStatusChecker.Stop();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            File.SetAttributes(iniFilePath, FileAttributes.ReadOnly);

            this.Text = formText;

            if (standartFiltersMenuItem.Checked) { SetFilters("StandardFilters"); }

            if (extendedFiltersMenuItem.Checked) { SetFilters("ExtendedFilters"); }

            progressBar1.MarqueeAnimationSpeed = 0;
        }

        private void standartFiltersMenuItem_Click(object sender, EventArgs e)
        {
            standartFiltersMenuItem.Checked = true;
            extendedFiltersMenuItem.Checked = false;

            SetFilters("StandardFilters");
        }

        private void extendedFiltersMenuItem_Click(object sender, EventArgs e)
        {
            standartFiltersMenuItem.Checked = false;
            extendedFiltersMenuItem.Checked = true;

            SetFilters("ExtendedFilters");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            progressBar1.Style = ProgressBarStyle.Marquee;
            setProgressBarSpeed(1);
            AudioProcessor.Recognition(this.outputAudioFile, this.srtFile);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            TimeSpan interval = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            toolStripStatusLabel1.Text = String.Format("Затрачено времени: {0}:{1}:{2}.{3}", interval.Hours, interval.Minutes, interval.Seconds, interval.Milliseconds);
            //toolStripStatusLabel1.Text = "Затрачено времени: " + sw.ElapsedMilliseconds;
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 100;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void inspectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + "\\MfccBaseManager.exe");
        }
        #endregion
    }
}
