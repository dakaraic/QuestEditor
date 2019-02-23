// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public class QuestAction
    {
        public QuestActionIfType IfType { get; set; } = QuestActionIfType.QUEST_ACTION_IF_NONE;
        public int IfTarget { get; set; }
        public QuestActionThenType ThenType { get; set; } = QuestActionThenType.QUEST_ACTION_THEN_NONE;
        public int ThenTarget { get; set; }
        public int ThenPercent { get; set; }
        public int ThenMinCount { get; set; }
        public int ThenMaxCount { get; set; }
        public byte TargetGroup { get; set; }
    }
}
