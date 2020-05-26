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
            string getItemDetails = "SELECT a.* FROM SalesOrderDetails a, SalesOrder b WHERE b.SalesOrderID = a.SalesOrderID AND a.SalesOrderID = \'"+salesOrderID+"\'";
            Items = Utility.useQuery(getItemDetails);
            View = new DataView(Items);
            ItemsQuoted.ItemsSource = View;

           fillInfo();
           itemNumbersForBOM("x");

            /**/
            ItemsQuoted.Columns[0].Visibility = Visibility.Hidden;
            ItemsQuoted.Columns[1].Visibility = Visibility.Hidden;
            ItemsQuoted.Columns[3].Visibility = Visibility.Hidden;
            ItemsQuoted.Columns[4].Visibility = Visibility.Hidden;
            ItemsQuoted.Columns[6].Visibility = Visibility.Hidden;
            ItemsQuoted.Columns[9].Visibility = Visibility.Hidden;
            ItemsQuoted.Columns[9].Visibility = Visibility.Hidden;
            /**/
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


        DataTable BoMData = new DataTable("BillOfMaterials");
        DataTable OpsData = new DataTable("Operations");
        DataView bomview;


        private void itemNumbersForBOM(string lineItem)
        {

            LineItems.Items.Add("All");

            for( int i = 0; i < View.Count; i++ )
            {
                LineItems.Items.Add((i+1)+" - "+
                    Utility.useQuery(
                        "Select a.[ItemNo] FROM Items a, SalesOrderDetails b WHERE a.ItemID = b.ItemID AND b.SalesOrderID = \'" + salesOrderID + "\'"
                    ).Rows[i]["ItemNo"].ToString());
                

              //  filters = filters + "SalesOrderDetails.ItemID = ";

                //lineID = Utility.useQuery("SELECT [ItemSpecID] FROM SalesorderDetails WHERE SalesOrderId = \'" + salesOrderID + "\'");

            }


            // Grab DataGrid items
            // Iterate through count for line items
            // Make new Item per line item
            // String of Line# + Item name
            //BoMData = Utility.useQuery("SELECT ")

            materialDetails();

            //MessageBox.Show(filters);


        }

        private void laborDetails(string value)
        {
            OpsData = Utility.useQuery(
                

            "SELECT DISTINCT ItemSpecFullStruc.ParentItemSpecID as [RefItemSpecID], "+
"CellCode, DescriptionMed, SetupUnits, RunUnits as [Run Time], CostPerHour, TotalCost, WOComment, \'\' as [RefRowNum], " +
"OpNo, UOM_1.UOMCode, UOM_2.UOMCode, PercentComplete, "+
"Notes, ItemSpecOps.JobCostCatID, CellTasks.TaskNo, "+
"UserDefined1, UserDefined2, UserDefined3, UserDefined4, ItemSpecOps.RopeLength "+
"FROM ItemSpecFullStruc "+
"INNER JOIN ItemSpecOps on ItemSpecOps.ItemSpecOpID = ItemSpecFullStruc.ItemSpecOpID "+
"INNER JOIN Cell on Cell.CellID = ItemSpecOps.CellID "+
"INNER JOIN UOMs as [UOM_1] on UOM_1.UOMID = SetupUOMID "+
"INNER JOIN UOMs as [UOM_2] on UOM_2.UOMID = RunUOMID "+
"LEFT OUTER JOIN CellTasks on CellTasks.CellTaskID = ItemSpecOps.CellTaskID "+
"WHERE ItemSpecFullStruc.RootItemSpecID = \'"+value +"\' and ItemSpecStrucID IS NULL ");

        }

        private void materialDetails()
        {
            BoMData = Utility.useQuery(//"SELECT ItemSpecStruc.*, UOMs.[UOMCode]" +
                "Select ItemSpecStruc.ItemSpecID, ItemSpecStruc.ItemSpecStrucID, ItemSpecStruc.[Name], ItemSpecStruc.ItemID, " +
                "ItemSpecStruc.QtyRequired, UOMs.[UOMCode], " +
                "ItemSpecStruc.TotalQty, ItemSpecStruc.WeightUOMID, " +
                "ItemSpecStruc.StdCost " + // Last Column
"FROM SalesOrderDetails, ItemSpecStruc " +
"INNER JOIN UOMs on UOMs.UOMID = ItemSpecStruc.UOMID " +
//"and UOMs.UOMID = ItemSpecStruc.WeightUOMID " +
"Where ItemSpecStruc.UOMID is NOT NULL and SalesOrderDetails.ItemSpecID = ItemSpecStruc.ItemSpecID " +
"and SalesOrderDetails.SalesOrderID = \'" + salesOrderID + "\'"//and SalesOrderDetails.ItemSpecID = \'"
);


            BoMData.Columns["StdCost"].ColumnName = "Calculated Cost";
        }


        private void LineItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int lineNumber = LineItems.SelectedIndex;

            bomview = new DataView(BoMData);
            //string SpecID = Utility.useQuery("SELECT a.[ItemSpecID] FROM ItemSpec a").ToString();

            

            MaterialsGrid.ItemsSource= bomview;
            if (lineNumber == 0)
            {
            }
            else
            {
                string debugNumbers = Items.Rows[lineNumber-1]["ItemSpecID"].ToString();
                bomview.RowFilter = "ItemSpecID = " + debugNumbers;
            }

  
        }

        private void getItemSpecs()
        {


        }

        private void updateVendor(object sender, EventArgs e)
        {
            int value = MaterialsGrid.SelectedIndex;


            // Run function to put vendor information in grid beneath each time cell changes on material grid
            DataView laborview = new DataView(OpsData);
            VendorGrid.ItemsSource = laborview;
            string info = "";
            int y;
            if (MaterialsGrid.SelectedIndex < 1) { y = 1; }
            else { y = MaterialsGrid.SelectedIndex; }





            try { info = bomview[y]["ItemSpecID"].ToString(); }
            catch
            {

            };
           // MessageBox.Show(info);
            laborDetails(info);
            /*
             * 
             *             int y = g.SelectedIndex;
            DataRow passToNextWindow = v[y].Row;
            try { string answer = ((string)View[y][s].ToString());
                Window openWindow = new SalesOrderDetails(answer, passToNextWindow);
                openWindow.Show();
            }
            catch (Exception) { MessageBox.Show("Cannot return answer. \n The stars aren't aligned. Can't do it tonight. The stars. \n" + s);
            }
             * 
             */



            //laborDetails("X");

            /*/if ()
            if (VendorGrid.Columns.Count > 0)
            {
                VendorGrid.Columns.
                VendorGrid.Columns[0].Visibility = Visibility.Hidden;
            }*/
        }

        private void MaterialsGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }


        ////////////////////////////////////// End
    }
}
