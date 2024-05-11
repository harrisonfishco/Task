namespace Task
{
    public class DatabaseService
    {
        private string ConnectionString { get; }
        public DatabaseService(string connectionString) 
        {
            ConnectionString = connectionString;
        }
    }
}
