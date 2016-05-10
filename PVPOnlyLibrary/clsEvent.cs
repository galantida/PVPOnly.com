using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum Time { Future, Present, Past }

    public class clsEvent : clsBase, intBase
    {
        private int _groupID, _creatorID;
        private string _name, _description, _location, _IPAddress, _notes;
        private bool _invitationOnly;
        private DateTime _eventStart, _eventEnd;

        // numeric properties
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
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        public string IPAddress
        {
            get
            {
                return _IPAddress;
            }
            set
            {
                _IPAddress = value;
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

        // date time properties
        public DateTime EventStart
        {
            get
            {
                return _eventStart;
            }
            set
            {
                _eventStart = value;
            }
        }
        public DateTime EventEnd
        {
            get
            {
                return _eventEnd;
            }
            set
            {
                _eventEnd = value;
            }
        }
        

        // constructors
        public clsEvent() : base() {}
        public clsEvent(SqlDataReader dr) : base(dr){}
        public clsEvent(int id) : base(id) { }

        // enumerator methods
        public static Time getTime(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            Time result = (Time)Enum.Parse(typeof(Time), name, true);
            return result;
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsEvent get(string sqlQuery)
        {
            return (clsEvent)get(sqlQuery, typeof(clsEvent));
        }


        // convert a generic list to a typed list
        private static List<clsEvent> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsEvent));

            List<clsEvent> results = new List<clsEvent>();
            foreach (object obj in objs)
            {
                clsEvent result = new clsEvent();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        // convert typed list to JSON array of objects
        public static string JSON(List<clsEvent> events)
        {
            string delimiter = "";
            string json = "[";
            foreach (clsEvent obj in events)
            {
                json += delimiter + obj.JSON();
                delimiter = ",";
            }
            return json + "]";
        }

        //public static clsEvent get(int id)
        //{
        //    return get("SELECT * FROM Events WHERE ID = " + id);
        //}

        public static string get(int id)
        {
            string sql = "";
            sql += " SELECT Events.*, Groups.Name as GroupName";
            sql += " FROM Events";
            sql += " JOIN Groups ON Events.GroupID = Groups.ID";
            sql += " WHERE Events.ID = " + id;
            return clsEvent.JSON(sql);
        }

        public static string getByAttendeeID(int attendeeID)
        {
            string sql = "";
            sql += " SELECT Events.*, Events.ID as EventID, Events.Name as EventName, Attendees.Attendance, Attendees.RSVP, Attendees.Coordinator, Groups.Name as GroupName, Groups.ID as GroupID";
            sql += " FROM Events";
            sql += " JOIN Attendees ON Events.ID = Attendees.EventID";
            sql += " JOIN Groups ON Events.GroupID = Groups.ID";
            sql += " WHERE Attendees.ID = " + attendeeID;
            sql += " ORDER BY Events.EventStart DESC";
            return clsEvent.JSON(sql);
        }
    }
}