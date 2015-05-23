using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static List<Frame> Frames
        {
            get { return combinedFrames; }
        }

        private static void ClearLists()
        {
            frames.Clear();
            IsSound.Clear();
            pointsOfChangeValues.Clear();
            borders.Clear();
        }

        public static void GetFrames()
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

    }
}
