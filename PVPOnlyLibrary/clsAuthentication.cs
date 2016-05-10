using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{

    public class clsAuthentication : clsBase, intBase
    {
        private string _identifier; 
        private DateTime _time; 
        
        // string properties
        public string Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                _identifier = value;
            }
        }

        // date time properties
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }


        // constructors
        private clsAuthentication() : base() {}
        public clsAuthentication(SqlDataReader dr) : base(dr){}
        public clsAuthentication(int id) : base(id) { }


        public clsAuthentication(string identifier) : base()
        {
            if (this.load("SELECT * FROM Authentications WHERE Identifier = '" + identifier + "'") != true)
            {
                // this is a new one
                _identifier = identifier;
                _time = DateTime.Now;
                this.save();
            }
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsAuthentication get(string sqlQuery)
        {
            return (clsAuthentication)get(sqlQuery, typeof(clsAuthentication));
        }


        // convert a generic list to a typed list
        private static List<clsAuthentication> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsAuthentication));

            List<clsAuthentication> results = new List<clsAuthentication>();
            foreach (object obj in objs)
            {
                clsAuthentication result = new clsAuthentication();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        // add time to this auths ban and save it
        public void addTime(int seconds)
        {
            this.Time.AddSeconds(seconds);
            this.save();
        }

        // check if this auth is banned
        public bool isBanned(int seconds)
        {
            if (this.Time > DateTime.Now.AddSeconds(seconds)) return true;
            else return false;
        }

        public static clsAuthentication get(int id)
        {
            return get("SELECT * FROM Authentications WHERE ID = " + id);
        }

        public static clsAuthentication getByIdentifier(string identifier)
        {
            return get("SELECT * FROM Authentications WHERE Identifier = " + identifier);
        }

        public bool reset(string identifier)
        {
            this.ID = 0; // clear this object
            if (execute("DELETE FROM Authentications WHERE Identifier = '" + identifier + "'") > 0) return true;
            else return false;
        }

        public static List<clsAuthentication> getBanned(int seconds)
        {
            return getList("SELECT * FROM Authentications WHERE Time > '" + DateTime.Now.AddSeconds(seconds) + "'");
        }
    }
}
