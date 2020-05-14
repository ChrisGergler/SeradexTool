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
        DataRow SalesOrder;
        Toolkit Utility = new Toolkit();

        //Gives Data table to populate and view
        DataTable Items = new DataTable("SalesOrderDetails");
        DataRow SalesOrderKeys;
        DataView View;

        // Used by Multiple methods
        public string salesOrderID;
        
        public SalesOrderDetails(string a, DataRow info)
        {
            InitializeComponent();
            SalesOrder = info;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string getItemDetails = "SELECT * FROM SalesOrderDetails";
            Items = Utility.useQuery(getItemDetails);
            View = new DataView(Items);
            ItemsQuoted.ItemsSource = View;

           //fillInfo();
        }






        ////////////////////////////////////// End
    }
}
