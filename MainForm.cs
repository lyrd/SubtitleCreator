using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OperationWithFiles;
using System.Drawing;

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
                WavData.ReadWavDataChunk("samples\\Russian Numbers 2.wav");//female1\\2.wav female1\\3.wav
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

        private void TEST_WavVisualization(int start, int finish)
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
                chart1.ChartAreas[0].AxisX.Minimum = coords_raw[0];
                chart1.ChartAreas[0].AxisX.Maximum = coords_raw[1];

                chart1.ChartAreas[0].AxisY.Minimum = coords_raw[2];
                chart1.ChartAreas[0].AxisY.Maximum = coords_raw[3];
                //testChart.ChartAreas[0].AxisX.Minimum = 0;
                //testChart.ChartAreas[0].AxisX.Maximum = 450;

                //testChart.ChartAreas[0].AxisY.Minimum = -4000;
                //testChart.ChartAreas[0].AxisY.Maximum = 4000;

                for (int i = start; i < finish; i++)
                    chart1.Series[0].Points.AddXY(i, WavData.RawData[i]);
            }
        }

        private static void TEST_SaveIntoFile<T>(T[] mas, string name)
        {
            using (StreamWriter str = new StreamWriter(name + ".txt"))
            {
                for (int i = 0; i < mas.Length; i++)
                {
                    //str.WriteLine(mas[i]);
                    str.WriteLine(Convert.ToString(mas[i]).Replace(",", "."));
                    //str.WriteLine(mas[i].ToString(System.Globalization.CultureInfo.InvariantCulture));
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
            //TEST_ReadData();

            //AudioProcessor.GetFrames();

            //for (int i = 0; i < AudioProcessor.Frames.Count; i++)
            //{
            //    AudioProcessor.Frames[i].InitMFCC(WavData.NornalizeData, AudioProcessor.Frames[i].GetStart, AudioProcessor.Frames[i].GetEnd, Constants.sampleRate);
            //    TEST_SaveIntoFile(AudioProcessor.Frames[i].GetMfcc, "test\\testMFCC_id" + AudioProcessor.Frames[i].GetId + "_lenght_" + (AudioProcessor.Frames[i].GetEnd - AudioProcessor.Frames[i].GetStart));
            //}

            //MessageBox.Show(AudioProcessor.Frames[5].GetMfcc[0].ToString());

            //chart1.ChartAreas[0].AxisX.Minimum = 0;
            //chart1.ChartAreas[0].AxisX.Maximum = WavData.SampleNumber;
            //chart1.ChartAreas[0].AxisY.Minimum = WavData.RawData.Min();
            //chart1.ChartAreas[0].AxisY.Maximum = WavData.RawData.Max();
            //int index = 6;
            //for (uint i = AudioProcessor.Frames[index].GetStart; i < AudioProcessor.Frames[index].GetEnd; i++)
            //    chart1.Series[0].Points.AddXY(i, WavData.RawData[i]);

            //Frame frameTEST = new Frame(0, 0, WavData.SampleNumber);
            //frameTEST.InitMFCC(WavData.NornalizeData, frameTEST.GetStart, frameTEST.GetEnd, Constants.sampleRate);
            //TEST_SaveIntoFile(frameTEST.GetMfcc, "samples\\Russian Numbers 2");

            //=================================================================================================
            double[] one = {102.87924439628,
18.9967437845749,
-8.31539443641299,
9.10741440509637,
1.54102943596866,
-6.35494285554927,
2.02850339623282,
-2.88594957956848,
1.20184174702284,
-0.661949628706233,
2.634916325507,
-1.21239577519056 };
            double[] two = { 84.5647217936344,
                            20.5062816907434,
                            -21.9285021043137,
                            -1.1673774733367,
                            2.29537113106229,
                            2.8849510546711,
                            7.45033987776026,
                            -2.4361706591776,
                            0.621853395872672,
                            3.14637229002995,
                            -2.79791133596741,
                            -0.313669848089029
                            };


            List<Frame> frame = new List<Frame>();

            Dictionary<string, double[]> samplesMFCC = new Dictionary<string, double[]>();

            samplesMFCC.Add("Один", new double[] {122.57713240288,
25.9806576797282,
-5.46809455639049,
10.582132869263,
-3.95365439477263,
4.02857386802241,
-1.3008323586463,
4.09646424702198,
-1.60406006996773,
1.78026356913057,
-0.290479473280312,
0.481294730906596
});

            samplesMFCC.Add("Два", new double[] { 102.138915198788,
40.800779822216,
2.50521835307931,
11.0664661924822,
-1.89507711945657,
4.81727708217499,
-1.47981366490812,
2.22123916592631,
-2.8288951393954,
-0.256760490955532,
-2.33408327080272,
-0.320943143709753


                            });

            frame.Add(new Frame(0, 0, 2));
            frame.Add(new Frame(0, 0, 2));

            frame[0].GetMfcc = one;
            frame[1].GetMfcc = two;

            //testTB.Text += samplesMFCC.ElementAt(0).Key; 
            //testTB.Text += samplesMFCC.ElementAt(0).Value[0];

            //for (int i = 0; i < frame.Count; i++)
            //    for (int j = 0; j < samples.Count; j++)
            //{
            //    testTB.Text += DTW.CalcDistance(samples[j].GetMfcc, Constants.mfccSize, frame[i].GetMfcc, Constants.mfccSize) + "\r\n";
            //}

            List<double> temp = new List<double>();

            for (int j = 0; j < frame.Count; j++)
            {
                for (int i = 0; i < samplesMFCC.Count; i++)
                {
                    //testTB.Text += DTW.CalcDistance(frame[j].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(i).Value, Constants.mfccSize) + "\r\n";
                    temp.Add(DTW.CalcDistance(frame[j].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(i).Value, Constants.mfccSize));
                }
                temp.Min();
                temp.IndexOf(temp.Min());
                testTB.Text += samplesMFCC.ElementAt(temp.IndexOf(temp.Min())).Key + "\r\n";
                temp.Clear();
            }

            //testTB.Text += Environment.NewLine;
            //testTB.Text += DTW.CalcDistance(frame[0].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(0).Value, Constants.mfccSize) + "\r\n";
            //testTB.Text += DTW.CalcDistance(frame[0].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(1).Value, Constants.mfccSize) + "\r\n";
            //testTB.Text += DTW.CalcDistance(frame[1].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(0).Value, Constants.mfccSize) + "\r\n";
            //testTB.Text += DTW.CalcDistance(frame[1].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(1).Value, Constants.mfccSize) + "\r\n";
            //=================================================================================================

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
