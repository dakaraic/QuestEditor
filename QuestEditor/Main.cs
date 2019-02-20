// Copyright 2019 RED Software, LLC. All Rights Reserved.

using QuestEditor.IO;
using QuestEditor.Quests;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace QuestEditor
{
    public partial class Main : Form
    {
        public static Quest ActiveQuest;

        public static Dictionary<uint, string> QuestDialog;
        public static Dictionary<ushort, Quest> QuestData;

        private UI.Quests quests;

        public Main()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Open QuestData.shn";
                dialog.Filter = "QuestData.shn|QuestData.shn";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Failed to open the file Dialog.");
                    return;
                }

                ParseQuestDialog(dialog.FileName);
                ParseQuestData(dialog.FileName);

                quests = new UI.Quests { Dock = DockStyle.Fill };

                inactiveLogo.Visible = false;
                saveToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = true;

                mainPanel.Controls.Add(quests);
                quests.RefreshList();
            }
        }

        private void ParseQuestDialog(string fileName)
        {
            var path = fileName.Replace("QuestData", "QuestDialog");

            if (!File.Exists(path))
            {
                MessageBox.Show("Failed to open corresponding QuestDialog.shn.\nYou can still edit quests, just without the dialogs.");
                return;
            }

            QuestDialog = new Dictionary<uint, string>();

            using (var file = new SHNFile(path))
            using (var reader = new DataTableReader(file.Table))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestDialog.Remove((uint) reader.GetValue(0));
                        QuestDialog.Add((uint) reader.GetValue(0), reader.GetString(1));
                    }
                }
            }
        }

        private void ParseQuestData(string fileName)
        {
            QuestData = new Dictionary<ushort, Quest>();

            using (var stream = new MemoryStream(File.ReadAllBytes(fileName)))
            using (var reader = new System.IO.BinaryReader(stream))
            {
                reader.ReadUInt16(); // header
                var count = reader.ReadInt16();

                for (var i = 0; i < count; i++)
                {
                    var size = reader.ReadInt32();
                    var data = reader.ReadBytes(size - 4);

                    var parsed = Quest.Parse(data);
                    if (parsed == null)
                    {
                        MessageBox.Show("Failed to parse QuestData.shn");
                        return;
                    }

                    QuestData.Add(parsed.ID, parsed);
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (quests == null)
            {
                return;
            }

            QuestData = null;
            QuestDialog = null;

            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;

            mainPanel.Controls.Remove(quests);
            inactiveLogo.Visible = true;
        }
    }
}
