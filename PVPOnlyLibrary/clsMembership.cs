using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum MembershipStatus { Volunteered, Invited, Member, None }

    public class clsMembership : clsBase, intBase
    {
        private int _accountID, _groupID, _creatorID;
        private string _serialNumber, _class;
        private MembershipStatus _status;


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
        public int GroupID
        {
            get
            {
                return _groupID;
            }
            set
            {
                _groupID = value;
            }
        }
        public int CreatorID
        {
            get
            {
                return _creatorID;
            }
            set
            {
                _creatorID = value;
            }
        }

        // string properties
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
            }
        }
        public string Class
        {
            get
            {
                return _class;
            }
            set
            {
                _class = value;
            }
        }
        public MembershipStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        

        // constructors
        public clsMembership() : base() {}
        public clsMembership(SqlDataReader dr) : base(dr){}
        public clsMembership(int id) : base(id) { }

        // enumerator methods
        public static MembershipStatus getMembershipStatus(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            MembershipStatus result = (MembershipStatus)Enum.Parse(typeof(MembershipStatus), name, true);
            return result;
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsMembership get(string sqlQuery)
        {
            return (clsMembership)get(sqlQuery, typeof(clsMembership));
        }


        // convert a generic list to a typed list
        private static List<clsMembership> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsMembership));

            List<clsMembership> results = new List<clsMembership>();
            foreach (object obj in objs)
            {
                clsMembership result = new clsMembership();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        public static clsMembership get(int id)
        {
            return get("SELECT * FROM Memberships WHERE ID = " + id);
        }

        public static List<clsMembership> getByAccountID(int accountID)
        {
            return getList("SELECT * FROM Memberships WHERE AccountID = " + accountID);
        }

        public static List<clsMembership> getByGroupID(int groupID)
        {
            return getList("SELECT * FROM Memberships WHERE GroupID = " + groupID);
        }
    }
}