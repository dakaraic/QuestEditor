// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System.Linq;
using System.Windows.Forms;
using QuestEditor.Quests;

namespace QuestEditor.UI
{
    public partial class Quests : UserControl
    {
        public Quest Quest => Main.ActiveQuest;
        public QuestView View;

        public Quests()
        {
            InitializeComponent();
        }

        public void RefreshList()
        {
            questList.Items.Clear();
            questList.Items.AddRange(Main.QuestData.Values.ToList().OrderBy(q => q.ID).ToArray());
        }

        public void RefreshQuest()
        {
            if (Main.ActiveQuest == null)
            {
                questGroup.Text = "";
                View = null;

                questGroup.Controls.Clear();
                return;
            }

            // Refreshes the current quest.
            questGroup.Text = Main.ActiveQuest?.Name;
            questGroup.Controls.Clear();

            View = new QuestView {Dock = DockStyle.Fill};
            questGroup.Controls.Add(View);
        }

        private void searchBox_TextChanged(object sender, System.EventArgs e)
        {
            var search = searchBox.Text;

            if (string.IsNullOrEmpty(search))
            {
                RefreshList();
                return;
            }

            var quests = (from q in Main.QuestData.Values where q.ToString().ToLower().Contains(search.ToLower()) select q).ToArray();

            if (search.StartsWith("/npc"))
            {
                var split = search.Split();

                if (split.Length > 1 && ushort.TryParse(split[1], out var npcID))
                {
                    quests = (from q in Main.QuestData.Values where q.StartCondition.RequiresNPC && q.StartCondition.NPCID == npcID select q).ToArray();
                }
            }

            questList.Items.Clear();
            questList.Items.AddRange(quests.OrderBy(q => q.ID).ToArray());
        }

        private void questList_DoubleClick(object sender, System.EventArgs e)
        {
            Open((Quest) questList.SelectedItem);
        }

        public void Open(Quest quest)
        {
            if (quest == null)
            {
                return;
            }

            View?.Save();

            Main.ActiveQuest = quest;
            RefreshQuest();
        }

        public void SaveView()
        {
            View?.Save();
        }

        public void DeleteCurrent()
        {
            if (questList.SelectedItem == null)
            {
                MessageBox.Show("You must select a quest to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var quest = (Quest) questList.SelectedItem;
            Main.QuestData?.Remove(quest.ID);

            if (quest == Main.ActiveQuest)
            {
                Main.ActiveQuest = null;
            }

            RefreshQuest();
            RefreshList();
        }
    }
}
