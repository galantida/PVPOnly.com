using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum EntryType { Request, Response, Action, Error, Warning, Protected, SendEmail }

    // to-do
    // include account id that created the event unless anonomous
    public class clsEventLog : clsBase, intBase
    {
        private EntryType _entryType;
        private string _entryText, _objectName, _extraInfo;

        // string properties
        public EntryType EntryType
        {
            get
            {
                return _entryType;
            }
            set
            {
                _entryType = value;
            }
        }

        public string EntryText
        {
            get
            {
                return _entryText;
            }
            set
            {
                _entryText = value;
            }
        }

        public string ObjectName
        {
            get
            {
                return _objectName;
            }
            set
            {
                _objectName = value;
            }
        }

        public string ExtraInfo
        {
            get
            {
                return _extraInfo;
            }
            set
            {
                _extraInfo = value;
            }
        }

        // constructors
        public clsEventLog() : base() {}
        public clsEventLog(SqlDataReader dr) : base(dr){}
        public clsEventLog(int id) : base(id) { }

        // enumerator methods
        public static EntryType getEntryType(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            EntryType result = (EntryType)Enum.Parse(typeof(EntryType), name, true);
            return result;
        }


        // generic methods
        // convert a generic object to a typed one
        private static clsEventLog get(string sqlQuery)
        {
            return (clsEventLog)get(sqlQuery, typeof(clsEventLog));
        }


        // convert a generic list to a typed list
        private static List<clsEventLog> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsEventLog));

            List<clsEventLog> results = new List<clsEventLog>();
            foreach (object obj in objs)
            {
                clsEventLog result = new clsEventLog();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }


        // web services
        public static clsEventLog get(int id)
        {
            return get("SELECT * FROM EventLogs WHERE ID = " + id);
        }

        public static List<clsEventLog> get()
        {
            return getList("SELECT TOP 50 * FROM EventLogs ORDER BY created DESC");
        }

        public static bool entry(EntryType entryType, string summary, string description, string objectName)
        {
            clsEventLog entry = new clsEventLog();
            entry.EntryType = entryType;
            entry.EntryText = summary;
            entry.ExtraInfo = description;
            entry.ObjectName = objectName;

            // return
            if (entry.save() == 0) return false;
            else return true;
        }
    }
}