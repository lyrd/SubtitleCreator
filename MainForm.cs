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
         * фильтр                                    †|-   
         *progressBar1.Width = this.Width - 40;
         *progressBar1.Top = this.Height - 73;
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
        private string[] videoFormats = { ".264", ".3g2", ".3gp", ".3gp2", ".3gpp", ".3gpp2", ".3mm", ".3p2", ".60d", ".787", ".890", ".aaf", ".aec", ".aep", ".aepx", ".aet", ".aetx", ".ajp", ".ale", ".am", ".amc", ".amv", ".amx", ".anim", ".aqt", ".arcut", ".arf", ".asf", ".asx", ".avb", ".avchd", ".avd", ".avi", ".avp", ".avs", ".avs", ".avv", ".awlive", ".axm", ".bdm", ".bdmv", ".bik", ".bin", ".bix", ".bnp", ".box", ".bs4", ".bsf", ".bu", ".bvr", ".byu", ".camproj", ".camrec", ".camv", ".ced", ".cine", ".cip", ".clpi", ".cmmp", ".cmmtpl", ".cvc", ".cx3", ".d2v", ".d3v", ".dash", ".dat", ".dav", ".dce", ".dck", ".ddat", ".dif", ".dir", ".divx", ".dlx", ".dmb", ".dmsd", ".dmsd3d", ".dmsm", ".dmsm3d", ".dmss", ".dmx", ".dnc", ".dpa", ".dpg", ".dream", ".dv", ".dv-avi", ".dv4", ".dvr", ".dvr-ms", ".dvx", ".dxr", ".dzm", ".dzp", ".dzt", ".edl", ".evo", ".eye", ".f4f", ".f4p", ".f4v", ".fbr", ".fbz", ".flc", ".flh", ".fli", ".flv", ".flx", ".ftc", ".gfp", ".gl", ".gom", ".grasp", ".gts", ".gvi", ".gvp", ".h264", ".hdmov", ".hdv", ".hkm", ".ifo", ".imovieproject", ".ircp", ".irf", ".ismc", ".ismv", ".iva", ".ivf", ".ivr", ".ivs", ".izz", ".izzy", ".jmv", ".jss", ".jts", ".jtv", ".k3g", ".kmv", ".lrec", ".lrv", ".lsf", ".lsx", ".lvix", ".m15", ".m1pg", ".m1v", ".m21", ".m21", ".m2a", ".m2t", ".m2ts", ".m2v", ".m4e", ".m4u", ".m4v", ".m75", ".mani", ".meta", ".mgv", ".mj2", ".mjp", ".mjpg", ".mk3d", ".mkv", ".mmv", ".mnv", ".mob", ".mod", ".moff", ".moi", ".moov", ".mov", ".movie", ".mp21", ".mp21", ".mp2v", ".mp4", ".mp4.infovid", ".mp4v", ".mpe", ".mpeg", ".mpeg1", ".mpeg4", ".mpf", ".mpg", ".mpg2", ".mpgindex", ".mpl", ".mpls", ".mpsub", ".mpv", ".mpv2", ".mqv", ".msdvd", ".msh", ".mswmm", ".mts", ".mtv", ".mvb", ".mvc", ".mvd", ".mve", ".mvex", ".mvp", ".mvy", ".mxf", ".mxv", ".mys", ".ncor", ".nsv", ".nut", ".nuv", ".nvc", ".ogm", ".ogv", ".ogx", ".orv", ".otrkey", ".par", ".pds", ".pgi", ".photoshow", ".piv", ".pjs", ".playlist", ".plproj", ".pmf", ".pmv", ".ppj", ".prel", ".pro", ".pro4dvd", ".pro5dvd", ".proqc", ".prproj", ".prtl", ".prx", ".psh", ".pssd", ".pva", ".pvr", ".pxv", ".qt", ".qtch", ".qtindex", ".qtl", ".qtm", ".qtz", ".r3d", ".rdb", ".rec", ".rm", ".rmd", ".rmp", ".rms", ".rmv", ".rmvb", ".roq", ".rp", ".rsx", ".rts", ".rts", ".rum", ".rv", ".rvl", ".sbk", ".sbt", ".scm", ".scm", ".scn", ".sdc", ".sdv", ".sedprj", ".sfvidcap", ".siv", ".smi", ".smil", ".smk", ".sml", ".smv", ".snagproj", ".spl", ".srt", ".ssm", ".str", ".stx", ".svi", ".swf", ".swi", ".swt", ".tda3mt", ".tdx", ".tid", ".tivo", ".tix", ".tod", ".tp", ".tp0", ".tpd", ".tpr", ".trp", ".ts", ".ttxt", ".tvs", ".usm", ".vbc", ".vc1", ".vcpf", ".vcr", ".vcv", ".vdo", ".vdr", ".veg", ".vem", ".vep", ".vf", ".vft", ".vfw", ".vfz", ".vgz", ".vid", ".video", ".viewlet", ".viv", ".vivo", ".vix", ".vlab", ".vob", ".vp3", ".vp6", ".vp7", ".vpj", ".vro", ".vs4", ".vse", ".vsp", ".w32", ".wcp", ".webm", ".wm", ".wmd", ".wmmp", ".wmv", ".wmx", ".wp3", ".wtv", ".wvx", ".xej", ".xel", ".xesc", ".xfl", ".xlmv", ".xmv", ".xvid", ".y4m", ".yog", ".yuv", ".zeg", ".zm1", ".zm2", ".zm3", ".zmv", ".264", ".3g2", ".3gp", ".3gp2", ".3gpp", ".3gpp2", ".3mm", ".3p2", ".60d", ".787", ".890", ".aaf", ".aec", ".aep", ".aepx", ".aet", ".aetx", ".ajp", ".ale", ".am", ".amc", ".amv", ".amx", ".anim", ".aqt", ".arcut", ".arf", ".asf", ".asx", ".avb", ".avchd", ".avd", ".avi", ".avp", ".avs", ".avs", ".avv", ".awlive", ".axm", ".bdm", ".bdmv", ".bik", ".bin", ".bix", ".bnp", ".box", ".bs4", ".bsf", ".bu", ".bvr", ".byu", ".camproj", ".camrec", ".camv", ".ced", ".cine", ".cip", ".clpi", ".cmmp", ".cmmtpl", ".cvc", ".cx3", ".d2v", ".d3v", ".dash", ".dat", ".dav", ".dce", ".dck", ".ddat", ".dif", ".dir", ".divx", ".dlx", ".dmb", ".dmsd", ".dmsd3d", ".dmsm", ".dmsm3d", ".dmss", ".dmx", ".dnc", ".dpa", ".dpg", ".dream", ".dv", ".dv-avi", ".dv4", ".dvr", ".dvr-ms", ".dvx", ".dxr", ".dzm", ".dzp", ".dzt", ".edl", ".evo", ".eye", ".f4f", ".f4p", ".f4v", ".fbr", ".fbz", ".flc", ".flh", ".fli", ".flv", ".flx", ".ftc", ".gfp", ".gl", ".gom", ".grasp", ".gts", ".gvi", ".gvp", ".h264", ".hdmov", ".hdv", ".hkm", ".ifo", ".imovieproject", ".ircp", ".irf", ".ismc", ".ismv", ".iva", ".ivf", ".ivr", ".ivs", ".izz", ".izzy", ".jmv", ".jss", ".jts", ".jtv", ".k3g", ".kmv", ".lrec", ".lrv", ".lsf", ".lsx", ".lvix", ".m15", ".m1pg", ".m1v", ".m21", ".m21", ".m2a", ".m2t", ".m2ts", ".m2v", ".m4e", ".m4u", ".m4v", ".m75", ".mani", ".meta", ".mgv", ".mj2", ".mjp", ".mjpg", ".mk3d", ".mkv", ".mmv", ".mnv", ".mob", ".mod", ".moff", ".moi", ".moov", ".mov", ".movie", ".mp21", ".mp21", ".mp2v", ".mp4", ".mp4.infovid", ".mp4v", ".mpe", ".mpeg", ".mpeg1", ".mpeg4", ".mpf", ".mpg", ".mpg2", ".mpgindex", ".mpl", ".mpls", ".mpsub", ".mpv", ".mpv2", ".mqv", ".msdvd", ".msh", ".mswmm", ".mts", ".mtv", ".mvb", ".mvc", ".mvd", ".mve", ".mvex", ".mvp", ".mvy", ".mxf", ".mxv", ".mys", ".ncor", ".nsv", ".nut", ".nuv", ".nvc", ".ogm", ".ogv", ".ogx", ".orv", ".otrkey", ".par", ".pds", ".pgi", ".photoshow", ".piv", ".pjs", ".playlist", ".plproj", ".pmf", ".pmv", ".ppj", ".prel", ".pro", ".pro4dvd", ".pro5dvd", ".proqc", ".prproj", ".prtl", ".prx", ".psh", ".pssd", ".pva", ".pvr", ".pxv", ".qt", ".qtch", ".qtindex", ".qtl", ".qtm", ".qtz", ".r3d", ".rdb", ".rec", ".rm", ".rmd", ".rmp", ".rms", ".rmv", ".rmvb", ".roq", ".rp", ".rsx", ".rts", ".rts", ".rum", ".rv", ".rvl", ".sbk", ".sbt", ".scm", ".scm", ".scn", ".sdc", ".sdv", ".sedprj", ".sfvidcap", ".siv", ".smi", ".smil", ".smk", ".sml", ".smv", ".snagproj", ".spl", ".srt", ".ssm", ".str", ".stx", ".svi", ".swf", ".swi", ".swt", ".tda3mt", ".tdx", ".tid", ".tivo", ".tix", ".tod", ".tp", ".tp0", ".tpd", ".tpr", ".trp", ".ts", ".ttxt", ".tvs", ".usm", ".vbc", ".vc1", ".vcpf", ".vcr", ".vcv", ".vdo", ".vdr", ".veg", ".vem", ".vep", ".vf", ".vft", ".vfw", ".vfz", ".vgz", ".vid", ".video", ".viewlet", ".viv", ".vivo", ".vix", ".vlab", ".vob", ".vp3", ".vp6", ".vp7", ".vpj", ".vro", ".vs4", ".vse", ".vsp", ".w32", ".wcp", ".webm", ".wm", ".wmd", ".wmmp", ".wmv", ".wmx", ".wp3", ".wtv", ".wvx", ".xej", ".xel", ".xesc", ".xfl", ".xlmv", ".xmv", ".xvid", ".y4m", ".yog", ".yuv", ".zeg", ".zm1", ".zm2", ".zm3", ".zmv"};
        private string formText = "Subtitle Creator";
        private enum Status { idle, in_work };
        private Status status = Status.idle;

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

        private string FormatListBuilder(string[] _formats)
        {
            string output = "(";

            for (int i = 0; i < _formats.Length; i++)
                output += String.Format("*{0}, ", _formats[i]);

            output = output.Substring(0, output.Length - 2);
            output += ")";

            return output;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            timerStatusChecker.Start();

            Process.Start(Directory.GetCurrentDirectory() + "\\ffmpeg.exe", String.Format(" -i \"{0}\" -vn -ar 44100 -ac 2 -ab 192k -f {1} {2}", inputVideoFile, format, outputAudioFile));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            string filter1 = FormatListBuilder(videoFormats);
            string filter2 = filter1.Replace(',', ';').Replace("(", "").Replace(")", "");

            dialog.Filter = String.Format("Video Files {0}|{1}", filter1, filter2);
            tBInputVideo.Text = inputVideoFile = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "err";
            name = Transliteration(System.IO.Path.GetFileNameWithoutExtension(inputVideoFile)).Replace(' ','_');
            outputAudioFile = String.Format("{0}\\{1}.{2}", Directory.GetCurrentDirectory(), name, format);
        }

        private void timerStatusChecker_Tick(object sender, EventArgs e)
        {
            status = Process.GetProcessesByName("ffmpeg").Any() ? Status.in_work : Status.idle;
            this.Text = status == Status.in_work ? formText + " - IN WORK" : formText;
            this.Enabled = status == Status.in_work ? false : true;
        }

    }
}
