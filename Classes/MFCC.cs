using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.Serialization;

//using size_t = System.UInt64;
using size_t = System.UInt32;


namespace SubtitleCreator
{
    [Serializable]
    public class ArrayDimensionException : Exception
    {
        public ArrayDimensionException() { }
        public ArrayDimensionException(string message) : base(message) { }
        public ArrayDimensionException(string message, Exception ex) : base(message) { }
        protected ArrayDimensionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    class MFCC
    {

        //Выполнение преобразования MFCC
        public static double[] Transform(double[] source, uint start, uint finish, byte mfccSize, short frequency, short freqMin, short freqMax)
        {
            uint sampleLength = finish - start + 1;
            double p2length = Math.Pow(2, Math.Floor(Math.Log(sampleLength, 2)));

            //double[] fourierRaw = FourierTransformFast(source + start, p2length, true);

            //double[] melFilters = GetMelFilters(mfccSize, p2length, frequency, freqMin, freqMax);
            //double[] logPower = CalcPower(fourierRaw, p2length, melFilters, mfccSize);
            //double[] dctRaw = DctTransform(logPower, mfccSize);
            double[] dctRaw = { 1, 2, 3 };//del
            return dctRaw;
        }

        double[] FourierTransform(double[] source, uint length, bool useWindow)
        {
            Complex[] fourierCmplxRaw = new Complex[length];
            double[] fourierRaw = new double[length];

            for (uint k = 0; k < length; k++)
            {
                fourierCmplxRaw[k] = new Complex(0d, 0d);

                for (uint n = 0; n < length; n++)
                {
                    double sample = source[n];

                    //По формуле Эйлера: e^(ix) = cos(x) + i*sin(x)
                    double x = -2 * Math.PI * k * n / (double)length;

                    Complex f = sample * new Complex(Math.Cos(x), Math.Sin(x));

                    double w = 1;
                    if (useWindow)
                    {
                        //Окно Хэмминга
                        w = Constants.alpha - Constants.beta * Math.Cos(2 * Math.PI * n / (length - 1));
                    }

                    fourierCmplxRaw[k] += f * w;
                }

                //Магнитуда
                fourierRaw[k] = fourierCmplxRaw[k].Magnitude;
            };

            return fourierRaw;
        }

        double[] FourierTransformFast(double[] source, uint length, bool useWindow)
        {
            //Расширить длину исходных данных до степени двойки
            uint p2length = length;

            //bool powerOfTwo = (length > 0) && !(length & (length - 1));
            //assert("FFT input data size must have 2^n size" && powerOfTwo);
            //// p2length = pow(2, ceil(log2(length)));

            double[] fourierRaw = new double[length];
            Complex[] fourierRawTmp = new Complex[length];

            for (uint i = 0; i < p2length; i++)
            {
                //Каждый элемент - вещественная часть комплексного числа
                if (i < length)
                {
                    fourierRawTmp[i] = new Complex(source[i], 0d);

                    if (useWindow)
                    {
                        fourierRawTmp[i] *= (Constants.alpha - Constants.beta * Math.Cos(2 * Math.PI * i / (length - 1)));
                    }

                }
                else
                {
                    fourierRawTmp[i] = new Complex(0d, 0d);
                }
            }

            //Рекурсивные вычисления
            FourierTransformFastRecursion(ref fourierRawTmp);

            //Магнитуда
            for (uint i = 0; i < length; i++)
            {
                fourierRaw[i] = fourierRawTmp[i].Magnitude;
            }

            return fourierRaw;
        }

        void FourierTransformFastRecursion(ref Complex[] data)
        {

            //Выход из рекурсии
            size_t n = (size_t)data.Count();
            if (n <= 1)
            {
                return;
            }

            //Разделение
            Complex[] even = data.AsParallel().Where((s, index) => { return index % 2 == 0; }).ToArray();
            Complex[] odd = data.AsParallel().Where((s, index) => { return index % 2 != 0; }).ToArray();

            FourierTransformFastRecursion(ref even);
            FourierTransformFastRecursion(ref odd);

            //Объединение 
            for (size_t i = 0; i < n / 2; i++)
            {
                Complex t = Complex.FromPolarCoordinates(1.0, -2 * Math.PI * i / n) * odd[i];
                data[i] = even[i] + t;
                data[i + n / 2] = even[i] - t;
            }
        }










        public static Complex[] Test()
        {
            //Complex c1 = new Complex(12, 6);
            Complex[] c2 = new Complex[4];

            c2[0] = new Complex(3.0, 4.0);
            c2[1] = new Complex(3.0, 4.0);
            c2[2] = new Complex(3.0, 4.0);
            c2[3] = new Complex(3.0, 4.0);
            return c2;
        }

    }
}
