﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OperationWithFiles;

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
        private string name = "";
        private readonly string format = "wav";

        private string filter1 = "";
        private string filter2 = "";

        private readonly string iniFilePath = Directory.GetCurrentDirectory() + "\\FileFilters.ini";
        private readonly string formText = "Subtitle Creator v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private enum Status { idle, in_work };
        private Status status = Status.idle;

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

        private void TEST_ReadData()
        {
            testChart.Series[0].Points.Clear();

            try
            {
                WavData.ReadWavDataChunk("female1\\2.wav");//female1\\2.wav
            }
            catch (WavException ex)
            {
                MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            TEST_WavVisualization();
        }

        private void TEST_WavVisualization()
        {
            short min = WavData.RawData.Min();
            short max = WavData.RawData.Max();

            bool isNorm = testCheckBox.Checked ? true : false;
            int[] coords_raw = { 0, (int)WavData.SampleNumber, -max, max };
            int[] coords_norm = { 0, (int)WavData.SampleNumber, -1, 1 };

            if (isNorm)
            {
                testChart.ChartAreas[0].AxisX.Minimum = coords_norm[0];
                testChart.ChartAreas[0].AxisX.Maximum = coords_norm[1];

                testChart.ChartAreas[0].AxisY.Minimum = coords_norm[2];
                testChart.ChartAreas[0].AxisY.Maximum = coords_norm[3];

                for (int i = 0; i < WavData.SampleNumber; i++)
                    testChart.Series[0].Points.AddXY(i, WavData.NornalizeData[i]);
            }
            else
            {
                testChart.ChartAreas[0].AxisX.Minimum = coords_raw[0];
                testChart.ChartAreas[0].AxisX.Maximum = coords_raw[1];

                testChart.ChartAreas[0].AxisY.Minimum = coords_raw[2];
                testChart.ChartAreas[0].AxisY.Maximum = coords_raw[3];

                for (int i = 0; i < WavData.SampleNumber; i++)
                    testChart.Series[0].Points.AddXY(i, WavData.RawData[i]);
            }
        }

        private void TEST_WavVisualization(short start, short finish)
        {
            short min = WavData.RawData.Min();
            short max = WavData.RawData.Max();

            bool isNorm = testCheckBox.Checked ? true : false;
            int[] coords_raw = { 0, (int)WavData.SampleNumber, -max, max };
            int[] coords_norm = { 0, (int)WavData.SampleNumber, -1, 1 };

            if (isNorm)
            {
                testChart.ChartAreas[0].AxisX.Minimum = coords_norm[0];
                testChart.ChartAreas[0].AxisX.Maximum = coords_norm[1];

                testChart.ChartAreas[0].AxisY.Minimum = coords_norm[2];
                testChart.ChartAreas[0].AxisY.Maximum = coords_norm[3];

                for (int i = start; i < finish; i++)
                    testChart.Series[0].Points.AddXY(i, WavData.NornalizeData[i]);
            }
            else
            {
                //testChart.ChartAreas[0].AxisX.Minimum = coords_raw[0];
                //testChart.ChartAreas[0].AxisX.Maximum = coords_raw[1];

                //testChart.ChartAreas[0].AxisY.Minimum = coords_raw[2];
                //testChart.ChartAreas[0].AxisY.Maximum = coords_raw[3];
                testChart.ChartAreas[0].AxisX.Minimum = 0;
                testChart.ChartAreas[0].AxisX.Maximum = 450;

                testChart.ChartAreas[0].AxisY.Minimum = -4000;
                testChart.ChartAreas[0].AxisY.Maximum = 4000;

                for (int i = start; i < finish; i++)
                    testChart.Series[0].Points.AddXY(i, WavData.RawData[i]);
            }
        }

        private static void TEST_SaveIntoFile<T>(T[] mas, string name)
        {
            using (StreamWriter str = new StreamWriter(name + ".txt"))
            {
                for (int i = 0; i < mas.Length; i++)
                {
                    str.WriteLine(mas[i]);
                }
            }
        }

        private static void TEST_SaveIntoFile<T>(List<T> mas, string name)
        {
            using (StreamWriter str = new StreamWriter(name + ".txt"))
            {
                for (int i = 0; i < mas.Count; i++)
                {
                    str.WriteLine(mas[i]);
                }
            }
        }
        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            TEST_ReadData();

            uint start = 0;
            uint finish = start + Constants.frameLenght;
            uint shift = (uint)(Constants.frameLenght * Constants.frameOverlap);
            List<Frame> frames = new List<Frame>();

            uint totalAmountOfFullFrames = (uint)Math.Floor((double)WavData.SampleNumber / 128);
            List<double> entTest = new List<double>();

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < totalAmountOfFullFrames; i++)
            {
                if (i < totalAmountOfFullFrames - 1)//1
                {
                    frames.Add(new Frame(i, start, finish));
                    start += shift;
                    finish = start + Constants.frameLenght;
                }
            }

            for (int i = 0; i < totalAmountOfFullFrames - 2; i++)//0
            //Parallel.For(0, totalAmountOfFullFrames, i =>//(int)
            {
                //if (i < totalAmountOfFullFrames - 1)//1 2forP
                //{
                //frames.Add(new Frame(i, start, finish));
                //testTB.Text += String.Format("{0}\t{1}\t{2}\t{3}\r\n", i + 1, frames[i].GetStart, frames[i].GetEnd, (frames[i].GetEnd - frames[i].GetStart));

                frames[i].Init(WavData.RawData, WavData.NornalizeData, frames[i].GetStart, frames[i].GetEnd);
                entTest.Add(frames[i].GetEntropy);
                //frames[i].InitMFCC(WavData.NornalizeData, frames[i].GetStart, frames[i].GetEnd, Constants.sampleRate);

                //start += shift;
                //finish = start + Constants.frameLenght;

                //TEST_SaveIntoFile(frames[i].GetMfcc, "test\\testMFCC_id" + frames[i].GetId + "_lenght_" + (finish - start));
                //}
                //else
                //{
                //    start += shift;
                //    finish = WavData.SampleNumber - 1;

                //    //frames.Add(new Frame(i, start, finish));
                //    //testTB.Text += String.Format("{0}\t{1}\t{2}\t{3}\r\n", i + 1, frames[i].GetStart, frames[i].GetEnd, (frames[i].GetEnd - frames[i].GetStart));

                //    frames[i].Init(WavData.RawData, WavData.NornalizeData, start, finish);
                //    frames[i].InitMFCC(WavData.NornalizeData, start, finish, Constants.sampleRate);

                //    //TEST_SaveIntoFile(frames[i].GetMfcc, "test\\testMFCC_id" + frames[i].GetId + "_lenght_" + (finish - start));
                //}
            }//);

            //TEST_SaveIntoFile(entTest, "test\\entropy.txt");

            //testChart.ChartAreas[0].AxisX.Minimum = 0;
            //for (int i = 0; i < totalAmountOfFullFrames - 2; i++)//1
            //{
            //    //testTB.Text += frames[i].GetEntropy + "\r\n";
            //    //chart1.Series[0].Points.AddXY(i, frames[i].GetEntropy);
            //    testChart.Series[0].Points.AddXY(i, frames[i].GetEntropy);
            //}

            //testChart.Series[1].Points.AddXY(0, Constants.entropyThreshold);
            //testChart.Series[1].Points.AddXY(totalAmountOfFullFrames - 1, Constants.entropyThreshold);
            //MessageBox.Show(sw.ElapsedMilliseconds.ToString());

            List<Frame> combinedFrames = new List<Frame>();

            List<bool> IsSound = new List<bool>();

            for (int i = 0; i < totalAmountOfFullFrames - 2; i++)
            {
                if (frames[i].GetEntropy > Constants.entropyThreshold)
                {
                    IsSound.Add(true);
                }
                else
                {
                    IsSound.Add(false);
                }
            }

            for (int i = 0; i < IsSound.Count; i++)
            {
                testTB.Text += (i + 0).ToString() + "\t" + IsSound[i] + "\t" + frames[i].GetEntropy + "\t" + frames[i].GetStart + "\t" + frames[i].GetEnd + "\r\n";
            }

            uint startIndex = 0;
            uint endIndex = 0;
            bool startIndexIsSet = false;


            for (int i = 0; i < totalAmountOfFullFrames - 3; i++)
            {
                if (!startIndexIsSet)
                {
                    startIndex = frames[i].GetStart;
                    startIndexIsSet = true;
                }

                if (IsSound[i] == IsSound[i + 1])
                {
                    endIndex = frames[i + 1].GetEnd;
                }
                else
                {
                    endIndex = frames[i].GetEnd;
                    combinedFrames.Add(new Frame(i, startIndex, endIndex));
                    startIndexIsSet = false;
                }

                //combinedFrames.Add(new Frame(i, startIndex, endIndex));
                //startIndexIsSet = false;
            }

            //for (uint i = combinedFrames[0].GetStart; i < combinedFrames[combinedFrames.Count].GetEnd; i++)
            //{
            //    chart1.Series[0].Points.AddXY(i, WavData.NornalizeData[i]);
            //}

            for (uint i = combinedFrames[0].GetStart; i < combinedFrames[0].GetEnd; i++)
            {
                chart1.Series[0].Points.AddXY(i, WavData.RawData[i]);
            }

            MessageBox.Show(combinedFrames.Count.ToString());
            MessageBox.Show(combinedFrames[0].GetStart + "\r\n" + combinedFrames[0].GetEnd + "\r\n" + combinedFrames[1].GetStart + "\r\n" + combinedFrames[1].GetEnd);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = String.Format("Video Files {0}|{1}", filter1, filter2);

            tBInputVideo.Text =
                inputVideoFile =
                dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "err";

            name = Transliteration(Path.GetFileNameWithoutExtension(inputVideoFile)).Replace(' ', '_');

            outputAudioFile = String.Format("{0}\\{1}.{2}", Directory.GetCurrentDirectory(), name, format);

            if (inputVideoFile != "err")
            {
                try
                {
                    Process.Start(Directory.GetCurrentDirectory() + "\\ffmpeg.exe", String.Format(" -i \"{0}\" -vn -ar 44100 -ac 2 -ab 192k -f {1} {2}", inputVideoFile, format, outputAudioFile));
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

            #region TEST GRAPH
            testChart.ChartAreas[0].CursorX.IsUserEnabled = true;
            testChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            testChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            testChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chart1.ChartAreas[0].AxisX.Minimum = 0;

            //testChart.ChartAreas[0].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            //chart1.ChartAreas[0].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;

            //chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            //testChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //testChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            #endregion
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
        #endregion
    }
}
