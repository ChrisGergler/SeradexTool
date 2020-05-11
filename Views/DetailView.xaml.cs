using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using SeradexToolv2.ViewModels;
using SeradexToolv2.Views.ViewPages.Estimates;

namespace SeradexToolv2.Views
{
    /// <summary>
    /// Interaction logic for DetailView.xaml
    /// </summary>
    public partial class DetailView : Window
    {
        EstimatesToolkit toolkit = new EstimatesToolkit();

        DataTable Data = new DataTable("EstimateDetails");

        DataView View;

        public string estimateID;

        public DetailView(string value)
        {
            InitializeComponent();
            estimateID = value;
            
        }

        DataSet detailedInfo = new DataSet("Details");


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           Data = toolkit.useQuery(toolkit.getItemDetails(estimateID));
           View = new DataView(Data);
           ItemsQuoted.ItemsSource = View;

           CustomerName.Content = toolkit.useQuery("SELECT x.[Name] FROM Estimate y, Customers x WHERE y.CustomerID = x.CustomerID"); 
            if(CustomerName.Content==null)
            {
                CustomerName.Content = "Panic";
            }

        }


        private void showItems()
        {


        }

    private void On_Clicked(object sender, MouseButtonEventArgs e)
    {
            /*
            string findEstimate = EstimateNo.Text;
            string query = "SELECT a.EstimateID FROM Estimate a WHERE a.EstimateNo = " + findEstimate;
            itemList = toolkit.useQuery(query);
            itemDisplay = new DataView(itemList);
            ItemsQuoted.ItemsSource = itemDisplay;
            */
            MessageBox.Show(toolkit.getItemDetails(estimateID));
        }
    }
}
