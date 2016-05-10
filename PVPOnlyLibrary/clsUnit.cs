using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public class clsUnit : clsBase, intBase
    {
        private int _size;
        private string _name, _description;

        // numeric properties
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        // string properties
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }


        // constructors
        public clsUnit() : base() {}
        public clsUnit(SqlDataReader dr) : base(dr){}
        public clsUnit(int id) : base(id) { }

        // generic methods
        // convert a generic object to a typed one
        private static clsUnit get(string sqlQuery)
        {
            return (clsUnit)get(sqlQuery, typeof(clsUnit));
        }


        // convert a generic list to a typed list
        private static List<clsUnit> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsUnit));

            List<clsUnit> results = new List<clsUnit>();
            foreach (object obj in objs)
            {
                clsUnit result = new clsUnit();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }


        // methods
        public static clsUnit get(int id)
        {
            return get("SELECT * FROM Units WHERE ID = " + id);
        }

        public static clsUnit getByName(string name)
        {
            return get("SELECT * FROM Units WHERE Name = '" + name + "'");
        }

        public static List<clsUnit> get()
        {
            return getList("SELECT * FROM Units ORDER BY Size DESC");
        }
    }
}