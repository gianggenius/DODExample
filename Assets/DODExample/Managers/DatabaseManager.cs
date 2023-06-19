using DODExample;
using Unity.VisualScripting;
using UnityEngine;

namespace DODExample
{
    public class DatabaseManager : MonoBehaviour
    {
        public PlayerAttackTable PlayerAttackTable;
        public PlayerHealthTable PlayerHealthTable;
        public PlayerCombatTable PlayerCombatTable;

        #region Singleton

        private static DatabaseManager _instance;
        public static  DatabaseManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }

            Initialize();
        }

        #endregion

        private void Initialize()
        {
            PlayerAttackTable = new PlayerAttackTable(new PlayerAttackData
            {
                PlayerIDs = new[] { 1, 2, 3, 4, 5 },
                Damage    = new[] { 10, 10, 10, 10, 10 }
            });
            PlayerHealthTable = new PlayerHealthTable(new PlayerHealthData
            {
                PlayerIDs = new[] { 1, 2, 3, 4, 5 },
                Hp        = new[] { 50, 50, 50, 50, 50 },
                IsDead    = new[] { false, false, false, false, false }
            });
            PlayerCombatTable = new PlayerCombatTable(new PlayerCombatData
            {
                AttackerIDs = new[] { 1, 1 },
                TargetIDs   = new[] { 2, 3 },
            });
        }
    }
}