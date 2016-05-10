using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public enum ContentIdentifier { Concept_Art, Group_Logo, GameBox_Pic, Photograph, Profile_Pic, Replay, ScreenShot, Tactical_View }

    public class clsFile : clsBase, intBase
    {
        private string _fileName;
        private ContentIdentifier _contentType;

        // string properties
        public ContentIdentifier ContentType
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

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        // constructors
        public clsFile() : base() {}
        public clsFile(SqlDataReader dr) : base(dr){}
        public clsFile(int id) : base(id) {}


        // enumerator methods
        public static ContentIdentifier getContentIdentifer(string name)
        {
            name = name.Replace(" ", "_"); // string version may or may not have spaces (enum does not)
            ContentIdentifier result = (ContentIdentifier)Enum.Parse(typeof(ContentIdentifier), name, true);
            return result;
        }

        // generic methods
        // convert a generic object to a typed one
        private static clsFile get(string sqlQuery)
        {
            return (clsFile)get(sqlQuery, typeof(clsFile));
        }


        // convert a generic list to a typed list
        private static List<clsFile> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsFile));

            List<clsFile> results = new List<clsFile>();
            foreach (object obj in objs)
            {
                clsFile result = new clsFile();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }

        public static clsFile get(int id)
        {
            return get("SELECT * FROM Files WHERE ID = " + id);
        }

        public static List<clsFile> get(ContentIdentifier contentType)
        {
            return getList("SELECT * FROM Files WHERE ContentType = '" + contentType + "'");
        }
    }
}