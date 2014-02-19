namespace Kwyjibo.UserInterface.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Q : 5 (x3)", 0, 0);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("U : 3");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("QUIGYBO : 10", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Player 1 : 10", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Player 2");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Player 3");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Player = new AForge.Controls.VideoSourcePlayer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourcePropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileOCRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileRegionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileExtractionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boardRegionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oCRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateBoardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flattenLightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.fpsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Output = new System.Windows.Forms.PictureBox();
            this.colourCalibrationGroup = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.modeCombo = new System.Windows.Forms.ComboBox();
            this.hueVal = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.hueTol = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.satVal = new System.Windows.Forms.NumericUpDown();
            this.EnhanceCheck = new System.Windows.Forms.CheckBox();
            this.satTol = new System.Windows.Forms.NumericUpDown();
            this.UpdateBoardCheck = new System.Windows.Forms.CheckBox();
            this.briVal = new System.Windows.Forms.NumericUpDown();
            this.briTol = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Input = new System.Windows.Forms.PictureBox();
            this.Rectified = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.sampleInfoLabel = new System.Windows.Forms.Label();
            this.calibrateNextButton = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.nextButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.gameView = new System.Windows.Forms.PictureBox();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addPlayerButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.newGameButton = new System.Windows.Forms.Button();
            this.gameTree = new System.Windows.Forms.TreeView();
            this.icons = new System.Windows.Forms.ImageList(this.components);
            this.liveView = new System.Windows.Forms.PictureBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Output)).BeginInit();
            this.colourCalibrationGroup.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hueVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueTol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satTol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.briVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.briTol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rectified)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.mainTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // Player
            // 
            this.Player.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Player.BorderColor = System.Drawing.Color.Transparent;
            this.Player.ForeColor = System.Drawing.Color.White;
            this.Player.Location = new System.Drawing.Point(139, 191);
            this.Player.Name = "Player";
            this.Player.Size = new System.Drawing.Size(16, 18);
            this.Player.TabIndex = 13;
            this.Player.VideoSource = null;
            this.Player.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setSourceToolStripMenuItem,
            this.sourcePropertiesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // setSourceToolStripMenuItem
            // 
            this.setSourceToolStripMenuItem.Name = "setSourceToolStripMenuItem";
            this.setSourceToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.setSourceToolStripMenuItem.Text = "Set Source";
            this.setSourceToolStripMenuItem.Click += new System.EventHandler(this.setSourceToolStripMenuItem_Click);
            // 
            // sourcePropertiesToolStripMenuItem
            // 
            this.sourcePropertiesToolStripMenuItem.Name = "sourcePropertiesToolStripMenuItem";
            this.sourcePropertiesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sourcePropertiesToolStripMenuItem.Text = "Source Properties...";
            this.sourcePropertiesToolStripMenuItem.Click += new System.EventHandler(this.sourcePropertiesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tileOCRToolStripMenuItem,
            this.tileRegionsToolStripMenuItem,
            this.tileExtractionsToolStripMenuItem,
            this.boardRegionToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // tileOCRToolStripMenuItem
            // 
            this.tileOCRToolStripMenuItem.CheckOnClick = true;
            this.tileOCRToolStripMenuItem.Name = "tileOCRToolStripMenuItem";
            this.tileOCRToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.tileOCRToolStripMenuItem.Text = "Tile OCR";
            this.tileOCRToolStripMenuItem.Click += new System.EventHandler(this.tileOCRToolStripMenuItem_Click);
            // 
            // tileRegionsToolStripMenuItem
            // 
            this.tileRegionsToolStripMenuItem.Checked = true;
            this.tileRegionsToolStripMenuItem.CheckOnClick = true;
            this.tileRegionsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tileRegionsToolStripMenuItem.Name = "tileRegionsToolStripMenuItem";
            this.tileRegionsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.tileRegionsToolStripMenuItem.Text = "Tile Regions";
            // 
            // tileExtractionsToolStripMenuItem
            // 
            this.tileExtractionsToolStripMenuItem.Checked = true;
            this.tileExtractionsToolStripMenuItem.CheckOnClick = true;
            this.tileExtractionsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tileExtractionsToolStripMenuItem.Name = "tileExtractionsToolStripMenuItem";
            this.tileExtractionsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.tileExtractionsToolStripMenuItem.Text = "Tile Extractions";
            // 
            // boardRegionToolStripMenuItem
            // 
            this.boardRegionToolStripMenuItem.Checked = true;
            this.boardRegionToolStripMenuItem.CheckOnClick = true;
            this.boardRegionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boardRegionToolStripMenuItem.Name = "boardRegionToolStripMenuItem";
            this.boardRegionToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.boardRegionToolStripMenuItem.Text = "Board Region";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oCRToolStripMenuItem,
            this.updateBoardToolStripMenuItem,
            this.flattenLightingToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // oCRToolStripMenuItem
            // 
            this.oCRToolStripMenuItem.CheckOnClick = true;
            this.oCRToolStripMenuItem.Name = "oCRToolStripMenuItem";
            this.oCRToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.oCRToolStripMenuItem.Text = "Real-time OCR";
            this.oCRToolStripMenuItem.Click += new System.EventHandler(this.oCRToolStripMenuItem_Click);
            // 
            // updateBoardToolStripMenuItem
            // 
            this.updateBoardToolStripMenuItem.Checked = true;
            this.updateBoardToolStripMenuItem.CheckOnClick = true;
            this.updateBoardToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.updateBoardToolStripMenuItem.Name = "updateBoardToolStripMenuItem";
            this.updateBoardToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.updateBoardToolStripMenuItem.Text = "Update Board";
            this.updateBoardToolStripMenuItem.Click += new System.EventHandler(this.updateBoardToolStripMenuItem_Click);
            // 
            // flattenLightingToolStripMenuItem
            // 
            this.flattenLightingToolStripMenuItem.CheckOnClick = true;
            this.flattenLightingToolStripMenuItem.Name = "flattenLightingToolStripMenuItem";
            this.flattenLightingToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.flattenLightingToolStripMenuItem.Text = "Flatten Lighting";
            this.flattenLightingToolStripMenuItem.Click += new System.EventHandler(this.flattenLightingToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fpsLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 708);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // fpsLabel
            // 
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(35, 17);
            this.fpsLabel.Text = "0 FPS";
            // 
            // Output
            // 
            this.Output.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Output.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Output.Location = new System.Drawing.Point(3, 3);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(230, 186);
            this.Output.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Output.TabIndex = 23;
            this.Output.TabStop = false;
            // 
            // colourCalibrationGroup
            // 
            this.colourCalibrationGroup.Controls.Add(this.panel1);
            this.colourCalibrationGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colourCalibrationGroup.Location = new System.Drawing.Point(3, 195);
            this.colourCalibrationGroup.Name = "colourCalibrationGroup";
            this.colourCalibrationGroup.Size = new System.Drawing.Size(230, 263);
            this.colourCalibrationGroup.TabIndex = 24;
            this.colourCalibrationGroup.TabStop = false;
            this.colourCalibrationGroup.Text = "Colour Calibration";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.modeCombo);
            this.panel1.Controls.Add(this.Player);
            this.panel1.Controls.Add(this.hueVal);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.hueTol);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.satVal);
            this.panel1.Controls.Add(this.EnhanceCheck);
            this.panel1.Controls.Add(this.satTol);
            this.panel1.Controls.Add(this.UpdateBoardCheck);
            this.panel1.Controls.Add(this.briVal);
            this.panel1.Controls.Add(this.briTol);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 244);
            this.panel1.TabIndex = 30;
            // 
            // modeCombo
            // 
            this.modeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeCombo.FormattingEnabled = true;
            this.modeCombo.Items.AddRange(new object[] {
            "Board (Triple Word Score Colour)",
            "Tile Colour"});
            this.modeCombo.Location = new System.Drawing.Point(14, 15);
            this.modeCombo.Name = "modeCombo";
            this.modeCombo.Size = new System.Drawing.Size(202, 21);
            this.modeCombo.TabIndex = 27;
            this.modeCombo.SelectedIndexChanged += new System.EventHandler(this.modeCombo_SelectedIndexChanged);
            // 
            // hueVal
            // 
            this.hueVal.Location = new System.Drawing.Point(40, 78);
            this.hueVal.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.hueVal.Name = "hueVal";
            this.hueVal.Size = new System.Drawing.Size(55, 20);
            this.hueVal.TabIndex = 1;
            this.hueVal.ValueChanged += new System.EventHandler(this.hueVal_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(38, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Colour";
            // 
            // hueTol
            // 
            this.hueTol.Location = new System.Drawing.Point(139, 78);
            this.hueTol.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.hueTol.Name = "hueTol";
            this.hueTol.Size = new System.Drawing.Size(54, 20);
            this.hueTol.TabIndex = 2;
            this.hueTol.ValueChanged += new System.EventHandler(this.hueTol_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(136, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Tolerance";
            // 
            // satVal
            // 
            this.satVal.DecimalPlaces = 2;
            this.satVal.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.satVal.Location = new System.Drawing.Point(40, 104);
            this.satVal.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.satVal.Name = "satVal";
            this.satVal.Size = new System.Drawing.Size(55, 20);
            this.satVal.TabIndex = 3;
            this.satVal.ValueChanged += new System.EventHandler(this.satVal_ValueChanged);
            // 
            // EnhanceCheck
            // 
            this.EnhanceCheck.AutoSize = true;
            this.EnhanceCheck.Location = new System.Drawing.Point(18, 169);
            this.EnhanceCheck.Name = "EnhanceCheck";
            this.EnhanceCheck.Size = new System.Drawing.Size(98, 17);
            this.EnhanceCheck.TabIndex = 13;
            this.EnhanceCheck.Text = "Flatten Lighting";
            this.EnhanceCheck.UseVisualStyleBackColor = true;
            this.EnhanceCheck.CheckedChanged += new System.EventHandler(this.EnhanceCheck_CheckedChanged);
            // 
            // satTol
            // 
            this.satTol.DecimalPlaces = 2;
            this.satTol.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.satTol.Location = new System.Drawing.Point(139, 104);
            this.satTol.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.satTol.Name = "satTol";
            this.satTol.Size = new System.Drawing.Size(54, 20);
            this.satTol.TabIndex = 4;
            this.satTol.ValueChanged += new System.EventHandler(this.satTol_ValueChanged);
            // 
            // UpdateBoardCheck
            // 
            this.UpdateBoardCheck.AutoSize = true;
            this.UpdateBoardCheck.Location = new System.Drawing.Point(18, 192);
            this.UpdateBoardCheck.Name = "UpdateBoardCheck";
            this.UpdateBoardCheck.Size = new System.Drawing.Size(89, 17);
            this.UpdateBoardCheck.TabIndex = 28;
            this.UpdateBoardCheck.Text = "Detect Board";
            this.UpdateBoardCheck.UseVisualStyleBackColor = true;
            this.UpdateBoardCheck.Visible = false;
            this.UpdateBoardCheck.CheckedChanged += new System.EventHandler(this.UpdateBoardCheck_CheckedChanged);
            // 
            // briVal
            // 
            this.briVal.DecimalPlaces = 2;
            this.briVal.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.briVal.Location = new System.Drawing.Point(40, 130);
            this.briVal.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.briVal.Name = "briVal";
            this.briVal.Size = new System.Drawing.Size(55, 20);
            this.briVal.TabIndex = 5;
            this.briVal.ValueChanged += new System.EventHandler(this.briVal_ValueChanged);
            // 
            // briTol
            // 
            this.briTol.DecimalPlaces = 2;
            this.briTol.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.briTol.Location = new System.Drawing.Point(139, 130);
            this.briTol.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.briTol.Name = "briTol";
            this.briTol.Size = new System.Drawing.Size(54, 20);
            this.briTol.TabIndex = 6;
            this.briTol.ValueChanged += new System.EventHandler(this.briTol_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(103, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "+ / -";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "H";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(103, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "+ / -";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "S";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "+ / -";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "B";
            // 
            // Input
            // 
            this.Input.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Input.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Input.Location = new System.Drawing.Point(0, 0);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(754, 573);
            this.Input.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Input.TabIndex = 25;
            this.Input.TabStop = false;
            // 
            // Rectified
            // 
            this.Rectified.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Rectified.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Rectified.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Rectified.Location = new System.Drawing.Point(0, 0);
            this.Rectified.Name = "Rectified";
            this.Rectified.Size = new System.Drawing.Size(236, 183);
            this.Rectified.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Rectified.TabIndex = 26;
            this.Rectified.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(994, 648);
            this.splitContainer1.SplitterDistance = 601;
            this.splitContainer1.TabIndex = 29;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer8);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(994, 648);
            this.splitContainer2.SplitterDistance = 754;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.tableLayoutPanel3);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.Input);
            this.splitContainer8.Size = new System.Drawing.Size(754, 648);
            this.splitContainer8.SplitterDistance = 71;
            this.splitContainer8.TabIndex = 26;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.18263F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.81737F));
            this.tableLayoutPanel3.Controls.Add(this.sampleInfoLabel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.calibrateNextButton, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(754, 71);
            this.tableLayoutPanel3.TabIndex = 27;
            // 
            // sampleInfoLabel
            // 
            this.sampleInfoLabel.AutoEllipsis = true;
            this.sampleInfoLabel.BackColor = System.Drawing.SystemColors.Info;
            this.sampleInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sampleInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sampleInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sampleInfoLabel.Location = new System.Drawing.Point(3, 10);
            this.sampleInfoLabel.Name = "sampleInfoLabel";
            this.sampleInfoLabel.Padding = new System.Windows.Forms.Padding(8);
            this.sampleInfoLabel.Size = new System.Drawing.Size(591, 51);
            this.sampleInfoLabel.TabIndex = 26;
            this.sampleInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // calibrateNextButton
            // 
            this.calibrateNextButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrateNextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calibrateNextButton.Location = new System.Drawing.Point(602, 15);
            this.calibrateNextButton.Margin = new System.Windows.Forms.Padding(5);
            this.calibrateNextButton.Name = "calibrateNextButton";
            this.calibrateNextButton.Size = new System.Drawing.Size(147, 41);
            this.calibrateNextButton.TabIndex = 26;
            this.calibrateNextButton.Text = "Next";
            this.calibrateNextButton.UseVisualStyleBackColor = true;
            this.calibrateNextButton.Click += new System.EventHandler(this.calibrateNextButton_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.Rectified);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tableLayoutPanel4);
            this.splitContainer3.Size = new System.Drawing.Size(236, 648);
            this.splitContainer3.SplitterDistance = 183;
            this.splitContainer3.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.colourCalibrationGroup, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.Output, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(236, 461);
            this.tableLayoutPanel4.TabIndex = 24;
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.tabPage1);
            this.mainTabs.Controls.Add(this.tabPage2);
            this.mainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabs.Location = new System.Drawing.Point(0, 24);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.Padding = new System.Drawing.Point(10, 5);
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(1008, 684);
            this.mainTabs.TabIndex = 30;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1000, 654);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Calibrate";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer4);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1000, 654);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Game";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer4.Size = new System.Drawing.Size(994, 648);
            this.splitContainer4.SplitterDistance = 717;
            this.splitContainer4.TabIndex = 7;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer5.IsSplitterFixed = true;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer5.Panel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.gameView);
            this.splitContainer5.Size = new System.Drawing.Size(717, 648);
            this.splitContainer5.SplitterDistance = 67;
            this.splitContainer5.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.67442F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.32558F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(717, 47);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.nextButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.editButton, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(521, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(196, 47);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // nextButton
            // 
            this.nextButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.Color.Black;
            this.nextButton.Location = new System.Drawing.Point(103, 5);
            this.nextButton.Margin = new System.Windows.Forms.Padding(5);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(88, 37);
            this.nextButton.TabIndex = 3;
            this.nextButton.Text = "OK";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // editButton
            // 
            this.editButton.BackColor = System.Drawing.Color.Transparent;
            this.editButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editButton.ForeColor = System.Drawing.Color.Black;
            this.editButton.Location = new System.Drawing.Point(15, 10);
            this.editButton.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(68, 27);
            this.editButton.TabIndex = 4;
            this.editButton.Text = "Edit ...";
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.SystemColors.Info;
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.Black;
            this.statusLabel.Location = new System.Drawing.Point(3, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(515, 47);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Awaiting board...";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gameView
            // 
            this.gameView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gameView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gameView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameView.Location = new System.Drawing.Point(0, 0);
            this.gameView.Name = "gameView";
            this.gameView.Size = new System.Drawing.Size(717, 577);
            this.gameView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.gameView.TabIndex = 1;
            this.gameView.TabStop = false;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.splitContainer7);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.liveView);
            this.splitContainer6.Size = new System.Drawing.Size(273, 648);
            this.splitContainer6.SplitterDistance = 376;
            this.splitContainer6.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer7.IsSplitterFixed = true;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.gameTree);
            this.splitContainer7.Size = new System.Drawing.Size(273, 376);
            this.splitContainer7.SplitterDistance = 67;
            this.splitContainer7.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.addPlayerButton);
            this.flowLayoutPanel1.Controls.Add(this.resetButton);
            this.flowLayoutPanel1.Controls.Add(this.newGameButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 15);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(272, 40);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // addPlayerButton
            // 
            this.addPlayerButton.Location = new System.Drawing.Point(239, 3);
            this.addPlayerButton.Name = "addPlayerButton";
            this.addPlayerButton.Size = new System.Drawing.Size(30, 28);
            this.addPlayerButton.TabIndex = 3;
            this.addPlayerButton.Text = "+";
            this.addPlayerButton.UseVisualStyleBackColor = true;
            this.addPlayerButton.Click += new System.EventHandler(this.addPlayerButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(165, 3);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(68, 28);
            this.resetButton.TabIndex = 7;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // newGameButton
            // 
            this.newGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newGameButton.Location = new System.Drawing.Point(71, 3);
            this.newGameButton.Name = "newGameButton";
            this.newGameButton.Size = new System.Drawing.Size(88, 28);
            this.newGameButton.TabIndex = 5;
            this.newGameButton.Text = "New";
            this.newGameButton.UseVisualStyleBackColor = true;
            this.newGameButton.Click += new System.EventHandler(this.newGameButton_Click);
            // 
            // gameTree
            // 
            this.gameTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameTree.ImageIndex = 0;
            this.gameTree.ImageList = this.icons;
            this.gameTree.Location = new System.Drawing.Point(0, 0);
            this.gameTree.Name = "gameTree";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "Node4";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "Q : 5 (x3)";
            treeNode2.Name = "Node9";
            treeNode2.SelectedImageIndex = 0;
            treeNode2.Text = "U : 3";
            treeNode3.Name = "Node3";
            treeNode3.SelectedImageIndex = 0;
            treeNode3.Text = "QUIGYBO : 10";
            treeNode4.ImageIndex = 1;
            treeNode4.Name = "Node0";
            treeNode4.Text = "Player 1 : 10";
            treeNode5.ImageIndex = 1;
            treeNode5.Name = "Node1";
            treeNode5.SelectedImageKey = "user.png";
            treeNode5.Text = "Player 2";
            treeNode6.ImageIndex = 1;
            treeNode6.Name = "Node2";
            treeNode6.Text = "Player 3";
            this.gameTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
            this.gameTree.SelectedImageIndex = 1;
            this.gameTree.Size = new System.Drawing.Size(273, 305);
            this.gameTree.TabIndex = 0;
            // 
            // icons
            // 
            this.icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("icons.ImageStream")));
            this.icons.TransparentColor = System.Drawing.Color.Transparent;
            this.icons.Images.SetKeyName(0, "font.png");
            this.icons.Images.SetKeyName(1, "user.png");
            // 
            // liveView
            // 
            this.liveView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.liveView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.liveView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveView.Location = new System.Drawing.Point(0, 0);
            this.liveView.Name = "liveView";
            this.liveView.Size = new System.Drawing.Size(273, 268);
            this.liveView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.liveView.TabIndex = 4;
            this.liveView.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.mainTabs);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kwyjibo";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Output)).EndInit();
            this.colourCalibrationGroup.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hueVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueTol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satTol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.briVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.briTol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rectified)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.mainTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gameView)).EndInit();
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.liveView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AForge.Controls.VideoSourcePlayer Player;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSourceToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel fpsLabel;
        private System.Windows.Forms.PictureBox Output;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.GroupBox colourCalibrationGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown briTol;
        private System.Windows.Forms.NumericUpDown briVal;
        private System.Windows.Forms.NumericUpDown satTol;
        private System.Windows.Forms.NumericUpDown satVal;
        private System.Windows.Forms.NumericUpDown hueTol;
        private System.Windows.Forms.NumericUpDown hueVal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox Input;
        private System.Windows.Forms.CheckBox EnhanceCheck;
        private System.Windows.Forms.PictureBox Rectified;
        private System.Windows.Forms.ComboBox modeCombo;
        private System.Windows.Forms.CheckBox UpdateBoardCheck;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ToolStripMenuItem tileRegionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileExtractionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boardRegionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileOCRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sourcePropertiesToolStripMenuItem;
        private System.Windows.Forms.TabControl mainTabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView gameTree;
        private System.Windows.Forms.ImageList icons;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.PictureBox gameView;
        private System.Windows.Forms.PictureBox liveView;
        private System.Windows.Forms.Button addPlayerButton;
        private System.Windows.Forms.Button newGameButton;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oCRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateBoardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flattenLightingToolStripMenuItem;
        private System.Windows.Forms.Label sampleInfoLabel;
        private System.Windows.Forms.Button calibrateNextButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

