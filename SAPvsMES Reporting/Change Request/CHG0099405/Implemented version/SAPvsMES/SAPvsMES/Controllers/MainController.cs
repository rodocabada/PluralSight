using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Data;
using SAPvsMES.Models;
using clsHanaMES;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

namespace SAPvsMES.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/
        public ActionResult Index()
        {
            //For testing only
            //Response.Cookies["imgCustomer"].Value = "logo-jabil@2x.png";
            //Response.Cookies["imgCustomer"].Expires = DateTime.Now.AddDays(1);
            //Response.Cookies["customer"].Value = "Thermofisher";
            //Response.Cookies["customer"].Expires = DateTime.Now.AddDays(1);
            //End For testing only

            ViewBag.imgCustomer = "";
            ViewBag.imgCustomerVisible = "display: none";
            ViewBag.dashBoardVisible = "display: none";

            string submit, dateFrom, dateTo;
            clModelMain clModelMaint = new clModelMain();            
            submit = "";
            string selectedCustomerValue = Request.Form["ddlCustomers"];
            ViewBag.selectedCustomer = selectedCustomerValue;
            string selectedCustomerText = Request.Form["hfCustomerText"];

            ddlCustomersFill(clModelMaint);

            //if customer/image is null redirect to Main Page to select a customer
            //if (!(Request.Cookies["customer"] != null && Request.Cookies["imgCustomer"] != null))
            //{
            //    Response.Redirect("http://mxchim0web06/ManagementReports/");
            //}
            dateFrom = Request["txtStartDate"];
            submit = Request["txtHiddenSubmit"];
           
            
            //prevents resources problems (images, css, js)
            if (!Request.Path.EndsWith("/") && submit != "submit")
            {
                return RedirectToAction("Index", "Main");
            }
            if (submit == "submit")
            {
                clParent clParent;
                CT_Hana obj;
                DataTable dt;
                List<clsHanaInfo> HanaInfoList = new List<Models.clsHanaInfo>();
                dateFrom = Request["dateFrom"];
                dateTo = Request["dateTo"];
                clParent = new clParent();
                obj = new CT_Hana(clParent.strConnectionString2);
                Dictionary<object, object> customersLogo = new Dictionary<object, object>();
                string imgCustomer;
                try
                {
                    dt = obj.GetMESInfo(Int32.Parse(selectedCustomerValue));
                    clModelMaint.dt = dt;
                    if (selectedCustomerValue == null || selectedCustomerValue == "" || clModelMaint.dt.Rows.Count <= 0)
                    {
                        ViewBag.imgCustomerVisible = "display: none";
                        clModelMaint = new clModelMain();
                        return View(clModelMaint);
                    }
                    clModelMaint.lastUpdated = dt.Rows[0].ItemArray[5].ToString();
                    customersLogo = ViewBag.customersLogo;
                    imgCustomer = (string)customersLogo[Int32.Parse(selectedCustomerValue)];
                    if (string.IsNullOrEmpty(imgCustomer))
                    {
                        ViewBag.imgCustomer = "";
                        ViewBag.imgCustomerVisible = "display: none";
                    }
                    else
                    {
                        if (Int32.Parse(selectedCustomerValue).Equals(1))
                        {
                            ViewBag.dashBoardVisible = "display: inline-block";
                        }
                        else
                        {
                            ViewBag.dashBoardVisible = "display: none";
                        }
                        ViewBag.imgCustomer = imgCustomer;
                        ViewBag.imgCustomerVisible = "display: inline-block";
                    }                   
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                }
                finally
                {
                    dt = null;
                }
                //if submit button is pressed
                //here you can send user parameters to Model(or Class Library)

            }

            return View(clModelMaint);
        }

        private void ddlCustomersFill(clModelMain clModelMaint)
        {
            DataTable customers = new DataTable();
            Dictionary<object, object> customersLogo = new Dictionary<object,object>();
            try
            {
                customers = getCustomersList();
                ViewBag.Customers = new SelectList(customers.AsDataView(), "PKCustomer", "Description");
                customersLogo = DataTableToDictionary(customers, "PKCustomer", "ImageLogo");
                ViewBag.customersLogo = customersLogo;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message.ToString());
            }
        }

        private DataTable getCustomersList()
        {
            clParent clParent = new clParent();
            DataTable customers = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clParent.strConnectionString2))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "up_GetCustomers";
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    dataAdapter.Fill(customers);
                    return customers;
                }
            }

        }

        private Dictionary<object, object> DataTableToDictionary(DataTable dt, string keyColName, string ValColName)
        {
            return dt.AsEnumerable()
                 .ToDictionary<DataRow, object, object>(row => row.Field<object>(keyColName),
                                                row => row.Field<object>(ValColName));
        }
       
    }
}
