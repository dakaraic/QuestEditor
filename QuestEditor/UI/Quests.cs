// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System.Linq;
using System.Windows.Forms;
using QuestEditor.Quests;

namespace QuestEditor.UI
{
    public partial class Quests : UserControl
    {
        public Quest Quest => Main.ActiveQuest;

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
            // Refreshes the current quest.
            questGroup.Text = Main.ActiveQuest?.Name;
            questGroup.Controls.Clear();
            questGroup.Controls.Add(new QuestView(){Dock = DockStyle.Fill});

            // Populate all the options with the current quests's options.

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

            questList.Items.Clear();
            questList.Items.AddRange(quests.OrderBy(q => q.ID).ToArray());
        }

        private void questList_DoubleClick(object sender, System.EventArgs e)
        {
            if (questList.SelectedItem == null)
            {
                return;
            }

            Main.ActiveQuest = (Quest) questList.SelectedItem;
            RefreshQuest();
        }
    }
}
