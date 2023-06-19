using System;
using DODExample;


public class PlayerInfoData:ITableData
{
    public int[]    PlayerIDs;
    public string[] Names;
}

public class PlayerAttackData:ITableData
{
    public int[] PlayerIDs;
    public int[] Damage;
}

public class PlayerAttackRecord:ITableRecordData
{
    public int PlayerID;
    public int Damage;
}

public class PlayerHealthData:ITableData
{
    public int[]  PlayerIDs;
    public int[]  Hp;
    public bool[] IsDead;
}

[Serializable]
public class PlayerHealthRecord:ITableRecordData
{
    public int PlayerID;
    public int Hp;
    public bool IsDead;
}

public class PlayerDefenseData:ITableData
{
    public int[] PlayerIDs;
    public int[] Defense;
}

public class PlayerSkillData:ITableData
{
    public int[] PlayerIDs;
    public int[] SkillIDs;
}