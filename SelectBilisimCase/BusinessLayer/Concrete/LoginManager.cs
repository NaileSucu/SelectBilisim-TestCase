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
    public class LoginManager:IUserLoginsService
    {
        IUserLoginsDal _userLoginsDAL;

        public LoginManager(IUserLoginsDal userLoginsDAL)
        {
            _userLoginsDAL = userLoginsDAL;
        }

        public void TAdd(UserLogins t)
        {
            _userLoginsDAL.Insert(t);
        }

        public void TDelete(UserLogins t)
        {
            _userLoginsDAL.Delete(t);
        }

        public UserLogins TGetByID(int id)
        {
            return _userLoginsDAL.GetByID(id);
        }

        public List<UserLogins> TGetList()
        {
            return _userLoginsDAL.GetList();
        }

        public void TUpdate(UserLogins t)
        {
            _userLoginsDAL.Update(t);
        }
    }
}
