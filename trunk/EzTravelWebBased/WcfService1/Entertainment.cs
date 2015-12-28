using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Entertainment
    {

        private string enVenue;

        public string EnVenue
        {
            get { return enVenue; }
            set { enVenue = value; }
        }
        private string enDesc;

        public string EnDesc
        {
            get { return enDesc; }
            set { enDesc = value; }
        }
        private decimal enLong;

        public decimal EnLong
        {
            get { return enLong; }
            set { enLong = value; }
        }
        private decimal enLat;

        public decimal EnLat
        {
            get { return enLat; }
            set { enLat = value; }
        }
        private byte enImage;

        public byte EnImage
        {
            get { return enImage; }
            set { enImage = value; }
        }

        private string enName;

        public string EnName
        {
            get { return enName; }
            set { enName = value; }
        }

    }
}
