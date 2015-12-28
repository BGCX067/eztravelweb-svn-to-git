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

namespace EzTravelWeb.Resources.Class
{
    public class pushpinLocation
    {
        private Location location;


        public Location Location
        {
            get { return location; }
            set { location = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int busNumber;

        public int BusNumber
        {
            get { return busNumber; }
            set { busNumber = value; }
        }
    }
}
