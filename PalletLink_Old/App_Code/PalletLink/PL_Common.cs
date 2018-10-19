using System;
using System.Data;

/// <summary>
/// Summary description for Common
/// </summary>
public class PL_Common
{
    public string strDomain = System.Configuration.ConfigurationManager.AppSettings["Domain"].ToString();
    public string strSQLServer = System.Configuration.ConfigurationManager.AppSettings["SQLServer"].ToString();
    public string strSQLDataBase = System.Configuration.ConfigurationManager.AppSettings["SQLDatabase"].ToString();
    public string strSQLDataBasePallet = System.Configuration.ConfigurationManager.AppSettings["SQLDatabasePallet"].ToString();
    public string ERROR;

    public PL_Common()
    {
        //
        // TODO: Add constructor logic here
        //
    }
//    public  bool fnUserHasAccess(string pageName, int fkUser)
    public bool fnUserHasAccess(string pageName, int fkUser)
    {

        CLPalletLink.SC_Users Users = new CLPalletLink.SC_Users();
        DataSet dsUsersAccess = new DataSet();
        dsUsersAccess = Users.UserHasAccess(strSQLServer, strSQLDataBase, pageName, fkUser);
        //return dsUsersAccess.Tables[0].Rows.Count > 0;
        if(dsUsersAccess.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private static DataTable _Data;

    /// <summary>
    /// Get or set the static important data.
    /// </summary>
    public DataTable globalDataTable
    {
        get
        {
            return _Data;
        }
        set
        {
            _Data = value;
        }
    }

    #region <<< GLOBAL METHODS >>>

    //public string fnGetServerName
    //{
        //get
        //{
        //    //DS_ConfigTableAdapters.up_GetServerNameTableAdapter serverTA = new DS_ConfigTableAdapters.up_GetServerNameTableAdapter();
        //    //DS_Config.up_GetServerNameDataTable serverDT = new DS_Config.up_GetServerNameDataTable();

        //    serverDT = serverTA.GetData();

        //    return serverDT.Rows[0]["ServerName"].ToString();
        //}
    //}

    public bool fnCheckUser(string user, int PKCustomer)
    {
        bool aux = false;
        CLPalletLink.SC_Users Users = new CLPalletLink.SC_Users();
        DataSet dsUsers = new DataSet();

        try
        {
            dsUsers = Users.GetCheckUser(strSQLServer, strSQLDataBase, user, PKCustomer);

            if (dsUsers.Tables[0].Rows.Count > 0)
            {
                aux = true;
            }
        }
        catch (Exception ex)
        {
            aux = false;
            ERROR += "<br/>" + ex.Message.ToString();
        }
        return aux;
    }

    public int fnGetUserID(string user, int PKCustomer)
    {
        int userID = 0;
        CLPalletLink.SC_Users Users = new CLPalletLink.SC_Users();
        DataSet dsUsers = new DataSet();

        try
        {
            dsUsers = Users.GetUserID(strSQLServer, strSQLDataBase, user, PKCustomer);
            if (dsUsers.Tables[0].Rows.Count > 0)
                userID = Convert.ToInt32(dsUsers.Tables[0].Rows[0][0]);
        }
        catch (Exception ex)
        {
            ERROR += "<br/>" + ex.Message.ToString();
        }

        return userID;
    }

    //public int fnGetUserIDByEmployeeID(int employeeID)
    //{
    //    int userID = 0;
    //    DS_AdminTableAdapters.up_GetUserIDByEmployeeIDTableAdapter idUserTA;
    //    DS_Admin.up_GetUserIDByEmployeeIDDataTable idUserDT;

    //    try
    //    {
    //        idUserTA = new DS_AdminTableAdapters.up_GetUserIDByEmployeeIDTableAdapter();
    //        idUserDT = new DS_Admin.up_GetUserIDByEmployeeIDDataTable();

    //        idUserDT = idUserTA.GetData(employeeID);

    //        userID = Convert.ToInt32(idUserDT.Rows[0][0]);
    //    }
    //    catch (Exception ex)
    //    {
    //        ERROR += "<br/>" + ex.Message.ToString();
    //    }

    //    return userID;
    //}

    public int fnVerifyType(int userID, int PKCustomer)
    {
        int aux = 6;
        CLPalletLink.SC_Users Users = new CLPalletLink.SC_Users();
        DataSet dsUsers = new DataSet();

        try
        {

            dsUsers = Users.GetUserType(strSQLServer, strSQLDataBase, userID, PKCustomer);
            if (dsUsers.Tables[0].Rows.Count > 0)
            {
                aux = Convert.ToInt32(dsUsers.Tables[0].Rows[0][0]);
            }
        
        }
        catch (Exception ex)
        {
            aux = 6;
            ERROR += "<br/>" + ex.Message.ToString();
        }
        return aux;
    }

    public string fnRoleName(int userID, int PKCustomer)
    {
        string role = " ";
        CLPalletLink.SC_Users Users = new CLPalletLink.SC_Users();
        DataSet dsUsers = new DataSet();

        try
        {
            dsUsers = Users.GetUserType(strSQLServer, strSQLDataBase, userID, PKCustomer);


          

            if (dsUsers.Tables[0].Rows.Count > 0)
                role = dsUsers.Tables[0].Rows[0]["Role"].ToString();
        }
        catch (Exception ex)
        {
            ERROR += "<br/>" + ex.Message.ToString();
        }
        return role;
    }

    //public string fnGetEmail(int userID)
    //{
    //    string mail = "";
    //    DS_AdminTableAdapters.up_GetEmailUserTableAdapter emailTA;
    //    DS_Admin.up_GetEmailUserDataTable emailDT;

    //    try
    //    {
    //        emailTA = new DS_AdminTableAdapters.up_GetEmailUserTableAdapter();
    //        emailDT = new DS_Admin.up_GetEmailUserDataTable();

    //        emailDT = emailTA.GetData(userID);

    //        mail = emailDT.Rows[0][0].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        ERROR += "<br/>" + ex.Message.ToString();
    //    }

    //    return mail;
    //}

    public DataSet fnGetUserDetails(int userID)
    {
        CLPalletLink.SC_Users Users = new CLPalletLink.SC_Users();
        DataSet dsUsers = new DataSet();
        DataSet ds = null;
        
        try
        {
            dsUsers = Users.GetUserDetail(strSQLServer, strSQLDataBase, userID);



            if (dsUsers.Tables[0].Rows.Count > 0)
            {
                ds = dsUsers;
            }
            else
                ds = null;
        }
        catch (Exception ex)
        {
            ERROR = "Error to get user data information, " + ex.Message.ToString();
        }

        return ds;
    }

    //public DataTable fnShowSites(int regionID)
    //{
    //    DataTable dtSites = null;
    //    try
    //    {
    //        DS_AdminTableAdapters.up_GetSitesTableAdapter adapter = new DS_AdminTableAdapters.up_GetSitesTableAdapter();
    //        DS_Admin.up_GetSitesDataTable data = new DS_Admin.up_GetSitesDataTable();
    //        data = adapter.GetData(regionID);
    //        dtSites = data;
    //    }
    //    catch (Exception exception)
    //    {
    //        ERROR = "Error to show the sites in the list. " + exception.Message.ToString();
    //    }
    //    return dtSites;
    //}

    //public int fnGetRegion()
    //{
    //    DS_AdminTableAdapters.up_GetRegionsTableAdapter adapter = new DS_AdminTableAdapters.up_GetRegionsTableAdapter();
    //    DS_Admin.up_GetRegionsDataTable data = new DS_Admin.up_GetRegionsDataTable();

    //    int regionID = 0;
    //    try
    //    {
    //        data = adapter.GetData();
    //        if (data.Rows.Count > 0)
    //        {
    //            regionID = Convert.ToInt32(data.Rows[0]["PKRegionID"]);
    //        }
    //    }
    //    catch (Exception exception)
    //    {
    //        ERROR = "Error to get the region <br/>" + exception.Message.ToString();
    //    }

    //    return regionID;
    //}

    //public string fnGetUserDepartment(int userID)
    //{
    //    DS_AdminTableAdapters.up_GetUserDetailTableAdapter userTA;
    //    DS_Admin.up_GetUserDetailDataTable userDT;

    //    string depa = "0";

    //    try
    //    {
    //        userTA = new DS_AdminTableAdapters.up_GetUserDetailTableAdapter();
    //        userDT = new DS_Admin.up_GetUserDetailDataTable();

    //        userDT = userTA.GetData(userID);

    //        if (userDT.Rows.Count > 0)
    //        {
    //            depa = userDT.Rows[0]["FKCostCenterID"].ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ERROR = "Error to get user department. " + ex.Message.ToString();
    //    }

    //    return depa;
    //}

    #endregion <<< GLOBAL METHODS >>>
}