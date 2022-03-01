using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buycoins_test.Models
{
    public class DataContext : DbContext
    {
       
        public DbSet<UserBankInfo> UserBankInfos { get; set; }

        public string DbPath { get; }

        public DataContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "userData.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite($"Data Source={DbPath}");
    }
}
