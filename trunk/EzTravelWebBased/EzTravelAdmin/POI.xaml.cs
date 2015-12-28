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
    public partial class POI : Page
    {

        #region Variables

        Service1Client WCF;
        string poiName = "";
        string poiAdd = "";
        double poiPrice = 0.0;
        double poiLat = 0.0;
        double poiLong = 0.0;
        string poiDes = "";
        double Child = 0.0;
        double Adult = 0.0;
        double Student = 0.0;
        double Elderly = 0.0;
        byte[] poiImg = null;
        List<string> poiNList = new List<string>();
        bool editText = false;
        byte[] bytes;
        byte[] prevBytes = null;
        bool selected = false;
        WebService.POI poi;
        string checkPrice = "";

        #endregion

        #region Load/Main

        public POI()
        {
            InitializeComponent();
         
            tabPOIControl.SelectedItem = tabPOIInsert;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }

        private void tabPOIInsert_LostFocus(object sender, RoutedEventArgs e)
        {
            tbxPOIName.Text = "";
            tbxPOIAddress.Text = "";
            tbxPOIPrice.Text = "";
            tbxPOILatitude.Text = "";
            tbxPOILongitude.Text = "";
            tbxPOIDescription.Text = "";
            tbxPOIChildPrice.Text = "";
            tbxPOIElderlyPrice.Text = "";
            tbxPOIAdultPrice.Text = "";
            tbxPOIStudentPrice.Text = "";
            rbStandard.IsChecked = false;
            rbPriceCat.IsChecked = false;
            gridStandard.Visibility = Visibility.Collapsed;
            gridPriceCat.Visibility = Visibility.Collapsed;
            uploadPOIImg.Source = null;
        }

        #endregion

        #region InsertTab

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            poiName = tbxPOIName.Text;
            WCF = new WebService.Service1Client();
            WCF.checkPOIAsync(poiName);
            WCF.checkPOICompleted+=new EventHandler<checkPOICompletedEventArgs>(WCF_checkPOICompleted);
        }

        void WCF_checkPOICompleted(object sender, WebService.checkPOICompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Identical Data found!", "Insert", MessageBoxButton.OK);
            }
            else
            {
                poiAdd = tbxPOIAddress.Text;
                poiLat = Convert.ToDouble(tbxPOILatitude.Text);
                poiLong = Convert.ToDouble(tbxPOILongitude.Text);
                poiDes = tbxPOIDescription.Text;

                if (rbStandard.IsChecked == true)
                {
                    checkPrice = "Standard";
                    poiPrice = Convert.ToDouble(tbxPOIPrice.Text);
                }
                else if (rbPriceCat.IsChecked == true)
                {
                    checkPrice = "Price Categories";
                    Child = Convert.ToDouble(tbxPOIChildPrice.Text);
                    Adult = Convert.ToDouble(tbxPOIAdultPrice.Text);
                    Student = Convert.ToDouble(tbxPOIStudentPrice.Text);
                    Elderly = Convert.ToDouble(tbxPOIElderlyPrice.Text);
                }
                else if (rbStandard.IsChecked == false && rbPriceCat.IsChecked == false)
                {
                    MessageBox.Show("Please choose price type and key in the following information!", "Select Price Type", MessageBoxButton.OK);
                }

                poiImg = bytes;

                poi = new WebService.POI();
                poi.Poi_Name = poiName;
                poi.Poi_Address = poiAdd;
                poi.Poi_Price = poiPrice;
                poi.Poi_Latitude = poiLat;
                poi.Poi_Longitude = poiLong;
                poi.Poi_Descript = poiDes;
                poi.Poi_Child_Price = Child;
                poi.Poi_Adult_Price = Adult;
                poi.Poi_Student_Price = Student;
                poi.Poi_Elderly_Price = Elderly;
                poi.Poi_Img = poiImg;
                poi.Price_Cat = checkPrice;
                WCF = new WebService.Service1Client();
                WCF.InsertPOIAsync(poi);
                WCF.InsertPOICompleted += new EventHandler<InsertPOICompletedEventArgs>(WCF_InsertPOICompleted);
            }
        }

        void WCF_InsertPOICompleted(object sender, WebService.InsertPOICompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("New POI Information Inserted Successfully!","New POI Information", MessageBoxButton.OK);
                tbxPOIName.Text = "";
                tbxPOIAddress.Text = "";
                tbxPOIPrice.Text = "";
                tbxPOILatitude.Text = "";
                tbxPOILongitude.Text = "";
                tbxPOIDescription.Text = "";
                tbxPOIChildPrice.Text = "";
                tbxPOIElderlyPrice.Text = "";
                tbxPOIAdultPrice.Text = "";
                tbxPOIStudentPrice.Text = "";
                rbStandard.IsChecked = false;
                rbPriceCat.IsChecked = false;
                uploadPOIImg.Source = null;
            }
            else
            {
                MessageBox.Show("Unable to enter POI information!","New POI Information", MessageBoxButton.OK);
            }
        }

        private void btnUploadPOI_Click(object sender, RoutedEventArgs e)
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
                uploadPOIImg.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!", "Upload Photo", MessageBoxButton.OK);
            }
        }

        private void rbStandard_Checked(object sender, RoutedEventArgs e)
        {
            gridStandard.Visibility = Visibility.Visible;
            gridPriceCat.Visibility = Visibility.Collapsed;
        }

        private void rbPriceCat_Checked(object sender, RoutedEventArgs e)
        {
            gridPriceCat.Visibility = Visibility.Visible;
            gridStandard.Visibility = Visibility.Collapsed;
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
                    foreach (string s in poiNList)
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
            WCF.retrievePOINameAsync();
            WCF.retrievePOINameCompleted += new EventHandler<retrievePOINameCompletedEventArgs>(WCF_retrievePOINameCompleted);
        }

        void WCF_retrievePOINameCompleted(object sender, WebService.retrievePOINameCompletedEventArgs e)
        {
            poiNList = e.Result;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            poiName = tbxSearch.Text;
            WCF = new WebService.Service1Client();
            WCF.retrievePOIAsync(poiName);
            WCF.retrievePOICompleted += new EventHandler<retrievePOICompletedEventArgs>(WCF_retrievePOICompleted);
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

        void WCF_retrievePOICompleted(object sender, WebService.retrievePOICompletedEventArgs e)
        {
            if (selected == false)
            {
                poi = new WebService.POI();
                poi = e.Result;
                if (poi != null)
                {
                    gridUpdate.Visibility = Visibility.Visible;
                    lblHiddenPOIID.Content = poi.Poi_ID;
                    lblUName.Content = poi.Poi_Name;
                    tbxUpdateAddress.Text = poi.Poi_Address;
                    tbxUpdateLatitude.Text = poi.Poi_Latitude.ToString();
                    tbxUpdateLongitude.Text = poi.Poi_Longitude.ToString();
                    tbxUpdateDescription.Text = poi.Poi_Descript;
            
                    checkPrice = poi.Price_Cat;

                    if (checkPrice.Equals("Standard"))
                    {
                        
                        rBStan.IsChecked = true;
                        tbxUpdateStandard.Text = poi.Poi_Price.ToString();
                    }
                    else if(checkPrice.Equals("Price Categories"))
                    {
                        rBPriceC.IsChecked = true;
                        tbxUpdateChildPrice.Text = poi.Poi_Child_Price.ToString();
                        tbxUpdateStudPrice.Text = poi.Poi_Student_Price.ToString();
                        tbxUpdateAdultPrice.Text = poi.Poi_Adult_Price.ToString();
                        tbxUpdateElderlyPrice.Text = poi.Poi_Elderly_Price.ToString();
                    }

                    prevBytes = poi.Poi_Img;
                    tbxSearch.Text = "";


                    BitmapImage bmpImage = new BitmapImage();
                    MemoryStream mystream = new MemoryStream(prevBytes);
                    bmpImage.SetSource(mystream);
                    ImgUpdated.Source = bmpImage;

                    btnUpdateInfo.IsEnabled = true;
                    btnBDelete.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("No Information Found!", "Display Info", MessageBoxButton.OK);
                }
                selected = true;
            }
        }

        private void rBStan_Checked(object sender, RoutedEventArgs e)
        {
            gridUpdateStan.Visibility = Visibility.Visible;
            gridUpdatePrice2.Visibility = Visibility.Collapsed;
        }

        private void rBPriceC_Checked(object sender, RoutedEventArgs e)
        {
            gridUpdateStan.Visibility = Visibility.Collapsed;
            gridUpdatePrice2.Visibility = Visibility.Visible;
        }

        #endregion

        #region UpdateTab

        private void btnUpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            poi = new WebService.POI();
            poi.Poi_ID = Convert.ToInt32(lblHiddenPOIID.Content);
            poi.Poi_Name = lblUName.Content.ToString();
            
            poi.Poi_Address = tbxUpdateAddress.Text;
            poi.Poi_Latitude = Convert.ToDouble(tbxUpdateLatitude.Text);
            poi.Poi_Longitude = Convert.ToDouble(tbxUpdateLongitude.Text);
            poi.Poi_Descript = tbxUpdateDescription.Text;

            if (rbStandard.IsChecked == true)
            {
                checkPrice = "Standard";
                poi.Poi_Price = Convert.ToDouble(tbxUpdateStandard.Text);
                tbxUpdateChildPrice.Text = "";
                tbxUpdateAdultPrice.Text = "";
                tbxUpdateElderlyPrice.Text = "";
                tbxUpdateStudPrice.Text = "";
            
                Child = Convert.ToDouble(tbxUpdateChildPrice.Text);
                Adult = Convert.ToDouble(tbxUpdateAdultPrice.Text);
                Elderly = Convert.ToDouble(tbxUpdateElderlyPrice.Text);
                Student = Convert.ToDouble(tbxUpdateStudPrice.Text);
                poi.Poi_Child_Price = Child;
                poi.Poi_Adult_Price = Adult;
                poi.Poi_Elderly_Price = Elderly;
                poi.Poi_Student_Price = Student;
            }
            else if (rbPriceCat.IsChecked == true)
            {
                checkPrice = "Price Categories";
                poi.Poi_Child_Price = Convert.ToDouble(tbxUpdateChildPrice.Text);
                poi.Poi_Adult_Price = Convert.ToDouble(tbxUpdateAdultPrice.Text);
                poi.Poi_Elderly_Price = Convert.ToDouble(tbxUpdateElderlyPrice.Text);
                poi.Poi_Student_Price = Convert.ToDouble(tbxUpdateStudPrice.Text);
                tbxUpdateStandard.Text = "";
                poiPrice = Convert.ToDouble(tbxUpdateStandard.Text);
                poi.Poi_Price = poiPrice;
            }

            poi.Price_Cat = checkPrice;

            if (bytes != null)
            {
                poi.Poi_Img = bytes;
            }
            else
            {
                poi.Poi_Img = prevBytes;
            }
            selected = false;
            WCF = new WebService.Service1Client();
            WCF.UpdatePOIAsync(poi);
            WCF.UpdatePOICompleted +=new EventHandler<UpdatePOICompletedEventArgs>(WCF_UpdatePOICompleted);
        }

        void WCF_UpdatePOICompleted(object sender, WebService.UpdatePOICompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("POI Information Updated Successfully!", "Update POI Info", MessageBoxButton.OK);

                btnUpdateInfo.IsEnabled = false;
                btnBDelete.IsEnabled = false;
                WCF = new WebService.Service1Client();
                WCF.retrievePOIAsync(lblUName.Content.ToString());
                WCF.retrievePOICompleted += new EventHandler<retrievePOICompletedEventArgs>(WCF_retrievePOICompleted);
            }
            else
            {
                MessageBox.Show("Unable to update POI information!", "Update POI Info", MessageBoxButton.OK);
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

        private void btnBDelete_Click(object sender, RoutedEventArgs e)
        {
            selected = false;
            WCF = new WebService.Service1Client();
            WCF.DeletePOIAsync(Convert.ToInt32(lblHiddenPOIID.Content));
            WCF.DeletePOICompleted += new EventHandler<DeletePOICompletedEventArgs>(WCF_DeletePOICompleted);
        }


        void WCF_DeletePOICompleted(object sender, DeletePOICompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("POI has been deleted!", "Delete POI Info", MessageBoxButton.OK);
                btnUpdateInfo.IsEnabled = false;
                btnBDelete.IsEnabled = false;

                lblHiddenPOIID.Content = "";
                lblUName.Content = "";
                tbxUpdateAddress.Text = "";
                tbxUpdateLatitude.Text = "";
                tbxUpdateLongitude.Text = "";
                tbxUpdateDescription.Text = "";
                checkPrice = "";
                rBStan.IsChecked = false;
                tbxUpdateStandard.Text = "";
                rBPriceC.IsChecked = false;
                tbxUpdateChildPrice.Text = "";
                tbxUpdateStudPrice.Text = "";
                tbxUpdateAdultPrice.Text = "";
                tbxUpdateElderlyPrice.Text = "";
                tbxSearch.Text = "";
                ImgUpdated.Source = null;
                gridUpdate.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Error in deleting POI!", "Delete POI Info", MessageBoxButton.OK);
            }
        }

        #endregion

    

    }
}
