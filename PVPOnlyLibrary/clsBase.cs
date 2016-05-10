using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using PVPOnlyLibrary;

namespace PVPOnlyLibrary
{
    public class clsBase
    {
        protected int _id;
        protected DateTime _created, _modified;
        protected string _tableName;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                _created = value;
            }
        }
        public DateTime Modified
        {
            get
            {
                return _modified;
            }
            set
            {
                _modified = value;
            }
        }

        public clsBase()
        {

        }

        public clsBase(SqlDataReader dr)
        {
            this.populateBySqlDataReader(dr);
        }

        public clsBase(int id)
        {
            this.load(id);
        }

        protected string tableName
        {
            get
            {
                // get table name
                string typeName = this.GetType().ToString(); // get the matching object name
                int start = typeName.LastIndexOf(".") + 4; // skip the cls
                int length = typeName.Length - start;
                return typeName.Substring(start, length);
            }
        }

        // return an object based on the contents of a SQLDataReader
        protected static object get(SqlDataReader dr, Type type)
        {
            intBase obj = (intBase)Activator.CreateInstance(type);
            if (dr.HasRows) obj.populateBySqlDataReader(dr);
            return obj;
        }

        // get an object from database based on query
        protected static object get(string sqlQuery, Type type)
        {
            clsDBConnection dbCon = new clsDBConnection();
            dbCon.open();
            object obj = get(dbCon.query(sqlQuery), type);
            dbCon.close();
            return obj;
        }

        // return a list of objects from database based on query
        protected static List<object> getList(string sqlQuery, Type type)
        {
            // get a list of generic objects based on  query
            clsDBConnection dbCon = new clsDBConnection();
            dbCon.open();

            // get sqlDataReader from dbCon and copy it to generic list so we can close the connection
            List<object> results = getList(dbCon.query(sqlQuery), type);

            dbCon.close();
            return results;
        }

        // return a list of objects from database based on the contents of a SQLDataReader
        protected static List<object> getList(SqlDataReader dr, Type type)
        {
            List<object> objects = new List<object>();
            while (dr.Read())
            {
                intBase obj = (intBase)Activator.CreateInstance(type);
                obj.populateBySqlDataReader(dr);
                objects.Add(obj);
            }
            return objects;
        }

        // load this object with the record in the DB with the same ID
        public bool load(SqlDataReader dr)
        {
            return this.populateBySqlDataReader(dr);
        }

        // load this object with the first record in the DB that matches the query
        public bool load(string sqlQuery)
        {
            bool result;
            clsDBConnection db = new clsDBConnection();
            result = this.load(db.query(sqlQuery));
            db.close();
            return result;
        }

        // load this object with the record in the DB with the same ID
        public bool load(int id)
        {
            return this.load("SELECT * FROM " + tableName + "s WHERE id = " + id);
        }

        protected static int execute(string sqlQuery)
        {
            clsDBConnection db = new clsDBConnection();
            db.open();
            int result = db.execute(sqlQuery);
            db.close();
            return result;
        }

        // populate object based on the current row of a sql result
        public bool populateBySqlDataReader(SqlDataReader sqlReader)
        {
            // verify
            if (sqlReader.HasRows == false) return false;

            // check if reader has been initialized
            try
            {
                sqlReader.IsDBNull(0);
            }
            catch
            {
                sqlReader.Read();
            }


            //loop through the objects Properties and Lookup in the sql results
            string colName;
            foreach (var propertyInfo in this.GetType().GetProperties()) // loop through the objects properties
            {
                for (int col = 0; col < sqlReader.FieldCount; col++) // loop through the columns
                {
                    if (sqlReader.IsDBNull(col) == false) // verify we have data in the current col to read
                    {
                        colName = sqlReader.GetName(col).ToString();
                        if (propertyInfo.Name == colName) // if we have a match populate
                        {
                            switch (propertyInfo.PropertyType.ToString())
                            {
                                case "System.String":
                                    {
                                        propertyInfo.SetValue(this, sqlReader.GetString(col), null);
                                        break;
                                    }
                                case "System.Int16":
                                case "System.Int32":
                                case "System.Decimal":
                                    {
                                        propertyInfo.SetValue(this, (Int32)sqlReader.GetInt32(col), null);
                                        break;
                                    }
                                case "System.DateTime":
                                    {
                                        propertyInfo.SetValue(this, sqlReader.GetDateTime(col), null);
                                        break;
                                    }
                                case "System.Boolean":
                                    {
                                        propertyInfo.SetValue(this, sqlReader.GetBoolean(col), null);
                                        break;
                                    }
                                case "PVPOnlyLibrary.EntryType":
                                    {
                                        propertyInfo.SetValue(this, clsEventLog.getEntryType(sqlReader.GetString(col)), null);
                                        break;
                                    }

                                case "PVPOnlyLibrary.GroupType":
                                    {
                                        propertyInfo.SetValue(this, clsGroup.getGroupType(sqlReader.GetString(col)), null);
                                        break;
                                    }
                                default:
                                    {
                                        propertyInfo.SetValue(this, sqlReader.GetString(col), null); // best try might need to add a custom type above
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            return true;
        }

        // populate this typed object based on a generic object
        protected bool populateByObject(object obj)
        {
            try
            {
                // loop through the typed objects properties and lookup those values in the generic object
                foreach (var thisPropertyInfo in this.GetType().GetProperties()) // loop through the objects properties
                {
                    foreach (var objPropertyInfo in obj.GetType().GetProperties()) // loop through the objects properties
                    {
                        if (objPropertyInfo.Name == thisPropertyInfo.Name) thisPropertyInfo.SetValue(this, objPropertyInfo.GetValue(obj, null), null);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        // save and update
        public int save()
        {
            string sql, names = "", values = "", where = "", delimiter;
            int result = 0;

            // connect to db
            clsDBConnection db = new clsDBConnection();
            db.open();

            if (this.ID == 0)
            {
                // insert - INSERT INTO table (column1, column2, ... ) VALUES (expression1, expression2, ... );
                // select - SELECT * FROM table WHERE column1 = expression1 and column2 = expression2 and .....

                // set defaults
                this.Created = DateTime.Now;
                this.Modified = DateTime.Now;
                
                // build names and value for insert statment
                delimiter = "";
                foreach (var propertyInfo in this.GetType().GetProperties())
                {
                    if (propertyInfo.Name != "ID")
                    {
                        names += delimiter + propertyInfo.Name; // build name list
                        values += delimiter + getProperty(propertyInfo.Name);
                        delimiter = ", ";
                    }
                }

                // build and execute insert query
                sql = "INSERT INTO " + this.tableName + "s";
                sql += " (" + names + ")";
                sql += " VALUES (" + values + ");";
                result = db.execute(sql);


                // build query string for reselect statment
                delimiter = "";
                foreach (var propertyInfo in this.GetType().GetProperties())
                {
                    if (propertyInfo.Name != "ID")
                    {
                        where += delimiter + propertyInfo.Name + "=" + getProperty(propertyInfo.Name);
                        delimiter = " AND ";
                    }
                }

                // build query string for read back statment
                SqlDataReader dr = db.query("SELECT * FROM " + this.tableName + "s WHERE " + where + ";");
                this.load(dr);
            }
            else
            {
                // update - UPDATE table SET column1 = expression1,column2 = expression2,... WHERE conditions;

                // set defaults
                this.Modified = DateTime.Now;

                sql = "UPDATE " + this.tableName + "s SET ";
                delimiter = "";
                foreach (var propertyInfo in this.GetType().GetProperties())
                {
                    if (propertyInfo.Name != "ID")
                    {
                        sql += delimiter + propertyInfo.Name + " = " + getProperty(propertyInfo.Name);
                        delimiter = ", ";
                    }
                }
                sql += " WHERE ID = " + this.ID + ";";

                result = db.execute(sql);
            }

            db.close();
            return result;
        }


        // convert an object to JSON
        public string JSON()
        {
            string json = "";
            json += "{";

            string delimiter = "";

            // read this objects properties
            foreach (var propertyInfo in this.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    // add property name to json string
                    json += delimiter + "\"" + propertyInfo.Name + "\":";

                    // add null to json string
                    if (propertyInfo.GetValue(this, null) == null) json += "null";
                    else 
                    {
                        switch (propertyInfo.PropertyType.ToString())
                        {
                            case "System.Boolean":
                                {
                                    // add unquoted bool type property to json string
                                    if ((bool)propertyInfo.GetValue(this, null) == true) json += "true";
                                    else json += "false";
                                    break;
                                }
                            case "System.Int16":
                            case "System.Int32":
                                {
                                    // add unquoted type property to json string
                                    json += propertyInfo.GetValue(this, null);
                                    break;
                                }
                            case "System.DateTime":
                                {
                                    // add quoted type property to json string
                                    json += "\"" + propertyInfo.GetValue(this, null) + "\"";
                                    break;
                                }
                            default:
                                {
                                    // add quoted type property to json string
                                    //string value = (string)propertyInfo.GetValue(this, null);
                                    string value = propertyInfo.GetValue(this, null).ToString();
                                    value = value.Replace("\\", "\\\\");
                                    value = value.Replace("\"", "\\\"");
                                    json += "\"" + value + "\"";
                                    break;
                                }
                        }
                    }
                    delimiter = ",";
                }
            }
            json += "}";
            return json;
        }

        // return query results as JSON
        public static string JSON(string sqlQuery)
        {
            // connect to db
            clsDBConnection db = new clsDBConnection();
            db.open();
            string json = db.JSON(sqlQuery);
            db.close();
            return json;
        }

        public string getProperty(string name)
        {
            foreach (var propertyInfo in this.GetType().GetProperties())
            {
                if (propertyInfo.Name == name)
                {
                    switch (propertyInfo.PropertyType.ToString())
                    {
                        case "System.Boolean":
                            {
                                // add unquoted bool type property to json string
                                if ((bool)propertyInfo.GetValue(this, null) == true) return "1";
                                else return "0";
                            }
                        case "System.Int16":
                        case "System.Int32":
                            {
                                // add unquoted type property to json string
                                return propertyInfo.GetValue(this, null).ToString();
                            }

                        default:
                            {
                                // add quoted type property to json string
                                string text = propertyInfo.GetValue(this, null).ToString();
                                text = text.Replace("'", "''"); // db friendly
                                return "'" + text + "'";
                            }
                    }
                }
            }
            return null;
        }
    }
}