using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
             * block form while cmd
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

        private void btnTest_Click(object sender, EventArgs e)
        {       
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\ffmpeg.exe", String.Format(" -i \"{0}\" -vn -ar 44100 -ac 2 -ab 192k -f {1} {2}", inputVideoFile, format, outputAudioFile));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                inputVideoFile = dialog.FileName;
            }
            tBInputVideo.Text = inputVideoFile;

            name = Transliteration(System.IO.Path.GetFileNameWithoutExtension(inputVideoFile)).Replace(' ','_');
            outputAudioFile = String.Format("{0}\\{1}.{2}", Directory.GetCurrentDirectory(), name, format);
        }
    }
}
