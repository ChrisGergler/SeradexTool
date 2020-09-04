using LSGDatabox.ViewModels;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LSGDatabox.Views.ViewPages.PurchaseOrders
{
    /// <summary>
    /// Interaction logic for PurchaseOrders.xaml
    /// </summary>
    public partial class PurchaseOrders : Page
    {

        Toolkit Utility = new Toolkit();                // Access to useful functions and query builders for security

        bool loaded;                                    // Prevents loading PO list from DB multiple times on a single open

        // Create the table and view here, assign it on load, and leave it public to filter and adjust. NEVER PASS THIS BACK TO DB.
        DataTable Data = new DataTable("PurchaseOrderSummary");
        DataView View;

        public PurchaseOrders()
        {
            InitializeComponent();
        }

        // Page Loaded - Make initial query for Purchase Orders


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Run query for PO data

            // Hardcode the query for security and saftey of software and database

            if (loaded == false)
            {
                Data = Utility.useQuery("SELECT " +
                "po.PONo [PO Number], vend.[Name] [Vendor Name], city.DescriptionShort [City], st.DescriptionShort [State], " + // PO Number, Vendor name, CHECK CITY VS SERADEX, CHECK STATE VS SERADEX, 
                "po.TotalTaxes [Taxes], po.SubTotal [Sub total], po.TotalTaxes+po.SubTotal [Grand Total], po.Reference [Reference Numbers], po.DateCreated [Created], po.DueDate, po.Completed, " +
                "po.Comment " +
                ", po.* " +
                "FROM [dbo].[PO] po " +
                "LEFT OUTER JOIN [dbo].[Vendors] vend on po.VendorID = vend.VendorID " +
                "LEFT OUTER JOIN [dbo].[Addresses] ad on po.AddressID = ad.AddressID " +
                "INNER JOIN [dbo].[Cities] city on ad.CityID = city.CityID " +
                "INNER JOIN [dbo].[StateProv] st on ad.StateProvID = st.StateProvID " +
                "ORDER BY po.PONo");

                View = new DataView(Data);
                PurchaseOrderGrid.ItemsSource = View; //Push view to our XAML Datagrid
                loaded = true; //Prevents multiple loads of same data.
            }

        }

        private void oSearchFor_KeyUp(object sender, KeyEventArgs e)
        {



            switch (oSearchBy.SelectedIndex)
            {
                case 0: // Purchase Order
                    var searchOne = "[PONo] LIKE \'*" + oSearchFor.Text + "*\'";
                    View.RowFilter = searchOne;
                    break;

                case 1: // Grand Total
                        //var searchTwo = "[Grand Total] LIKE \'*" + oSearchFor.Text + "*\'";
                        // View.RowFilter = searchTwo;
                    break;

                case 2: // Creation Date
                    var searchThree = "[DateCreated] LIKE \'*" + oSearchFor.Text + "*\'";
                    View.RowFilter = searchThree;
                    break;

                case 3: // Vendor
                    var searchFour = "[Vendor Name] LIKE \'*" + oSearchFor.Text + "*\'";
                    View.RowFilter = searchFour;
                    break;

                case 4: // City
                    var searchFive = "[City] LIKE \'*" + oSearchFor.Text + "*\'";
                    View.RowFilter = searchFive;
                    break;

                case 5: // State
                    var searchSix = "[State] LIKE \'*" + oSearchFor.Text + "*\'";
                    View.RowFilter = searchSix;
                    break;
            }

        }


        //private find cell on Grid (string s, DataView v, DataGrid g)
        //      Opens up the details page in a separate window.
        //      Scan through the row to find the PO number cell
        //      Pass that information on through to the next window.

        private void findCell(string s, DataView v, DataGrid g)
        {

            int y = g.SelectedIndex; // Get row number
            DataRow pass = v[y].Row; // Get row at dataview's index
            string number = v[y][s].ToString(); // Grab PO number out of view from selected row
            Window PODetails = new PODetails(number, pass); //Create the PO Details window, hand off PO number and row data
            MessageBox.Show(number);
            PODetails.Show();
        }

        private void PurchaseOrderGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            findCell("PO Number", View, PurchaseOrderGrid);
        }
    }
}
