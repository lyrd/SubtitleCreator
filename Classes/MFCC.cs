using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    class MFCC
    {
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
    }
}
