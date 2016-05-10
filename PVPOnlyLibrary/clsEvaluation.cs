using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Threading.Tasks; 4.5 only
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum ContentType { File, Post };
    public enum Result { Like, Report, None };

    public class clsEvaluation : clsBase, intBase
    {
        private int _accountID, _contentID;
        private ContentType _contentType;
        private Result _result;

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
        public int ContentID
        {
            get
            {
                return _contentID;
            }
            set
            {
                _contentID = value;
            }
        }
        // string properties
        public ContentType ContentType
        {
            get
            {
                return _contentType;
            }
            set
            {
                _contentType = value;
            }
        }
        public Result Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        // constructors
        public clsEvaluation() : base() {}
        public clsEvaluation(SqlDataReader dr) : base(dr){}
        public clsEvaluation(int id) : base(id) { }

        // enumerator methods
        public static ContentType getOwnerType(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            ContentType result = (ContentType)Enum.Parse(typeof(ContentType), name, true);
            return result;
        }

        public static Result getResult(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            Result result = (Result)Enum.Parse(typeof(Result), name, true);
            return result;
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsEvaluation get(string sqlQuery)
        {
            return (clsEvaluation)get(sqlQuery, typeof(clsEvaluation));
        }


        // convert a generic list to a typed list
        private static List<clsEvaluation> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsEvaluation));

            List<clsEvaluation> results = new List<clsEvaluation>();
            foreach (object obj in objs)
            {
                clsEvaluation result = new clsEvaluation();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        public static List<clsEvaluation> getByContentID(int contentID)
        {
            return clsEvaluation.getList("SELECT * FROM Evaluations WHERE ContentID = " + contentID);
        }

        public static List<clsEvaluation> getByAccountID(int accountID)
        {
            return clsEvaluation.getList("SELECT * FROM Evaluations WHERE AccountID = " + accountID);
        }
    }
}
