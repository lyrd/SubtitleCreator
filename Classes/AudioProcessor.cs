using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    static class AudioProcessor
    {
        private static uint start = 0;
        private static uint finish = start + Constants.frameLenght;
        private static uint shift = (uint)(Constants.frameLenght * Constants.frameOverlap);
        private static uint totalAmountOfFullFrames = (uint)Math.Floor((double)WavData.SampleNumber / 128);
        private static int count = 0;
        private static bool flag;
        private static List<Frame> frames = new List<Frame>();
        private static List<Frame> combinedFrames = new List<Frame>();
        private static List<bool> IsSound = new List<bool>();
        private static List<int> pointsOfChangeValues = new List<int>();
        private static List<Point> borders = new List<Point>();

        private static List<Frame> Frames
        {
            get { return combinedFrames; }
        }

        private static string GetTime(uint size)
        {
            //double time = size / (44100d * 1d * (16d / 8d));
            //return String.Format("{0}:{1}:{2}", 00, 00, Math.Round(time, 3));
            TimeSpan interval = TimeSpan.FromSeconds(size / (44100d * 1d * (16d / 8d)));
            return String.Format("{0}:{1}:{2},{3}", interval.Hours, interval.Minutes, interval.Seconds, interval.Milliseconds);
        }

        private static void ClearLists()
        {
            frames.Clear();
            IsSound.Clear();
            pointsOfChangeValues.Clear();
            borders.Clear();
        }

        private static void GetFrames()
        {
            //Создание фреймов
            for (int i = 0; i < totalAmountOfFullFrames; i++)
            {
                if (i < totalAmountOfFullFrames - 1)
                {
                    frames.Add(new Frame(i, start, finish));
                    start += shift;
                    finish = start + Constants.frameLenght;
                }
            }

            //Вычисления энтропии фреймов
            for (int i = 0; i < totalAmountOfFullFrames - 2; i++)
            {
                frames[i].Init(WavData.RawData, WavData.NornalizeData, frames[i].GetStart, frames[i].GetEnd);
            }

            //Определение содержимого фреймов (тишина или звук)
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

            flag = IsSound[0];

            //Определение точек, где изменяется содержимое фрейма
            pointsOfChangeValues.Add(0);

            for (int i = 0; i < IsSound.Count; i++)
            {

                if (IsSound[i] == flag)
                {
                    count++;
                }
                else
                {
                    pointsOfChangeValues.Add(count);
                    flag = !flag;
                    i--;
                }
            }

            pointsOfChangeValues.Add(IsSound.Count - 1);

            //Определения новых границ фреймов на основе точек
            for (int i = 0; i < pointsOfChangeValues.Count - 1; i++)
                borders.Add(new Point(pointsOfChangeValues[i], pointsOfChangeValues[i + 1] - 1));

            //Создание новый фреймов
            for (int i = 0; i < borders.Count; i++)
            {
                combinedFrames.Add(new Frame(i, frames[borders[i].X].GetStart, frames[borders[i].Y].GetEnd));
            }

            //Вычисление энтропии
            for (int i = 0; i < combinedFrames.Count; i++)
            {
                combinedFrames[i].Init(WavData.RawData, WavData.NornalizeData, combinedFrames[i].GetStart, combinedFrames[i].GetEnd);
            }

            //Содержание фреймов
            for (int i = 0; i < combinedFrames.Count; i++)
            {
                if (combinedFrames[i].GetEntropy > Constants.entropyThreshold)
                    combinedFrames[i].IsSound = true;
                else
                    combinedFrames[i].IsSound = false;
            }

            List<double> temp = new List<double>();
            for (int j = 0; j < combinedFrames.Count; j++)
            {
                for (uint i = combinedFrames[j].GetStart; i < combinedFrames[j].GetEnd; i++)
                {
                    temp.Add(WavData.RawData[i]);               
                }

                if (temp.Max() < 6000 & combinedFrames[j].IsSound == true)
                    combinedFrames[j].IsSound = false;

                temp.Clear();
            }

            ClearLists();
        }

        private static void GetMFCC()
        {
            double[] rawdata = WavData.NornalizeData;

            Parallel.For(0, AudioProcessor.Frames.Count, (i) =>
            {
                if (AudioProcessor.Frames[i].IsSound)
                    AudioProcessor.Frames[i].InitMFCC(ref rawdata, AudioProcessor.Frames[i].GetStart, AudioProcessor.Frames[i].GetEnd, Constants.sampleRate);
            });
        }

        //TODO: LoadMFCC from file
        private static void GetCaption()
        {
            Dictionary<string, double[]> samplesMFCC = new Dictionary<string, double[]>();

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
            
            List<double> tempMinDistances = new List<double>();

            for (int j = 0; j < AudioProcessor.Frames.Count; j++)
            {
                if (AudioProcessor.Frames[j].IsSound)
                {
                    for (int i = 0; i < samplesMFCC.Count; i++)
                    {
                        tempMinDistances.Add(DTW.CalcDistance(AudioProcessor.Frames[j].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(i).Value, Constants.mfccSize));
                    }
                    tempMinDistances.Min();
                    tempMinDistances.IndexOf(tempMinDistances.Min());
                    AudioProcessor.Frames[j].Caption = samplesMFCC.ElementAt(tempMinDistances.IndexOf(tempMinDistances.Min())).Key;
                    tempMinDistances.Clear();
                }
            }
        }

        private static void GetSrtFile(string path)
        {
            string subtitleText = "";
            uint subtitleNumber = 1;
            for (int j = 0; j < AudioProcessor.Frames.Count; j++)
            {
                if (AudioProcessor.Frames[j].IsSound)
                {
                    subtitleText += String.Format("{0}\r\n{1} --> {2}\r\n{3}\r\n\r\n", subtitleNumber, GetTime(AudioProcessor.Frames[j].GetStart),
                        GetTime(AudioProcessor.Frames[j].GetEnd), AudioProcessor.Frames[j].Caption);
                    subtitleNumber++;
                }
            }

            using (StreamWriter str = new StreamWriter(path + ".srt"))
            {
                str.WriteLine(subtitleText);
            }
        }

        public static void Recognition(string audioFile, string srtFile)
        {
            GetFrames();
            GetMFCC();
            GetCaption();
            GetSrtFile(srtFile);
            File.Delete(audioFile);
        }
    }
}
