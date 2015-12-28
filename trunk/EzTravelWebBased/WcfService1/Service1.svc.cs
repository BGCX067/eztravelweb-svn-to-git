using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Web;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {
        SqlConnection conn = new SqlConnection();

        private string conString = "Server=46.4.195.237; Initial Catalog=EZTravel; User Id=sa; Password=u1!sa;";
        // private string conString = "Server=localhost; Initial Catalog=EZTravel; User Id=sa; Password=imsa;";

        #region Check

        public bool checkPOI(string poiName)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT * FROM POI WHERE poi_Name = @name";
                comm.Parameters.AddWithValue("@name", poiName);

                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();

            return res;
        }

        public bool checkShop(string shopName)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();
            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT * FROM Shopping WHERE Shop_Name = @shopName";
                comm.Parameters.AddWithValue("@shopName", shopName);

                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        public bool checkDining(string dineName)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();
            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT * FROM Dining WHERE Dining_Name = @dineName";
                comm.Parameters.AddWithValue("@dineName", dineName);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        public bool checkEvent(string Name)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();
            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT * FROM Events WHERE Event_Name = @Name";
                comm.Parameters.AddWithValue("@Name", Name);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        public bool checkAcc(string accName)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();
            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT * FROM Accommodation WHERE Name = @accName";
                comm.Parameters.AddWithValue("@accName", accName);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }


        public bool checkLogin(string userName, string password)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT * FROM AdminLogin WHERE user_id = @userName AND password = @password";
                comm.Parameters.AddWithValue("@userName", userName);
                comm.Parameters.AddWithValue("@password", password);

                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();

            return res;
        }

        #endregion


        #region Retrieve

        public List<MarkerDB> retrieveMarker(int JourneyID)
        {
            SqlCommand comm = new SqlCommand();
            List<MarkerDB> marker = new List<MarkerDB>();
            MarkerDB m = null;
            try
            {
                conn.ConnectionString = conString;
                conn.Open();
                comm.Connection = conn;
                comm.CommandText = "SELECT JID, MarkerID, Direction From Marker Where JID =@JID";
                comm.Parameters.AddWithValue("@JID", JourneyID);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    m = new MarkerDB();
                    m.JID = Convert.ToInt32(dr["JID"].ToString());
                    m.MarkerID = dr["MarkerID"].ToString();
                    m.Direction = dr["Direction"].ToString();
                    marker.Add(m);
                }
                dr.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);

            }
            conn.Close();
            return marker;
        }

        public Event retrieveEvent(string EventName)
        {

            SqlCommand comm = new SqlCommand();
            Event e = new Event();
            List<Event> eList = new List<Event>();
            try
            {
                conn.ConnectionString = conString;
                conn.Open();
                comm.Connection = conn;

                comm.CommandText = "SELECT Event_ID, Event_Name, Event_Venue, Event_Standard_Price, Event_Desc, Event_Longtitude, Event_Latitude, Event_Image, Event_Date_Start, Event_Date_End, Event_Start_Time, Event_End_Time, Event_Price_Low, Event_Price_High From Events Where Event_Name =@EventName";
                comm.Parameters.AddWithValue("@EventName", EventName);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    e.EID = Convert.ToInt32(dr["Event_ID"].ToString());
                    e.EName = dr["Event_Name"].ToString();
                    e.EVenue = dr["Event_Venue"].ToString();
                    e.EPrice = Convert.ToDecimal(dr["Event_Standard_Price"].ToString());
                    e.EDesc = dr["Event_Desc"].ToString();
                    e.ELong = Convert.ToDecimal(dr["Event_Longtitude"].ToString());
                    e.ELat = Convert.ToDecimal(dr["Event_Latitude"].ToString());
                    e.EImage = (byte[])dr["Event_Image"];
                    e.StartDate = Convert.ToDateTime(dr["Event_Date_Start"].ToString());
                    e.EndDate = Convert.ToDateTime(dr["Event_Date_End"].ToString());
                    e.StartTime = Convert.ToDateTime(dr["Event_Start_Time"].ToString());
                    e.EndTime = Convert.ToDateTime(dr["Event_End_Time"].ToString());
                    e.LowPrice = Convert.ToDecimal(dr["Event_Price_Low"].ToString());
                    e.HighPrice = Convert.ToDecimal(dr["Event_Price_High"].ToString());


                }
                dr.Close();
            }
            catch (SqlException f)
            {
                Console.WriteLine(f.Message);

            }
            conn.Close();
            return e;
        }

        public Dining retrieveDining(string DiningName)
        {

            SqlCommand comm = new SqlCommand();
            Dining d = new Dining();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT * From Dining Where Dining_Name =@DiningName";
                comm.Parameters.AddWithValue("@DiningName", DiningName);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    d.DiningID = Convert.ToInt32(dr["Dining_ID"].ToString());
                    d.DName = dr["Dining_Name"].ToString();
                    d.DVenue = dr["Dining_Venue"].ToString();
                    d.DDesc = dr["Dining_Desc"].ToString();
                    d.DLat = Convert.ToDecimal(dr["Dining_Latitude"].ToString());
                    d.DLong = Convert.ToDecimal(dr["Dining_Longtitude"].ToString());
                    d.DImage = (byte[])dr["Dining_Image"];

                }
                dr.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);

            }
            conn.Close();
            return d;
        }

        public Shopping retrieveShopping(string ShoppingName)
        {
            SqlCommand comm = new SqlCommand();
            Shopping s = new Shopping();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT Shop_ID, Shop_Name, Shop_Venue, Shop_Desc, Shop_Longtitude, Shop_Latitude, Shop_Image From Shopping Where Shop_Name =@ShoppingName";
                comm.Parameters.AddWithValue("@ShoppingName", ShoppingName);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    s.SID = Convert.ToInt32(dr["Shop_ID"].ToString());
                    s.SName = dr["Shop_Name"].ToString();
                    s.SVenue = dr["Shop_Venue"].ToString();
                    s.SDesc = dr["Shop_Desc"].ToString();
                    s.SLong = Convert.ToDecimal(dr["Shop_Longtitude"].ToString());
                    s.SLat = Convert.ToDecimal(dr["Shop_Latitude"].ToString());
                    s.SImage = (byte[])dr["Shop_Image"];

                }
                dr.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);

            }
            conn.Close();
            return s;
        }

        public List<string> retrieveDictionary()
        {
            List<string> dictionary = new List<string>();
            SqlCommand comm = new SqlCommand();
            string name = "";

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT Name FROM Accommodation";
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    name = dr["Name"].ToString();
                    dictionary.Add(name);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return dictionary;
        }

        public Accommodation retrieveHotel(string h_Name)
        {

            Accommodation acc = new Accommodation();
            SqlCommand comm = new SqlCommand();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT hotel_ID, Address, Description, Price_High, Facilities, Hotel_Longtitude, Hotel_Latitude, Hotel_Img, Price_Low FROM Accommodation WHERE Name = @name";
                comm.Parameters.AddWithValue("@name", h_Name);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    acc.HID = Convert.ToInt32(dr["hotel_ID"].ToString());
                    acc.H_Address = dr["Address"].ToString();
                    acc.H_descript = dr["Description"].ToString();
                    acc.High_price = Convert.ToDouble(dr["Price_High"].ToString());
                    acc.H_Facilities = dr["Facilities"].ToString();
                    acc.H_Longitude = Convert.ToDouble(dr["Hotel_Longtitude"].ToString());
                    acc.H_Latitude = Convert.ToDouble(dr["Hotel_Latitude"].ToString());
                    acc.H_Img = (Byte[])dr["Hotel_Img"];
                    acc.Low_price = Convert.ToDouble(dr["Price_Low"].ToString());
                    acc.H_Name = h_Name;

                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return acc;
        }

        public List<string> retrieveShopName()
        {
            List<string> shopName = new List<string>();
            SqlCommand comm = new SqlCommand();
            string sName = "";

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT Shop_Name FROM Shopping";
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    sName = dr["Shop_Name"].ToString();
                    shopName.Add(sName);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return shopName;
        }

        public List<string> retrieveDiningName()
        {
            List<string> diningName = new List<string>();
            SqlCommand comm = new SqlCommand();
            string dName = "";

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT Dining_Name FROM Dining";
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    dName = dr["Dining_Name"].ToString();
                    diningName.Add(dName);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return diningName;
        }

        public List<string> retrieveEventName()
        {
            List<string> eventName = new List<string>();
            SqlCommand comm = new SqlCommand();
            string eName = "";

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT Event_Name FROM Events";
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    eName = dr["Event_Name"].ToString();
                    eventName.Add(eName);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return eventName;
        }

        public List<string> retrievePOIName()
        {
            List<string> poiName = new List<string>();
            SqlCommand comm = new SqlCommand();
            string pName = "";

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT poi_Name FROM POI";
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    pName = dr["poi_Name"].ToString();
                    poiName.Add(pName);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return poiName;
        }

        public POI retrievePOI(string poiName)
        {

            POI places = new POI();
            SqlCommand comm = new SqlCommand();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "SELECT poi_ID, poi_Address, latitude, longitude, description, Standard_Price, poi_Img, Child_Price, Adult_Price, Student_Price, Elderly_Price, price_Category FROM POI WHERE poi_Name = @name";
                comm.Parameters.AddWithValue("@name", poiName);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    places.Poi_ID = Convert.ToInt32(dr["poi_ID"].ToString());
                    places.Poi_Address = dr["poi_Address"].ToString();
                    places.Poi_Latitude = Convert.ToDouble(dr["latitude"].ToString());
                    places.Poi_Longitude = Convert.ToDouble(dr["longitude"].ToString());
                    places.Poi_Descript = dr["description"].ToString();
                    places.Poi_Price = Convert.ToDouble(dr["Standard_Price"].ToString());
                    places.Poi_Img = (byte[])dr["poi_Img"];
                    places.Poi_Child_Price = Convert.ToDouble(dr["Child_Price"].ToString());
                    places.Poi_Adult_Price = Convert.ToDouble(dr["Adult_Price"].ToString());
                    places.Poi_Student_Price = Convert.ToDouble(dr["Student_Price"].ToString());
                    places.Poi_Elderly_Price = Convert.ToDouble(dr["Elderly_Price"].ToString());
                    places.Price_Cat = dr["price_Category"].ToString();
                    places.Poi_Name = poiName;

                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return places;
        }

        #endregion

        #region Insert

        public bool InsertMarker(MarkerDB marker)
        {

            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "INSERT INTO Marker values (@jid, @markerid, @direction)";
                comm.Parameters.AddWithValue("@jid", marker.JID);
                comm.Parameters.AddWithValue("@markerid", marker.MarkerID);
                comm.Parameters.AddWithValue("@direction", marker.Direction);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool InsertEvent(Event events)
        {
            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                conn.Open();
                comm.Connection = conn;

                comm.CommandText = "INSERT INTO Events values (@venue,@desc,@price,@long,@lat,@image,@name,@startdate,@endDate,@starttime,@endtime,@lowprice,@highPrice)";
                comm.Parameters.AddWithValue("@venue", events.EVenue);
                comm.Parameters.AddWithValue("@desc", events.EDesc);
                comm.Parameters.AddWithValue("@price", events.EPrice);
                comm.Parameters.AddWithValue("@long", events.ELong);
                comm.Parameters.AddWithValue("@lat", events.ELat);
                comm.Parameters.AddWithValue("@image", events.EImage);
                comm.Parameters.AddWithValue("@name", events.EName);
                comm.Parameters.AddWithValue("@startdate", events.StartDate);
                comm.Parameters.AddWithValue("@endDate", events.EndDate);
                comm.Parameters.AddWithValue("@starttime", events.StartTime);
                comm.Parameters.AddWithValue("@endtime", events.EndTime);
                comm.Parameters.AddWithValue("@lowprice", events.LowPrice);
                comm.Parameters.AddWithValue("@highPrice", events.HighPrice);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool InsertDining(Dining dining)
        {

            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "INSERT INTO Dining values (@venue,@desc,@long,@lat,@image,@name)";
                comm.Parameters.AddWithValue("@venue", dining.DVenue);
                comm.Parameters.AddWithValue("@desc", dining.DDesc);
                comm.Parameters.AddWithValue("@long", dining.DLong);
                comm.Parameters.AddWithValue("@lat", dining.DLat);
                comm.Parameters.AddWithValue("@image", dining.DImage);
                comm.Parameters.AddWithValue("@name", dining.DName);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool InsertShopping(Shopping shopping)
        {

            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "INSERT INTO Shopping values (@venue,@desc,@long,@lat,@image,@name)";
                comm.Parameters.AddWithValue("@venue", shopping.SVenue);
                comm.Parameters.AddWithValue("@desc", shopping.SDesc);
                comm.Parameters.AddWithValue("@long", shopping.SLong);
                comm.Parameters.AddWithValue("@lat", shopping.SLat);
                comm.Parameters.AddWithValue("@image", shopping.SImage);
                comm.Parameters.AddWithValue("@name", shopping.SName);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool InsertAccommodation(Accommodation acc)
        {
            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "INSERT INTO Accommodation VALUES (@name,@address,@description,@priceRate,@facilities,@img,@longitude,@latitude,@lowprice)";
                comm.Parameters.AddWithValue("@name", acc.H_Name);
                comm.Parameters.AddWithValue("@address", acc.H_Address);
                comm.Parameters.AddWithValue("@description", acc.H_descript);
                comm.Parameters.AddWithValue("@priceRate", acc.High_price);
                comm.Parameters.AddWithValue("@facilities", acc.H_Facilities);
                comm.Parameters.AddWithValue("@img", acc.H_Img);
                comm.Parameters.AddWithValue("@longitude", acc.H_Longitude);
                comm.Parameters.AddWithValue("@latitude", acc.H_Latitude);
                comm.Parameters.AddWithValue("@lowprice", acc.Low_price);

                int results = comm.ExecuteNonQuery();

                if (results == 1)
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();

            return res;
        }

        public bool InsertPOI(POI poi)
        {
            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "INSERT INTO POI VALUES(@poiName, @poiAddress, @poiLat, @poiLong, @poiDescript, @poiPrice, @poiImg, @childprice, @adultprice, @studentprice, @elderlyprice, @priceCat)";
                comm.Parameters.AddWithValue("@poiName", poi.Poi_Name);
                comm.Parameters.AddWithValue("@poiAddress", poi.Poi_Address);
                comm.Parameters.AddWithValue("@poiLat", poi.Poi_Latitude);
                comm.Parameters.AddWithValue("@poiLong", poi.Poi_Longitude);
                comm.Parameters.AddWithValue("@poiDescript", poi.Poi_Descript);
                comm.Parameters.AddWithValue("@poiPrice", poi.Poi_Price);
                comm.Parameters.AddWithValue("@poiImg", poi.Poi_Img);
                comm.Parameters.AddWithValue("@childprice", poi.Poi_Child_Price);
                comm.Parameters.AddWithValue("@adultprice", poi.Poi_Adult_Price);
                comm.Parameters.AddWithValue("@studentprice", poi.Poi_Student_Price);
                comm.Parameters.AddWithValue("@elderlyprice", poi.Poi_Elderly_Price);
                comm.Parameters.AddWithValue("@priceCat", poi.Price_Cat);

                int results = comm.ExecuteNonQuery();

                if (results == 1)
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        #endregion

        #region Update

        public bool UpdateMarker(MarkerDB marker)
        {

            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "UPDATE Marker set MarkerID = @markerID, Direction = @direction WHERE JID = @JID";
                comm.Parameters.AddWithValue("@JID", marker.JID);
                comm.Parameters.AddWithValue("@markerID", marker.MarkerID);
                comm.Parameters.AddWithValue("@direction", marker.Direction);
                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool UpdateEvent(Event events)
        {
            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "UPDATE Events set Event_Venue = @EVenue, Event_Desc = @EDesc, Event_Standard_Price = @EPrice, Event_Longtitude = @ELong, Event_Latitude = @ELat, Event_Image = @EImage, Event_Date_Start = @EDS, Event_Date_End = @EDE, Event_Start_Time = @EST, Event_End_Time = @EET, Event_Price_Low = @EPL, Event_Price_High = @EPH  WHERE Event_Name = @EventName";
                comm.Parameters.AddWithValue("@EVenue", events.EVenue);
                comm.Parameters.AddWithValue("@EDesc", events.EDesc);
                comm.Parameters.AddWithValue("@EPrice", events.EPrice);
                comm.Parameters.AddWithValue("@ELong", events.ELong);
                comm.Parameters.AddWithValue("@ELat", events.ELat);
                comm.Parameters.AddWithValue("@EImage", events.EImage);
                comm.Parameters.AddWithValue("@EventName", events.EName);
                comm.Parameters.AddWithValue("@EDS", events.StartDate);
                comm.Parameters.AddWithValue("@EDE", events.EndTime);
                comm.Parameters.AddWithValue("@EST", events.StartTime);
                comm.Parameters.AddWithValue("@EET", events.EndTime);
                comm.Parameters.AddWithValue("@EPL", events.LowPrice);
                comm.Parameters.AddWithValue("@EPH", events.HighPrice);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool UpdateDining(Dining dining)
        {
            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "UPDATE Dining set Dining_Venue = @DVenue, Dining_Desc = @DDesc, Dining_Longtitude = @DLong, Dining_Latitude = @DLat, Dining_Image = @DImage WHERE Dining_Name = @DName";
                comm.Parameters.AddWithValue("@DVenue", dining.DVenue);
                comm.Parameters.AddWithValue("@DDesc", dining.DDesc);
                comm.Parameters.AddWithValue("@DLong", dining.DLong);
                comm.Parameters.AddWithValue("@DLat", dining.DLat);
                comm.Parameters.AddWithValue("@DImage", dining.DImage);
                comm.Parameters.AddWithValue("@DName", dining.DName);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool UpdateShopping(Shopping shopping)
        {

            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "UPDATE Shopping set Shop_Venue = @SVenue, Shop_Desc = @SDesc, Shop_Longtitude = @SLong, Shop_Latitude = @SLat, Shop_Image = @SImage WHERE Shop_Name = @SName ";
                comm.Parameters.AddWithValue("@SVenue", shopping.SVenue);
                comm.Parameters.AddWithValue("@SDesc", shopping.SDesc);
                comm.Parameters.AddWithValue("@SLong", shopping.SLong);
                comm.Parameters.AddWithValue("@SLat", shopping.SLat);
                comm.Parameters.AddWithValue("@SImage", shopping.SImage);
                comm.Parameters.AddWithValue("@SName", shopping.SName);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool UpdateAccommodation(Accommodation acc)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "UPDATE Accommodation set Name = @name, Address = @address, Description = @description, Price_High = @price, Facilities = @facilities, Hotel_Img = @img, Hotel_Longtitude = @long, Hotel_Latitude = @lat, Price_Low = @pricelow WHERE hotel_ID = @id";
                comm.Parameters.AddWithValue("@name", acc.H_Name);
                comm.Parameters.AddWithValue("@address", acc.H_Address);
                comm.Parameters.AddWithValue("@description", acc.H_descript);
                comm.Parameters.AddWithValue("@price", acc.High_price);
                comm.Parameters.AddWithValue("@facilities", acc.H_Facilities);
                comm.Parameters.AddWithValue("@img", acc.H_Img);
                comm.Parameters.AddWithValue("@long", acc.H_Longitude);
                comm.Parameters.AddWithValue("@lat", acc.H_Latitude);
                comm.Parameters.AddWithValue("@id", acc.HID);
                comm.Parameters.AddWithValue("@pricelow", acc.Low_price);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        public bool UpdatePOI(POI poi)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "UPDATE POI set poi_Name = @poiName, poi_Address = @poiAdd, latitude = @poiLat, longitude = @poiLong, description = @poiDes, Standard_Price = @poiPrice, poi_Img = @pImg, Child_Price = @childprice, Adult_Price =@adultprice, Student_Price = @studentprice, Elderly_Price = @elderlyprice, price_Category = @priceCat WHERE poi_ID = @id";
                comm.Parameters.AddWithValue("@id", poi.Poi_ID);
                comm.Parameters.AddWithValue("@poiName", poi.Poi_Name);
                comm.Parameters.AddWithValue("@poiAdd", poi.Poi_Address);
                comm.Parameters.AddWithValue("@poiLat", poi.Poi_Latitude);
                comm.Parameters.AddWithValue("@poiLong", poi.Poi_Longitude);
                comm.Parameters.AddWithValue("@poiDes", poi.Poi_Descript);
                comm.Parameters.AddWithValue("@poiPrice", poi.Poi_Price);
                comm.Parameters.AddWithValue("@pImg", poi.Poi_Img);
                comm.Parameters.AddWithValue("@childprice", poi.Poi_Child_Price);
                comm.Parameters.AddWithValue("@adultprice", poi.Poi_Adult_Price);
                comm.Parameters.AddWithValue("@studentprice", poi.Poi_Student_Price);
                comm.Parameters.AddWithValue("@elderlyprice", poi.Poi_Elderly_Price);
                comm.Parameters.AddWithValue("@priceCat", poi.Price_Cat);

                int results = comm.ExecuteNonQuery();

                if (results == 1)
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        #endregion

        #region Delete

        public bool DeleteMarker(int JID, string MarkerID)
        {

            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "DELETE FROM Marker where JID = @JID AND MarkerID = @MarkerID";
                comm.Parameters.AddWithValue("@JID", JID);
                comm.Parameters.AddWithValue("@MarkerID", MarkerID);
                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool DeleteEvent(int eventID)
        {
            SqlConnection connection = conn;
            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "DELETE FROM Events where Event_ID = @eventID";
                comm.Parameters.AddWithValue("@eventID", eventID);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool DeleteDining(int Dining)
        {

            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "DELETE FROM Dining where Dining_ID = @diningID";
                comm.Parameters.AddWithValue("@diningID", Dining);
                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool DeleteShopping(int ShopID)
        {
            SqlCommand comm = new SqlCommand();
            bool res = false;

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "DELETE FROM Shopping where Shop_ID = @ShopID";
                comm.Parameters.AddWithValue("@ShopID", ShopID);
                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool DeleteAccommodation(int id)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();
            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "DELETE FROM ACCOMMODATION WHERE hotel_ID = @id";
                comm.Parameters.AddWithValue("@id", id);

                int dr = comm.ExecuteNonQuery();

                if (dr == 1)
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        public bool DeletePOI(int poiID)
        {
            bool res = false;
            SqlCommand comm = new SqlCommand();

            try
            {
                conn.ConnectionString = conString;
                comm.Connection = conn;
                conn.Open();
                comm.CommandText = "DELETE FROM POI WHERE poi_ID = @poiID";
                comm.Parameters.AddWithValue("@poiID", poiID);

                int results = comm.ExecuteNonQuery();

                if (results == 1)
                {
                    res = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return res;
        }

        #endregion


        public List<displayItem> retrieveBriefInfo(string name)
        {
            SqlCommand comm = new SqlCommand();
            List<displayItem> briefItemList = new List<displayItem>();
            displayItem item = null;
            try
            {
                conn.ConnectionString = conString;
                conn.Open();
                comm.Connection = conn;

                switch (name)
                {
                    case "Accommodation":

                        comm.CommandText = "SELECT Hotel_Img,Name,Description From Accommodation";
                        break;

                    case "Dining":

                        comm.CommandText = "Select Dining_Image,Dining_Name,Dining_Desc from Dining";

                        break;

                    case "Events":

                        comm.CommandText = "Select Event_Image,Event_Name,Event_Desc from Events";

                        break;

                    case "POI":

                        comm.CommandText = "Select poi_Img,poi_Name,description from POI";

                        break;

                    case "Shopping":

                        comm.CommandText = "Select Shop_Image,Shop_Name,Shop_Desc from Shopping";

                        break;
                }


                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    item = new displayItem();
                    item.Image = (byte[])dr[0];
                    item.Name = dr[1].ToString();
                    item.Description = dr[2].ToString();

                    briefItemList.Add(item);

                }
                dr.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);

            }
            conn.Close();
            return briefItemList;
        }

        #region Itinerary
        public bool insertItinerary(List<Itinerary> it)
        {

            int count = 0;
            int row = 0;
            bool res = false;
            SqlCommand comm = null;
            try
            {
                conn.ConnectionString = conString;

                conn.Open();

                for (int i = 0; i < it.Count; i++)
                {
                    comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = "INSERT INTO Itinerary values (@deviceID" + count + ",@jid" + count + " ,@name" + count + " ,@lat" + count + " ,@long" + count + ")";
                    comm.Parameters.AddWithValue("@deviceID" + count, it[i].DeviceID);
                    comm.Parameters.AddWithValue("@jid" + count, it[i].JourneyID);
                    comm.Parameters.AddWithValue("@name" + count, it[i].Place);
                    comm.Parameters.AddWithValue("@lat" + count, it[i].Latitude);
                    comm.Parameters.AddWithValue("@long" + count, it[i].Longitude);
                    row += comm.ExecuteNonQuery();
                    count++;
                }


                if (row == it.Count)
                {
                    res = true;
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool checkItinerary(string deviceID, int jid)
        {

            bool res = false;
            SqlCommand comm = null;
            try
            {
                conn.ConnectionString = conString;

                conn.Open();

                comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select count(*) from Itinerary where deviceID=@deviceID AND jid=@jid";
                comm.Parameters.AddWithValue("@deviceID", deviceID);
                comm.Parameters.AddWithValue("@jid", jid);

                int row = (int)comm.ExecuteScalar();

                if (row > 0)
                {
                    res = true;
                }



            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        public bool deleteItinerary(string deviceID, int jid)
        {

            bool res = false;
            SqlCommand comm = null;
            try
            {
                conn.ConnectionString = conString;

                conn.Open();

                comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "delete from Itinerary where deviceID=@deviceID AND jid=@jid";
                comm.Parameters.AddWithValue("@deviceID", deviceID);
                comm.Parameters.AddWithValue("@jid", jid);

                int row = comm.ExecuteNonQuery();

                if (row > 0)
                {
                    res = true;
                }



            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
            return res;
        }

        #endregion
    }
    
}
