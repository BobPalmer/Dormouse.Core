using System;
using NHibernate;

namespace Dormouse.Core.Repository
{
    public abstract class RepositoryBase<TD> 
        : IRepositoryBase<TD>
        , IDisposable
          where TD : class
    {
        private ISession _curSession;

        public ISession CurrentSession
        {
            get
            {
                return _curSession ?? 
                    (_curSession = NHConfiguration.SessionFactory.OpenSession());
            }
        }

        public void Dispose()
        {
            if (_curSession != null)
            {
                _curSession.Dispose();
            }
        }
    }
}
