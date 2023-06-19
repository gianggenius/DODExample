using System;

namespace DODExample
{
    public class PlayerCombatTable:BaseTable<PlayerCombatData, PlayerCombatRecord>
    {
        public PlayerCombatTable(PlayerCombatData data) : base(data)
        {
        }
    }
}