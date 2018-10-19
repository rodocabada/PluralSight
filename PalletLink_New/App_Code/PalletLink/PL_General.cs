using System;
using System.Web;


/// <summary>
/// Summary description for General
/// </summary>
public class PL_General
{
    public PL_General()
    {
    }

    public string UserName
    {
        get { return GetCookie("UserName"); }
        set { SetCookie("UserName", value); }
    }

    public int Number
    {
        get
        {
            int inumber;
            if (!int.TryParse(GetCookie("Number"), out inumber))
                inumber = -1;
            return inumber;
        }
        set { SetCookie("Number", value.ToString()); }
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
       // Cook.Expires = DateTime.Now.AddDays(365);
        Cook.Expires = DateTime.Now.AddHours(6);
        HttpContext.Current.Response.Cookies.Add(Cook);
    }

    public string MainPath
    {
        get
        {
            return "\\\\gdlcad01\\CAD_DATA\\";
        }
    }

    public void DeleteCookie(HttpCookie httpCookie)
    {
        try
        {
            httpCookie.Value = null;
            httpCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Request.Cookies.Add(httpCookie);
        }
        catch 
        {
           
        }
    }

}