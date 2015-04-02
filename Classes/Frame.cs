using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    class Frame
    {
        private uint id;
        private double rms;
        private double entropy;
        private double[] mfcc;

        public Frame(UInt32 id)
        {
            this.id = id;
            this.rms = 0;
            this.entropy = 0;
            this.mfcc = null;
        }

        public void Init(short[] source, double[] sourceNormalized, uint start, uint finish)
        {

            this.rms = Basic.RMS(source, start, finish);
            this.entropy = Basic.Entropy(sourceNormalized, start, finish, Constants.entropyBins, -1, 1);
        }

        public double[] InitMFCC(double[] source, uint start, uint finish, short freq)
        {

            this.mfcc = MFCC.Transform(source, start, finish, Constants.mfccSize, freq, Constants.mfccFreqMin, Constants.mfccFreqMax);
            return this.mfcc;
        }

    }
}
