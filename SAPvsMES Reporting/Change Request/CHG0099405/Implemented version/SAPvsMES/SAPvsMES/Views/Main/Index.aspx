<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Main.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<script runat="server">

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://mxchim0web06/DashboardSAPvsMES/Aspx/SAPvsMESDashboard.aspx");
    }

</script>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    SAPvsMES
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!--DataTables JS-->
    <!--DataTables JS-->
    <script src="js/DataTables/jquery.dataTables.min.js"></script>
    <script src="js/DataTables/dataTables.bootstrap.min.js"></script>
    <script src="js/DataTables/dataTables.buttons.min.js"></script>
    <script src="js/DataTables/buttons.bootstrap.min.js"></script>
    <script src="js/DataTables/jszip.min.js"></script>
    <script src="js/DataTables/pdfmake.min.js"></script>
    <script src="js/DataTables/vfs_fonts.js"></script>
    <script src="js/DataTables/buttons.html5.min.js"></script>
    <script src="js/DataTables/buttons.print.min.js"></script>

    <script src="js/DatePicker/flatpickr2-3-4.js"></script>
    <script type ="text/javascript">
        $(document).ready(function () {
            $("#ddlCustomers").change(function (evt) {
                $("#hfCustomerText").val($("#ddlCustomers option:selected").text());
            });
        });
    </script>

    <script type="text/javascript">
        function redirectTo() {
            window.location = "http://mxchim0web06/DashboardSAPvsMES/Aspx/SAPvsMESDashboard.aspx";
        }
    </script>

    <div class="col-lg-12">
        <div class="block block-themed">
            <div class="block-header bg-primary">
                <h3 class="block-title">SAP vs MES</h3>
            </div>
            <div class="block-content border-black-op">
                <form class="form-horizontal border-black-op-b" action="Main" method="post">
                    <div class="form-group">
                        <div class="dropdown">
                            <%=Html.DropDownList("ddlCustomers", new SelectList(ViewBag.Customers, "Value", "Text", ViewBag.selectedCustomer),"Select Customer", 
                            new { @id = "ddlCustomers", @class = "btn btn-primary dropdown-toggle", @EnableViewState="true"})%>
                            <%=Html.Hidden("hfCustomerText", null, new { @id = "hfCustomerText" }) %>
                            <button class="btn btn-primary" id="btnSubmit"><i class=""></i>Submit</button>
                            <input type="hidden" value="submit" name="txtHiddenSubmit">
                            <input type="button" class="btn btn-primary" id="btnDashboard" value="Dashboard" onclick="redirectTo();" style="<%=ViewBag.dashBoardVisible%>"/> 
                        </div>         
                    </div>
                </form>
                    <br />
                    <div class="table-responsive">
                        <%
                            if (Model.dt != null)
                            {
                                tblSAPvsMES.DataSource = Model.dt;
                                tblSAPvsMES.DataBind();
                                tblSAPvsMES.HeaderRow.TableSection = TableRowSection.TableHeader;

                            }
                        %>
                        <form runat="server">
                            <asp:GridView ID="tblSAPvsMES" CssClass="table table-striped table-bordered js-dataTable-full" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField HeaderText="Model" DataField="MANTR" />
                                    <asp:BoundField HeaderText="SAP Qty" DataField="SAP_Qty" />
                                    <asp:HyperLinkField HeaderText="MES Qty"
                                        DataTextField="MESQty"
                                        DataNavigateUrlFields="MANTR"
                                        DataNavigateUrlFormatString="http://mxchim0web05/eDashboard_CUU/_webforms/rep_mes_birthaging.aspx?a={0}" />
                                    <asp:BoundField HeaderText="Delta" DataField="Delta" />
                                    <asp:BoundField HeaderText="%Dif" DataField="%Dif" />
                                    <asp:BoundField HeaderText="Last Updated" DataField="LastUpdated" />
                                </Columns>
                            </asp:GridView>
                        </form>                     
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
