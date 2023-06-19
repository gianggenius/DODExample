using System;

namespace DODExample
{
    public class PlayerHealthTable:BaseTable<PlayerHealthData, PlayerHealthRecord>
    {
        public int GetPlayerID(int index)
        {
            return data.PlayerIDs[index];
        }
        
        public int GetHeath(int index)
        {
            return data.Hp[index];
        }
        
        public int GetIndexByPlayerID(int playerID)
        {
            return Array.IndexOf(data.PlayerIDs,playerID);
        }
        
        public int GetHealthByPlayerID(int playerID)
        {
            return data.Hp[GetIndexByPlayerID(playerID)];
        }
        
        public bool GetDeadStatusByPlayerID(int playerID)
        {
            return data.IsDead[GetIndexByPlayerID(playerID)];
        }
        
        public PlayerHealthRecord GetRecordByPlayerID(int playerID)
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
        
        public void SetHealthByPlayerID(int playerID, int hp)
        {
            data.Hp[GetIndexByPlayerID(playerID)] = hp;
        }

        public PlayerHealthTable(PlayerHealthData data) : base(data)
        {
        }
    }
}