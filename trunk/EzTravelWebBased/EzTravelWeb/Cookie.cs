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
using System.Windows.Browser;

namespace EzTravelWeb
{
    public class Cookie
    {
        public void SetCookie(string value)
        {
            DateTime expireDate = DateTime.Now + TimeSpan.FromDays(14);
            string newCookie = "showGuide =" + value + ";expires=" + expireDate.ToString("R"); 
            HtmlPage.Document.SetProperty("cookie", newCookie);
        }

        public string GetCookie(string key)
        { 
           string[] cookies = HtmlPage.Document.Cookies.Split(';'); 
            
           foreach (string cookie in cookies) 
           
           { 
               string[] keyValue = cookie.Split('='); 
               if (keyValue.Length == 2) 
               { if (keyValue[0].ToString() == key)        
                   return keyValue[1]; 
               } 
           } 
            return null;
        }
    }
}
