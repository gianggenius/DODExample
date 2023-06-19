using UnityEngine;

namespace DODExample
{
    public class PlayerDeadSystem : BaseSystem
    {

        public override void Update()
        {
            //TODO: Check if PlayerHealthTable is dirty before processing
            Process();
        }

        protected override void Process()
        {
            // Loop through all records in PlayerHealthTables to find dead players
            for (var i = 0; i < _databaseManager.PlayerHealthTable.Length; i++)
            {
                var playerHealthRecord = _databaseManager.PlayerHealthTable.GetRecordByIndex(i);
                
                // Check Player Dead Status and Hp
                if (playerHealthRecord.IsDead || playerHealthRecord.Hp > 0) continue;
                
                // Set Record data
                playerHealthRecord.Hp     = 0;
                playerHealthRecord.IsDead = true;
                
                // Update Record in Table
                _databaseManager.PlayerHealthTable.UpdateRecordByIndex(playerHealthRecord, i);
                Debug.Log($"Player {playerHealthRecord.PlayerID} is dead");
            }
        }
    }
}