using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.API.UnitTests.Mocks
{
  public static class DbSetMock
  {
    public static Mock<DbSet<TEntity>> MockSet<TEntity>(IEnumerable<TEntity> data)
        where TEntity : class
    {
      var queryableData = data.AsQueryable();
      var mockSet = new Mock<DbSet<TEntity>>();
      mockSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryableData.Provider);
      mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableData.Expression);
      mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
      mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

      return mockSet;
    }
  }
}
