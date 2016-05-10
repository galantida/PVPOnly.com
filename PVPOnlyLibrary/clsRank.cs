using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public class clsRank : clsBase, intBase
    {
        private int _pathID, _rankOrder;
        private string _name, _abbreviation, _address, _description;


        // numeric properties
        public int PathID
        {
            get
            {
                return _pathID;
            }
            set
            {
                _pathID = value;
            }
        }
        public int RankOrder
        {
            get
            {
                return _rankOrder;
            }
            set
            {
                _rankOrder = value;
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
        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                _abbreviation = value;
            }
        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
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
        public clsRank() : base() {}
        public clsRank(SqlDataReader dr) : base(dr){}
        public clsRank(int id) : base(id) { }


        // generic methods
        // convert a generic object to a typed one
        private static clsRank get(string sqlQuery)
        {
            return (clsRank)get(sqlQuery, typeof(clsRank));
        }


        // convert a generic list to a typed list
        private static List<clsRank> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsRank));

            List<clsRank> results = new List<clsRank>();
            foreach (object obj in objs)
            {
                clsRank result = new clsRank();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }


        // methods
        public static clsRank get(int id)
        {
            return get("SELECT * FROM Ranks WHERE ID = " + id);
        }

        public static clsRank getByName(string name)
        {
            return get("SELECT * FROM Ranks WHERE Name = '" + name + "'");
        }

        public static List<clsRank> get()
        {
            return getList("SELECT * FROM Ranks ORDER BY RankOrder DESC");
        }
    }
}