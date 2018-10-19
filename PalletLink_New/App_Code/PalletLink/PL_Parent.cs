using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;
using System.Web.UI.WebControls;

    public class PL_Parent : System.Web.UI.Page
    {
        public string strDomain = System.Configuration.ConfigurationManager.AppSettings["Domain"].ToString();
        public string strSQLServer = System.Configuration.ConfigurationManager.AppSettings["SQLServer"].ToString();
        public string strSQLDataBase = System.Configuration.ConfigurationManager.AppSettings["SQLDatabase"].ToString();
        public string strSQLDataBasePallet = System.Configuration.ConfigurationManager.AppSettings["SQLDatabasePallet"].ToString();
        public string strSender = System.Configuration.ConfigurationManager.AppSettings["SenderEMail"].ToString();
        public string strSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
        public string strSQLServerTE = System.Configuration.ConfigurationManager.AppSettings["SQLServer"].ToString();
        public string strSQLDatabaseTE = System.Configuration.ConfigurationManager.AppSettings["SQLDatabase"].ToString();

        // public int iLimitCycles = 500;

        public void SendNotificationMail(string MailFrom, string Body, string Subject, string Email, MailPriority Priority)
        {
            if (!string.IsNullOrEmpty(MailFrom) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Body))
            {
                MailMessage objMail = new MailMessage();
                SmtpClient objSMTPServer = new SmtpClient(strSMTPServer);

                objMail.Priority = Priority;
                objMail.Subject = Subject;
                objMail.IsBodyHtml = true;
                objMail.Body = Body;
                objMail.From = new MailAddress(MailFrom);
                objMail.To.Add(Email);
                if (objMail.To.Count > 0)
                {
                    objSMTPServer.Send(objMail);
                }
                objMail.Dispose();
                objMail = null;
                objSMTPServer = null;
            }
        }

        public static void fillCombo(DataSet dataSet, DropDownList combo, string values, string items)
        {
            combo.Items.Clear();
            combo.Items.Add(new ListItem("Selecciona Una Opcion", "-1"));
            combo.SelectedItem.Enabled = false;

            try
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    int a = 0;
                    while (a < dataSet.Tables[0].Rows.Count)
                    {
                        combo.Items.Add(
                            new ListItem(dataSet.Tables[0].Rows[a][items].ToString(),
                            dataSet.Tables[0].Rows[a][values].ToString()));
                        a++;
                    }
                }
                dataSet.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void clear()
        {
            Session["PKUser"] = 0;
            Session["User"] = "";
            Session["PKCustomer"] = 0;
            Session["Customer"] = "";
            Session["Rows"] = 0;
            Session["Columns"] = 0;
            Session["AssemblyID"] = 0;
            Session["EquipmentID"] = 0;
            Session["Equipment"] = "";
            Session["MARouteID"] = 0;
            Session["Route"] = "";
            Session["Prefix"] = "";
            Session["PalletID"] = "";





        }
        public string GetCookie(string Name)
        {
            System.Web.HttpCookie Cook = HttpContext.Current.Request.Cookies[Name];
            if (Cook == null)
                return "";
            else
                return Cook.Value;
        }

        public void SetCookie(string Name, string Value)
        {
            System.Web.HttpCookie Cook = new HttpCookie(Name, Value);
            Cook.Expires = DateTime.Now.AddDays(365);
            HttpContext.Current.Response.Cookies.Add(Cook);
        }

        public static bool fnUserHasAccess(string strSQLServer, string strSQLDataBase, int fkUser)
        {

            CLPalletLink.SC_Users scUsers = new CLPalletLink.SC_Users();
            DataSet dsAccess = new DataSet();
            bool HasAccess = false;


            dsAccess = scUsers.CheckIfHaveAccess(strSQLServer, strSQLDataBase, fkUser);
            if (dsAccess.Tables[0].Rows.Count > 0)
            {
                int haveAccess = Int32.Parse(dsAccess.Tables[0].Rows[0].ItemArray[3].ToString());
                if (haveAccess == 1)
                {
                    HasAccess = true;

                }

            }
            return HasAccess;
        }
    }
