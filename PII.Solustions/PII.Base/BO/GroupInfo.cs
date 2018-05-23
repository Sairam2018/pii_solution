using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PII.Base.BO
{
   public class GroupInfo
    {
       public int groupId { get; set; }
       public string GroupName { get; set; }
       public string GroupDescription { get; set; }
       public int ParentID { get; set; }
       public int LevelID { get; set; }
       public int CompanyId { get; set; }
       public int IsActive { get; set; }
       
    }
}
