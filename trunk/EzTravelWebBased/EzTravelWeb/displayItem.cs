using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace EzTravelWeb
{
    public class displayItem
    {

        private BitmapImage image;

        public BitmapImage Image
        {
            get { return image; }
            set { image = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;


        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}