using Expectativa_do_Mercado_Mensal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expectativa_do_Mercado_Mensal.Service
{
    internal class ExpectativasContext:DbContext
    {
        public DbSet<ExpectativasMercado>Expectativas{  get; set; }
       public ExpectativasContext() : base("name=ExpectativasContext")
        {

        }
        public bool IsDatabaseConnected()
        {
            try
            {
                Database.Connection.Open();
                Database.Connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
