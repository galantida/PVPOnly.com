using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    // might make this in to groupable chat. Isnt that what a board is for?

    public class clsMessage : clsBase, intBase
    {
        private int _toAccountID, _fromAccountID;
        private string _message;
        private bool _messageRead, _HTML;

        // numeric properties
        public int ToAccountID
        {
            get
            {
                return _toAccountID;
            }
            set
            {
                _toAccountID = value;
            }
        }
        public int FromAccountID
        {
            get
            {
                return _fromAccountID;
            }
            set
            {
                _fromAccountID = value;
            }
        }

        // string properties
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        // bool properties
        public bool HTML
        {
            get
            {
                return _HTML;
            }
            set
            {
                _HTML = value;
            }
        }
        public bool MessageRead
        {
            get
            {
                return _messageRead;
            }
            set
            {
                _messageRead = value;
            }
        }
        

        // constructors
        public clsMessage() : base() {}
        public clsMessage(SqlDataReader dr) : base(dr){}
        public clsMessage(int id) : base(id) { }


        // generic methods
        // convert a generic object to a typed one
        private static clsMessage get(string sqlQuery)
        {
            return (clsMessage)get(sqlQuery, typeof(clsMessage));
        }


        // convert a generic list to a typed list
        private static List<clsMessage> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsMessage));

            List<clsMessage> results = new List<clsMessage>();
            foreach (object obj in objs)
            {
                clsMessage result = new clsMessage();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        // methods
        public static clsMessage get(int id)
        {
            return get("SELECT * FROM Messages WHERE ID = " + id);
        }

        public static List<clsMessage> getByFromAccountID(int fromAccountID)
        {
            return getList("SELECT * FROM Messages WHERE FromAccountID = " + fromAccountID);
        }

        public static string getByToAccountID(int toAccountID)
        {
            string sql = "";
            sql += " SELECT TOP 10 Derived.FromAccountID AS FromAccountID, FromAccounts.CallSign AS FromCallSign,";
            sql += " Derived.ToAccountID AS ToAccountID, ToAccounts.CallSign AS ToCallSign, ";
            sql += " Messages.Message, Messages.MessageRead, Messages.Created";
            sql += " FROM (";
            sql += " SELECT ToAccountID, FromAccountID, MAX(Messages.ID) AS LatestMessageID";
            sql += " FROM Messages";
            sql += " WHERE " + toAccountID + " IN (ToAccountID, FromAccountID)";
            sql += " GROUP BY ToAccountID, FromAccountID";
            sql += " ) Derived";
            sql += " INNER JOIN Messages ON Messages.ID = Derived.LatestMessageID";
            sql += " LEFT OUTER JOIN Accounts ToAccounts ON ToAccounts.ID = Derived.ToAccountID";
            sql += " LEFT OUTER JOIN Accounts FromAccounts ON FromAccounts.ID = Derived.FromAccountID";
            sql += " ORDER BY Created DESC";
            return clsMessage.JSON(sql);
        }
    }
}