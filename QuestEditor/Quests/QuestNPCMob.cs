// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public class QuestNPCMob
    {
        public bool IsRequired { get; set; }
        public ushort ID { get; set; }
        public QuestNPCMobAction Action { get; set; } = QuestNPCMobAction.QNMA_REWARD_OBJECT;
        public byte Count { get; set; }
        public byte TargetGroup { get; set; }
    }
}
