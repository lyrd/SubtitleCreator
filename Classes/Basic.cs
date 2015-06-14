using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    class Basic
    {
        public static double RMS(short[] source, uint start, uint finish)
        {
            double value = 0;

            for (uint i = start; i <= finish; i++)
            {
                value += source[i] * source[i];
            }
            value /= (finish - start + 1);

            return Math.Sqrt(value);
        }

        public static double Entropy(double[] source, uint start, uint finish, uint binsCount, double minRaw, double maxRaw)
        {
            double entropy = 0;
            double binSize = Math.Abs(maxRaw - minRaw) / (binsCount);

            if (Math.Abs(binSize) < Constants.Epsilon())
            {
                return 0;
            }

            double[] p = new double[binsCount];

            for (uint i = 0; i < binsCount; i++)
            {
                p[i] = 0d;//0.
            }

            //Расчет вероятностей
            uint index;

            //for (uint i = start; i <= finish; i++)
            for (uint i = start; i < finish; i++)
            {
                double value = source[i];
                index = (uint)Math.Floor((value - minRaw) / binSize);

                if (index >= binsCount)
                {
                    index = binsCount - 1;
                }

                p[index] += 1d;//1.
            }

            //Нормализация вероятностей
            uint size = finish - start + 1;

            for (uint i = 0; i < binsCount; i++)
            {
                p[i] /= size;
            }

            //Вычисление энтропии
            for (uint i = 0; i < binsCount; i++)
            {
                if (p[i] > Constants.Epsilon())
                {
                    entropy += p[i] * Math.Log(p[i], 2);
                }
            }

            entropy = -entropy;
            return entropy;
        }

        public double EuclideanDistance(double[] a, double[] b, uint size)
        {
            double distance = 0;

            for (uint i = 0; i < size; i++)
            {
                distance += Math.Pow((a[i] - b[i]), 2);
            }

            return Math.Sqrt(distance);
        }

        public double EuclideanDistanceWithWeights(double[] a, double[] b, double[] weights, uint size)
        {
            double distance = 0;

            for (uint i = 0; i < size; i++)
            {
                distance += Math.Pow((a[i] - b[i]), 2) * weights[i];
            }

            return Math.Sqrt(distance);
        }
    }
}
