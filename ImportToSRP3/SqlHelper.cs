﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportToSRP3
{
    public class SqlHelper
    {
        public static List<string> GetSRPDatabases()
        {
            List<string> list = new List<string>();

            // Open connection to the database
            string conString = @"Data Source=(localdb)\SRP;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("select top 1 name,state_desc,create_date from sys.databases where name like '%srp%' and state_desc='ONLINE' order by create_date desc", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr[0].ToString());
                        }
                    }
                }
            }
            return list;
        }

        public static string GetEFConnectionString(string databaseName)
        {
            if (databaseName == null)
                return string.Empty;
            return string.Format(@"metadata=res://*/SRP3Model.csdl|res://*/SRP3Model.ssdl|res://*/SRP3Model.msl;provider=System.Data.SqlClient;provider connection string=""Data Source=(localdb)\SRP;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True;App=EntityFramework""" , databaseName);
        }
    }
}
