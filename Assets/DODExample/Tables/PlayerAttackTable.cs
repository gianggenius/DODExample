using System;

namespace DODExample
{
    public class PlayerAttackTable:BaseTable<PlayerAttackData, PlayerAttackRecord>
    {
        public string GetPlayerID(int index)
        {
            return data.PlayerIDs[index];
        }
        
        public int GetIndexByPlayerID(string playerID)
        {
            return Array.IndexOf(data.PlayerIDs, playerID);
        }
        
        public int GetDamageByPlayerID(string playerID)
        {
            return data.Damage[GetIndexByPlayerID(playerID)];
        }
        
        public int GetDamage(int index)
        {
            return data.Damage[index];
        }
        
        public void SetDamage(int index, int damage)
        {
            data.Damage[index] = damage;
        }

        public PlayerAttackTable(PlayerAttackData data) : base(data)
        {
        }
    }
}