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

        DataTable SalesOrderTable = new DataTable("SalesOrderResults");
        DataView View;

        bool Loaded=false;

        public SalesOrders()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string salesOrderString = ""; //Query Goes here

            if (Loaded == false)
            {
                SalesOrderTable = Utility.useQuery(salesOrderString); // Pass Query here
                View = new DataView(SalesOrderTable);
                SalesOrderGrid.ItemsSource = View;

                Loaded = true;
            }
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            findCell("SalesOrderID", View, SalesOrderGrid);
        }

        private void findCell(string colName, DataView view, DataGrid grid)
        {
            // get Row Index 
            int y = grid.SelectedIndex;
            DataRow passToNextWindow = view[y].Row;
            string answer = "";
            try
            {
                answer = ((string)view[y][colName].ToString()); 
            }
            catch
            {
                MessageBox.Show("Steve, what did you do?");
            };
            //Show Detailed Sales order View

        }

        private void executeSearch(Object sender, KeyEventArgs e)
        {
            string searchString = "";
            switch (SearchParam.SelectedIndex)
            {
                case 0:
                    searchString = "SalesOrderNo LIKE '*" + SearchBox.Text + "*'";
                    break;

                case 1:

                    break;

                case 2:

                    break;

                case 3:

                    break;
            }
            View.RowFilter = searchString;
        }

    }
}
