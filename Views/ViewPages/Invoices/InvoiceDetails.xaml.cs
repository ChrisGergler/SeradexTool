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
                    "FROM "
                    );

            }
            catch
            {
                MessageBox.Show("Error on loading lines");
            }

        }

        private void Label_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void fillInfo()
        {
            // Populate table with details

            try
            {
                DataTable BillingAddress = Utility.useQuery(
                    "SELECT c.* " +
                    "" +
                    "FROM Invoices inv " +
                    "INNER JOIN Addresses a on c.AddressID = a.AddressID " +
                    "INNER JOIN Customers c on inv.CustomerBillToID = c.CustomerID"
                    );
                MessageBox.Show(BillingAddress.Rows[0]["Name"].ToString());
            }
            catch { MessageBox.Show(" Billing didn't pull "); }
            //DataTable ShipTo

            //
        }
    }
}
