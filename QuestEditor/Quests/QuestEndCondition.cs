// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public class QuestEndCondition
    {
        public bool IsWaitListProgress { get; set; }
        public bool RequiresLevel { get; set; }
        public byte Level { get; set; }
        public QuestNPCMob[] NPCMobs { get; set; }
        public QuestItem[] Items { get; set; }
        public bool RequiresLocation { get; set; }
        public ushort LocationMapID { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int LocationRange { get; set; }
        public bool IsScenario { get; set; }
        public ushort ScenarioID { get; set; }
        public bool RequiresRace { get; set; }
        public byte Race { get; set; }
        public bool RequiresClass { get; set; }
        public UseClassType Class { get; set; } = UseClassType.UCT_ALL;
        public bool IsTimeLimit { get; set; }
        public ushort TimeLimit { get; set; }

        public QuestEndCondition()
        {
            NPCMobs = new[] {new QuestNPCMob(), new QuestNPCMob(), new QuestNPCMob(), new QuestNPCMob(), new QuestNPCMob()};
            Items = new[] {new QuestItem(), new QuestItem(), new QuestItem(), new QuestItem(), new QuestItem()};
        }
    }
}
