namespace QuestEditor.UI
{
    partial class Quests
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.questsGroup = new System.Windows.Forms.GroupBox();
            this.questList = new System.Windows.Forms.ListBox();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.questGroup = new System.Windows.Forms.GroupBox();
            this.questsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // questsGroup
            // 
            this.questsGroup.Controls.Add(this.questList);
            this.questsGroup.Controls.Add(this.searchBox);
            this.questsGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.questsGroup.Location = new System.Drawing.Point(0, 0);
            this.questsGroup.Name = "questsGroup";
            this.questsGroup.Size = new System.Drawing.Size(253, 595);
            this.questsGroup.TabIndex = 0;
            this.questsGroup.TabStop = false;
            this.questsGroup.Text = "Quests";
            // 
            // questList
            // 
            this.questList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.questList.FormattingEnabled = true;
            this.questList.Location = new System.Drawing.Point(3, 42);
            this.questList.Name = "questList";
            this.questList.Size = new System.Drawing.Size(247, 550);
            this.questList.TabIndex = 2;
            this.questList.DoubleClick += new System.EventHandler(this.questList_DoubleClick);
            // 
            // searchBox
            // 
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchBox.Location = new System.Drawing.Point(3, 16);
            this.searchBox.Multiline = true;
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(247, 25);
            this.searchBox.TabIndex = 1;
            this.searchBox.WordWrap = false;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // questGroup
            // 
            this.questGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.questGroup.Location = new System.Drawing.Point(253, 0);
            this.questGroup.Name = "questGroup";
            this.questGroup.Size = new System.Drawing.Size(706, 595);
            this.questGroup.TabIndex = 1;
            this.questGroup.TabStop = false;
            // 
            // Quests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.questGroup);
            this.Controls.Add(this.questsGroup);
            this.Name = "Quests";
            this.Size = new System.Drawing.Size(959, 595);
            this.questsGroup.ResumeLayout(false);
            this.questsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox questsGroup;
        private System.Windows.Forms.GroupBox questGroup;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.ListBox questList;
    }
}
