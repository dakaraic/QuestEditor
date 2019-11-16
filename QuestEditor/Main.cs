// Copyright 2019 RED Software, LLC. All Rights Reserved.

using QuestEditor.IO;
using QuestEditor.Quests;
using QuestEditor.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuestEditor
{
    public partial class Main : Form
    {
        public static Quest ActiveQuest;
        public static UI.Quests Quests;

        public static Dictionary<uint, string> QuestDialog;
        public static Dictionary<ushort, Quest> QuestData;

        private ushort header;
        private string filePath;

        public Main()
        {
            InitializeComponent();
            header = 6;

            massModifiersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                massEXPModifierToolStripMenuItem,
                massMoneyModifierToolStripMenuItem,
                massFameModifierToolStripMenuItem
            });
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Open QuestData.shn";
                dialog.Filter = "SHN File|*.shn";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filePath = dialog.FileName;
                Open();
            }
        }

        private void Open()
        {
            if (QuestData != null)
            {
                CloseFile();
            }

            ParseQuestDialog(filePath);
            ParseQuestData(filePath);

            Quests = new UI.Quests { Dock = DockStyle.Fill };

            inactiveLogo.Visible = false;
            saveToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            deleteAllQuestsToolStripMenuItem.Enabled = true;
            newQuestToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            massModifiersToolStripMenuItem.Enabled = true;
            massEXPModifierToolStripMenuItem.Enabled = true;
            massMoneyModifierToolStripMenuItem.Enabled = true;
            massFameModifierToolStripMenuItem.Enabled = true;

            mainPanel.Controls.Add(Quests);
            Quests.RefreshList();
        }

        private void ParseQuestDialog(string fileName)
        {
            var path = Path.GetDirectoryName(fileName) + "/QuestDialog.shn";

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
                        var id = Convert.ToUInt32(reader.GetValue(0));
                        QuestDialog.Remove(id);
                        QuestDialog.Add(id, reader.GetString(1));
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
                header = reader.ReadUInt16();
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void CloseFile()
        {
            if (Quests == null)
            {
                return;
            }

            ActiveQuest = null;

            QuestData = null;
            QuestDialog = null;

            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            deleteAllQuestsToolStripMenuItem.Enabled = false;
            newQuestToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            massModifiersToolStripMenuItem.Enabled = false;
            massEXPModifierToolStripMenuItem.Enabled = false;
            massMoneyModifierToolStripMenuItem.Enabled = false;
            massFameModifierToolStripMenuItem.Enabled = false;

            mainPanel.Controls.Remove(Quests);
            inactiveLogo.Visible = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Quests == null)
            {
                return;
            }

            SaveFile();
        }

        private void SaveFile()
        {
            Quests?.SaveView(); // Saves the currently active quest.

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(header);
                writer.Write((ushort) QuestData.Count);

                foreach (var quest in QuestData.Values)
                {
                    var lengthPosition = stream.Position;
                    var startLength = stream.Length;

                    writer.Write(0);
                    writer.Write((int) quest.ID);
                    writer.Write(quest.NameID);
                    writer.Write(quest.BriefID);
                    writer.Write(quest.Region);
                    writer.Write((byte)quest.Type);
                    writer.Write(quest.IsRepeatable);
                    writer.Write((byte)quest.DailyQuestType);
                    writer.Fill(4);

                    // Start Condition
                    writer.Write(quest.StartCondition.IsWaitListView);
                    writer.Write(quest.StartCondition.IsWaitListProgress);
                    writer.Write(quest.StartCondition.RequiresLevel);
                    writer.Write(quest.StartCondition.MinLevel);
                    writer.Write(quest.StartCondition.MaxLevel);
                    writer.Write(quest.StartCondition.RequiresNPC);
                    writer.Write(quest.StartCondition.NPCID);
                    writer.Write(quest.StartCondition.RequiresItem);
                    writer.Fill(1);
                    writer.Write(quest.StartCondition.ItemID);
                    writer.Write(quest.StartCondition.ItemLot);
                    writer.Write(quest.StartCondition.RequiresLocation);
                    writer.Fill(1);
                    writer.Write(quest.StartCondition.LocationMapID);
                    writer.Fill(2);
                    writer.Write(quest.StartCondition.LocationX);
                    writer.Write(quest.StartCondition.LocationY);
                    writer.Write(quest.StartCondition.LocationRange);
                    writer.Write(quest.StartCondition.RequiresQuest);
                    writer.Fill(1);
                    writer.Write(quest.StartCondition.QuestID);
                    writer.Write(quest.StartCondition.RequiresRace);
                    writer.Write(quest.StartCondition.Race);
                    writer.Write(quest.StartCondition.RequiresClass);
                    writer.Write((byte)quest.StartCondition.Class);
                    writer.Write(quest.StartCondition.RequiresGender);
                    writer.Write((byte)quest.StartCondition.Gender);
                    writer.Write(quest.StartCondition.RequiresDateMode);
                    writer.Write((byte)quest.StartCondition.DateMode);
                    writer.Fill(4);
                    writer.Write(quest.StartCondition.StartDate);
                    writer.Write(quest.StartCondition.EndDate);

                    writer.Write(quest.EndCondition.IsWaitListProgress);
                    writer.Write(quest.EndCondition.RequiresLevel);
                    writer.Write(quest.EndCondition.Level);
                    writer.Fill(1);

                    for (var i = 0; i < 5; i++)
                    {
                        var mob = quest.EndCondition.NPCMobs[i];

                        writer.Write(mob.IsRequired);
                        writer.Fill(1);
                        writer.Write(mob.ID);
                        writer.Write((byte)mob.Action);
                        writer.Write(mob.Count);
                        writer.Write(mob.TargetGroup);
                        writer.Fill(1);
                    }

                    for (var i = 0; i < 5; i++)
                    {
                        var item = quest.EndCondition.Items[i];

                        writer.Write(item.IsRequired);
                        writer.Fill(1);
                        writer.Write(item.ID);
                        writer.Write(item.Lot);
                    }

                    writer.Write(quest.EndCondition.RequiresLocation);
                    writer.Fill(1);
                    writer.Write(quest.EndCondition.LocationMapID);
                    writer.Fill(2);
                    writer.Write(quest.EndCondition.LocationX);
                    writer.Write(quest.EndCondition.LocationY);
                    writer.Write(quest.EndCondition.LocationRange);
                    writer.Write(quest.EndCondition.IsScenario);
                    writer.Fill(1);
                    writer.Write(quest.EndCondition.ScenarioID);
                    writer.Write(quest.EndCondition.RequiresRace);
                    writer.Write(quest.EndCondition.Race);
                    writer.Write(quest.EndCondition.RequiresClass);
                    writer.Write((byte)quest.EndCondition.Class);
                    writer.Write(quest.EndCondition.IsTimeLimit);
                    writer.Fill(1);
                    writer.Write(quest.EndCondition.TimeLimit);

                    writer.Write(quest.ActionCount);
                    writer.Fill(3);

                    for (var i = 0; i < 10; i++)
                    {
                        var action = quest.Actions[i];

                        writer.Write((byte)action.IfType);
                        writer.Fill(3);
                        writer.Write(action.IfTarget);
                        writer.Write((byte)action.ThenType);
                        writer.Fill(3);
                        writer.Write(action.ThenTarget);
                        writer.Write(action.ThenPercent);
                        writer.Write(action.ThenMinCount);
                        writer.Write(action.ThenMaxCount);
                        writer.Write(action.TargetGroup);
                        writer.Fill(3);
                    }

                    for (var i = 0; i < 12; i++)
                    {
                        var reward = quest.Rewards[i];

                        writer.Write((byte)reward.Use);
                        writer.Write((byte)reward.Type);
                        writer.Fill(2);

                        switch (reward.Type)
                        {
                            case QuestRewardType.QRT_EXP:
                                writer.Write(reward.Value.EXP);
                                break;
                            case QuestRewardType.QRT_MONEY:
                                writer.Write(reward.Value.Money);
                                break;
                            case QuestRewardType.QRT_ITEM:
                                writer.Write(reward.Value.ItemID);
                                writer.Write(reward.Value.ItemLot);
                                writer.Fill(4);
                                break;
                            case QuestRewardType.QRT_ABSTATE:
                                writer.Write(reward.Value.AbStateKeepTime);
                                writer.Write(reward.Value.AbStateID);
                                writer.Write(reward.Value.AbStateStrength);
                                writer.Fill(1);
                                break;
                            case QuestRewardType.QRT_FAME:
                                writer.Write(reward.Value.Fame);
                                writer.Fill(4);
                                break;
                            case QuestRewardType.QRT_PET:
                                writer.Write(reward.Value.PetID);
                                writer.Fill(4);
                                break;
                            case QuestRewardType.QRT_MINIHOUSE:
                                writer.Write(reward.Value.MiniHouseID);
                                writer.Fill(7);
                                break;
                            case QuestRewardType.QRT_TITLE:
                                writer.Write(reward.Value.TitleType);
                                writer.Write(reward.Value.TitleElementNo);
                                writer.Fill(6);
                                break;
                            case QuestRewardType.QRT_KILLPOINT:
                                writer.Write(reward.Value.KillPoints);
                                writer.Fill(4);
                                break;
                        }
                    }

                    writer.Write((ushort) (quest.StartScript.Length + 1));
                    writer.Write((ushort) (quest.EndScript.Length + 1));
                    writer.Write((ushort) (quest.DoingScript.Length + 1));
                    writer.Write((ushort) 0);
                    writer.Write(quest.StartScriptID);
                    writer.Write(quest.DoingScriptID);
                    writer.Write(quest.EndScriptID);
                    writer.Write(Encoding.ASCII.GetBytes(quest.StartScript + char.MinValue));
                    writer.Write(Encoding.ASCII.GetBytes(quest.DoingScript + char.MinValue));
                    writer.Write(Encoding.ASCII.GetBytes(quest.EndScript + char.MinValue));

                    var endLength = stream.Length;

                    writer.Seek((int) lengthPosition, SeekOrigin.Begin);
                    writer.Write((int) (endLength - startLength));
                    stream.Position = endLength;
                }

                File.WriteAllBytes(filePath, stream.ToArray());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Exit())
            {
                return;
            }

            Environment.Exit(0);
        }

        private static bool Exit()
        {
            if (QuestData == null)
            {
                return true;
            }

            var result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private static bool Save()
        {
            var result = MessageBox.Show("Do you want to save the current file?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !Exit();
        }

        private void newQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QuestData == null)
            {
                return;
            }

            var newID = (ushort) (QuestData.Values.OrderBy(q => q.ID).LastOrDefault()?.ID + 1 ?? 1);
            var quest = new Quest {ID = newID, NameID = -1};

            QuestData.Add(newID, quest);

            Quests.RefreshList();
            Quests.Open(quest);
        }

        private void deleteAllQuestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QuestData == null)
            {
                return;
            }

            QuestData.Clear();
            ActiveQuest = null;

            Quests.RefreshList();
            Quests.RefreshQuest();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QuestData != null)
            {
                // Save the current file
                if (Save())
                {
                    SaveFile();
                }
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Choose Location";
                dialog.Filter = "QuestData.shn|*.shn";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filePath = dialog.FileName;

                QuestData = new Dictionary<ushort, Quest>();

                SaveFile();
                Open();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quests?.DeleteCurrent();
        }

        private void MassEXPModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MassEXPEditor();
            form.ShowDialog(this);
        }

        private void MassMoneyModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MassCenEditor();
            form.ShowDialog(this);
        }

        private void MassFameModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MassFameEditor();
            form.ShowDialog(this);
        }
    }
}