using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;
using NFX.IO.ErrorHandling;
using NFX.Log;
using NFX.Wave.MVC;
using Wave.UrlShortener.Models;

namespace Wave.UrlShortener.Controllers
{
    public class Home:Controller
    {
        private char[] possibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public Home()
        {
            App.Log.Write(MessageType.Debug,"Create home controller");
        }
        [Action]
        public object Index()
        {
            var query = new Query(Data.Scripts.GetAllUrls, typeof(UrlRecord));
            var urls = ((ICRUDOperations)App.DataStore)
                                    .LoadOneRowset(query)
                                    .AsEnumerableOf<UrlRecord>();
            return new Pages.Index() { Urls = urls};   
        }

        [Action(name: "AddUrl", order: 0, matchScript: "match{methods=POST}")]
        public object AddUrl(String url)
        {
            if (!String.IsNullOrWhiteSpace(url))
            {
                var __baseUrl = WorkContext.Request.UserHostName;
                var __short = String.Format("{0}://{1}/${2:X}", WorkContext.Request.Url.Scheme, __baseUrl, GetRandomString(6));
                while (!ShortUrlExists(__short))
                {
                    App.Log.Write(MessageType.Debug, String.Format("Dublicate of the short url:{0}", __short));
                    __short = String.Format("{0}://{1}/${2:X}", WorkContext.Request.Url.Scheme, __baseUrl, GetRandomString(6));
                }
                App.Log.Write(MessageType.Debug, String.Format("Url :{0} - short:{1}", url, __short));

                var __urlRecord = new UrlRecord
                {
                    ID = Guid.NewGuid().ToString("N"),
                    OriginalUrl = url,
                    ShortUrl = __short,
                    CreateDate = DateTime.Now
                };
                var __error = __urlRecord.Validate();
                if (__error == null)
                {
                    AppContext.DataStore.Insert(__urlRecord);
                }
                if (WorkContext.RequestedJSON)
                    return new ClientRecord(__urlRecord, __error);
            }
            return new Redirect("/");
        }

        [Action]
        public void Remove(String id)
        {
            AppContext.DataStore.Delete(new UrlRecord { ID = id });
        }

        private string GetRandomString(int num)
        {
            var rnd = new Random();
            var result = new char[num];
            while (num-- > 0)
            {
                result[num] = possibleChars[rnd.Next(possibleChars.Length)];
            }
            return new string(result);
        }

        private bool ShortUrlExists(String shortUrl)
        {
            var query = new Query(Data.Scripts.GetUrlByShort, typeof(UrlRecord))
                                {
                                     new Query.Param("shorturl", shortUrl)
                                };
            var exists = ((ICRUDOperations)App.DataStore)
                                    .Load(query).Any();
            return exists;
        }
    }

}
