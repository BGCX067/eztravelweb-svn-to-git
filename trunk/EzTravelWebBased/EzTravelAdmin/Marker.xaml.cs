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
using EzTravelAdmin.WebService;

namespace EzTravelAdmin
{
    public partial class Marker : Page
    {
        Service1Client WCF;
        public Marker()
        { 
            InitializeComponent();
            WCF = new WebService.Service1Client();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int jid = Convert.ToInt32(tbxJID.Text);
            WCF.retrieveMarkerAsync(jid);
            WCF.retrieveMarkerCompleted +=new EventHandler<retrieveMarkerCompletedEventArgs>(WCF_retrieveMarkerCompleted);

        }

        void WCF_retrieveMarkerCompleted(object sender, WebService.retrieveMarkerCompletedEventArgs e)
        {
            List<WebService.MarkerDB> mark = e.Result;
            DGInfoMarker.ItemsSource = mark;
            DGInfoMarker.AutoGenerateColumns = false;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Insert();
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MarkerDB m = (MarkerDB)DGInfoMarker.SelectedItem;
            this.Content = new Delete(m);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            MarkerDB m = (MarkerDB)DGInfoMarker.SelectedItem;
            this.Content = new Update(m);
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }

    }
}
