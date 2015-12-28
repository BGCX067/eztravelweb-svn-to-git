using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl;

namespace EzTravelWeb.Views
{
    public class POI
    {
        private string number;

        public string Number
        {
            get { return number + ". "; }
            set { number = value; }
        }

        private string place;

        public string Place
        {
            get { return  place; }
            set { place = value; }
        }

        private Location poiLocation;

        public Location PoiLocation
        {
            get { return poiLocation; }
            set { poiLocation = value; }
        }

        private string category;

        public string Category
        {
            get { return category; }
            set { category = value; }
        }
    }
}
