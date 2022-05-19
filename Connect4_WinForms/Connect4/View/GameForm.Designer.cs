using System;

namespace ELTE.Connect4.View
{
    partial class GameForm
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
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._menuFileLoadGame = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileSaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this._menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this._menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this._menu10TableSize = new System.Windows.Forms.ToolStripMenuItem();
            this._menu20TableSize = new System.Windows.Forms.ToolStripMenuItem();
            this._menu30TableSize = new System.Windows.Forms.ToolStripMenuItem();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._toolLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._toolLabelPlayer1Time = new System.Windows.Forms.ToolStripStatusLabel();
            this._toolLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this._toolLabelPlayer2Time = new System.Windows.Forms.ToolStripStatusLabel();
            this._panelTable = new System.Windows.Forms.Panel();
            this._menuStrip.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menuStrip
            // 
            this._menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuFile,
            this._menuSettings});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Padding = new System.Windows.Forms.Padding(11, 5, 0, 5);
            this._menuStrip.Size = new System.Drawing.Size(730, 29);
            this._menuStrip.TabIndex = 0;
            this._menuStrip.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            this._menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuFileNewGame,
            this.toolStripMenuItem1,
            this._menuFileLoadGame,
            this._menuFileSaveGame,
            this.toolStripMenuItem2,
            this._menuFileExit});
            this._menuFile.Name = "_menuFile";
            this._menuFile.Size = new System.Drawing.Size(37, 19);
            this._menuFile.Text = "Fájl";
            // 
            // _menuFileNewGame
            // 
            this._menuFileNewGame.Name = "_menuFileNewGame";
            this._menuFileNewGame.Size = new System.Drawing.Size(160, 22);
            this._menuFileNewGame.Text = "Új játék";
            this._menuFileNewGame.Click += new System.EventHandler(this.MenuFileNewGame_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // _menuFileLoadGame
            // 
            this._menuFileLoadGame.Name = "_menuFileLoadGame";
            this._menuFileLoadGame.Size = new System.Drawing.Size(160, 22);
            this._menuFileLoadGame.Text = "Játék betöltése...";
            this._menuFileLoadGame.Click += new System.EventHandler(this.MenuFileLoadGame_Click);
            // 
            // _menuFileSaveGame
            // 
            this._menuFileSaveGame.Name = "_menuFileSaveGame";
            this._menuFileSaveGame.Size = new System.Drawing.Size(160, 22);
            this._menuFileSaveGame.Text = "Játék mentése...";
            this._menuFileSaveGame.Click += new System.EventHandler(this.MenuFileSaveGame_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(157, 6);
            // 
            // _menuFileExit
            // 
            this._menuFileExit.Name = "_menuFileExit";
            this._menuFileExit.Size = new System.Drawing.Size(160, 22);
            this._menuFileExit.Text = "Kilépés";
            this._menuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);
            // 
            // _menuSettings
            // 
            this._menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menu10TableSize,
            this._menu20TableSize,
            this._menu30TableSize});
            this._menuSettings.Name = "_menuSettings";
            this._menuSettings.Size = new System.Drawing.Size(75, 19);
            this._menuSettings.Text = "Beállítások";
            // 
            // _menu10TableSize
            // 
            this._menu10TableSize.Name = "_menu10TableSize";
            this._menu10TableSize.Size = new System.Drawing.Size(167, 22);
            this._menu10TableSize.Text = "10x10 tábla méret";
            this._menu10TableSize.Click += new System.EventHandler(this.Menu10TableSize_Click);
            // 
            // _menu20TableSize
            // 
            this._menu20TableSize.Name = "_menu20TableSize";
            this._menu20TableSize.Size = new System.Drawing.Size(167, 22);
            this._menu20TableSize.Text = "20x20 tábla méret";
            this._menu20TableSize.Click += new System.EventHandler(this.Menu20TableSize_Click);
            // 
            // _menu30TableSize
            // 
            this._menu30TableSize.Name = "_menu30TableSize";
            this._menu30TableSize.Size = new System.Drawing.Size(167, 22);
            this._menu30TableSize.Text = "30x30 tábla méret";
            this._menu30TableSize.Click += new System.EventHandler(this.Menu30TableSize_Click);
            // 
            // _openFileDialog
            // 
            this._openFileDialog.Filter = "Potyogós amőba tábla (*.ctl)|*.ctl";
            this._openFileDialog.Title = "Potyogós amőba játék betöltése";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "Potyogós amőba tábla (*.ctl)|*.ctl";
            this._saveFileDialog.Title = "Potyogós amőba játék mentése";
            // 
            // _statusStrip
            // 
            this._statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolLabel1,
            this._toolLabelPlayer1Time,
            this._toolLabel2,
            this._toolLabelPlayer2Time});
            this._statusStrip.Location = new System.Drawing.Point(0, 550);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 25, 0);
            this._statusStrip.Size = new System.Drawing.Size(730, 24);
            this._statusStrip.TabIndex = 1;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _toolLabel1
            // 
            this._toolLabel1.Name = "_toolLabel1";
            this._toolLabel1.Size = new System.Drawing.Size(57, 19);
            this._toolLabel1.Text = "Játékos 1:";
            // 
            // _toolLabelPlayer1Time
            // 
            this._toolLabelPlayer1Time.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._toolLabelPlayer1Time.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this._toolLabelPlayer1Time.Name = "_toolLabelPlayer1Time";
            this._toolLabelPlayer1Time.Size = new System.Drawing.Size(47, 19);
            this._toolLabelPlayer1Time.Text = "0:00:00";
            // 
            // _toolLabel2
            // 
            this._toolLabel2.Name = "_toolLabel2";
            this._toolLabel2.Size = new System.Drawing.Size(57, 19);
            this._toolLabel2.Text = "Játékos 2:";
            // 
            // _toolLabelPlayer2Time
            // 
            this._toolLabelPlayer2Time.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._toolLabelPlayer2Time.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this._toolLabelPlayer2Time.Name = "_toolLabelPlayer2Time";
            this._toolLabelPlayer2Time.Size = new System.Drawing.Size(47, 19);
            this._toolLabelPlayer2Time.Text = "0:00:00";
            // 
            // _panelTable
            // 
            this._panelTable.AllowDrop = true;
            this._panelTable.AutoSize = true;
            this._panelTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._panelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelTable.Location = new System.Drawing.Point(0, 29);
            this._panelTable.Name = "_panelTable";
            this._panelTable.Size = new System.Drawing.Size(730, 521);
            this._panelTable.TabIndex = 2;
            // 
            // GameForm
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(730, 574);
            this.Controls.Add(this._panelTable);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this._menuStrip;
            this.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.Text = "Potyogós amőba játék";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _menuFile;
        private System.Windows.Forms.ToolStripMenuItem _menuFileNewGame;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _menuFileLoadGame;
        private System.Windows.Forms.ToolStripMenuItem _menuFileSaveGame;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem _menuFileExit;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem _menuSettings;
        private System.Windows.Forms.ToolStripMenuItem _menu10TableSize;
        private System.Windows.Forms.ToolStripMenuItem _menu20TableSize;
        private System.Windows.Forms.ToolStripMenuItem _menu30TableSize;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _toolLabel1;
        private System.Windows.Forms.ToolStripStatusLabel _toolLabelPlayer1Time;
        private System.Windows.Forms.ToolStripStatusLabel _toolLabel2;
        private System.Windows.Forms.ToolStripStatusLabel _toolLabelPlayer2Time;
        private System.Windows.Forms.Panel _panelTable;
    }
}

