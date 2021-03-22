using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {

    }
}
