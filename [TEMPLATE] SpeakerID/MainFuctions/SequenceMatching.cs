using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Recorder.MFCC;

namespace Recorder
{
    class SequenceMatching
    {
        public static int DTW_NoPruning(Sequence input, Sequence template, int N, int M)
        {
            int dissimilarityScore = 0; // Total Distance

            // TODO: Implement Matching Algorithm without Pruning.

            return dissimilarityScore;
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

        #endregion
    }
}
