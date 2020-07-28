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

namespace SeradexToolv2.Views.ViewPages.PurchaseOrders
{
    /// <summary>
    /// Interaction logic for PODetails.xaml
    /// </summary>
    public partial class PODetails : Window
    {

        Toolkit Utility = new Toolkit();

        DataTable Items = new DataTable("PurchaseOrderDetails");
        DataTable Vendor;
        DataRow poKeys;
        DataView View;

        string poID;


        // Establish initialiation with values passed in.
        // This allows data to pass from the search into the details window.
        public PODetails(string value, DataRow row)
        {
            InitializeComponent();
            poID = value; 
            poKeys = row; 

        }


        private void fillInfo()
        {
            /*
             Do all the queries needed in a few goes. This step takes a hot second.
             We only want to have to pull the information one time and then adjust the info in memory rather than trying to actually do anything to the database. Safety.
             Run Try-Catch on address info just in case.
            */


            // Pull PO Details to populate viewgrid
            Items = Utility.useQuery(
                "SELECT * " +
                "FROM PODetails p " +
                "LEFT INNER JOIN PO on PO.poID = p.POID " +
                "WHERE PO.poID = " + poID
                );
            View = new DataView(Items);
            poLineItems.ItemsSource = View;

            // Pull vendor information to populate header
            Vendor = Utility.useQuery(
                "SELECT DISTINCT v.VendorID, " +
                "v.[Name], v.[AddressID], v.email, v.TermsCodeID, v.ShipViaID " +
                "FROM Vendors v " +
                "Inner Join Addresses a on v.AddressID = a.AddressID " +
                "Inner Join Cities c on a.CityID = c.CityID " +
                "Inner Join StateProv s on a.StateProvID = s.StateProvID " +
                "LEFT OUTER JOIN PO po on v.VendorID = po.VendorID " +
                "WHERE po.VendorID = v.VendorID");

            // Assign the values of the vendor query to the appropriate text boxes



            DataTable shippingAddress; // UseQuery, join city, state, and country tables.

            double GrandTotal = 0; // Set to 0 for integrity. Calc and convert to get grand total.
        }
        


    }
}
