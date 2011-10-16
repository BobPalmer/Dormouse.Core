using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
namespace Dormouse.Core
{
    public static class NHConfiguration 
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var configuration = new Configuration().Configure();
            return configuration.BuildSessionFactory();
        }
    }
}
