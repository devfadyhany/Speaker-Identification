using Recorder.MFCC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Recorder
{
    class UserIdentification
    {
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

        //static string trainpath = @"C:\\MyData\\FCIS\\3rd Year\\second term\\analysis and design of algorithms\\Project\\Speaker-Identification\\TEST CASES\\[2] COMPLETE\\Complete SpeakerID Dataset\\TrainingList.txt";
        //public static List<User> training1 = TestcaseLoader.LoadTestcase1Training(trainpath);
        //public static List<User> training2 = TestcaseLoader.LoadTestcase2Training(trainpath);
        //static List<User> training3 = TestcaseLoader.LoadTestcase3Training(trainpath);

        //static string testpath = @"C:\\MyData\\FCIS\\3rd Year\\second term\\analysis and design of algorithms\\Project\\Speaker-Identification\\TEST CASES\\[2] COMPLETE\\Complete SpeakerID Dataset\\TestingList.txt";
        //public static List<User> testing1 = TestcaseLoader.LoadTestcase1Testing(testpath);
        //public static List<User> testing2 = TestcaseLoader.LoadTestcase2Testing(testpath);
        //static List<User> testing2 = TestcaseLoader.LoadTestcase2Testing(testpath);

        public static List<User> DB = new List<User>();
        private static Dictionary<AudioSignal, Sequence> templateSequences = new Dictionary<AudioSignal, Sequence>();


        public static void AddVoice(string name, AudioSignal myaudio)
        {
            foreach (User existUser in DB)
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

            DB.Add(newUser);

            MessageBox.Show("New user (" + newUser.UserName + ") has been added successfully!");
        }

        public static User IdentifyVoice(Sequence testSequence, List<User> templateDB, bool with_pruning = false, int pruning_width = 0, bool with_syncSearch = false, int shiftSize = 0, bool listMode = false)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (templateDB != null)
                DB = templateDB;

            int testFrameCount = testSequence.Frames.Count();

            User bestUser = new User();
            //string bestUser = "";
            double bestDistance = double.MaxValue;

            foreach (User trainUser in DB)
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

                    int trainFrameCount = trainSequence.Frames.Count();

                    double distance = 0;

                    if (with_syncSearch)
                        distance = SequenceMatching.Sync_Search(testSequence, trainSequence, testFrameCount, trainFrameCount, shiftSize);
                    else if (with_pruning)
                        distance = SequenceMatching.DTW_Pruning(testSequence, trainSequence, testFrameCount, trainFrameCount, pruning_width);
                    else
                        distance = SequenceMatching.DTW_NoPruning(testSequence, trainSequence, testFrameCount, trainFrameCount);


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

            stopwatch.Stop();

            if (!listMode)
            {
                MessageBox.Show("Matched user: " + bestUser.UserName
                                + "\nDistance: " + bestDistance
                                + "\nExecution Time: " + stopwatch.ElapsedMilliseconds + " ms");
            }

            return bestUser;
        }
    
        public static void IdentifyList(List<User> inputDB, List<User> templateDB, bool with_pruning = false, int pruning_width = 0, bool with_syncSearch = false, int shiftSize = 0)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<string> matchedUsers = new List<string>();

            int i = 1;

            foreach (User inputUser in inputDB)
            {
                foreach (AudioSignal inputSignal in inputUser.UserTemplates)
                {
                    Sequence inputSequence;

                    if (templateSequences.ContainsKey(inputSignal))
                    {
                        inputSequence = templateSequences[inputSignal];
                    }
                    else
                    {
                        templateSequences.Add(inputSignal, null);
                        inputSequence = MFCC.MFCC.ExtractFeatures(inputSignal.data, inputSignal.sampleRate);
                        templateSequences[inputSignal] = inputSequence;
                    }

                    matchedUsers.Add(IdentifyVoice(inputSequence, templateDB, with_pruning, pruning_width, with_syncSearch, shiftSize, true).UserName);
                }

                Console.WriteLine("Checked " + i++);
            }

            stopwatch.Stop();

            double error = TestcaseLoader.CheckTestcaseAccuracy(inputDB, matchedUsers);

            double accuracy = (1 - error) * 100;

            MessageBox.Show("Test Accuracy: " + accuracy + "%" 
                        + "\nExecution Time: " + stopwatch.ElapsedMilliseconds + " ms");
        }
    }
}