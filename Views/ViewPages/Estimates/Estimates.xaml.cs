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





        // On page load, populate the table
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // hardcoded Queries locked behind the load event to prevent mistaken queries.
            string estimateString = "SELECT " +
                "es.EstimateID, es.EstimateNo, so.SalesOrderNo, es.CustRefNo, " +

                "c.[Name] as [Customer Name], " +
                "con.[Name] as [Contact Name], " +
                "con.[Phone], con.email, " +
                
                "city.DescriptionShort as [City name], st.StateProvCode as [State], " +
                "es.SubTotal, es.TotalTaxes, " +
                "es.EntryDate, es.DueDate, e.UserName, es.TermsCodeID, " +
                "con.CellPhone " +
                "FROM Estimate es " +
                "INNER JOIN Customers c on es.CustomerID = c.CustomerID " +
                "INNER JOIN CustomerShipTo ship on es.CustomerShipToID = ship.CustomerShipToID "+
                "INNER JOIN Addresses a on ship.AddressID = a.AddressID " +
                "INNER JOIN Cities city on a.CityID = city.CityID " +
                "INNER JOIN StateProv st on a.StateProvID = st.StateProvID " +
                "INNER JOIN SalesOrder so on es.EstimateID = so.EstimateID " +
                "INNER JOIN SalesReps sr on es.SalesRepID = sr.SalesRepID " +
                "INNER JOIN Employees e on sr.EmployeeID = e.EmployeeID " +
                "INNER JOIN Contacts con on es.ContactID = con.ContactID;";
    
    //"WHERE e.CustomerID = c.CustomerID";

            if (loaded == false)
            { 
            Data = Utility.useQuery(estimateString);
            View = new DataView(Data);
            EstimateResults.ItemsSource = View; //Push view to our XAML Datagrid
                loaded = true; //Prevents multiple loads of same data.
            }

            // Results exist but are for debugging the query
            EstimateResults.Columns[0].Visibility = Visibility.Hidden;
            EstimateResults.Columns[14].Visibility = Visibility.Hidden;
        }

        string searchString;
        private void executeSearch(object sender, KeyEventArgs e)
        {
            
            switch (SearchParam.SelectedIndex)
            {
                case 0:
                    searchString = "EstimateNo LIKE \'*" + SearchBox.Text + "*\'";
                    break;

                case 1:
                    searchString = "[Customer Name] LIKE \'" + SearchBox.Text + "*\'";
                    break;

                case 2:
                    searchString = "SalesOrderNo LIKE \'*" + SearchBox.Text + "*\'";
                    break;

                case 3:
                    searchString = "[City name] LIKE \'*" + SearchBox.Text + "*\'";
                    break;

                case 4:
                    break;
            }
            View.RowFilter = searchString;
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            findCell("EstimateID", View, EstimateResults);
        }


        private void findCell(string s, DataView v, DataGrid g)
        {
            // Get Row index
            try {
                int y = g.SelectedIndex;
                DataRow passToNextWindow = v[y].Row;
                string answer = (string)v[y][s].ToString();
                Window detailView = new DetailView(answer, passToNextWindow);
                detailView.Show();
            }
            catch { //MessageBox.Show("You dun goofed, buddy.");
            };

        }



        //Context menu for Estimates

    }
}
