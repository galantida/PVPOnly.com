using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public class clsPath : clsBase, intBase
    {
        private int _pathOrder;
        private string _name, _description;

        // numeric properties
        public int PathOrder
        {
            get
            {
                return _pathOrder;
            }
            set
            {
                _pathOrder = value;
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
        public clsPath() : base() {}
        public clsPath(SqlDataReader dr) : base(dr){}
        public clsPath(int id) : base(id) { }

        // generic methods
        // convert a generic object to a typed one
        private static clsPath get(string sqlQuery)
        {
            return (clsPath)get(sqlQuery, typeof(clsPath));
        }


        // convert a generic list to a typed list
        private static List<clsPath> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsPath));

            List<clsPath> results = new List<clsPath>();
            foreach (object obj in objs)
            {
                clsPath result = new clsPath();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }


        // methods
        public static clsPath get(int id)
        {
            return get("SELECT * FROM Paths WHERE ID = " + id);
        }

        public static clsPath getByName(string name)
        {
            return get("SELECT * FROM Paths WHERE Name = '" + name + "'");

        }

        public static List<clsPath> get()
        {
            return getList("SELECT * FROM Paths ORDER BY PathOrder DESC");
        }
    }
}