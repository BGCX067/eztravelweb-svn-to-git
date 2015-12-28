using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class PictureFile
    {
        private string pictureName;

        public string PictureName
        {
            get { return pictureName; }
            set { pictureName = value; }
        }

        private byte[] pictureStream;

        public byte[] PictureStream
        {
            get { return pictureStream; }
            set { pictureStream = value; }
        }
    }
}