namespace DODExample
{
    public abstract class BaseSystem
    {
        protected DatabaseManager _databaseManager;

        protected BaseSystem()
        {
            _databaseManager = DatabaseManager.Instance;
        }

        public abstract void Update();

        protected abstract void Process();
    }
}