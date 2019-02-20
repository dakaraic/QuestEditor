// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public enum QuestStatus
    {
        QS_NONE = 0,
        QS_ABORT = 1,
        QS_DONE = 2,
        QS_SOON = 3,
        QS_REPEAT = 4,
        QS_ABLE = 5,
        QS_ING = 6,
        QS_FAILED = 7,
        QS_REWARD = 8,
        QS_LOWABLE = 9,
        QS_READ_ABLE = 0x14,
        QS_MAX_PLAYER_QUEST_STATUS = 0x15
    }
}
