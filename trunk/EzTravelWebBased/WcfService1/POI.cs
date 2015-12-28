using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class POI
    {
        private int poi_ID;

        public int Poi_ID
        {
            get { return poi_ID; }
            set { poi_ID = value; }
        }

        private string poi_Name;

        public string Poi_Name
        {
            get { return poi_Name; }
            set { poi_Name = value; }
        }

        private string poi_Address;

        public string Poi_Address
        {
            get { return poi_Address; }
            set { poi_Address = value; }
        }

        private double poi_Longitude;

        public double Poi_Longitude
        {
            get { return poi_Longitude; }
            set { poi_Longitude = value; }
        }

        private double poi_Latitude;

        public double Poi_Latitude
        {
            get { return poi_Latitude; }
            set { poi_Latitude = value; }
        }

        private string poi_Descript;

        public string Poi_Descript
        {
            get { return poi_Descript; }
            set { poi_Descript = value; }
        }

        private double poi_Price;

        public double Poi_Price
        {
            get { return poi_Price; }
            set { poi_Price = value; }
        }

        private double poi_Child_Price;

        public double Poi_Child_Price
        {
            get { return poi_Child_Price; }
            set { poi_Child_Price = value; }
        }
        
        private double poi_Student_Price;

        public double Poi_Student_Price
        {
            get { return poi_Student_Price; }
            set { poi_Student_Price = value; }
        }
        
        private double poi_Adult_Price;

        public double Poi_Adult_Price
        {
            get { return poi_Adult_Price; }
            set { poi_Adult_Price = value; }
        }
        
        private double poi_Elderly_Price;

        public double Poi_Elderly_Price
        {
            get { return poi_Elderly_Price; }
            set { poi_Elderly_Price = value; }
        }
        
        private byte[] poi_Img;

        public byte[] Poi_Img
        {
            get { return poi_Img; }
            set { poi_Img = value; }
        }

        private string price_Cat;

        public string Price_Cat
        {
            get { return price_Cat; }
            set { price_Cat = value; }
        }
    }
}