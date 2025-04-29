using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Recorder.MFCC;

namespace Recorder
{
    class SequenceMatching
    {
        public static double DTW_NoPruning(Sequence input, Sequence template, int N, int M)
        {
            // TODO: Implement Matching Algorithm without Pruning.
            double[,] dissimilarityMatrix = new double[N + 1, M + 1];

            for (int i = 0; i <= N; i++)
            {
                if (i == 0)
                    dissimilarityMatrix[0, i] = 0;
                else
                    dissimilarityMatrix[0, i] = dissimilarityMatrix[0, i - 1] + 1;
            }

            for (int i = 0; i <= M; i++)
            {
                dissimilarityMatrix[i, 0] = dissimilarityMatrix[i - 1, 0] + 1;
            }

            if (N == 0 || M == 0)
                return dissimilarityMatrix[N, M];

            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= M; j++)
                {
                    double insertCost = dissimilarityMatrix[i, j - 1] + 1;
                    double deleteCost = dissimilarityMatrix[i - 1, j] + 1;
                    double replaceCost = dissimilarityMatrix[i - 1, j - 1];

                    if (CompareFrames(input.Frames[i - 1], template.Frames[j - 1]) != 0)
                        replaceCost++;

                    dissimilarityMatrix[i, j] = Math.Min(Math.Min(insertCost, deleteCost), replaceCost);
                }
            }

            return dissimilarityMatrix[N, M];
        }

        public static int DTW_Pruning(Sequence input, Sequence template, int N, int M, int pruningWidth)
        {
            int dissimilarityScore = 0; // Total Distance

            // TODO: Implement Matching Algorithm with Pruning.

            return dissimilarityScore;
        }

        #region BonusFunctions
        public static void Sync_Search()
        {
            // TODO: Implement Time Synchronous Search.
        }

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
                sum += Math.Pow(frame1.Features[i] - frame2.Features[i], 2);
            }

            sum = Math.Sqrt(sum);

            return sum;
        }

        #endregion
    }
}
