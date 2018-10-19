using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class SC_Users
    {
        public DataSet GetUserInfo(string strSQLServer, string strDataBase, string User)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetUserAvailable @windowsUser";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@windowsUser", DataAccessNet.Command.ParameterType.VarChar, User);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
        public DataSet UserHasAccess(string strSQLServer, string strDataBase, string pageName, int fkUser)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetUserAccessTo @FKUser,@URL";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FKUser", DataAccessNet.Command.ParameterType.VarChar, fkUser);
            objSQLCommand.AddParameter("@URL", DataAccessNet.Command.ParameterType.VarChar, pageName);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }

        public DataSet GetCheckUser(string strSQLServer, string strDataBase, string User, int PKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetCheckUser @WindowsID, @PKCustomer";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@WindowsID", DataAccessNet.Command.ParameterType.VarChar, User);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, PKCustomer);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
        public DataSet GetUserID(string strSQLServer, string strDataBase, string User, int PKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetUserID @WindowsID, @PKCustomer";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@WindowsID", DataAccessNet.Command.ParameterType.VarChar, User);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, PKCustomer);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
        public DataSet GetUserType(string strSQLServer, string strDataBase, int User, int PKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetUserType @userID, @PKCustomer";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@userID", DataAccessNet.Command.ParameterType.Int, User);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, PKCustomer);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
        public DataSet GetUserDetail(string strSQLServer, string strDataBase, int User)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetUserDetail @userID";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@userID", DataAccessNet.Command.ParameterType.Int, User);
           

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
        public DataSet GetRoles(string strSQLServer, string strDataBase)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetRoles";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            //objSQLCommand.AddParameter("@userID", DataAccessNet.Command.ParameterType.Int, User);


            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
        public DataSet GetUserPermissionDetail(string strSQLServer, string strDataBase, int FKCustomerID, int userID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetUserPermissionDetail @FKCustomerID, @userID";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FKCustomerID", DataAccessNet.Command.ParameterType.Int, FKCustomerID);
            objSQLCommand.AddParameter("@userID", DataAccessNet.Command.ParameterType.Int, userID);


            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
        public DataSet chgUserPermissionDetail(string strSQLServer, string strDataBase, int FKCustomerID, int FKUserID, int FKroleID, bool Available, int FKLastUpdatedUserID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_chgUserPermissionDetail @FKCustomerID, @FKUserID,@FKroleID,@Available,@FKLastUpdatedUserID,@UserReadOnly";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FKCustomerID", DataAccessNet.Command.ParameterType.Int, FKCustomerID);
            objSQLCommand.AddParameter("@FKUserID", DataAccessNet.Command.ParameterType.Int, FKUserID);
            objSQLCommand.AddParameter("@FKroleID", DataAccessNet.Command.ParameterType.Int, FKroleID);
            objSQLCommand.AddParameter("@Available", DataAccessNet.Command.ParameterType.Bit, Available);
            objSQLCommand.AddParameter("@FKLastUpdatedUserID", DataAccessNet.Command.ParameterType.Int, @FKLastUpdatedUserID);
         

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;


        }
      
        public DataSet RegisterUser(string strSQLServer, string strDataBase, int EmployeeNumber, string User, string Name, string LastName, string email)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "INSERT INTO [dbo].[SC_Users]"+
                           "([EmployeeNumber]"+
                           ",[WindowsUser]"+
                           ",[FirstName]"+
                           ",[LastName]"+
                           ",[Email])" +
                    " VALUES"+
                           "(@EmployeeNumber"+
                           ",@User"+
                           ",@Name" +
                           ",@LastName" +
                           ",@email)";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@EmployeeNumber", DataAccessNet.Command.ParameterType.Int, EmployeeNumber);
            objSQLCommand.AddParameter("@User", DataAccessNet.Command.ParameterType.VarChar, User);
            objSQLCommand.AddParameter("@Name", DataAccessNet.Command.ParameterType.VarChar, Name);
            objSQLCommand.AddParameter("@LastName", DataAccessNet.Command.ParameterType.VarChar, LastName);
            objSQLCommand.AddParameter("@email", DataAccessNet.Command.ParameterType.VarChar, email);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet RegisterUserCustomer(string strSQLServer, string strDataBase, int PKUser, int PKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "INSERT INTO [dbo].[SC_UserCustomer]" +
                               "([FKUser]" +
                               ",[FKCustomer])" +
                        " VALUES" +
                               "(@PKUser" +
                               ",@PKCustomer)";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKUser", DataAccessNet.Command.ParameterType.Int, PKUser);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, PKCustomer);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet GetUserCustomers(string strSQLServer, string strDataBase, int PKUser)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [PKUserCustomer]" +
                ",[FKUser]" +
                ",[FKCustomer]" +
                ",UPPER(Customer)"+
                " FROM [ValeoApps].[dbo].[SC_UserCustomer] UC (NOLOCK)" +
                " INNER JOIN CT_Customers C ON UC.FKCustomer = C.PKCustomer"+
                " WHERE [FKUser] = @PKUser";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKUser", DataAccessNet.Command.ParameterType.Int, PKUser);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet GetUserCustomer(string strSQLServer, string strDataBase, int PKUser, int PKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [PKUserCustomer]" +
                ",[FKUser]" +
                ",[FKCustomer]" +
                "FROM [ValeoApps].[dbo].[SC_UserCustomer] (NOLOCK)" +
                "WHERE [FKUser] = @PKUser AND FKCustomer = @PKCUstomer";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKUser", DataAccessNet.Command.ParameterType.Int, PKUser);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, PKCustomer);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet GetUserCustomer(string strSQLServer, string strDataBase, string User, int PKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [PKUserCustomer]" +
                ",[FKUser]" +
                ",[FKCustomer]" +
                " FROM [ValeoApps].[dbo].[SC_UserCustomer] UC (NOLOCK)" +
                " INNER JOIN SC_Users U ON U.PKUser = UC.FKUser " +
                " WHERE U.[WindowsUser] = @User AND FKCustomer = @PKCUstomer";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@User", DataAccessNet.Command.ParameterType.NChar, User);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, PKCustomer);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet UpdateUser(string strSQLServer, string strDataBase, int EmployeeNumber, string User, string Name, string LastName, string email)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "UPDATE [dbo].[SC_Users]"+
                           "SET [EmployeeNumber] = @EmployeeNumber" +
                              ",[WindowsUser] = @User" +
                              ",[FirstName] = @Name" +
                              ",[LastName] = @LastName" +
                              ",[Email] = @email" +
                              ",[LastUpdated] = GETDATE()"+
                         "WHERE WindowsUser = @User";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@EmployeeNumber", DataAccessNet.Command.ParameterType.Int, EmployeeNumber);
            objSQLCommand.AddParameter("@User", DataAccessNet.Command.ParameterType.VarChar, User);
            objSQLCommand.AddParameter("@Name", DataAccessNet.Command.ParameterType.VarChar, Name);
            objSQLCommand.AddParameter("@LastName", DataAccessNet.Command.ParameterType.VarChar, LastName);
            objSQLCommand.AddParameter("@email", DataAccessNet.Command.ParameterType.VarChar, email);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet CheckIfHaveAccess(string strSQLServer, string strDataBase, int PKUser)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [PKUserApplication]"+
                          ",[FKUser]"+
                          ",[FKApplication]"+
                          ",[HasAccess]"+
                          ",[FKUserUpdater]"+
                          ",[LastUpdated]"+
                          ",[Available]"+
                      " FROM [ValeoApps].[dbo].[SC_UserApplication]"+
                      " WHERE FKUser = @PKUser AND FKApplication = 5";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKUser", DataAccessNet.Command.ParameterType.Int, PKUser);

            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }
    
    }
}
