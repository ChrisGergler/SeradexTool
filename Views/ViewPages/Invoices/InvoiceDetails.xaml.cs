using LSGDatabox.ViewModels;
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

namespace LSGDatabox.Views.ViewPages.Invoices
{
    /// <summary>
    /// Interaction logic for InvoiceDetais.xaml
    /// </summary>
    public partial class InvoiceDetais : Window
    {

        Toolkit Utility = new Toolkit();
        DataTable Items;
        DataView view;
        DataRow invoiceKeys;
        string invoiceID;

        public InvoiceDetais(string value, DataRow row)
        {
            InitializeComponent();

            invoiceKeys = row;
            invoiceID = value;

            try
            {
                Items = Utility.useQuery(
                    "SELECT " +
                    "inv.[LineNo], inv.[ItemNo], inv.[Name] [Description], " +
                    "inv.QtyOrdered, inv.QtyPriced, inv.QtyAgainstSalesOrder, " +
                    "inv.NetPrice, inv.UnitPrice, inv.ExtendedPrice, inv.TotalTaxes, " +
                    "inv.* " +
                    "FROM InvoiceDetails inv " +
                    "INNER JOIN Invoice on inv.InvoiceID = Invoice.InvoiceID " +
                    "WHERE Invoice.InvoiceNo = \'" + invoiceID + "\'"
                    );
                view = new DataView(Items);
                InvoiceResults.ItemsSource = view;
            }
            catch
            {
                MessageBox.Show("Error on loading lines");
            }

        }

        private void Label_Loaded(object sender, RoutedEventArgs e)
        {
            Title.Content += " " + invoiceID;
        }

        private void fillInfo()
        {
            // Populate table with details
            DataTable BillingAddress = Utility.useQuery(
                    "SELECT c.*, a.*, city.DescriptionShort [City], st.DescriptionShort [State] " +
                    "FROM Invoice inv " +
                    "INNER JOIN Customers c on inv.CustomerID = c.CustomerID " + // Changed from CustomerBillToID to CustomerID
                    "INNER JOIN Addresses a on c.AddressID = a.AddressID " + 
                    "INNER JOIN Cities city on a.CityID = city.CityID " +
                    "INNER JOIN StateProv st on a.StateProvID = st.StateProvID " +
                    "WHERE inv.InvoiceNo = \'" + invoiceID + "\'");

            
            BillingName.Text = BillingAddress.Rows[0]["Name"].ToString(); 
            BillingStreet.Text = BillingAddress.Rows[0]["AddressL1"].ToString() + "\n" + BillingAddress.Rows[0]["AddressL2"].ToString() + "\n" + BillingAddress.Rows[0]["AddressL3"].ToString();
            BillingCity.Text = BillingAddress.Rows[0]["City"].ToString();
            BillingState.Text = BillingAddress.Rows[0]["State"].ToString();

            DataTable shipping = Utility.useQuery("SELECT a.AddressL1, a.AddressL2, a.AddressL3, c.DescriptionShort [City], st.DescriptionShort [State] " +
                "FROM Invoice inv " +
                "INNER JOIN Addresses a on inv.ShipToAddressID = a.AddressID " +
                "INNER JOIN Cities c on a.CityID = c.CityID " +
                "INNER JOIN StateProv st on a.StateProvID = st.StateProvID " +
                "WHERE inv.InvoiceNo = \'" + invoiceID + "\'"
                );

            ShipToAddress.Text = shipping.Rows[0]["AddressL1"].ToString() + "\n" + shipping.Rows[0]["AddressL2"].ToString() +"\n"+ shipping.Rows[0]["AddressL3"].ToString();
            ShipToCity.Text = shipping.Rows[0]["City"].ToString();
            ShipToState.Text = shipping.Rows[0]["State"].ToString();

            GrandTotal.Text = invoiceKeys["SubTotal"].ToString();

            BillingAddress.Dispose();
            shipping.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fillInfo();
        }
    }
}
