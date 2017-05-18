using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Intranet.API.UnitTests.Fakes
{
  public static class DbContextFake
  {
    public static TContext GetDbContext<TContext>([CallerMemberName] string callerName = "")
      where TContext : DbContext
    {
      var data = SingleItem(String.Empty, new List<object>());
      return GetDbContext<TContext>(data, callerName);
    }

    public static TContext GetDbContext<TContext>(string property, object obj, [CallerMemberName] string callerName = "")
      where TContext : DbContext
    {
      var listType = typeof(List<>).MakeGenericType(obj.GetType());
      var listOfEntities = (IList)Activator.CreateInstance(listType);
      listOfEntities.Add(obj);

      return GetDbContext<TContext>(new List<(string, IEnumerable<object>)> { (property, listOfEntities as IEnumerable<object>) }, callerName);
    }

    public static TContext GetDbContext<TContext>(string property, IEnumerable<object> obj, [CallerMemberName] string callerName = "")
      where TContext : DbContext
    {
      var data = SingleItem(property, obj);
      return GetDbContext<TContext>(data, callerName);
    }

    public static TContext GetDbContext<TContext>(IEnumerable<(string property, IEnumerable<object> obj)> data, [CallerMemberName] string callerName = "")
      where TContext : DbContext
    {
      var options = new DbContextOptionsBuilder<TContext>()
          .UseInMemoryDatabase(databaseName: callerName)
          .Options;

      var context = (TContext)Activator.CreateInstance(typeof(TContext), options);

      var addRangeMethodName = nameof(List<string>.AddRange);
      var setMethodName = nameof(context.Set);

      foreach (var item in data)
      {
        if (String.IsNullOrWhiteSpace(item.property) || item.obj == null)
        {
          continue;
        }

        var typeOfEntity = typeof(TContext)
          .GetProperty(item.property)
          .PropertyType
          .GetGenericArguments()
          .SingleOrDefault();

        var dbSet = context
          .GetType()
          .GetMethod(setMethodName)
          .MakeGenericMethod(typeOfEntity)
          .Invoke(context, null);

        dbSet
          .GetType()
          .GetMethod(addRangeMethodName, new Type[] { item.obj.GetType() })
          .Invoke(dbSet, new[] { item.obj });
      }
      context.Database.EnsureDeleted();
      context.SaveChanges();

      return context;
    }

    #region Private helpers

    private static IEnumerable<(string, IEnumerable<T>)> SingleItem<T>(string property, IEnumerable<T> data)
    {
      return new List<(string, IEnumerable<T>)> { (property, data) };
    }

    #endregion
  }
}
