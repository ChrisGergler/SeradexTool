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
using System.Windows.Shapes;
using System.Data;
using SeradexToolv2.ViewModels;
using SeradexToolv2.Views.ViewPages.Estimates;
using System.Diagnostics;

namespace SeradexToolv2.Views
{
    /// <summary>
    /// Interaction logic for DetailView.xaml
    /// </summary>
    public partial class DetailView : Window
    {
        //Sets up the query functions
        Toolkit Utility = new Toolkit();

        // Gives us a Data Table to populate and a view to add
        DataTable Items = new DataTable("EstimateDetails");
        DataRow EstimateKeys;
        DataView View;

        // Used by multiple methods in here
        public string estimateID;

        public DetailView(string value, DataRow row)
        {
            InitializeComponent();
            estimateID = value;
            EstimateKeys = row;


        }

        //DataSet detailedInfo = new DataSet("Details");


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Items = Utility.useQuery(Utility.getItemDetails(estimateID));
            View = new DataView(Items);
            ItemsQuoted.ItemsSource = View;

            fillInfo();

        }

        // This method is supposed to launch the folder for the estimate
        void GoToFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("explorer.exe",
                    // String built from comments
                    @""+Utility.useQuery("SELECT FileAttachment FROM Comments WHERE EstimateID = " + estimateID).Rows[0][0].ToString()
                    );
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening folder. Please find the folder manually.");
                return;
            }
        }


        private void On_Clicked(object sender, MouseButtonEventArgs e)
        {
                /*
                string findEstimate = EstimateNo.Text;
                string query = "SELECT a.EstimateID FROM Estimate a WHERE a.EstimateNo = " + findEstimate;
                itemList = Utility.useQuery(query);
                itemDisplay = new DataView(itemList);
                ItemsQuoted.ItemsSource = itemDisplay;
                */
                MessageBox.Show(Utility.getItemDetails(estimateID));
        }

        private void fillInfo()
        {
            // DataTable Customer = Utility.useQuery("SELECT [Name] FROM Customers WHERE Customers.CustomerID = " + EstimateKeys["CustomerID"]);

            //Header
            EstimatesTitle.Content = EstimateKeys["EstimateNo"].ToString();
            try { SalesOrderNumber.Content = "Sales Order: " + Utility.useQuery("SELECT a.SalesOrderNo FROM SalesOrder a WHERE a.EstimateID = \'" + estimateID + "\'").Rows[0][0].ToString(); }
            catch (Exception)
            {
                SalesOrderNumber.Content = "No Sales Number Available";
            };


            CustomerName.Text =
               Convert.ToString(Utility.useQuery("SELECT a.[Name] FROM Customers a, Estimate b WHERE b.CustomerID = a.CustomerID AND b.EstimateID = " + estimateID).Rows[0][0]);



            DataTable billingAddress = Utility.useQuery(
                "SELECT DISTINCT e.EstimateNo, t1.CustomerBillToID, t1.CustomerID, t1.[Name], t2.AddressL1, t2.AddressL2, t2.AddressL3, t3.DescriptionShort, t4.DescriptionShort, t5.DescriptionTiny, t2.PostalCode FROM CustomerBillTo t1 INNER JOIN Addresses t2 ON t1.AddressID = t2.AddressID INNER JOIN Cities t3 ON t2.CityID = t3.CityID INNER JOIN StateProv t4 ON t2.StateProvID = t4.StateProvID INNER JOIN Countries t5 ON t2.CountryID = t5.CountryID INNER JOIN Estimate e on e.CustomerID = t1.CustomerID WHERE e.EstimateID = \'" + estimateID + "\'"
                );
            billingAddress.Columns[7].ColumnName = "City";
            billingAddress.Columns[8].ColumnName = "State";
            billingAddress.Columns[9].ColumnName = "County";
            //Customer Billing



            //I'm leaving this in here so that I can debug this later in case multiple rows show up. It should only pass back one singular row, but if it passes back two it'll print out a HUGE text volume.

            /***********************************
            string dumbBuildingString="";
            for (int j = 0; j < address.Rows.Count; j++)
            {
                for (int i = 0; i < address.Columns.Count; i++)
                {
                    dumbBuildingString = dumbBuildingString + "\n" +address.Columns[i].ColumnName.ToString() +": "+ address.Rows[j][i].ToString();
                }
            }
            
            MessageBox.Show(dumbBuildingString);
            *************************************/
            try
            {
                BillToStreet.Text = billingAddress.Rows[0]["AddressL1"].ToString();
                BillToCity.Text = billingAddress.Rows[0]["City"].ToString();
                BillToLine2.Text = billingAddress.Rows[0]["AddressL2"].ToString();
                BillToLine3.Text = billingAddress.Rows[0]["AddressL3"].ToString();


            }
            catch (Exception)
            {
                BillToStreet.Text = "No Address Listed";
                BillingAddress.Content = "";
                BillingCity.Content = "";
                BillToLine2.Text = "";
                BillToLine3.Text = "";
            }
            // Billing City
            // Billing Country
            // Billing Zip


            DataTable shippingAddress = Utility.useQuery(
    "SELECT DISTINCT e.EstimateNo, t1.CustomerShipToID, t1.CustomerID, t1.[Name], t2.AddressL1, t2.AddressL2, t2.AddressL3, t3.DescriptionShort, t4.DescriptionShort, t5.DescriptionTiny, t2.PostalCode FROM CustomerShipTo t1 INNER JOIN Addresses t2 ON t1.AddressID = t2.AddressID INNER JOIN Cities t3 ON t2.CityID = t3.CityID INNER JOIN StateProv t4 ON t2.StateProvID = t4.StateProvID INNER JOIN Countries t5 ON t2.CountryID = t5.CountryID INNER JOIN Estimate e on e.CustomerID = t1.CustomerID WHERE e.EstimateID = \'" + estimateID + "\'"
    );
            shippingAddress.Columns[7].ColumnName = "City";
            shippingAddress.Columns[8].ColumnName = "State";
            shippingAddress.Columns[9].ColumnName = "County";


            //Customer ShipTo
            // Ship Street
            // Ship City
            // Ship Country
            // Ship Zip

            // Subtotal
            // Tax
            // Grand Total
            GrandTotal.Content = GrandTotal + ":  " + EstimateKeys["SubTotal"] + EstimateKeys["TotalTaxes"];

            /*******************************************
             * DEBUGGING TOOL                         *
            *******************************************/
            string b = "** Billing **\n";
            for(int i = 0; i < billingAddress.Rows.Count; i++)
            {
                for(int j = 0; j < billingAddress.Columns.Count; j++)
                {
                    b = b + "\n" + billingAddress.Columns[j].ColumnName + ": " + billingAddress.Rows[i][j].ToString();
                }
            }

            b = b + "\n\n ** Shipping**";

            for (int i = 0; i < shippingAddress.Rows.Count; i++)
            {
                for (int j = 0; j < shippingAddress.Columns.Count; j++)
                {
                    b = b + "\n" + shippingAddress.Columns[j].ColumnName + ": " + shippingAddress.Rows[i][j].ToString();
                }
                b = b + "\n";
            }

            MessageBox.Show(b);
        }

        private void OpenSalesorder(object sender, MouseButtonEventArgs e)
        {
            if (SalesOrderNumber.Content.ToString() != "No Sales Number Available")
            {
                MessageBox.Show("Sales Order Opens here. Complete fucntion, Chris. If you're seeing this after release, please go tell Chris to get his jazz together.");
            }
            else
            {
             
            }
        }
    }
}
