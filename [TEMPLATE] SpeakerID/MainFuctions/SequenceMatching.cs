using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Recorder.MFCC;
using System.Threading.Tasks;

namespace Recorder
{
    class SequenceMatching
    {

        public static double DTW_NoPruning(Sequence input, Sequence template, int N, int M)
        {
            // TODO: Implement Matching Algorithm without Pruning.
            double[,] dissimilarityMatrix = new double[N + 1, M + 1];

            dissimilarityMatrix[0, 0] = 0;

            for (int i = 1; i <= N; i++)
            {
                dissimilarityMatrix[i, 0] = double.MaxValue;
            }

            for (int i = 1; i <= M; i++)
            {
                dissimilarityMatrix[0, i] = double.MaxValue;
            }

            if (N == 0 || M == 0)
                return dissimilarityMatrix[N, M];

            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= M; j++)
                {
                    double eculadianDistance = CompareFrames(input.Frames[i - 1], template.Frames[j - 1]);

                    double stretchCost = dissimilarityMatrix[i - 1, j];
                    double matchCost = dissimilarityMatrix[i - 1, j - 1];
                    double shrinkCost = double.MaxValue;

                    if (j >= 2)
                        shrinkCost = dissimilarityMatrix[i - 1, j - 2];

                    dissimilarityMatrix[i, j] = eculadianDistance + Math.Min(Math.Min(stretchCost, matchCost), shrinkCost);
                }
            }

            return dissimilarityMatrix[N, M];
        }

        public static double DTW_Pruning(Sequence input, Sequence template, int N, int M, int pruningWidth)
        {
            pruningWidth /= 2;
            pruningWidth = Math.Max(pruningWidth, Math.Abs(N - M));

            double[] previousRow = new double[2 * pruningWidth + 1];
            double[] currentRow = new double[2 * pruningWidth + 1];

            for (int i = 0; i < previousRow.Length; i++)
                previousRow[i] = double.MaxValue;

            previousRow[pruningWidth] = 0;

            for (int i = 1; i <= N; i++)
            {
                for (int l = 0; l < currentRow.Length; l++)
                    currentRow[l] = double.MaxValue;

                int minJ = Math.Max(1, i - pruningWidth);
                int maxJ = Math.Min(M, i + pruningWidth);

                for (int j = minJ; j <= maxJ; j++)
                {
                    int offsetJ = j - (i - pruningWidth);
                    int offsetPrevious = j - (i - 1 - pruningWidth);

                    double cost = CompareFrames(input.Frames[i - 1], template.Frames[j - 1]);

                    double stretchCost = double.MaxValue, matchCost = double.MaxValue, shrinkCost = double.MaxValue;

                    if (j >= 1)
                    {
                        if (offsetPrevious >= 0 && offsetPrevious < previousRow.Length)
                            stretchCost = previousRow[offsetPrevious];

                        if (offsetPrevious - 1 >= 0 && offsetPrevious - 1 < previousRow.Length)
                            matchCost = previousRow[offsetPrevious - 1];

                        if (j >= 1 && offsetPrevious - 2 >= 0 && offsetPrevious - 2 < previousRow.Length)
                            shrinkCost = previousRow[offsetPrevious - 2];
                    }

                    currentRow[offsetJ] = cost + Math.Min(Math.Min(stretchCost, matchCost), shrinkCost);
                }

                // Swap rows
                double[] temp = previousRow;
                previousRow = currentRow;
                currentRow = temp;
            }

            int offsetFinal = M - (N - pruningWidth);
            if (offsetFinal < 0 || offsetFinal >= previousRow.Length)
                return double.MaxValue;

            return previousRow[offsetFinal];
        }

        #region BonusFunctions

        public static void BeamSearch()
        {
            // TODO: Implement Pruning by limiting Path Cost (Beam Search).
        }

        public static void Sync_BeamSearch()
        {
            // TODO: Implement Time Synchronous Beam Search.
        }
        #endregion

        #region HelperFunctions
        public static double CompareFrames(MFCCFrame frame1, MFCCFrame frame2)
        {
            double sum = 0;

            for (int i = 0; i < 13; i++)
            {
                double diff = frame1.Features[i] - frame2.Features[i];
                sum += diff * diff;
                //sum += Math.Pow(frame1.Features[i] - frame2.Features[i], 2);
            }

            sum = Math.Sqrt(sum);

            return sum;
        }
        #endregion
    }
}
