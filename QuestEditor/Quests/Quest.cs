// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace QuestEditor.Quests
{
    public class Quest
    {
        public ushort ID { get; set; }
        public int NameID { get; set; }
        public string Name
        {
            get
            {
                if (Main.QuestDialog == null || !Main.QuestDialog.TryGetValue((uint)NameID, out var name))
                {
                    return "Unnamed Quest";
                }

                return name;
            }
        }

        public int BriefID { get; set; }
        public byte Region { get; set; }
        public QuestType Type { get; set; } = QuestType.QT_NORMAL;
        public bool IsRepeatable { get; set; }
        public DailyQuestType DailyQuestType { get; set; } = DailyQuestType.DQT_NONE;
        public QuestStartCondition StartCondition { get; set; } = new QuestStartCondition();
        public QuestEndCondition EndCondition { get; set; } = new QuestEndCondition();
        public byte ActionCount { get; set; }
        public QuestAction[] Actions { get; set; }
        public QuestReward[] Rewards { get; set; }
        public ushort ScriptStartSize { get; set; }
        public ushort ScriptEndSize { get; set; }
        public ushort ScriptDoingSize { get; set; }
        public int StartScriptID { get; set; }
        public int DoingScriptID { get; set; }
        public int EndScriptID { get; set; }
        public string StartScript { get; set; } = "";
        public string DoingScript { get; set; } = "";
        public string EndScript { get; set; } = "";
        public ushort RewardNPC => EndCondition.NPCMobs.ToList().FirstOrDefault(m => m.Action == QuestNPCMobAction.QNMA_REWARD_OBJECT)?.ID ?? 0;

        public Quest()
        {
            Actions = new[] { new QuestAction(), new QuestAction(), new QuestAction(), new QuestAction(), new QuestAction(), new QuestAction(), new QuestAction(), new QuestAction(), new QuestAction(), new QuestAction() };
            Rewards = new[] { new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward(), new QuestReward() };
        }

        public static Quest Parse(byte[] data)
        {
            try
            {
                var quest = new Quest();

                using (var stream = new MemoryStream(data))
                using (var reader = new BinaryReader(stream))
                {
                    quest.ID = (ushort) reader.ReadInt32();
                    quest.NameID = reader.ReadInt32();
                    quest.BriefID = reader.ReadInt32();
                    quest.Region = reader.ReadByte();
                    quest.Type = (QuestType)reader.ReadByte();
                    quest.IsRepeatable = reader.ReadBoolean();
                    quest.DailyQuestType = (DailyQuestType)reader.ReadByte();
                    reader.ReadBytes(4);

                    // Start Condition
                    quest.StartCondition = new QuestStartCondition();
                    quest.StartCondition.IsWaitListView = reader.ReadBoolean();
                    quest.StartCondition.IsWaitListProgress = reader.ReadBoolean();
                    quest.StartCondition.RequiresLevel = reader.ReadBoolean();
                    quest.StartCondition.MinLevel = reader.ReadByte();
                    quest.StartCondition.MaxLevel = reader.ReadByte();
                    quest.StartCondition.RequiresNPC = reader.ReadBoolean();
                    quest.StartCondition.NPCID = reader.ReadUInt16();
                    quest.StartCondition.RequiresItem = reader.ReadBoolean();
                    reader.ReadBytes(1);
                    quest.StartCondition.ItemID = reader.ReadUInt16();
                    quest.StartCondition.ItemLot = reader.ReadUInt16();
                    quest.StartCondition.RequiresLocation = reader.ReadBoolean();
                    reader.ReadBytes(1);
                    quest.StartCondition.LocationMapID = reader.ReadUInt16();
                    reader.ReadBytes(2);
                    quest.StartCondition.LocationX = reader.ReadInt32();
                    quest.StartCondition.LocationY = reader.ReadInt32();
                    quest.StartCondition.LocationRange = reader.ReadInt32();
                    quest.StartCondition.RequiresQuest = reader.ReadBoolean();
                    reader.ReadBytes(1);
                    quest.StartCondition.QuestID = reader.ReadUInt16();
                    quest.StartCondition.RequiresRace = reader.ReadBoolean();
                    quest.StartCondition.Race = reader.ReadByte();
                    quest.StartCondition.RequiresClass = reader.ReadBoolean();
                    quest.StartCondition.Class = (CharacterClass) reader.ReadByte();
                    quest.StartCondition.RequiresGender = reader.ReadBoolean();
                    quest.StartCondition.Gender = (Gender)reader.ReadByte();
                    quest.StartCondition.RequiresDateMode = reader.ReadBoolean();
                    quest.StartCondition.DateMode = (QuestStartDateMode)reader.ReadByte();
                    reader.ReadBytes(4);
                    quest.StartCondition.StartDate = reader.ReadInt64();
                    quest.StartCondition.EndDate = reader.ReadInt64();

                    // End Condition
                    quest.EndCondition = new QuestEndCondition();
                    quest.EndCondition.IsWaitListProgress = reader.ReadBoolean();
                    quest.EndCondition.RequiresLevel = reader.ReadBoolean();
                    quest.EndCondition.Level = reader.ReadByte();
                    reader.ReadBytes(1);

                    for (var i = 0; i < 5; i++)
                    {
                        var npcMob = quest.EndCondition.NPCMobs[i];

                        npcMob.IsRequired = reader.ReadBoolean();
                        reader.ReadBytes(1);
                        npcMob.ID = reader.ReadUInt16();
                        npcMob.Action = (QuestNPCMobAction)reader.ReadByte();
                        npcMob.Count = reader.ReadByte();
                        npcMob.TargetGroup = reader.ReadByte();
                        reader.ReadBytes(1);
                    }

                    for (var i = 0; i < 5; i++)
                    {
                        var item = quest.EndCondition.Items[i];

                        item.IsRequired = reader.ReadBoolean();
                        reader.ReadBytes(1);
                        item.ID = reader.ReadUInt16();
                        item.Lot = reader.ReadUInt16();
                    }

                    quest.EndCondition.RequiresLocation = reader.ReadBoolean();
                    reader.ReadBytes(1);
                    quest.EndCondition.LocationMapID = reader.ReadUInt16();
                    reader.ReadBytes(2);
                    quest.EndCondition.LocationX = reader.ReadInt32();
                    quest.EndCondition.LocationY = reader.ReadInt32();
                    quest.EndCondition.LocationRange = reader.ReadInt32();
                    quest.EndCondition.IsScenario = reader.ReadBoolean();
                    reader.ReadBytes(1);
                    quest.EndCondition.ScenarioID = reader.ReadUInt16();
                    quest.EndCondition.RequiresRace = reader.ReadBoolean();
                    quest.EndCondition.Race = reader.ReadByte();
                    quest.EndCondition.RequiresClass = reader.ReadBoolean();
                    quest.EndCondition.Class = (CharacterClass)reader.ReadByte();
                    quest.EndCondition.IsTimeLimit = reader.ReadBoolean();
                    reader.ReadBytes(1);
                    quest.EndCondition.TimeLimit = reader.ReadUInt16();

                    // Actions
                    quest.ActionCount = reader.ReadByte();
                    reader.ReadBytes(3);

                    for (var i = 0; i < 10; i++)
                    {
                        var action = quest.Actions[i];

                        action.IfType = (QuestActionIfType)reader.ReadByte();
                        reader.ReadBytes(3);
                        action.IfTarget = reader.ReadInt32();
                        action.ThenType = (QuestActionThenType)reader.ReadByte();
                        reader.ReadBytes(3);
                        action.ThenTarget = reader.ReadInt32();
                        action.ThenPercent = reader.ReadInt32();
                        action.ThenMinCount = reader.ReadInt32();
                        action.ThenMaxCount = reader.ReadInt32();
                        action.TargetGroup = reader.ReadByte();
                        reader.ReadBytes(3);
                    }

                    // Rewards
                    for (var i = 0; i < 12; i++)
                    {
                        var reward = quest.Rewards[i];

                        reward.Use = (QuestRewardUse)reader.ReadByte();
                        reward.Type = (QuestRewardType)reader.ReadByte();
                        reader.ReadBytes(2);

                        switch (reward.Type)
                        {
                            case QuestRewardType.QRT_EXP:
                                reward.Value.EXP = reader.ReadInt64();
                                break;
                            case QuestRewardType.QRT_MONEY:
                                reward.Value.Money = reader.ReadInt64();
                                break;
                            case QuestRewardType.QRT_ITEM:
                                reward.Value.ItemID = reader.ReadUInt16();
                                reward.Value.ItemLot = reader.ReadUInt16();
                                reader.ReadBytes(4);
                                break;
                            case QuestRewardType.QRT_ABSTATE:
                                reward.Value.AbStateKeepTime = reader.ReadInt32();
                                reward.Value.AbStateID = reader.ReadUInt16();
                                reward.Value.AbStateStrength = reader.ReadByte();
                                reader.ReadBytes(1);
                                break;
                            case QuestRewardType.QRT_FAME:
                                reward.Value.Fame = reader.ReadInt32();
                                reader.ReadBytes(4);
                                break;
                            case QuestRewardType.QRT_PET:
                                reward.Value.PetID = reader.ReadInt32();
                                reader.ReadBytes(4);
                                break;
                            case QuestRewardType.QRT_MINIHOUSE:
                                reward.Value.MiniHouseID = reader.ReadByte();
                                reader.ReadBytes(7);
                                break;
                            case QuestRewardType.QRT_TITLE:
                                reward.Value.TitleType = reader.ReadByte();
                                reward.Value.TitleElementNo = reader.ReadByte();
                                reader.ReadBytes(6);
                                break;
                            case QuestRewardType.QRT_KILLPOINT:
                                reward.Value.KillPoints = reader.ReadInt32();
                                reader.ReadBytes(4);
                                break;
                        }
                    }

                    quest.ScriptStartSize = reader.ReadUInt16();
                    quest.ScriptEndSize = reader.ReadUInt16();
                    quest.ScriptDoingSize = reader.ReadUInt16();
                    reader.ReadUInt16();
                    quest.StartScriptID = reader.ReadInt32();
                    quest.DoingScriptID = reader.ReadInt32();
                    quest.EndScriptID = reader.ReadInt32();
                    quest.StartScript = Encoding.ASCII.GetString(reader.ReadBytes(quest.ScriptStartSize));
                    quest.DoingScript = Encoding.ASCII.GetString(reader.ReadBytes(quest.ScriptDoingSize));
                    quest.EndScript = Encoding.ASCII.GetString(reader.ReadBytes(quest.ScriptEndSize));

                    quest.StartScript = quest.StartScript.Substring(0, Math.Max(0, quest.StartScript.IndexOf(char.MinValue)));
                    quest.DoingScript = quest.DoingScript.Substring(0, Math.Max(0, quest.DoingScript.IndexOf(char.MinValue)));
                    quest.EndScript = quest.EndScript.Substring(0, Math.Max(0, quest.EndScript.IndexOf(char.MinValue)));
                }

                return quest;
            }
            catch
            {
                return null;
            }
        }

        public override string ToString()
        {
            return $"{ID} - {Name}";
        }
    }
}
