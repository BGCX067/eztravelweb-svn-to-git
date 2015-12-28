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
using System.Windows.Media.Imaging;
using System.IO;

namespace EzTravelAdmin
{
    public partial class Dining : Page
    {
        Service1Client WCF;
        WebService.Dining dining;
        byte[] img = null;
        byte[] bytes;
        byte[] preBytes = null;
        string DName = "";
        List<string> diningNameList = new List<string>();
        List<WebService.Dining> dList = new List<WebService.Dining>();
        bool editText = false;
        bool selected = false;
        
        public Dining()
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

        #region InsertTab

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            DName = tbxInsertDiningName.Text;
            WCF.checkDiningAsync(DName);
            WCF.checkDiningCompleted +=new EventHandler<checkDiningCompletedEventArgs>(WCF_checkDiningCompleted);

        }

        void WCF_checkDiningCompleted(object sender, WebService.checkDiningCompletedEventArgs e)
        {
            if(e.Result == true)
            {
                MessageBox.Show("Identical Data Found!", "Insert", MessageBoxButton.OK);
            }
            else
            {
                WebService.Dining inDining = new WebService.Dining();
                inDining.DName = tbxInsertDiningName.Text;
                inDining.DVenue = tbxInsertDiningVenue.Text;
                inDining.DLat = Convert.ToDecimal(tbxInsertDininglat.Text);
                inDining.DLong = Convert.ToDecimal(tbxInsertDininglong.Text);
                inDining.DDesc = tbxInsertDiningDesc.Text;
                img = bytes;
                inDining.DImage = img;

                WCF.InsertDiningAsync(inDining);
                WCF.InsertDiningCompleted += new EventHandler<InsertDiningCompletedEventArgs>(WCF_InsertDiningCompleted);
            }
        }
            
        void WCF_InsertDiningCompleted(object sender, InsertDiningCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Data has been entered");
                tbxInsertDiningName.Text = "";
                tbxInsertDiningVenue.Text = "";
                tbxInsertDininglat.Text = "";
                tbxInsertDininglong.Text = "";
                tbxInsertDiningDesc.Text = "";
                insertDiningImage.Source = null;
            }
            else
            {
                MessageBox.Show("Data is not being entered");
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
                insertDiningImage.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!");
            }
        }


        #endregion

        #region ManageTab

        private void tbxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadNameList();
            List<string> searchList = new List<string>();

            int wordCount = tbxName.Text.Length;

            if (wordCount == 0)
            {
                lbxSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (editText == true)
                {
                    lbxSearch.Visibility = Visibility.Visible;
                    foreach (string s in diningNameList)
                    {
                        if (s.Length >= wordCount)
                        {
                            string subString = s.Substring(0, wordCount);

                            if (tbxName.Text.Equals(subString, StringComparison.OrdinalIgnoreCase))
                            {
                                searchList.Add(s);
                            }
                        }
                    }
                    lbxSearch.ItemsSource = searchList;
                }
            }
        }

        void loadNameList()
        {
            WCF = new WebService.Service1Client();
            lbxSearch.ItemsSource = null;
            WCF.retrieveDiningNameAsync();
            WCF.retrieveDiningNameCompleted += new EventHandler<retrieveDiningNameCompletedEventArgs>(WCF_retrieveDiningNameCompleted);
        }

        void WCF_retrieveDiningNameCompleted(object sender, retrieveDiningNameCompletedEventArgs e)
        {
            diningNameList = e.Result;
        }


        private void tbxName_GotFocus(object sender, RoutedEventArgs e)
        {
            editText = true;
        }

        private void lbxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxSearch.SelectedItem != null)
            {
                tbxName.Text = lbxSearch.SelectedItem.ToString();
                lbxSearch.Visibility = Visibility.Collapsed;
                editText = false;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            DGDining.ItemsSource = null;
            WCF.retrieveDiningAsync(tbxName.Text);
            WCF.retrieveDiningCompleted += new EventHandler<retrieveDiningCompletedEventArgs>(WCF_retrieveDiningCompleted);
        }

        void WCF_retrieveDiningCompleted(object sender, retrieveDiningCompletedEventArgs e)
        {
            if (selected == false)
            {
                dList.Clear();
                DGDining.ItemsSource = null;
                dList.Add(e.Result);
                if (dList.Count >= 1)
                {
                    DGDining.ItemsSource = dList;
                    DGDining.AutoGenerateColumns = false;
                }
                else
                {
                    MessageBox.Show("No Information Found!", "Display Info", MessageBoxButton.OK);
                }
                selected = true;
            }
        }

        private void DGDining_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGDining.SelectedIndex == 0)
            {
                btnBUpdate.IsEnabled = true;
                btnBDelete.IsEnabled = true;
            }
        }

        private void btnBUpdate_Click(object sender, RoutedEventArgs e)
        {
            tabUpdate.Visibility = Visibility.Visible;
            tabControl.SelectedItem = tabUpdate;
            dining = (WebService.Dining)DGDining.SelectedItem;
            lblUpdateDiningName.Content = dining.DName;
            tbxUpdateDiningVenue.Text = dining.DVenue;
            tbxUpdateDininglong.Text = dining.DLong.ToString();
            tbxUpdateDininglat.Text = dining.DLat.ToString();
            tbxUpdateDiningDesc.Text = dining.DDesc;
            preBytes = dining.DImage;

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(dining.DImage);

            bmpImage.SetSource(mystream);

            updateDiningImage.Source = bmpImage;
        }

        private void btnBDelete_Click(object sender, RoutedEventArgs e)
        {
            tabDelete.Visibility = Visibility.Visible;
            tabControl.SelectedItem = tabDelete;
            dining = (WebService.Dining)DGDining.SelectedItem;
            lblDeleteDiningName.Content = dining.DName;
            lblDeleteDiningVenue.Content = dining.DVenue;
            lblDeleteDiningLong.Content = dining.DLong;
            lblDeleteDiningLat.Content = dining.DLat;
            lblDeleteDiningDesc.Content = dining.DDesc;
            preBytes = dining.DImage;

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(dining.DImage);

            bmpImage.SetSource(mystream);

            deleteDiningImage.Source = bmpImage;

        }

        #endregion

        #region UpdateTab

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            WebService.Dining upDining = new WebService.Dining();
            upDining.DName = lblUpdateDiningName.Content.ToString();
            upDining.DVenue = tbxUpdateDiningVenue.Text;
            upDining.DDesc = tbxUpdateDiningDesc.Text;
            upDining.DLat = Convert.ToDecimal(tbxUpdateDininglat.Text);
            upDining.DLong = Convert.ToDecimal(tbxUpdateDininglong.Text);


            if (bytes != null)
            {
                upDining.DImage = bytes;
            }
            else
            {
                upDining.DImage = preBytes;
            }
            selected = false;
            WCF.UpdateDiningAsync(upDining);
            WCF.UpdateDiningCompleted += new EventHandler<UpdateDiningCompletedEventArgs>(WCF_UpdateDiningCompleted);
        }

        void WCF_UpdateDiningCompleted(object sender, UpdateDiningCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Dining has been updated");
                tabControl.SelectedItem = tabManage;
                tabUpdate.Visibility = Visibility.Collapsed;
                WCF = new WebService.Service1Client();
                WCF.retrieveDiningAsync(lblUpdateDiningName.Content.ToString());
                WCF.retrieveDiningCompleted += new EventHandler<retrieveDiningCompletedEventArgs>(WCF_retrieveDiningCompleted);
            }
            else
            {
                MessageBox.Show("Error in updating dining");
            }
        }

        #endregion

        #region DeleteTab

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            WCF.DeleteDiningAsync(dining.DiningID);
            WCF.DeleteDiningCompleted += new EventHandler<DeleteDiningCompletedEventArgs>(WCF_DeleteDiningCompleted);
        }

        void WCF_DeleteDiningCompleted(object sender, DeleteDiningCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Dining has been deleted");
                tabControl.SelectedItem = tabManage;
                tabDelete.Visibility = Visibility.Collapsed;
                WCF = new WebService.Service1Client();
                WCF.retrieveDiningAsync(lblDeleteDiningName.Content.ToString());
                WCF.retrieveDiningCompleted += new EventHandler<retrieveDiningCompletedEventArgs>(WCF_retrieveDiningCompleted);
            }
            else
            {
                MessageBox.Show("Error in deleting dining");
            }
        }

        #endregion 

        private void btnUpdateImage_Click(object sender, RoutedEventArgs e)
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
                updateDiningImage.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!");
            }
        }
    }
}
