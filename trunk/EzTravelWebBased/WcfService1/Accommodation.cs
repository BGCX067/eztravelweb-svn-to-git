using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Accommodation
    {
        private int hID;

        public int HID
        {
            get { return hID; }
            set { hID = value; }
        }

        private string h_Name;

        public string H_Name
        {
            get { return h_Name; }
            set { h_Name = value; }
        }
        private string h_Address;

        public string H_Address
        {
            get { return h_Address; }
            set { h_Address = value; }
        }

        private double low_price;

        public double Low_price
        {
            get { return low_price; }
            set { low_price = value; }
        }
        private double high_price;

        public double High_price
        {
            get { return high_price; }
            set { high_price = value; }
        }
        private double h_Latitude;

        public double H_Latitude
        {
            get { return h_Latitude; }
            set { h_Latitude = value; }
        }
        private double h_Longitude;

        public double H_Longitude
        {
            get { return h_Longitude; }
            set { h_Longitude = value; }
        }
        private string h_descript;

        public string H_descript
        {
            get { return h_descript; }
            set { h_descript = value; }
        }
        private string h_Facilities;

        public string H_Facilities
        {
            get { return h_Facilities; }
            set { h_Facilities = value; }
        }
        private byte[] h_Img;

        public byte[] H_Img
        {
            get { return h_Img; }
            set { h_Img = value; }
        }

    
    }
}