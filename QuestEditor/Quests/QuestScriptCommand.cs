// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public enum QuestScriptCommand
    {
        QSC_ERROR = 0x0,
        QSC_END = 0x1,
        QSC_SAY = 0x2,
        QSC_SCENARIO = 0x3,
        QSC_CALLPS = 0x4,
        QSC_CLEAR = 0x5,
        QSC_ACCEPT = 0x6,
        QSC_CANCEL = 0x7,
        QSC_PROGRESS = 0x8,
        QSC_FAILED = 0x9,
        QSC_DONE = 0xA,
        QSC_LINK = 0xB,
        QSC_ABORT = 0xC,
        QSC_DELETE_ITEM = 0xD,
        QSC_CREATE_ITEM = 0xE,
        QSC_DROP_ITEM = 0xF,
        QSC_REMARK = 0x10,
        QSC_IF = 0x11,
        QSC_GOTO = 0x12,
        QSC_MARK = 0x13,
        QSC_SET = 0x14,
        QSC_ADD = 0x15,
        QSC_SUB = 0x16,
        QSC_GET_PLAYER_RACE = 0x17,
        QSC_GET_PLAYER_CLASS = 0x18,
        QSC_GET_PLAYER_LEVEL = 0x19,
        QSC_GET_PLAYER_GENDER = 0x1A,
        QSC_GET_PLAYER_EMPTY_INVENTORY = 0x1B,
        QSC_REPEAT_QUEST_GIVE_UP = 0x1C,
        QSC_UNKNOWNED = 0x1D,
        QSC_SET_ABSTATE = 0x1E,
        QSC_RESET_ABSTATE = 0x1F,
        QSC_IS_ABSTATE = 0x20,
        QSC_GET_ITEM_LOT = 0x21,
        QSC_MAX = 0x22,
        QSC_EOF = 0x23
    }
}
