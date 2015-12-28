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
    public partial class Delete : Page
    {
        Service1Client WCF;
        public Delete(MarkerDB m)
        {
            InitializeComponent();
            WCF = new WebService.Service1Client();
            tbkJID.Text = m.JID.ToString();
            tbkMarkerID.Text = m.MarkerID.ToString();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int Jid = Convert.ToInt32(tbkJID.Text);
            string markerID = tbkMarkerID.Text;

            WCF.DeleteMarkerAsync(Jid, markerID);
            WCF.DeleteMarkerCompleted +=new EventHandler<DeleteMarkerCompletedEventArgs>(WCF_DeleteMarkerCompleted);
            
        }

        void WCF_DeleteMarkerCompleted(object sender, DeleteMarkerCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Data has been deleted");
                this.Content = new Marker();
            }
            else
            {
                MessageBox.Show("Data has not being deleted");
            }
        }

    }
}
