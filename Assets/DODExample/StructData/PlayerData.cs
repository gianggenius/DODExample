using System;
using DODExample;
using UnityEngine;


public class PlayerPositionData:ITableData
{
    public string[] PlayerIDs;
    public Vector3[] Positions;
}

public class PlayerPositionRecord:ITableRecordData
{
    public string  PlayerID;
    public Vector3 Position;
}

public class PlayerAttackData:ITableData
{
    public string[] PlayerIDs;
    public int[]    Damage;
}

public class PlayerAttackRecord:ITableRecordData
{
    public string PlayerID;
    public int    Damage;
}

public class PlayerHealthData:ITableData
{
    public string[] PlayerIDs;
    public int[]    Hp;
    public bool[]   IsDead;
}

[Serializable]
public class PlayerHealthRecord:ITableRecordData
{
    public string PlayerID;
    public int    Hp;
    public bool   IsDead;
}

public class PlayerDefenseData:ITableData
{
    public string[] PlayerIDs;
    public int[]    Defense;
}

public class PlayerSkillData:ITableData
{
    public string[] PlayerIDs;
    public int[] SkillIDs;
}