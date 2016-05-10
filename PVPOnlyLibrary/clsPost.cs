using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum PostType { General, Game, Genere, Map, Description, Strategy, Support, Tip, Bug_Report, Bug_Resolution, Feature_Request};

    public class clsPost : clsBase, intBase
    {
        private int _boardID, _parentID, _accountID, _postFileID;
        private string _subject, _body;
        private PostType _postType;

        // numeric properties
        public int BoardID
        {
            get
            {
                return _boardID;
            }
            set
            {
                _boardID = value;
            }
        }
        public int ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                _parentID = value;
            }
        }
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
        public int PostFileID
        {
            get
            {
                return _postFileID;
            }
            set
            {
                _postFileID = value;
            }
        }

        // string properties
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }
        public PostType PostType
        {
            get
            {
                return _postType;
            }
            set
            {
                _postType = value;
            }
        }


        // constructors
        public clsPost() : base() {}
        public clsPost(SqlDataReader dr) : base(dr){}
        public clsPost(int id) : base(id) { }


        // enumerator methods
        public static PostType getPostType(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            PostType result = (PostType)Enum.Parse(typeof(PostType), name, true);
            return result;
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsPost get(string sqlQuery)
        {
            return (clsPost)get(sqlQuery, typeof(clsPost));
        }


        // convert a generic list to a typed list
        private static List<clsPost> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsPost));

            List<clsPost> results = new List<clsPost>();
            foreach (object obj in objs)
            {
                clsPost result = new clsPost();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        public static clsPost get(int id)
        {
            return get("SELECT * FROM Posts WHERE ID = " + id);
        }

        public static List<clsPost> getByBoardID(int boardID)
        {
            return getList("SELECT * FROM Posts WHERE BoardID = " + boardID);
        }

        public static List<clsPost> getByParentID(int parentID)
        {
            return getList("SELECT * FROM Posts WHERE parentID = " + parentID);
        }

        public static List<clsPost> getByAccountID(int accountID)
        {
            return getList("SELECT * FROM Posts WHERE AccountID = " + accountID);
        }



    }
}