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
    public partial class Hotels : Page
    {
    
        #region Variables

        EzTravelAdmin.WebService.Service1Client WCF;
        string h_Name = "";
        string h_Address = "";
        double h_Latitude = 0.0;
        double h_Longitude = 0.0;
        string h_descript = "";
        string h_Facilities = "";
        double LowPrice = 0.0;
        double HighPrice = 0.0;
        byte[] img = null;
        List<string> Dictionary = new List<string>();
        bool editText = false;
        Accommodation acc;
        byte[] bytes = null;
        byte[] prevBytes = null;
        bool selected = false;
        string checkSwim = "";
        string checkRestaurant = "";
        string checkFitness = "";
        string checkJacuzzi = "";
        string checkCon = "";
        string checkShop = "";
        string checkKid = "";

        #endregion

        #region Load/Main

        public Hotels()
        {
            InitializeComponent();
           
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }

        #endregion

        #region InsertTab

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            h_Name = tbxName.Text;
            WCF = new WebService.Service1Client();
            WCF.checkAccAsync(h_Name);
            WCF.checkAccCompleted+=new EventHandler<checkAccCompletedEventArgs>(WCF_checkAccCompleted);
        }

        void WCF_checkAccCompleted(object sender, WebService.checkAccCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Identical Data found!", "Insert", MessageBoxButton.OK);
            }
            else
            {
                string oFac = "";
                h_Address = tbxAddress.Text;
                LowPrice = Convert.ToDouble(tbxinlowPrice.Text);
                h_Latitude = Convert.ToDouble(tbxLatitude.Text);
                h_Longitude = Convert.ToDouble(tbxLongitude.Text);
                h_descript = tbxDescription.Text;
                HighPrice = Convert.ToDouble(tbxinHighPrice.Text);
                img = bytes;
                acc = new Accommodation();

                if (LowPrice < HighPrice)
                {
                    acc.H_Name = h_Name;
                    acc.H_Address = h_Address;
                    acc.H_Latitude = h_Latitude;
                    acc.H_Longitude = h_Longitude;
                    acc.H_descript = h_descript;

                    if (chkSwim.IsChecked == true)
                    {
                        checkSwim = chkSwim.Content.ToString();
                    }
                    
                    if (chkShopping.IsChecked == true)
                    {
                        checkShop = chkShopping.Content.ToString();
                    }

                    if (chkRestaurants.IsChecked == true)
                    {
                        checkRestaurant = chkRestaurants.Content.ToString();
                    }
                    
                    if (chkKids.IsChecked == true)
                    {
                        checkKid = chkKids.Content.ToString();
                    }
                                    
                    if (chkJacuzzi.IsChecked == true)         
                    {
                        checkJacuzzi = chkJacuzzi.Content.ToString();
                    }

                    if (chkFitness.IsChecked == true)
                    {
                        checkFitness = chkFitness.Content.ToString();
                    }

                    if (chkConference.IsChecked == true)
                    {
                        checkCon = chkConference.Content.ToString();
                    }

                    if (!tbxFacilities.Text.Equals(""))
                    {
                        oFac = tbxFacilities.Text;
                    }
                    
                    h_Facilities = checkSwim;
                    h_Facilities += " " +checkShop;
                    h_Facilities += " " +checkRestaurant;
                    h_Facilities += " " +checkKid;
                    h_Facilities += " " +checkJacuzzi;
                    h_Facilities += " " +checkFitness;
                    h_Facilities += " " +checkCon;
                    h_Facilities += " " +oFac;

                    acc.H_Facilities = h_Facilities;
                    acc.H_Img = img;
                    acc.Low_price = LowPrice;
                    acc.High_price = HighPrice;
                    WCF = new WebService.Service1Client();
                    WCF.InsertAccommodationAsync(acc);
                    WCF.InsertAccommodationCompleted += new EventHandler<InsertAccommodationCompletedEventArgs>(WCF_InsertAccommodationCompleted);
                }
                else
                {
                    MessageBox.Show("The value of low price cannot be more than the value of high price!", "Error Message", MessageBoxButton.OK);
                }
            }
        }
        

        void WCF_InsertAccommodationCompleted(object sender, WebService.InsertAccommodationCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("New Hotel Information Inserted Successfully!","New Hotel Information", MessageBoxButton.OK);
                tbxName.Text = "";
                tbxAddress.Text = "";
                tbxinlowPrice.Text = "";
                tbxLatitude.Text = "";
                tbxLongitude.Text = "";
                tbxDescription.Text = "";
                tbxFacilities.Text = "";
                tbxinHighPrice.Text = "";
                uploadImg.Source = null;
            }
            else
            {
                MessageBox.Show("Unable to enter hotel information!","New Hotel Information", MessageBoxButton.OK);
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
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
                uploadImg.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!", "Upload Photo", MessageBoxButton.OK);
            }
        }

        #endregion
     
        #region ManageTab

        private void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadDictionary();
            List<string> searchList = new List<string>();

            int wordCount = tbxSearch.Text.Length;

            if (wordCount == 0)
            {
                lbxSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (editText == true)
                {
                    lbxSearch.Visibility = Visibility.Visible;
                    foreach (string s in Dictionary)
                    {
                        if (s.Length >= wordCount)
                        {
                            string subString = s.Substring(0, wordCount);

                            if (tbxSearch.Text.Equals(subString, StringComparison.OrdinalIgnoreCase))
                            {
                                searchList.Add(s);
                            }
                        }
                    }
                    lbxSearch.ItemsSource = searchList;
                }
            }
        }

        void loadDictionary()
        {
            lbxSearch.ItemsSource = null;
            WCF = new WebService.Service1Client();
            WCF.retrieveDictionaryAsync();
            WCF.retrieveDictionaryCompleted += new EventHandler<retrieveDictionaryCompletedEventArgs>(WCF_retrieveDictionaryCompleted);
        }

        void WCF_retrieveDictionaryCompleted(object sender, WebService.retrieveDictionaryCompletedEventArgs e)
        {
            Dictionary = e.Result;
        }

        private void lbxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxSearch.SelectedItem != null)
            {
                tbxSearch.Text = lbxSearch.SelectedItem.ToString();
                lbxSearch.Visibility = Visibility.Collapsed;
                editText = false;
            }
        }

        private void tbxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            editText = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            h_Name = tbxSearch.Text;
            WCF = new WebService.Service1Client();
            WCF.retrieveHotelAsync(h_Name);
            WCF.retrieveHotelCompleted += new EventHandler<retrieveHotelCompletedEventArgs>(WCF_retrieveHotelCompleted);
        }

        void WCF_retrieveHotelCompleted(object sender, WebService.retrieveHotelCompletedEventArgs e)
        {
            if (selected == false)
            {
                acc = new Accommodation();
                acc = e.Result;
                if (acc != null)
                {
                    lblHiddenID.Content = acc.HID;
                    lblName.Content = acc.H_Name;
                    tbxUpdateAddress.Text = acc.H_Address;
                    tbxUpdatePrice.Text = acc.Low_price.ToString();
                    tbxUpdateLatitude.Text = acc.H_Latitude.ToString();
                    tbxUpdateLongitude.Text = acc.H_Longitude.ToString();
                    tbxUpdateDescription.Text = acc.H_descript;
                    tbxUpdateFacilities.Text = acc.H_Facilities;
                    tbxupdatehighprice.Text = acc.High_price.ToString();
                    prevBytes = acc.H_Img;
                    tbxSearch.Text = "";

                    BitmapImage bmpImage = new BitmapImage();

                    MemoryStream mystream = new MemoryStream(prevBytes);

                    bmpImage.SetSource(mystream);

                    ImgUpdated.Source = bmpImage;

                    btnUpdate.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("No Information Found!","Display Info", MessageBoxButton.OK);
                }
                selected = true;
            }
        }

        #endregion

        #region UpdateTab

        private void btnUpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            acc = new Accommodation();

            LowPrice = Convert.ToDouble(tbxUpdatePrice.Text);
            HighPrice = Convert.ToDouble(tbxupdatehighprice.Text);

            if (LowPrice < HighPrice)
            {
                acc.HID = Convert.ToInt32(lblHiddenID.Content);
                acc.H_Name = lblName.Content.ToString();
                acc.H_Address = tbxUpdateAddress.Text;
                acc.Low_price = LowPrice;
                acc.H_Latitude = Convert.ToDouble(tbxUpdateLatitude.Text);
                acc.H_Longitude = Convert.ToDouble(tbxUpdateLongitude.Text);
                acc.H_descript = tbxUpdateDescription.Text;
                acc.H_Facilities = tbxUpdateFacilities.Text;
                acc.High_price = HighPrice;

                if (bytes != null)
                {
                    acc.H_Img = bytes;
                }
                else
                {
                    acc.H_Img = prevBytes;
                }
                selected = false;
                WCF = new WebService.Service1Client();
                WCF.UpdateAccommodationAsync(acc);
                WCF.UpdateAccommodationCompleted += new EventHandler<UpdateAccommodationCompletedEventArgs>(WCF_UpdateAccommodationCompleted);
            }
            else
            {
                 MessageBox.Show("The value of low price cannot be more than the value of high price!", "Error Message", MessageBoxButton.OK);
            }
        }

        void WCF_UpdateAccommodationCompleted(object sender, WebService.UpdateAccommodationCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Hotel Information Updated Successfully!","Update Hotel Info",MessageBoxButton.OK);
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
                WCF = new WebService.Service1Client();
                WCF.retrieveHotelAsync(lblName.Content.ToString());
                WCF.retrieveHotelCompleted += new EventHandler<retrieveHotelCompletedEventArgs>(WCF_retrieveHotelCompleted);
            }
            else
            {
                MessageBox.Show("Unable to update hotel information!", "Update Hotel Info", MessageBoxButton.OK);
            }
        }

        private void btnUpdateUpload_Click(object sender, RoutedEventArgs e)
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
                ImgUpdated.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!", "Upload Photo", MessageBoxButton.OK);
            }
        }

        #endregion

        #region DeleteTab


        private void btnDeleteInfo_Click(object sender, RoutedEventArgs e)
        {
            selected = false;
            WCF = new WebService.Service1Client();
            WCF.DeleteAccommodationAsync(Convert.ToInt32(lblHiddenID.Content));
            WCF.DeleteAccommodationCompleted += new EventHandler<DeleteAccommodationCompletedEventArgs>(WCF_DeleteAccommodationCompleted);
        }

        void WCF_DeleteAccommodationCompleted(object sender, DeleteAccommodationCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Hotel has been deleted!", "Delete Hotel Info", MessageBoxButton.OK);
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
                lblHiddenID.Content = "";
                lblName.Content = "";
                tbxUpdateAddress.Text = "";
                tbxUpdatePrice.Text = "";
                tbxUpdateLatitude.Text = "";
                tbxUpdateLongitude.Text = "";
                tbxUpdateDescription.Text = "";
                tbxUpdateFacilities.Text = "";
                tbxupdatehighprice.Text = "";
                prevBytes = null;
                
            }
            else
            {
                MessageBox.Show("Error in deleting hotel!", "Delete Hotel Info", MessageBoxButton.OK);
            }
        }

        #endregion


    }
}
