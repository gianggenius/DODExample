using UnityEngine;

namespace DODExample
{
    public class PlayerCombatSystem : BaseSystem
    {
        public override void Update()
        {
            if (_databaseManager.PlayerCombatTable.Length > 0)
            {
                Process();
            }
        }

        protected override void Process()
        {
            var table = _databaseManager.PlayerCombatTable;
            while (table.Length > 0)
            {
                var record             = table.GetRecordByIndex(0);
                var targetDeadStatus   = _databaseManager.PlayerHealthTable.GetDeadStatusByPlayerID(record.TargetID);
                var attackerDeadStatus = _databaseManager.PlayerHealthTable.GetDeadStatusByPlayerID(record.AttackerID);
                if (!attackerDeadStatus && !targetDeadStatus)
                {
                    CalculateDamage(record.AttackerID, record.TargetID);
                }

                RemoveRecord(0);
            }
        }

        private void CalculateDamage(int attackerId, int targetId)
        {
            var attackerDamage = _databaseManager.PlayerAttackTable.GetDamageByPlayerID(attackerId);
            var targetHealth   = _databaseManager.PlayerHealthTable.GetHealthByPlayerID(targetId);
            targetHealth -= attackerDamage;
            _databaseManager.PlayerHealthTable.SetHealthByPlayerID(targetId, targetHealth);
            Debug.Log(
                $"Player {attackerId} deal {attackerDamage} damage to player {targetId}. Player {targetId} health is {targetHealth}");
        }

        private void RemoveRecord(int index)
        {
            _databaseManager.PlayerCombatTable.RemoveRecord(index);
        }
    }
}