using System;
using UnityEngine;

namespace DODExample.Components
{
    public class EnemyEntity:MonoBehaviour,IEntity
    {
        [field:SerializeField] public string UniqueId { get; set; }

        [SerializeField] private int                hp;
        [SerializeField] private PlayerHealthRecord record;
        
        private void Update()
        {
            SynchronizeEntityData();
        }

        private void SynchronizeEntityData()
        {
            record = DatabaseManager.Instance.PlayerHealthTable.GetRecordByPlayerID(UniqueId);
        }
    }
}