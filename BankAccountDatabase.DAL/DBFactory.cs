using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankAccountDatabase.DAL
{
    public class DBFactory
    {
        private readonly IConfigurationRoot Config;
        private readonly bool Test;

        public DBFactory(IConfigurationRoot config, bool test = false)
        {
            Config = config;
            Test = test;
        }

        public AppDbContext GetDbContext()
        {
            string environment = Test ? "Test" : "Prod";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(Config[$"ConnectionStrings:{environment}"])
                .Options;
            return new AppDbContext(options);
        }
    }
}
