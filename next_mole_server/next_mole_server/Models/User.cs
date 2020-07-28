using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using next_mole_server.Models.DAL;

namespace next_mole_server.Models
{
    public class User
    {
        //in DB it calls userPlayer because user is save word
        string userEmail;
        string userPassword;
        string userName;
        //string userBirthdate;
        string gender;
        //bool isFunder;
        //string userImage;
        //int score;
        //bool soundPer;
        //bool notificationPer;

        public string UserEmail { get => userEmail; set => userEmail = value; }
        public string UserPassword { get => userPassword; set => userPassword = value; }
        public string UserName { get => userName; set => userName = value; }
        //public string UserBirthdate { get => userBirthdate; set => userBirthdate = value; }
        public string Gender { get => gender; set => gender = value; }
        //public bool IsFunder { get => isFunder; set => isFunder = value; }
        //public string UserImage { get => userImage; set => userImage = value; }
        //public int Score { get => score; set => score = value; }
        //public bool SoundPer { get => soundPer; set => soundPer = value; }
        //public bool NotificationPer { get => notificationPer; set => notificationPer = value; }




        public bool checkLogin(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.checkLogin(email, password);
        }
        public List<User> getAll()
        {
            DBservices dbs = new DBservices();
            return dbs.getAllusers();

        }
        public int InsertUser()
        {
            DBservices dbs = new DBservices();
            bool isExist= dbs.checkEmail(this.userEmail);
            if (!isExist)
            {
                int num = dbs.DBinsertUser(this);
                return num;
            }
            else{
                return 0;
            }
           
           
        }
    }
}