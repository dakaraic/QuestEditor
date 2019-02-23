// Copyright 2019 RED Software, LLC. All Rights Reserved.

using QuestEditor.Quests;
using System;
using System.Windows.Forms;

namespace QuestEditor.UI
{
    public partial class QuestView : UserControl
    {
        private static Quest quest => Main.ActiveQuest;

        public QuestView()
        {
            InitializeComponent();
        }

        private void QuestView_Load(object sender, EventArgs e)
        {
            PopulateView();
        }

        private void PopulateView()
        {
            #region Add Grid Rows

            for (var i = 0; i < 5; i++)
            {
                gridMobs.Rows.Add();
            }

            for (var i = 0; i < 5; i++)
            {
                gridItems.Rows.Add();
            }

            for (var i = 0; i < 10; i++)
            {
                gridActions.Rows.Add();
            }

            for (var i = 0; i < 12; i++)
            {
                gridRewards.Rows.Add();
            }

            #endregion

            #region Add Enumeration Options

            for (var i = 0; i < (int) QuestType.QT_MAX_QUEST_TYPE; i++)
            {
                cmbType.Items.Add((QuestType) i);
            }

            for (var i = 0; i < (int) DailyQuestType.DQT_MAX; i++)
            {
                cmbDailyType.Items.Add((DailyQuestType) i);
            }

            for (var i = 0; i < (int) UseClassType.MAX_USECLASSTYPE; i++)
            {
                cmbClass.Items.Add((UseClassType) i);
                cmbEndClass.Items.Add((UseClassType) i);
            }

            for (var i = 0; i < (int) Gender.Max; i++)
            {
                cmbGender.Items.Add((Gender) i);
            }

            for (var i = 0; i < (int) QuestStartDateMode.QSDM_MAX; i++)
            {
                cmbDateMode.Items.Add((QuestStartDateMode) i);
            }

            for (var i = 0; i < (int) QuestNPCMobAction.QNMA_MAX; i++)
            {
                Action.Items.Add((QuestNPCMobAction) i);
            }

            for (var i = 0; i < (int) QuestActionIfType.QUEST_ACTION_IF_MAX; i++)
            {
                actionIfType.Items.Add((QuestActionIfType) i);
            }

            for (var i = 0; i < (int) QuestActionThenType.QUEST_ACTION_THEN_MAX; i++)
            {
                actionThenType.Items.Add((QuestActionThenType) i);
            }

            for (var i = 0; i < (int) QuestRewardUse.QRU_MAX; i++)
            {
                rewardUse.Items.Add((QuestRewardUse) i);
            }

            for (var i = 0; i < (int) QuestRewardType.QRT_MAX; i++)
            {
                rewardType.Items.Add((QuestRewardType) i);
            }

            #endregion

            if (quest == null)
            {
                return;
            }

            #region General

            txtID.Text = quest.ID.ToString();
            txtName.Text = quest.NameID.ToString();
            txtBrief.Text = quest.BriefID.ToString();
            txtRegion.Text = quest.Region.ToString();
            cmbType.SelectedIndex = (int) quest.Type;
            chkRepeat.Checked = quest.IsRepeatable;
            cmbDailyType.SelectedIndex = (int) quest.DailyQuestType;

            #endregion

            #region Start Conditions

            chkWaitlist.Checked = quest.StartCondition.IsWaitListView;
            chkWaitlistProgress.Checked = quest.StartCondition.IsWaitListProgress;
            chkLevel.Checked = quest.StartCondition.RequiresLevel;
            txtMinLevel.Text = quest.StartCondition.MinLevel.ToString();
            txtMaxLevel.Text = quest.StartCondition.MaxLevel.ToString();
            chkNPC.Checked = quest.StartCondition.RequiresNPC;
            txtNPC.Text = quest.StartCondition.NPCID.ToString();
            chkItem.Checked = quest.StartCondition.RequiresItem;
            txtItem.Text = quest.StartCondition.ItemID.ToString();
            txtItemLot.Text = quest.StartCondition.ItemLot.ToString();
            chkLocation.Checked = quest.StartCondition.RequiresLocation;
            txtMap.Text = quest.StartCondition.LocationMapID.ToString();
            txtX.Text = quest.StartCondition.LocationX.ToString();
            txtY.Text = quest.StartCondition.LocationY.ToString();
            txtRange.Text = quest.StartCondition.LocationRange.ToString();
            chkRequiresQuest.Checked = quest.StartCondition.RequiresQuest;
            txtQuest.Text = quest.StartCondition.QuestID.ToString();
            chkRace.Checked = quest.StartCondition.RequiresRace;
            txtRace.Text = quest.StartCondition.Race.ToString();
            chkClass.Checked = quest.StartCondition.RequiresClass;
            cmbClass.SelectedIndex = (int) quest.StartCondition.Class;
            chkGender.Checked = quest.StartCondition.RequiresGender;
            cmbGender.SelectedIndex = (int) quest.StartCondition.Gender;
            chkDate.Checked = quest.StartCondition.RequiresDateMode;
            cmbDateMode.SelectedIndex = (int) quest.StartCondition.DateMode;
            txtStartDate.Text = quest.StartCondition.StartDate.ToString();
            txtEndDate.Text = quest.StartCondition.EndDate.ToString();

            #endregion

            #region End Conditions

            chkEndWaitList.Checked = quest.EndCondition.IsWaitListProgress;
            chkEndLevel.Checked = quest.EndCondition.RequiresLevel;
            txtEndLevel.Text = quest.EndCondition.Level.ToString();
            chkEndLocation.Checked = quest.EndCondition.RequiresLocation;
            txtEndMap.Text = quest.EndCondition.LocationMapID.ToString();
            txtEndX.Text = quest.EndCondition.LocationX.ToString();
            txtEndY.Text = quest.EndCondition.LocationY.ToString();
            txtEndRange.Text = quest.EndCondition.LocationRange.ToString();
            chkEndScenario.Checked = quest.EndCondition.IsScenario;
            txtEndScenario.Text = quest.EndCondition.ScenarioID.ToString();
            chkEndRace.Checked = quest.EndCondition.RequiresRace;
            txtEndRace.Text = quest.EndCondition.Race.ToString();
            chkEndClass.Checked = quest.EndCondition.RequiresClass;
            cmbEndClass.SelectedIndex = (int) quest.EndCondition.Class;
            chkTimeLimit.Checked = quest.EndCondition.IsTimeLimit;
            txtTimeLimit.Text = quest.EndCondition.TimeLimit.ToString();

            for (var i = 0; i < quest.EndCondition.NPCMobs.Length; i++)
            {
                var mob = quest.EndCondition.NPCMobs[i];
                var row = gridMobs.Rows[i];

                row.Cells["Required"].Value = mob.IsRequired;
                row.Cells["Mob"].Value = mob.ID.ToString();
                row.Cells["Action"].Value = Action.Items[(int) mob.Action];
                row.Cells["Count"].Value = mob.Count.ToString();
                row.Cells["TargetGroup"].Value = mob.TargetGroup.ToString();
            }

            for (var i = 0; i < quest.EndCondition.Items.Length; i++)
            {
                var item = quest.EndCondition.Items[i];
                var row = gridItems.Rows[i];

                row.Cells["ItemRequired"].Value = item.IsRequired;
                row.Cells["Item"].Value = item.ID.ToString();
                row.Cells["Lot"].Value = item.Lot.ToString();
            }

            #endregion

            #region Actions

            txtActionCount.Text = quest.ActionCount.ToString();

            for (var i = 0; i < quest.Actions.Length; i++)
            {
                var action = quest.Actions[i];
                var row = gridActions.Rows[i];

                row.Cells["actionIfType"].Value = actionIfType.Items[(int) action.IfType];
                row.Cells["actionIfTarget"].Value = action.IfTarget.ToString();
                row.Cells["actionThenType"].Value = actionThenType.Items[(int) action.ThenType];
                row.Cells["actionThenTarget"].Value = action.ThenTarget.ToString();
                row.Cells["actionThenPercent"].Value = action.ThenPercent.ToString();
                row.Cells["actionThenMinCount"].Value = action.ThenMinCount.ToString();
                row.Cells["actionThenMaxCount"].Value = action.ThenMaxCount.ToString();
                row.Cells["actionTargetGroup"].Value = action.TargetGroup.ToString();
            }

            #endregion

            #region Rewards

            for (var i = 0; i < quest.Rewards.Length; i++)
            {
                var reward = quest.Rewards[i];
                var row = gridRewards.Rows[i];

                row.Cells["rewardUse"].Value = rewardUse.Items[(int) reward.Use];
                row.Cells["rewardType"].Value = rewardType.Items[(int) reward.Type];
                row.Cells["rewardEXP"].Value = reward.Value.EXP.ToString();
                row.Cells["rewardMoney"].Value = reward.Value.Money.ToString();
                row.Cells["rewardItem"].Value = reward.Value.ItemID.ToString();
                row.Cells["rewardItemLot"].Value = reward.Value.ItemLot.ToString();
                row.Cells["rewardAbState"].Value = reward.Value.AbStateID.ToString();
                row.Cells["rewardAbStateStrength"].Value = reward.Value.AbStateStrength.ToString();
                row.Cells["rewardAbStateKeepTime"].Value = reward.Value.AbStateKeepTime.ToString();
                row.Cells["rewardFame"].Value = reward.Value.Fame.ToString();
                row.Cells["rewardPet"].Value = reward.Value.PetID.ToString();
                row.Cells["rewardMiniHouse"].Value = reward.Value.MiniHouseID.ToString();
                row.Cells["rewardTitleType"].Value = reward.Value.TitleType.ToString();
                row.Cells["rewardTitleElement"].Value = reward.Value.TitleElementNo.ToString();
            }

            #endregion

            #region Scripts

            txtStartScriptID.Text = quest.StartScriptID.ToString();
            txtStartScript.Text = quest.StartScript;
            txtDoingScriptID.Text = quest.DoingScriptID.ToString();
            txtDoingScript.Text = quest.DoingScript;
            txtEndScriptID.Text = quest.EndScriptID.ToString();
            txtEndScript.Text = quest.EndScript;

            #endregion
        }

        public void Save()
        {
            try
            {
                quest.ID = ushort.Parse(txtID.Text);
                quest.NameID = int.Parse(txtName.Text);
                quest.BriefID = int.Parse(txtBrief.Text);
                quest.Region = byte.Parse(txtRegion.Text);
                quest.Type = (QuestType) cmbType.SelectedIndex;
                quest.IsRepeatable = chkRepeat.Checked;
                quest.DailyQuestType = (DailyQuestType) cmbDailyType.SelectedIndex;

                quest.StartCondition.IsWaitListView = chkWaitlist.Checked;
                quest.StartCondition.IsWaitListProgress = chkWaitlistProgress.Checked;
                quest.StartCondition.RequiresLevel = chkLevel.Checked;
                quest.StartCondition.MinLevel = byte.Parse(txtMinLevel.Text);
                quest.StartCondition.MaxLevel = byte.Parse(txtMaxLevel.Text);
                quest.StartCondition.RequiresNPC = chkNPC.Checked;
                quest.StartCondition.NPCID = ushort.Parse(txtNPC.Text);
                quest.StartCondition.RequiresItem = chkItem.Checked;
                quest.StartCondition.ItemID = ushort.Parse(txtItem.Text);
                quest.StartCondition.ItemLot = ushort.Parse(txtItemLot.Text);
                quest.StartCondition.RequiresLocation = chkLocation.Checked;
                quest.StartCondition.LocationMapID = ushort.Parse(txtMap.Text);
                quest.StartCondition.LocationX = int.Parse(txtX.Text);
                quest.StartCondition.LocationY = int.Parse(txtY.Text);
                quest.StartCondition.LocationRange = int.Parse(txtRange.Text);
                quest.StartCondition.RequiresQuest = chkRequiresQuest.Checked;
                quest.StartCondition.QuestID = ushort.Parse(txtQuest.Text);
                quest.StartCondition.RequiresRace = chkRace.Checked;
                quest.StartCondition.Race = byte.Parse(txtRace.Text);
                quest.StartCondition.RequiresClass = chkClass.Checked;
                quest.StartCondition.Class = (UseClassType) cmbClass.SelectedIndex;
                quest.StartCondition.RequiresGender = chkGender.Checked;
                quest.StartCondition.Gender = (Gender) cmbGender.SelectedIndex;
                quest.StartCondition.RequiresDateMode = chkDate.Checked;
                quest.StartCondition.DateMode = (QuestStartDateMode) cmbDateMode.SelectedIndex;
                quest.StartCondition.StartDate = long.Parse(txtStartDate.Text);
                quest.StartCondition.EndDate = long.Parse(txtEndDate.Text);

                quest.EndCondition.IsWaitListProgress = chkEndWaitList.Checked;
                quest.EndCondition.RequiresLevel = chkEndLevel.Checked;
                quest.EndCondition.Level = byte.Parse(txtEndLevel.Text);
                quest.EndCondition.RequiresLocation = chkEndLocation.Checked;
                quest.EndCondition.LocationMapID = ushort.Parse(txtEndMap.Text);
                quest.EndCondition.LocationX = int.Parse(txtEndX.Text);
                quest.EndCondition.LocationY = int.Parse(txtEndY.Text);
                quest.EndCondition.LocationRange = int.Parse(txtEndRange.Text);
                quest.EndCondition.IsScenario = chkEndScenario.Checked;
                quest.EndCondition.ScenarioID = ushort.Parse(txtEndScenario.Text);
                quest.EndCondition.RequiresRace = chkEndRace.Checked;
                quest.EndCondition.Race = byte.Parse(txtEndRace.Text);
                quest.EndCondition.RequiresClass = chkEndClass.Checked;
                quest.EndCondition.Class = (UseClassType) cmbEndClass.SelectedIndex;
                quest.EndCondition.IsTimeLimit = chkTimeLimit.Checked;
                quest.EndCondition.TimeLimit = ushort.Parse(txtTimeLimit.Text);

                for (var i = 0; i < quest.EndCondition.NPCMobs.Length; i++)
                {
                    var mob = quest.EndCondition.NPCMobs[i];
                    var row = gridMobs.Rows[i];

                    mob.IsRequired = (bool) row.Cells["Required"].Value;
                    mob.ID = ushort.Parse((string) row.Cells["Mob"].Value);
                    mob.Action = (QuestNPCMobAction) row.Cells["Action"].Value;
                    mob.Count = byte.Parse((string) row.Cells["Count"].Value);
                    mob.TargetGroup = byte.Parse((string) row.Cells["TargetGroup"].Value);
                }

                for (var i = 0; i < quest.EndCondition.Items.Length; i++)
                {
                    var item = quest.EndCondition.Items[i];
                    var row = gridItems.Rows[i];

                    item.IsRequired = (bool) row.Cells["ItemRequired"].Value;
                    item.ID = ushort.Parse((string) row.Cells["Item"].Value);
                    item.Lot = ushort.Parse((string) row.Cells["Lot"].Value);
                }

                quest.ActionCount = byte.Parse(txtActionCount.Text);

                for (var i = 0; i < quest.Actions.Length; i++)
                {
                    var action = quest.Actions[i];
                    var row = gridActions.Rows[i];

                    action.IfType = (QuestActionIfType) row.Cells["actionIfType"].Value;
                    action.IfTarget = int.Parse((string) row.Cells["actionIfTarget"].Value);
                    action.ThenType = (QuestActionThenType) row.Cells["actionThenType"].Value;
                    action.ThenTarget = int.Parse((string) row.Cells["actionThenTarget"].Value);
                    action.ThenPercent = int.Parse((string) row.Cells["actionThenPercent"].Value);
                    action.ThenMinCount = int.Parse((string) row.Cells["actionThenMinCount"].Value);
                    action.ThenMaxCount = int.Parse((string) row.Cells["actionThenMaxCount"].Value);
                    action.TargetGroup = byte.Parse((string) row.Cells["actionTargetGroup"].Value);
                }

                for (var i = 0; i < quest.Rewards.Length; i++)
                {
                    var reward = quest.Rewards[i];
                    var row = gridRewards.Rows[i];

                    reward.Use = (QuestRewardUse) row.Cells["rewardUse"].Value;
                    reward.Type = (QuestRewardType) row.Cells["rewardType"].Value;
                    reward.Value.EXP = long.Parse((string) row.Cells["rewardEXP"].Value);
                    reward.Value.Money = long.Parse((string) row.Cells["rewardMoney"].Value);
                    reward.Value.ItemID = ushort.Parse((string) row.Cells["rewardItem"].Value);
                    reward.Value.ItemLot = ushort.Parse((string) row.Cells["rewardItemLot"].Value);
                    reward.Value.AbStateID = ushort.Parse((string) row.Cells["rewardAbState"].Value);
                    reward.Value.AbStateStrength = byte.Parse((string) row.Cells["rewardAbStateStrength"].Value);
                    reward.Value.AbStateKeepTime = int.Parse((string) row.Cells["rewardAbStateKeepTime"].Value);
                    reward.Value.Fame = int.Parse((string) row.Cells["rewardFame"].Value);
                    reward.Value.PetID = int.Parse((string) row.Cells["rewardPet"].Value);
                    reward.Value.MiniHouseID = byte.Parse((string) row.Cells["rewardMiniHouse"].Value);
                    reward.Value.TitleType = byte.Parse((string) row.Cells["rewardTitleType"].Value);
                    reward.Value.TitleElementNo = byte.Parse((string) row.Cells["rewardTitleElement"].Value);
                }

                quest.StartScriptID = int.Parse(txtStartScriptID.Text);
                quest.DoingScriptID = int.Parse(txtDoingScriptID.Text);
                quest.EndScriptID = int.Parse(txtEndScriptID.Text);

                quest.StartScript = txtStartScript.Text;
                quest.DoingScript = txtDoingScript.Text;
                quest.EndScript = txtEndScript.Text;

                quest.ScriptStartSize = (ushort) quest.StartScript.Length;
                quest.ScriptDoingSize = (ushort) quest.DoingScript.Length;
                quest.ScriptEndSize = (ushort) quest.EndScript.Length;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}