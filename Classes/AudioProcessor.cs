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

        private static readonly string pathToBase = /*Directory.GetCurrentDirectory() +*/ "mfccBase.mfcc";

        private static List<Frame> Frames
        {
            get { return combinedFrames; }
        }

        private static string GetTime(uint size)
        {
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

            //List<double> temp = new List<double>();
            //for (int j = 0; j < combinedFrames.Count; j++)
            //{
            //    for (uint i = combinedFrames[j].GetStart; i < combinedFrames[j].GetEnd; i++)
            //    {
            //        temp.Add(WavData.RawData[i]);               
            //    }

            //    if (temp.Max() < Constants.freqThreshold & combinedFrames[j].IsSound == true)//6000
            //        combinedFrames[j].IsSound = false;

            //    temp.Clear();
            //}

            //---------------------------------------------
            for (int j = 0; j < combinedFrames.Count; j++)
            {
                if ((combinedFrames[j].GetEnd - combinedFrames[j].GetStart) < Constants.wordMinSize)
                    combinedFrames[j].IsSound = false;
            }
            //---------------------------------------------
            ////---------------------------------------------
            //for (int j = 0; j < combinedFrames.Count; j++)
            //{
            //    if ((combinedFrames[j].GetEnd - combinedFrames[j].GetStart) < Constants.wordMinSize & combinedFrames[j].IsSound == false)
            //    {
 
            //    }
            //}
            ////---------------------------------------------

            ClearLists();
        }

        private static void GetMFCC()
        {
            double[] rawdata = WavData.NornalizeData;

            //Parallel.For(0, AudioProcessor.Frames.Count, (i) =>
            //{
            //    if (AudioProcessor.Frames[i].IsSound)
            //        AudioProcessor.Frames[i].InitMFCC(ref rawdata, AudioProcessor.Frames[i].GetStart, AudioProcessor.Frames[i].GetEnd, Constants.sampleRate);
            //});

            Parallel.For(0, combinedFrames.Count, (i) =>
            {
                if (combinedFrames[i].IsSound)
                    combinedFrames[i].InitMFCC(ref rawdata, combinedFrames[i].GetStart, combinedFrames[i].GetEnd, Constants.sampleRate);
            });
        }

        private static void ReadFromDataBase(ref Dictionary<string, double[]> samplesMFCC, string pathToBase)
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

        private static void GetCaption()
        {
            Dictionary<string, double[]> samplesMFCC = new Dictionary<string, double[]>();
          
            ReadFromDataBase(ref samplesMFCC, pathToBase);

            List<double> tempMinDistances = new List<double>();

            //for (int j = 0; j < AudioProcessor.Frames.Count; j++)
            //{
            //    if (AudioProcessor.Frames[j].IsSound)
            //    {
            //        for (int i = 0; i < samplesMFCC.Count; i++)
            //        {
            //            tempMinDistances.Add(DTW.CalcDistance(AudioProcessor.Frames[j].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(i).Value, Constants.mfccSize));
            //        }
            //        tempMinDistances.Min();
            //        tempMinDistances.IndexOf(tempMinDistances.Min());
            //        AudioProcessor.Frames[j].Caption = samplesMFCC.ElementAt(tempMinDistances.IndexOf(tempMinDistances.Min())).Key;
            //        tempMinDistances.Clear();
            //    }
            //}
            for (int j = 0; j < combinedFrames.Count; j++)
            {
                if (combinedFrames[j].IsSound)
                {
                    for (int i = 0; i < samplesMFCC.Count; i++)
                    {
                        tempMinDistances.Add(DTW.CalcDistance(combinedFrames[j].GetMfcc, Constants.mfccSize, samplesMFCC.ElementAt(i).Value, Constants.mfccSize));
                    }
                    tempMinDistances.Min();
                    tempMinDistances.IndexOf(tempMinDistances.Min());
                    combinedFrames[j].Caption = samplesMFCC.ElementAt(tempMinDistances.IndexOf(tempMinDistances.Min())).Key;
                    tempMinDistances.Clear();
                }
            }
        }

        private static void GetSrtFile(string path)
        {
            string subtitleText = "";
            uint subtitleNumber = 1;
            //for (int j = 0; j < AudioProcessor.Frames.Count; j++)
            //{
            //    if (AudioProcessor.Frames[j].IsSound)
            //    {
            //        subtitleText += String.Format("{0}\r\n{1} --> {2}\r\n{3}\r\n\r\n", subtitleNumber, GetTime(AudioProcessor.Frames[j].GetStart),
            //            GetTime(AudioProcessor.Frames[j].GetEnd), AudioProcessor.Frames[j].Caption);
            //        subtitleNumber++;
            //    }
            //}
            for (int j = 0; j < combinedFrames.Count; j++)
            {
                if (combinedFrames[j].IsSound)
                {
                    subtitleText += String.Format("{0}\r\n{1} --> {2}\r\n{3}\r\n\r\n", subtitleNumber, GetTime(combinedFrames[j].GetStart),
                        GetTime(combinedFrames[j].GetEnd), combinedFrames[j].Caption);
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
