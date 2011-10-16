namespace Dormouse.Core.Repository
{
    public abstract class RepositoryCRUD<TD, TK> 
        : RepositoryBase<TD>
          , IRepositoryCRUD<TD, TK>
        where TD : class 
    {
        public virtual TD Get(TK keyval)
        {
            TD r;
            r = CurrentSession.Get<TD>(keyval);
            return r;
        }

        public virtual void Update(TD objToUpdate)
        {
            using (var tran = CurrentSession.BeginTransaction())
            {
                CurrentSession.SaveOrUpdate(objToUpdate);
                tran.Commit();
            }
        }

        public virtual void Create(TD objToUpdate)
        {
            Update(objToUpdate);
        }

        public void Delete(TD objToDelete)
        {
            using (var tran = CurrentSession.BeginTransaction())
            {
                CurrentSession.Delete(objToDelete);
                tran.Commit();
            }
        }
    }
}