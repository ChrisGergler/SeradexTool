using LSGDatabox.ViewModels;
using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for Invoices.xaml
    /// </summary>
    public partial class Invoices : Page
    {
        Toolkit Utility = new Toolkit();

        DataTable invoiceSearch = new DataTable("invoiceSearch");
        DataView view;
        DataRow pass;
        bool loaded = false;

        public Invoices()
        {
            InitializeComponent();
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Assign information to the view
            if (loaded == false)
            {
                invoiceSearch = Utility.useQuery("SELECT " +
                    "inv.InvoiceNo, c.[Name], inv.BalanceOwing [Remaining Balance], so.SalesOrderNo [Sales Order], city.DescriptionShort [City], ad.AddressL1, * " +
                    "FROM Invoice inv " +
                    "LEFT OUTER JOIN SalesOrder so on inv.SalesOrderID = so.SalesOrderID " +
                    "LEFT OUTER JOIN Addresses ad on inv.ShipToAddressID = ad.AddressID " +
                    "LEFT OUTER JOIN Cities city on ad.CityID = city.CityID " +
                    "LEFT OUTER JOIN StateProv st on ad.StateProvID = st.StateProvID " +
                    "LEFT OUTER JOIN Customers c on inv.CustomerID = c.CustomerID "
                    );
                view = new DataView(invoiceSearch);
                invoiceResults.ItemsSource = view;
                loaded = true;
            }

            // Hide columns below
        }

        private void SearchText_KeyUp(object sender, KeyEventArgs e)
        {
            // Update row-filters based on the release of a key.
            
            switch (SearchBy.SelectedIndex)
            {
                default:
                view.RowFilter = "InvoiceNo LIKE \'*" + SearchText.Text + "*\'";
                break;

                case 1:
                    // Change which column this searches by.
                    break;

            }
        }

        private void invoiceResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                findCell("InvoiceNo", view, invoiceResults);
            }
            catch { MessageBox.Show("InvoiceNo, View, and Results failed to pass"); }
        }



        // Utilized for finding the Invoice Number cell based on any doubleclick in the grids
        private void findCell(string s, DataView v, DataGrid g)
        {
            // Calls the information based on invoice number by searching the selected row

            try
            {
                int y = g.SelectedIndex;
                DataRow pass = v[y].Row;
                string answer = v[y][s].ToString();
                Window invoiceDetails = new InvoiceDetails(answer, pass);
                invoiceDetails.Show();
            }
            catch { MessageBox.Show("Dun Goofed buddy"); }
        }

    }
}
