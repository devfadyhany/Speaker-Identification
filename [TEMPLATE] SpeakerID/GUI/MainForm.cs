using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Accord.Audio;
using Accord.Audio.Formats;
using Accord.DirectSound;
using Accord.Audio.Filters;
using Recorder.Recorder;
using Recorder.MFCC;
using System.Collections.Generic;
using System.ComponentModel;


namespace Recorder
{
    /// <summary>
    ///   Speaker Identification application.
    /// </summary>
    /// 
    public partial class MainForm : Form
    {
        /// <summary>
        /// Data of the opened audio file, contains:
        ///     1. signal data
        ///     2. sample rate
        ///     3. signal length in ms
        /// </summary>
        private AudioSignal signal = null;
        Sequence seq = null;
       
        private string path;

        private Encoder encoder;
        private Decoder decoder;

        private bool isRecorded;


        // ============================ Our Added Variables =============================
        private static List<User> templateData = new List<User>();
        private static List<User> testData;

        private bool usePruning;
        private int pruningWidth;

        private bool useSyncSearch;
        private int shiftSize;

        private bool removeSilence;

        private bool listMode;

        List<double> buffer = new List<double>();
        // ============================ Our Added Variables =============================

        public MainForm()
        {
            InitializeComponent();

            // Configure the wavechart
            chart.SimpleMode = true;
            chart.AddWaveform("wave", Color.Green, 1, false);
            updateButtons();


            // ======================== Our Initilization ==============================
            ResetModes();
            // ======================== Our Initilization ==============================
        }


        #region Helper Functions

        private void UpdateModesStatus(string mode)
        {
            switch (mode)
            {
                case "TSS":
                    if (useSyncSearch)
                    {
                        Label_syncSearch.Text = "True";
                        Label_syncSearch.ForeColor = Color.Green;

                        Label_pruning.Text = "False";
                        Label_pruning.ForeColor = Color.Red;
                        Label_width.Text = "";
                    }
                    else
                    {
                        Label_syncSearch.Text = "False";
                        Label_syncSearch.ForeColor = Color.Red;
                    }
                    break;
                case "pruning":
                    if (usePruning)
                    {
                        Label_pruning.Text = "True";
                        Label_pruning.ForeColor = Color.Green;
                        Label_width.Text = pruningWidth.ToString();

                        Label_syncSearch.Text = "False";
                        Label_syncSearch.ForeColor = Color.Red;
                    }
                    else
                    {
                        Label_pruning.Text = "False";
                        Label_pruning.ForeColor = Color.Red;
                        Label_width.Text = "";
                    }
                    break;
                default:
                    Label_syncSearch.Text = "False";
                    Label_syncSearch.ForeColor = Color.Red;

                    Label_pruning.Text = "False";
                    Label_pruning.ForeColor = Color.Red;
                    Label_width.Text = "";
                    break;
            }
        }

        private void ResetModes()
        {
            usePruning = true;
            pruningWidth = 333;

            useSyncSearch = false;
            shiftSize = 0;

            removeSilence = true;
            listMode = false;

            if (removeSilence)
            {
                Label_RemoveSilence.Text = "True";
                Label_RemoveSilence.ForeColor = Color.Green;
            }
            else
            {
                Label_RemoveSilence.Text = "False";
                Label_RemoveSilence.ForeColor = Color.Red;
            }

            UpdateModesStatus("pruning");
            UpdateModesStatus("TSS");
        }

        private void UpdateDBSize()
        {
            if (templateData == null)
                Label_DBSize.Text = "0";
            else
                Label_DBSize.Text = templateData.Count.ToString();
        }

        #endregion


        #region Recorder

        /// <summary>
        ///   Starts recording audio from the sound card
        /// </summary>
        /// 
        private void btnRecord_Click(object sender, EventArgs e)
        {
            isRecorded = true;
            this.encoder = new Encoder(source_NewFrame, source_AudioSourceError);
            this.encoder.Start();
            updateButtons();
        }

        /// <summary>
        ///   Plays the recorded audio stream.
        /// </summary>
        /// 
        private void btnPlay_Click(object sender, EventArgs e)
        {
            InitializeDecoder();
            // Configure the track bar so the cursor
            // can show the proper current position
            if (trackBar1.Value < this.decoder.frames)
                this.decoder.Seek(trackBar1.Value);
            trackBar1.Maximum = this.decoder.samples;
            this.decoder.Start();
            updateButtons();
        }

        private void InitializeDecoder()
        {
            if (isRecorded)
            {
                // First, we rewind the stream
                this.encoder.stream.Seek(0, SeekOrigin.Begin);
                this.decoder = new Decoder(this.encoder.stream, this.Handle, output_AudioOutputError, output_FramePlayingStarted, output_NewFrameRequested, output_PlayingFinished);
            }
            else
            {
                this.decoder = new Decoder(this.path, this.Handle, output_AudioOutputError, output_FramePlayingStarted, output_NewFrameRequested, output_PlayingFinished);
            }
        }

        /// <summary>
        ///   Stops recording or playing a stream.
        /// </summary>
        /// 
        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();   
            updateButtons();
            updateWaveform(new float[BaseRecorder.FRAME_SIZE], BaseRecorder.FRAME_SIZE);
        }

        /// <summary>
        ///   This callback will be called when there is some error with the audio 
        ///   source. It can be used to route exceptions so they don't compromise 
        ///   the audio processing pipeline.
        /// </summary>
        /// 
        private void source_AudioSourceError(object sender, AudioSourceErrorEventArgs e)
        {
            throw new Exception(e.Description);
        }

        /// <summary>
        ///   This method will be called whenever there is a new input audio frame 
        ///   to be processed. This would be the case for samples arriving at the 
        ///   computer's microphone
        /// </summary>
        /// 
        private void source_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Signal newSignal = eventArgs.Signal;

            this.encoder.addNewFrame(newSignal);
            updateWaveform(this.encoder.current, newSignal.Length);

            //===================== Our Code ===============================
            if (!useSyncSearch)
                return;

            buffer.AddRange(newSignal.ToDouble());

            int requiredSamples = newSignal.SampleRate * 2;
            if (buffer.Count >= requiredSamples)
            {
                AudioSignal newAudioSignal = new AudioSignal();
                newAudioSignal.data = newSignal.ToDouble();
                newAudioSignal.sampleRate = newSignal.SampleRate;

                buffer.Clear();

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, args) =>
                {
                    Console.WriteLine("searching...");
                    string speaker = UserIdentification.Time_Sync_Search(newAudioSignal, templateData);
                    Console.WriteLine("speaker: " + speaker);

                    args.Result = speaker;
                };

                worker.RunWorkerCompleted += (s, args) =>
                {
                    string speaker = args.Result as string;
                    //Label_Speaker.Text = speaker;

                    if (Label_Speaker.InvokeRequired)
                    {
                        Label_Speaker.Invoke(new MethodInvoker(() =>
                        {
                            Label_Speaker.Text = speaker;
                        }));
                    }
                    else
                    {
                        Label_Speaker.Text = speaker;
                    }
                };

                worker.RunWorkerAsync();
            }
            //===================== Our Code ===============================
        }


        /// <summary>
        ///   This event will be triggered as soon as the audio starts playing in the 
        ///   computer speakers. It can be used to update the UI and to notify that soon
        ///   we will be requesting additional frames.
        /// </summary>
        /// 
        private void output_FramePlayingStarted(object sender, PlayFrameEventArgs e)
        {
            updateTrackbar(e.FrameIndex);

            if (e.FrameIndex + e.Count < this.decoder.frames)
            {
                int previous = this.decoder.Position;
                decoder.Seek(e.FrameIndex);

                Signal s = this.decoder.Decode(e.Count);
                decoder.Seek(previous);

                updateWaveform(s.ToFloat(), s.Length);
            }
        }

        /// <summary>
        ///   This event will be triggered when the output device finishes
        ///   playing the audio stream. Again we can use it to update the UI.
        /// </summary>
        /// 
        private void output_PlayingFinished(object sender, EventArgs e)
        {
            updateButtons();
            updateWaveform(new float[BaseRecorder.FRAME_SIZE], BaseRecorder.FRAME_SIZE);
        }

        /// <summary>
        ///   This event is triggered when the sound card needs more samples to be
        ///   played. When this happens, we have to feed it additional frames so it
        ///   can continue playing.
        /// </summary>
        /// 
        private void output_NewFrameRequested(object sender, NewFrameRequestedEventArgs e)
        {
            this.decoder.FillNewFrame(e);
        }


        void output_AudioOutputError(object sender, AudioOutputErrorEventArgs e)
        {
            throw new Exception(e.Description);
        }

        /// <summary>
        ///   Updates the audio display in the wave chart
        /// </summary>
        /// 
        private void updateWaveform(float[] samples, int length)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    chart.UpdateWaveform("wave", samples, length);
                }));
            }
            else
            {
                if (this.encoder != null) { chart.UpdateWaveform("wave", this.encoder.current, length); }
            }
        }

        /// <summary>
        ///   Updates the current position at the trackbar.
        /// </summary>
        /// 
        private void updateTrackbar(int value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    trackBar1.Value = Math.Max(trackBar1.Minimum, Math.Min(trackBar1.Maximum, value));
                }));
            }
            else
            {
                trackBar1.Value = Math.Max(trackBar1.Minimum, Math.Min(trackBar1.Maximum, value));
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (this.encoder != null) { lbLength.Text = String.Format("Length: {0:00.00} sec.", this.encoder.duration / 1000.0); }
        }

        private void Stop()
        {
            if (this.encoder != null) { this.encoder.Stop(); }
            if (this.decoder != null) { this.decoder.Stop(); }
        }


        #endregion


        #region ToolStrip Menu

        #region File Menu

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
                OpenFileDialog open = new OpenFileDialog();

                if (open.ShowDialog() == DialogResult.OK)
                {
                    isRecorded = false;
                    path = open.FileName;
                    //Open the selected audio file
                    signal = AudioOperations.OpenAudioFile(path);

                    if (removeSilence)
                        signal = AudioOperations.RemoveSilence(signal);

                    seq = AudioOperations.ExtractFeatures(signal);
                    for (int i = 0; i < seq.Frames.Length; i++)
                    {
                        for (int j = 0; j < 13; j++)
                        {

                            if (double.IsNaN(seq.Frames[i].Features[j]) || double.IsInfinity(seq.Frames[i].Features[j]))
                                throw new Exception("NaN");
                        }
                    }
                    updateButtons();

                    MessageBox.Show("Input loaded successfully!");
                }
            //}
            //catch (Exception exp)
            //{
            //    Console.WriteLine(exp);
            //    return;
            //}
        }
        
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog(this);
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.encoder != null)
            {
                Stream fileStream = saveFileDialog1.OpenFile();
                this.encoder.Save(fileStream);
            }
        }

        private void clearMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            templateData = new List<User>();
            testData = new List<User>();
            btnIdentify.Enabled = false;
            btnPlay.Enabled = false;
            btnAdd.Enabled = false;
            listMode = false;

            UpdateDBSize();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Edit Menu

        private void loadTrain1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Choose Training Set";
                fileDialog.ShowDialog();

                templateData = TestcaseLoader.LoadTestcase1Training(fileDialog.FileName);

                MessageBox.Show("Training data loaded successfully!");

                OpenFileDialog fileDialog2 = new OpenFileDialog();
                fileDialog2.Title = "Choose Test Set";
                fileDialog2.ShowDialog();

                testData = TestcaseLoader.LoadTestcase1Testing(fileDialog2.FileName);

                MessageBox.Show("Test data loaded successfully!");

                btnIdentify.Enabled = true;
                listMode = true;
            }
            catch(Exception exp)
            {
                return;
            }
        }

        private void loadTestCase2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Choose Training Set";
            fileDialog.ShowDialog();

            templateData = TestcaseLoader.LoadTestcase2Training(fileDialog.FileName);

            MessageBox.Show("Training data loaded successfully!");

            OpenFileDialog fileDialog2 = new OpenFileDialog();
            fileDialog2.Title = "Choose Test Set";
            fileDialog2.ShowDialog();

            testData = TestcaseLoader.LoadTestcase2Testing(fileDialog2.FileName);

            testData = templateData;

            MessageBox.Show("Test data loaded successfully!");

            //UserIdentification.IdentifyList(this.testData, this.templateData, usePruning, pruningWidth, useSyncSearch, shiftSize);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, args) =>
            {
                UserIdentification.IdentifyList(testData, templateData, usePruning, pruningWidth, useSyncSearch, shiftSize);
            };

            worker.RunWorkerCompleted += (s, args) =>
            {
                btnIdentify.Enabled = true;
                UpdateDBSize();
            };

            worker.RunWorkerAsync();
        }

        private void loadTestCase3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Choose Training Set";
            fileDialog.ShowDialog();

            templateData = TestcaseLoader.LoadTestcase3Training(fileDialog.FileName);

            MessageBox.Show("Training data loaded successfully!");

            OpenFileDialog fileDialog2 = new OpenFileDialog();
            fileDialog2.Title = "Choose Test Set";
            fileDialog2.ShowDialog();

            testData = TestcaseLoader.LoadTestcase3Testing(fileDialog2.FileName);

            testData = templateData;

            MessageBox.Show("Test data loaded successfully!");

            //UserIdentification.IdentifyList(this.testData, this.templateData, usePruning, pruningWidth, useSyncSearch, shiftSize);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, args) =>
            {
                UserIdentification.IdentifyList(testData, templateData, usePruning, pruningWidth, useSyncSearch, shiftSize);
            };

            worker.RunWorkerCompleted += (s, args) =>
            {
                btnIdentify.Enabled = true;
                UpdateDBSize();
            };

            worker.RunWorkerAsync();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();

            templateData = TestcaseLoader.LoadSampleTrainingTest(fileDialog.FileName);

            MessageBox.Show("Sample Training data loaded successfully!");

            btnIdentify.Enabled = true;

            UpdateDBSize();
        }

        private void loadSingleTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3|All Files (*.*)|*.*";
                fileDialog.ShowDialog();

                foreach (string path in fileDialog.FileNames)
                {
                    string[] splittedPath = path.Split('\\');
                    string fileName = splittedPath[splittedPath.Length - 1];

                    AudioSignal audio = AudioOperations.OpenAudioFile(path);

                    if (removeSilence)
                        audio = AudioOperations.RemoveSilence(audio);

                    User u1 = new User
                    {
                        UserName = fileName,
                        UserTemplates = new List<AudioSignal> { audio }
                    };

                    templateData.Add(u1);
                }

                MessageBox.Show("Templates loaded successfully!");

                btnIdentify.Enabled = true;
                UpdateDBSize();
            }
            catch (Exception exp)
            {
                return;
            }
        }

        private void loadUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                folderDialog.ShowDialog();

                string selectedFolder = folderDialog.SelectedPath;
                string userName = new DirectoryInfo(selectedFolder).Name;
                string[] audioFiles = Directory.GetFiles(selectedFolder, "*.wav");

                List<AudioSignal> templates = new List<AudioSignal>();

                foreach (string audioPath in audioFiles)
                {
                    AudioSignal audio = AudioOperations.OpenAudioFile(audioPath);
                    if (removeSilence)
                        audio = AudioOperations.RemoveSilence(audio);
                    templates.Add(audio);
                }

                if (templates.Count > 0)
                {
                    User u = new User
                    {
                        UserName = userName,
                        UserTemplates = templates
                    };

                    templateData.Add(u);
                }
            }
            catch (Exception exp)
            {
                return;
            }
            
        }

        #endregion

        #region Modes Menu
        private void togglePruningToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            usePruning = !usePruning;

            UpdateModesStatus("pruning");
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                pruningWidth = Convert.ToInt32(TB_pruningWidth.Text);
            }
            catch (Exception exp)
            {
                return;
            }

            UpdateModesStatus("pruning");
        }

        private void toggleSyncSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useSyncSearch = !useSyncSearch;

            UpdateModesStatus("TSS");
        }

        #endregion

        #endregion


        #region Controls

        private void updateButtons()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(updateButtons));
                return;
            }

            if (this.encoder != null && this.encoder.IsRunning())
            {
                btnAdd.Enabled = false;
                btnIdentify.Enabled = false;
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = false;
            }
            else if (this.decoder != null && this.decoder.IsRunning())
            {
                btnAdd.Enabled = false;
                btnIdentify.Enabled = false;
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = this.path != null || this.encoder != null;
                btnIdentify.Enabled = false;
                btnPlay.Enabled = this.path != null || this.encoder != null;//stream != null;
                btnStop.Enabled = false;
                btnRecord.Enabled = true;
                trackBar1.Enabled = this.decoder != null;
                trackBar1.Value = 0;
            }

            // ======================== Our Code ==============================
            btnIdentify.Enabled = templateData.Count != 0 && btnPlay.Enabled == true;
            // ======================== Our Code ==============================
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog(this);

            string[] splittedString = saveFileDialog1.FileName.Split('\\');

            string name = splittedString[splittedString.Length - 2];

            UserIdentification.AddVoice(name, signal, templateData);

            UpdateDBSize();
        }

        private void btnIdentify_Click(object sender, EventArgs e)
        {
            btnIdentify.Enabled = false;

            Label_Speaker.Text = "Loading...";

            string matchedUser = "";

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, args) =>
            {
                if (listMode)
                {
                    UserIdentification.IdentifyList(testData, templateData, usePruning, pruningWidth, useSyncSearch, shiftSize);
                }
                else
                {
                    User identifiedUser = UserIdentification.IdentifyVoice(seq, templateData, usePruning, pruningWidth, useSyncSearch, shiftSize);
                    matchedUser = identifiedUser.UserName;
                }
            };

            worker.RunWorkerCompleted += (s, args) =>
            {
                Label_Speaker.Text = matchedUser;
                btnIdentify.Enabled = true;
            };

            worker.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            removeSilence = !removeSilence;

            if (removeSilence)
            {
                Label_RemoveSilence.Text = "True";
                Label_RemoveSilence.ForeColor = Color.Green;
            }
            else
            {
                Label_RemoveSilence.Text = "False";
                Label_RemoveSilence.ForeColor = Color.Red;
            }
        }

        #endregion

        private void MainFormFormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
        }
    }
}
