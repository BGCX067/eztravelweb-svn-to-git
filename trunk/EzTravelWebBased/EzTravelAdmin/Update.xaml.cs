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
    public partial class Update : Page
    {
        Service1Client WCF;

        public Update(MarkerDB m)
        {
            InitializeComponent();
            WCF = new WebService.Service1Client();
            tbkJID.Text = m.JID.ToString();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            WebService.MarkerDB markerDB = new WebService.MarkerDB();
            markerDB.JID = Convert.ToInt32(tbkJID.Text);
            markerDB.MarkerID = tbxmarkerID.Text;
            markerDB.Direction = tbxDirection.Text;

            WCF.UpdateMarkerAsync(markerDB);
            WCF.UpdateMarkerCompleted +=new EventHandler<UpdateMarkerCompletedEventArgs>(WCF_UpdateMarkerCompleted);
        }

        void WCF_UpdateMarkerCompleted(object sender, WebService.UpdateMarkerCompletedEventArgs e)
        {
            if (e.Result==true)
            {
                MessageBox.Show("Data has been updated");
                this.Content = new Marker();
            }
            else
            {
                MessageBox.Show("Error updating data");
            }
            
        }

    }
}
