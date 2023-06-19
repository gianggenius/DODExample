namespace DODExample
{
    public class PlayerCombatData : ITableData
    {
        public int[] AttackerIDs;
        public int[] TargetIDs;
    }
    
    public class PlayerCombatRecord:ITableRecordData
    {
        public int AttackerID;
        public int TargetID;
    }
}