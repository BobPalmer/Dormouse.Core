namespace Dormouse.Core.Repository
{
    public interface IRepositoryCRUD<T, K>
        where T : class
    {
        T Get(K id);
        void Create(T newobj);
        void Update(T chgobj);
        void Delete(T delobj);
    }
}