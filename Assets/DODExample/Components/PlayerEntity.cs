using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DODExample.Components
{
    public class PlayerEntity : MonoBehaviour, IEntity
    {
        [field: SerializeField] public string UniqueId { get; set; }

        [SerializeField] private int                  hp;
        [SerializeField] private PlayerHealthRecord   health;
        [SerializeField] private PlayerPositionRecord position;


        private void Update()
        {
            SynchronizeEntityData();
        }

        private void SynchronizeEntityData()
        {
            health             = DatabaseManager.Instance.PlayerHealthTable.GetRecordByPlayerID(UniqueId);
            position           = DatabaseManager.Instance.PlayerPositionTable.GetRecordByPlayerID(UniqueId);
            transform.position = position.Position;
        }
    }
}