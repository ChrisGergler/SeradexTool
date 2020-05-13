using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Microsoft.Data.SqlClient;
using SeradexToolv2.Views;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace SeradexToolv2.ViewModels
{
    class Toolkit
    {
        
        private void SetPrimaryKeys(DataTable data)
        {
            DataColumn[] key = { data.Columns[0] };

            data.PrimaryKey = key;
        }

        public DataTable useQuery(string query)
        {
            using (SqlConnection connect = new SqlConnection(connectString()))
            {
                connect.Open();
                DataTable results = new DataTable("results_table");

                new SqlDataAdapter(new SqlCommand(query, connect)).Fill(results);

                return results;
            }
        }

        public void openFolder(string esNumber)
        {
           // Process.Start(@"c:\");
        }

        // Private Methods

        //Use to connect to DB. This one could be changed to less-hardcoded as needed. But for the most part we'll keep it safe. May replace with a config file later which will be easier to patch as needed
        private string connectString() {return "Data Source=LSG-SQL\\Seradex;Initial Catalog=ActiveM_Lauretano;Integrated Security=SSPI;applicationIntent=ReadOnly;";}

    }// End of Class Estimates Functions
}// End of Seradex Models Namespace
