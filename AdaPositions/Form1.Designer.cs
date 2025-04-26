namespace AdaPositions {
  partial class Form1 {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      scMain = new SplitContainer();
      tcMain = new TabControl();
      tpSetup = new TabPage();
      btnRemoveStake = new Button();
      label3 = new Label();
      vrStakes = new DataGridView();
      btnAddStake = new Button();
      lbLookup2 = new Label();
      lbLookup1 = new Label();
      btnLookupAddress = new Button();
      label2 = new Label();
      tbAddress = new TextBox();
      btnVerifyApiKey = new Button();
      label1 = new Label();
      tbApiKey = new TextBox();
      tpExplore = new TabPage();
      splitExplore = new SplitContainer();
      cbShowEmptyAddr = new CheckBox();
      tvExplore = new TreeView();
      cmsTvExplore = new ContextMenuStrip(components);
      miResyncAssets = new ToolStripMenuItem();
      miGetAddressesForStake = new ToolStripMenuItem();
      miResyncStake = new ToolStripMenuItem();
      miResync = new ToolStripMenuItem();
      miRemove = new ToolStripMenuItem();
      imageList1 = new ImageList(components);
      tbMain = new TextBox();
      tpLog = new TabPage();
      tbLog = new TextBox();
      lbTrackBarValue = new Label();
      trackBar1 = new TrackBar();
      lbRateTotals = new Label();
      btnProcessStop = new Button();
      timer1 = new System.Windows.Forms.Timer(components);
      stakeAddressesBindingSource = new BindingSource(components);
      ((System.ComponentModel.ISupportInitialize)scMain).BeginInit();
      scMain.Panel1.SuspendLayout();
      scMain.Panel2.SuspendLayout();
      scMain.SuspendLayout();
      tcMain.SuspendLayout();
      tpSetup.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)vrStakes).BeginInit();
      tpExplore.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitExplore).BeginInit();
      splitExplore.Panel1.SuspendLayout();
      splitExplore.Panel2.SuspendLayout();
      splitExplore.SuspendLayout();
      cmsTvExplore.SuspendLayout();
      tpLog.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
      ((System.ComponentModel.ISupportInitialize)stakeAddressesBindingSource).BeginInit();
      SuspendLayout();
      // 
      // scMain
      // 
      scMain.Dock = DockStyle.Fill;
      scMain.Location = new Point(0, 0);
      scMain.Margin = new Padding(3, 4, 3, 4);
      scMain.Name = "scMain";
      scMain.Orientation = Orientation.Horizontal;
      // 
      // scMain.Panel1
      // 
      scMain.Panel1.Controls.Add(tcMain);
      // 
      // scMain.Panel2
      // 
      scMain.Panel2.Controls.Add(lbTrackBarValue);
      scMain.Panel2.Controls.Add(trackBar1);
      scMain.Panel2.Controls.Add(lbRateTotals);
      scMain.Panel2.Controls.Add(btnProcessStop);
      scMain.Size = new Size(816, 835);
      scMain.SplitterDistance = 574;
      scMain.SplitterWidth = 5;
      scMain.TabIndex = 1;
      // 
      // tcMain
      // 
      tcMain.Controls.Add(tpSetup);
      tcMain.Controls.Add(tpExplore);
      tcMain.Controls.Add(tpLog);
      tcMain.Dock = DockStyle.Fill;
      tcMain.Location = new Point(0, 0);
      tcMain.Margin = new Padding(3, 4, 3, 4);
      tcMain.Name = "tcMain";
      tcMain.SelectedIndex = 0;
      tcMain.Size = new Size(816, 574);
      tcMain.TabIndex = 1;
      tcMain.Selecting += tcMain_Selecting;
      // 
      // tpSetup
      // 
      tpSetup.BackColor = SystemColors.ButtonFace;
      tpSetup.Controls.Add(btnRemoveStake);
      tpSetup.Controls.Add(label3);
      tpSetup.Controls.Add(vrStakes);
      tpSetup.Controls.Add(btnAddStake);
      tpSetup.Controls.Add(lbLookup2);
      tpSetup.Controls.Add(lbLookup1);
      tpSetup.Controls.Add(btnLookupAddress);
      tpSetup.Controls.Add(label2);
      tpSetup.Controls.Add(tbAddress);
      tpSetup.Controls.Add(btnVerifyApiKey);
      tpSetup.Controls.Add(label1);
      tpSetup.Controls.Add(tbApiKey);
      tpSetup.Location = new Point(4, 29);
      tpSetup.Margin = new Padding(3, 4, 3, 4);
      tpSetup.Name = "tpSetup";
      tpSetup.Padding = new Padding(3, 4, 3, 4);
      tpSetup.Size = new Size(808, 541);
      tpSetup.TabIndex = 0;
      tpSetup.Text = "Setup";
      tpSetup.Click += tpSetup_Click;
      // 
      // btnRemoveStake
      // 
      btnRemoveStake.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnRemoveStake.Location = new Point(562, 275);
      btnRemoveStake.Margin = new Padding(3, 4, 3, 4);
      btnRemoveStake.Name = "btnRemoveStake";
      btnRemoveStake.Size = new Size(114, 31);
      btnRemoveStake.TabIndex = 11;
      btnRemoveStake.Text = "Remove strOpParam1";
      btnRemoveStake.UseVisualStyleBackColor = true;
      btnRemoveStake.Click += btnRemoveStake_Click;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(13, 280);
      label3.Name = "label3";
      label3.Size = new Size(208, 20);
      label3.TabIndex = 10;
      label3.Text = "Explore strOpParam1 Address:";
      label3.Click += label3_Click;
      // 
      // vrStakes
      // 
      vrStakes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      vrStakes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      vrStakes.Location = new Point(13, 317);
      vrStakes.Margin = new Padding(3, 4, 3, 4);
      vrStakes.Name = "vrStakes";
      vrStakes.RowHeadersWidth = 51;
      vrStakes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      vrStakes.Size = new Size(785, 204);
      vrStakes.TabIndex = 9;
      vrStakes.SelectionChanged += vrStakes_SelectionChanged;
      // 
      // btnAddStake
      // 
      btnAddStake.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnAddStake.Location = new Point(683, 275);
      btnAddStake.Margin = new Padding(3, 4, 3, 4);
      btnAddStake.Name = "btnAddStake";
      btnAddStake.Size = new Size(114, 31);
      btnAddStake.TabIndex = 8;
      btnAddStake.Text = "Add strOpParam1";
      btnAddStake.UseVisualStyleBackColor = true;
      btnAddStake.Click += btnAddStake_Click;
      // 
      // lbLookup2
      // 
      lbLookup2.AutoSize = true;
      lbLookup2.Location = new Point(40, 225);
      lbLookup2.Name = "lbLookup2";
      lbLookup2.Size = new Size(125, 20);
      lbLookup2.TabIndex = 7;
      lbLookup2.Text = "Payment Address:";
      // 
      // lbLookup1
      // 
      lbLookup1.AutoSize = true;
      lbLookup1.Location = new Point(40, 187);
      lbLookup1.Name = "lbLookup1";
      lbLookup1.Size = new Size(125, 20);
      lbLookup1.TabIndex = 6;
      lbLookup1.Text = "Payment Address:";
      // 
      // btnLookupAddress
      // 
      btnLookupAddress.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnLookupAddress.Location = new Point(683, 135);
      btnLookupAddress.Margin = new Padding(3, 4, 3, 4);
      btnLookupAddress.Name = "btnLookupAddress";
      btnLookupAddress.Size = new Size(114, 31);
      btnLookupAddress.TabIndex = 5;
      btnLookupAddress.Text = "Lookup Address";
      btnLookupAddress.UseVisualStyleBackColor = true;
      btnLookupAddress.Click += btnLookupAddress_Click;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(9, 139);
      label2.Name = "label2";
      label2.Size = new Size(125, 20);
      label2.TabIndex = 4;
      label2.Text = "Payment Address:";
      // 
      // tbAddress
      // 
      tbAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      tbAddress.Location = new Point(141, 135);
      tbAddress.Margin = new Padding(3, 4, 3, 4);
      tbAddress.Name = "tbAddress";
      tbAddress.Size = new Size(535, 27);
      tbAddress.TabIndex = 3;
      // 
      // btnVerifyApiKey
      // 
      btnVerifyApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnVerifyApiKey.Location = new Point(695, 17);
      btnVerifyApiKey.Margin = new Padding(3, 4, 3, 4);
      btnVerifyApiKey.Name = "btnVerifyApiKey";
      btnVerifyApiKey.Size = new Size(103, 31);
      btnVerifyApiKey.TabIndex = 2;
      btnVerifyApiKey.Text = "Verify Api Key";
      btnVerifyApiKey.UseVisualStyleBackColor = true;
      btnVerifyApiKey.Click += btnVerifyApiKey_Click;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(13, 24);
      label1.Name = "label1";
      label1.Size = new Size(133, 20);
      label1.TabIndex = 1;
      label1.Text = "Blockfrost Api Key:";
      // 
      // tbApiKey
      // 
      tbApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      tbApiKey.Location = new Point(141, 19);
      tbApiKey.Margin = new Padding(3, 4, 3, 4);
      tbApiKey.Name = "tbApiKey";
      tbApiKey.Size = new Size(547, 27);
      tbApiKey.TabIndex = 0;
      // 
      // tpExplore
      // 
      tpExplore.BackColor = SystemColors.ButtonFace;
      tpExplore.Controls.Add(splitExplore);
      tpExplore.Location = new Point(4, 29);
      tpExplore.Margin = new Padding(3, 4, 3, 4);
      tpExplore.Name = "tpExplore";
      tpExplore.Padding = new Padding(3, 4, 3, 4);
      tpExplore.Size = new Size(808, 541);
      tpExplore.TabIndex = 1;
      tpExplore.Text = "Explore";
      // 
      // splitExplore
      // 
      splitExplore.Dock = DockStyle.Fill;
      splitExplore.Location = new Point(3, 4);
      splitExplore.Margin = new Padding(3, 4, 3, 4);
      splitExplore.Name = "splitExplore";
      // 
      // splitExplore.Panel1
      // 
      splitExplore.Panel1.Controls.Add(cbShowEmptyAddr);
      splitExplore.Panel1.Controls.Add(tvExplore);
      splitExplore.Panel1.Resize += splitExplore_Panel1_Resize;
      // 
      // splitExplore.Panel2
      // 
      splitExplore.Panel2.Controls.Add(tbMain);
      splitExplore.Size = new Size(802, 533);
      splitExplore.SplitterDistance = 431;
      splitExplore.SplitterWidth = 5;
      splitExplore.TabIndex = 0;
      // 
      // cbShowEmptyAddr
      // 
      cbShowEmptyAddr.AutoSize = true;
      cbShowEmptyAddr.Location = new Point(14, 17);
      cbShowEmptyAddr.Margin = new Padding(3, 4, 3, 4);
      cbShowEmptyAddr.Name = "cbShowEmptyAddr";
      cbShowEmptyAddr.Size = new Size(113, 24);
      cbShowEmptyAddr.TabIndex = 1;
      cbShowEmptyAddr.Text = "Show Empty";
      cbShowEmptyAddr.UseVisualStyleBackColor = true;
      cbShowEmptyAddr.CheckedChanged += cbShowEmptyAddr_CheckedChanged;
      // 
      // tvExplore
      // 
      tvExplore.ContextMenuStrip = cmsTvExplore;
      tvExplore.Dock = DockStyle.Bottom;
      tvExplore.ImageIndex = 0;
      tvExplore.ImageList = imageList1;
      tvExplore.Location = new Point(0, 54);
      tvExplore.Margin = new Padding(3, 4, 3, 4);
      tvExplore.Name = "tvExplore";
      tvExplore.SelectedImageIndex = 0;
      tvExplore.Size = new Size(431, 479);
      tvExplore.TabIndex = 0;
      tvExplore.AfterSelect += tvExplore_AfterSelect;
      // 
      // cmsTvExplore
      // 
      cmsTvExplore.ImageScalingSize = new Size(20, 20);
      cmsTvExplore.Items.AddRange(new ToolStripItem[] { miResyncAssets, miGetAddressesForStake, miResyncStake, miResync, miRemove });
      cmsTvExplore.Name = "cmsTvExplore";
      cmsTvExplore.Size = new Size(224, 152);
      cmsTvExplore.Opening += cmsTvExplore_Opening;
      // 
      // miResyncAssets
      // 
      miResyncAssets.Name = "miResyncAssets";
      miResyncAssets.Size = new Size(223, 24);
      miResyncAssets.Text = "Resync Assets";
      miResyncAssets.Click += miResyncAssets_Click;
      // 
      // miGetAddressesForStake
      // 
      miGetAddressesForStake.Name = "miGetAddressesForStake";
      miGetAddressesForStake.Size = new Size(223, 24);
      miGetAddressesForStake.Text = "Get All Addr for Stake";
      miGetAddressesForStake.Click += miGetAddressesForStake_Click;
      // 
      // miResyncStake
      // 
      miResyncStake.Name = "miResyncStake";
      miResyncStake.Size = new Size(223, 24);
      miResyncStake.Text = "Resync Stake";
      miResyncStake.Click += miResyncStake_Click;
      // 
      // miResync
      // 
      miResync.Name = "miResync";
      miResync.Size = new Size(223, 24);
      miResync.Text = "Resync Totals";
      miResync.Click += miResync_Click;
      // 
      // miRemove
      // 
      miRemove.Name = "miRemove";
      miRemove.Size = new Size(223, 24);
      miRemove.Text = "Remove";
      miRemove.Click += miRemove_Click;
      // 
      // imageList1
      // 
      imageList1.ColorDepth = ColorDepth.Depth32Bit;
      imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
      imageList1.TransparentColor = Color.Transparent;
      imageList1.Images.SetKeyName(0, "server-53-16.png");
      imageList1.Images.SetKeyName(1, "folder-457-16.png");
      imageList1.Images.SetKeyName(2, "add-50-16.png");
      imageList1.Images.SetKeyName(3, "stop-62-16.png");
      imageList1.Images.SetKeyName(4, "delete-1203-16.png");
      imageList1.Images.SetKeyName(5, "share-978-16.png");
      imageList1.Images.SetKeyName(6, "label-387-16.png");
      imageList1.Images.SetKeyName(7, "scanning-92-16.png");
      imageList1.Images.SetKeyName(8, "data-331-16.png");
      imageList1.Images.SetKeyName(9, "lookup-61-16.png");
      imageList1.Images.SetKeyName(10, "collection-710-16.png");
      imageList1.Images.SetKeyName(11, "find-228-16.png");
      imageList1.Images.SetKeyName(12, "gift-551-16.png");
      // 
      // tbMain
      // 
      tbMain.AcceptsReturn = true;
      tbMain.AcceptsTab = true;
      tbMain.Dock = DockStyle.Bottom;
      tbMain.Location = new Point(0, 58);
      tbMain.Margin = new Padding(3, 4, 3, 4);
      tbMain.Multiline = true;
      tbMain.Name = "tbMain";
      tbMain.Size = new Size(366, 475);
      tbMain.TabIndex = 0;
      // 
      // tpLog
      // 
      tpLog.Controls.Add(tbLog);
      tpLog.Location = new Point(4, 29);
      tpLog.Margin = new Padding(3, 4, 3, 4);
      tpLog.Name = "tpLog";
      tpLog.Size = new Size(808, 542);
      tpLog.TabIndex = 2;
      tpLog.Text = "Log";
      tpLog.UseVisualStyleBackColor = true;
      // 
      // tbLog
      // 
      tbLog.Dock = DockStyle.Fill;
      tbLog.Location = new Point(0, 0);
      tbLog.Margin = new Padding(3, 4, 3, 4);
      tbLog.Multiline = true;
      tbLog.Name = "tbLog";
      tbLog.Size = new Size(808, 542);
      tbLog.TabIndex = 0;
      // 
      // lbTrackBarValue
      // 
      lbTrackBarValue.AutoSize = true;
      lbTrackBarValue.Location = new Point(97, 89);
      lbTrackBarValue.Name = "lbTrackBarValue";
      lbTrackBarValue.Size = new Size(36, 20);
      lbTrackBarValue.TabIndex = 23;
      lbTrackBarValue.Text = "13.7";
      // 
      // trackBar1
      // 
      trackBar1.Location = new Point(90, 49);
      trackBar1.Margin = new Padding(3, 4, 3, 4);
      trackBar1.Maximum = 80;
      trackBar1.Minimum = 3;
      trackBar1.Name = "trackBar1";
      trackBar1.Size = new Size(305, 56);
      trackBar1.TabIndex = 2;
      trackBar1.TickFrequency = 5;
      trackBar1.Value = 25;
      trackBar1.ValueChanged += trackBar1_ValueChanged;
      // 
      // lbRateTotals
      // 
      lbRateTotals.AutoSize = true;
      lbRateTotals.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbRateTotals.Location = new Point(97, 19);
      lbRateTotals.Name = "lbRateTotals";
      lbRateTotals.Size = new Size(113, 25);
      lbRateTotals.TabIndex = 1;
      lbRateTotals.Text = "lbRateTotals";
      // 
      // btnProcessStop
      // 
      btnProcessStop.BackColor = Color.Red;
      btnProcessStop.FlatStyle = FlatStyle.Flat;
      btnProcessStop.ForeColor = Color.Yellow;
      btnProcessStop.Location = new Point(13, 15);
      btnProcessStop.Margin = new Padding(3, 4, 3, 4);
      btnProcessStop.Name = "btnProcessStop";
      btnProcessStop.Size = new Size(71, 65);
      btnProcessStop.TabIndex = 0;
      btnProcessStop.Text = "Stop";
      btnProcessStop.UseVisualStyleBackColor = false;
      btnProcessStop.Click += btnProcessStop_Click;
      // 
      // timer1
      // 
      timer1.Tick += timer1_Tick;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(816, 835);
      Controls.Add(scMain);
      Name = "Form1";
      Text = "Form1";
      scMain.Panel1.ResumeLayout(false);
      scMain.Panel2.ResumeLayout(false);
      scMain.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)scMain).EndInit();
      scMain.ResumeLayout(false);
      tcMain.ResumeLayout(false);
      tpSetup.ResumeLayout(false);
      tpSetup.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)vrStakes).EndInit();
      tpExplore.ResumeLayout(false);
      splitExplore.Panel1.ResumeLayout(false);
      splitExplore.Panel1.PerformLayout();
      splitExplore.Panel2.ResumeLayout(false);
      splitExplore.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)splitExplore).EndInit();
      splitExplore.ResumeLayout(false);
      cmsTvExplore.ResumeLayout(false);
      tpLog.ResumeLayout(false);
      tpLog.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
      ((System.ComponentModel.ISupportInitialize)stakeAddressesBindingSource).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer scMain;
    private TabControl tcMain;
    private TabPage tpSetup;
    private Button btnVerifyApiKey;
    private Label label1;
    private TextBox tbApiKey;
    private TabPage tpExplore;
    private TabPage tpLog;
    private TextBox tbLog;
    private System.Windows.Forms.Timer timer1;
    private Button btnProcessStop;
    private Button btnLookupAddress;
    private Label label2;
    private TextBox tbAddress;
    private Label lbLookup1;
    private Button btnAddStake;
    private Label lbLookup2;
    private DataGridView vrStakes;
    private BindingSource stakeAddressesBindingSource;
    private Label label3;
    private SplitContainer splitExplore;
    private TreeView tvExplore;
    private ContextMenuStrip cmsTvExplore;
    private ToolStripMenuItem miGetAddressesForStake;
    private ToolStripMenuItem miResyncStake;
    private ImageList imageList1;
    private ToolStripMenuItem miResync;
    private ToolStripMenuItem miRemove;
    private Label lbRateTotals;
    private TrackBar trackBar1;
    private Label lbTrackBarValue;
    private TextBox tbMain;
    private Button btnRemoveStake;
    private CheckBox cbShowEmptyAddr;
    private ToolStripMenuItem miResyncAssets;
  }
}
