// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public class QuestReward
    {
        public QuestRewardUse Use { get; set; } = QuestRewardUse.QRU_UNUSE;
        public QuestRewardType Type { get; set; } = QuestRewardType.QRT_EXP;
        public QuestRewardValue Value { get; set; }

        public QuestReward()
        {
            Value = new QuestRewardValue();
        }
    }
}
