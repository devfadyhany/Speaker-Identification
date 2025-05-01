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

            dissimilarityMatrix[0, 0] = 0;

            for (int i = 1; i <= N; i++)
            {
                dissimilarityMatrix[0, i] = double.MaxValue;
            }

            for (int i = 1; i <= M; i++)
            {
                dissimilarityMatrix[i, 0] = double.MaxValue;
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

            return CalcTotalDistance(dissimilarityMatrix, N, M);
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

        public static int MinDirection(double a, double b, double c)
        {
            if (b <= a && b <= c)
                return 1;

            if (a <= b && a <= c)
                return 2;

            return 3;
        }

        public static double CalcTotalDistance(double[,] dissimilarityMatrix, int N, int M)
        {
            int x = N, y = M;
            double sum = 0;

            while (x > 0 && y > 0)
            {
                sum += dissimilarityMatrix[x, y];

                int direction = MinDirection(dissimilarityMatrix[x - 1, y], dissimilarityMatrix[x - 1, y - 1], dissimilarityMatrix[x - 1, y - 2]);

                switch (direction)
                {
                    case 1:
                        x--;
                        y--;
                        break;
                    case 2:
                        x--;
                        break;
                    case 3:
                        x--;
                        y -= 2;
                        break;
                }
            }

            return sum;
        }

        #endregion
    }
}
