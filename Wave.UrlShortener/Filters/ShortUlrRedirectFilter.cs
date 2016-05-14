using NFX;
using NFX.DataAccess.CRUD;
using NFX.Environment;
using NFX.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using Wave.UrlShortener.Models;

namespace Wave.UrlShortener.Filters
{
    public class ShortUlrRedirectFilter : WorkFilter
    {

     

        #region .ctor

        public ShortUlrRedirectFilter(WorkDispatcher dispatcher, string name, int order) : base(dispatcher, name, order)
        {
        }

        public ShortUlrRedirectFilter(WorkHandler handler, string name, int order) : base(handler, name, order)
        {
        }
        public ShortUlrRedirectFilter(WorkDispatcher dispatcher, IConfigSectionNode confNode) : base(dispatcher, confNode)
        {
        }

        public ShortUlrRedirectFilter(WorkHandler handler, IConfigSectionNode confNode) : base(handler, confNode)
        {
        }

        #endregion
        protected override void DoFilterWork(WorkContext work, IList<WorkFilter> filters, int thisFilterIndex)
        {
            if (work.Request.Url.LocalPath.StartsWith("/$"))
            {
                var shortUrl = work.Request.Url.AbsoluteUri;
                UrlRecord url = null;
                if (!AppContext.UrlCache.TryGetValue(shortUrl,out url))
                {
                    url = GetFromDB(shortUrl);
                   
                    if (url != null)
                    {
                        AppContext.UrlCache[shortUrl] = url;
                    }
                }
                if (url != null)
                {
                    url.LastAccessDate = DateTime.Now;
                    ((ICRUDOperations)App.DataStore).UpdateAsync(url);
                    work.Response.RedirectAndAbort(url.OriginalUrl);
                }
                else
                {
                    work.Response.RedirectAndAbort("/");
                }
            }
            this.InvokeNextWorker(work, filters, thisFilterIndex);
        }

        private UrlRecord GetFromDB(string shortUrl)
        {
            var query = new Query(Data.Scripts.GetUrlByShort, typeof(UrlRecord))
                                {
                                     new Query.Param("shorturl", shortUrl)
                                };
            return ((ICRUDOperations)App.DataStore)
                                    .LoadOneRow(query) as UrlRecord;
        }
    }
}
