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
using EzTravelAdmin;
using EzTravelAdmin.WebService;

namespace EzTravelAdmin
{
    public partial class MainPage : UserControl
    {
        Service1Client WCF;
        public MainPage()
        {
            InitializeComponent();
            WCF = new WebService.Service1Client();
     
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = tbxUser.Text;
            string pass = pbPass.Password;

            WCF.checkLoginAsync(userName, pass);
            WCF.checkLoginCompleted +=new EventHandler<checkLoginCompletedEventArgs>(WCF_checkLoginCompleted);
  
        }

        void WCF_checkLoginCompleted(object sender, WebService.checkLoginCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                this.Content = new MainMenu();
            }
            else
            {
                MessageBox.Show("Invalid Password and Username!","Login", MessageBoxButton.OK);
                tbxUser.Text = "";
                pbPass.Password = "";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbxUser.Text = "";
            pbPass.Password = "";
        }

    }
}
