using System;

namespace DODExample
{
    public class PlayerHealthTable:BaseTable<PlayerHealthData, PlayerHealthRecord>
    {
        public string GetPlayerID(int index)
        {
            return data.PlayerIDs[index];
        }
        
        public int GetHeath(int index)
        {
            return data.Hp[index];
        }
        
        public int GetIndexByPlayerID(string playerID)
        {
            return Array.IndexOf(data.PlayerIDs,playerID);
        }
        
        public int GetHealthByPlayerID(string playerID)
        {
            return data.Hp[GetIndexByPlayerID(playerID)];
        }
        
        public bool GetDeadStatusByPlayerID(string playerID)
        {
            return data.IsDead[GetIndexByPlayerID(playerID)];
        }
        
        public PlayerHealthRecord GetRecordByPlayerID(string playerID)
        {
            return GetRecordByIndex(GetIndexByPlayerID(playerID));
        }
        
        public void SetHealth(int index, int hp)
        {
            data.Hp[index] = hp;
        }
        public void SetDeadStatus(int index, bool isDead)
        {
            data.IsDead[index] = isDead;
        }
        
        public void SetHealthByPlayerID(string playerID, int hp)
        {
            data.Hp[GetIndexByPlayerID(playerID)] = hp;
        }

        public PlayerHealthTable(PlayerHealthData data) : base(data)
        {
        }
    }
}