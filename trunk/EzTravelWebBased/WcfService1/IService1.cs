using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool checkPOI(string poiName);

        [OperationContract]
        bool checkShop(string shopName);

        [OperationContract]
        bool checkAcc(string accName);

        [OperationContract]
        bool checkEvent(string Name);

        [OperationContract]        
        bool checkDining(string dineName);

        [OperationContract]
        List<MarkerDB> retrieveMarker(int JourneyID);

        [OperationContract]
        Event retrieveEvent(string EventName);

        [OperationContract]
        Dining retrieveDining(string DiningName);

        [OperationContract]
        Shopping retrieveShopping(string ShoppingName);

        [OperationContract]
        List<string> retrieveDictionary();

        [OperationContract]
        POI retrievePOI(string poiName);

        [OperationContract]
        Accommodation retrieveHotel(string h_Name);

        [OperationContract]
        List<string> retrievePOIName();

        [OperationContract]
        List<string> retrieveEventName();

        [OperationContract]
        List<string> retrieveShopName();

        [OperationContract]
        List<string> retrieveDiningName();

        [OperationContract]
        bool checkLogin(string userName, string password);

        [OperationContract]
        bool InsertMarker(MarkerDB marker);

        [OperationContract]
        bool InsertEvent(Event events);

        [OperationContract]
        bool InsertDining(Dining dining);

        [OperationContract]
        bool InsertShopping(Shopping shopping);

        [OperationContract]
        bool InsertAccommodation(Accommodation acc);

        [OperationContract]
        bool InsertPOI(POI poi);

        [OperationContract]
        bool UpdateMarker(MarkerDB marker);

        [OperationContract]
        bool UpdateEvent(Event events);

        [OperationContract]
        bool UpdateDining(Dining dining);

        [OperationContract]
        bool UpdateShopping(Shopping shopping);

        [OperationContract]
        bool UpdateAccommodation(Accommodation acc);

        [OperationContract]
        bool UpdatePOI(POI poi);

        [OperationContract]
        bool DeleteMarker(int JID, string MarkerID);

        [OperationContract]
        bool DeleteEvent(int eventID);

        [OperationContract]
        bool DeleteDining(int Dining);

        [OperationContract]
        bool DeleteShopping(int ShopID);

        [OperationContract]
        bool DeleteAccommodation(int id);

        [OperationContract]
        bool DeletePOI(int poiID);

        [OperationContract]
        List<displayItem> retrieveBriefInfo(string name);

        [OperationContract]
        bool insertItinerary(List<Itinerary> it);

        [OperationContract]
        bool checkItinerary(string deviceID, int jid);

        [OperationContract]
        bool deleteItinerary(string deviceID, int jid);
    }
}
