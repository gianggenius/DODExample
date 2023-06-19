using System;
using DODExample.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace DODExample
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private EnemyEntity enemyEntity;
        [SerializeField] private float       attackDuration;


        private PlayerEntity _entity;


        private float _lastAttackTime;

        private void Awake()
        {
            _entity = GetComponent<PlayerEntity>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && Time.time - _lastAttackTime > attackDuration)
            {
                _lastAttackTime = Time.time;
                DatabaseManager.Instance.PlayerCombatTable.AddRecord(new PlayerCombatRecord()
                    { AttackerID = _entity.UniqueId, TargetID = enemyEntity.UniqueId });
            }
        }
    }
}