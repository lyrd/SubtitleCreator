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

        private void CreateFrame(ref List<Frame> frames)
        {
            uint start = 0;
            uint end = start + Constants.frameLenght;
            uint shift = (uint)(Constants.frameLenght * Constants.frameOverlap);
            uint totalAmountOfFullFrames = (uint)Math.Floor((double)(WavData.SampleNumber / shift));
            int id = 0;

            while (end <= WavData.SampleNumber)
            {
                frames.Add(new Frame(id, start, end));
                start += shift;
                end += shift;
                id++;
            }
        }

        private void GetEnthropy(ref List<Frame> frames)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                frames[i].Init(WavData.RawData, WavData.NornalizeData, frames[i].Start, frames[i].End);
            }
        }

        private void ContentsOfFrames(ref List<Frame> frames)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                if (frames[i].GetEntropy > Constants.entropyThreshold)
                {
                    frames[i].IsSound = true;
                }
                else
                {
                    frames[i].IsSound = false;
                }
            }
        }

        private List<Frame> CombiningFrames(ref List<Frame> frames)
        {
            List<Frame> combinedFrames = new List<Frame>();

            uint start = 0;
            uint end = 0;
            int id = 0;
            bool isSound = false;

            bool startIsSet = false;

            for (int i = 1; i < frames.Count; i++)
            {
                if (!startIsSet)
                {
                    start = frames[i].Start;
                    isSound = frames[i].IsSound;
                    startIsSet = true;
                }

                if (frames[i].IsSound == frames[i - 1].IsSound)//-------------------------------------------
                {
                    end = frames[i].End;
                }
                else
                {
                    combinedFrames.Add(new Frame(id, start, end, isSound));
                    id++;
                    startIsSet = false;
                    i--;
                }
            }

            return combinedFrames;
        }

        public void Recognition()
        {
            CreateFrame(ref frames);
            GetEnthropy(ref frames);
            ContentsOfFrames(ref frames);
            combinedFrames = CombiningFrames(ref frames);

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
                    streamwriter.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}", frames[j].GetId, frames[j].Start, frames[j].End, (frames[j].End - frames[j].Start)));
                }
        }

    }
}
