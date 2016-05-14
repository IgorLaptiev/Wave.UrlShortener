using System;
using NFX;
using NFX.DataAccess.CRUD;
using Wave.UrlShortener.Models;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Wave.UrlShortener
{
    internal static class AppContext
    {
        private static ConcurrentDictionary<string, UrlRecord> _urlCache;

        public static ICRUDOperations DataStore
        {
            get { return App.DataStore as ICRUDOperations; }
        }

       public static IDictionary<String,UrlRecord> UrlCache
       {
            get
            {
                if (_urlCache == null)
                {
                    _urlCache = new ConcurrentDictionary<String, UrlRecord>();
                }
                return _urlCache;
            }
        } 
    }
}
