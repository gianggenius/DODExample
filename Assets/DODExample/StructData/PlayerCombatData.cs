namespace DODExample
{
    public class PlayerCombatData : ITableData
    {
        public string[] AttackerIDs;
        public string[] TargetIDs;
    }
    
    public class PlayerCombatRecord:ITableRecordData
    {
        public string AttackerID;
        public string TargetID;
    }
}