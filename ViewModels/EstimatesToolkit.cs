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

namespace SeradexToolv2.ViewModels
{
    class EstimatesToolkit
    {
        
        public DataTable populateEstimatesTable()
        {
            using (SqlConnection connect = new SqlConnection(connectString()))
            {
                connect.Open();
                DataTable results = new DataTable("results_table");
                new SqlDataAdapter(new SqlCommand(estimateQuery(), connect)).Fill(results);

                SetPrimaryKeys(results, "EstimateID");
                return results;
            }

        }
        private void SetPrimaryKeys(DataTable data, string colName)
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


        // Private Methods

        //Use to connect to DB. This one could be changed to less-hardcoded as needed. But for the most part we'll keep it safe. May replace with a config file later which will be easier to patch as needed
        private string connectString() {return "Data Source=LSG-SQL\\Seradex;Initial Catalog=ActiveM_Lauretano;Integrated Security=SSPI;applicationIntent=ReadOnly;";}

        

        //Set as a hard-coded private function for security. Can't fuck it up if it's locked up tight.
        private string estimateQuery()
        {
            string estimateString = "SELECT e.EstimateNo, c.[name], e.CustomerShipToID, e.EntryDate, e.TermsCodeID, e." +
                "Approved, e.CustRefNo, e.TerritoryID, e.TaxGroupID, e.TotalTaxes, e.AddressID, e.ShipToAddressID, e." +
                "OrderDate, e.ShipDate, e.[Name], e.Comment, e.SubTotal, e.CSREmployeeID, e.UserCreated, e.DateCreated " +
                "UserModified, e.DateModified, e.CustomerBillToID, e.Closed, e.EstimateID " +
                "FROM Estimate e, Customers c WHERE e.CustomerID = c.CustomerID";
            return estimateString;
        }

        // Same as above, hard coded for safety.
        public string getItemDetails(string a)
        {
            string queryString = ItemDetails+a;
            return queryString;
        }

        private string ItemDetails = "SELECT a.[LineNo], a.[ItemNo], a.[QtyOrdered], a.[ListPrice], a.[UnitPrice], a.[DiscountPct], a.[DiscountAmt], a.[NetPrice] " +
                "FROM EstimateDetails a " +
                "WHERE EstimateID = ";

        private string CustomerInfo;

        private string ShippingDetails;
        private string Comments;

    }// End of Class Estimates Functions
}// End of Seradex Models Namespace
