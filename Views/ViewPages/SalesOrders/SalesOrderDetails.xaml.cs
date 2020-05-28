using Microsoft.IdentityModel.Tokens;
using SeradexToolv2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            string getItemDetails = "SELECT a.* " +
                "FROM SalesOrderDetails a, SalesOrder b " +
               // "Inner Join Addresses c on a.AddressID = c.AddressID " +
               // "Inner Join Addresses d on a.ShipToAddressID = d.AddressID " +
                "WHERE b.SalesOrderID = a.SalesOrderID AND a.SalesOrderID = \'"+salesOrderID+"\'";
            Items = Utility.useQuery(getItemDetails);
            View = new DataView(Items);
            ItemsQuoted.ItemsSource = View;

           fillInfo();
           itemNumbersForBOM();

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
               Convert.ToString(Utility.useQuery("SELECT a.[Name] FROM Customers a, SalesOrder b " +
               "WHERE b.CustomerID = a.CustomerID AND b.SalesOrderID = " + salesOrderID).Rows[0]["Name"]);

            DataTable billingAddress = Utility.useQuery(
                "SELECT DISTINCT so.SalesOrderNo, t1.CustomerBillToID, t1.CustomerID, t1.[Name], " +
                "t2.AddressL1, t2.AddressL2, t2.AddressL3, t3.DescriptionShort, t4.DescriptionShort, t5.DescriptionTiny, t2.PostalCode " +
                "FROM CustomerBillTo t1 " +
                "INNER JOIN Addresses t2 ON t1.AddressID = t2.AddressID " +
                "INNER JOIN Cities t3 ON t2.CityID = t3.CityID " +
                "INNER JOIN StateProv t4 ON t2.StateProvID = t4.StateProvID " +
                "INNER JOIN Countries t5 ON t2.CountryID = t5.CountryID " +
                "INNER JOIN SalesOrder so on so.CustomerID = t1.CustomerID " +
                "WHERE so.SalesOrderID = \'" + salesOrderID + "\'"
                );
            billingAddress.Columns[7].ColumnName = "City";
            billingAddress.Columns[8].ColumnName = "State";
            billingAddress.Columns[9].ColumnName = "County";
            //Customer Billing

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
    "SELECT DISTINCT SalesOrder.CustomerShipToID, CustomerShipTo.[Name] as CustomerName, t1a.AddressL1, t1a.AddressL2, t1a.AddressL3, t1a.AddressL4, " +
    "t1b.AddressL1 as BackUpAdd1, t1b.AddressL2 as BackUpAdd2, t1b.AddressL3 as BackUpAdd3, t1b.AddressL4 as BackUpAdd4, " +
    "city.[DescriptionShort] as City, state.[StateProvCode] as State, t1a.PostalCode, country.[DescriptionTiny] as Country " +
    "From SalesOrder " +
    "Inner Join CustomerShipTo on CustomerShipTo.CustomerShipToID = SalesOrder.CustomerShipToID " +
    "Inner Join Addresses t1a on t1a.AddressID = SalesOrder.CustomerShipToID " +
    "Inner Join Addresses t1b on t1b.AddressID = SalesOrder.AddressID " +
    "Inner Join Cities city on t1a.CityID = city.CityID and t1b.CityID = city.CityID " +
    "Inner join StateProv state on t1a.StateProvID = state.StateProvID and t1b.StateProvID = state.StateProvID " +
    "Inner Join Countries country on t1a.CountryID = country.CountryID and t1b.CountryID = country.CountryID "+
    "WHERE SalesOrderID = \'" + salesOrderID + "\'"
);
            
            try
            {
                string blah;
                for(int x = 0; x < shippingAddress.Rows.Count; x++)
                {
                    blah = shippingAddress.Rows[x].ToString();
                }

                ShipToName.Text = shippingAddress.Rows[0]["CustomerName"].ToString();
                ShipToStreet.Text = shippingAddress.Rows[0]["AddressL1"].ToString();
                ShipToCity.Text = shippingAddress.Rows[0]["City"].ToString();
                ShipToLine2.Text = shippingAddress.Rows[0]["AddressL2"].ToString();
                ShipToLine3.Text = shippingAddress.Rows[0]["AddressL3"].ToString();


            }
            catch (Exception)
            {


                {
                    try
                    {
                        if (shippingAddress.Rows[0]["AddressL1"] == null &&
                            shippingAddress.Rows[0]["City"] == null)
                        {
                            ShipToName.Text = shippingAddress.Rows[0]["CustomerName"].ToString();
                            ShipToStreet.Text = shippingAddress.Rows[0]["BackUpAdd1"].ToString();
                            ShipToCity.Text = shippingAddress.Rows[0]["City"].ToString();
                            ShipToLine2.Text = shippingAddress.Rows[0]["BackUpAdd2"].ToString();
                            ShipToLine3.Text = shippingAddress.Rows[0]["BackUpAdd3"].ToString();
                        }
                    }
                    catch(Exception)
                    {

                    }
                }

                ShipToStreet.Text = "No Address Listed";
                ShipToAddress.Content = "";
                ShippingCity.Content = "";
                ShipToLine2.Text = "";
                ShipToLine3.Text = "";
            }

            double subtotal = Math.Round(Convert.ToDouble(SalesOrderKeys["SubTotal"]), 2);
            double taxtotal = Math.Round(Convert.ToDouble(SalesOrderKeys["TotalTaxes"]), 2);

            SubtotalDisplay.Content = "$" + subtotal.ToString();
            TaxTotalDisplay.Content = "$" + taxtotal.ToString();
            GrandTotalDisplay.Content = GrandTotalDisplay.Content + Convert.ToString(Math.Round(subtotal + taxtotal, 2));

            //PaymentTermsDisplay.Content = Utility.useQuery("SELECT a.TermsCode FROM TermsCodes a WHERE a.TermsCodeID = " + SalesOrderKeys["TermsCodeID"]).Rows[0][0].ToString();

            string debugtext = "Numbers";//SalesOrderKeys["CustRefNo"].ToString();
            VantageNo.Content = debugtext;

        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////
        /// 
        /// END OF INFO FILL
        /// 
        /// ////////////////////////////////////////////////////////////////////////
        /// </summary>

        DataTable BoMData = new DataTable("BillOfMaterials");
        DataTable OpsData = new DataTable("Operations");
        DataView bomview;


        private void itemNumbersForBOM()
        {

            LineItems.Items.Add("All");

            for( int i = 0; i < View.Count; i++ )
            {
                LineItems.Items.Add((i+1) + " - " +
                    Utility.useQuery(
                        "Select a.[ItemNo] FROM Items a, SalesOrderDetails b " +
                        "WHERE a.ItemID = b.ItemID AND b.SalesOrderID = \'" + salesOrderID + "\'"
                    ).Rows[i]["ItemNo"].ToString());
                
            }

            materialDetails();
            try
            {
                MaterialsGrid.Columns[0].Visibility = Visibility.Hidden;
                MaterialsGrid.Columns[1].Visibility = Visibility.Hidden;
            }
            catch { };
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

            VendorGrid.Columns[0].Visibility = Visibility.Hidden;
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

        private void openFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("explorer.exe",
                    @"" + Utility.useQuery("Select com.FileAttachment " +
                    "FROM Comments com " +
                    "Inner Join SalesOrder so on so.EstimateID = com.EstimateID " +
                    "WHERE so.SalesOrderID = \'" + salesOrderID + "\'").Rows[0][0].ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening Folder. Please find the folder manually.");
            }
        }
        ////////////////////////////////////// End
    }
}
