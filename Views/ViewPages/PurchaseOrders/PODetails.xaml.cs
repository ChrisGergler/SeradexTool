using LSGDatabox.ViewModels;
using System.Data;
using System.Windows;

namespace LSGDatabox.Views.ViewPages.PurchaseOrders
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

        string poNum;


        // Establish initialiation with values passed in.
        // This allows data to pass from the search into the details window.
        public PODetails(string value, DataRow row)
        {
            InitializeComponent();
            poNum = value;
            poKeys = row;

            try
            {
                Items = Utility.useQuery(
                    "SELECT " +
                    "pd.[LineNo], " +
                    "pd.[ItemNo], " +
                    "pd.[ItemVendorNumber], " +
                    "pd.[Description], " +
                    "pd.[QtyToBuy], " +
                    "pd.[QtyReceivedToDate], " +
                    "pd.[QtyInvoicedToDate], " +
                    "pd.UnitCost [Unit Cost], " +
                    "pd.ListPrice, " +
                    "pd.DiscountAmt, " +
                    "pd.ExtendedCost, " +
                    "pd.Comment, " +
                    "pd.OwnerNo [Sales Order] " +
                    "FROM PODetails pd " +
                    "INNER JOIN PO on pd.POID = PO.POID " + 
                    "WHERE PO.PONo = \'" + poNum + "\'"
                    );

                View = new DataView(Items);
                poLineItems.ItemsSource = View;
            }
            catch { MessageBox.Show("Error at view grid population."); }

        }


        private void fillInfo()
        {
            /*
             Do all the queries needed in a few goes. This step takes a hot second.
             We only want to have to pull the information one time and then adjust the info in memory rather than trying to actually do anything to the database. Safety.
             Run Try-Catch on address info just in case.
            */


            // Pull PO Details to populate viewgrid



            // Pull vendor information to populate header
            try
            {
                Vendor = Utility.useQuery(
                    "SELECT DISTINCT v.VendorID, " +
                    "v.[Name], v.[AddressID], a.[AddressL1], a.[AddressL2], a.[AddressL3], v.email, v.TermsCodeID, v.ShipViaID, " +
                    "c.DescriptionShort [City], s.DescriptionShort [State] " +
                    "FROM Vendors v " +
                    "Inner Join Addresses a on v.AddressID = a.AddressID " +
                    "Inner Join Cities c on a.CityID = c.CityID " +
                    "Inner Join StateProv s on a.StateProvID = s.StateProvID " +
                    "LEFT OUTER JOIN PO po on v.VendorID = po.VendorID " +
                    "WHERE po.PONo = \'" + poNum + "\'");
            }
            catch { MessageBox.Show("Error at Vendor query"); }

            // Assign the values of the vendor query to the appropriate text boxes
            vendorName.Text = Vendor.Rows[0]["Name"].ToString();
            vendorStreet.Text = Vendor.Rows[0]["AddressL1"].ToString() + " " + Vendor.Rows[0]["AddressL2"].ToString() + " " + Vendor.Rows[0]["AddressL3"].ToString();
            vendorCity.Text = Vendor.Rows[0]["City"].ToString();
            vendorState.Text = Vendor.Rows[0]["State"].ToString();

            DataTable shippingAddress; // UseQuery, join city, state, and country tables.



            shippingAddress = Utility.useQuery(
                    "SELECT " +
                    "vend.[Name] [Vendor Name], cust.[Name] [Customer Name], " +
                    "a.AddressL1, a.AddressL2, a.AddressL3, " +
                    "c.DescriptionShort [City], st.DescriptionShort [State] " +
                    "FROM Addresses a " +
                    "INNER JOIN PO on a.AddressID = PO.ShipToAddressID " +
                    "LEFT OUTER JOIN CustomerShipTo cust on PO.CustomerShipToID = cust.CustomerShipToID " +
                    "LEFT OUTER JOIN Vendors vend on PO.ShipToVendorID = vend.AddressID " +
                    "INNER JOIN Cities c on a.CityID = c.CityID " +
                    "INNER JOIN StateProv st on a.StateProvID = st.StateProvID " +
                    "WHERE PO.PONo = \'" + poNum +"\'"
                    );


            try
            {
                //shipName.Text
                shipStreet.Text = shippingAddress.Rows[0]["AddressL1"].ToString() + " " + shippingAddress.Rows[0]["AddressL2"].ToString()
                    + " " + shippingAddress.Rows[0]["AddressL3"].ToString();
            }
            catch { shipStreet.Text = ""; }
            try
            {
                shipCity.Text = shippingAddress.Rows[0]["City"].ToString();
            }
            catch { shipCity.Text = ""; }
            try
            {
                shipState.Text = shippingAddress.Rows[0]["State"].ToString();
            } catch{ shipState.Text = ""; }

            try { 
                if(poKeys.Table.Rows[0]["CustomerShipToID"].ToString()=="")
                {
                    if(poKeys.Table.Rows[0]["ShipToVendorID"].ToString() == "")
                    {
                        
                    }
                    else
                    {
                        shipName.Text = shippingAddress.Rows[0]["Vendor Name"].ToString();
                    }

                }
                else
                {
                    shipName.Text = shippingAddress.Rows[0]["Customer Name"].ToString();
                }
            }
            catch { };

            grandTotal.Text = poKeys["Grand Total"].ToString(); // Set to 0 for integrity. Calc and convert to get grand total.
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fillInfo();
        }
    }
}
