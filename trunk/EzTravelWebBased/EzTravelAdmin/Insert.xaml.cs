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
    public partial class Insert : Page
    {
        Service1Client WCF;
        public Insert()
        {
            InitializeComponent();
            
            // if the webservice manage to get the information 
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            int jid = 0;
            string markerid = "";
            string direction = "";
            WCF = new WebService.Service1Client();
            jid = Convert.ToInt32(tbxJID.Text);
            markerid = tbxMarkerID.Text;
            direction = tbxDirection.Text;

            WebService.MarkerDB markerDB = new WebService.MarkerDB();
            markerDB.JID = jid;
            markerDB.MarkerID = markerid;
            markerDB.Direction = direction;
            WCF.InsertMarkerAsync(markerDB);
            WCF.InsertMarkerCompleted +=new EventHandler<InsertMarkerCompletedEventArgs>(WCF_InsertMarkerCompleted);
            
        }

        void WCF_InsertMarkerCompleted(object sender, WebService.InsertMarkerCompletedEventArgs e)
        {
            if (e.Result==true)
            {
                MessageBox.Show("Data has been entered");
                this.Content = new Marker();
            }
            else
            {
                MessageBox.Show("Data is not being entered");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }


    }
}
