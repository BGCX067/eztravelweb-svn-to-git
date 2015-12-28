using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace EzTravelAdmin
{
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage();
        }
     
        private void btnMarker_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Marker();
        }

        private void btnAccommodation_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Hotels();
        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new EventsTab();
        }

        private void btnPOI_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new POI();
        }

        private void btnEat_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Dining();
        }

        private void btnShopping_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ShopTab();
        }
    }
}
