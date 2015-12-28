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
using System.IO;
using System.Windows.Media.Imaging;

namespace EzTravelAdmin
{
    public partial class ShopTab : Page
    {
        Service1Client WCF;
        WebService.Shopping shop ;
        byte[] img = null;
        byte[] bytes;
        byte[] preBytes = null;
        bool selected = false;
        string SName = "";
        List<Shopping> sp = new List<Shopping>();

        public ShopTab()
        {
            InitializeComponent();
            btnBUpdate.IsEnabled = false;
            btnBDelete.IsEnabled = false;
            tabUpdate.Visibility = Visibility.Collapsed;
            tabDelete.Visibility = Visibility.Collapsed;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
            tabUpdate.Visibility = Visibility.Collapsed;
            tabDelete.Visibility = Visibility.Collapsed;
        }

        # region InsertTab

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            SName = tbxInsertName.Text;
            WCF.checkShopAsync(SName);
            WCF.checkShopCompleted +=new EventHandler<checkShopCompletedEventArgs>(WCF_checkShopCompleted);
        }

        void WCF_checkShopCompleted(object sender, WebService.checkShopCompletedEventArgs e)
        {
            if(e.Result == true)
            {
                MessageBox.Show("Identical Data Found!","Insert",MessageBoxButton.OK);
            }
            else
            {
            string svenue = tbxSVenue.Text;
                decimal slong = Convert.ToDecimal(tbxSlong.Text);
                decimal slat = Convert.ToDecimal(tbxSlat.Text);
                string desc = tbxDesc.Text;
                img = bytes;

                WebService.Shopping Insertshop = new WebService.Shopping();
                Insertshop.SName = SName;
                Insertshop.SVenue = svenue;
                Insertshop.SLong = slong;
                Insertshop.SLat = slat;
                Insertshop.SDesc = desc;
                Insertshop.SImage = img;
                WCF.InsertShoppingAsync(Insertshop);
                WCF.InsertShoppingCompleted +=new EventHandler<InsertShoppingCompletedEventArgs>(WCF_InsertShoppingCompleted);

            }
        }

        void WCF_InsertShoppingCompleted(object sender, WebService.InsertShoppingCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Data has been entered");
                tbxInsertName.Text = "";
                tbxSVenue.Text = "";
                tbxSlong.Text = "";
                tbxSlat.Text = "";
                tbxDesc.Text = "";
                insertimage.Source = null;
            }
            else
            {
                MessageBox.Show("Data is not being entered");
            }

        }

        private void btnInsertImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG files|*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                Stream stream = (Stream)openFileDialog.File.OpenRead();
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);

                string fileName = openFileDialog.File.Name;

                BitmapImage imageSource = new BitmapImage();

                imageSource.SetSource(openFileDialog.File.OpenRead());
                insertimage.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!");
            }

        }

        #endregion

        #region ManageTab

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            string sname = tbxName.Text;
            WCF.retrieveShoppingAsync(sname);
            WCF.retrieveShoppingCompleted +=new EventHandler<retrieveShoppingCompletedEventArgs>(WCF_retrieveShoppingCompleted);
        }

        void WCF_retrieveShoppingCompleted(object sender, retrieveShoppingCompletedEventArgs e)
        {
            if (selected == false)
            {
                sp.Clear();
                DGShop.ItemsSource = null;
                sp.Add(e.Result);
                if (sp.Count >= 1)
                {
                    DGShop.ItemsSource = sp;
                    DGShop.AutoGenerateColumns = false;
                }
                else
                {
                    MessageBox.Show("No Information Found!", "Display Info", MessageBoxButton.OK);
                }
                selected = true;
            }
        }

        private void DGShop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGShop.SelectedIndex == 0)
            {
                btnBUpdate.IsEnabled = true;
                btnBDelete.IsEnabled = true;
            }
        }

        private void btnBUpdate_Click(object sender, RoutedEventArgs e)
        {
            tabUpdate.Visibility = Visibility.Visible;
            tabControlShop.SelectedItem = tabUpdate;
            shop = new Shopping();
            shop = (Shopping)DGShop.SelectedItem;
            lblShoppName.Content = shop.SName;
            tbxShopVenue.Text = shop.SVenue;
            tbxShoplong.Text = shop.SLong.ToString();
            tbxShoplat.Text = shop.SLat.ToString();
            tbxShopDesc.Text = shop.SDesc;
            preBytes = shop.SImage;
            lblHidden.Content = shop.SID.ToString();

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(shop.SImage);

            bmpImage.SetSource(mystream);

            UpdateImg.Source = bmpImage;
        }

        private void btnBDelete_Click(object sender, RoutedEventArgs e)
        {
            tabDelete.Visibility = Visibility.Visible;
            tabControlShop.SelectedItem = tabDelete;
            shop = new Shopping();
            shop = (Shopping)DGShop.SelectedItem;
            lblSpName.Content = shop.SName;
            lblSpVenue.Content = shop.SVenue;
            lblSpLong.Content = shop.SLong.ToString();
            lblSpLat.Content = shop.SLat.ToString();
            lblSpDesc.Content = shop.SDesc;
            preBytes = shop.SImage;
            lblHidden2.Content = shop.SID.ToString();

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(shop.SImage);

            bmpImage.SetSource(mystream);

            DeleteImg.Source = bmpImage;
        }

        #endregion

        #region UpdateTab

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            shop = new Shopping();
            shop.SName = lblShoppName.Content.ToString();
            shop.SVenue = tbxShopVenue.Text;
            shop.SLong = Convert.ToDecimal(tbxShoplong.Text);
            shop.SLat = Convert.ToDecimal(tbxShoplat.Text);
            shop.SDesc = tbxShopDesc.Text;


            if (bytes != null)
            {
                shop.SImage = bytes;
            }
            else
            {
                shop.SImage = preBytes;
            }

            WCF = new WebService.Service1Client();
            selected = false;
            WCF.UpdateShoppingAsync(shop);
            WCF.UpdateShoppingCompleted +=new EventHandler<UpdateShoppingCompletedEventArgs>(WCF_UpdateShoppingCompleted);
        }

        void WCF_UpdateShoppingCompleted(object sender, UpdateShoppingCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Shops has been updated");
                tabControlShop.SelectedItem = tabManage;
                tabUpdate.Visibility = Visibility.Collapsed;

                WCF = new WebService.Service1Client();
                WCF.retrieveShoppingAsync(lblShoppName.Content.ToString());
                WCF.retrieveShoppingCompleted+=new EventHandler<retrieveShoppingCompletedEventArgs>(WCF_retrieveShoppingCompleted);
            }
            else
            {
                MessageBox.Show("Error in updating Shop");
            }
        }

        private void btnUpdateImage_Click(object sender, RoutedEventArgs e)
        {
            UpdateImg.Source = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG files|*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                Stream stream = (Stream)openFileDialog.File.OpenRead();
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);

                string fileName = openFileDialog.File.Name;

                BitmapImage imageSource = new BitmapImage();

                imageSource.SetSource(openFileDialog.File.OpenRead());
                UpdateImg.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!");
            }

        }


        #endregion

        #region DeleteTab

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            WCF.DeleteShoppingAsync(Convert.ToInt32(lblHidden2.Content));
            WCF.DeleteShoppingCompleted +=new EventHandler<DeleteShoppingCompletedEventArgs>(WCF_DeleteShoppingCompleted);
        }

        void WCF_DeleteShoppingCompleted(object sender, DeleteShoppingCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Shop has been deleted");
                tabControlShop.SelectedItem = tabManage;
                tabDelete.Visibility = Visibility.Collapsed;

                WCF = new WebService.Service1Client();
                WCF.retrieveShoppingAsync(lblSpName.Content.ToString());
                WCF.retrieveShoppingCompleted += new EventHandler<retrieveShoppingCompletedEventArgs>(WCF_retrieveShoppingCompleted);
            }
            else
            {
                MessageBox.Show("Error in deleting shop");
            }
        }

        

        #endregion



    }
}