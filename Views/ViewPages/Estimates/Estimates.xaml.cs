using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Tools = SeradexToolv2.ViewModels;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
using SeradexToolv2.ViewModels;

namespace SeradexToolv2.Views.ViewPages.Estimates
{
    
    /// <summary>
    /// Interaction logic for Estimates.xaml
    /// </summary>
    public partial class Estimates : Page
    {
        Toolkit Utility = new Toolkit();



        public Estimates()
        {
            InitializeComponent();
        }

        bool loaded;

        DataTable Data = new DataTable("EstimatesSummary");

        DataView View;



        private void executeSearch(object sender, KeyEventArgs e)
        {
            string searchString = "";
            switch (SearchParam.SelectedIndex)
            {
                case 0:

                    searchString = "EstimateNo LIKE '*" + SearchBox.Text + "*'";
                    break;

                case 1:

                    searchString = "name LIKE '" + SearchBox.Text + "*'";
                    break;

                case 2:

                    searchString = "";
                    break;

                case 3:
                    break;

                case 4:
                    break;
            }
            View.RowFilter = searchString;
        }

        // On page load, populate the table
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // hardcoded Queries locked behind the load event to prevent mistaken queries.
            string estimateString = "SELECT e.EstimateNo, c.[name], e.CustomerShipToID, e.EntryDate, e.TermsCodeID, e.SubTotal, e.TotalTaxes, e." +
    "Approved, e.CustRefNo, e.TerritoryID, e.TaxGroupID, e.TotalTaxes, e.AddressID, e.ShipToAddressID, e." +
    "OrderDate, e.ShipDate, e.[Name], e.Comment, e.SubTotal, e.CSREmployeeID, e.UserCreated, e.DateCreated " +
    "UserModified, e.DateModified, e.CustomerBillToID, e.Closed, e.EstimateID " +
    "FROM Estimate e, Customers c WHERE e.CustomerID = c.CustomerID";

            if (loaded == false)
            { 
            //Data = Utility.populateEstimatesTable();
            Data = Utility.useQuery(estimateString);
            
            View = new DataView(Data);
            EstimateResults.ItemsSource = View; 

            loaded = true;
            }
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            findCell("EstimateID", View, EstimateResults);
        }


        private void findCell(string colName, DataView view, DataGrid grid)
        {
            // Get Row index
            int y = grid.SelectedIndex;
            DataRow passToNextWindow = view[y].Row;
            string answer = "";
            try { answer = ((string)view[y][colName].ToString()); }
            catch { MessageBox.Show("You dun goofed, buddy.");
            };
            Window detailView = new DetailView(answer, passToNextWindow);

            detailView.Show();

            
        }

    }
}
