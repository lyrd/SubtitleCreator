using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    class Frame
    {
        private int id;//uint
        private double rms;
        private double entropy;
        private double[] mfcc;

        private uint start;
        private uint end;

        private bool isSound;

        public Frame(int id, uint start, uint end)
        {
            this.id = id;
            this.rms = 0;
            this.entropy = 0;
            this.mfcc = null;

            this.start = start;
            this.end = end;

            this.isSound = false;
        }

        public int GetId
        {
            get { return this.id; }
        }

        public double GetRms
        {
            get { return this.rms; }
        }

        public double GetEntropy
        {
            get { return this.entropy; }
        }

        public double[] GetMfcc
        {
            get { return this.mfcc; }
        }

        public uint GetStart
        {
            get { return this.start; }
            set { this.start = value; }
        }

        public uint GetEnd
        {
            get { return this.end; }
            set { this.end = value; }
        }

        public bool IsSound
        {
            get { return this.isSound; }
            set { this.isSound = value; }
        }

        public void Init(short[] source, double[] sourceNormalized, uint start, uint finish)
        {
            this.rms = Basic.RMS(source, start, finish);
            this.entropy = Basic.Entropy(sourceNormalized, start, finish, Constants.entropyBins, -1, 1);
        }

        //public double[] InitMFCC(double[] source, uint start, uint finish, uint freq)//short freq
        public void InitMFCC(double[] source, uint start, uint finish, uint freq)//short freq
        {
            this.mfcc = MFCC.Transform(source, start, finish, Constants.mfccSize, freq, Constants.mfccFreqMin, Constants.mfccFreqMax);
            //return this.mfcc;
        }

    }
}
