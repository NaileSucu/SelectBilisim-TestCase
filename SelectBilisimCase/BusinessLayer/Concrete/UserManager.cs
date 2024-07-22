using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserManager:IUserService
    {
        IUserDal _userDAL;

        public UserManager(IUserDal userDAL)
        {
            _userDAL = userDAL;
        }

        public void TAdd(User t)
        {
            _userDAL.Insert(t);
        }

        public void TDelete(User t)
        {
            _userDAL.Delete(t);
        }

        public User TGetByID(int id)
        {
            return _userDAL.GetByID(id);
        }

        public List<User> TGetList()
        {
            return _userDAL.GetList();
        }

        public void TUpdate(User t)
        {
            _userDAL.Update(t);
        }
    }
}
