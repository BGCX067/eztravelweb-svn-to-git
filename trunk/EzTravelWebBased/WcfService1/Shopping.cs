using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Shopping
    {
        private int sID;

        public int SID
        {
            get { return sID; }
            set { sID = value; }
        }


        private string sVenue;

        public string SVenue
        {
            get { return sVenue; }
            set { sVenue = value; }
        }
        private string sDesc;

        public string SDesc
        {
            get { return sDesc; }
            set { sDesc = value; }
        }
        private decimal sLong;

        public decimal SLong
        {
            get { return sLong; }
            set { sLong = value; }
        }
        private decimal sLat;

        public decimal SLat
        {
            get { return sLat; }
            set { sLat = value; }
        }
        private byte[] sImage;

        public byte[] SImage
        {
            get { return sImage; }
            set { sImage = value; }
        }

        private string sName;

        public string SName
        {
            get { return sName; }
            set { sName = value; }
        }



    }
}