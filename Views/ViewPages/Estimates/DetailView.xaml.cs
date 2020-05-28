using SeradexToolv2.ViewModels;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace SeradexToolv2.Views
{
    /// <summary>
    /// Interaction logic for DetailView.xaml
    /// </summary>
    public partial class DetailView : Window
    {
        //Sets up the query functions
        Toolkit Utility = new Toolkit();

        // Gives us a Data Table to populate and a view to add
        DataTable Items = new DataTable("EstimateDetails");
        DataRow EstimateKeys;
        DataView View;

        // Used by multiple methods in here
        string estimateID;

        public DetailView(string value, DataRow row)
        {
            InitializeComponent();
            estimateID = value;
            EstimateKeys = row;


        }

        //DataSet detailedInfo = new DataSet("Details");


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            /*
            string getItemDetails = "SELECT a.[LineNo], a.[ItemNo], a.[Description], a.[QtyOrdered], a.[ListPrice], " +
                "a.[UnitPrice], a.[DiscountPct], a.[DiscountAmt], a.[TotalTaxes], a.[NetPrice] " +
                "FROM EstimateDetails a " +
                "WHERE EstimateID = \'"+estimateID+"\'" +
                "Order by a.[LineNo]";
            *///Old String

            string getItemDetails = "SELECT a.ItemNo, * " +
                "FROM EstimateDetails a " +
                "WHERE a.EstimateID = \'" + estimateID +"\' " //+
//                "Order By EstimateDetails.LineNo"
                ;

            Items = Utility.useQuery(getItemDetails);
            View = new DataView(Items);
            ItemsQuoted.ItemsSource = View;

            fillInfo();
            itemNumbersForBOM();
        }

        // This method is supposed to launch the folder for the estimate
        void GoToFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("explorer.exe",
                    // String built from comments
                    @"" + Utility.useQuery("SELECT FileAttachment FROM Comments WHERE EstimateID = " + estimateID).Rows[0][0].ToString()
                    );
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening folder. Please find the folder manually.");
                return;
            }
        }

        private void OpenSalesorder(object sender, MouseButtonEventArgs e)
        {
            if (SalesOrderNumber.Content.ToString() != "No Sales Number Available")
            {
                MessageBox.Show("This should launch the Sales order Window.");
            }
            else
            {

            }
        }


        private void fillInfo()
        {
            // DataTable Customer = Utility.useQuery("SELECT [Name] FROM Customers WHERE Customers.CustomerID = " + EstimateKeys["CustomerID"]);

            //Header
            EstimatesTitle.Content = EstimateKeys["EstimateNo"].ToString();


            try { SalesOrderNumber.Content = "Sales Order: " + Utility.useQuery("SELECT a.SalesOrderNo FROM SalesOrder a WHERE a.EstimateID = \'" + estimateID + "\'").Rows[0][0].ToString(); }
            catch (Exception)
            {
                SalesOrderNumber.Content = "No Sales Number Available";
            };


            CustomerName.Text =
               Convert.ToString(Utility.useQuery("SELECT a.[Name] " +
               "FROM Customers a, Estimate b " +
               "WHERE b.CustomerID = a.CustomerID AND b.EstimateID = " + estimateID).Rows[0]["Name"]);



            DataTable billingAddress = Utility.useQuery(
                "SELECT DISTINCT e.EstimateNo, t1.CustomerBillToID, t1.CustomerID, t1.[Name], t2.AddressL1, " +
                "t2.AddressL2, t2.AddressL3, t3.DescriptionShort, t4.DescriptionShort, " +
                "t5.DescriptionTiny, t2.PostalCode " +
                "FROM CustomerBillTo t1 " +
                "INNER JOIN Addresses t2 ON t1.AddressID = t2.AddressID " +
                "INNER JOIN Cities t3 ON t2.CityID = t3.CityID " +
                "INNER JOIN StateProv t4 ON t2.StateProvID = t4.StateProvID " +
                "INNER JOIN Countries t5 ON t2.CountryID = t5.CountryID " +
                "INNER JOIN Estimate e on e.CustomerID = t1.CustomerID " +
                "WHERE e.EstimateID = \'" + estimateID + "\'"
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
            // Billing City
            // Billing Country
            // Billing Zip


            DataTable shippingAddress = Utility.useQuery(
    "SELECT DISTINCT Estimate.CustomerShipToID, CustomerShipTo.[Name] as CustomerName, t1a.AddressL1, t1a.AddressL2, t1a.AddressL3, t1a.AddressL4, " +
    "t1b.AddressL1 as BackUpAdd1, t1b.AddressL2 as BackUpAdd2, t1b.AddressL3 as BackUpAdd3, t1b.AddressL4 as BackUpAdd4, " +
    "city.[DescriptionShort] as City, state.[StateProvCode] as State, t1a.PostalCode, country.[DescriptionTiny] as Country " +
    "From Estimate " +
    "Inner Join CustomerShipTo on CustomerShipTo.CustomerShipToID = Estimate.CustomerShipToID " +
    "Inner Join Addresses t1a on t1a.AddressID = Estimate.CustomerShipToID " +
    "Inner Join Addresses t1b on t1b.AddressID = Estimate.AddressID " +
    "Inner Join Cities city on t1a.CityID = city.CityID and t1b.CityID = city.CityID " +
    "Inner join StateProv state on t1a.StateProvID = state.StateProvID and t1b.StateProvID = state.StateProvID " +
    "Inner Join Countries country on t1a.CountryID = country.CountryID and t1b.CountryID = country.CountryID " +
    "WHERE EstimateID = \'" + estimateID + "\'"
                );
            /*
            shippingAddress.Columns[7].ColumnName = "City";
            shippingAddress.Columns[8].ColumnName = "State";
            shippingAddress.Columns[9].ColumnName = "County";
            */

            try
            {
                string blah;
                for (int x = 0; x < shippingAddress.Rows.Count; x++)
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
                    catch (Exception)
                    {

                    }
                }

                ShipToStreet.Text = "No Address Listed";
                ShipToAddress.Content = "";
                ShippingCity.Content = "";
                ShipToLine2.Text = "";
                ShipToLine3.Text = "";
            }


            double subtotal = Math.Round(Convert.ToDouble(EstimateKeys["SubTotal"]), 2);
            double taxtotal = Math.Round(Convert.ToDouble(EstimateKeys["TotalTaxes"]), 2);

            SubtotalDisplay.Content = "$" + subtotal.ToString();
            TaxTotalDisplay.Content = "$" + taxtotal.ToString();
            GrandTotalDisplay.Content = GrandTotalDisplay.Content + Convert.ToString(Math.Round(subtotal + taxtotal, 2));

            PaymentTermsDisplay.Content = Utility.useQuery("SELECT a.TermsCode FROM TermsCodes a WHERE a.TermsCodeID = " + EstimateKeys["TermsCodeID"]).Rows[0][0].ToString();
            /////////////////////////////////////////////
            ///End of Displays

            string debugstring = EstimateKeys["CustRefNo"].ToString();

            VantageNumber.Text = debugstring;

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

                    LineItems.Items.Add((i + 1) + " - " +
                        Utility.useQuery(
                            "Select b.[ItemNo] FROM EstimateDetails b " +
                            "WHERE b.EstimateID = \'" + estimateID + "\'"
                        ).Rows[i]["ItemNo"].ToString());

            }
            
            materialDetails();
        }

        private void materialDetails()
        {
            BoMData = Utility.useQuery(
                "Select ItemSpecStruc.ItemSpecID, ItemSpecStruc.ItemSpecStrucID, ItemSpecStruc.[Name], ItemSpecStruc.ItemID, " +
                "ItemSpecStruc.QtyRequired, UOMs.[UOMCode], " +
                "ItemSpecStruc.TotalQty, ItemSpecStruc.WeightUOMID, " +
                "ItemSpecStruc.StdCost " + // Last Column
                "FROM EstimateDetails, ItemSpecStruc " +
                "INNER JOIN UOMs on UOMs.UOMID = ItemSpecStruc.UOMID " +
                //"and UOMs.UOMID = ItemSpecStruc.WeightUOMID " +
                "Where ItemSpecStruc.UOMID is NOT NULL and EstimateDetails.ItemSpecID = ItemSpecStruc.ItemSpecID " +
                "and EstimateDetails.EstimateID = \'" + estimateID + "\'"//and SalesOrderDetails.ItemSpecID = \'"
                );

            BoMData.Columns["StdCost"].ColumnName = "Calculated Cost";
        }

        private void LineItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int lineNumber = LineItems.SelectedIndex;

            bomview = new DataView(BoMData);
            // Keeping spacing between details and sales order versions



            MaterialsGrid.ItemsSource = bomview;
            if(lineNumber == 0)
            {
            }
            else
            {
                string debugNumbers = Items.Rows[lineNumber - 1]["ItemSpecID"].ToString();
                bomview.RowFilter = "ItemSpecID = " + debugNumbers;
            }
            MaterialsGrid.Columns[0].Visibility = Visibility.Hidden;
            MaterialsGrid.Columns[1].Visibility = Visibility.Hidden;
            MaterialsGrid.Columns[3].Visibility = Visibility.Hidden;
        }

        private void laborDetails(string value)
        {
            OpsData = Utility.useQuery(


            "SELECT DISTINCT ItemSpecFullStruc.ParentItemSpecID as [RefItemSpecID], " +
"CellCode, DescriptionMed, SetupUnits, RunUnits as [Run Time], CostPerHour, TotalCost, WOComment, \'\' as [RefRowNum], " +
"OpNo, UOM_1.UOMCode, UOM_2.UOMCode, PercentComplete, " +
"Notes, ItemSpecOps.JobCostCatID, CellTasks.TaskNo, " +
"UserDefined1, UserDefined2, UserDefined3, UserDefined4, ItemSpecOps.RopeLength " +
"FROM ItemSpecFullStruc " +
"INNER JOIN ItemSpecOps on ItemSpecOps.ItemSpecOpID = ItemSpecFullStruc.ItemSpecOpID " +
"INNER JOIN Cell on Cell.CellID = ItemSpecOps.CellID " +
"INNER JOIN UOMs as [UOM_1] on UOM_1.UOMID = SetupUOMID " +
"INNER JOIN UOMs as [UOM_2] on UOM_2.UOMID = RunUOMID " +
"LEFT OUTER JOIN CellTasks on CellTasks.CellTaskID = ItemSpecOps.CellTaskID " +
"WHERE ItemSpecFullStruc.RootItemSpecID = \'" + value + "\' and ItemSpecStrucID IS NULL ");

        }


        private void updateVendor(object sender, EventArgs e)
        {
            int value = MaterialsGrid.SelectedIndex;

            DataView laborView = new DataView(OpsData);
            VendorGrid.ItemsSource = laborView;
            string info = "";
            int y;
            if(MaterialsGrid.SelectedIndex < 1) { y = 1; }
            else { y = MaterialsGrid.SelectedIndex; }

            try { info = bomview[y]["ItemSpecID"].ToString(); }
            catch
            {

            };

            laborDetails(info);
            try
            {
                VendorGrid.Columns[0].Visibility = Visibility.Hidden;
            }
            catch { }
        }

       

    }
}
