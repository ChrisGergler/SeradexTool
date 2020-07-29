using LSG_Databox.ViewModels;
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

namespace LSG_Databox.Views.ViewPages.Invoices
{
    /// <summary>
    /// Interaction logic for Invoices.xaml
    /// </summary>
    public partial class Invoices : Page
    {
        Toolkit Utilities = new Toolkit();

        DataTable invoiceSearch = new DataTable("invoiceSearch");
        DataView view;
        DataRow pass;
        bool loaded = false;

        public Invoices()
        {
            InitializeComponent();
        }


        // Utilized for finding the Invoice Number cell based on any doubleclick in the grids
        private void findCell(string s, DataView v, DataGrid g)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
