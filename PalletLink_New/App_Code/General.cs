using System;
using System.Web;


/// <summary>
/// Summary description for General
/// </summary>
public class General
{
    public General()
    {
    }

    public string UserName
    {
        get { return GetCookie("TMUserName"); }
        set { SetCookie("TMUserName", value); }
    }

    public int Number
    {
        get
        {
            int inumber;
            if (!int.TryParse(GetCookie("TMNumber"), out inumber))
                inumber = -1;
            return inumber;
        }
        set { SetCookie("TMNumber", value.ToString()); }
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
        Cook.Expires = DateTime.Now.AddDays(1);
        HttpContext.Current.Response.Cookies.Add(Cook);
    }

    public string MainPath
    {
        get
        {
            return "\\\\gdlcad01\\CAD_DATA\\";
        }
    }
}