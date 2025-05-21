using Recorder.MFCC;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Recorder
{
    class UserIdentification
    {
        #region Requirements

        /*
         * Inputs
         * ==========================================
         * 1.Labeled template sequences (i.e. a name associated with each sequence).
         * 2.New unknown input sequence.
         * 3.Pruning width (W).
         * 
         * Outputs
         * ==========================================
         * 1.Label of the unknown input sequence.
         * 2.Final cost of the matched sequence.
         */


        /*
         * TODO: Implement Enrollment Phase Functions
         * ==========================================
         * New users can enroll (register) themselves by recording a few words,
         * and typing their name at the end.
         */

        /*
         * TODO: Implement Identification Phase Functions
         * ==========================================
         * The program can identify an existing user 
         * by recording a new speech segment for the user,
         * and by applying the sequence matching algorithm
         * between the input sequence and the entire pre-existing template sequences,
         * the program should determine the name of the user and output it on the screen.
         */

        #endregion

        public static Dictionary<AudioSignal, Sequence> templateSequences = new Dictionary<AudioSignal, Sequence>();

        private static bool matchLock = false;

        public static void AddVoice(string name, AudioSignal myaudio, List<User> templateDB)
        {
            foreach (User existUser in templateDB)
            {
                if (existUser.UserName == name)
                {
                    existUser.UserTemplates.Add(myaudio);
                    MessageBox.Show("New audio has been added successfully to (" + existUser.UserName + ")!");
                    return;
                }
            }

            User newUser = new User();

            newUser.UserName = name;
            newUser.UserTemplates = new List<AudioSignal>() { myaudio };

            templateDB.Add(newUser);

            MessageBox.Show("New user (" + newUser.UserName + ") has been added successfully!");
        }

        public static User IdentifyVoice(Sequence testSequence, List<User> templateDB, bool with_pruning = false, int pruning_width = 0, bool with_beamSearch = false, int beam_width = 0, bool listMode = false)
        {
            Stopwatch totalTime = Stopwatch.StartNew();

            long DTW_ONLY_TIME = 0;

            int testFrameCount = testSequence.Frames.Count();

            User bestUser = new User();
            double bestDistance = double.MaxValue;

            foreach (User trainUser in templateDB)
            {
                foreach (AudioSignal trainSignal in trainUser.UserTemplates)
                {
                    Sequence trainSequence;

                    if (templateSequences.ContainsKey(trainSignal))
                    {
                        trainSequence = templateSequences[trainSignal];
                    }
                    else
                    {
                        templateSequences.Add(trainSignal, null);
                        trainSequence = MFCC.MFCC.ExtractFeatures(trainSignal.data, trainSignal.sampleRate);
                        templateSequences[trainSignal] = trainSequence;
                    }

                    if (testSequence == trainSequence)
                    {
                        bestDistance = 0;
                        bestUser = trainUser;
                        break;
                    }

                    int trainFrameCount = trainSequence.Frames.Count();

                    double distance = 0;

                    Stopwatch stopwatch;
                    

                    if (with_pruning)
                    {
                        stopwatch = Stopwatch.StartNew();
                        distance = SequenceMatching.DTW_Pruning(testSequence, trainSequence, testFrameCount, trainFrameCount, pruning_width);
                    }
                    else if (with_beamSearch)
                    {
                        stopwatch = Stopwatch.StartNew();
                        distance = SequenceMatching.BeamSearch(testSequence, trainSequence, testFrameCount, trainFrameCount, beam_width);
                    }
                    else
                    {
                        stopwatch = Stopwatch.StartNew();
                        distance = SequenceMatching.DTW_NoPruning(testSequence, trainSequence, testFrameCount, trainFrameCount);
                    }
                    
                    stopwatch.Stop();

                    DTW_ONLY_TIME += stopwatch.ElapsedMilliseconds;

                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestUser = trainUser;
                    }

                    if (bestDistance == 0)
                        break;
                }

                if (bestDistance == 0)
                    break;
            }


            totalTime.Stop();

            if (!listMode)
            {
                MessageBox.Show("Matched user: " + bestUser.UserName
                                + "\nDistance: " + bestDistance
                                + "\nDTW_ONLY Time: " + DTW_ONLY_TIME + " ms"
                                + "\nTotal Execution Time: " + totalTime.ElapsedMilliseconds + " ms");
            }

            return bestUser;
        }
    
        public static void IdentifyList(List<User> inputDB, List<User> templateDB, bool with_pruning = false, int pruning_width = 0, bool with_syncSearch = false, int shiftSize = 0)
        {
            Stopwatch totalTime = Stopwatch.StartNew();

            Stopwatch trainTime = Stopwatch.StartNew();
            foreach (User templateUser in templateDB)
            {
                foreach (AudioSignal templateSignal in templateUser.UserTemplates)
                {
                    Sequence templateSequence;

                    if (templateSequences.ContainsKey(templateSignal))
                    {
                        templateSequence = templateSequences[templateSignal];
                    }else
                    {
                        templateSequence = MFCC.MFCC.ExtractFeatures(templateSignal.data, templateSignal.sampleRate);
                        templateSequences[templateSignal] = templateSequence;
                    }
                }
            }
            trainTime.Stop();

            Console.WriteLine("train time: " + ((trainTime.ElapsedMilliseconds / 1000) / 60) + " min");

            int size = 0;
            foreach (User x in inputDB)
            {
                size += x.UserTemplates.Count;
            }

            string[] matchedArray = new string[size];

            Parallel.For(0, inputDB.Count, i =>
            {
                User inputUser = inputDB[i];

                for (int j = 0; j < inputUser.UserTemplates.Count; j++)
                {
                    AudioSignal inputSignal = inputUser.UserTemplates[j];

                    Sequence inputSequence = MFCC.MFCC.ExtractFeatures(inputSignal.data, inputSignal.sampleRate);
                    matchedArray[(i * inputUser.UserTemplates.Count) + j] = IdentifyVoice(inputSequence, templateDB, with_pruning, pruning_width, with_syncSearch, shiftSize, true).UserName;
                }

                Console.WriteLine("Checked " + i++);
            });

            List<string> matchedUsers = new List<string>();

            foreach (string s in matchedArray)
                matchedUsers.Add(s);

            totalTime.Stop();

            double error = TestcaseLoader.CheckTestcaseAccuracy(inputDB, matchedUsers);
            double accuracy = (1 - error) * 100;

            MessageBox.Show("Test Accuracy: " + accuracy + "%" 
                        + "\nExecution Time: " + ((totalTime.ElapsedMilliseconds / 1000) / 60)  + " min");
        }

        #region Bonus Functions

        public static string Time_Sync_Search(AudioSignal signal, List<User> templateDB, bool usePruning, int pruningWidth, bool useBeam, int beamWidth)
        {
            Sequence inputSequence = AudioOperations.ExtractFeatures(signal);

            string bestUser = "";
            double bestDistance = double.MaxValue;

            foreach (User trainUser in templateDB)
            {
                foreach (AudioSignal trainSignal in trainUser.UserTemplates)
                {
                    Sequence trainSequence;

                    if (templateSequences.ContainsKey(trainSignal))
                    {
                        trainSequence = templateSequences[trainSignal];
                    }
                    else
                    {
                        trainSequence = MFCC.MFCC.ExtractFeatures(trainSignal.data, trainSignal.sampleRate);
                        templateSequences[trainSignal] = trainSequence;
                    }

                    int N = inputSequence.Frames.Length;
                    int M = trainSequence.Frames.Length;

                    while (matchLock) { }

                    matchLock = true;
                    double distance;

                    if (usePruning)
                        distance = SequenceMatching.DTW_Pruning(inputSequence, trainSequence, N, M, pruningWidth);
                    if (useBeam)
                        distance = SequenceMatching.BeamSearch(inputSequence, trainSequence, N, M, beamWidth);
                    else
                        distance = SequenceMatching.DTW_NoPruning(inputSequence, trainSequence, N, M);
                    matchLock = false;
                    

                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestUser = trainUser.UserName;
                    }
                }
            }

            return bestUser;
        }

        #endregion
    }
}