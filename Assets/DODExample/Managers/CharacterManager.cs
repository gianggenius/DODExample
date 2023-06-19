using System;
using System.Collections;
using System.Collections.Generic;
using DODExample;
using DODExample.Components;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject testPrefab;
    [SerializeField] private Transform  parentHolder;

    private List<PlayerEntity> _players;

    private void Start()
    {
        _players = new();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var player = Instantiate(testPrefab, parentHolder).GetComponent<PlayerEntity>();
                player.transform.position = new Vector3(i * 2, 1, j * 2);
                player.UniqueId           = Guid.NewGuid().ToString();
                DatabaseManager.Instance.PlayerHealthTable.AddRecord(new PlayerHealthRecord()
                    { PlayerID = player.UniqueId, Hp = 100000, IsDead = false });
                DatabaseManager.Instance.PlayerAttackTable.AddRecord(new PlayerAttackRecord()
                    { PlayerID = player.UniqueId, Damage = 1 });
                DatabaseManager.Instance.PlayerPositionTable.AddRecord(new PlayerPositionRecord()
                    { PlayerID = player.UniqueId, Position = player.transform.position });
                _players.Add(player);
            }
        }
    }

    private void Update()
    {
        foreach (var player in _players)
        {
            // Player attack another random player
            var randomPlayer = _players[UnityEngine.Random.Range(0, _players.Count)];
            DatabaseManager.Instance.PlayerCombatTable.AddRecord(new PlayerCombatRecord()
                { AttackerID = player.UniqueId, TargetID = randomPlayer.UniqueId });
        }
    }
}