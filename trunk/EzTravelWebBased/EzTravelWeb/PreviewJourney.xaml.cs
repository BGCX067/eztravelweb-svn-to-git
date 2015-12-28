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
using EzTravelWeb.Views;
using Microsoft.Maps.MapControl;
using EzTravelWeb.Resources.Model;
using EzTravelWeb.GeocodeService;
using EzTravelWeb.localService;
using EzTravelWeb.RouteService;



namespace EzTravelWeb
{
    public partial class PreviewJourney : UserControl
    {
        #region Constants

        public static List<POI> poiJourney = new List<POI>();
        Location startLoc;
        Location desti;
        List<localService.PathLocation> pathList;
        List<localService.busStop> busStopList;
        Location previous = null;
        Location current = null;
        MapLayer pinLayer;
        MapLayer busLayer;
        MapLayer POILayer;
        PushpinModel ppM;
        MapPolyline routeLine;
        MapLayer myRouteLayer;
        Pushpin p;
        ToolTip t;
        public static string startLocation;
        public static string destination;
        int journeyID = 0;
        string deviceID="";
        List<Itinerary> iList = new List<Itinerary>();

        #endregion

        #region initialise Section
        public PreviewJourney()
        {
            InitializeComponent();
            bingMap.SetView(new Location(1.3667, 103.8), 12);
            phoneGuide.Visibility = Visibility.Collapsed;
            clearLayer();
            
            ppM = new PushpinModel();
            pathList = new List<localService.PathLocation>();
            myRouteLayer = new MapLayer();
            pinLayer = new MapLayer();
            busLayer = new MapLayer();
            POILayer = new MapLayer();
            p = new Pushpin();
            startLoc = new Location();
            desti = new Location();
            bingMap.Children.Add(busLayer);
            bingMap.Children.Add(PushpinModel.contentLayer);
            bingMap.Children.Add(pinLayer);
            bingMap.Children.Add(myRouteLayer);
            bingMap.Children.Add(POILayer);
            POILayer.Visibility = Visibility.Collapsed;
            WebService.Service1Client poiClient;
          

            if (poiJourney.Count!=0)
            {
                foreach (POI poi in poiJourney)
                {
                    switch (poi.Category)
                    {
                        case "Events":
                            poiClient=  new WebService.Service1Client();
                            poiClient.retrieveEventAsync(poi.Place);
                            poiClient.retrieveEventCompleted+=new EventHandler<WebService.retrieveEventCompletedEventArgs>(poiClient_retrieveEventCompleted);
                           
                            break;

                        case "Accommodation":
                            poiClient = new WebService.Service1Client();
                            poiClient.retrieveHotelAsync(poi.Place);
                            poiClient.retrieveHotelCompleted+=new EventHandler<WebService.retrieveHotelCompletedEventArgs>(poiClient_retrieveHotelCompleted);
                         
                            break;

                        case "Dining":
                            poiClient = new WebService.Service1Client();
                            poiClient.retrieveDiningAsync(poi.Place);
                            poiClient.retrieveDiningCompleted+=new EventHandler<WebService.retrieveDiningCompletedEventArgs>(poiClient_retrieveDiningCompleted);
                         
                            break;

                        case "POI":
                            poiClient = new WebService.Service1Client();
                            poiClient.retrievePOIAsync(poi.Place);
                            poiClient.retrievePOICompleted+=new EventHandler<WebService.retrievePOICompletedEventArgs>(poiClient_retrievePOICompleted);
                          
                            break;

                        case "Shopping":
                            poiClient = new WebService.Service1Client();
                            poiClient.retrieveShoppingAsync(poi.Place);
                            poiClient.retrieveShoppingCompleted+=new EventHandler<WebService.retrieveShoppingCompletedEventArgs>(poiClient_retrieveShoppingCompleted);
                          
                            break;
                    }

                }
            }
       
            if (startLocation != null && destination != null)
            {

                localService.Service1Client journeyClient = new localService.Service1Client();
                journeyClient.GetJourneyAsync(startLocation, destination);
                journeyClient.GetJourneyCompleted += new EventHandler<GetJourneyCompletedEventArgs>(journeyClient_GetJourneyCompleted);
            }

            lbxPreviewItinerary.ItemsSource = poiJourney;
                       

        }

        void setMapPath(int journeyID)
        {
            localService.Service1Client pathSvc = new localService.Service1Client();
            pathSvc.GetPathAsync(journeyID);
            pathSvc.GetPathCompleted += new EventHandler<GetPathCompletedEventArgs>(busService_GetPathCompleted);
        }

        #endregion

        #region wcf completion method
        void poiClient_retrieveEventCompleted(object sender, WebService.retrieveEventCompletedEventArgs e)
        {
           WebService.Event eventInfo = e.Result;
           foreach (POI px in poiJourney)
           {
               if (px.Place.Equals(eventInfo.EName))
               {
                  p = new Pushpin();
                 p.Content =processValue(px.Number);
               }
           }
            
                                      
            p.Location = new Location(Convert.ToDouble(eventInfo.ELat),Convert.ToDouble( eventInfo.ELong));
            t = new ToolTip();
            t.Content = eventInfo.EName + Environment.NewLine + eventInfo.EVenue; 
            ToolTipService.SetToolTip(p, t);
            bingMap.Children.Add(p);
            iList.Add(new Itinerary { Place = eventInfo.EName, Latitude = Convert.ToDouble(eventInfo.ELat), Longitude = Convert.ToDouble(eventInfo.ELong) });      
        }

        void poiClient_retrieveHotelCompleted(object sender, WebService.retrieveHotelCompletedEventArgs e)
        {
            WebService.Accommodation accInfo = e.Result;
            foreach (POI px in poiJourney)
            {
                if (px.Place.Equals(accInfo.H_Name))
                {
                    p = new Pushpin();
                    p.Content = processValue(px.Number);
                }
            }
                    
              p.Location = new Location(Convert.ToDouble(accInfo.H_Latitude), Convert.ToDouble(accInfo.H_Longitude));
            t = new ToolTip();
            t.Content = accInfo.H_Name + Environment.NewLine + accInfo.H_Address; 
            ToolTipService.SetToolTip(p, t);
            bingMap.Children.Add(p);
            iList.Add (new Itinerary{ Place=accInfo.H_Name , Latitude= Convert.ToDouble(accInfo.H_Latitude), Longitude= Convert.ToDouble(accInfo.H_Longitude) });
          
        }

        void poiClient_retrieveDiningCompleted(object sender, WebService.retrieveDiningCompletedEventArgs e)
        {
            WebService.Dining dineInfo = e.Result;
            foreach (POI px in poiJourney)
            {
                if (px.Place.Equals(dineInfo.DName))
                {
                    p = new Pushpin();
                    p.Content = processValue(px.Number);
                }
            }
                    
            p.Location = new Location(Convert.ToDouble(dineInfo.DLat), Convert.ToDouble(dineInfo.DLong));
            t = new ToolTip();
            t.Content = dineInfo.DName + Environment.NewLine + dineInfo.DVenue;
            ToolTipService.SetToolTip(p, t);
            bingMap.Children.Add(p);
            iList.Add(new Itinerary { Place = dineInfo.DName, Latitude = Convert.ToDouble(dineInfo.DLat), Longitude = Convert.ToDouble(dineInfo.DLong) });
        }

        void poiClient_retrievePOICompleted(object sender, WebService.retrievePOICompletedEventArgs e)
        {
            WebService.POI poiInfo = e.Result;
            foreach (POI px in poiJourney)
            {
                if (px.Place.Equals(poiInfo.Poi_Name))
                {
                    p = new Pushpin();
                    p.Content = processValue(px.Number);
                }
            }
                    
            p.Location = new Location(Convert.ToDouble(poiInfo.Poi_Latitude), Convert.ToDouble(poiInfo.Poi_Longitude));
            t = new ToolTip();
            t.Content = poiInfo.Poi_Name + Environment.NewLine + poiInfo.Poi_Address;
            ToolTipService.SetToolTip(p, t);
            bingMap.Children.Add(p);
            iList.Add(new Itinerary { Place = poiInfo.Poi_Name, Latitude = Convert.ToDouble(poiInfo.Poi_Latitude), Longitude = Convert.ToDouble(poiInfo.Poi_Longitude) });
        }

        void poiClient_retrieveShoppingCompleted(object sender, WebService.retrieveShoppingCompletedEventArgs e)
        {
            WebService.Shopping shopInfo = e.Result;
            foreach (POI px in poiJourney)
            {
                if (px.Place.Equals(shopInfo.SName))
                {
                    p = new Pushpin();
                    p.Content =processValue(px.Number);
                }
            }
                    
            p.Location = new Location(Convert.ToDouble(shopInfo.SLat), Convert.ToDouble(shopInfo.SLong));
            t = new ToolTip();
            t.Content = shopInfo.SName + Environment.NewLine + shopInfo.SVenue;
            ToolTipService.SetToolTip(p, t);
            bingMap.Children.Add(p);
            iList.Add(new Itinerary { Place = shopInfo.SName, Latitude = Convert.ToDouble(shopInfo.SLat), Longitude = Convert.ToDouble(shopInfo.SLong) });
        }
        

        internal GeocodeService.GeocodeResult[] geocodeResults;

        private void journeyClient_GetJourneyCompleted(object sender, GetJourneyCompletedEventArgs e)
        {
            journeyID = e.Result;
            setMapPath(journeyID);
        }

        void geoCodeService(string address)
        {
            GeocodeRequest geocodeRequest = new GeocodeRequest();
            geocodeRequest.Credentials = new Credentials();

            geocodeRequest.Credentials.ApplicationId = ((ApplicationIdCredentialsProvider)bingMap.CredentialsProvider).ApplicationId;

            geocodeRequest.Query = address;

            GeocodeService.ConfidenceFilter[] filters = new GeocodeService.ConfidenceFilter[1];
            filters[0] = new GeocodeService.ConfidenceFilter();
            filters[0].MinimumConfidence = GeocodeService.Confidence.High;

            GeocodeService.GeocodeOptions geocodeOptions = new GeocodeService.GeocodeOptions();
            geocodeOptions.Filters = filters;

            geocodeRequest.Options = geocodeOptions;

            GeocodeService.GeocodeServiceClient geocodeService = new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            geocodeService.GeocodeCompleted += new EventHandler<GeocodeCompletedEventArgs>(geocodeService_GeocodeCompleted);
            geocodeService.GeocodeAsync(geocodeRequest);
        }

        void geocodeService_GeocodeCompleted(object sender, GeocodeCompletedEventArgs e)
        {
            GeocodeResponse geocodeResponse = e.Result;
            Pushpin p = null;

            if (geocodeResponse.Results.Length > 0)
            {
                p = new Pushpin();
                p.Location = new Location(geocodeResponse.Results[0].Locations[0].Latitude, geocodeResponse.Results[0].Locations[0].Longitude);
                bingMap.Children.Add(p);
            }
        }

        private void Geocode(string strAddress, int wayPointIndex)
        {
            GeocodeService.GeocodeServiceClient geocodeService = new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            geocodeService.GeocodeCompleted += new EventHandler<GeocodeService.GeocodeCompletedEventArgs>(geoCodeService_GeocodeCompleted);

            GeocodeService.GeocodeRequest geocodeRequest = new GeocodeService.GeocodeRequest();
            geocodeRequest.Credentials = new Credentials();

            geocodeRequest.Credentials.ApplicationId = ((ApplicationIdCredentialsProvider)bingMap.CredentialsProvider).ApplicationId;
            geocodeRequest.Query = strAddress;

            geocodeService.GeocodeAsync(geocodeRequest, wayPointIndex);
        }

        private void geoCodeService_GeocodeCompleted(object sender, GeocodeService.GeocodeCompletedEventArgs e)
        {
            int waypointIndex = Convert.ToInt32(e.UserState);
            geocodeResults[waypointIndex] = e.Result.Results[0];

            bool doneGeocoding = true;

            foreach (GeocodeService.GeocodeResult gr in geocodeResults)
            {
                if (gr == null)
                {
                    doneGeocoding = false;
                }
            }

            if (doneGeocoding)
                CalculateRoute(geocodeResults);
            
        }

        private void CalculateRoute(GeocodeService.GeocodeResult[] results)
        {
            RouteService.RouteServiceClient routeService = new RouteService.RouteServiceClient("BasicHttpBinding_IRouteService");

            routeService.CalculateRouteCompleted += new EventHandler<RouteService.CalculateRouteCompletedEventArgs>(routeService_CalculateRouteCompleted);

            RouteService.RouteRequest routeRequest = new RouteService.RouteRequest();
            routeRequest.Credentials = new Credentials();
            routeRequest.Credentials.ApplicationId = ((ApplicationIdCredentialsProvider)bingMap.CredentialsProvider).ApplicationId;

            routeRequest.Options = new RouteService.RouteOptions();
            routeRequest.Options.RoutePathType = RouteService.RoutePathType.Points;

            routeRequest.Waypoints = new System.Collections.ObjectModel.ObservableCollection<RouteService.Waypoint>();

            foreach (GeocodeService.GeocodeResult result in results)
            {
                routeRequest.Waypoints.Add(GeoCodeResultToWaypoint(result));
            }
            routeService.CalculateRouteAsync(routeRequest);
        }

        private RouteService.Waypoint GeoCodeResultToWaypoint(GeocodeService.GeocodeResult result)
        {
            RouteService.Waypoint waypoint = new RouteService.Waypoint();
            waypoint.Description = result.DisplayName;
            waypoint.Location = new Location();
            waypoint.Location.Latitude = result.Locations[0].Latitude;
            waypoint.Location.Longitude = result.Locations[0].Longitude;
            return waypoint;
        }

        private void routeService_CalculateRouteCompleted(object sender, RouteService.CalculateRouteCompletedEventArgs e)
        {
            if ((e.Result.ResponseSummary.StatusCode == RouteService.ResponseStatusCode.Success) && (e.Result.Result.Legs.Count != 0))
            {
                System.Windows.Media.Color routeColor = Colors.Blue;
                SolidColorBrush routeBrush = new SolidColorBrush(routeColor);
                MapPolyline routeLine = new MapPolyline();
                routeLine.Locations = new LocationCollection();

                routeLine.Stroke = routeBrush;
                routeLine.Opacity = 0.65;
                routeLine.StrokeThickness = 5.0;

                foreach (Location p in e.Result.Result.RoutePath.Points)
                {
                    Location x = new Location();
                    x.Latitude = p.Latitude;
                    x.Longitude = p.Longitude;
                    routeLine.Locations.Add(x);

                }
                myRouteLayer.Children.Add(routeLine);
            }
        }

        private void busService_GetPathCompleted(object sender, localService.GetPathCompletedEventArgs e)
        {
            pathList = e.Result;

            startLoc.Latitude = pathList[0].StartLatitude;
            startLoc.Longitude = pathList[0].StartLongitude;
            desti.Latitude = pathList[pathList.Count - 1].EndLatitude;
            desti.Longitude = pathList[pathList.Count - 1].EndLongitude;

            for (int i = 0; i < pathList.Count; i++)
            {
                if (pathList[i].BusServiceNum != 0)
                {
                    localService.Service1Client transitPath = new localService.Service1Client();
                    transitPath.GetBusStopAsync(Convert.ToInt32(pathList[i].BusServiceNum), pathList[i].StartLatitude, pathList[i].StartLongitude, pathList[i].EndLatitude, pathList[i].EndLongitude);
                    transitPath.GetBusStopCompleted += new EventHandler<GetBusStopCompletedEventArgs>(transitPath_GetBusStopCompleted);

                    previous = new Location();
                    previous.Latitude = pathList[i].StartLatitude;
                    previous.Longitude = pathList[i].StartLongitude;

                    pinLayer.Children.Add(ppM.createPushPin(pathList[i].Mode, previous, pathList[i].Sequence, pathList[i].Location));

                    current = new Location();
                    current.Latitude = pathList[i].EndLatitude;
                    current.Longitude = pathList[i].EndLongitude;

                    pinLayer.Children.Add(ppM.createPushPin(pathList[i].Mode, current, pathList[i].Sequence, pathList[i].Location));
                }
                else
                {
                    if (pathList[i].Mode.Equals("Walk"))
                    {
                        routeLine = new MapPolyline();
                        System.Windows.Media.Color routeColor = Colors.Red;
                        routeLine.Locations = new LocationCollection();
                        SolidColorBrush routeBrush = new SolidColorBrush(routeColor);
                        routeLine.Stroke = routeBrush;
                        routeLine.Opacity = 0.65;
                        routeLine.StrokeThickness = 5.0;
                        myRouteLayer.Children.Add(routeLine);
                    }
                    else
                    {
                        routeLine = new MapPolyline();
                        System.Windows.Media.Color routeColor = Colors.Green;
                        routeLine.Locations = new LocationCollection();
                        SolidColorBrush routeBrush = new SolidColorBrush(routeColor);
                        routeLine.Stroke = routeBrush;
                        routeLine.Opacity = 0.65;
                        routeLine.StrokeThickness = 5.0;
                        myRouteLayer.Children.Add(routeLine);
                    }

                    previous = new Location();
                    previous.Latitude = pathList[i].StartLatitude;
                    previous.Longitude = pathList[i].StartLongitude;
                    routeLine.Locations.Add(previous);
                    pinLayer.Children.Add(ppM.createPushPin(pathList[i].Mode, previous, pathList[i].Sequence, pathList[i].Location));

                    current = new Location();
                    current.Latitude = pathList[i].EndLatitude;
                    current.Longitude = pathList[i].EndLongitude;
                    routeLine.Locations.Add(current);
                }
                if (i == pathList.Count - 1)
                {
                    LocationRect rect = new LocationRect(startLoc, desti);
                    bingMap.SetView(rect);

                }
            }

        }

        private void transitPath_GetBusStopCompleted(object sender, localService.GetBusStopCompletedEventArgs e)
        {
            busStopList = e.Result;

            geocodeResults = new GeocodeService.GeocodeResult[busStopList.Count];

            for (int i = 0; i < busStopList.Count; i++)
            {
                Geocode((busStopList[i].Latitude + "," + busStopList[i].Longitude).ToString(), i);
            }

        }

        #endregion

        

  

        public void clearLayer()
        {
            if (myRouteLayer != null)
            {
                myRouteLayer.Children.Clear();
                busLayer.Children.Clear();
                pinLayer.Children.Clear();
                PushpinModel.contentLayer.Children.Clear();
                bingMap.Children.Remove(PushpinModel.contentLayer);
                PushpinModel.contentLayer.Children.Clear();
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            Cookie c = new Cookie();
            string s = c.GetCookie("showGuide");
            if (s==null)
            {
                phoneGuide.Visibility = Visibility.Visible;
                bingMap.Opacity = 0.5;
            }
            else
            {
                
                SessionService.Service1Client sessionClient = new SessionService.Service1Client();
                sessionClient.getDeviceIDAsync();
                sessionClient.getDeviceIDCompleted += new EventHandler<SessionService.getDeviceIDCompletedEventArgs>(sessionClient_getDeviceIDCompleted);
                            
            }
        }

     
      
        void sessionClient_getDeviceIDCompleted(object sender, SessionService.getDeviceIDCompletedEventArgs e)
        {
            deviceID = e.Result;

            if (!deviceID.Equals(""))
            {
                localService.Service1Client getJID = new localService.Service1Client();
                getJID.GetJourneyAsync(startLocation, destination);
                getJID.GetJourneyCompleted += new EventHandler<GetJourneyCompletedEventArgs>(getJID_GetJourneyCompleted);
            }
            else
            {
                MessageBox.Show("Session could not be established. Ensure that the phone is connected to network and running application");
            }
        }

        void getJID_GetJourneyCompleted(object sender, GetJourneyCompletedEventArgs e)
        {
            int journeyID = e.Result;

            WebService.Service1Client checkItinerary = new WebService.Service1Client();
            checkItinerary.checkItineraryAsync(deviceID, journeyID);
            checkItinerary.checkItineraryCompleted+=new EventHandler<WebService.checkItineraryCompletedEventArgs>(checkItinerary_checkItineraryCompleted);
      
        }

        void checkItinerary_checkItineraryCompleted(object sender, WebService.checkItineraryCompletedEventArgs e)
        {
            bool res = e.Result;

            if (res == true)
            {
                MessageBoxResult result = MessageBox.Show("An existing journey with itinerary is present. Do you want to replace the current journey", "Existed Journey", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    WebService.Service1Client deleteItinerary = new WebService.Service1Client();
                    deleteItinerary.deleteItineraryAsync(deviceID, journeyID);
                    deleteItinerary.deleteItineraryCompleted+=new EventHandler<WebService.deleteItineraryCompletedEventArgs>(deleteItinerary_deleteItineraryCompleted);
                }
            }
            else
            {
                List<WebService.Itinerary> itList = new List<WebService.Itinerary>();
                WebService.Service1Client insertItinerary = new WebService.Service1Client();

                foreach (Itinerary i in iList)
                {
                    itList.Add(new WebService.Itinerary { DeviceID = deviceID, JourneyID = journeyID, Latitude = i.Latitude, Longitude = i.Longitude, Place = i.Place });

                }

                insertItinerary.insertItineraryAsync(itList);
                insertItinerary.insertItineraryCompleted += new EventHandler<WebService.insertItineraryCompletedEventArgs>(insertItinerary_insertItineraryCompleted);
            }

        }

        void deleteItinerary_deleteItineraryCompleted(object sender, WebService.deleteItineraryCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                List<WebService.Itinerary> itList = new List<WebService.Itinerary>();
                WebService.Service1Client insertItinerary = new WebService.Service1Client();

                foreach (Itinerary i in iList)
                {
                    itList.Add(new WebService.Itinerary { DeviceID = deviceID, JourneyID = journeyID, Latitude = i.Latitude, Longitude = i.Longitude, Place = i.Place });

                }

                insertItinerary.insertItineraryAsync(itList);
                insertItinerary.insertItineraryCompleted += new EventHandler<WebService.insertItineraryCompletedEventArgs>(insertItinerary_insertItineraryCompleted);
            }
        }

        void insertItinerary_insertItineraryCompleted(object sender, WebService.insertItineraryCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                SessionService.Service1Client sessionClient = new SessionService.Service1Client();
                sessionClient.deleteAsync();
                sessionClient.deleteCompleted+=new EventHandler<SessionService.deleteCompletedEventArgs>(sessionClient_deleteCompleted);
            }
        }

        void sessionClient_deleteCompleted(object sender, SessionService.deleteCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Journey saved to phone");
               
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage();
            clearLayer();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbxNoShow.IsChecked==true)
            {
                Cookie c = new Cookie();
                c.SetCookie("True");
            }
            phoneGuide.Visibility = Visibility.Collapsed;
            bingMap.Opacity = 1.0;

            SessionService.Service1Client sessionClient = new SessionService.Service1Client();
            sessionClient.getDeviceIDAsync();
            sessionClient.getDeviceIDCompleted += new EventHandler<SessionService.getDeviceIDCompletedEventArgs>(sessionClient_getDeviceIDCompleted);
        }

        string processValue(string s)
        {
            string res = "";
            foreach (char c in s)
            {
                if (char.IsDigit(c))
                {
                    res += c;
                }
            }
            return res;
        }

    
    }
}
