using QuestEditor.Quests;
using System.Linq;
using System.Windows.Forms;

namespace QuestEditor.UI
{
    public partial class MassEXPEditor : Form
    {
        public MassEXPEditor()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, System.EventArgs e)
        {
            if (!double.TryParse(modifierField.Text, out var modifier))
            {
                MessageBox.Show("You must enter a valid number.");
                return;
            }

            if (!Edit())
            {
                return;
            }

            var quests = Main.QuestData.Values.Where(q => q.Rewards.ToList().Exists(r => r.Type == QuestRewardType.QRT_EXP && r.Use != QuestRewardUse.QRU_UNUSE)).ToList();

            for (var i = 0; i < quests.Count; i++)
            {
                var quest = quests[i];
                var expReward = quest.Rewards.FirstOrDefault(r => r.Type == QuestRewardType.QRT_EXP && r.Use != QuestRewardUse.QRU_UNUSE);

                if (expReward == null)
                {
                    continue;
                }

                expReward.Value.EXP = (long) (expReward.Value.EXP * modifier);
            }

            Main.Quests?.RefreshQuest();
            Close();
        }

        private bool Edit()
        {
            var result = MessageBox.Show("Are you sure you want to make this change?\nOnce finished, it cannot be undone.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
    }
}
