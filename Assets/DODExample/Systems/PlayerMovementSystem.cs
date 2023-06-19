using UnityEngine;

namespace DODExample
{
    public class PlayerMovementSystem : BaseSystem
    {
        public override void Update()
        {
            Process();
        }

        protected override void Process()
        {
            var table = _databaseManager.PlayerPositionTable;
            for (var i = 0; i < table.Length; i++)
            {
                var record = table.GetRecordByIndex(i);
                record.Position.x += i % 2 == 0 ? 0.1f : -0.1f;
                if (Mathf.Abs(record.Position.x) > 10)
                {
                    record.Position.x = 0;
                }
                _databaseManager.PlayerPositionTable.SetPositionByIndex(i, record.Position);
            }
        }
    }
}