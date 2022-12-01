using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Newtonsoft.Json;
using System.Security;

namespace Proxy
{
    // DOC : 
    // https://www.c-sharpcorner.com/article/memory-cache-in-c-sharp/

    /**
     * Utilisez cette classe JCDecauxItem dans le GenericProxyCache 
     * que vous avez créé pour gérer les demandes à JCDecaux à la volée.
     */
    internal class GenericProxyCache<T>
    {
        ObjectCache cache = MemoryCache.Default;

        /**
         * où CacheItemName est la clé de l'entrée dans le cache. 
         * Si CacheItemName n'existe pas ou a un contenu nul, 
         * créez un nouvel objet T et placez-le dans le cache avec
         * CacheItemName comme clé correspondante. Dans ce cas, 
         * le délai d'expiration est "dt_default" 
         * ( public DateTimeOffset dt_default in ProxyCache class). 
         * À l'instanciation d'un objet ProxyCache, 
         * dt_default = ObjectCache.InfiniteAbsoluteExpiration 
         * (aucun délai d'expiration), mais dt_default peut être modifié. 
         */
        public T Get(string CacheItemName)
        {
            T val = (T)cache[CacheItemName];

            if( val == null)
            {
                ApiManager api = new ApiManager();
                DateTimeOffset dt_default = ObjectCache.InfiniteAbsoluteExpiration;
                cache.Add(CacheItemName, api.run(CacheItemName), dt_default);
                val = (T)cache[CacheItemName];
                Console.WriteLine("Pas en cache");
            }
            else
            {
                Console.WriteLine("En cache");
            }

            return val;
        }
    

        /**
         * où CacheItemName est la clé de l'entrée dans le cache. 
         * Si CacheItemName n'existe pas ou a un contenu nul, 
         * créez un nouvel objet T et placez-le dans le cache 
         * avec CacheItemName comme clé correspondante. 
         * Dans ce cas, le délai d'expiration est maintenant + dt_secondes secondes.
         */
        public T Get(string CacheItemName, double dt_seconds)
        {
            T val = (T)cache[CacheItemName];

            if (val == null)
            {
                ApiManager api = new ApiManager();

                /**
                 * Initializes a new instance of the DateTime structure to the specified
                 * year, month, day, hour, minute, second, millisecond, 
                 * and Coordinated Universal Time (UTC) or local time for the specified calendar.
                 * DateTime(Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32)
                 */
                cache.Add(CacheItemName, api.run(CacheItemName), DateTimeOffset.Now.AddSeconds(dt_seconds));
                val = (T)cache[CacheItemName];
                Console.WriteLine("Pas en cache");
            }
            else
            {
                Console.WriteLine("En cache");
            }

            return val;
        }

        /**
         * où CacheItemName est la clé de l'entrée dans le cache. 
         * Si CacheItemName n'existe pas ou a un contenu nul, 
         * créez un nouvel objet T et placez-le dans le cache 
         * avec CacheItemName comme clé correspondante. 
         * Dans ce cas, le délai d'expiration est dt (classe DateTimeOffset).
         */
        public T Get(string CacheItemName, DateTimeOffset dt)
        {
            T val = (T)cache[CacheItemName];

            if (val == null)
            {
                ApiManager api = new ApiManager();

                cache.Add(CacheItemName, api.run(CacheItemName), dt);
                val = (T)cache[CacheItemName];
                Console.WriteLine("Pas en cache");
            }
            else
            {
                Console.WriteLine("En cache");
            }

            return val;
        }
    }
}
