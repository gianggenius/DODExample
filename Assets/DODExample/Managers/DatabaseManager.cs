using DODExample;
using Unity.VisualScripting;
using UnityEngine;

namespace DODExample
{
    public class DatabaseManager : MonoBehaviour
    {
        public PlayerAttackTable   PlayerAttackTable;
        public PlayerHealthTable   PlayerHealthTable;
        public PlayerCombatTable   PlayerCombatTable;
        public PlayerPositionTable PlayerPositionTable;

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
            Application.targetFrameRate = 60;
        }

        #endregion

        private void Initialize()
        {
            PlayerAttackTable = new PlayerAttackTable(new PlayerAttackData(){PlayerIDs = new string[]{},Damage = new int[]{}});
            PlayerHealthTable = new PlayerHealthTable(new PlayerHealthData(){PlayerIDs = new string[]{}, Hp = new int[]{}, IsDead = new bool[]{}});
            PlayerCombatTable = new PlayerCombatTable(new PlayerCombatData(){AttackerIDs = new string[]{},TargetIDs = new string[]{}});
            PlayerPositionTable = new PlayerPositionTable(new PlayerPositionData()
                { PlayerIDs = new string[] { }, Positions = new Vector3[] { } });
        }
    }
}