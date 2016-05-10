using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Threading.Tasks; 4.5 only
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public class clsEventGame : clsBase, intBase
    {
        private int _eventID, _gameID;

        // numeric properties
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
        public int GameID
        {
            get
            {
                return _gameID;
            }
            set
            {
                _gameID = value;
            }
        }
        

        // constructors
        public clsEventGame() : base() {}
        public clsEventGame(SqlDataReader dr) : base(dr){}
        public clsEventGame(int id) : base(id) { }

        // generic methods
        // convert a generic object to a typed one
        private static clsEventGame get(string sqlQuery)
        {
            return (clsEventGame)get(sqlQuery, typeof(clsEventGame));
        }


        // convert a generic list to a typed list
        private static List<clsEventGame> getList(string sqlQuery)
        {
            List<object> objs = getList(sqlQuery, typeof(clsEventGame));

            List<clsEventGame> results = new List<clsEventGame>();
            foreach (object obj in objs)
            {
                clsEventGame result = new clsEventGame();
                result.populateByObject(obj);
                results.Add(result);
            }
            return results;
        }


        // methods
        public static clsEventGame get(int id)
        {
            return get("SELECT * FROM EventGames WHERE ID = " + id);
        }

        public static List<clsEventGame> getByEventID(int eventID)
        {
            return getList("SELECT * FROM EventGames WHERE EventID = " + eventID);
        }

        public static List<clsEventGame> getByGameID(int gameID)
        {
            return getList("SELECT * FROM EventGames WHERE GameID = " + gameID);
        }
    }
}
