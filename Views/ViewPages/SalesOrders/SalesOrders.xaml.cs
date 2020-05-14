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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeradexToolv2.Views.ViewPages.SalesOrders
{
    /// <summary>
    /// Interaction logic for SalesOrders.xaml
    /// </summary>
    public partial class SalesOrders : Page
    {
        Toolkit Utility = new Toolkit();

        public SalesOrders()
        {
            InitializeComponent();

        }

        DataTable Data = new DataTable("SalesOrderSummary");
        DataView View;

        //bool Loaded=false;



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string fillString = "SELECT so.SalesOrderID, so.SalesOrderNo, c.[Name], so.ShipViaID, so.ShipToAddressID, so.CustomerPO, so.SubTotal, so.TotalTaxes, so.CustomerBillToID, so.OrderDate, so.ShipDate, so.ShipViaID FROM SalesOrder so "+
"LEFT OUTER JOIN Customers c on so.CustomerID = c.CustomerID Order By so.SalesOrderID";



            //Data = Utility.populateEstimatesTable();
            Data = Utility.useQuery(fillString);

            View = new DataView(Data);
            SalesOrderGrid.ItemsSource = View;

        }

        string searchString;
        private void executeSearch(object sender, KeyEventArgs e)
        {
            switch(SearchParam.SelectedIndex)
            {
                case 0:
                    searchString = "SalesOrderNo LIKE \'*"+SearchBox.Text+"*\'";
                    break;

                case 1:
                    searchString = "[Name] LIKE \'*" + SearchBox.Text + "*\'";
                    break;

                case 2:
                    // More search options
                    break;

                case 3:
                    // More search options
                    break;
            }

            View.RowFilter = searchString;
        }

        private void GridDoubleClick(object sender, RoutedEventArgs e)
        {
            findCell("SalesOrderID", View, SalesOrderGrid);

        }

        private void findCell(string s, DataView v, DataGrid g)
        {
            int y = g.SelectedIndex;
            DataRow passToNextWindow = v[y].Row;
            try { string answer = ((string)View[y][s].ToString());
                Window detailView = new SalesOrderDetails(answer, passToNextWindow);

            }
            catch (Exception) { MessageBox.Show("Cannot return answer. \n The stars aren't aligned. Can't do it tonight. The stars. \n" + s);
            }

            //thing.Show();

        }

    }
}
