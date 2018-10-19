using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;


public partial class PalletLink_aspx_Linking : PL_Parent
{

    private PL_Common c = new PL_Common();

    protected CLPalletLink.SY_PalletLink wpLog = new CLPalletLink.SY_PalletLink();

    protected void Page_Load(object sender, EventArgs e)
    {


        lblMessage.Visible = false;
        lblMessage1.Visible = false;
        if (!IsPostBack)
        {
            fnGetConfig();
          
        }

    }
    private void fillgvFixtureData()
    {
        CLPalletLink.CT_Fixtures ctFixtures = new CLPalletLink.CT_Fixtures();
        DataSet dsStatusPallet = new DataSet();
        dsStatusPallet = ctFixtures.StatusPallet(strSQLServer, strSQLDataBasePallet, txtSerial.Text);
        gvFixtureData.DataSource = dsStatusPallet;
        gvFixtureData.DataBind();


    }
    private Boolean ValidateWashingAndCycles(DataSet dsPallet)
    {
        General GN = new General();

        CLPalletLink.CT_Fixtures ctFixtures = new CLPalletLink.CT_Fixtures();
        DataSet dsCycles = new DataSet();
        DataSet dsStatusPallet = new DataSet();
        bool Valid = false;
        Boolean CorrectStatus = false;
        int PalletWashed = 4;
        int iStatusPallet = 0;

        int iCyclesNumber = 0;
        int iAccumulatedCycles = 0;
        int iLimitCycles = 0;
        int iMaintenanceCycles = 0;
        int iMaintenanceCyclesLimit = 0;


        dsCycles = ctFixtures.PalletCycles(strSQLServer, strSQLDataBasePallet, txtSerial.Text);
        if (dsCycles.Tables[0].Rows.Count > 0)
        {
            if (DBNull.Value.Equals(dsCycles.Tables[0].Rows[0]["AccumulatedCycles"]))
            {
                iAccumulatedCycles = 0;
            }
            else
            {
                iAccumulatedCycles = Convert.ToInt32(dsCycles.Tables[0].Rows[0]["AccumulatedCycles"]);
            }

            if (DBNull.Value.Equals(dsCycles.Tables[0].Rows[0]["NumberOfCycles"]))
            {
                iCyclesNumber = 0;
            }
            else
            {
                iCyclesNumber = Convert.ToInt32(dsCycles.Tables[0].Rows[0]["NumberOfCycles"]);
            }
            iLimitCycles = Convert.ToInt32(dsCycles.Tables[0].Rows[0]["CycleLimit"]);
            iMaintenanceCycles = Convert.ToInt32(dsCycles.Tables[0].Rows[0]["MaintenanceCycles"]);
            iMaintenanceCyclesLimit = Convert.ToInt32(dsCycles.Tables[0].Rows[0]["MaintenanceCyclesLimit"]);
        }
        else
        {
            iAccumulatedCycles = 0;
            iCyclesNumber = 0;
            iMaintenanceCycles = 0;
            iMaintenanceCyclesLimit = 500;
            iLimitCycles = 500;

        }

        dsStatusPallet = ctFixtures.StatusPallet(strSQLServer, strSQLDataBasePallet, txtSerial.Text);
        if (dsStatusPallet.Tables[0].Rows.Count > 0)
        {
            iStatusPallet = Convert.ToInt32(dsStatusPallet.Tables[0].Rows[0]["FKPalletStatus"]);
            if (iStatusPallet.Equals(PalletWashed))
            {
                CorrectStatus = true;
            }

        }
        if ((iCyclesNumber <= iLimitCycles) & (CorrectStatus == true) & (iMaintenanceCycles <= iMaintenanceCyclesLimit))
        {
            //iCyclesNumber = iCyclesNumber + 1;
            //iMaintenanceCycles = iMaintenanceCycles + 1;
            //iAccumulatedCycles = iAccumulatedCycles + 1;

            GN.SetCookie("iCyclesNumberPL", iCyclesNumber.ToString());
            GN.SetCookie("iMaintenanceCyclesPL", iMaintenanceCycles.ToString());
            GN.SetCookie("iAccumulatedCyclesPL", iMaintenanceCycles.ToString());
            GN.SetCookie("iLimitCyclesPL", iLimitCycles.ToString());
            GN.SetCookie("iMaintenanceCyclesLimitPL", iMaintenanceCyclesLimit.ToString());

            //ctFixtures.UpdateCounter(strSQLServer, strSQLDataBasePallet, txtSerial.Text, iCyclesNumber, iAccumulatedCycles, iLimitCycles, iMaintenanceCycles, iMaintenanceCyclesLimit, DateTime.Now.ToLocalTime(), 0);
            Valid = true;

        }
        return Valid;
    }
    private string[] ValidateWashingAndCyclesTooling(DataSet dsPallet)
    {
        // gvFixtureData Columns
        // 0  PKToolingID
        // 1  GRN
        // 2  FKStatusID
        // 3  Description
        // 4  SerialNumber
        // 5  Quantity
        // 6  CurrentWash
        // 7  LimitWash
        // 8  CurrentMain
        // 9  LimitMain
        // 10 LastDateWash
        string palletStatus = gvFixtureData.Rows[0].Cells[3].Text;
        int currentWash = Int32.Parse(gvFixtureData.Rows[0].Cells[6].Text);
        int limitWash = Int32.Parse(gvFixtureData.Rows[0].Cells[7].Text);
        int currentMain = Int32.Parse(gvFixtureData.Rows[0].Cells[8].Text);
        int limitMain = Int32.Parse(gvFixtureData.Rows[0].Cells[9].Text);


        string[] retunValues = new string[3];


        // Pallet Status        
        if (palletStatus == "In Use")
        {
            retunValues[0] = "Pallet " + palletStatus;
        }
        else
        {
            retunValues[0] = " El estatus del pallet \"" + palletStatus + "\" no es valido. ";
        }

        // Pallet Washing
        if (currentWash <= limitWash)
        {
            retunValues[1] = "Washing OK";

        }
        else
        {
            retunValues[1] = " El pallet excedió los " + limitWash + " ciclos de lavado. ";
        }


        if (currentMain <= limitMain)
        {
            retunValues[2] = "Maintenance OK";

        }
        else
        {
            retunValues[2] = " El pallet excedió los " + limitMain + " ciclos de mantenimiento. ";
        }



        return retunValues;
    }
    protected void txtGRN_TextChanged(object sender, EventArgs e)
    {
   
        CLPalletLink.CT_Fixtures ctFixtures = new CLPalletLink.CT_Fixtures();
        DataSet ds = new DataSet();
        DataSet dsStatusPallet = new DataSet();
        lblMessage0.Text = string.Empty;
        ImgPalomitaOK.Style.Add("display", "none");
        lblMessage2.Visible = false;
        //Unificar formato de scaner
        string sPalletID = txtSerial.Text;
        txtSerial.Text = sPalletID.Replace("'", "-");
        try
        {
            /************************************************************************************************************************/
            /* Revisar si el Pallet existe en Tooling Managmente */
            /************************************************************************************************************************/
            // ds = ctFixtures.checkIfPalletExists(strSQLServer, strSQLDataBase, txtSerial.Text.Trim());

            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[up_GetPalletStatusAndCounters] " +
                            "@GRN = '" + txtSerial.Text.Trim() + "', " +
                            "@CustomerID = '" + Hidden_CustomerPLID.Value + "' ";

            try
            {
                ds = SQL.dsSQLQuery(strSQLServer, strSQLDataBase, strSQL);
            }
            catch (Exception e3)
            {
                Console.WriteLine(e3.Message);
                ds = null;
            }
    
            /************************************************************************************************************************/

            if (ds.Tables[0].Rows.Count > 0)
            {
                
                gvFixtureData.DataSource = ds;
                gvFixtureData.DataBind();
           
                HiddenPalletID.Value = ds.Tables[0].Rows[0].ItemArray[1].ToString();


                /*Revisar si timeout del Pallet */
                bool validTimeOut = false;
                validTimeOut = fnPalletTimeOut(sPalletID);

                if (validTimeOut)
                {

                    /*Revisar los ciclos de lavado */
                    string[] palletValidation = ValidateWashingAndCyclesTooling(ds);


                    if ((palletValidation[0] == "Pallet In Use") &&
                        (palletValidation[1] == "Washing OK") &&
                        (palletValidation[2] == "Maintenance OK"))
                    {
                        pnlPiezas.Visible = true;
						pnlPiezas.Enabled = true;
                        pnlUnitsInfo.Visible = true;
                        txtSerial.ReadOnly = true;
                        txtSerial.Font.Bold = true;
                        txtSerial.BackColor = System.Drawing.Color.LightGray;
                        txtUnit.Focus();
                    }
                    else
                    {
                        lblMessage.Text = "";

                        if (palletValidation[0] != "Pallet In Use")
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text += "&#8226; " + palletValidation[0] + " </br>";
                            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", txtSerial.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);
                        }
                        if (palletValidation[1] != "Washing OK")
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text += "&#8226; " + palletValidation[1] + " </br>";
                            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", txtSerial.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);
                        }
                        if (palletValidation[2] != "Maintenance OK")
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text += "&#8226; " + palletValidation[2] + " </br>";
                            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", txtSerial.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);
                        }

                        lblMessage.Text += "Favor de llevarlo al cuarto de pallets. </br>";
                    }

                }

                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "&#8226; El pallet \"" + txtSerial.Text.ToUpper().Trim() + "\" ya esta en uso.";  // agregar detalle de la hora
                    fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", txtSerial.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);

                    return;
                }

                
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "</br> El Pallet no ha sido dado de alta en Tooling Management.  </br> O el Pallet corresponde a otro cliente. </br>  Favor de contactar el personal de Ingenieria de Manufactura.";
                fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(),  Hidden_Customer.Value.ToString(), "", txtSerial.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);

                return;

            }
        }
        catch (Exception e2)
        {
            lblMessage.Visible = true;
            lblMessage.Text = e2.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", txtSerial.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);

        }

    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        clear();

    }
    private int CheckBoardCounterValidation(string SerialNumber, int iWip_ID, int iLoopsAllowed, int CustomerId, string Assembly)
    {

        int iProcess_ID = 0;
        int iHoldType_ID = 60;
        int iHoldBy_ID = 1;
        string sHoldEMO = "Pieza enviada a Hold por exceder el número de loops";
        int iBoardStatus = 0;
        int iBoardLoops = 0;
        wsMes.Service Mes = new wsMes.Service();
        /*************************************************************************/
        wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
        DataSet dsLoopsCounter = new DataSet();

        string strSQL = "EXEC up_GetSerialLoopsCounterOtro " +
                        "@SerialNumber = '" + SerialNumber + "', "+
                        "@Assembly = '" + Assembly + "' ";

        try
        {
            dsLoopsCounter = SQL.dsSQLQuery(strSQLServer, strSQLDataBase, strSQL);
        }
        catch (Exception e3)
        {
            Console.WriteLine(e3.Message);
            dsLoopsCounter = null;
        }
        /*************************************************************************/


        if (dsLoopsCounter.Tables[0].Rows.Count > 0)
        {
            iBoardLoops = Convert.ToInt32(dsLoopsCounter.Tables[0].Rows[0]["LoopsNumber"]);
        }


        if (iBoardLoops >= iLoopsAllowed)
        {

            if (CustomerId == 148)
            {
                iBoardStatus = 1;


            }
            else
            {
                iBoardStatus = Mes.PlaceBoardOnHold(iWip_ID, iProcess_ID, iHoldType_ID, iHoldBy_ID, sHoldEMO);


            }

        }

        return iBoardStatus;
    }
    private Boolean VerifyIfBoardOnHold(int iWipId)
    {
        Boolean Result = false;
        wsMes.Service Mes = new wsMes.Service();
        Result = Mes.BoardInHold(iWipId);
        return Result;
    }
    protected DataSet VerifiyCheckPoint(string SerialNumber, string ChkProcess, int CustomerID, int AssemblyID)
    {
        System.Data.DataSet dsResultCheckpoint = new System.Data.DataSet();
        wsMes.Service Mes = new wsMes.Service();
        dsResultCheckpoint = Mes.GetCheckPoint(SerialNumber, "CHKLNKPALLET", CustomerID, AssemblyID);

        return dsResultCheckpoint;

    }
    protected Boolean IsSerialScanned(string serialNumber)
    {
        Boolean isScanned = false;

        if (gvUnitsInfo.Rows.Count > 0)
        {

            for (int r = 0; r < gvUnitsInfo.Rows.Count; r++)
            {
                for (int c = 7; c < gvUnitsInfo.Rows[r].Cells.Count; c++)
                {

                    if (serialNumber == gvUnitsInfo.Rows[r].Cells[c].Text)
                    {
                        isScanned = true;

                    }

                }

            }

        }
        else
        {
            isScanned = false;

        }


        return isScanned;

    }
    protected int SerialNumberValidation(string SerialNumber)
    {
        wsMes.Service Mes = new wsMes.Service();
        CLPalletLink.CT_Assemblies ctAssy = new CLPalletLink.CT_Assemblies();
        CLPalletLink.WP_Panels wpPanel = new CLPalletLink.WP_Panels();
        CLPalletLink.SY_PalletLink wpLog = new CLPalletLink.SY_PalletLink();
        DataSet dsMESAssembly = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet dsLocalAssembly = new DataSet();
        HiddenBoardAssembly.Value = string.Empty;
        int iWipId = 0;
        int CustomerId = 0;
        Boolean CheckPointVerification = false;
        Boolean CheckLoops = false;
        int LoopsNumber = 0;
        int PalletPieces = 0;
        Boolean ValidaPanel = false;

        try
        {
            /*Obtener el Ensamble de MES con el numero de serie introducido*/
            dsMESAssembly = ctAssy.GetAssemblyBySNTHLevel(strSQLServer, strSQLDataBase, Hidden_Customer.Value.ToString(), SerialNumber);
            //dsMESAssembly = Mes.SelectBySerialNumber(SerialNumber);
            if (dsMESAssembly.Tables[0].Rows.Count == 0)
            {

                if (Hidden_Customer.Value.ToString() == "Tesla")
                {

                    DataSet dsCheckPoint = new DataSet();
                    dsCheckPoint = VerifiyCheckPoint(SerialNumber, "0", 148, Convert.ToInt32(HiddenAssembly.Value.ToString()));
                    if (dsCheckPoint.Tables[0].Rows.Count > 0)
                    {
                        string message = string.Empty;
                        for (int i = 0; i < dsCheckPoint.Tables[0].Rows.Count; i++)
                        {

                            message = message + dsCheckPoint.Tables[0].Rows[i]["DescrText"].ToString() + ", ";
                        }

                        lblMessage0.Visible = true;
                        lblMessage0.Text = "A la pieza le faltan los siguientes procesos requeridos en MES: "
                                            + message + " " + "Favor de contactar al departamento de CALIDAD para continuar";
                        fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, txtSerial.Text, HiddenAssembly.Value.ToString(), "", "",
                         "", "", "", "", "", "", "", "", "", "", lblMessage0.Text);

                        return PalletPieces;

                    }
                }
                lblMessage0.Visible = true;
                lblMessage0.Text = "La tablilla no se encuentra en la progresión correcta, favor de verificar en el historial de la pieza en MES";
                fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, txtSerial.Text, "", "", "",
                       "", "", "", "", "", "", "", "", "", "", lblMessage0.Text);
                return PalletPieces;

            }
            else
            {

                HiddenAssembly.Value = (dsMESAssembly.Tables[0].Rows[0]["Assembly_ID"].ToString());
                HiddenBoardAssembly.Value = dsMESAssembly.Tables[0].Rows[0]["NUMBER"].ToString();
                iWipId = Convert.ToInt32(dsMESAssembly.Tables[0].Rows[0]["WIP_ID"].ToString());
                CustomerId = Convert.ToInt32(dsMESAssembly.Tables[0].Rows[0]["Customer_ID"].ToString());


            }
            /*Revisar que exista en la BD local, es decir que se haya hecho la configuracion necesaria*/

            dsLocalAssembly = ctAssy.checkIfAssyLocallyExistsUpd(strSQLServer, strSQLDataBase, HiddenBoardAssembly.Value.ToString(), Int32.Parse(HiddenIdCustomer.Value.ToString()), HiddenPalletID.Value);
            if (dsLocalAssembly.Tables[0].Rows.Count > 0)
            {
                // version 2.0


                CheckPointVerification = Convert.ToBoolean(dsLocalAssembly.Tables[0].Rows[0]["CheckPointValidation"].ToString());
                CheckLoops = Convert.ToBoolean(dsLocalAssembly.Tables[0].Rows[0]["LoopValidation"].ToString());
                LoopsNumber = Convert.ToInt32(dsLocalAssembly.Tables[0].Rows[0]["LoopsAllowed"].ToString());
                ValidaPanel = Convert.ToBoolean(dsLocalAssembly.Tables[0].Rows[0]["Panel"].ToString());
                PalletPieces = Convert.ToInt32(dsLocalAssembly.Tables[0].Rows[0]["Pieces"].ToString());



            }
            else
            {
                lblMessage0.Visible = true;
                lblMessage0.Text = "No se encontró información del ensamble de la tablilla en PALLET LINK, favor de verificar";
                fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                            HiddenAssembly.Value, HiddenBoardAssembly.Value, iWipId.ToString(), PalletPieces.ToString(),
                           "", "", "", "", "", "", "", LoopsNumber.ToString(), LoopsNumber.ToString(), lblMessage0.Text + " Valida Panel " + ValidaPanel.ToString());
                PalletPieces = 0;
                return PalletPieces;
            }

            /*Verifica si la pieza está en Hold*/
            if (VerifyIfBoardOnHold(iWipId))
            {
                lblMessage0.Visible = true;
                lblMessage0.Text = "Tablilla en HOLD favor de contactar al departamento de Calidad para liberarla";
                fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                           HiddenAssembly.Value, HiddenBoardAssembly.Value, iWipId.ToString(), PalletPieces.ToString(),
                           "", "", "", "", "", "", "", LoopsNumber.ToString(), LoopsNumber.ToString(), lblMessage0.Text + " Valida Panel " + ValidaPanel.ToString());
                PalletPieces = 0;
                return PalletPieces;
            }
            /* Verificar checkpoint */

            if (CheckPointVerification)
            {

                DataSet dsCheckPoint = new DataSet();
                dsCheckPoint = VerifiyCheckPoint(SerialNumber, "0", CustomerId, Convert.ToInt32(HiddenAssembly.Value.ToString()));
                if (dsCheckPoint.Tables[0].Rows.Count > 0)
                {
                    string message = string.Empty;
                    for (int i = 0; i < dsCheckPoint.Tables[0].Rows.Count; i++)
                    {

                        message = message + dsCheckPoint.Tables[0].Rows[i]["DescrText"].ToString() + ", ";
                    }

                    lblMessage0.Visible = true;
                    lblMessage0.Text = "A la pieza le faltan los siguientes procesos requeridos en MES: "
                                        + message + " " + "Favor de contactar al departamento de CALIDAD para continuar";
                    fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                            HiddenAssembly.Value, HiddenBoardAssembly.Value, iWipId.ToString(), PalletPieces.ToString(),
                            "", "", "", "", "", "", "", LoopsNumber.ToString(), LoopsNumber.ToString(), lblMessage0.Text);
                    PalletPieces = 0;
                    return PalletPieces;

                }
            }

            if (CheckLoops)
            {
                int BoardLoops = CheckBoardCounterValidation(SerialNumber, iWipId, LoopsNumber, CustomerId, HiddenBoardAssembly.Value.ToString());
                if (BoardLoops > 0)
                {
                    if (CustomerId == 148)
                    {

                        lblMessage0.Visible = true;
                        lblMessage0.Text = " Error en ligado. La tablilla ha EXCEDIDO el número de ligados permitidos";
                        fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                       HiddenAssembly.Value, HiddenBoardAssembly.Value, iWipId.ToString(), PalletPieces.ToString(),
                       "", "", "", "", "", "", "", BoardLoops.ToString(), LoopsNumber.ToString(), lblMessage0.Text);

                    }
                    else
                    {

                        lblMessage0.Visible = true;
                        lblMessage0.Text = "Status de la tablilla ONHOLD por EXCEDER el número de ligados permitidos, favor de contactar al encargado de CALIDAD para liberarla";
                        fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                       HiddenAssembly.Value, HiddenBoardAssembly.Value, iWipId.ToString(), PalletPieces.ToString(),
                       "", "", "", "", "", "", "", BoardLoops.ToString(), LoopsNumber.ToString(), lblMessage0.Text);


                    }
                    PalletPieces = 0;
                    return PalletPieces;
                }
            }

        }
        catch (Exception ex)
        {
            lblMessage0.Visible = true;
            lblMessage0.Text = ex.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                           HiddenAssembly.Value, HiddenBoardAssembly.Value, iWipId.ToString(), PalletPieces.ToString(),
                           "", "", "", "", "", "", "", LoopsNumber.ToString(), LoopsNumber.ToString(), lblMessage0.Text);
            PalletPieces = 0;
            return PalletPieces;

        }
        return PalletPieces;

    }
    protected void btnClean_Click(object sender, EventArgs e)
    {
        clearAll();

        txtSerial.Focus();
    }
    private void clearAll()
    {
        pnlPiezas.Visible = false;
        pnlPiezas.Enabled = false;

        txtSerial.Text = "";
        txtSerial.Font.Bold = false;
        txtSerial.BackColor = System.Drawing.Color.White;
        txtSerial.ReadOnly = false;

        gvFixtureData.DataSource = null;
        gvFixtureData.DataBind();

        txtUnit.Text = "";
        txtUnit.Font.Bold = false;
        txtUnit.BackColor = System.Drawing.Color.White;
        txtUnit.ReadOnly = false;
        gvUnitsInfo.DataSource = null;
        gvUnitsInfo.DataBind();

        lblMessage0.Visible = false;
        btnOK.Visible = false;
		
		
    }
    protected void IncreaseLoopCounter(string SerialNumber, string Assembly)
    {
        CLPalletLink.SY_BoardLoopsCounter syBoardLoopsCounter = new CLPalletLink.SY_BoardLoopsCounter();
        DataSet dsGetLoops = new DataSet();
        DataSet dsUpdLoops = new DataSet();
        DataSet ds = new DataSet();
        int PKBoardLoopCounter = 0;
        int LoopsNumber = 0;

        dsGetLoops = syBoardLoopsCounter.GetBoardLoopsNumber(strSQLServer, strSQLDataBase, SerialNumber, Assembly);
        if (dsGetLoops.Tables[0].Rows.Count > 0)
        {
            PKBoardLoopCounter = Convert.ToInt32(dsGetLoops.Tables[0].Rows[0]["PKBoardLoopCounter"]);
            LoopsNumber = Convert.ToInt32(dsGetLoops.Tables[0].Rows[0]["LoopsNumber"]);
            LoopsNumber = LoopsNumber + 1;
            /*Actualiza loops*/
            dsUpdLoops = syBoardLoopsCounter.UpdateBoardLoopsCounter(strSQLServer, strSQLDataBase, PKBoardLoopCounter, LoopsNumber, "");
        }
        else
        {
            /*Inserta fila en la tabla para contabilizar loops*/
            LoopsNumber = 1;
            ds = syBoardLoopsCounter.InsertBoardLoopsCounter(strSQLServer, strSQLDataBase, SerialNumber, LoopsNumber, "", Assembly);
        }

    }
    private void RegisterLinkInBD(string serialNumber, int LinkMaterialID, string EquipmentName, string Assembly)
    {
        try
        {
            CLPalletLink.CR_RouteSteps crRS = new CLPalletLink.CR_RouteSteps();
            CLPalletLink.SY_PalletLink ctLink = new CLPalletLink.SY_PalletLink();
            CLPalletLink.CR_Equipments crEquiment = new CLPalletLink.CR_Equipments();
            CLPalletLink.CT_Assemblies ctAssy = new CLPalletLink.CT_Assemblies();
            DataSet ds = new DataSet();
            string Equipment = null;
            wsMes.Service Mes = new wsMes.Service();
            string Server = strSQLServer;
            string DataBase = strSQLDataBase;
            ds = crRS.GetMESRouteStepPalletPos(strSQLServer, strSQLDataBase, Hidden_Customer.Value.ToString(), Int32.Parse(HiddenMARouteID.Value.ToString()));
            int RouteStepID = Int32.Parse(ds.Tables[0].Rows[0].ItemArray[5].ToString());
            /*Obtener el WIPID del serial en MES*/

            ds = Mes.SelectBySerialNumber(serialNumber);
            //ds = ctAssy.GetWIPID(strSQLServer, strSQLDataBase, TextBox1.Text);
            int WipID = Int32.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());

            /*Obtener el equipo y el ID del mismo en MES*/
            Equipment = EquipmentName;
            ds = crEquiment.GetEquipmentID(strSQLServer, strSQLDataBase, Equipment);
            int EquipmentID = Int32.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());

            /*Registrar la prueba de PALLETPOSITION como pasada en MES*/
            ctAssy.UpdateProcessStatusOnMES(strSQLServer, strSQLDataBase, WipID, RouteStepID, EquipmentID, "1");
            /*Registrar el ligado en MES*/
            ctAssy.LinkNonUniqueComponent(Server, DataBase, WipID, txtSerial.Text, LinkMaterialID, "1",
                                        Int32.Parse(HiddenRouteStepID.Value.ToString()),
                                        Int32.Parse(HiddenEquipmentID.Value.ToString()));

            /*Registrar el ligado en ValeoApps*/
            ctLink.InsertPalletLink(Server, DataBase, serialNumber, txtSerial.Text, "PalletLink",
                                    (HiddenUser.Value.ToString()), 1);
            /* Actualizar el contador de loops */
            IncreaseLoopCounter(serialNumber, Assembly);
        }

        catch (Exception Register)
        {
            lblMessage0.Visible = true;
            lblMessage0.Text = Register.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(),
                serialNumber, HiddenPalletID.Value,
               HiddenAssembly.Value, HiddenBoardAssembly.Value, "", "", "", "", "", "", HiddenEquipmentID.Value, HiddenEquipment.Value, HiddenRouteStepID.Value, "", "", lblMessage0.Text);

        }

    }
    protected void btnLogout_Click1(object sender, EventArgs e)
    {
        clearAll();
        clear();
        Response.Redirect("login.aspx");
    }
    protected void gvFixtureData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int CiclosMtto = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "CurrentMain"));
            int LimitesMtto = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "LimitMain"));
            float porcentaje = (CiclosMtto / LimitesMtto) * 100;
            System.Web.UI.WebControls.Image Semaforo = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgMtto");




            if (porcentaje <= 80)
            {
                Semaforo.ImageUrl = "~/Pallet_Link/Images/Verde.JPG";

            }
            else if ((porcentaje >= 81) && (porcentaje <= 92))
            {
                Semaforo.ImageUrl = "~/Pallet_Link/Images/Amarillo.JPG";
            }
            else if ((porcentaje >= 93) && (porcentaje <= 99))
            {
                Semaforo.ImageUrl = "~/Pallet_Link/Images/Naranja.JPG";
            }
            else
            {
                Semaforo.ImageUrl = "~/Pallet_Link/Images/Rojo.JPG";
            }


        }
    }
    private void fnFillgvEscalation()
    {
        CLPalletLink.CT_LinkPath ct_LinkPath = new CLPalletLink.CT_LinkPath();
        DataSet ds = new DataSet();

        try
        {
            ds = ct_LinkPath.GetLinkPath(strSQLServer, strSQLDataBase, Hidden_Customer.Value, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEscalation.DataSource = ds;
                gvEscalation.DataBind();
            }
        }
        catch (Exception e2)
        {
            lblMessage1.Visible = true;
            lblMessage1.Text = e2.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage1.Text);

        }

    }
     private void fnUpdateCounter()
    {
        // gvFixtureData Columns
        // 0  PKToolingID
        // 1  GRN
        // 2  FKStatusID
        // 3  Description
        // 4  SerialNumber
        // 5  Quantity
        // 6  CurrentWash
        // 7  LimitWash
        // 8  CurrentMain
        // 9  LimitMain
        // 10 LastDateWash
        int palletID = Int32.Parse(gvFixtureData.Rows[0].Cells[0].Text);
        int newWash = Int32.Parse(gvFixtureData.Rows[0].Cells[6].Text) + 1;
        int limitWash = Int32.Parse(gvFixtureData.Rows[0].Cells[7].Text);
        int newMain = Int32.Parse(gvFixtureData.Rows[0].Cells[8].Text) + 1;
        int limitMain = Int32.Parse(gvFixtureData.Rows[0].Cells[9].Text);

        int status = 10; // Status "In Use"

        if ((newWash >= limitWash) || (newMain >= limitMain))
        {
            status = 11; // Status "Burned"
        }

        /************************************************************************************************************************/
      
        wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();

         string strSQL = strSQL = "EXEC up_ChgToolingCountersPalletLink " +
                                 "@ToolingID = '" + palletID + "', " +
                                 "@newWash = '" + newWash + "', " +
                                 "@newMain = '" + newMain + "', " +
                                 "@FKStatusID = '" + status + "'";


        try
        {
            SQL.dsSQLQuery(strSQLServer, strSQLDataBase, strSQL);
        }
        catch (Exception e4)
        {
            Console.WriteLine(e4.Message);
           
        }

        /************************************************************************************************************************/

        
     
  
    }
  
   private bool fnPalletTimeOut(string PalletName)
    {
        CLPalletLink.CT_PalletTimeOutByCustomer ct_PalletTimeOutByCustomer = new CLPalletLink.CT_PalletTimeOutByCustomer();
        DataSet ds = new DataSet();
        bool isValidPallet = false;

        try
        {
            string customer = Hidden_Customer.Value.ToString();

            ds = ct_PalletTimeOutByCustomer.GetPalletTimeOut(strSQLServerTE, strSQLDatabaseTE, Hidden_CustomerPLID.Value.ToString(), true, PalletName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime palletAvailable = DateTime.Parse(ds.Tables[0].Rows[0]["AvailableDatetime"].ToString());

                if (palletAvailable < DateTime.Now)
                    isValidPallet = true;

            }
            else
            {
                //  ct_PalletTimeOutByCustomer.InsertPalletTimeOut(strSQLServer, strSQLDataBase, PalletName, HiddenIdCustomer.Value.ToString(), HiddenPalletTimeout.Value.ToString());
                isValidPallet = true;
            }


            return isValidPallet;

        }
        catch (Exception e2)
        {
            lblMessage.Visible = true;
            lblMessage.Text = e2.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);
            return false;
        }

    }
    private void fnGetPalletTimeout()
    {
        CLPalletLink.CT_PalletTimeOutByCustomer ct_PalletTimeOutByCustomer = new CLPalletLink.CT_PalletTimeOutByCustomer();
        DataSet ds = new DataSet();

        try
        {


            ds = ct_PalletTimeOutByCustomer.GetCustomerTimeOut(strSQLServerTE, strSQLDatabaseTE, Hidden_CustomerPLID.Value.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                HiddenPalletTimeout.Value = ds.Tables[0].Rows[0]["TimeOut"].ToString();

            }
            else
            {
                HiddenPalletTimeout.Value = "120";
            }

        }
        catch (Exception e2)
        {
            lblMessage.Visible = true;
            lblMessage.Text = e2.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);

        }


    }
    private void fnUpdatePalletTimeout()
    {
        CLPalletLink.CT_PalletTimeOutByCustomer ct_PalletTimeOutByCustomer = new CLPalletLink.CT_PalletTimeOutByCustomer();
        DataSet ds = new DataSet();

        try
        {

            ds = ct_PalletTimeOutByCustomer.GetPalletTimeOut(strSQLServerTE, strSQLDatabaseTE, Hidden_CustomerPLID.Value.ToString(), true, txtSerial.Text.Trim());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ct_PalletTimeOutByCustomer.UpdatePalletTimeOut(strSQLServerTE, strSQLDatabaseTE, txtSerial.Text.Trim(), Hidden_CustomerPLID.Value.ToString(), HiddenPalletTimeout.Value.ToString());

            }
            else
            {
                ct_PalletTimeOutByCustomer.InsertPalletTimeOut(strSQLServerTE, strSQLDatabaseTE, txtSerial.Text.Trim(), Hidden_CustomerPLID.Value.ToString(), HiddenPalletTimeout.Value.ToString());

            }


            ct_PalletTimeOutByCustomer.UpdatePalletTimeOut(strSQLServerTE, strSQLDatabaseTE, txtSerial.Text.Trim(), Hidden_CustomerPLID.Value.ToString(), HiddenPalletTimeout.Value.ToString());

        }
        catch (Exception e2)
        {
            lblMessage.Visible = true;
            lblMessage.Text = e2.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage.Text);

        }


    }
    protected void txtUnit_TextChanged(object sender, EventArgs e)
    {
        
        //Unificar formato de scaner
        string sBoardSerialUpper = txtUnit.Text.ToUpper(new System.Globalization.CultureInfo("es-ES", false));
        string sBoardSerialDash = sBoardSerialUpper.Replace("'", "-");
        txtUnit.Text = sBoardSerialDash.Replace("Ñ", ":");
        txtUnit.Text = txtUnit.Text.Trim();
     
        //Validar si el numero de serie ya fue escaneado
        if (!IsSerialScanned(txtUnit.Text))
        {
            
            //Valida serial

            lblMessage0.Text = string.Empty;
            fnUnitValidation(txtUnit.Text);
          
        }
        else
        {
            btnOK.Visible = true;
            lblMessage0.Visible = true;
            lblMessage0.Text = "Tablilla REPETIDA, favor de escanear una tablilla diferente" + "<br /> Presione el boton OK para continuar <br /> ";



        }



    }
     protected bool fnUnitValidation(string SerialNumber)
    {
        CLPalletLink.CT_LocalDataBase ctLocalDB = new CLPalletLink.CT_LocalDataBase();
        wsMes.Service MES = new wsMes.Service();
        wsMESTIS.MES_TIS MESTIS = new wsMESTIS.MES_TIS();
        DataSet dsLocalAssembly = new DataSet();
        DataSet dsSelectBySerialNumber = new DataSet();
        DataSet dsListByBoard = new DataSet();
        DataSet dsListAllByWipID = new DataSet();
        DataSet dsRevisionAssembly = new DataSet();
        DataSet dsUnitHistory = new DataSet();
        DataSet dsIDAssembly = new DataSet();
        DataTable dtGetMaterialID = new DataTable();

        CLPalletLink.CT_Assemblies ctAssy = new CLPalletLink.CT_Assemblies();
        CLPalletLink.WP_Panels wpPanel = new CLPalletLink.WP_Panels();
        CLPalletLink.SY_PalletLink wpLog = new CLPalletLink.SY_PalletLink();
        DataSet dsMESAssembly = new DataSet();
        DataSet ds2 = new DataSet();
       
        Dictionary<int, string> UnitsInPanel = new Dictionary<int, string>();
        Dictionary<int, string> UnitsInPanelOK = new Dictionary<int, string>();
        bool unitOK = true;
        int panel_ID = 0;
        bool boardPanelBroken = false;
        string number = "";
        string assembly_ID = "";
        string assembly = "";
        string revision = "";
        string customerText = "";
        int wIP_ID = 0;
        int customer_ID = 0;
        string divisionText = "";

        Boolean CheckPointVerification = false;
        Boolean CheckLoops = false;
        int LoopsNumber = 0;
        int PalletPieces = 0;
        Boolean ValidaPanel = false;
        string[] historyInfo = new string[7];

        try
        {
            //Obtener Informacion del Numero de Serie en MES
            historyInfo = fnGetHistory(SerialNumber);
            wIP_ID = Int32.Parse(historyInfo[0]);
            customer_ID = Int32.Parse(historyInfo[1]);
            customerText = historyInfo[2].ToString();
            assembly = historyInfo[3].ToString();
            number = historyInfo[4].ToString();
            revision = historyInfo[5].ToString();
            assembly_ID = historyInfo[6].ToString();

            // Obtiene la informacion de la base de datos de Pallet Link
            dsLocalAssembly = ctLocalDB.checkIfAssyLocallyExistsUpd(strSQLServer, strSQLDataBase, number, Int32.Parse(Hidden_CustomerPLID.Value), HiddenPalletID.Value);

            if (dsLocalAssembly.Tables[0].Rows.Count == 0)
            {
                lblMessage0.Text = lblMessage0.Text + " Error, configuracion de ensamble  NO encontrada en PALLET LINK, favor dar de alta el ensamble";
                lblMessage0.Visible = true;
                fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", HiddenPalletID.Value,
                     HiddenAssembly.Value, HiddenBoardAssembly.Value, "", "",
                     "", "", "", "", "", "", "", "", "", lblMessage0.Text);
                return false;
            }

            int Rows = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["Rows"].ToString());
            int Columns = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["Columns"].ToString());
            int PanelTotal = Rows * Columns;
            ValidaPanel = Boolean.Parse(dsLocalAssembly.Tables[0].Rows[0]["Panel"].ToString());
            CheckPointVerification = Boolean.Parse(dsLocalAssembly.Tables[0].Rows[0]["CheckPointValidation"].ToString());
            CheckLoops = Boolean.Parse(dsLocalAssembly.Tables[0].Rows[0]["LoopValidation"].ToString());
            LoopsNumber = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["LoopsAllowed"].ToString());
            PalletPieces = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["Pieces"].ToString());

            //Obtener Informacion del Panel de MES (ListByBoard)
            dsListByBoard = MES.ListByBoard(wIP_ID);

            // Coloca los numeros de serie del panel en un diccionario
            if (ValidaPanel)
            {
                foreach (DataRow dr in dsListByBoard.Tables[0].Rows)
                {
                    UnitsInPanel.Add(Int32.Parse(dr["Mapping"].ToString()), dr["SerialNumber"].ToString());
                }
            }
            else
            {
                UnitsInPanel.Add(1, SerialNumber);
            }

            // Valida los numeros de serie
            foreach (int key in UnitsInPanel.Keys)
            {
                // Obtiene informacion del historial en MES 
                string snToValidate = UnitsInPanel[key].ToString();
                historyInfo = fnGetHistory(snToValidate);
                wIP_ID = Int32.Parse(historyInfo[0]);
                customer_ID = Int32.Parse(historyInfo[1]);
                customerText = historyInfo[2].ToString();
                assembly = historyInfo[3].ToString();
                number = historyInfo[4].ToString();
                revision = historyInfo[5].ToString();
                assembly_ID = historyInfo[6].ToString();

                if (!fnSerialNumberValidation(snToValidate, wIP_ID, customer_ID, customerText, assembly_ID, number, CheckPointVerification, CheckLoops, LoopsNumber, PalletPieces, ValidaPanel))
                {
                    unitOK = false;
                }

                UnitsInPanelOK.Add(key, snToValidate + "|" + wIP_ID);

            }

            if (unitOK)
            {
                fnfillUnitsInfo(customer_ID, wIP_ID, divisionText, number, assembly_ID, assembly, revision, UnitsInPanelOK, PalletPieces,ValidaPanel);
                this.lblMessage0.Visible = false;
                this.btnOK.Visible = false;
                return true;
            }
            else
            {
                return false;

            }
           
        }
        catch (Exception ex)
        {
            lblMessage0.Visible = true;
            lblMessage0.Text = ex.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                           HiddenAssembly.Value, HiddenBoardAssembly.Value, wIP_ID.ToString(), PalletPieces.ToString(),
                           "", "", "", "", "", "", "", LoopsNumber.ToString(), LoopsNumber.ToString(), lblMessage0.Text);
            PalletPieces = 0;
            //    return PalletPieces;

        }
        return true;

    }
  

  private void fnSetgvUnitsInfoRows(int rows, int columns)
    {
        DataTable dtdrig = new DataTable();
        int dgRows = rows * columns;


        for (int i = 1; i <= dgRows; i++)
        {
            dtdrig.Rows.Add();
        }

        gvUnitsInfo.DataSource = dtdrig;



    }
    private bool fnSerialNumberValidation(string SerialNumber, int wIP_ID, int customer_ID, string Customer, string Assembly_Id, string number, bool CheckPointVerification, bool CheckLoops, int LoopsNumber, int PalletPieces, bool ValidaPanel)
    {
        /*Verifica si la pieza está en Hold*/
      if (VerifyIfBoardOnHold(wIP_ID))
        {
            btnOK.Visible = true;
            lblMessage0.Visible = true;
            lblMessage0.Text = "Tablilla en HOLD favor de contactar al departamento de Calidad para liberarla" + "<br /> Retire la unidad del pallet y presione el boton OK para continuar";
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                       Assembly_Id, number, wIP_ID.ToString(), PalletPieces.ToString(),
                       "", "", "", "", "", "", "", LoopsNumber.ToString(), LoopsNumber.ToString(), lblMessage0.Text + " Valida Panel " + ValidaPanel.ToString());
            return false;
        }

        /* Verificar checkpoint */
        if (CheckPointVerification)
        {

            DataSet dsCheckPoint = new DataSet();
            dsCheckPoint = VerifiyCheckPoint(SerialNumber, "0", customer_ID, Convert.ToInt32(Assembly_Id.ToString()));
            if (dsCheckPoint.Tables[0].Rows.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < dsCheckPoint.Tables[0].Rows.Count; i++)
                {

                    message = message + dsCheckPoint.Tables[0].Rows[i]["DescrText"].ToString() + ", ";
                }

                btnOK.Visible = true;
                lblMessage0.Visible = true;
                lblMessage0.Text = "A la pieza le faltan los siguientes procesos requeridos en MES: "
                                    + message + " " + "Favor de contactar al departamento de CALIDAD para continuar" + "<br /> Retire la unidad del pallet y presione el boton OK para continuar";
                fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                        Assembly_Id, number, wIP_ID.ToString(), PalletPieces.ToString(),
                        "", "", "", "", "", "", "", LoopsNumber.ToString(), LoopsNumber.ToString(), lblMessage0.Text);

                return false;

            }
        }

        if (CheckLoops)
        {
            int BoardLoops = CheckBoardCounterValidation(SerialNumber, wIP_ID, LoopsNumber, customer_ID, number);
            if (BoardLoops > 0)
            {
                if (customer_ID == 148)
                {
                    btnOK.Visible = true;
                    lblMessage0.Visible = true;
                    lblMessage0.Text = " Error en ligado. La tablilla ha EXCEDIDO el número de ligados permitidos" + "<br /> Retire la unidad del pallet y presione el boton OK para continuar";
                    fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                   Assembly_Id, number, wIP_ID.ToString(), PalletPieces.ToString(),
                   "", "", "", "", "", "", "", BoardLoops.ToString(), LoopsNumber.ToString(), lblMessage0.Text);

                }
                else
                {
                    btnOK.Visible = true;
                    lblMessage0.Visible = true;
                    lblMessage0.Text = "Status de la tablilla ONHOLD por EXCEDER el número de ligados permitidos, favor de contactar al encargado de CALIDAD para liberarla" + "<br /> Retire la unidad del pallet y presione el boton OK para continuar";
                    fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), SerialNumber, HiddenPalletID.Value,
                   Assembly_Id, number, wIP_ID.ToString(), PalletPieces.ToString(),
                   "", "", "", "", "", "", "", BoardLoops.ToString(), LoopsNumber.ToString(), lblMessage0.Text);


                }
                return false;
            }
        }

        return true;
    }
    //private void fnfillUnitsInfo(int customer_ID, int wip_ID, string divisionText, string number, string assembly_ID, string assembly, string revision, Dictionary<int, string> serialNumbers, int palletPieces)
    private void fnfillUnitsInfo(int customer_ID, int wip_ID, string divisionText, string number, string assembly_ID, string assembly, string revision, Dictionary<int, string> serialNumbers, int palletPieces,bool isPanel)
	{
        DataTable dt_drig = new DataTable();

        dt_drig.Columns.Add("Panel", typeof(string));
        dt_drig.Columns.Add("CUSTOMER_ID", typeof(string));
        dt_drig.Columns.Add("divisionText", typeof(string));
        dt_drig.Columns.Add("number", typeof(string));
        dt_drig.Columns.Add("assembly_ID", typeof(string));
        dt_drig.Columns.Add("assembly", typeof(string));
        dt_drig.Columns.Add("revision", typeof(string));

        foreach (int key in serialNumbers.Keys)
        {
            dt_drig.Columns.Add("Unit " + key, typeof(string));
            dt_drig.Columns.Add("WipID " + key, typeof(string));

        }



        if (gvUnitsInfo.Rows.Count > 0)
        {

            for (int r = 0; r < gvUnitsInfo.Rows.Count; r++)
            {
                DataRow dr0 = dt_drig.NewRow();

                for (int c = 0; c < gvUnitsInfo.Rows[r].Cells.Count; c++)
                {

                    dr0[c] = gvUnitsInfo.Rows[r].Cells[c].Text.Replace("&nbsp;", ""); // Text.Trim();

                }

                dt_drig.Rows.Add(dr0);

            }

        }


        DataRow dr = dt_drig.NewRow();
        dr[0] = gvUnitsInfo.Rows.Count + 1;
        dr[1] = customer_ID;
        dr[2] = divisionText;
        dr[3] = number;
        dr[4] = assembly_ID;
        dr[5] = assembly;
        dr[6] = revision;

        int x = 7;

        foreach (string value in serialNumbers.Values)
        {
            string[] snInfo = value.Split('|');

            dr[x] = snInfo[0];
            dr[x + 1] = snInfo[1]; ;
            x = x + 2;
        }

        dt_drig.Rows.Add(dr);


        gvUnitsInfo.DataSource = dt_drig;
        gvUnitsInfo.DataBind();


        txtUnit.Text = "";

        //if (gvUnitsInfo.Rows.Count == palletPieces)
	    if ((gvUnitsInfo.Rows.Count == palletPieces) || (isPanel))
        {
            pnlPiezas.Visible = false;
            btnLink.Visible = true;
            btnLink.Enabled = true;
            btnLink.Focus();

        }
        else
        {
            txtUnit.Focus();

        }




    }
    protected void gvUnitsInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;

        //for (int x = 8; x <= e.Row.Cells.Count; x = x + 2)
        //{
        //    e.Row.Cells[x].Visible = false;
        //}
        for (int x = 8; x <= e.Row.Cells.Count; x = x + 2)
        {

            e.Row.Cells[x].Visible = false;
        }


    }
    protected void btnLink_Click(object sender, EventArgs e)
    {
		
        wsMes.Service MES = new wsMes.Service();
        wsMESTIS.MES_TIS MESTIS = new wsMESTIS.MES_TIS();
        DataTable dtGetMaterialID = new DataTable();
        DataSet dsEquipmentID = new DataSet();
        
        btnLink.Visible = false;
        btnLink.Enabled = false;
        Label masterLblProcess = this.Master.FindControl("lblProcess") as Label;
        string customer_ID = "";
        string wip_ID = "";
        string divisionText = "";
        string number = "";
        string assembly_ID = "";
        string assembly = "";
        string revision = "";
        string serialNumberToLink = "";
        int linkMaterialID = 0;
                
        
        if (gvUnitsInfo.Rows.Count > 0)
        {

            for (int r = 0; r < gvUnitsInfo.Rows.Count; r++)
            {

                // Obtener informacion para ligar

                customer_ID = gvUnitsInfo.Rows[r].Cells[1].Text;
                divisionText = gvUnitsInfo.Rows[r].Cells[2].Text; ;
                number = gvUnitsInfo.Rows[r].Cells[3].Text;
                assembly_ID = gvUnitsInfo.Rows[r].Cells[4].Text;
                assembly = gvUnitsInfo.Rows[r].Cells[5].Text;
                revision = gvUnitsInfo.Rows[r].Cells[6].Text;


                for (int c = 7; c < gvUnitsInfo.Rows[r].Cells.Count; c = c + 2)
                {


                    // Obtener numero de serie para ligar 
                    serialNumberToLink = gvUnitsInfo.Rows[r].Cells[c].Text;
                    wip_ID = gvUnitsInfo.Rows[r].Cells[c + 1].Text;


                    // Obtener el Objeto de ligado
                    dtGetMaterialID = MES.GetMaterialID(Convert.ToInt32(assembly_ID), "");

                    if (dtGetMaterialID.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable dtMaterialID = new DataTable();
                            DataRow[] drMaterialID;

                            drMaterialID = dtGetMaterialID.Select("LinkObject = '" + Hidden_LinkObject.Value.ToString() + "'");
                            dtMaterialID = drMaterialID.CopyToDataTable();

                            linkMaterialID = Int32.Parse(dtMaterialID.Rows[0]["LinkMaterial_ID"].ToString());

                        }
                        catch
                        {
                            lblMessage1.Visible = true;
                            lblMessage1.Text = "No se encontro material de ligado para la siguiente configuracion.</br> " +
                                               "Proceso: " + Hidden_Process.Value.ToString() + ".</br> " +
                                               "Objeto de ligado: " + Hidden_LinkObject.Value.ToString() + ".</br> " +
                                               "ID del ensamble: " + assembly_ID + ".</br > " +
                                               "Numero de ensamble: " + number + ".</br > " +
                                               "Revision: " + revision + ".</br > " +
                                               "Favor de contactar el personal de Calidad.";
                            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage1.Text);
                            pnlPiezas.Visible = false;
                            pnlPiezas.Enabled = false;
                            return;
                        }
                    }
                    else
                    {
                        lblMessage1.Visible = true;
                        lblMessage1.Text = "No se encontro material de ligado para la siguiente configuracion.</br> " +
                                           "Proceso: " + Hidden_Process.Value.ToString() + ".</br> " +
                                           "Objeto de ligado: " + Hidden_LinkObject.Value.ToString() + ".</br> " +
                                           "ID del ensamble: " + assembly_ID + ".</br > " +
                                           "Numero de ensamble: " + number + ".</br > " +
                                           "Revision: " + revision + ".</br > " +
                                           "Favor de contactar el personal de Calidad.";
                        fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage1.Text);
                        pnlPiezas.Visible = false;
                        pnlPiezas.Enabled = false;
                        return;
                    }

                    
                    ////Obetner el Route Step ID
                    //string RouteStep = MESTIS.GetCurrentRouteStep(serialNumberToLink);
                    int routeStep_ID = 0;
                    //if (RouteStep.ToUpper().Contains("NO WIP INFORMATION"))
                    //{
                    //    lblMessage1.Visible = true;
                    //    lblMessage1.Text = "No se encontro Route Step.</br> " +
                    //                       "Numero de serie: " + serialNumberToLink + ".</br> " +
                    //                       "Favor de contactar el personal de Calidad.";
                    //    fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage1.Text);
                    //    pnlPiezas.Visible = false;
                    //    pnlPiezas.Enabled = false;
                    //    return;
                    //}
                    //else
                    //{

                    //    StringReader xmlRouteStep = new StringReader(RouteStep);
                    //    DataSet dsRouteStepID = new DataSet();
                    //    dsRouteStepID.ReadXml(xmlRouteStep);
                    //    routeStep_ID = Int32.Parse(dsRouteStepID.Tables[0].Rows[0]["RouteStep_ID"].ToString());
                    //    
                    //    //routeStep_ID = 7863;

                    //}

                    //Obtener Equipment ID  LDC18266118
                    //dsEquipmentID = MES.GetEquipmentID(routeStep_ID);
                    int equipment_ID = 0;
					//int equipment_ID = Int32.Parse(dsEquipmentID.Tables[0].Rows[0]["Equipment_ID"].ToString());

                    //Realizar ligado
                    //bool linkOK = MES.EPS_LinkNonUniqueComponent(Int32.Parse(customer_ID), Int32.Parse(wip_ID), linkMaterialID, txtSerial.Text, routeStep_ID, equipment_ID);
                    bool linkOK = MES.EPS_LinkNonUniqueComponent(Int32.Parse(customer_ID), Int32.Parse(wip_ID), linkMaterialID, txtSerial.Text, 1, 1);

                    if (!linkOK)
                    {
                        lblMessage1.Visible = true;
                        lblMessage1.Text = "No fue posible ligar el numero de serie: " + serialNumberToLink + ". </br> " +
                                           "Con el pallet: " + txtSerial.Text  + ".";
                        fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), serialNumberToLink, txtSerial.Text,
                                   assembly_ID, number, wip_ID, "", "", "", Hidden_LinkObject.Value.ToString(), linkMaterialID.ToString(), "", Hidden_MesEquipment.Value.ToString(),
                                   "", "", "", lblMessage1.Text);
                        pnlPiezas.Visible = false;
                        pnlPiezas.Enabled = false;
                        return;

                    }
                             

                    //Enviar pass a MES
                    string sMES = "S" + serialNumberToLink + Environment.NewLine +
                                      "C" + Hidden_Customer.Value.ToString() + Environment.NewLine +
                                      "I" + divisionText + Environment.NewLine +
                                      "N" + Hidden_MesEquipment.Value.ToString() + Environment.NewLine +
                                      "P" + Hidden_RouteStep.Value.ToString() + Environment.NewLine +
                                      "O" + Environment.NewLine +
                                      "p12" + Environment.NewLine +
                                      "L" + Environment.NewLine +
                                      "n" + number + Environment.NewLine +
                                      "r" + revision + Environment.NewLine +
                                      "TP" + Environment.NewLine +
                                      "[" + DateTime.Now.AddSeconds(3).ToString("MM/dd/yyyy HH:mm:ss.fff") + Environment.NewLine +
                                      "]" + DateTime.Now.AddSeconds(4).ToString("MM/dd/yyyy HH:mm:ss.fff") + Environment.NewLine;


                        string Resultado = MESTIS.ProcessTestData(sMES, "Generic");

                        if (Resultado.ToUpper() != "PASS")
                        {
                            lblMessage1.Visible = true;
                            lblMessage1.Text = "No se puede enviar el \"Step\" a MES. </br> " +
                                               "Se encontro el siguiente error: </br> " +
                                               Resultado + ".";
                            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), serialNumberToLink, txtSerial.Text,
                                       assembly_ID, number, wip_ID, "", "", "", Hidden_LinkObject.Value.ToString(), linkMaterialID.ToString(), "", Hidden_MesEquipment.Value.ToString(),
                                       "", "", "", lblMessage1.Text);
                            pnlPiezas.Visible = false;
                            pnlPiezas.Enabled = false;
                            return;

                        }


                        // Registra el ligado en la Base De Datos Local
                        fnregisterLinkInBD(serialNumberToLink, linkMaterialID, equipment_ID, Hidden_MesEquipment.Value.ToString(), number, int.Parse(wip_ID), routeStep_ID);

                    

                }

            }

            //Termina el Ligado
            fnUpdateCounter();
            fnUpdatePalletTimeout();
            ImgPalomitaOK.Style.Add("display", "inline-block");
            ImgPalomitaOK.Visible = true;
            lblMessage2.Visible = true;
            clearAll();
            txtSerial.Focus();

        }

    }


	private void fnregisterLinkInBD(string serialNumber, int LinkMaterialID, int equipment_ID, string EquipmentName, string Assembly, int wip_ID, int routeStep_ID)
    {
        CLPalletLink.SY_PalletLink ctLink = new CLPalletLink.SY_PalletLink();
        string Server = strSQLServer;
        string DataBase = strSQLDataBase;
        try
        {
            // Registra ligado en base de datos local
            ctLink.fnInsertPalletLink(strSQLServerTE, strSQLDatabaseTE, serialNumber, txtSerial.Text, "PalletLink", (HiddenUser.Value.ToString()), 1);

            /* Actualizar el contador de loops */

            CLPalletLink.SY_BoardLoopsCounter syBoardLoopsCounter = new CLPalletLink.SY_BoardLoopsCounter();
            DataSet dsGetLoops = new DataSet();
            DataSet dsUpdLoops = new DataSet();
            DataSet ds = new DataSet();
            int PKBoardLoopCounter = 0;
            int LoopsNumber = 0;

            dsGetLoops = syBoardLoopsCounter.fnGetBoardLoopsNumber(strSQLServerTE, strSQLDatabaseTE, serialNumber, Assembly);
            if (dsGetLoops.Tables[0].Rows.Count > 0)
            {
                PKBoardLoopCounter = Convert.ToInt32(dsGetLoops.Tables[0].Rows[0]["PKBoardLoopCounter"]);
                LoopsNumber = Convert.ToInt32(dsGetLoops.Tables[0].Rows[0]["LoopsNumber"]);
                LoopsNumber = LoopsNumber + 1;
                /*Actualiza loops*/
                dsUpdLoops = syBoardLoopsCounter.fnUpdateBoardLoopsCounter(strSQLServerTE, strSQLDatabaseTE, PKBoardLoopCounter, LoopsNumber, "");
            }
            else
            {
                /*Inserta fila en la tabla para contabilizar loops*/
                LoopsNumber = 1;
                ds = syBoardLoopsCounter.fnInsertBoardLoopsCounter(strSQLServerTE, strSQLDatabaseTE, serialNumber, LoopsNumber, "", Assembly);
            }






        }

        catch (Exception Register)
        {
            lblMessage0.Visible = true;
            lblMessage0.Text = Register.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(),
                serialNumber, HiddenPalletID.Value,
               HiddenAssembly.Value, HiddenBoardAssembly.Value, "", "", "", "", "", "", HiddenEquipmentID.Value, HiddenEquipment.Value, HiddenRouteStepID.Value, "", "", lblMessage0.Text);

        }

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        txtUnit.Text = "";
        lblMessage0.Visible = false;
        btnOK.Visible = false;
        txtUnit.Focus();
    }
    private void fnGetConfig()
    {
        CLPalletLink.CT_LocalDataBase ctLocalDB = new CLPalletLink.CT_LocalDataBase();
        DataSet ds = new DataSet();
        
        bool customerExist = false;
        string machineName = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
        string[] machinesplit = machineName.Split('.');
        machineName = machinesplit[0];

        ds = ctLocalDB.getPalletLinkConfig(strSQLServerTE, strSQLDatabaseTE, machineName);

        if (ds.Tables[0].Rows.Count == 1)
        {
            pnlCliente.Visible = false;
            pnlLinking.Visible = true;

            Hidden_PKMachine.Value = ds.Tables[0].Rows[0]["PKMachine"].ToString();
            Hidden_Customer.Value = ds.Tables[0].Rows[0]["Customer"].ToString();
            Hidden_Process.Value = ds.Tables[0].Rows[0]["Process"].ToString();
            Hidden_MesEquipment.Value = ds.Tables[0].Rows[0]["MesEquipment"].ToString();
            Hidden_RouteStep.Value = ds.Tables[0].Rows[0]["RouteStep"].ToString();
			Hidden_LinkObject.Value = ds.Tables[0].Rows[0]["LinkObject"].ToString();

            // lblVersion.Text = "Pallet Link " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            lblVersion.Text = "Pallet Link " + "3.0.0.0";
            lblProcess.Text = "&nbsp; - &nbsp;" + Hidden_Process.Value;

            customerExist = fnGetCustomersLocalDB(Hidden_Customer.Value);

            if(customerExist)
            {
                fnStartPalletLink();
            }
            
        }
        else if (ds.Tables[0].Rows.Count > 1)
        {
            pnlCliente.Visible = true;
            pnlLinking.Visible = false;
            lblProcess.Text = "";

            cmbSelectCustomer1.DataMember = "PKId";
            cmbSelectCustomer1.DataTextField = "Customer";
            cmbSelectCustomer1.DataSource = (ds.Tables[0]);
            cmbSelectCustomer1.DataBind();
            
            gvCustomerTemp.DataSource = (ds.Tables[0]);
            gvCustomerTemp.DataBind();

        }
        else
        {
            pnlCliente.Visible = false;
            pnlLinking.Visible = true;
            lblMessage1.Visible = true;
            lblMessage1.Text = "<br /> <br /> Este equipo (" + machineName + ") no es valido para pallet link.<br /> <br /> Favor de contactar el personal de Ingenieria de Manufactura. <br /> <br /> <br />";


        }

    }
    private bool fnGetCustomersLocalDB(string customer)
    {
        CLPalletLink.CT_Customers ctCustomers = new CLPalletLink.CT_Customers();
        DataSet ds = new DataSet();
        bool customerFind = false;
		customer = "'" + customer + "'";

        try
        {
            ds = ctCustomers.getCustomersInfo(strSQLServerTE, strSQLDatabaseTE, customer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Hidden_CustomerPLID.Value = ds.Tables[0].Rows[0]["PKCustomerID"].ToString();
                customerFind = true;

                Byte[] data = new byte[0];
                data = (Byte[])(ds.Tables[0].Rows[0]["Logotype"]);

                RadBinaryImage1.DataValue = data;


                //imgCustomer.ImageUrl = ds.Tables[0].Rows[0]["Logotype"];


            }
            else
            {
                lblMessage1.Visible = true;
                lblMessage1.Text = "<br /> <br /> No se encontro la configuracion de cliente (" + customer + ") en la base de datos.<br /> <br /> Favor de contactar el personal de Ingenieria de Manufactura. <br /> <br /> <br />";
                customerFind = false;

            }
        }
        catch (Exception e2)
        {
            lblMessage1.Visible = true;
            lblMessage1.Text = e2.Message;
            fnWriteLog(strSQLServer, strSQLDataBase, Hidden_CustomerPLID.Value.ToString(), Hidden_Customer.Value.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", lblMessage1.Text);

        }
        return customerFind;
    }
    private void fnStartPalletLink()
    {
        fnGetPalletTimeout();
        pnlSN.Visible = true;
        this.lblMessage0.Visible = false;
        pnlCustomer.Visible = true;
        pnlUnitsInfo.Visible = false;

        gvFixtureData.DataSource = null;
        gvFixtureData.DataBind();
        gvEscalation.DataSource = null;
        gvEscalation.DataBind();

        txtSerial.Text = "";
        txtUnit.Text = "";

    }
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        for (int r = 0; r < gvCustomerTemp.Rows.Count; r++)
        {

            if (gvCustomerTemp.Rows[r].Cells[2].Text == cmbSelectCustomer1.Text)
            {
                Hidden_PKMachine.Value = gvCustomerTemp.Rows[r].Cells[1].Text;
                Hidden_Customer.Value = gvCustomerTemp.Rows[r].Cells[2].Text;
                Hidden_Process.Value = gvCustomerTemp.Rows[r].Cells[3].Text;
                Hidden_MesEquipment.Value = gvCustomerTemp.Rows[r].Cells[4].Text;
                Hidden_RouteStep.Value = gvCustomerTemp.Rows[r].Cells[5].Text;
				Hidden_LinkObject.Value = gvCustomerTemp.Rows[r].Cells[6].Text;

                
                lblProcess.Text = "&nbsp; - &nbsp;" + Hidden_Process.Value;

                bool customerExist = fnGetCustomersLocalDB(Hidden_Customer.Value);

                if (customerExist)
                {
                    fnStartPalletLink();
                }

                pnlCliente.Visible = false;
                pnlLinking.Visible = true;
                txtSerial.Focus();

                break;
            }


        }


    }
     private string[] fnGetHistory(string snToValidate)
    {
        wsMes.Service MES = new wsMes.Service();
        DataSet dsSelectBySerialNumber = new DataSet();
        DataSet dsBoardHistory = new DataSet();
        DataSet dsUnitHistory = new DataSet();
        DataTable dtAssemblyID = new DataTable();
        DataTable dtBoardHistory = new DataTable();
        string[] validationResult = new string[7];
        int lastRow;

        //Obtener Informacion del Numero de Serie en MES (SelectBySerialNumber)
        dsSelectBySerialNumber = MES.SelectBySerialNumber(snToValidate);
        validationResult[0] = dsSelectBySerialNumber.Tables[0].Rows[0]["WIP_ID"].ToString();
        validationResult[1] = dsSelectBySerialNumber.Tables[0].Rows[0]["Customer_ID"].ToString();
        validationResult[2] = dsSelectBySerialNumber.Tables[0].Rows[0]["CustomerText"].ToString();
        int customer_ID = Int32.Parse(validationResult[1]);

        // Obtiene informacion del historial en MES (BoardHistoryReport)
        dsBoardHistory = MES.BoardHistoryReport(snToValidate, customer_ID);
        DataRow[] foundRows = dsBoardHistory.Tables[0].Select("TestType not like '%Desviation%' AND TestType not like '%Link%' AND TestType not like '%Unlink%'", "StartDatetime ASC");
        dtBoardHistory = foundRows.CopyToDataTable();
        lastRow = dtBoardHistory.Rows.Count - 1;
        validationResult[3] = dtBoardHistory.Rows[lastRow]["Assembly"].ToString();
        validationResult[4] = dtBoardHistory.Rows[lastRow]["Number"].ToString();
        validationResult[5] = dtBoardHistory.Rows[lastRow]["Revision"].ToString();

        // Obtiene el ASSEMBLY_ID de MES (IDAssembly)
        dtAssemblyID = MES.IDAssembly(validationResult[4].ToString(), validationResult[5].ToString(), ""); // Number, Revision, Version
        validationResult[6] = dtAssemblyID.Rows[0]["Assembly_ID"].ToString();
        
        return validationResult;


    }
	  private void fnWriteLog(string strSQLServer, string strDataBase, string Customer_Id, string Customer, string SerialNumber, string PalletId,
                            string Assembly_Id, string Assembly, string Wip_Id, string PanelNumberPL, string PanelSizePL,
                            string PanelSizeMES, string LinkObject, string LinkMaterialID, string EquipmentValue,
                            string EquipmentName, string RouteStepID, string SerialLoops, string LoopsAllowed, string Message)
    {

        wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
        DataSet ds = new DataSet();

        string strSQL = "DECLARE	@return_value int " +
                        "EXEC	@return_value = [dbo].[up_AddLinkingLog] " +
                        "@Customer_Id = '" + Customer_Id + "', " +
                        "@Customer = '" + Customer + "', " +
                        "@SerialNumber = '" + SerialNumber + "', " +
                        "@PalletId = '" + PalletId + "', " +
                        "@Assembly_Id = '" + Assembly_Id + "', " +
                        "@Assembly = '" + Assembly + "', " +
                        "@Wip_Id = '" + Wip_Id + "', " +
                        "@PanelNumberPL = '" + PanelNumberPL + "', " +
                        "@PanelSizePL = '" + @PanelSizePL + "', " +
                        "@PanelSizeMES = '" + PanelSizeMES + "', " +
                        "@LinkObject = '" + LinkObject + "', " +
                        "@LinkMaterialID = '" + LinkMaterialID + "', " +
                        "@EquipmentValue = '" + EquipmentValue + "', " +
                        "@EquipmentName = '" + EquipmentName + "', " +
                        "@RouteStepID = '" + RouteStepID + "', " +
                        "@SerialLoops = '" + SerialLoops + "', " +
                        "@LoopsAllowed = '" + LoopsAllowed + "', " +
                        "@Message = '" + Message + "' ";
        

        try
        {
            ds = SQL.dsSQLQuery(strSQLServer, strSQLDataBase, strSQL);
        }
        catch (Exception e3)
        {
            Console.WriteLine(e3.Message);
            ds = null;
        }


    }
   


}