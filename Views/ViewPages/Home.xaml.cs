using System;
using System.Windows;
using System.Windows.Controls;

namespace LSG_Databox.Views.ViewPages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Estimates_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("\\Views\\ViewPages\\Estimates\\Estimates.xaml", UriKind.Relative));
        }

        private void SalesOrders_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("\\Views\\ViewPages\\SalesOrders\\SalesOrders.xaml", UriKind.Relative));
        }

        private void PurchaseOrders_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("\\Views\\ViewPages\\PurchaseOrders\\PurchaseOrders.xaml", UriKind.Relative));
        }

        private void Invoices_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("\\Views\\ViewPages\\Invoices\\Invoices.xaml", UriKind.Relative));
        }
    }
}
