using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class displayItem
    {
        private byte[] image;

        public byte[] Image
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