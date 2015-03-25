using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using NAudio.Wave;
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
        private string format = "wav";

        private string filter1 = "";
        private string filter2 = "";

        private string iniFilePath = Directory.GetCurrentDirectory() + "\\FileFilters.ini";
        private string formText = "Subtitle Creator v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private enum Status { idle, in_work };
        private Status status = Status.idle;

        //Struct?
        private const byte frameLenght = 50;
        private const float frameOverlap = 0.5F;
        private const byte wordMinSize = (byte)((200 / frameLenght) / (1 - frameOverlap));
        private const byte wordMinDistance = (byte)(wordMinSize * 0.5F);

        private float[] nornalizeData;
        private short[] rawData;

        #region Methods
        private string Transliteration(string source)
        {
            #region Letters
            Dictionary<string, string> letters = new Dictionary<string, string>();
            letters.Add("а", "a");
            letters.Add("б", "b");
            letters.Add("в", "v");
            letters.Add("г", "g");
            letters.Add("д", "d");
            letters.Add("е", "e");
            letters.Add("ё", "yo");
            letters.Add("ж", "zh");
            letters.Add("з", "z");
            letters.Add("и", "i");
            letters.Add("й", "j");
            letters.Add("к", "k");
            letters.Add("л", "l");
            letters.Add("м", "m");
            letters.Add("н", "n");
            letters.Add("о", "o");
            letters.Add("п", "p");
            letters.Add("р", "r");
            letters.Add("с", "s");
            letters.Add("т", "t");
            letters.Add("у", "u");
            letters.Add("ф", "f");
            letters.Add("х", "h");
            letters.Add("ц", "c");
            letters.Add("ч", "ch");
            letters.Add("ш", "sh");
            letters.Add("щ", "sch");
            letters.Add("ъ", "j");
            letters.Add("ы", "i");
            letters.Add("ь", "j");
            letters.Add("э", "e");
            letters.Add("ю", "yu");
            letters.Add("я", "ya");
            letters.Add("А", "A");
            letters.Add("Б", "B");
            letters.Add("В", "V");
            letters.Add("Г", "G");
            letters.Add("Д", "D");
            letters.Add("Е", "E");
            letters.Add("Ё", "Yo");
            letters.Add("Ж", "Zh");
            letters.Add("З", "Z");
            letters.Add("И", "I");
            letters.Add("Й", "J");
            letters.Add("К", "K");
            letters.Add("Л", "L");
            letters.Add("М", "M");
            letters.Add("Н", "N");
            letters.Add("О", "O");
            letters.Add("П", "P");
            letters.Add("Р", "R");
            letters.Add("С", "S");
            letters.Add("Т", "T");
            letters.Add("У", "U");
            letters.Add("Ф", "F");
            letters.Add("Х", "H");
            letters.Add("Ц", "C");
            letters.Add("Ч", "Ch");
            letters.Add("Ш", "Sh");
            letters.Add("Щ", "Sch");
            letters.Add("Ъ", "J");
            letters.Add("Ы", "I");
            letters.Add("Ь", "J");
            letters.Add("Э", "E");
            letters.Add("Ю", "Yu");
            letters.Add("Я", "Ya");
            #endregion

            foreach (KeyValuePair<string, string> pair in letters)
            {
                source = source.Replace(pair.Key, pair.Value);
            }

            return source;
        }

        private string FormatListBuilder(ArrayList _formats)
        {
            string output = "(";

            for (int i = 0; i < _formats.Count; i++)
                output += String.Format("*{0}, ", _formats[i]);

            output = output.Substring(0, output.Length - 2);
            output += ")";

            return output;
        }

        //type
        //"StandardFilters"
        //"ExtendedFilters"
        private void SetFilters(string type)
        {
            using (IniFile ini = new IniFile(iniFilePath))
            {
                filter1 = ini.IniReadValue(type, "filter1");
                filter2 = ini.IniReadValue(type, "filter2");
            }
        }

        private void ReadWavDataChunk()
        {
            using (WaveFileReader reader = new WaveFileReader(outputAudioFile))
            {
                if (reader.WaveFormat.BitsPerSample == 16)
                {
                    byte[] buffer = new byte[reader.Length];
                    int read = reader.Read(buffer, 0, buffer.Length);
                    short[] sampleBuffer = new short[read / 2];
                    Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, read);

                    buffer = null;
                    //sampleBuffer = null;

                    //test TODO return
                    rawData = sampleBuffer;
                    sampleBuffer = null;
                    //test

                    GC.Collect();
                }
                else
                {
                    MessageBox.Show("Only works with 16 bit audio");
                }
            }
        }
        #endregion

        #region Methods for tests
        private static void SaveIntoFile(short[] mas, string name)
        {
            StreamWriter str = new StreamWriter(name + ".txt");
            for (int i = 0; i < mas.Length; i++)
            {
                str.WriteLine(mas[i]);
            }
            str.Close();
        }

        private static void SaveIntoFile(string variable, string name)
        {
            StreamWriter sw = new StreamWriter(name + ".txt");
            sw.Write(variable);
            sw.Close();
        }
        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            //timerStatusChecker.Start();
            //Process.Start(Directory.GetCurrentDirectory() + "\\ffmpeg.exe", String.Format(" -i \"{0}\" -vn -ar 44100 -ac 2 -ab 192k -f {1} {2}", inputVideoFile, format, outputAudioFile));

            //using (WaveFileReader reader = new WaveFileReader(outputAudioFile))
            //{
            //    if (reader.WaveFormat.BitsPerSample == 16)
            //    {
            //        byte[] buffer = new byte[reader.Length];
            //        int read = reader.Read(buffer, 0, buffer.Length);
            //        short[] sampleBuffer = new short[read / 2];
            //        Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, read);

            //        buffer = null;
            //        //sampleBuffer = null;
            //        GC.Collect();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Only works with 16 bit audio");
            //    }
            //}     

            //wavFile.normalizedData = new double[sampleNumber];
            nornalizeData = new float[rawData.Length];

            short minData = rawData.Min();
            short maxData = rawData.Max();

            testTB.Text += minData + "\r\n";
            testTB.Text += maxData + "\r\n";

            //minData = Math.Abs(minData);
            //maxData = Math.Abs(maxData);

            //short maxAbs = Math.Max(minData, maxData);
            short maxAbs = Math.Max(Math.Abs(minData), Math.Abs(maxData));

            testTB.Text += maxAbs + "\r\n";

            //for (uint32_t i = 0; i < sampleNumber; i++)
            //{
            //    wavFile.normalizedData[i] = static_cast<double>(wavFile.rawData[i]) / maxAbs;
            //}

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = String.Format("Video Files {0}|{1}", filter1, filter2);
            tBInputVideo.Text = inputVideoFile = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "err";
            name = Transliteration(System.IO.Path.GetFileNameWithoutExtension(inputVideoFile)).Replace(' ', '_');
            outputAudioFile = String.Format("{0}\\{1}.{2}", Directory.GetCurrentDirectory(), name, format);

            if (inputVideoFile != "err")
            {
                try
                {
                    timerStatusChecker.Start();
                    Process.Start(Directory.GetCurrentDirectory() + "\\ffmpeg.exe", String.Format(" -i \"{0}\" -vn -ar 44100 -ac 2 -ab 192k -f {1} {2}", inputVideoFile, format, outputAudioFile));                   
                }
                catch
                {
                    MessageBox.Show("ffmpeg.exe не найден",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                ReadWavDataChunk();

                this.Activate();

                timerStatusChecker.Stop(); 
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            File.SetAttributes(iniFilePath, FileAttributes.ReadOnly);

            this.Text = formText;

            if (стандартныеToolStripMenuItem.Checked) { SetFilters("StandardFilters"); }

            if (расширенныеToolStripMenuItem.Checked) { SetFilters("ExtendedFilters"); }
        }

        private void стандартныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            стандартныеToolStripMenuItem.Checked = true;
            расширенныеToolStripMenuItem.Checked = false;

            SetFilters("StandardFilters");
        }

        private void расширенныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            стандартныеToolStripMenuItem.Checked = false;
            расширенныеToolStripMenuItem.Checked = true;

            SetFilters("ExtendedFilters");

        }
        #endregion
    }
}
