using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DODExample
{
    public class SystemManager : MonoBehaviour
    {
        #region Singleton

        private static SystemManager _instance;
        public static  SystemManager Instance => _instance;
    
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }else
            {
                Destroy(gameObject);
            }
        }
    
        #endregion

        private List<BaseSystem> _systems;

        private async void OnEnable()
        {
            _systems = new List<BaseSystem>();
            while (DatabaseManager.Instance == null)
            {
                await Task.Delay(200);
            }
            InitializeSystems();
        }

        private void Update()
        {
            foreach (var system in _systems)
            {
                system.Update();
            }
        }

        private void InitializeSystems()
        {
            _systems.Add(new PlayerCombatSystem());
            _systems.Add(new PlayerDeadSystem());
            _systems.Add(new PlayerMovementSystem());
        }
    }
}
