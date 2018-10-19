<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteWithoutMenu.Master" CodeFile="Linking.aspx.cs" Inherits="PalletLink_aspx_Linking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <title>Pallet Link App</title>
    <style type="text/css">
        
        body
        {
            background-repeat: repeat-x;
            background-image: linear-gradient(to bottom,#3c3c3c 0,#222 100%);
            background-color: #222;
            border-color: #080808;
        }
        table
        {
            border-collapse: collapse;
            font-size: 12pt;
           
           
        }
         #divGrid
        {
            border-collapse: collapse;
            font-size: 12pt;
            align-items:center;
           
           
           
        }
        #tableData tr td
        {
            padding: 1px !important;
          
        }
        #tableData1 tr td
        {
            padding: 1px !important;
            align-items:center;
        }
        body
        {
            margin: 0 !important;
            padding: 0 !important;
            width: 100% !important;
        }
        .data
        {
            width: 100%;
        }
       
        #content
        {
            padding: 10px !important;
        }
        .row2
        {
            width: 200px;
        }
        
        #tableData1 > tbody:hover th, #tablePartName > tbody:hover th, #tabledefectParts > tbody:hover th, #tableActions > tbody:hover th
        {
            background: #0F0 !important;
        }
        #tableData1 > tbody tr:hover td, #tablePartName > tbody tr:hover td, #tabledefectParts > tbody tr:hover td, #tableActions > tbody tr:hover td
        {
            background: #FF0 !important;
        }
        .panel
        {
            margin-bottom: 10px !important;
            align-content:center;
            
        }
        .callout
        {
            text-align: center;
            font-weight: bold;
            background-color: khaki;
            border: 1px solid #AAA;
            border-radius: 5px;
        }
      
        .clickable
        {
            cursor: pointer;
        }
        .loader
        {
            position: absolute;
            width: 8%;
            z-index: 100;
            opacity: 0.5;
            top: 987px;
            left: 790px;
            height: 25px;
        }
        .loader img
        {
            margin-left: 300px;
            width: 26px;
        }
        .Hide
        {
            display:none
        }
        </style>
     <%--<script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>--%>
      <script type="text/javascript">
          function DisableButton() {
              $('#ContentPlaceHolder1_btnSubmit').hide();
              console.log("hide button");
              $('#ContentPlaceHolder1_btnSubmit').prop('hidden', 'hidden');
              $('#ContentPlaceHolder1_btnSubmit').click(function () { console.log("nop") });
             

          }
          function DisableEnterButton(e) {
              if (e.keyCode == 13) {
                  //$('#ContentPlaceHolder1_TextBoxPanel1').addClass("aspNetDisabled");
                  $('#ContentPlaceHolder1_btnSubmit').hide();
                  console.log("hide button");
                  $('#ContentPlaceHolder1_btnSubmit').prop('hidden', 'hidden');

              }
          }
          function DisableTextPanelUno(e) {
              if (e.keyCode == 13) {
                  //$('#ContentPlaceHolder1_TextBoxPanel1').addClass("aspNetDisabled");
                  $('#ContentPlaceHolder1_TextBoxPanel1').prop('hidden', 'hidden');
                  $('#ContentPlaceHolder1_TextBoxPanel1').hide();
                
              }
          }
          function DisableTextPanelDos(e) {
              if (e.keyCode == 13) {
                  //$('#ContentPlaceHolder1_TextBoxPanel2').addClass("aspNetDisabled");
                  $('#ContentPlaceHolder1_TextBoxPanel2').prop('hidden', 'hidden');
                  $('#ContentPlaceHolder1_TextBoxPanel2').hide();
                  
              }
          }
          function DisableTextPanelTres(e) {
              if (e.keyCode == 13) {
                  //$('#ContentPlaceHolder1_TextBoxPanel3').addClass("aspNetDisabled");
                  $('#ContentPlaceHolder1_TextBoxPanel3').prop('hidden', 'hidden');
                  //$('#ContentPlaceHolder1_TextBoxPanel3').hide();
              }
          }
          $(function () {
              $('#ContentPlaceHolder1_btnSubmit').click(DisableButton);
              $('#ContentPlaceHolder1_btnSubmit').keypress(DisableEnterButton);            
              $('#ContentPlaceHolder1_TextBoxPanel1').keypress(DisableTextPanelUno);
              $('#ContentPlaceHolder1_TextBoxPanel2').keypress(DisableTextPanelDos);
              //$('#ContentPlaceHolder1_TextBoxPanel3').keypress(DisableTextPanelTres);
              //$('#ContentPlaceHolder1_TextBoxPanel2:enabled').prop('hidden', false);
              //$('#ContentPlaceHolder1_TextBoxPanel2:enabled').show();
              //$('#ContentPlaceHolder1_TextBoxPanel2:enabled').focus();
              
              $('#ContentPlaceHolder1_btnSubmit:enabled').prop('hidden', false);
              $('#ContentPlaceHolder1_btnSubmit:enabled').show();
              $('#ContentPlaceHolder1_btnSubmit:enabled').focus();
             
          });
     </script>
 </asp:Content>
  <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
      <br />
       <br />
      <br />

       <table width="99%">
          <tr>
              <td align ="left" width="15%"> 
                  <telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" Height="55" ImageStorageLocation="Cache" AutoAdjustImageControlSize="false" />
              </td>
              <td align ="left"> 
                   <asp:Label ID="lblVersion" runat="server" Text="Pallet Link" Font-Size="XX-Large" ForeColor="#0033CC" Font-Bold="True" ></asp:Label>
              </td>
              <td align ="right"> 
                  <asp:Label ID="lblProcess" runat="server" Text="Process &nbsp;" Font-Size="XX-Large" ForeColor="#0033CC" Font-Bold="True" ></asp:Label>
              </td>

          </tr>

      </table>
      
   
     
    <hr />


      
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Metro" EnableEmbeddedSkins="false" >
    </telerik:RadAjaxLoadingPanel>
              <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Modal="true" Behaviors="Close"
        Skin="WebBlue" EnableEmbeddedSkins="false">
    </telerik:RadWindowManager>

      <asp:Panel ID="pnlCliente" class="panel panel-primary" runat="server" style="margin-bottom: 10px; margin-top: 9px;" Visible="False" HorizontalAlign="Center" Font-Bold="True" Font-Size="X-Large">
       
          <br />
          <br />
          <br />
           <asp:Label ID="Label1" runat="server" Text="Cliente:" Font-Bold="True" Font-Size="X-Large" ForeColor="Blue" ></asp:Label>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <asp:DropDownList ID="cmbSelectCustomer1" runat="server" Font-Size="Large" Width="334px" AutoPostBack="True" OnSelectedIndexChanged="cmbSelectCustomer1_SelectedIndexChanged"></asp:DropDownList>
          <br />
          <br />
           <asp:Label ID="lblSelectProcess" runat="server" Text="Proceso:" Font-Bold="True" Font-Size="X-Large" ForeColor="Blue" ></asp:Label>
          &nbsp;&nbsp;&nbsp;&nbsp;
          <asp:DropDownList ID="cmbProcess" runat="server" Font-Size="Large" Width="334px"></asp:DropDownList>
          
          
          <br />
          <br />
       <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" Font-Size="Large"  />

          <br />   <br />
          <asp:GridView ID="gvCustomerTemp" runat="server" HorizontalAlign="Center" CellPadding="4" ForeColor="#333333" GridLines="None" Visible="False">
              <AlternatingRowStyle BackColor="White" />
              <EditRowStyle BackColor="#2461BF" />
              <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
              <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
              <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
              <RowStyle BackColor="#EFF3FB" />
              <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
              <SortedAscendingCellStyle BackColor="#F5F7FB" />
              <SortedAscendingHeaderStyle BackColor="#6D95E1" />
              <SortedDescendingCellStyle BackColor="#E9EBEF" />
              <SortedDescendingHeaderStyle BackColor="#4870BE" />
          </asp:GridView>

          </asp:Panel>
     
     
      <asp:Panel ID="pnlLinking" class="panel panel-primary" runat="server" style="margin-bottom: 10px; margin-top: 9px;" Visible="False">
         

     <div id="allpage">
        <%-- <div style="height: 95px; text-align: center; vertical-align: top;">--%>
            
         <table style="table-layout: fixed; empty-cells: show" width="100%">
             <tr valign="top"  >
                 <td width="33.333%" style="visibility: visible">
                     
                 </td>
                 <td width="33.333%" align="center">
                     
                            
                 </td>
                <td width="33.333%">
                   
                        <asp:GridView ID="gvEscalation" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="1" Font-Size="Small" Height="50px"   HorizontalAlign="Right" float="none" Width="500px"  Caption="Ruta de escalación ?" CaptionAlign="Bottom">
                            <Columns>
                                <asp:BoundField DataField="PKLinkPath" HeaderText="PKLinkPath" Visible="false" />
                                <asp:BoundField DataField="FKCustomer" HeaderText="FKCustomer" Visible="false" />
                                <asp:BoundField DataField="FKLevel" HeaderText="Nivel" Visible="true" />
                                <asp:BoundField DataField="Shift" HeaderText="Turno" Visible="true" />
                                <asp:BoundField DataField="Name" HeaderText="Nombre" Visible="true" />
                                <asp:BoundField DataField="Phone" HeaderText="Teléfono" Visible="true" />
                                <asp:BoundField DataField="User" HeaderText="FKUserUpdater" Visible="false" />
                                <asp:BoundField DataField="LastUpdated" HeaderText="LastUpdated" Visible="false" />
                                <asp:BoundField DataField="Available" HeaderText="Available" Visible="false" />
                            

                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#005288" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                           <%-- <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />--%>
                        </asp:GridView>


                 </td>

             </tr>

         </table>
                     
                         
                                
              

            <%--<asp:Label ID="lblUser" runat="server" style="float:right"></asp:Label>--%>
        <%--<asp:Image ID="imgUser" runat="server" Height="16px"
            ImageUrl="~/Images/user.png" style="float:right " Width="18px" />--%>
         
                 

       <%--</div>--%>
       
       
         </div>
     <asp:Panel ID="pnlSN" class="panel panel-primary" runat="server" style="margin-bottom: 10px; margin-top: 9px;" Visible="False">
    <div id="feature">

         <table class="text-center" align="center" width="80%">
             <tr>
                
                
                 <td align="center" style="font-weight: bold" >

                     Numero de Serie del Pallet:
                      &nbsp;&nbsp; &nbsp;

                     <asp:TextBox ID="txtSerial" runat="server" AutoPostBack="True" ontextchanged="txtGRN_TextChanged" style="text-align: center" Width="200px" ></asp:TextBox>

                     &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;

                     <asp:LinkButton ID="btnClean" runat="server" onclick="btnClean_Click">Limpiar </asp:LinkButton>
                     
                     &nbsp;&nbsp; &nbsp;&nbsp;

                     <asp:LinkButton ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" >Regresar </asp:LinkButton>
                     
                     
                     <br />

                 </td>
         
             </tr>
             <tr>
                 <td align="center" colspan="2">
                     
                     
                      <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="25px" ForeColor="#FF6000" ></asp:Label>

                     <br />
                    
                 </td>

             </tr>

         </table>
        
      
        <%--   <tr style="align-content:center">
               
                    <td class="tdGrid">--%>
                        <asp:GridView ID="gvFixtureData" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="1" Font-Size="Small" GridLines="Horizontal" Height="80px" HorizontalAlign="Center" onrowdatabound="gvFixtureData_RowDataBound" Width="800px">
                            <Columns>
                                
                                <asp:BoundField DataField="PKToolingID" HeaderText="ID"  Visible ="true"/>
                                <asp:BoundField DataField="GRN" HeaderText="GRN" Visible ="false"/>
                                <asp:BoundField DataField="FKStatusID" HeaderText="FKStatusID" Visible ="false"/>
                                <asp:BoundField DataField="Description" HeaderText="Estatus" />
                                <asp:BoundField DataField="Quantity" HeaderText="Total de pallets" />
                                <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" Visible ="false"/>
                                <asp:BoundField DataField="CurrentWash" HeaderText="Ciclos de lavado" />
                                <asp:BoundField DataField="LimitWash" HeaderText="LimitWash" >
                                    <ItemStyle CssClass="Hide" />
                                    <HeaderStyle CssClass="Hide" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrentMain" HeaderText="Ciclos de mantenimientos" />
                                <asp:BoundField DataField="LimitMain" HeaderText="LimitMain"  >
                                    <ItemStyle CssClass="Hide" />
                                    <HeaderStyle CssClass="Hide" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LastDateWash" HeaderText="Fecha de lavado" />
                                <asp:TemplateField HeaderText="Mantenimiento">
                                    <ItemTemplate>
                                        <asp:Image ID="imgMtto" runat="server" ImageUrl=""  Width="25px" Height ="25px"/>
                                    </ItemTemplate>
                                    <HeaderStyle Width="32px" />
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#005288" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                         <%--   <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                   --%>     </asp:GridView>
                        <%--<asp:Button ID="Button1" runat="server" BackColor="WhiteSmoke" BorderColor="WhiteSmoke" BorderStyle="None" ForeColor="WhiteSmoke" Height="2px" Width="2px" />--%></td>
               <%-- </td>
           
            </tr>--%>
    <%--</table>--%>
</div>
</asp:Panel>
  
     <asp:Panel ID="pnlCustomer" runat="server" >

<div id="feature2" align="center" style="background-color: #FF0000; font-weight: bold; color: #FFFFFF;">
   <asp:Label ID="lblMessage1" runat="server" ForeColor="White" Font-Size="25pt"></asp:Label> 
                 
</div>
    
</asp:Panel>
  
     <asp:Panel ID="pnlUnitsInfo" class="panel panel-primary" runat="server" style="margin-bottom: 10px; margin-top: 9px;" Visible="False">
    <div id="feature">

         <table class="text-center" align="center" width="90%">
             <tr>
                 <td style="font-weight: bold" align="center">

                     <asp:Panel ID="pnlPiezas" runat="server">
                           Piezas:
                           &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;
                          <asp:TextBox ID="txtUnit" runat="server" AutoPostBack="true" style="text-align: center" Width="330px" 
                              OnTextChanged="txtUnit_TextChanged" EnableViewState="true" ViewStateMode="Enabled"></asp:TextBox>
                           &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                          <asp:ImageButton ID="imgBtnLink" runat="server" Height="30px" ImageAlign="Top" ImageUrl="~/Pallet_Link/images/link.png" Width="126px" OnClick="imgBtnLink_Click" Visible="false" focus="False" Focusable="false" />
                         </asp:Panel>

                 </td>
           
             </tr>
                            
             <tr>

                 <td align="center" >

                  
                      <div style="overflow-x:auto;width:1500px">
                      <br />
                        <asp:GridView ID="gvUnitsInfo" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="1" Font-Size="Small"
                        Height="50px" float="none" OnRowDataBound="gvUnitsInfo_RowDataBound" AutoGenerateColumns="True">
                            
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#005288" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" VerticalAlign="Middle"  />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        <%--    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                       --%>
                       
                      
                        </asp:GridView>
                   
                     </div>


                 </td>

             </tr>
             

               <tr>
                 <td align="center" >
                     <br />
                              <asp:Label ID="lblMessage0" runat="server" ForeColor="Red" Font-Size="Large" style="width:100%"></asp:Label>
                    <br />
                     </td>
              </tr>
              <tr>
                 <td align="center" >
                             <asp:Button ID="btnOK" runat="server" Text="OK" Width="95px" Height="26px" Visible="False" OnClick="btnOK_Click"  />
         
                     </td>
              </tr>
              <tr>
                 <td align="center" >
                             <asp:Button ID="btnLink" runat="server" Enabled="False" Text="Ligar" Width="95px" Height="26px" TabIndex="65" OnClick="btnLink_Click" Visible="False"  />
         
                     </td>
              </tr>
             <tr>
                       <td align="center" >
                           <asp:Image ID="ImgPalomitaOK" runat="server"  Height="200px" ImageAlign="AbsMiddle" ImageUrl="~/Pallet_Link/Images/palomita.png" Width="400px" Visible="False" />
                        </td>
                        
                   </tr>
              <tr>
                 <td align="center" >
                     <br />
                              <asp:Label ID="lblMessage2" runat="server" ForeColor="Green" Font-Size="XX-Large" style="width:100%" Font-Bold="True" Visible="False">¡ Ligado completo !</asp:Label>
                    <br />
                     </td>
              </tr>
         </table>
        
      
     </div>
      
    </asp:Panel>

         <br />

       <div id="HiddenFields">
             <table>
                 <tr>
                      <td >
                         <asp:HiddenField runat="server"  ID="HiddenCustomer" />
                     </td>
					 
                     <td >
                         <asp:HiddenField runat="server"  ID="HiddenIdCustomer" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenBahia" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenAssembly" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenMARouteID" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenRoute" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenRouteStepID" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenRouteStep" runat="server" />
                     </td>
                 </tr>
                 <tr>
                       <td>
                         <asp:HiddenField ID="HiddenPalletID" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenPrefix" runat="server" />
                     </td>
                      <td>
                         <asp:HiddenField ID="HiddenUser" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenEquipmentID" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenEquipment" runat="server" />
                     </td>
                      <td>
                         <asp:HiddenField ID="HiddenBoardAssembly" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="HiddenLastIndexCreated" runat="server" />
                     </td>
                     <td>
                       <asp:HiddenField ID="HiddenLastSerialScanned" runat="server" />
                      </td>   
                     <td>
                       <asp:HiddenField ID="HiddenPalletTimeout" runat="server" />
                      </td>  
                 </tr>
                  <tr>
                       <td>
                         <asp:HiddenField ID="Hidden_PKMachine" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="Hidden_Customer" runat="server" />
                     </td>
                      <td>
                         <asp:HiddenField ID="Hidden_CustomerMES" runat="server" />
                     </td>
                      <td>
                         <asp:HiddenField ID="Hidden_Process" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="Hidden_MesEquipment" runat="server" />
                     </td>
                     <td>
                         <asp:HiddenField ID="Hidden_RouteStep" runat="server" />
                     </td>
                       <td>
                         <asp:HiddenField ID="Hidden_CustomerPLID" runat="server" />
                     </td>
					   <td>
                         <asp:HiddenField ID="Hidden_LinkObject" runat="server" />                         
                     </td>
                       <td>
                         <asp:HiddenField ID="Hidden_PKID_Config" runat="server" />
                     </td>
                      </tr>
             </table>
         </div>
      

             </asp:Panel>
</asp:Content>


