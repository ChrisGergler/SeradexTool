using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeradexToolv2.Views.ViewPages.SalesOrders
{
    /// <summary>
    /// Interaction logic for SalesOrders.xaml
    /// </summary>
    public partial class SalesOrders : Page
    {
        Toolkit Utility = new Toolkit();

        public SalesOrders()
        {
            InitializeComponent();
        }

        DataTable Data = new DataTable("SalesOrderSummary");
        DataView View;

        //bool Loaded=false;



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string estimateString = "SELECT e.EstimateNo, c.[name], e.CustomerShipToID, e.EntryDate, e.TermsCodeID, e.SubTotal, e.TotalTaxes, e." +
    "Approved, e.CustRefNo, e.TerritoryID, e.TaxGroupID, e.TotalTaxes, e.AddressID, e.ShipToAddressID, e." +
    "OrderDate, e.ShipDate, e.[Name], e.Comment, e.SubTotal, e.CSREmployeeID, e.UserCreated, e.DateCreated " +
    "UserModified, e.DateModified, e.CustomerBillToID, e.Closed, e.EstimateID " +
    "FROM Estimate e, Customers c WHERE e.CustomerID = c.CustomerID";



            //Data = Utility.populateEstimatesTable();
            Data = Utility.useQuery(estimateString);

            View = new DataView(Data);
            SalesOrderGrid.ItemsSource = View;


        }
    }
}
