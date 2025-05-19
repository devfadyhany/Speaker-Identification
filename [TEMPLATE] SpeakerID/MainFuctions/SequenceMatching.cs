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

            if (input == template)
                return 0;

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
            // TODO: Implement Matching Algorithm with Pruning.

            if (input == template)
                return 0;

            pruningWidth /= 2;
            pruningWidth = Math.Max(pruningWidth, Math.Abs(N - M));

            //Dictionary<(int, int), double> distanceMemo = new Dictionary<(int, int), double>();

            //return DTW_Pruning_DP(input, template, N, M, pruningWidth, distanceMemo);

            return 0;

            //double[,] D = new double[N + 1, M + 1];

            //for (int i = 0; i <= N; i++)
            //{
            //    for (int j = 0; j <= M; j++)
            //    {
            //        D[i, j] = double.MaxValue;
            //    }
            //}

            //D[0, 0] = 0;

            //for (int i = 1; i <= N; i++)
            //{
            //    int minJ = Math.Max(1, i - pruningWidth);
            //    int maxJ = Math.Min(M, i + pruningWidth);

            //    for (int j = minJ; j <= maxJ; j++)
            //    {
            //        double cost = CompareFrames(input.Frames[i - 1], template.Frames[j - 1]);

            //        double stretchCost = D[i - 1, j];
            //        double matchCost = D[i - 1, j - 1];
            //        double shrinkCost = double.MaxValue;

            //        if (j >= 2)
            //            shrinkCost = D[i - 1, j - 2];

            //        D[i, j] = cost + Math.Min(Math.Min(stretchCost, matchCost), shrinkCost);
            //    }
            //}

            //return D[N, M];
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

        //public static double DTW_Pruning_DP(Sequence input, Sequence template, int i, int j, int pruningWidth, Dictionary<(int, int), double> distanceMemo)
        //{
        //    if (i == 0 && j == 0)
        //        return 0;

        //    if (i < 0 || j < 0 || Math.Abs(i - j) > pruningWidth)
        //        return double.MaxValue;

        //    if (distanceMemo.ContainsKey((i, j)))
        //        return distanceMemo[(i, j)];

        //    double cost = CompareFrames(input.Frames[i - 1], template.Frames[j - 1]);

        //    double stretchCost = DTW_Pruning_DP(input, template, i - 1, j, pruningWidth, distanceMemo);
        //    double matchCost = DTW_Pruning_DP(input, template, i - 1, j - 1, pruningWidth, distanceMemo);
        //    double shrinkCost = double.MaxValue;

        //    if (j >= 2)
        //        shrinkCost = DTW_Pruning_DP(input, template, i - 1, j - 2, pruningWidth, distanceMemo);

        //    distanceMemo[(i, j)] = cost + Math.Min(Math.Min(stretchCost, matchCost), shrinkCost);

        //    return distanceMemo[(i, j)];
        //}

        public static int MinDirection(double a, double b, double c)
        {
            if (b <= a && b <= c)
                return 2;

            if (a <= b && a <= c)
                return 1;

            return 3;
        }

        public static double CalcTotalDistance(double[,] dissimilarityMatrix, int N, int M)
        {
            int x = N, y = M;
            double sum = 0;

            while (x > 0 && y > 0)
            {
                sum += dissimilarityMatrix[x, y];

                int direction;

                if (y > 1)
                    direction = MinDirection(dissimilarityMatrix[x - 1, y], dissimilarityMatrix[x - 1, y - 1], dissimilarityMatrix[x - 1, y - 2]);
                else
                    direction = 2;

                if (y <= 1 && x == 0)
                    return sum;

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
