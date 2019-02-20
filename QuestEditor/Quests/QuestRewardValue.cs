// Copyright 2019 RED Software, LLC. All Rights Reserved.

namespace QuestEditor.Quests
{
    public class QuestRewardValue
    {
        public long EXP { get; set; }
        public long Money { get; set; }
        public ushort ItemID { get; set; }
        public ushort ItemLot { get; set; }
        public int AbStateKeepTime { get; set; }
        public ushort AbStateID { get; set; }
        public byte AbStateStrength { get; set; }
        public int Fame { get; set; }
        public int PetID { get; set; }
        public byte MiniHouseID { get; set; }
        public byte TitleType { get; set; }
        public byte TitleElementNo { get; set; }
        public int KillPoints { get; set; }
    }
}
