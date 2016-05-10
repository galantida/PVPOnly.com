using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum GroupType { Group, Clan, Guild }

    public class clsGroup : clsBase, intBase
    {
        private int _creatorID, _logoImageFileID;
        private string _name, _description, _slogan, _tag, _email, _notes;
        private bool _invitationOnly;
        private GroupType _type;

        // numeric properties
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
        public int LogoImageFileID
        {
            get
            {
                return _logoImageFileID;
            }
            set
            {
                _logoImageFileID = value;
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
        public GroupType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        public string Slogan
        {
            get
            {
                return _slogan;
            }
            set
            {
                _slogan = value;
            }
        }
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        public string eMail
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
            }
        }

        // bool properties
        public bool InvitationOnly
        {
            get
            {
                return _invitationOnly;
            }
            set
            {
                _invitationOnly = value;
            }
        }
        

        // constructors
        public clsGroup() : base() {}
        public clsGroup(SqlDataReader dr) : base(dr){}
        public clsGroup(int id) : base(id) { }


        // enumerator methods
        public static GroupType getGroupType(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            GroupType result = (GroupType)Enum.Parse(typeof(GroupType), name, true);
            return result;
        }

        // generic methods
        // interface for the generic getQuery
        private static clsGroup get(string sqlQuery)
        {
            return (clsGroup)get(sqlQuery, typeof(clsGroup));
        }


        // interface for the generic getListQuery
        private static List<clsGroup> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsGroup));

            List<clsGroup> results = new List<clsGroup>();
            foreach (object obj in objs)
            {
                clsGroup result = new clsGroup();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;            
        }

        // convert typed list to JSON array of objects
        public static string JSON(List<clsGroup> groups)
        {
            string delimiter = "";
            string json = "[";
            foreach (clsGroup obj in groups)
            {
                json += delimiter + obj.JSON();
                delimiter = ",";
            }
            return json + "]";
        }

        public static clsGroup get(int id)
        {
            return get("SELECT * FROM Groups WHERE ID = " + id);
        }

        public static List<clsGroup> getByAccountID(int accountID)
        {
            string sql = "";
            sql += " SELECT Groups.*";
            sql += " FROM Memberships";
            sql += " JOIN Groups";
            sql += " ON Memberships.GroupID = Groups.ID";
            sql += " WHERE Memberships.AccountID = " + accountID;
            sql += " ORDER BY Groups.Name ASC";
            return getList(sql);
        }

        // get all groups that this account ID is a member of
        public static string getMemberships(int accountID)
        {
            string sql = "";
            sql += " SELECT Memberships.*, Groups.Name, Groups.Tag";
            sql += " FROM Memberships";
            sql += " JOIN Groups";
            sql += " ON Memberships.GroupID = Groups.ID";
            sql += " WHERE Memberships.AccountID = " + accountID;
            sql += " ORDER BY Groups.Name ASC";
            return clsMessage.JSON(sql);
        }

        // get all members of a particular group
        public static string getMembers(int groupID)
        {
            string sql = "";
            sql += " SELECT Memberships.*, Accounts.CallSign, Groups.Name, Groups.Tag";
            sql += " FROM Memberships";
            sql += " JOIN Groups ON Memberships.GroupID = Groups.ID";
            sql += " JOIN Accounts ON Memberships.AccountID = Accounts.ID";
            sql += " WHERE Memberships.GroupID = " + groupID;
            sql += " ORDER BY Groups.Name ASC";
            return clsMessage.JSON(sql);
        }
    }
}