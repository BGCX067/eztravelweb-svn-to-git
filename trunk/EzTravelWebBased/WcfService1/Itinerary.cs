using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Itinerary
    {
        private string deviceID;

        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        private int journeyID;

        public int JourneyID
        {
            get { return journeyID; }
            set { journeyID = value; }
        }

        private string place;

        public string Place
        {
            get { return place; }
            set { place = value; }
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
    }
}