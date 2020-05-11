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
using System.Diagnostics;

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
        public string estimateID;

        public DetailView(string value)
        {
            InitializeComponent();
            estimateID = value;
            EstimateKeys = Utility.useQuery("SELECT * FROM Estimate WHERE Estimate.EstimateID = "+estimateID).NewRow();
        }

        //DataSet detailedInfo = new DataSet("Details");


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Items = Utility.useQuery(Utility.getItemDetails(estimateID));
            View = new DataView(Items);
            ItemsQuoted.ItemsSource = View;

            //Utility.useQuery("SELECT a.[Name] FROM Customers a, Estimate b WHERE b.CustomerID = a.CustomerID And b.EstimateID = 45" );

            //CustomerName.Content = row[0] + " Check";

           

        }

        void GoToFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", @"C:\test");
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening folder. Please find the folder manually.");
                return;
            }
        }


    private void On_Clicked(object sender, MouseButtonEventArgs e)
    {
            /*
            string findEstimate = EstimateNo.Text;
            string query = "SELECT a.EstimateID FROM Estimate a WHERE a.EstimateNo = " + findEstimate;
            itemList = Utility.useQuery(query);
            itemDisplay = new DataView(itemList);
            ItemsQuoted.ItemsSource = itemDisplay;
            */
            MessageBox.Show(Utility.getItemDetails(estimateID));
        }
    }
}
