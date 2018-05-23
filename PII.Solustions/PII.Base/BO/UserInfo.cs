using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PII.Base.BO
{
   public class UserInfo
    {
       public int userid { get; set; }
       public string NewPassword { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zip { get; set; }      
        public int Roleid { get; set; }
        public int DealershipId { get; set; }
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public int IsActive { get; set; }
        public string IsCRMUser { get; set; }
       
    }   
}
