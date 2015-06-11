using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    class AudioProcessorNew
    {
        private List<Frame> frames = new List<Frame>();
        private List<Frame> combinedFrames = new List<Frame>();
        private string pathToBase;
        private string pathToSrt;

        public AudioProcessorNew(string pathToSrt)
        {
            this.pathToBase = "mfccBase.mfcc";
            this.pathToSrt = pathToSrt;
        }

        private string GetTime(uint size)
        {
            TimeSpan interval = TimeSpan.FromSeconds(size / (44100d * 1d * (16d / 8d)));
            return String.Format("{0}:{1}:{2},{3}", interval.Hours, interval.Minutes, interval.Seconds, interval.Milliseconds);
        }

        private string DoubleToString(double[] array)
        {
            string str = "";
            for (int i = 0; i < array.Length; i++)
                str += array[i] + "/";
            str = str.Substring(0, str.Length - 1);
            return str;
        }

        private List<Frame> CreateFrame()
        {
            uint start = 0;
            uint end = start + Constants.frameLenght;
            uint shift = (uint)(Constants.frameLenght * Constants.frameOverlap);
            uint totalAmountOfFullFrames = (uint)Math.Floor((double)(WavData.SampleNumber / shift));
            int id = 0;

            List<Frame> _frames = new List<Frame>();

            while (end <= WavData.SampleNumber)
            {
                _frames.Add(new Frame(id, start, end));
                start += shift;
                end += shift;
                id++;
            }

            return _frames;
        }

        private void GetEnthropy(ref List<Frame> _frames)
        {
            for (int i = 0; i < _frames.Count; i++)
            {
                _frames[i].Init(WavData.RawData, WavData.NornalizeData, _frames[i].Start, _frames[i].End);
            }
        }

        private void ContentsOfFrames(ref List<Frame> _frames)
        {
            for (int i = 0; i < _frames.Count; i++)
            {
                if (_frames[i].GetEntropy > Constants.entropyThreshold)
                {
                    _frames[i].IsSound = true;
                }
                else
                {
                    _frames[i].IsSound = false;
                }
            }
        }

        private List<Frame> CombiningFrames(ref List<Frame> _frames)
        {
            List<Frame> _combinedFrames = new List<Frame>();

            uint start = 0;
            uint end = 0;
            int id = 0;
            bool isSound = false;

            bool startIsSet = false;

            for (int i = 0; i < _frames.Count - 1; i++)
            {             
                if (!startIsSet)
                {
                    start = _frames[i].Start;
                    isSound = _frames[i].IsSound;
                    startIsSet = true;
                }

                if (_frames[i].IsSound == _frames[i + 1].IsSound)
                {
                    
                }
                else
                {
                    end = _frames[i].End;
                    _combinedFrames.Add(new Frame(id, start, end, isSound));
                    id++;
                    startIsSet = false;
                }
            }
            //=========================================================================================================================================
            for (int i = 0; i < _combinedFrames.Count; i++)
                _combinedFrames[i].Lenght = _combinedFrames[i].End - _combinedFrames[i].Start;

            for (int i = 0; i < _combinedFrames.Count - 1; i++)
                if (_combinedFrames[i + 1].Start - _combinedFrames[i].End < Constants.wordMinDistance)
                {
                    //if ((_combinedFrames[i].End - _combinedFrames[i].Start < Constants.wordMinSize) || (_combinedFrames[i + 1].End - _combinedFrames[i + 1].Start < Constants.wordMinSize))
                    {
                        _combinedFrames[i].End = _combinedFrames[i + 1].End;
                        _combinedFrames[i + 1].IsSound = false;
                    }
                }//+else

            ////----------------------------------
            //for (int i = 0; i < _combinedFrames.Count - 1; i++)
            //    if (_combinedFrames[i + 1].End - _combinedFrames[i].Start < Constants.wordMinDistance)
            //    {
            //        _combinedFrames[i].End = _combinedFrames[i + 1].End;
            //        _combinedFrames[i + 1].IsSound = false;
            //    }
            ////----------------------------------

            //for (int i = 0; i < _combinedFrames.Count; i++)
            //    if (_combinedFrames[i].End - _combinedFrames[i].Start < Constants.wordMinDistance)
            //        _combinedFrames[i].IsSound = false;

            for (int i = 0; i < _combinedFrames.Count; i++)
                if (!_combinedFrames[i].IsSound)
                {
                    _combinedFrames.RemoveAt(i);
                    i = 0;
                }

            ////----------------------------------
            //for (int i = 0; i < _combinedFrames.Count - 1; i++)
            //    if (_combinedFrames[i + 1].End - _combinedFrames[i].Start < Constants.wordMinDistance)
            //    {
            //        _combinedFrames[i].End = _combinedFrames[i + 1].End;
            //        _combinedFrames[i + 1].IsSound = false;
            //    }
            ////----------------------------------

            return _combinedFrames;
        }

        private void CalculateMFCC(List<Frame> _combinedFrames)
        {
            double[] rawdata = WavData.NornalizeData;

            Parallel.For(0, _combinedFrames.Count, (i) =>
            {
                    _combinedFrames[i].InitMFCC(ref rawdata, _combinedFrames[i].Start, _combinedFrames[i].End, Constants.sampleRate);
            });

            //for (int j = 0; j < combinedFrames.Count; j++)
            //    if (combinedFrames[j].IsSound)
            //        using (StreamWriter streamwriter = new StreamWriter("111111111111111111111111111111.txt", true, Encoding.UTF8))
            //        {
            //            streamwriter.WriteLine(String.Format("{0};{1}", "test" + j, DoubleToString(combinedFrames[j].GetMfcc)));
            //        }
        }

        private void ReadFromDataBase(ref Dictionary<string, double[]> samplesMFCC, string pathToBase)
        {
            using (StreamReader streamReader = new StreamReader(pathToBase, Encoding.UTF8))
            {
                while (true)
                {
                    string[] line = new string[2];
                    string[] array = new string[12];
                    double[] mfccs = new double[12];

                    string temp = streamReader.ReadLine();

                    if (temp == null) break;

                    line = temp.Split(';');
                    array = line[1].Split('/');

                    for (int i = 0; i < array.Length; i++)
                        Double.TryParse(array[i], out mfccs[i]);

                    samplesMFCC.Add(line[0], mfccs);
                }
            }
        }

        private void CreateCaption(ref List<Frame> _combinedFrames)
        {
            Dictionary<string, double[]> samplesMFCC = new Dictionary<string, double[]>();

            ReadFromDataBase(ref samplesMFCC, pathToBase);

            List<double> tempMinDistances = new List<double>();

            for (int j = 0; j < _combinedFrames.Count; j++)
            {
                //if (_combinedFrames[j].IsSound)
                {
                    for (int i = 0; i < samplesMFCC.Count; i++)
                    {
                        tempMinDistances.Add(DTW.CalcDistance(_combinedFrames[j].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(i).Value, Constants.mfccSize));
                    }
                    tempMinDistances.Min();
                    tempMinDistances.IndexOf(tempMinDistances.Min());
                    _combinedFrames[j].Caption = samplesMFCC.ElementAt(tempMinDistances.IndexOf(tempMinDistances.Min())).Key;
                    tempMinDistances.Clear();
                }
            }
        }

        private void SaveIntoSrtFile(ref List<Frame> _combinedFrames, string path)
        {
            string subtitleText = "";
            uint subtitleNumber = 1;

            for (int j = 0; j < _combinedFrames.Count; j++)
            {
                //if (_combinedFrames[j].IsSound)
                //if (combinedFrames[j].IsSound & !combinedFrames[j].Caption.Contains('#'))
                {
                    subtitleText += String.Format("{0}\r\n{1} --> {2}\r\n{3}\r\n\r\n", subtitleNumber, GetTime(_combinedFrames[j].Start),
                        GetTime(_combinedFrames[j].End), _combinedFrames[j].Caption);
                    subtitleNumber++;
                }
            }
            using (StreamWriter str = new StreamWriter(path + ".srt"))
            {
                str.WriteLine(subtitleText);
            }
        }

        public void Recognition()
        {
            frames = CreateFrame();
            GetEnthropy(ref frames);
            ContentsOfFrames(ref frames);
            combinedFrames = CombiningFrames(ref frames);
            CalculateMFCC(combinedFrames);
            CreateCaption(ref combinedFrames);
            SaveIntoSrtFile(ref combinedFrames, pathToSrt);
        }

        private void SaveFrames(List<Frame> frames)
        {
            try
            {
                File.Delete("111111111111111111111111111111.txt");
            }
            catch { }

            using (StreamWriter streamwriter = new StreamWriter("111111111111111111111111111111.txt", true, Encoding.UTF8))
            {
                streamwriter.WriteLine(WavData.SampleNumber + "\r\n" + frames.Count + "\r\n" + "Id\tStart\tEnd\tDelta");
            }

            for (int j = 0; j < frames.Count; j++)
                using (StreamWriter streamwriter = new StreamWriter("111111111111111111111111111111.txt", true, Encoding.UTF8))
                {
                    streamwriter.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}", frames[j].GetId, frames[j].Start, frames[j].End, (frames[j].End - frames[j].Start), frames[j].IsSound));
                }
        }
       
    }
}
