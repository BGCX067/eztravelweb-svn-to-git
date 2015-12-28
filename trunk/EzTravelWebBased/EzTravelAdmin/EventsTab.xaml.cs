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
using System.Windows.Data;
using System.Globalization;

namespace EzTravelAdmin
{
    public partial class EventsTab : Page
    {
        Service1Client WCF;
        WebService.Event events;
        byte[] img = null;
        byte[] bytes;
        byte[] preBytes = null;
        string ename = "";
        string evenue = "";
        DateTime startDate;
        DateTime endDate;
        List<Event> eve = new List<Event>();
        bool selected = false;

        public EventsTab()
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
           ename = tbxEName.Text;
           WCF.checkEventAsync(ename);
           WCF.checkEventCompleted +=new EventHandler<checkEventCompletedEventArgs>(WCF_checkEventCompleted);
        }
        void WCF_checkEventCompleted(object sender, WebService.checkEventCompletedEventArgs e)
        {
            if(e.Result == true)
            {
                MessageBox.Show("Identical Data found!","Insert",MessageBoxButton.OK);
            }
            else
            {
               decimal price = 0;
               if (!tbxPrice.Text.Equals(""))
               {
                   price = Convert.ToDecimal(tbxPrice.Text);
               }
               decimal elong = Convert.ToDecimal(tbxElong.Text);

               decimal elat = Convert.ToDecimal(tbxElat.Text);
               string desc = tbxDesc.Text;
               img = bytes;
               decimal lowprice = 0;
               if(!tbxinLowPrice.Text.Equals(""))
               {
                   lowprice = Convert.ToDecimal(tbxinLowPrice.Text);
               }

               decimal highprice = 0;
               if (!tbxinHighPrice.Text.Equals(""))
               {
                   highprice = Convert.ToDecimal(tbxinHighPrice.Text);
               }
               evenue = tbxEVenue.Text;
               DateTime StartTime = Convert.ToDateTime(tbxinStartTime.Text);
               DateTime EndTime = Convert.ToDateTime(tbxinEndTime.Text);


               WebService.Event upEvent = new WebService.Event();
               if (lowprice < highprice)
               {
                   upEvent.EName = ename;
                   upEvent.EVenue = evenue;
                   upEvent.EPrice = price;
                   upEvent.ELong = elong;
                   upEvent.ELat = elat;
                   upEvent.EDesc = desc;
                   upEvent.EImage = img;
                   upEvent.LowPrice = lowprice;
                   upEvent.HighPrice = highprice;
                   upEvent.StartDate = startDate;
                   upEvent.EndDate = endDate;
                   upEvent.StartTime = StartTime;
                   upEvent.EndTime = EndTime;
                   WCF.InsertEventAsync(upEvent);
                   WCF.InsertEventCompleted += new EventHandler<InsertEventCompletedEventArgs>(WCF_InsertEventCompleted);
               }
               else if (lowprice == highprice)
               {
                   upEvent.EName = ename;
                   upEvent.EVenue = evenue;
                   upEvent.EPrice = price;
                   upEvent.ELong = elong;
                   upEvent.ELat = elat;
                   upEvent.EDesc = desc;
                   upEvent.EImage = img;
                   upEvent.LowPrice = lowprice;
                   upEvent.HighPrice = highprice;
                   upEvent.StartDate = startDate;
                   upEvent.EndDate = endDate;
                   upEvent.StartTime = StartTime;
                   upEvent.EndTime = EndTime;
                   WCF.InsertEventAsync(upEvent);
                   WCF.InsertEventCompleted += new EventHandler<InsertEventCompletedEventArgs>(WCF_InsertEventCompleted);
               }
               else
               {
                   MessageBox.Show("The value of low price cannot be more than the value of high price!", "Error Message", MessageBoxButton.OK);
               }
                }
            }
           
        void WCF_InsertEventCompleted(object sender, WebService.InsertEventCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Data has been entered");
                tbxEName.Text = "";
                tbxEVenue.Text = "";
                tbxPrice.Text = "";
                tbxElong.Text = "";
                tbxElat.Text = "";
                tbxDesc.Text = "";
                insertImage.Source = null;
                tbxinLowPrice.Text = "";
                tbxinHighPrice.Text = "";
                tbxinStartDate.Text ="";
                tbxinEndTime.Text = "";
                tbxinEndDate.Text = "";
                tbxinStartTime.Text = "";

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
                insertImage.Source = imageSource;
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
            string ename = tbxName.Text;
            WCF.retrieveEventAsync(ename);
            WCF.retrieveEventCompleted += new EventHandler<retrieveEventCompletedEventArgs>(WCF_retrieveEventCompleted);
        }

        void WCF_retrieveEventCompleted(object sender, retrieveEventCompletedEventArgs e)
        {
            if (selected == false)
            {
                eve.Clear();
                DGEvents.ItemsSource = null;
                eve.Add(e.Result);
                if (eve.Count >= 1)
                {
                    DGEvents.ItemsSource = eve;
                    DGEvents.AutoGenerateColumns = false;
                }
                else
                {
                    MessageBox.Show("No Information Found!", "Display Info", MessageBoxButton.OK);
                }
                selected = true;
            }
        }

        private void DGEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGEvents.SelectedIndex == 0)
            {
                btnBUpdate.IsEnabled = true;
                btnBDelete.IsEnabled = true;

            }
        }

        #endregion

        #region UpdateTab

        private void btnBUpdate_Click(object sender, RoutedEventArgs e)
        {
            tabUpdate.Visibility = Visibility.Visible;
            tabControl.SelectedItem = tabUpdate;
            events = (Event)DGEvents.SelectedItem;
            lblEvName.Content = events.EName;
            tbxEventVenue.Text = events.EVenue;
            tbxEventPrice.Text = events.EPrice.ToString();
            tbxEventlong.Text = events.ELong.ToString();
            tbxEventlat.Text = events.ELat.ToString();
            tbxEventDesc.Text = events.EDesc;
            tbxupHighPrice.Text = events.HighPrice.ToString();
            tbxupLowPrice.Text = events.LowPrice.ToString();
            tbxupStartDate.Text = events.StartDate.ToShortDateString();
            tbxupEndDate.Text = events.EndDate.ToShortDateString(); ;
            tbxupStartTime.Text = events.StartTime.ToShortTimeString();
            tbxupEndTime.Text = events.EndTime.ToShortTimeString();
            preBytes = events.EImage;

            BitmapImage bmpImage = new BitmapImage();
            MemoryStream mystream = new MemoryStream(events.EImage);
            bmpImage.SetSource(mystream);
            updateImage.Source = bmpImage;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            WebService.Event upEvent = new WebService.Event();
            decimal high = Convert.ToDecimal(tbxupHighPrice.Text);
            decimal low = Convert.ToDecimal(tbxupLowPrice.Text);
            if (low < high)
            {
                upEvent.EName = lblEvName.Content.ToString();
                upEvent.EVenue = tbxEventVenue.Text;
                upEvent.EPrice = Convert.ToDecimal(tbxEventPrice.Text);
                upEvent.ELong = Convert.ToDecimal(tbxEventlong.Text);
                upEvent.ELat = Convert.ToDecimal(tbxEventlat.Text);
                upEvent.EDesc = tbxEventDesc.Text;
                upEvent.LowPrice = low;
                upEvent.HighPrice = high;
                upEvent.StartDate = Convert.ToDateTime(tbxupStartDate.Text);
                upEvent.EndDate = Convert.ToDateTime(tbxupEndDate.Text);
                upEvent.StartTime = Convert.ToDateTime(tbxupStartTime.Text);
                upEvent.EndTime = Convert.ToDateTime(tbxupEndTime.Text);

                if (bytes != null)
                {
                    upEvent.EImage = bytes;
                }
                else
                {
                    upEvent.EImage = preBytes;
                }
                selected = false;
                WCF = new WebService.Service1Client();
                WCF.UpdateEventAsync(upEvent);
                WCF.UpdateEventCompleted += new EventHandler<UpdateEventCompletedEventArgs>(WCF_UpdateEventCompleted);
            }
            else
            {
                MessageBox.Show("The value of low price cannot be more than the value of high price!", "Error Message", MessageBoxButton.OK);
            }
            
        }

        void WCF_UpdateEventCompleted(object sender, UpdateEventCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Event has been updated");
                tabControl.SelectedItem = tabManage;
                tabUpdate.Visibility = Visibility.Collapsed;
                WCF = new WebService.Service1Client();
                WCF.retrieveEventAsync(lblEvName.Content.ToString());
                WCF.retrieveEventCompleted +=new EventHandler<retrieveEventCompletedEventArgs>(WCF_retrieveEventCompleted);


            }
            else
            {
                MessageBox.Show("Error in updating Event");
            }
        }

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
                updateImage.Source = imageSource;
            }
            else
            {
                MessageBox.Show("Please press upload button again!");
            }
        }






        #endregion

        #region DeleteTab


        private void btnBDelete_Click(object sender, RoutedEventArgs e)
        {
            tabDelete.Visibility = Visibility.Visible;
            tabControl.SelectedItem = tabDelete;
            events = (Event)DGEvents.SelectedItem;
            lblEvenName.Content = events.EName;
            lblEvenVenue.Content = events.EVenue;
            lblEvenPrice.Content = events.EPrice;
            lblEvenLong.Content = events.ELong;
            lblEvenLat.Content = events.ELat;
            lblEvenDesc.Content = events.EDesc;
            lblEID.Content = events.EID;
            lbldeHighPrice.Content = events.HighPrice;
            lbldeLowPrice.Content = events.LowPrice;
            lbldeStartDate.Content = events.StartDate.ToShortDateString();
            lbldeEndDate.Content = events.EndDate.ToShortDateString();
            lbldeStartTime.Content = events.StartTime.ToShortTimeString();
            lbldeEndTime.Content = events.EndTime.ToShortTimeString();
            preBytes = events.EImage;

            if (lbldeHighPrice.Content.Equals("0"))
            {
                lbldeHighPrice.Visibility = Visibility.Collapsed;
                lbldeleteHighPrice.Visibility = Visibility.Collapsed;
            }

            if (lbldeLowPrice.Content.Equals("0"))
            {
                lbldeLowPrice.Visibility = Visibility.Collapsed;
                lbldeleteLowPrice.Visibility = Visibility.Collapsed;
            }

            if (lblEvenPrice.Content.Equals("0"))
            {
                lblEvenPrice.Visibility = Visibility.Collapsed;
                lblEvePrice.Visibility = Visibility.Collapsed;
            }

            BitmapImage bmpImage = new BitmapImage();
            MemoryStream mystream = new MemoryStream(events.EImage);
            bmpImage.SetSource(mystream);
            ImgDeleteImage.Source = bmpImage;
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            WCF = new WebService.Service1Client();
            WCF.DeleteEventAsync(events.EID);
            WCF.DeleteEventCompleted += new EventHandler<DeleteEventCompletedEventArgs>(WCF_DeleteEventCompleted);
        }

        void WCF_DeleteEventCompleted(object sender, DeleteEventCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Event has been deleted");
                tabControl.SelectedItem = tabManage;
                tabDelete.Visibility = Visibility.Collapsed;
                WCF = new WebService.Service1Client();
                WCF.retrieveEventAsync(lblEvenName.Content.ToString());
                WCF.retrieveEventCompleted +=new EventHandler<retrieveEventCompletedEventArgs>(WCF_retrieveEventCompleted);
                
            }
            else
            {
                MessageBox.Show("Error in deleting event");
            }
        }


        #endregion


        private void tbxinEndTime_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxinStartTime != null)
            {
                DateTime startTime = Convert.ToDateTime(tbxinStartTime.Text);
                DateTime endTime = Convert.ToDateTime(tbxinEndTime.Text);
                if (startTime > endTime)
                {
                    MessageBox.Show("Start Time should be earlier than End Time","Time", MessageBoxButton.OK); 
                }
            }
        }

        private void tbxinEndDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxinStartDate != null)
            {
                try
                {
                    IFormatProvider culture = new CultureInfo("fr-FR");
                    startDate = DateTime.Parse(tbxinStartDate.Text, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    endDate = DateTime.Parse(tbxinEndDate.Text, culture, System.Globalization.DateTimeStyles.AssumeLocal);

                    if (startDate > endDate)
                    {
                        MessageBox.Show("Start Date should be earlier than End Date", "Date", MessageBoxButton.OK);
                    }
                }
                catch (FormatException )
                {
                    MessageBox.Show("Please put in dd-MM-yyyy format", "Date format", MessageBoxButton.OK);
                }
            }
        }




    }
}
