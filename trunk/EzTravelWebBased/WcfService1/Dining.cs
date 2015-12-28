using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Dining
    {
        private int diningID;

        public int DiningID
        {
            get { return diningID; }
            set { diningID = value; }
        }

        private string dVenue;

        public string DVenue
        {
            get { return dVenue; }
            set { dVenue = value; }
        }
        private string dDesc;

        public string DDesc
        {
            get { return dDesc; }
            set { dDesc = value; }
        }
        private decimal dLong;

        public decimal DLong
        {
            get { return dLong; }
            set { dLong = value; }
        }
        private decimal dLat;

        public decimal DLat
        {
            get { return dLat; }
            set { dLat = value; }
        }
        private byte[] dImage;

        public byte[] DImage
        {
            get { return dImage; }
            set { dImage = value; }
        }

        private string dName;

        public string DName
        {
            get { return dName; }
            set { dName = value; }
        }



    }
}