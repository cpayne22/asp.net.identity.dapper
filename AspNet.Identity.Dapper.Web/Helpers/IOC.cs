using AspNet.Identity.Dapper.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.Identity.Dapper.Web
{
    public class IOC
    {
        private static IDB db;
        public static IDB GetDB()
        {
            if (db == null)
            {
                db = new SQLDB();
            }

            return db;
        }
    }
}