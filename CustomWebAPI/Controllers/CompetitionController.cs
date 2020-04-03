using CustomWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Results;
using System.Windows;

namespace CustomWebAPI.Controllers
{
    public class CompetitionController : ApiController
    {
        private CustomAPIEntities db = new CustomAPIEntities();
        private string msg = "";
        [HttpGet]
        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }
        [HttpGet]
        public List<User> GetTopUsers()
        {
            return db.Users.OrderByDescending(x => x.Score).Take(10).ToList();
        }
        public string Register(string firstname, string lastname, string phone, string email)
        {
            User user = new User();
            if (!db.Users.Where(x => x.Email.Contains(email)).Any())
            {
                user.Email = email;
                user.Score = 1;
                user.FirstName = firstname;
                user.LastName = lastname;
                user.PhoneNo = phone;
                db.Users.Add(user);
                user.Link = "https://localhost:44391/api/Competition?assignedby=";
                db.SaveChanges();
                user.Link+= user.ID;
                db.SaveChanges();

                sendMail(user.Email, user.Link);

                msg = "User Registered Succesfully. Check your email for referral link.";
            }
            else
            {
                msg = "User Already Registered !";
            }
            return msg;
        }

        private void sendMail(string email, string link)
        {
            try
            {
                MailMessage mailMessage = new MailMessage("olumideadewusis@gmail.com", email);
                mailMessage.Subject = "Referral Link";
                string msg = "";
                msg = "Following is your referral code you can share: ";
                mailMessage.Body = msg + link;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("olumideadewusis@gmail.com", "Confidence24");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch(Exception ex)
                {
                MessageBox.Show(ex.ToString());
            }
        }

        public string Register(int assignedby, string firstname, string lastname, string phone, string email)
        {
            User user = new User();
            if(!db.Users.Where(x => x.Email.Contains(email)).Any())
            {
                user.Email = email;
                user.Score = 1;
                user.FirstName = firstname;
                user.LastName = lastname;
                user.PhoneNo = phone;
                user.AssignedBy = assignedby;
                db.Users.Add(user);
                user.Link = "https://localhost:44391/api/Competition?assignedby=";
                db.SaveChanges();
                user.Link += user.ID;
                db.SaveChanges();
                var user1 = user.ID;
                user = db.Users.Find(assignedby);
                user.Score++;
                db.SaveChanges();

                sendMail(user.Email, user.Link);

                msg = "User Registered Succesfully. Check your email for referral link.";
            }
            else
            {
                msg = "User Already Registered !";
            }
            return msg;
        }
    }
}
