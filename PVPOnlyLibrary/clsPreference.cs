using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{

    public class clsPreference : clsBase, intBase
    {
        private int _accountID;
        private string _name, _value, _ownerName;
        private OwnerType _ownerType;


        // numeric properties
        public int AccountID
        {
            get
            {
                return _accountID;
            }
            set
            {
                _accountID = value;
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
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public string OwnerName
        {
            get
            {
                return _ownerName;
            }
            set
            {
                _ownerName = value;
            }
        }
        public OwnerType OwnerType
        {
            get
            {
                return _ownerType;
            }
            set
            {
                _ownerType = value;
            }
        }
                

        // constructors
        public clsPreference() : base() {}
        public clsPreference(SqlDataReader dr) : base(dr){}
        public clsPreference(int id) : base(id) { }

        // generic methods
        // convert a generic object to a typed one
        private static clsPreference get(string sqlQuery)
        {
            return (clsPreference)get(sqlQuery, typeof(clsPreference));
        }


        // convert a generic list to a typed list
        private static List<clsPreference> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsPreference));

            List<clsPreference> results = new List<clsPreference>();
            foreach (object obj in objs)
            {
                clsPreference result = new clsPreference();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }


        // methods
        public static clsPreference get(int id)
        {
            return get("SELECT * FROM Preferences WHERE ID = " + id);
        }

        public static List<clsPreference> getByName(string name)
        {
            return getList("SELECT * FROM Preferences WHERE Name = '" + name + "'");
        }

        public static clsPreference getByAccountID(int accountID, string name)
        {
            return get("SELECT * FROM Preferences WHERE AccountID = " + accountID + " AND Name = '" + name + "'");
        }
    }
}