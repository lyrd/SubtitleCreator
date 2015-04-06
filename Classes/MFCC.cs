using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SubtitleCreator
{
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
                fourierCmplxRaw[k] = new Complex(0, 0);

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
                        //w = 0.54 - 0.46 * Math.Cos(2 * Math.PI * n / (length - 1));
                        //w = 0.53836 - 0.46164 * Math.Cos(2 * Math.PI * n / (length - 1));
                        w = Constants.alpha - Constants.beta * Math.Cos(2 * Math.PI * n / (length - 1));
                    }

                    fourierCmplxRaw[k] += f * w;
                }

                //Магнитуда
                //fourierRaw[k] = Math.Sqrt(norm(fourierCmplxRaw[k]));
                fourierRaw[k] = fourierCmplxRaw[k].Magnitude;
            };

            return fourierRaw;
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
