using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCreator
{
    static class DTW
    {
        public static double CalcDistance(double[] seq1, uint seq1size, double[] seq2, uint seq2size)
        {

            // Create diff matrix
            //double** diffM = new double*[seq1size];
            //for (uint32_t i = 0; i < seq1size; i++) {
            //	diffM[i] = new  double[seq2size];
            //}
            double[,] diffM = new double[seq1size, seq2size];

            for (uint i = 0; i < seq1size; i++)
            {
                for (uint j = 0; j < seq2size; j++)
                {
                    diffM[i, j] = Math.Abs(seq1[i] - seq2[j]);
                }
            }

            // Compute distance
            double distance = FindDistance(seq1size, seq2size, diffM);

            return distance;
        }

        public static double CalcDistanceVector(double[] seq1, uint seq1size, double[] seq2, uint seq2size, short vectorSize)
        {

            uint seq1sizeV = (uint)(seq1size / vectorSize);
            uint seq2sizeV = (uint)(seq2size / vectorSize);

            // Create diff matrix
            //double** diffM = new double*[seq1sizeV];
            //for (uint32_t i = 0; i < seq1sizeV; i++) {
            //	diffM[i] = new  double[seq2sizeV];
            //}

            double[,] diffM = new double[seq1sizeV, seq2sizeV];

            for (uint i = 0; i < seq1sizeV; i++)
            {
                for (uint j = 0; j < seq2sizeV; j++)
                {

                    // Calc distance between vectors
                    double distance = 0d;
                    for (uint k = 0; k < vectorSize; k++)
                    {
                        distance += Math.Pow(seq1[i * vectorSize + k] - seq2[j * vectorSize + k], 2);
                    }

                    diffM[i, j] = Math.Sqrt(distance);
                }
            }

            // Compute distance
            double _distance = FindDistance(seq1sizeV, seq2sizeV, diffM);

            return _distance;
        }

        private static double FindDistance(uint seq1size, uint seq2size, double[,] diffM)
        {

            // Create distance matrix (forward direction)
            //double** pathM = new double*[seq1size];
            //for (uint32_t i = 0; i < seq1size; i++) {
            //	pathM[i] = new double[seq2size];
            //}

            double[,] pathM = new double[seq1size, seq2size];

            pathM[0, 0] = diffM[0, 0];
            for (uint i = 1; i < seq1size; i++)
            {
                pathM[i, 0] = diffM[i, 0] + pathM[i - 1, 0];
            }
            for (uint j = 1; j < seq2size; j++)
            {
                pathM[0, j] = diffM[0, j] + pathM[0, j - 1];
            }

            for (uint i = 1; i < seq1size; i++)
            {
                for (uint j = 1; j < seq2size; j++)
                {
                    if (pathM[i - 1, j - 1] < pathM[i - 1, j])
                    {
                        if (pathM[i - 1, j - 1] < pathM[i, j - 1])
                        {
                            pathM[i, j] = diffM[i, j] + pathM[i - 1, j - 1];
                        }
                        else
                        {
                            pathM[i, j] = diffM[i, j] + pathM[i, j - 1];
                        }
                    }
                    else
                    {
                        if (pathM[i - 1, j] < pathM[i, j - 1])
                        {
                            pathM[i, j] = diffM[i, j] + pathM[i - 1, j];
                        }
                        else
                        {
                            pathM[i, j] = diffM[i, j] + pathM[i, j - 1];
                        }
                    }
                }
            }

            // Find the warping path (backward direction)
            uint warpSize = seq1size * seq2size;
            double[] warpPath = new double[warpSize];

            uint warpPathIndex = 0;
            uint ii = seq1size - 1;
            uint jj = seq2size - 1;

            warpPath[warpPathIndex] = pathM[ii, jj];

            do
            {
                if (ii > 0 && jj > 0)
                {

                    if (pathM[ii - 1, jj - 1] < pathM[ii - 1, jj])
                    {
                        if (pathM[ii - 1, jj - 1] < pathM[ii, jj - 1])
                        {
                            ii--;
                            jj--;
                        }
                        else
                        {
                            jj--;
                        }

                    }
                    else
                    {
                        if (pathM[ii - 1, jj] < pathM[ii, jj - 1])
                        {
                            ii--;
                        }
                        else
                        {
                            jj--;
                        }
                    }

                }
                else
                {
                    if (0 == ii)
                    {
                        jj--;
                    }
                    else
                    {
                        ii--;
                    }
                }

                warpPath[++warpPathIndex] = pathM[ii, jj];

            } while (ii > 0 || jj > 0);

            // Calculate path measure
            double distance = 0d;
            for (uint k = 0; k < warpPathIndex + 1; k++)
            {
                distance += warpPath[k];
            }
            distance = distance / (warpPathIndex + 1);

            return distance;
        }
    }
}
