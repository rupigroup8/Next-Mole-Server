using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using next_mole_server.Models.DAL;

namespace next_mole_server.Models
{
    public class Table
    {
        string name;
        public string Name { get => name; set => name = value; }

        public static string createTable(string tableName)
        {
            string res;
            int num;
            DBservices dbs = new DBservices();
            bool isExist = dbs.checkCategory(tableName);
            if (!isExist)
            {
                 num = dbs.DBcreateTable(tableName);
                //return res;
            }
            else
            {
                num = 0;
                //return 0;
            }
            res = num + " categories added";
            return res;
        }
    }
}