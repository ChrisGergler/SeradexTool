using SeradexToolv2.ViewModels;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                "WHERE a.EstimateID = \'" + estimateID + "\' " //+
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
            if (SalesOrderNumber.Text.ToString() != "No Sales Number Available")
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
            EstimatesTitle.Text = EstimateKeys["EstimateNo"].ToString();


            try { SalesOrderNumber.Text = "Sales Order: " + Utility.useQuery("SELECT a.SalesOrderNo FROM SalesOrder a WHERE a.EstimateID = \'" + estimateID + "\'").Rows[0][0].ToString(); }
            catch (Exception)
            {
                SalesOrderNumber.Text = "No Sales Number Available";
            };


            CustomerName.Text =
               Convert.ToString(Utility.useQuery("SELECT a.[Name] " +
               "FROM Customers a, Estimate b " +
               "WHERE b.CustomerID = a.CustomerID AND b.EstimateID = " + estimateID).Rows[0]["Name"]);



            DataTable billingAddress = Utility.useQuery(
                "SELECT DISTINCT e.EstimateNo, t1.CustomerBillToID, t1.CustomerID, t1.[Name], t2.AddressL1, " +
                "t2.AddressL2, t2.AddressL3, t3.DescriptionShort as [City], t4.StateProvCode as [State], " +
                "t5.DescriptionTiny, t2.PostalCode " +
                "FROM CustomerBillTo t1 " +
                "INNER JOIN Addresses t2 ON t1.AddressID = t2.AddressID " +
                "INNER JOIN Cities t3 ON t2.CityID = t3.CityID " +
                "INNER JOIN StateProv t4 ON t2.StateProvID = t4.StateProvID " +
                "INNER JOIN Countries t5 ON t2.CountryID = t5.CountryID " +
                "INNER JOIN Estimate e on e.CustomerID = t1.CustomerID " +
                "WHERE e.EstimateID = \'" + estimateID + "\'"
                );

            //Customer Billing

            try {BillToStreet.Text = billingAddress.Rows[0]["AddressL1"].ToString(); } 
            catch (Exception) {BillToStreet.Text = "No Address Listed";
                BillingAddress.Text = "";}
            try {BillToLine2.Text = billingAddress.Rows[0]["AddressL2"].ToString(); } 
            catch (Exception) {BillToLine2.Text = "";; }
            try {BillToLine3.Text = billingAddress.Rows[0]["AddressL3"].ToString(); } 
            catch (Exception) {BillToLine3.Text = ""; }
            try {BillToCity.Text = billingAddress.Rows[0]["City"].ToString(); } 
            catch (Exception) {BillingCity.Text = ""; }
            try { BillToState.Text = billingAddress.Rows[0]["State"].ToString(); }
            catch (Exception) { BillToState.Text = ""; }
            try { BillToZip.Text = billingAddress.Rows[0]["PostalCode"].ToString(); } 
            catch (Exception) { BillToState.Text = ""; }

            // Billing City
            // Billing Country
            // Billing Zip


            DataTable shippingAddress = Utility.useQuery(
                "SELECT Estimate.EstimateID, Estimate.CustomerShipToID, ship.[Name] as [Customer Name], " +
                "ship.AddressID, a.AddressL1, a.AddressL2, a.AddressL3, city.DescriptionShort as [City], st.StateProvCode as [State], " +
                "ship.SalesRepID, a.PostalCode " +
                "FROM Estimate " +
                "INNER JOIN CustomerShipTo ship on ship.CustomerShipToID = Estimate.CustomerShipToID " +
                "INNER JOIN Addresses a on ship.AddressID = a.AddressID " +
                "INNER JOIN Cities city on a.CityID = city.CityID " +
                "INNER JOIN StateProv st on a.StateProvID = st.StateProvID " +
                "WHERE Estimate.EstimateID = \'" + estimateID + "\'");

            try {ShipToName.Text = shippingAddress.Rows[0]["Customer Name"].ToString(); }
            catch (Exception) {ShipToStreet.Text = "No Address Listed"; };
            try {ShipToStreet.Text = shippingAddress.Rows[0][4].ToString(); } 
            catch (Exception) {ShipToAddress.Text = ""; }
            try {ShipToCity.Text = shippingAddress.Rows[0]["City"].ToString(); } 
            catch (Exception) {ShippingCity.Text = ""; }
            try {ShipToLine2.Text = shippingAddress.Rows[0]["AddressL2"].ToString(); } 
            catch (Exception) { ShipToLine2.Text = ""; }
            try {ShipToLine3.Text = shippingAddress.Rows[0]["AddressL3"].ToString(); } 
            catch (Exception) {ShipToLine3.Text = ""; }
            try {ShipToState.Text = shippingAddress.Rows[0]["State"].ToString(); } 
            catch (Exception) {ShipToCity.Text = ""; }
            try { ShipToZip.Text = shippingAddress.Rows[0]["PostalCode"].ToString(); }
            catch (Exception) { ShipToCity.Text = ""; }








            double subtotal = Math.Round(Convert.ToDouble(EstimateKeys["SubTotal"]), 2);
            double taxtotal = Math.Round(Convert.ToDouble(EstimateKeys["TotalTaxes"]), 2);

            SubtotalDisplay.Text = "$" + subtotal.ToString();
            TaxTotalDisplay.Text = "$" + taxtotal.ToString();
            GrandTotalDisplay.Text = GrandTotalDisplay.Text + Convert.ToString(Math.Round(subtotal + taxtotal, 2));

            PaymentTermsDisplay.Text = Utility.useQuery("SELECT a.TermsCode FROM TermsCodes a WHERE a.TermsCodeID = " + EstimateKeys["TermsCodeID"]).Rows[0][0].ToString();
 

            try { ContactName.Text = EstimateKeys["Contact Name"].ToString(); }
            catch { ContactName.Text = ""; }
            try { ContactEmail.Text = EstimateKeys["email"].ToString(); }
            catch { ContactEmail.Text = ""; }
            try { ContactPhone.Text = EstimateKeys["Phone"].ToString(); }
            catch { ContactPhone.Text = ""; }
            try { ContactCell.Text = EstimateKeys["Cell"].ToString(); }
            catch { ContactCell.Text = ""; }


            try { VantageNumber.Text = EstimateKeys["CustRefNo"].ToString(); }
            catch { VantageNumber.Text = ""; }

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




            for (int i = 0; i < View.Count; i++)
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
            if (lineNumber == 0)
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
            if (MaterialsGrid.SelectedIndex < 1) { y = 1; }
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

        private void CopyDetails_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(

                // ES and SO numbers
                "EstimateNumber: " + EstimatesTitle.Text + " (" + SalesOrderNumber.Text + ")"
                + "\n" +
                // Customer Name
                CustomerTextBox.Text + ": " + CustomerName.Text + "\n" +
                "\n" +

                // Billing Info Copied
                "Billing Address Information\n" + BillToStreet.Text + "\n" +
                BillToLine2.Text + "\n" +
                BillToLine3.Text + "\n" +
                BillToCity.Text + "\n" +
                BillToState.Text + "\n" +
               // BillToCountry.Text + "\n" +
                BillToZip.Text + "\n"+
                // Add State
                // Add Country


                // Shipping Info Copied
                ShipToTextBox.Text + ShipToName.Text + "\n" +
                ShipToAddress.Text + ": " + ShipToStreet.Text + "\n" +
                ShipToLine2.Text + "\n" +
                ShipToLine3.Text + "\n" +
                ShipToCity.Text + ": " + ShipToCity.Text + "\n" +
                ShipToState.Text + "\n" //+
                //ShipToCountry.Text 
                //Add Country

                // Contact Info
                //Add Contact Name
                //Add Contact Email
                //Add Contact Phone
                //Add Contact cell no

                );
        }

        private void ShippingState_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
