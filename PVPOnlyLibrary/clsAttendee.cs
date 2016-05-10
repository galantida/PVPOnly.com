using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum RSVP { Invited, Accepted, Declined, Volunteered, None }
    public enum Attendance { Attended, Virtual, None }

    public class clsAttendee : clsBase, intBase
    {
        private int _accountID, _eventID;
        
        private bool _coordinator;
        private RSVP _RSVP;
        private Attendance _attendance;

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
        public int EventID
        {
            get
            {
                return _eventID;
            }
            set
            {
                _eventID = value;
            }
        }
        // string properties
        public RSVP RSVP
        {
            get
            {
                return _RSVP;
            }
            set
            {
                _RSVP = value;
            }
        }
        public Attendance Attendance
        {
            get
            {
                return _attendance;
            }
            set
            {
                _attendance = value;
            }
        }
        // bool properties
        public bool Coordinator
        {
            get
            {
                return _coordinator;
            }
            set
            {
                _coordinator = value;
            }
        }


        // constructors
        public clsAttendee() : base() {}
        public clsAttendee(SqlDataReader dr) : base(dr){}
        public clsAttendee(int id) : base(id) {}
        public clsAttendee(int eventID, int accountID)
        {
            this.load("SELECT * FROM Attendees WHERE EventID = " + eventID + " AND AccountID = " + AccountID);
        }

        // enumerator methods
        public static RSVP getRSVP(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            RSVP result = (RSVP)Enum.Parse(typeof(RSVP), name, true);
            return result;
        }

        public static Attendance getAttendance(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            Attendance result = (Attendance)Enum.Parse(typeof(Attendance), name, true);
            return result;
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsAttendee get(string sqlQuery)
        {
            return (clsAttendee)get(sqlQuery, typeof(clsAttendee));
        }


        // convert a generic list to a typed list
        private static List<clsAttendee> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsAttendee));

            List<clsAttendee> results = new List<clsAttendee>();
            foreach (object obj in objs)
            {
                clsAttendee result = new clsAttendee();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        // get methods
        public static clsAttendee get(int id)
        {
            return (clsAttendee)get("SELECT * FROM Attendees WHERE ID = " + id, typeof(clsAttendee));
        }

        public static string getByAccountID(int accountID)
        {
            string sql = "";
            sql += " SELECT Attendees.*,Events.ID as EventID, Events.Name as EventName, Events.EventStart, Events.EventEnd, Groups.Name as GroupName";
            sql += " FROM Attendees";
            sql += " JOIN Events ON Attendees.EventID = Events.ID";
            sql += " JOIN Groups ON Events.GroupID = Groups.ID";
            sql += " WHERE Attendees.AccountID = " + accountID;
            sql += " ORDER BY Events.EventStart DESC";
            return clsAttendee.JSON(sql);
        }

        public static string getByEventID(int eventID)
        {
            string sql = "";
            sql += " SELECT Attendees.*, Accounts.CallSign, Events.EventStart, Events.EventEnd";
            sql += " FROM Attendees";
            sql += " JOIN Accounts ON Attendees.AccountID = Accounts.ID";
            sql += " JOIN Events ON Attendees.EventID = Events.ID";
            sql += " WHERE Attendees.EventID = " + eventID;
            sql += " ORDER BY Accounts.CallSign DESC";
            return clsAttendee.JSON(sql);
        }

        public static string updateRSVP(int attendeeID, RSVP RSVP)
        {
            string sql = "";
            sql += " Update Attendees";
            sql += " set RSVP = '" + RSVP.ToString() + "'";
            sql += " WHERE Attendees.ID = " + attendeeID;
            return clsAttendee.JSON(sql);
        }

        public static string updateAttendance(int attendeeID, Attendance attendance)
        {
            string sql = "";
            sql += " Update Attendees";
            sql += " set Attendance = '" + attendance.ToString() + "'";
            sql += " WHERE Attendees.ID = " + attendeeID;
            return clsAttendee.JSON(sql);
        }
    }
}
