using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    class Frame
    {
        private int id;
        private double rms;
        private double entropy;
        private double[] mfcc;

        private uint start;
        private uint end;

        private bool isSound;

        private string caption;

        private uint lenght;

        public Frame(int id, uint start, uint end)
        {
            this.id = id;
            this.rms = 0;
            this.entropy = 0;
            this.mfcc = null;

            this.start = start;
            this.end = end;

            this.isSound = false;

            this.caption = "";

            this.lenght = Constants.frameLenght;
        }

        public Frame(int id, uint start, uint end, bool isSound)
        {
            this.id = id;
            this.rms = 0;
            this.entropy = 0;
            this.mfcc = null;

            this.start = start;
            this.end = end;

            this.isSound = isSound;

            this.caption = "";

            this.lenght = Constants.frameLenght;
        }

        public uint Lenght
        {
            get { return this.lenght; }
            set { this.lenght = value; }
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
//#if DEBUG
//            set { this.mfcc = value; }
//#endif
        }

        public uint Start
        {
            get { return this.start; }
            set { this.start = value; }
        }

        public uint End
        {
            get { return this.end; }
            set { this.end = value; }
        }

        public bool IsSound
        {
            get { return this.isSound; }
            set { this.isSound = value; }
        }

        public string Caption
        {
            get { return this.caption; }
            set { this.caption = value; }
        }

        public void Init(short[] source, double[] sourceNormalized, uint start, uint finish)
        {
            //this.rms = Basic.RMS(source, start, finish);
            this.entropy = Basic.Entropy(sourceNormalized, start, finish, Constants.entropyBins, -1, 1);
        }

        public void InitMFCC(ref double[] source, uint start, uint finish, uint freq)//short freq
        {
            this.mfcc = MFCC.Transform(ref source, start, finish, Constants.mfccSize, freq, Constants.mfccFreqMin, Constants.mfccFreqMax);
        }

    }
}
