using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class UserManager : GenericManager<User>, IUserService
    {
        public UserManager(IUserDAL dal) : base(dal) { }

    }
}
