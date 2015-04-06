using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    static class Constants
    {
        // Hamming Window double -> decimal?

        //Float - 7 digits (32 bit)
        //Double-15-16 digits (64 bit)
        //Decimal -28-29 significant digits (128 bit)

        //float flt = 1F/3;
        //double dbl = 1D/3;
        //decimal dcm = 1M/3;
        //Console.WriteLine("float: {0} double: {1} decimal: {2}", flt, dbl, dcm);

        //Result :

        //float: 0.3333333  
        //double: 0.333333333333333  
        //decimal: 0.3333333333333333333333333333

        //Коэффициенты окна Хэмминга
        public static readonly double alpha = 25d / 46d;
        public static readonly double beta = (1d - alpha);
        //public static readonly decimal betta = (1m - alpha) / 2m;

        //Длина фрейма (миллисекунды)
        public static readonly byte frameLenght = 50;

        //Процент наложения фреймов (0 <= x < 1)
        public static readonly float frameOverlap = 0.5F;

        /*
        Минимальный размер слова (во фреймах)
        В базе слов - avg 500 мс.
        Пусть - min 200 мс.
        */
        public static readonly byte wordMinSize = (byte)((200 / frameLenght) / (1 - frameOverlap));

        /*
        Минимальное количество фреймов между двумя словами.
        Пусть минимальное расстояние между двумя словами составляет 50% от минимального размера слова 
        */
        public static readonly byte wordMinDistance = (byte)(wordMinSize * 0.5F);

        //Количество MFCC коэффициетов
        public static readonly byte mfccSize = 12;

        //Диапазон частот
        public static readonly short mfccFreqMin = 300;
        public static readonly short mfccFreqMax = 4000;

        //Параметры энтропии
        public static readonly byte entropyBins = 75;
        public static readonly double entropyThreshold = 0.1;

        public static double Epsilon()
        {
            double epsilon = 1d;
            double tmp = 1d;
            while ((1d + (tmp /= 2d)) != 1d) epsilon = tmp;
            return epsilon;
        }
    }
}
