using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace StarterPack.Core.Helpers
{
    public class Services
    {
         private static IServiceProvider _provider { get; set; }
         public static IDictionary<int, DbContext> DbContextByThread { get; set; }
       
         public static T Resolve<T>() => (T)_provider.GetService(typeof(T)); 

         public static object Resolve(Type t) => _provider.GetService(t);

         public static void SetProvider(IServiceProvider provider){
             _provider = provider;
         }
        
         public static DbContext DefaultDbContext {
             get{
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                if(DbContextByThread.ContainsKey(threadId)){
                    return DbContextByThread.First(s=>s.Key == threadId).Value;
                }
                else {
                    return null;
                }                
            }
            set{
                if(DbContextByThread == null) {
                    DbContextByThread = new Dictionary<int, DbContext>();
                }
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

                if(!DbContextByThread.ContainsKey(threadId)){
                    DbContextByThread[threadId] = value;
                }
            }
         }       
    }
}


