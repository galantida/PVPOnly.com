using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace PVPOnlyLibrary
{
    class clsDBConnection
    {
        private string _conString = null;
        private SqlConnection _con = null;

        public string conString
        {
            get
            {
                if (_conString == null)
                {
                    _conString = Properties.Settings.Default.prodConnectionString;
                    if (Properties.Settings.Default.deployment == "dev") _conString = Properties.Settings.Default.devConnectionString;
                }
                return _conString;
            }
        }

        public SqlConnection con
        {
            get
            {
                return _con;
            }
        }

        public clsDBConnection() 
        {
            open();
        }

        public void open() {
            // if there is not a connection create one
            if (_con == null) _con = new SqlConnection(this.conString);

            // if the connection is not open then open it
            if (_con.State == ConnectionState.Closed)
            {
                // open database connection
                _con.Open();
            }
        }

        public void close()
        {
            // if there is a connection and it equal anything except closed then close it
            if ((_con != null) && (_con.State != ConnectionState.Closed))
            {
                _con.Close();
            }
        }

        public int execute(string sql) 
        {
            this.open();

            // execute sql and close
            SqlCommand cmd = new SqlCommand(sql, _con);
            return cmd.ExecuteNonQuery();
        }

        public SqlDataReader query(string sql)
        {
            this.open();

            SqlCommand cmd = new SqlCommand(sql, _con);
            return cmd.ExecuteReader();
        }

        public string JSON(string sqlQuery)
        {
            // open and execute query
            this.open();
            SqlCommand cmd = new SqlCommand(sqlQuery, _con);
            SqlDataReader dr = cmd.ExecuteReader();

            // build json string
            string json = "[";
            string objDelimiter = "";
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    json += objDelimiter + "{";
                    string propDelimiter = "";
                    for (int col = 0; col < dr.FieldCount; col++) // loop through the columns
                    {
                        json += propDelimiter + "\"" + dr.GetName(col).ToString() + "\":";
                        switch (dr.GetFieldType(col).ToString())
                        {
                            case "int":
                                {
                                    // unquoted properties
                                    json += dr.GetValue(col).ToString();
                                    break;
                                }
                            case "bool":
                                {
                                    // binary properties
                                    string value = dr.GetValue(col).ToString();
                                    if (value == "1") value = "true";
                                    else value = "false";
                                    json += value;
                                    break;
                                }
                            default:
                                {
                                    // quoted properties
                                    string value = dr.GetValue(col).ToString();
                                    value = value.Replace("\\", "\\\\");
                                    value = value.Replace("\"", "\\\"");
                                    json += "\"" + value + "\"";
                                    break;
                                }
                        }
                        propDelimiter = ",";
                    }
                    json += "}";
                    objDelimiter = ",";
                }
            }
            json += "]";

            // close and return result
            dr.Close();
            return json;
        }
    }
}
