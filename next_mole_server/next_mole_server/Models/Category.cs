using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using next_mole_server.Models.DAL;

namespace AdminPage.Models
{
    public class Category
    {
        int id;
        public int Id {
            get { return id; }
            set { id = value; }
        }

        string name;
        public string Name {
            get { return name; }
            set { name = value; }
        }

        int coiceCounter;
        public int CoiceCounter {
            get { return coiceCounter; }
            set { coiceCounter = value; }
        }

        int verteciesInCat;
        public int VerteciesInCat {
            get { return verteciesInCat; }
            set { verteciesInCat = value; }
        }

        int edgesInCat;
        public int EdgesInCat
        {
            get { return edgesInCat; }
            set { edgesInCat = value; }
        }

        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public Category()
        {

        }

        public List<Category> Read()
        {
            DBservices dbs = new DBservices();
            List<Category> lc = dbs.GetCategories2("DBConnectionString", "Categories");
            return lc;

        }
        public List<Category> ReadCat()
        {
            DBservices dbs = new DBservices();
            List<string> lc = dbs.GetCategories("DBConnectionString", "Categories");
            List<Category> lisy = dbs.GetAmountForCategories("DBConnectionString", lc);
            return lisy;

        }
    }
}