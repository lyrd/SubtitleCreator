﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.IO;
using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
//using System.Collections;
//using NAudio.Wave;
using OperationWithFiles;
using System.Numerics;

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
                WavData.ReadWavDataChunk("female1\\2.wav");
            }
            catch (WavException ex)
            {
                MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //TEST_WavVisualization();
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

        //private object[] ReadWavDataChunk(string _outputAudioFile)//short
        //{
        //    try
        //    {
        //        using (WaveFileReader reader = new WaveFileReader(_outputAudioFile))
        //        {
        //            if (reader.WaveFormat.BitsPerSample == 16)
        //            {
        //                byte[] buffer = new byte[reader.Length];
        //                int read = reader.Read(buffer, 0, buffer.Length);
        //                short[] sampleBuffer = new short[read / 2];
        //                Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, read);

        //                uint _sampleNumber = (uint)sampleBuffer.Length;

        //                object[] ret = { _sampleNumber, sampleBuffer };
        //                return ret;
        //            }
        //            else
        //            {
        //                object[] temp = { null, null };
        //                MessageBox.Show("Only works with 16 bit audio");
        //                return temp;
        //            }
        //        }
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        MessageBox.Show(String.Format("Звуковая дорожка не была извлечена\n{0}", ex.Message),
        //                        "Ошибка",
        //                        MessageBoxButtons.OK,
        //                        MessageBoxIcon.Error);

        //        object[] temp = { null, null };
        //        return temp;
        //    }
        //}

        //private static double[] Normalization(short[] _rawData)//float
        //{
        //    uint _sampleNumber = (uint)_rawData.Length;
        //    double[] _nornalizeData = new double[_sampleNumber];

        //    double minData = (double)Math.Abs((double)_rawData.Min());
        //    double maxData = (double)Math.Abs((double)_rawData.Max());
        //    double max = Math.Max(minData, maxData);

        //    for (uint i = 0; i < _sampleNumber; i++)
        //    {
        //        _nornalizeData[i] = _rawData[i] / max;
        //    }

        //    return _nornalizeData;
        //}

        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            //forTests
            //Complex comp = new Complex(1, 5);
            //MessageBox.Show(String.Format(new ComplexFormatter(), "Imaginary = {0}\nMagnitude = {1}\nPhase = {2}\nReal = {3}\nSquaredMagnitude = {4}",
            //    comp.Imaginary, comp.Magnitude, comp.Phase, comp.Real, Math.Pow(comp.Magnitude, 2)));

            ////comp = Complex.FromPolarCoordinates(comp.Magnitude, comp.Phase);
            //Complex comp2 = Complex.FromPolarCoordinates(comp.Magnitude, comp.Phase);
            //MessageBox.Show(String.Format("Polar = {0}", comp2));
            ////decimal a = 25.0m / 46.0m;
            ////decimal b = (1.0m - a);// / 2.0m;
            ////MessageBox.Show(String.Format("{0}\n{1}", a, b));
            ////MessageBox.Show(String.Format("{0}\n{1}", Constants.alpha, Constants.beta));

            //Complex[] test = MFCC.Test();
            //MessageBox.Show(test.Count().ToString());

            //MessageBox.Show(Constants.wordMinSize.ToString());

            TEST_ReadData();

            testTB.Text += "Sample Number: " + WavData.SampleNumber + "\r\n";
            testTB.Text += "Frame Lenght: " + Constants.frameLenght + "\r\n";
            testTB.Text += "Frame Overlap: " + Constants.frameLenght * Constants.frameOverlap + "\r\n";

            uint start = 0;
            uint finish = start + Constants.frameLenght;
            uint shift = (uint)(Constants.frameLenght * Constants.frameOverlap); 
            List<Frame> frames = new List<Frame>();

            testTB.Text += "Shift: " + shift + "\r\n";
            testTB.Text += String.Format("{0}\t{1}\r\n", start, finish);

            for (int i = 0; i < 10; i++ )
            {
                frames.Add(new Frame(i));

                frames[i].Init(WavData.RawData, WavData.NornalizeData, start, finish);
                frames[i].InitMFCC(WavData.NornalizeData, start, finish, Constants.sampleRate);
                TEST_SaveIntoFile(frames[i].GetMfcc, "test\\testMFCC_id" + frames[i].GetId);

                start += shift;
                finish = start + Constants.frameLenght;

                testTB.Text += String.Format("{0}\t{1}\r\n", start, finish);
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
                //object[] tempObj = new object[2];

                //WavData.ReadWavDataChunk("female1.wav").CopyTo(tempObj, 0);

                //try
                //{
                //    WavData.SampleNumber = (uint)tempObj[0];
                //    WavData.RawData = new short[WavData.SampleNumber];
                //    WavData.RawData = (short[])tempObj[1];

                //    WavData.NornalizeData = new double[WavData.SampleNumber];
                //    WavData.Normalization(WavData.RawData).CopyTo(WavData.NornalizeData, 0);
                //}
                //catch (WavException ex) { MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }

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

                #region TEST GRAPH
                //short min = rawData.Min();
                //short max = rawData.Max();
                ////int[] coords_raw = { 0, (int)sampleNumber, short.MinValue, short.MaxValue };
                ////int[] coords_raw = { 0, (int)sampleNumber, min, max };
                //int[] coords_raw = { 0, (int)sampleNumber, -max, max };
                //int[] coords_norm = { 0, (int)sampleNumber, -1, 1 };

                //chart1.ChartAreas[0].AxisX.Minimum = coords_norm[0];
                //chart1.ChartAreas[0].AxisX.Maximum = coords_norm[1];

                //chart1.ChartAreas[0].AxisY.Minimum = coords_norm[2];
                //chart1.ChartAreas[0].AxisY.Maximum = coords_norm[3];

                //for (int i = 0; i < sampleNumber; i++)
                //    chart1.Series[0].Points.AddXY(i, nornalizeData[i]);

                //chart1.SaveImage("rawData.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                #endregion
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
