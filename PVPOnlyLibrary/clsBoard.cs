using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum OwnerType { General, Group, Event, Game, Genere, Map }
    

    public class clsBoard : clsBase, intBase
    {
        private int _ownerID; 
        private string _description, _notes, _name;
        private OwnerType _ownerType;

        // numeric properties
        public int OwnerID
        {
            get
            {
                return _ownerID;
            }
            set
            {
                _ownerID = value;
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

        // constructors
        public clsBoard() : base() {}
        public clsBoard(SqlDataReader dr) : base(dr){}
        public clsBoard(int id) : base(id) { }


        // enumerator methods
        public static OwnerType getOwnerType(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            OwnerType result = (OwnerType)Enum.Parse(typeof(OwnerType), name, true);
            return result;
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsBoard get(string sqlQuery)
        {
            return (clsBoard)get(sqlQuery, typeof(clsBoard));
        }


        // convert a generic list to a typed list
        private static List<clsBoard> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsBoard));

            List<clsBoard> results = new List<clsBoard>();
            foreach (object obj in objs)
            {
                clsBoard result = new clsBoard();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

       
        public static List<clsBoard> get()
        {
            return getList("SELECT * FROM Boards");
        }

        public static List<clsBoard> getByOwnerType(OwnerType ownerType)
        {
            return getList("SELECT * FROM Boards WHERE OwnerType = '" + ownerType.ToString() + "'");
        }

    }
}
