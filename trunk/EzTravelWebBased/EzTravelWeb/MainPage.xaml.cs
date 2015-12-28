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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Browser;
using EzTravelWeb.Views;
using System.IO;


namespace EzTravelWeb
{
    public partial class MainPage : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        private ObservableCollection<string> _images;
        private int _currentIndex = 0;
        bool sameDropSource = false;
        int count = 1;
        public List<POI> poiList = new List<POI>();
       WebService.Service1Client retrieveClient=null;
       DispatcherTimer startClickTimer;
       List<displayItem> itemDisplayList;
      public static string currentRetrieveCategory = "";

        public MainPage()
        {
           
            
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            InitializeImages();


            startClickTimer = new DispatcherTimer();
            startClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            startClickTimer.Tick += new EventHandler(startClickTimer_Tick);

            retrieveClient = new WebService.Service1Client();
            DetailedInfo.Visibility = Visibility.Collapsed;
            tbxImageGuide.Visibility = Visibility.Collapsed;
            
              
        }

        void startClickTimer_Tick(object sender, EventArgs e)
        {
            startClickTimer.Stop();

        }



        void retrieveClient_retrieveBriefInfoCompleted(object sender, WebService.retrieveBriefInfoCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                List<WebService.displayItem> itemsRetrieve = e.Result;

             itemDisplayList  = new List<displayItem>();

                foreach (WebService.displayItem item in itemsRetrieve)
                {
                    string desc = "";

                    for (int c = 0; c < item.Description.Length; c++)
                    {
                        if (c <=100)

                            desc += item.Description[c];
                    }
                    desc += "...";
                    byte[] imagebyte = item.Image;

                    BitmapImage bmpImage = new BitmapImage();

                    MemoryStream mystream = new MemoryStream(imagebyte);

                    bmpImage.SetSource(mystream);

                    itemDisplayList.Add(new displayItem { Image = bmpImage, Description = desc, Name = item.Name });



                }

                switch (currentRetrieveCategory)
                {
                    case "Dining":

                        lbxDining.ItemsSource = itemDisplayList;
                        break;
                     case "Shopping":
                        lbxShopping.ItemsSource = itemDisplayList;
                        break;
                       
                    case "Events":
                        lbxEvents.ItemsSource = itemDisplayList;
                        break;

                    case "Accommodation":
                        lbxRest.ItemsSource = itemDisplayList;
                        break;

                    case "POI":
                        lbxPOI.ItemsSource = itemDisplayList;
                        break;


                   
                }
             
            

            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            //e.Handled = true;
            //ChildWindow errorWin = new ErrorWindow(e.Uri);
            //errorWin.Show();
        }

        
        void timer_Tick(object sender, EventArgs e)
        {
            _currentIndex++;

            if (_currentIndex == _images.Count)
            {
                _currentIndex = 0;
            }

            FadeOutAnimation.Begin();
            FadeOutAnimation.Completed += new EventHandler(FadeOutAnimation_Completed);

        }

        void FadeOutAnimation_Completed(object sender, EventArgs e)
        {
            SetImageSource(_images[_currentIndex]);
            FadeInAnimation.Begin();
        }

        private void InitializeImages()
        {
            _images = new ObservableCollection<string>();
            for(int i = 0; i <= 3; i++)
            {
                _images.Add("IMG_"+i+".png");
            }

            SetImageSource(_images[_currentIndex]);
        }

        private void SetImageSource(string imageName)
        {
            string name = "Assets/slideshowimgs/" + imageName;
            BitmapImage bmi = new BitmapImage(new Uri(name, UriKind.Relative));
            ImgWhatNewImg.Source = bmi;
        }

        private void ImgWhatNewImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Uri url = null;
            switch (_currentIndex)
            {
                case 0:
                    url = new Uri("http://ez-travel.weebly.com", UriKind.Absolute);
                    HtmlPage.Window.Navigate(url, "_blank");
                    break;
                case 1:
                    url = new Uri("http://www.yoursingapore.com/content/traveller/en/browse/whats-on/festivals-and-events/grand-prix-season-singapore.html", UriKind.Absolute);
                    HtmlPage.Window.Navigate(url, "_blank");
                    break;
                case 2:
                    url = new Uri("http://www.yoursingapore.com/content/traveller/en/browse/apps/eventdetails.1081.html", UriKind.Absolute);
                    HtmlPage.Window.Navigate(url, "_blank");
                    break;
                case 3:
                    url = new Uri("http://www.yoursingapore.com/content/traveller/en/browse/whats-on/festivals-and-events/singapore-food-festival-2011.html", UriKind.Absolute);
                    HtmlPage.Window.Navigate(url, "_blank");
                    break;
            }
        }

        private void btnPlan_Click(object sender, RoutedEventArgs e)
        {
             if (tbxStart.Text.Equals("Enter your starting location") && tbxDestination.Text.Equals("Enter your Destination"))
            {
                MessageBox.Show("Please enter your starting location and destination");

            }
             else if (tbxStart.Text.Equals("Enter your starting location"))
            {
                MessageBox.Show("Please enter your starting location");
            }
            else if (tbxDestination.Text.Equals("Enter your Destination"))
            {
                MessageBox.Show("Please enter your destination");
            }
         
            else
            {
            PreviewJourney.poiJourney = poiList;
            PreviewJourney.startLocation = tbxStart.Text;
            PreviewJourney.destination = tbxDestination.Text;
            timer.Stop();
            this.Content = new PreviewJourney();
            currentRetrieveCategory = "";
            }
        }

        private void ListBoxDragDropTarget_ItemDroppedOnSource(object sender, Microsoft.Windows.DragEventArgs e)
        {
            sameDropSource = true;
        }

        private void ListBoxDragDropTarget_ItemDroppedOnTarget(object sender, ItemDragEventArgs e)
        {
            sameDropSource = false;
        }

        private void ListBoxDragDropTarget_ItemDragCompleted(object sender, ItemDragEventArgs e)
        {
            if (!sameDropSource)
            {
                POI place = (POI)lbxItinerary.SelectedItem;
                string s = place.Place;


                for (int i = 0; i < poiList.Count; i++)
                {
                    if (poiList[i].Place.Equals(s))
                    {
                        poiList.RemoveAt(i);
                        

                    }
                }

                    string number = "";



                    for (int i = 0; i < poiList.Count; i++)
                    {
                  
                    if (poiList.Count > 0)
                    {
                      
                            foreach (char c in poiList[i].Number)
                            {
                                if (char.IsDigit(c))
                                {
                                    number += c;
                                }
                            }

                            if (Convert.ToInt32(number) != i + 1)
                            {
                                poiList[i].Number = (i + 1).ToString();
                            }
                           
                    }
                }
                count = (lbxItinerary.Items.Count+1);
                lbxItinerary.ItemsSource = null;
                lbxItinerary.ItemsSource = poiList;
            }
        }

        private void lbxItinerary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxItinerary.SelectedItem != null)
            {
                POI place = (POI)lbxItinerary.SelectedItem;
                string s = place.Place;

                tbxDestination.Text = s;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image sendImage= sender as Image;
            string category = "";
            bool check = false;
            switch (tbControl.SelectedIndex)
            {
               

                case 1:
                    category = "Dining";
                    break;

                case 2:
                    category = "Shopping";
                    break;


                case 3:

                    category = "Events";
                    break;
               
                 case 4:

                    category = "POI";

                    break;


                 case 5:
                    category = "Accommodation";
                    break;


            }

            if (startClickTimer.IsEnabled)
            {
                if (poiList.Count > 0)
                {
                    for (int i = 0; i < poiList.Count; i++)
                    {
                        if (poiList[i].Place.Equals(sendImage.Tag.ToString()))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;
                        }
                    }
                    if (check == true)
                    {
                        poiList.Add(new POI { Number = count.ToString(), Place = sendImage.Tag.ToString(), Category = category });
                        lbxItinerary.ItemsSource = null;
                        lbxItinerary.ItemsSource = poiList;
                        count++;
                    }
                }
                else
                {
                    poiList.Add(new POI { Number = count.ToString(), Place = sendImage.Tag.ToString(), Category = category });
                    lbxItinerary.ItemsSource = null;
                    lbxItinerary.ItemsSource = poiList;
                    count++;
                }
            }
            else
            {
                startClickTimer.Start();
            }
           
        }

   
        private void tbxStart_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxStart.Text = "";
        }

        private void tbxDestination_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxDestination.Text="";
        }

        private void tbControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            retrieveClient = new WebService.Service1Client();
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            TabControl selectedMenu = (TabControl)sender;
            {

                switch (selectedMenu.SelectedIndex)
                {
                    case 0:

                        if (!currentRetrieveCategory.Equals(""))
                        {
                            tbxImageGuide.Visibility = Visibility.Collapsed;
                            timer.Start();
                        }
                    break;

                    case 1:
                        retrieveClient.retrieveBriefInfoAsync("Dining");
                        retrieveClient.retrieveBriefInfoCompleted += new EventHandler<WebService.retrieveBriefInfoCompletedEventArgs>(retrieveClient_retrieveBriefInfoCompleted);
                        currentRetrieveCategory = "Dining";
                        tbxImageGuide.Visibility = Visibility.Visible;
                        break;

                    case 2:
                        retrieveClient.retrieveBriefInfoAsync("Shopping");
                        retrieveClient.retrieveBriefInfoCompleted += new EventHandler<WebService.retrieveBriefInfoCompletedEventArgs>(retrieveClient_retrieveBriefInfoCompleted);
                        currentRetrieveCategory = "Shopping";
                        tbxImageGuide.Visibility = Visibility.Visible;
                        break;

                    case 3:
                        retrieveClient.retrieveBriefInfoAsync("Events");
                        retrieveClient.retrieveBriefInfoCompleted += new EventHandler<WebService.retrieveBriefInfoCompletedEventArgs>(retrieveClient_retrieveBriefInfoCompleted);
                        currentRetrieveCategory = "Events";
                        tbxImageGuide.Visibility = Visibility.Visible;
                        break;

                    case 4:
                        retrieveClient.retrieveBriefInfoAsync("POI");
                        retrieveClient.retrieveBriefInfoCompleted+=new EventHandler<WebService.retrieveBriefInfoCompletedEventArgs>(retrieveClient_retrieveBriefInfoCompleted);
                        currentRetrieveCategory = "POI";
                        tbxImageGuide.Visibility = Visibility.Visible;
                        break;

                    case 5:
                        retrieveClient.retrieveBriefInfoAsync("Accommodation");
                        retrieveClient.retrieveBriefInfoCompleted += new EventHandler<WebService.retrieveBriefInfoCompletedEventArgs>(retrieveClient_retrieveBriefInfoCompleted);
                        currentRetrieveCategory = "Accommodation";
                        tbxImageGuide.Visibility = Visibility.Visible;
                        break;

                 
                }
            }
            
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton navigate = sender as HyperlinkButton;
            WebService.Service1Client serviceClient = new WebService.Service1Client();
            string name = navigate.Tag.ToString();

            foreach (displayItem i in itemDisplayList)
            {
                if (name.Equals(i.Name))
                {

                    switch (currentRetrieveCategory)
                    {
                        case "Events":
                        serviceClient.retrieveEventAsync(name);
                        serviceClient.retrieveEventCompleted += new EventHandler<WebService.retrieveEventCompletedEventArgs>(serviceClient_retrieveEventCompleted);
                        break;

                        case "Dining":
                        serviceClient.retrieveDiningAsync(name);
                        serviceClient.retrieveDiningCompleted+=new EventHandler<WebService.retrieveDiningCompletedEventArgs>(serviceClient_retrieveDiningCompleted);
                        break;

                        case "Shopping":
                        serviceClient.retrieveShoppingAsync(name);
                        serviceClient.retrieveShoppingCompleted+=new EventHandler<WebService.retrieveShoppingCompletedEventArgs>(serviceClient_retrieveShoppingCompleted);
                        break;

                        case "POI":
                        serviceClient.retrievePOIAsync(name);
                        serviceClient.retrievePOICompleted+=new EventHandler<WebService.retrievePOICompletedEventArgs>(serviceClient_retrievePOICompleted);
                        break;

                        case "Accomodation":
                        serviceClient.retrieveHotelAsync(name);
                        serviceClient.retrieveHotelCompleted+=new EventHandler<WebService.retrieveHotelCompletedEventArgs>(serviceClient_retrieveHotelCompleted);
                        break;
                    }
                    break;
                }

            }
          
            DetailedInfo.Visibility = Visibility.Visible;
        }


        void serviceClient_retrieveEventCompleted(object sender, WebService.retrieveEventCompletedEventArgs e)
        {
            WebService.Event poi = e.Result;

            tbName.Text = poi.EName;
            tbxAddress.Text = poi.EVenue;
            tbDate.Text = poi.StartDate.ToString("dd-MM-yyyy") + " TO " + poi.EndDate.ToString("dd-MM-yyyy");

            if (poi.HighPrice != 0 && poi.LowPrice != 0)
                tbPrice.Text ="$"+ poi.LowPrice.ToString() + " TO $" + poi.HighPrice.ToString();
            else
            {
                tbPrice.Text = "Price is unavailable";
            }
            
            tbDescription.Text = poi.EDesc;
            tbTime.Text = poi.StartTime.ToShortTimeString() + " TO " + poi.EndTime.ToShortTimeString();

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(poi.EImage);
          
            bmpImage.SetSource(mystream);

            imgInfo.Source = bmpImage;
        }

        void serviceClient_retrieveDiningCompleted(object sender, WebService.retrieveDiningCompletedEventArgs e)
        {
            WebService.Dining dining = e.Result;

            tbName.Text = dining.DName;
            tbDate.Visibility = Visibility.Collapsed;
            tbdate.Visibility = Visibility.Collapsed;
            tbprice.Visibility=Visibility.Collapsed;
            tbPrice.Visibility = Visibility.Collapsed;
            tbxAddress.Text = dining.DVenue;
            tbDescription.Text = dining.DDesc;
            tbTime.Visibility = Visibility.Collapsed;
            tbtime.Visibility = Visibility.Collapsed;
          
            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(dining.DImage);

            bmpImage.SetSource(mystream);

            imgInfo.Source = bmpImage;
        }

        void serviceClient_retrieveShoppingCompleted(object sender, WebService.retrieveShoppingCompletedEventArgs e)
        {
            WebService.Shopping shop = e.Result;

            tbName.Text = shop.SName;
            tbDate.Visibility = Visibility.Collapsed;
            tbdate.Visibility = Visibility.Collapsed;
            tbprice.Visibility = Visibility.Collapsed;
            tbPrice.Visibility = Visibility.Collapsed;
            tbxAddress.Text = shop.SVenue;
            tbDescription.Text = shop.SDesc;
            tbTime.Visibility = Visibility.Collapsed;
            tbtime.Visibility = Visibility.Collapsed;

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(shop.SImage);

            bmpImage.SetSource(mystream);

            imgInfo.Source = bmpImage;
        }

        void serviceClient_retrievePOICompleted(object sender, WebService.retrievePOICompletedEventArgs e)
        {
            WebService.POI poi = e.Result;

            tbName.Text = poi.Poi_Name;
            tbDate.Visibility = Visibility.Collapsed;
            tbdate.Visibility = Visibility.Collapsed;
            tbprice.Visibility = Visibility.Visible;
            tbPrice.Visibility = Visibility.Visible;
            tbPrice.Text = "(Child) $" + poi.Poi_Child_Price + " (Adult) $" + poi.Poi_Adult_Price + " (Senior Citizen) $" + poi.Poi_Elderly_Price + " (Student) $" + poi.Poi_Student_Price;
            tbxAddress.Text = poi.Poi_Address;
            tbDescription.Text = poi.Poi_Descript;
            tbTime.Visibility = Visibility.Collapsed;
            tbtime.Visibility = Visibility.Collapsed;

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(poi.Poi_Img);

            bmpImage.SetSource(mystream);

            imgInfo.Source = bmpImage;
        }

        void serviceClient_retrieveHotelCompleted(object sender, WebService.retrieveHotelCompletedEventArgs e)
        {
            WebService.Accommodation acc = e.Result;

            tbName.Text = acc.H_Name;
            tbDate.Visibility = Visibility.Collapsed;
            tbdate.Visibility = Visibility.Collapsed;
            tbprice.Visibility = Visibility.Visible;
            tbPrice.Visibility = Visibility.Visible;
            tbPrice.Text = "From $" + acc.Low_price + " to $" + acc.High_price;
            tbxAddress.Text = acc.H_Address;
            tbDescription.Text = acc.H_descript;
            tbTime.Visibility = Visibility.Collapsed;
            tbtime.Visibility = Visibility.Collapsed;

            BitmapImage bmpImage = new BitmapImage();

            MemoryStream mystream = new MemoryStream(acc.H_Img);

            bmpImage.SetSource(mystream);

            imgInfo.Source = bmpImage;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DetailedInfo.Visibility = Visibility.Collapsed;
            ContentRoot.Opacity = 1.0;
        }

        private void tbxStart_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxStart.Text.Equals(""))
            {
                tbxStart.Text="Enter your starting location";
            }
        }

        private void tbxDestination_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxDestination.Text.Equals(""))
            {
                tbxDestination.Text="Enter your Destination";
            }

        }
       
    }
}