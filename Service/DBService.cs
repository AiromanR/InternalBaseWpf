using InternalBaseWpf.Data;

namespace InternalBaseWpf.Service
{
    public class DBService
    {
        private static DBService? _instance;
        public static DBService Instance => _instance ??= new DBService();
        public AppDbContext Context { get; }
        private DBService() => Context = new AppDbContext();
    }
}
