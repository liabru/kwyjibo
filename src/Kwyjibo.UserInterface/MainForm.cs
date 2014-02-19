/**
 * 
 *  This ugly lookin' code is for controling the front-end GUI interface
 *  The more interesting parts live in the Kwyjibo and Kwyjibo.ML projects! --->
 * 
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Speech.Synthesis;
using System.IO;
using Kwyjibo.ML.Classifiers;
using Kwyjibo.ML.Instances;
using Kwyjibo.Modules;

namespace Kwyjibo.UserInterface.Forms
{
    public enum GameUIState { WaitingCalibration, NewGame, AddedPlayer, WaitingTurn, Scoring }
    public enum CalibrateUIState { PlaceBoard, SetSource, DetectBoard, DetectTiles, Done }

    public partial class MainForm : Form
    {
        private KwyjiboController Controller { get; set; }
        private BackgroundWorker Worker { get; set; }
        private Timer timer;

        private int camFrames, processedFrames;
        private int mode;
        private int tabIndex;
        private bool updatePrefs;
        private bool pauseRealtime;

        private GameUIState gameUiState;
        private CalibrateUIState calibrateUiState;

        private const string basePath = @".\";
        private const string trainingPath = basePath + @"assets\training";
        private const string wordsPath = basePath + @"assets\words\words.txt";
        private const string wordProbPath = basePath + @"assets\words\word_prob.txt";

        public MainForm()
        {
            Controller = new KwyjiboController(trainingPath, wordsPath, wordProbPath);
            Worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            timer = new Timer();

            InitializeComponent();

            // Set up event handlers
            
            Shown += MainForm_Shown;
            FormClosed += MainForm_FormClosed;
            Player.NewFrame += player_NewFrame;
            Input.DoubleClick += Input_DoubleClick;
            Rectified.DoubleClick += Input_DoubleClick;
            gameTree.NodeMouseClick += gameTree_NodeMouseClick;
            gameTree.NodeMouseDoubleClick += gameTree_NodeMouseDoubleClick;
            mainTabs.SelectedIndexChanged += mainTabs_SelectedIndexChanged;
            Worker.DoWork += Worker_DoWork;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Frame counter set up
            camFrames = 0;
            processedFrames = 0;
            timer.Interval = 1000;
            timer.Tick += time_Tick;
            timer.Start();

            // Start webcam device
            Controller.Video.SetDevice(0);
            Player.VideoSource = Controller.Video.Source;
            Controller.Video.Start();

            // Scrabble game set up
            Controller.Game.Players.Add(new Player("Player 1"));
            Controller.Game.Players.Add(new Player("Player 2"));
            Controller.Game.Start();
            gameView.Image = new Bitmap(512, 512);
            modeCombo.SelectedIndex = 0;

            setGameUIState(GameUIState.WaitingCalibration);

            if (Controller.Video.Source == null)
            {
                setCalibrateUIState(CalibrateUIState.SetSource);
            }
            else
            {
                setCalibrateUIState(CalibrateUIState.PlaceBoard);
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var tileDetectionEnabled = tileExtractionsToolStripMenuItem.Checked || tileOCRToolStripMenuItem.Checked || pauseRealtime || gameUiState == GameUIState.WaitingTurn;
            var canPlace = gameUiState != GameUIState.WaitingCalibration && Controller.Game.HasStarted && Controller.NewWords.Count == 0;
            var srcImage = (Bitmap)e.Argument;

            processedFrames += 1;

            Controller.Process(srcImage, tileDetectionEnabled, pauseRealtime, 
                oCRToolStripMenuItem.Checked, tileOCRToolStripMenuItem.Checked, 
                tileRegionsToolStripMenuItem.Checked, tileExtractionsToolStripMenuItem.Checked, canPlace,
                boardRegionToolStripMenuItem.Checked, UpdateBoardCheck.Checked);

            // Update the visual outputs in the interface
            if (tabIndex == 0)
            {
                try { Input.Image = srcImage; } catch { /* sometimes fails :( */ }
                try { Rectified.Image = Controller.BoardDetector.RectifiedBoard; } catch { /* sometimes fails :( */ }

                try
                {
                    if (mode == 0)
                    {
                        Output.Image = Controller.BoardDetector.FilteredBoard;
                    }
                    else if (mode == 1)
                    {
                        Output.Image = Controller.TileDetector.FilteredBoard;
                    }
                }
                catch { /* sometimes fails :( */ }
            }
            else
            {
                try { liveView.Image = Controller.BoardDetector.RectifiedBoard; } catch { /* sometimes fails :( */ }

                try
                {
                    Controller.Game.Board.Draw(Graphics.FromImage(gameView.Image), 0, 0, gameView.Image.Width, gameView.Image.Height);
                    gameView.Invalidate();
                }
                catch { /* sometimes fails :( */ }
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (gameUiState == GameUIState.WaitingTurn)
            {
                DialogResult dg = MessageBox.Show("Ensure all new tiles have been detected. Are you sure you wish to end your turn, accepting any new words that have been placed on the board?",
                                                    "End Turn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.No)
                    return;

                // Disable highlights
                foreach (Player player in Controller.Game.Players)
                    foreach (Word word in player.Words)
                        word.SetHighlight(false);

                // Cancel any background job
                pauseRealtime = true;

                Worker.CancelAsync();
                while (Worker.IsBusy) 
                    System.Threading.Thread.Sleep(1);

                // Perform OCR
                if (!oCRToolStripMenuItem.Checked)
                    Worker_DoWork(this, new DoWorkEventArgs(Player.GetCurrentVideoFrame()));

                pauseRealtime = false;

                // Perform word detection
                Controller.NewWords = Controller.Game.FindNewWords();

                if (Controller.NewWords.Count > 0)
                {
                    setGameUIState(GameUIState.Scoring);

                    foreach (Word word in Controller.NewWords)
                    {
                        // Check word is invalid
                        if (!Controller.Game.ValidWords.Exists(word))
                        {
                            // Perform word correction
                            Result[] correctedWord = Controller.WordClassifier.FindKNearest(new InstanceS(word.ToString()));

                            // Check if correction fits
                            bool skip = false;
                            for (int i = 0; i < word.Cells.Count; i++)
                                if (word.Cells[i].contents.IsPlaced && correctedWord[0].Class[i] != word.Cells[i].contents.Letter[0])
                                    skip = true;

                            if (skip) 
                                continue;

                            for (int i = 0; i < word.Cells.Count; i++)
                                word.Cells[i].contents.Letter = correctedWord[0].Class[i].ToString();

                            word.Corrected = true;
                        }
                    }

                    // Update interface
                    setGameUIState(GameUIState.Scoring);
                    Controller.NewWords[0].SetHighlight(true);

                    try
                    {
                        Controller.Game.Board.Draw(Graphics.FromImage(gameView.Image), 0, 0, gameView.Image.Width, gameView.Image.Height);
                        gameView.Invalidate();
                    }
                    catch { }

                    return;
                }
                else if (Controller.NewWords.Count == 0)
                {
                    DialogResult res = MessageBox.Show("There were no new words detected. Are you sure you wish to pass?", "No Words Detected",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.Yes)
                    {
                        Controller.Game.EndTurn();
                        setGameUIState(GameUIState.WaitingTurn);
                    }

                    return;
                }
            }

            if (Controller.NewWords.Count > 0)
            {
                Controller.NewWords[0].SetPlaced(true);
                Controller.NewWords[0].SetHighlight(false);
                Controller.NewWords[0].Player.Words.Add(Controller.NewWords[0]);
                Controller.NewWords.RemoveAt(0);

                if (Controller.NewWords.Count > 0)
                {
                    setGameUIState(GameUIState.Scoring);
                    Controller.NewWords[0].SetHighlight(true);
                    try
                    {
                        Controller.Game.Board.Draw(Graphics.FromImage(gameView.Image), 0, 0, gameView.Image.Width, gameView.Image.Height);
                        gameView.Invalidate();
                    }
                    catch { }
                }
                else
                {
                    Controller.Game.EndTurn();
                    setGameUIState(GameUIState.WaitingTurn);
                }
            }
            else
            {
                Controller.Game.EndTurn();
                setGameUIState(GameUIState.WaitingTurn);
            }
        }

        private void editWord(Word word, bool disallowPlaced)
        {
            string newWord = InputDialog.Dialog("Enter the correct word.", "Edit Word", word.ToString());

            newWord = newWord.ToUpper();

            if (newWord.Length != word.ToString().Length)
            {
                MessageBox.Show("The corrected word must be the same length.", "New Word Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Controller.Game.ValidWords.Exists(newWord))
            {
                MessageBox.Show("The word '" + newWord + "' is not in the dictionary!", "New Word Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (disallowPlaced)
            {
                for (int i = 0; i < newWord.Length; i++)
                {
                    if (word.Cells[i].contents.IsPlaced &&
                        newWord[i] != word.Cells[i].contents.Letter[0])
                    {
                        MessageBox.Show("You can not change any letters that have been previously placed!", "New Word Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            for (int i = 0; i < newWord.Length; i++)
                word.Cells[i].contents.Letter = "" + newWord[i];

            word.Corrected = true;

            setGameUIState(gameUiState);
        }

        private void gameTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var tag = e.Node.Tag;

            if (tag is Word)
                editWord((Word)tag, false);
        }

        private void gameTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (gameUiState == GameUIState.Scoring) return;

            var tag = e.Node.Tag;

            foreach (Player player in Controller.Game.Players)
                foreach (Word word in player.Words)
                    word.SetHighlight(false);

            if (tag is Player)
            {
                foreach (Word word in ((Player)tag).Words)
                    word.SetHighlight(true);
            }

            if (tag is Word)
                ((Word)tag).SetHighlight(true);

            if (tag is Tile)
                ((Tile)tag).IsHighlight = true;
        }

        private void mainTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabIndex = mainTabs.SelectedIndex;

            if (mainTabs.SelectedIndex == 1)
            {
                boardRegionToolStripMenuItem.Checked = false;
                tileExtractionsToolStripMenuItem.Checked = true;
                tileOCRToolStripMenuItem.Checked = false;
                tileRegionsToolStripMenuItem.Checked = false;
            }

            if (mainTabs.SelectedIndex == 0)
            {
                boardRegionToolStripMenuItem.Checked = true;
                tileRegionsToolStripMenuItem.Checked = true;
            }
        }

        private void Input_DoubleClick(object sender, EventArgs e)
        {
            if (calibrateUiState != CalibrateUIState.DetectBoard && calibrateUiState != CalibrateUIState.DetectTiles)
                return;

            MouseEventArgs args = e as MouseEventArgs;
            Bitmap scaledImage = new Bitmap(Input.ClientSize.Width, Input.ClientSize.Height);

            Input.DrawToBitmap(scaledImage, Input.ClientRectangle);

            var sample = ImageFilters.SampleHSBAverage(scaledImage, args.X, args.Y);

            hueVal.Value = sample.Hue;
            satVal.Value = sample.Saturation;
            briVal.Value = sample.Brightness;

            // board calibration
            if (modeCombo.SelectedIndex == 0)
            {
                hueTol.Value = 10;
                satTol.Value = 0.2m;
                briTol.Value = 0.2m;
                UpdateBoardCheck.Checked = true;
            }

            // tiles calibration
            if (modeCombo.SelectedIndex == 1)
            {
                // offset as tile hue is too close to triple word score
                hueVal.Value += 10;

                hueTol.Value = 30;
                satTol.Value = 1m;
                briTol.Value = 1m;
                UpdateBoardCheck.Checked = false;
            }

            updateDetector();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Controller.Video.Stop();
        }

        private void savePrefs(string prefix)
        {
            Properties.Settings.Default["hueVal" + prefix] = hueVal.Value;
            Properties.Settings.Default["satVal" + prefix] = satVal.Value;
            Properties.Settings.Default["briVal" + prefix] = briVal.Value;
            Properties.Settings.Default["hueTol" + prefix] = hueTol.Value;
            Properties.Settings.Default["satTol" + prefix] = satTol.Value;
            Properties.Settings.Default["briTol" + prefix] = briTol.Value;
            Properties.Settings.Default.Save();
        }

        private void loadPrefs(string prefix)
        {
            updatePrefs = false;

            hueVal.Value = (Decimal)Properties.Settings.Default["hueVal" + prefix];
            satVal.Value = (Decimal)Properties.Settings.Default["satVal" + prefix];
            briVal.Value = (Decimal)Properties.Settings.Default["briVal" + prefix];
            hueTol.Value = (Decimal)Properties.Settings.Default["hueTol" + prefix];
            satTol.Value = (Decimal)Properties.Settings.Default["satTol" + prefix];
            briTol.Value = (Decimal)Properties.Settings.Default["briTol" + prefix];

            updatePrefs = true;
        }

        private void time_Tick(object sender, EventArgs e)
        {
            fpsLabel.Text = "I: " + (camFrames * (timer.Interval / 1000)) + " FPS   O: " + (processedFrames * (timer.Interval / 1000)) + " FPS";
            camFrames = 0;
            processedFrames = 0;
        }

        private void player_NewFrame(object sender, ref Bitmap image)
        {
            camFrames += 1;

            if (!Worker.IsBusy && !pauseRealtime && gameUiState != GameUIState.Scoring) 
                Worker.RunWorkerAsync(image.Clone());
        }

        private void setSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VideoSourceForm source = new VideoSourceForm(this);
            source.Visible = true;
            source.Focus();
        }

        public void setVideo(VideoInput vi)
        {
            Controller.Video = vi;
            Player.VideoSource = Controller.Video.Source;
            Controller.Video.Start();
        }

        private void updateDetector()
        {
            if (!updatePrefs) 
                return;

            Controller.BoardDetector.FlattenLighting = EnhanceCheck.Checked;
            Controller.TileOcr.Recognise = oCRToolStripMenuItem.Checked;

            if (modeCombo.SelectedIndex == 0)
            {
                savePrefs("Board");
                Controller.BoardDetector.SetColour((float)hueVal.Value, (float)satVal.Value, (float)briVal.Value);
                Controller.BoardDetector.SetTolerance((float)hueTol.Value, (float)satTol.Value, (float)briTol.Value);
            }

            if (modeCombo.SelectedIndex == 1)
            {
                savePrefs("Tile");
                Controller.TileDetector.SetColour((float)hueVal.Value, (float)satVal.Value, (float)briVal.Value);
                Controller.TileDetector.SetTolerance((float)hueTol.Value, (float)satTol.Value, (float)briTol.Value);
            }
        }

        private void hueVal_ValueChanged(object sender, EventArgs e) { updateDetector(); }
        private void hueTol_ValueChanged(object sender, EventArgs e) { updateDetector(); }
        private void satVal_ValueChanged(object sender, EventArgs e) { updateDetector(); }
        private void satTol_ValueChanged(object sender, EventArgs e) { updateDetector(); }
        private void briVal_ValueChanged(object sender, EventArgs e) { updateDetector(); }
        private void briTol_ValueChanged(object sender, EventArgs e) { updateDetector(); }

        private void EnhanceCheck_CheckedChanged(object sender, EventArgs e)
        {
            updateDetector();
            flattenLightingToolStripMenuItem.Checked = EnhanceCheck.Checked;
        }

        private void modeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setMode(modeCombo.SelectedIndex);
            updateDetector();
        }

        public void setMode(int newMode)
        {
            mode = newMode;

            switch (mode)
            {
                case 0:
                    loadPrefs("Board");
                    setCalibrateUIState(CalibrateUIState.DetectBoard);
                break;
                case 1:
                    loadPrefs("Tile");
                    setCalibrateUIState(CalibrateUIState.DetectTiles);
                break;
            }
        }

        private void sourcePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Video.DisplayPropertyPage();
        }

        private void updateGameTree()
        {
            gameTree.Nodes.Clear();

            foreach (Player player in Controller.Game.Players)
            {
                TreeNode node = new TreeNode(player.Name + " : " + player.Points, 1, 1);
                node.Tag = player;

                foreach (Word word in player.Words)
                {
                    TreeNode wn;

                    if (word.Corrected)
                    {
                        wn = new TreeNode(word + " * : " + word.Points, 0, 0);
                    }
                    else
                    {
                        wn = new TreeNode(word + " : " + word.Points, 0, 0);
                    }

                    wn.Tag = word;

                    foreach (Cell cell in word.Cells)
                    {
                        TreeNode cn = new TreeNode(cell.Contents.Letter + " : " + cell.Contents.Points + " : " + cell.ToString(), 0, 0);
                        cn.Tag = cell.Contents;

                        wn.Nodes.Add(cn);
                    }

                    node.Nodes.Add(wn);
                }

                node.Expand();
                gameTree.Nodes.Add(node);
            }
        }

        public void setCalibrateUIState(CalibrateUIState newState)
        {
            calibrateUiState = newState;

            switch (newState)
            {
                case CalibrateUIState.SetSource:
                    UpdateBoardCheck.Checked = false;
                    colourCalibrationGroup.Enabled = false;
                    tileExtractionsToolStripMenuItem.Checked = false;
                    tileRegionsToolStripMenuItem.Checked = false;
                    oCRToolStripMenuItem.Checked = false;
                    boardRegionToolStripMenuItem.Checked = false;
                    sampleInfoLabel.Text = "Set Controller.Video input source (e.g. webcam)";
                    modeCombo.SelectedIndex = 0;
                    calibrateNextButton.Text = "Next";
                    setSourceToolStripMenuItem_Click(null, null);
                    break;

                case CalibrateUIState.PlaceBoard:
                    UpdateBoardCheck.Checked = false;
                    colourCalibrationGroup.Enabled = false;
                    tileExtractionsToolStripMenuItem.Checked = false;
                    tileRegionsToolStripMenuItem.Checked = false;
                    oCRToolStripMenuItem.Checked = false;
                    boardRegionToolStripMenuItem.Checked = false;
                    sampleInfoLabel.Text = "Place board in view as tight and as level as possible";
                    calibrateNextButton.Text = "Next";
                    modeCombo.SelectedIndex = 0;
                    break;

                case CalibrateUIState.DetectBoard:
                    UpdateBoardCheck.Checked = true;
                    colourCalibrationGroup.Enabled = true;
                    tileExtractionsToolStripMenuItem.Checked = false;
                    tileRegionsToolStripMenuItem.Checked = false;
                    oCRToolStripMenuItem.Checked = false;
                    boardRegionToolStripMenuItem.Checked = true;
                    sampleInfoLabel.Text = "Double click on the red 'triple word score' to sample colour";
                    calibrateNextButton.Text = "Next";
                    modeCombo.SelectedIndex = 0;
                    break;

                case CalibrateUIState.DetectTiles:
                    UpdateBoardCheck.Checked = false;
                    colourCalibrationGroup.Enabled = true;
                    tileExtractionsToolStripMenuItem.Checked = true;
                    tileRegionsToolStripMenuItem.Checked = true;
                    oCRToolStripMenuItem.Checked = false;
                    boardRegionToolStripMenuItem.Checked = false;
                    sampleInfoLabel.Text = "Double click on a letter tile background to sample colour";
                    calibrateNextButton.Text = "Next";
                    modeCombo.SelectedIndex = 1;
                    break;

                case CalibrateUIState.Done:
                    UpdateBoardCheck.Checked = false;
                    colourCalibrationGroup.Enabled = true;
                    tileExtractionsToolStripMenuItem.Checked = false;
                    tileRegionsToolStripMenuItem.Checked = false;
                    oCRToolStripMenuItem.Checked = false;
                    boardRegionToolStripMenuItem.Checked = false;
                    sampleInfoLabel.Text = "Calibration complete";
                    calibrateNextButton.Text = "Recalibrate";
                    modeCombo.SelectedIndex = 1;

                    if (gameUiState == GameUIState.WaitingCalibration)
                        setGameUIState(GameUIState.WaitingTurn);

                    mainTabs.SelectTab(1);

                    break;
            }
        }

        private CalibrateUIState getNextCalibrateUIState(CalibrateUIState currentState)
        {
            switch (currentState)
            {
                case CalibrateUIState.SetSource:
                    return CalibrateUIState.PlaceBoard;
                case CalibrateUIState.PlaceBoard:
                    return CalibrateUIState.DetectBoard;
                case CalibrateUIState.DetectBoard:
                    return CalibrateUIState.DetectTiles;
                case CalibrateUIState.DetectTiles:
                    return CalibrateUIState.Done;
                case CalibrateUIState.Done:
                    return CalibrateUIState.DetectBoard;
            }

            return CalibrateUIState.Done;
        }

        private void setGameUIState(GameUIState newState)
        {
            gameUiState = newState;

            switch (newState)
            {
                case GameUIState.WaitingCalibration:
                    addPlayerButton.Enabled = false;
                    resetButton.Enabled = false;
                    nextButton.Enabled = false;
                    editButton.Enabled = false;
                    newGameButton.Enabled = false;
                    newGameButton.Text = "New";
                    statusLabel.Text = "Calibration required";
                    break;

                case GameUIState.NewGame:
                    addPlayerButton.Enabled = true;
                    resetButton.Enabled = false;
                    nextButton.Enabled = false;
                    editButton.Enabled = false;
                    newGameButton.Enabled = true;
                    newGameButton.Text = "Start";
                    statusLabel.Text = "Add players or press start";
                    break;

                case GameUIState.AddedPlayer:
                    addPlayerButton.Enabled = true;
                    resetButton.Enabled = false;
                    nextButton.Enabled = false;
                    editButton.Enabled = false;
                    newGameButton.Enabled = true;
                    newGameButton.Text = "Start";
                    statusLabel.Text = "Added '" + Controller.Game.Players[Controller.Game.Players.Count-1].Name + "'  -  Add more or start...";
                    break;

                case GameUIState.WaitingTurn:
                    addPlayerButton.Enabled = false;
                    resetButton.Enabled = true;
                    nextButton.Enabled = true;
                    editButton.Enabled = false;
                    newGameButton.Enabled = true;
                    newGameButton.Text = "New";
                    statusLabel.Text = Controller.Game.CurrentPlayer.Name + "'s turn to play...";
                    break;

                case GameUIState.Scoring:
                    addPlayerButton.Enabled = false;
                    resetButton.Enabled = true;
                    nextButton.Enabled = true;
                    editButton.Enabled = true;
                    newGameButton.Enabled = true;
                    newGameButton.Text = "New";

                    if (Controller.NewWords[0].Corrected)
                    {
                        statusLabel.Text = Controller.NewWords[0] + " scores " + Controller.NewWords[0].Points + "! (*)";
                    }
                    else
                    {
                        statusLabel.Text = Controller.NewWords[0] + " scores " + Controller.NewWords[0].Points + "!";
                    }

                    break;
            }

            updateGameTree();

            try
            {
                Controller.Game.Board.Draw(Graphics.FromImage(gameView.Image), 0, 0, gameView.Image.Width, gameView.Image.Height);
                gameView.Invalidate();
            }
            catch { }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            if (newGameButton.Text == "New")
            {
                Controller.Game.New();
                setGameUIState(GameUIState.NewGame);
            }
            else if (newGameButton.Text == "Start")
            {
                Controller.Game.Start();
                setGameUIState(GameUIState.WaitingTurn);
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Controller.Game.Start();
            setGameUIState(GameUIState.WaitingTurn);
        }

        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            Player p = new Player(InputDialog.Dialog("Enter the player name.", "Player Name", "Player " + (Controller.Game.Players.Count + 1)));
            Controller.Game.Players.Add(p);
            setGameUIState(GameUIState.AddedPlayer);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (gameUiState == GameUIState.Scoring)
            {
                Controller.NewWords[0].SetHighlight(true);
                editWord(Controller.NewWords[0], true);
            }
        }

        private void updateBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateBoardCheck.Checked = updateBoardToolStripMenuItem.Checked;
        }

        private void flattenLightingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnhanceCheck.Checked = flattenLightingToolStripMenuItem.Checked;
        }

        private void UpdateBoardCheck_CheckedChanged(object sender, EventArgs e)
        {
            updateBoardToolStripMenuItem.Checked = UpdateBoardCheck.Checked;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED. Prevents flickering.
                cp.ExStyle |= 0x00080000; // WS_EX_LAYERED. Transparency key.
                return cp;
            }
        }

        private void calibrateNextButton_Click(object sender, EventArgs e)
        {
            setCalibrateUIState(getNextCalibrateUIState(calibrateUiState));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://brm.io/kwyjibo");
        }

        private void tileOCRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tileOCRToolStripMenuItem.Checked)
            {
                tileRegionsToolStripMenuItem.Checked = false;
                tileExtractionsToolStripMenuItem.Checked = false;
                oCRToolStripMenuItem.Checked = true;
            }
            else
            {
                tileRegionsToolStripMenuItem.Checked = true;
                tileExtractionsToolStripMenuItem.Checked = true;
                oCRToolStripMenuItem.Checked = false;
            }
        }

        private void oCRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oCRToolStripMenuItem.Checked)
            {
                tileRegionsToolStripMenuItem.Checked = false;
                tileExtractionsToolStripMenuItem.Checked = false;
                tileOCRToolStripMenuItem.Checked = true;
            }
            else
            {
                tileRegionsToolStripMenuItem.Checked = true;
                tileExtractionsToolStripMenuItem.Checked = true;
                tileOCRToolStripMenuItem.Checked = false;
            }
        }
    }
}
