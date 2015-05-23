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
        private string srtFile = "";
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

        private double GetDuration(uint size)
        {
            return size / (44100 * 2 * (16 / 2));
        }

        private void TEST_ReadData()
        {
            testChart.Series[0].Points.Clear();

            try
            {
                WavData.ReadWavDataChunk("My_Edited_Video.wav");//female1\\2.wav female1\\3.wav samples\\
            }
            catch (WavException ex)
            {
                MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //TEST_WavVisualization();
        }

        private void TEST_GetMfcc(string file)
        {
            try
            {
                WavData.ReadWavDataChunk( "samples\\" + file + ".wav");//female1\\2.wav female1\\3.wav samples\\"My_Edited_Video.wav"
            }
            catch (WavException ex)
            {
                MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            //char[] ind = { '1', '2', '3', '4', '5' };
            //for (int i = 0; i < 5; i++)
            //{
            //    TEST_GetMfcc(ind[i].ToString());
            //    Frame frameTEST = new Frame(0, 0, WavData.SampleNumber);
            //    frameTEST.InitMFCC(WavData.NornalizeData, frameTEST.GetStart, frameTEST.GetEnd, Constants.sampleRate);
            //    TEST_SaveIntoFile(frameTEST.GetMfcc, "samples\\" + ind[i].ToString());
            //}

            TEST_ReadData();
            //Frame frameTEST = new Frame(0, 0, WavData.SampleNumber);
            //frameTEST.InitMFCC(WavData.NornalizeData, frameTEST.GetStart, frameTEST.GetEnd, Constants.sampleRate);
            //TEST_SaveIntoFile(frameTEST.GetMfcc, "samples\\Five");

            AudioProcessor.GetFrames();

            //for (int i = 0; i < AudioProcessor.Frames.Count; i++)
            //{
            //    AudioProcessor.Frames[i].InitMFCC(WavData.NornalizeData, AudioProcessor.Frames[i].GetStart, AudioProcessor.Frames[i].GetEnd, Constants.sampleRate);
            //    //TEST_SaveIntoFile(AudioProcessor.Frames[i].GetMfcc, "test\\testMFCC_id" + AudioProcessor.Frames[i].GetId + "_lenght_" + (AudioProcessor.Frames[i].GetEnd - AudioProcessor.Frames[i].GetStart));
            //}

            double[] rawdata = WavData.NornalizeData;

            Parallel.For(0, AudioProcessor.Frames.Count, (i) =>
            {
                if(AudioProcessor.Frames[i].IsSound)
                AudioProcessor.Frames[i].InitMFCC(ref rawdata, AudioProcessor.Frames[i].GetStart, AudioProcessor.Frames[i].GetEnd, Constants.sampleRate);
            });

            //MessageBox.Show(AudioProcessor.Frames[5].GetMfcc[0].ToString());

            //chart1.ChartAreas[0].AxisX.Minimum = 0;
            //chart1.ChartAreas[0].AxisX.Maximum = WavData.SampleNumber;
            //chart1.ChartAreas[0].AxisY.Minimum = WavData.RawData.Min();
            //chart1.ChartAreas[0].AxisY.Maximum = WavData.RawData.Max();
            ////int index = 19;
            ////for (uint i = AudioProcessor.Frames[index].GetStart; i < AudioProcessor.Frames[index].GetEnd; i++)
            ////    chart1.Series[0].Points.AddXY(i, WavData.RawData[i]);
            //for (int j = 0; j < AudioProcessor.Frames.Count; j++)
            //    for (uint i = AudioProcessor.Frames[j].GetStart; i < AudioProcessor.Frames[j].GetEnd; i++)
            //    {
            //        //if (AudioProcessor.Frames[index].IsSound)
            //        if (j == 1 | j == 5 | j == 9 | j == 17 | j == 19)
            //            chart1.Series[0].Points.AddXY(i, WavData.RawData[i]);
            //    }



            //=================================================================================================
            Dictionary<string, double[]> samplesMFCC = new Dictionary<string, double[]>();
            #region One-Five
//            samplesMFCC.Add("Один", new double[] {102.87924439628,
//18.9967437845749,
//-8.31539443641299,
//9.10741440509637,
//1.54102943596866,
//-6.35494285554927,
//2.02850339623282,
//-2.88594957956848,
//1.20184174702284,
//-0.661949628706233,
//2.634916325507,
//-1.21239577519056
//});

//            samplesMFCC.Add("Два", new double[] { 108.458383278458,
//23.7530682467297,
//-4.36526446729434,
//7.77997941631068,
//-1.15281695013042,
//-6.32192546644515,
//3.41412201405264,
//1.28586342509823,
//-0.531651633753943,
//-2.77953272288212,
//1.30463524394773,
//0.54036308097494
//            });

//            samplesMFCC.Add("Три", new double[] { 114.642651165638,
//8.89561806588891,
//-7.17258179111635,
//18.0156652689698,
//3.54273728138824,
//-1.31035596162442,
//3.3406453011858,
//-3.26017049701631,
//1.34014915986403,
//-0.55316300244534,
//2.29425678553576,
//-0.00354136452707066
//            });

//            samplesMFCC.Add("Четыре", new double[] { 101.328218872533,
//3.57330129527,
//-1.34401197393287,
//18.5283481555506,
//6.67967967007905,
//-1.54765088488265,
//2.86462275705537,
//-2.13952580088545,
//1.78939739204334,
//-0.990360024591741,
//0.672322455414861,
//-0.897978354276461
//            });

//            samplesMFCC.Add("Пять", new double[] { 122.549728900115,
//17.3960698308855,
//-8.89595538706509,
//10.8358464257385,
//0.281242118172386,
//-5.46953472971976,
//-1.60731240021606,
//-5.71761509737905,
//2.04363455137455,
//-1.07465236935577,
//2.82535966623671,
//-1.57109204284586
//            });
            #endregion

#region 1-5
            samplesMFCC.Add("Один", new double[] {115.902755904026,
26.4175781482317,
-19.4969240820016,
15.9409905913534,
-10.4284405539948,
2.96120634212882,
0.839603731203302,
-3.54910751739161,
7.03148074595154,
-3.38572657631652,
4.31563689768913,
-1.83372751201521
});

            samplesMFCC.Add("Два", new double[] { 87.4680793373385,
38.04600031747,
-13.1417323331799,
20.2925001019083,
-5.30224524545408,
7.91420814520266,
1.30036853260372,
-5.17687413666072,
4.37913810603501,
-6.49839167365708,
1.62132894134803,
-3.20190955072429
            });

            samplesMFCC.Add("Три", new double[] { 90.4941289080472,
26.0453417615593,
-18.6743363735187,
1.74459319558932,
-8.92167293158036,
2.24214961688164,
1.87286420970423,
-3.12817588006035,
4.87943552319365,
0.372040941839539,
1.6383080532519,
-1.84132489151332
            });

            samplesMFCC.Add("Четыре", new double[] { 105.925500830823,
23.8592925836023,
-22.2180238851642,
9.02482389959434,
-9.97740789791419,
1.1395326330837,
2.70388567208278,
-1.595824974041,
6.23204003075054,
-1.41871022985522,
2.82935206354146,
-1.50896863327325
            });

            samplesMFCC.Add("Пять", new double[] { 104.783221920618,
41.9621287055811,
-13.6065046872223,
11.7495013975808,
-10.9850334035548,
3.68997271801716,
2.65331674949439,
-3.00099818531845,
4.97058029287006,
-1.78037702637737,
3.76850879675914,
-1.85753593805106
            });
#endregion

            List<double> temp = new List<double>();

            for (int j = 0; j < AudioProcessor.Frames.Count; j++)
            {
                if (AudioProcessor.Frames[j].IsSound)
                {
                    for (int i = 0; i < samplesMFCC.Count; i++)
                    {
                        temp.Add(DTW.CalcDistance(AudioProcessor.Frames[j].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(i).Value, Constants.mfccSize));
                    }
                    temp.Min();
                    temp.IndexOf(temp.Min());
                    AudioProcessor.Frames[j].Caption = samplesMFCC.ElementAt(temp.IndexOf(temp.Min())).Key;
                    testTB.Text += samplesMFCC.ElementAt(temp.IndexOf(temp.Min())).Key + "\r\n";
                    temp.Clear();
                }
            }

            string str = "";
            for (int j = 0; j < AudioProcessor.Frames.Count; j++)
            {
                str += (j + 1).ToString() + "\t" + AudioProcessor.Frames[j].Caption + "\t" + AudioProcessor.Frames[j].GetStart + "\t" + AudioProcessor.Frames[j].GetEnd + "\r\n";
                //str += (j + 1).ToString() + "\t" + AudioProcessor.Frames[j].Caption + "\t" + GetDuration(AudioProcessor.Frames[j].GetStart) + "\t" + GetDuration(AudioProcessor.Frames[j].GetEnd) + "\r\n";
            }
            MessageBox.Show(str);

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = String.Format("Video Files {0}|{1}", filter1, filter2);

            tBInputVideo.Text =
                inputVideoFile =
                dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "err";

            name = Transliteration(Path.GetFileNameWithoutExtension(inputVideoFile)).Replace(' ', '_');

            //outputAudioFile = String.Format("{0}\\{1}.{2}", Directory.GetCurrentDirectory(), name, format);
            outputAudioFile = String.Format("{0}.{1}", name, format);

            srtFile = Path.GetFileNameWithoutExtension(inputVideoFile) + ".srt";

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

            //if (!this.Focused && status == Status.idle)
            //{
            //    try
            //    {
            //        //WavData.ReadWavDataChunk("My_Edited_Video.wav");//female1\\2.wav female1\\3.wav samples\\
            //        WavData.ReadWavDataChunk(outputAudioFile);//female1\\2.wav female1\\3.wav samples\\
            //    }
            //    catch (WavException ex)
            //    {
            //        MessageBox.Show(String.Format(ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            //    //TEST_WavVisualization();

            //    //AudioProcessor.GetFrames();

            //    this.Activate();
            //    timerStatusChecker.Stop();
            //}
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
