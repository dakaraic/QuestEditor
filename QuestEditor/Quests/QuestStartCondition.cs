// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public class QuestStartCondition
    {
        public bool IsWaitListView { get; set; }
        public bool IsWaitListProgress { get; set; }
        public bool RequiresLevel { get; set; }
        public byte MinLevel { get; set; }
        public byte MaxLevel { get; set; }
        public bool RequiresNPC { get; set; }
        public ushort NPCID { get; set; }
        public bool RequiresItem { get; set; }
        public ushort ItemID { get; set; }
        public ushort ItemLot { get; set; }
        public bool RequiresLocation { get; set; }
        public ushort LocationMapID { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int LocationRange { get; set; }
        public bool RequiresQuest { get; set; }
        public ushort QuestID { get; set; }
        public bool RequiresRace { get; set; }
        public byte Race { get; set; }
        public bool RequiresClass { get; set; }
        public CharacterClass Class { get; set; } = CharacterClass.None;
        public bool RequiresGender { get; set; }
        public Gender Gender { get; set; } = Gender.Female;
        public bool RequiresDateMode { get; set; }
        public QuestStartDateMode DateMode { get; set; } = QuestStartDateMode.QSDM_YEAR_TERM;
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }
}
