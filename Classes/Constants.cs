﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    static class Constants
    {
        //Коэффициенты окна Хэмминга
        /// <summary>
        ///  alpha = 0,543478260869565
        /// </summary>
        public static readonly double alpha = 25d / 46d; //or 0.54 or 0.53836
        /// <summary>
        ///  beta = 0,456521739130435
        /// </summary>
        public static readonly double beta = (1d - alpha); //or 0.46 or 0.46164  //public static readonly decimal betta = (1m - alpha) / 2m;

        //Длина фрейма (миллисекунды)
        public static readonly ushort frameLenght = 256;//50 byte 256

        //Процент наложения фреймов (0 <= x < 1)
        public static readonly float frameOverlap = 0.5F;//0.5F

        //Минимальный размер слова
        //public static readonly short wordMinSize = (short)((200 / frameLenght) / (1 - frameOverlap));//byte
        public static readonly short wordMinSize = 10000;//10000

        //Минимальное количество фреймов между двумя словами.
        //Пусть минимальное расстояние между двумя словами составляет 50% от минимального размера слова 
        public static readonly short wordMinDistance = (short)(wordMinSize * 0.5F);
        //public static readonly short wordMinDistance = 5000;

        ///Количество MFCC коэффициетов
        public static readonly byte mfccSize = 12;//12 10

        ///Диапазон частот 300 8000
        public static readonly short mfccFreqMin = 300;//300
        public static readonly short mfccFreqMax = 4000;//4000 8000

        //public static readonly short freqThreshold = 6000;//6000

        //Порог энтропии
        public static readonly double entropyThreshold = 0.05;//0.01 0,05
        //Количество значений
        public static readonly byte entropyBins = 75;//75

        public static readonly uint sampleRate = 44100;

        /// <summary>
        /// epsilon = 2,22044604925031E-16
        /// </summary>
        public static double Epsilon()
        {
            double epsilon = 1d;
            double tmp = 1d;
            while ((1d + (tmp /= 2d)) != 1d) epsilon = tmp;
            return epsilon;
        }
    }
}
