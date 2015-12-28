using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Event
    {
        private int eID;

        public int EID
        {
            get { return eID; }
            set { eID = value; }
        }

        private string eVenue;

        public string EVenue
        {
            get { return eVenue; }
            set { eVenue = value; }
        }
        private string eDesc;

        public string EDesc
        {
            get { return eDesc; }
            set { eDesc = value; }
        }
        private decimal ePrice;

        public decimal EPrice
        {
            get { return ePrice; }
            set { ePrice = value; }
        }
        private decimal eLong;

        public decimal ELong
        {
            get { return eLong; }
            set { eLong = value; }
        }
        private decimal eLat;

        public decimal ELat
        {
            get { return eLat; }
            set { eLat = value; }
        }
        private byte[] eImage;

        public byte[] EImage
        {
            get { return eImage; }
            set { eImage = value; }
        }

        private string eName;

        public string EName
        {
            get { return eName; }
            set { eName = value; }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
      

        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

    
        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private decimal lowPrice;

        public decimal LowPrice
        {
            get { return lowPrice; }
            set { lowPrice = value; }
        }
        private decimal highPrice;

        public decimal HighPrice
        {
            get { return highPrice; }
            set { highPrice = value; }
        }


        



    }
}