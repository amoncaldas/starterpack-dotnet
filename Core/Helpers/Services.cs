using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if(DbContextByThread.ContainsKey(threadId)){
                    return DbContextByThread.First(s=>s.Key == threadId).Value;
                }
                else {
                    return null;
                }                
            }            
         }  

         public static void SetCurrentThreadDbContext(DbContext dbContext){
            if(DbContextByThread == null) {
                DbContextByThread = new Dictionary<int, DbContext>();
            }
            int threadId = Thread.CurrentThread.ManagedThreadId;

            if(!DbContextByThread.ContainsKey(threadId)){
                DbContextByThread[threadId] = dbContext;
            }
         }

         public static void RemoveCurrentThreadDbContext(){
            int thredId = Thread.CurrentThread.ManagedThreadId;
            if(DbContextByThread.ContainsKey(thredId)) {
                DbContextByThread.Remove(thredId);
            }
         }     
    }
}


