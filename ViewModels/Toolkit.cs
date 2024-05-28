using Microsoft.Data.SqlClient;
using System.Data;

namespace LSGDatabox.ViewModels
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
            using (SqlConnection connect = new SqlConnection(ConnectString()))
            {
                connect.Open();
                DataTable results = new DataTable("results_table");

                new SqlDataAdapter(new SqlCommand(query, connect)).Fill(results);


                connect.Close();
                return results;
            }
        }

       

        // Private Methods

        //Use to connect to DB. This one could be changed to less-hardcoded as needed. But for the most part we'll keep it safe. May replace with a config file later which will be easier to patch as needed
        private static string ConnectString() { return "Data Source=Seradex;applicationIntent=ReadOnly;"; } // Correct connection string to properly formatted mssql server

    }// End of Class Estimates Functions
}// End of Seradex Models Namespace
