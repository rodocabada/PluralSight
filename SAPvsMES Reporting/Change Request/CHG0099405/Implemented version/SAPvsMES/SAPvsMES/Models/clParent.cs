using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SAPvsMES.Models
{
    public class clParent
    {
        public string strConnectionString = ConfigurationManager.ConnectionStrings["SiteLogix"].ConnectionString;
        public string strConnectionString2 = ConfigurationManager.ConnectionStrings["MESvsSAP"].ConnectionString;
    }
}