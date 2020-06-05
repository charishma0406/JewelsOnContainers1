using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    //for migrating our data to data base
    public class MigrateDataBase
    {
        public static void EnsureCreated(OrdersContext context)
        {
            context.Database.Migrate();
        }
    }
}
