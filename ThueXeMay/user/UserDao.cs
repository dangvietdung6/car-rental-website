using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThueXeMay.Models;
using PagedList;

namespace ThueXeMay.User
{
    public class UserDao
    {
        //Model1 db = null;
        RENT_MOTOREntities myObj = null;
        public UserDao()
        {
            //db = new Model1();
            myObj = new RENT_MOTOREntities();
        }
        public long Insert(user entity)
        {
            //db.users.Add(entity);
            //db.SaveChanges();
            myObj.users.Add(entity);
            myObj.SaveChanges();

            return entity.id_user;
        }

        public bool Update(user entity)
        {
            try
            {
                var user = myObj.users.Find(entity.id_user);
                user.firstName = entity.firstName;
                user.lastName = entity.lastName;
                user.address = entity.address;
                user.phoneNumber = entity.phoneNumber;
                user.gender = entity.gender;
                myObj.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<user> ListAllPaging(int page, int paseSize)
        {
            //return myObj.users.OrderByAscending(x=>x.id_user).ToPagedList(page, paseSize);
            return myObj.users.OrderByDescending(x => x.id_user).ToPagedList(page, paseSize);
        }

        public user GetById(string email)
        {
            return myObj.users.SingleOrDefault(x => x.email == email);
        }

        public user ViewDetail(int id)
        {
            return myObj.users.Find(id);
        }

        public int Login(string email, string password)
        {
            //var result = db.users.SingleOrDefault(x=> x.email == email  );
            //var result1= db.users.SingleOrDefault(x => x.email == email && x.status ==false);
            //var result2 = db.users.SingleOrDefault(x => x.email == email && x.password == password);

            var result = myObj.users.SingleOrDefault(x => x.email == email);
            var result1 = myObj.users.SingleOrDefault(x => x.email == email && x.status == false);
            var result2 = myObj.users.SingleOrDefault(x => x.email == email && x.password == password);
            //Console.WriteLine(result);
            if (result == null)
            {
                return 0;
            }
            else
            {
                //if(result.status== false)
                if (result1 != null)
                {
                    return -1;
                }
                else
                {
                    //if (result.password == password)
                    if (result2 != null)
                        return 1;
                    else
                        return -2;
                }

            }
        }
        public bool CheckEmail(string email)
        {
            //return db.users.Count(x => x.email == email) > 0;
            return myObj.users.Count(x => x.email == email) > 0;
        }



    }
}