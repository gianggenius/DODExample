using System;
using UnityEngine;

namespace DODExample
{
    public class PlayerPositionTable:BaseTable<PlayerPositionData, PlayerPositionRecord>
    {
        public string GetPlayerID(int index)
        {
            return data.PlayerIDs[index];
        }
        
        public int GetIndexByPlayerID(string playerID)
        {
            return Array.IndexOf(data.PlayerIDs,playerID);
        }
        
        
        public PlayerPositionRecord GetRecordByPlayerID(string playerID)
        {
            return GetRecordByIndex(GetIndexByPlayerID(playerID));
        }
        
        public void SetPositionByIndex(int index, Vector3 position)
        {
            data.Positions[index] = position;
        }
        
        
        public PlayerPositionTable(PlayerPositionData data) : base(data)
        {
        }
    }
}