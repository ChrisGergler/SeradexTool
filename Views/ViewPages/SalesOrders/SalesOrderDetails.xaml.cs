using SeradexToolv2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeradexToolv2.Views.ViewPages.SalesOrders
{
    /// <summary>
    /// Interaction logic for SalesOrderDetails.xaml
    /// </summary>
    public partial class SalesOrderDetails : Window
    {
        Toolkit Utility = new Toolkit();

        //Gives Data table to populate and view
        DataTable Items = new DataTable("SalesOrderData");
        DataRow SalesOrderKeys;
        DataView View;

        // Used by Multiple methods
        string salesOrderID;
        
        public SalesOrderDetails(string a, DataRow info)
        {
            InitializeComponent();
            SalesOrderKeys = info;
            salesOrderID = SalesOrderKeys["SalesOrderID"].ToString();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string getItemDetails = "SELECT * FROM SalesOrderDetails";
            Items = Utility.useQuery(getItemDetails);
            View = new DataView(Items);
            ItemsQuoted.ItemsSource = View;

           fillInfo();
        }
  private void fillInfo()
        {
            SalesOrderTitle.Content = SalesOrderKeys["SalesOrderNo"].ToString();

            // Builds an awkward Query to get an Estimate Number to display
            EstimateNumber.Content = Utility.useQuery("SELECT es.EstimateNo FROM Estimate es, SalesOrder so WHERE so.SalesOrderID = " 
                + SalesOrderKeys["SalesOrderID"].ToString() + " AND so.EstimateID = es. EstimateID").Rows[0]["EstimateNo"].ToString();
           
            CustomerName.Text =
               Convert.ToString(Utility.useQuery("SELECT a.[Name] FROM Customers a, SalesOrder b WHERE b.CustomerID = a.CustomerID AND b.SalesOrderID = " + salesOrderID).Rows[0]["Name"]);

            DataTable billingAddress = Utility.useQuery(
                "SELECT DISTINCT so.SalesOrderNo, t1.CustomerBillToID, t1.CustomerID, t1.[Name], t2.AddressL1, t2.AddressL2, t2.AddressL3, t3.DescriptionShort, t4.DescriptionShort, t5.DescriptionTiny, t2.PostalCode FROM CustomerBillTo t1 INNER JOIN Addresses t2 ON t1.AddressID = t2.AddressID INNER JOIN Cities t3 ON t2.CityID = t3.CityID INNER JOIN StateProv t4 ON t2.StateProvID = t4.StateProvID INNER JOIN Countries t5 ON t2.CountryID = t5.CountryID INNER JOIN SalesOrder so on so.CustomerID = t1.CustomerID WHERE so.SalesOrderID = \'" + salesOrderID + "\'"
                );
            billingAddress.Columns[7].ColumnName = "City";
            billingAddress.Columns[8].ColumnName = "State";
            billingAddress.Columns[9].ColumnName = "County";

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

            DataTable shippingAddress = Utility.useQuery(
"SELECT DISTINCT e.SalesOrderNo, t1.CustomerShipToID, t1.CustomerID, t1.[Name], t2.AddressL1, t2.AddressL2, t2.AddressL3, t3.DescriptionShort, t4.DescriptionShort, t5.DescriptionTiny, t2.PostalCode FROM CustomerShipTo t1 INNER JOIN Addresses t2 ON t1.AddressID = t2.AddressID INNER JOIN Cities t3 ON t2.CityID = t3.CityID INNER JOIN StateProv t4 ON t2.StateProvID = t4.StateProvID INNER JOIN Countries t5 ON t2.CountryID = t5.CountryID INNER JOIN SalesOrder e on e.CustomerID = t1.CustomerID WHERE e.SalesOrderID = \'" + salesOrderID + "\'"
);
            shippingAddress.Columns[7].ColumnName = "City";
            shippingAddress.Columns[8].ColumnName = "State";
            shippingAddress.Columns[9].ColumnName = "County";


            double subtotal = Math.Round(Convert.ToDouble(SalesOrderKeys["SubTotal"]), 2);
            double taxtotal = Math.Round(Convert.ToDouble(SalesOrderKeys["TotalTaxes"]), 2);

            SubtotalDisplay.Content = "$" + subtotal.ToString();
            TaxTotalDisplay.Content = "$" + taxtotal.ToString();
            GrandTotalDisplay.Content = GrandTotalDisplay.Content + Convert.ToString(Math.Round(subtotal + taxtotal, 2));

            //PaymentTermsDisplay.Content = Utility.useQuery("SELECT a.TermsCode FROM TermsCodes a WHERE a.TermsCodeID = " + SalesOrderKeys["TermsCodeID"]).Rows[0][0].ToString();

        }





        ////////////////////////////////////// End
    }
}
