using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
//using Unity.Services.Core.Environments;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public async void Awake()
    {
      try {
            //var options = new InitializationOptions()
            //  .SetEnvironmentName("dev");
            await UnityServices.InitializeAsync();

            if(UnityServices.InitializeAsync().IsCompleted)
            { 

              SendCustomeEvent();
            }
        }
        catch (Exception exception) {
            Debug.Log("There is an error to initialize env " + exception.Message);

        }
    }
    public void SendCustomeEvent()
    {
        Dictionary<string , object> userParams = new Dictionary<string , object>()
        {
            { "userLocale", "WA" },
            { "sessionID", "New Player SessionId" },
            { "clientVersion", "New Client Version" },
            { "platform", "New PlayerPlatform" },
            { "userCountry", "USA" }
        };

        Dictionary<string , object> parameters = new Dictionary<string , object>()
        {
           // { "userID", "5960d0de0422eb34c841634421d67fb8" },
           // { "eventName", "myevent" },
            // { "eventUUID", evId }
            {"userLocale" , "en_US" },
            {"vrName" , "Angelique" }
        };
        Events.CustomData("myevent", parameters); 
        
        Events.Flush();
    }
    public void SendTransaction()
    {

        var productsReceived = new Events.Product()
        {
            items = new List<Events.Item>()

        {    new Events.Item(){ itemName = "Short Handle Racket" , itemType = "Racket" , itemAmount = 1 },
             new Events.Item() { itemName = "Green Ball" , itemType = "Ball" , itemAmount = 1 } ,
             new Events.Item() { itemName = "Red Ball" , itemType = "Ball" , itemAmount = 1 }
        } ,
               virtualCurrencies = new List<Events.VirtualCurrency>()

                { new Events.VirtualCurrency(){ virtualCurrencyName = "Gold", virtualCurrencyType = "PREMIUM", virtualCurrencyAmount = 100}
                }
        };

        var productsSpent = new Events.Product()

        {
            realCurrency = new Events.RealCurrency() { realCurrencyType = "USD" , realCurrencyAmount = 499 }
        };

        Events.Transaction( new Events.TransactionParameters()

        { productsReceived = productsReceived , productsSpent = productsSpent , transactionID = "100000576198248" , transactionName = "IAP - A Large Treasure Chest" , transactionType = Events.TransactionType.PURCHASE , transactionServer = Events.TransactionServer.APPLE , transactionReceipt = "ewok9Ja81............991KS==" }
        );
    }
}
