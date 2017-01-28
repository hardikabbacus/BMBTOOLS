<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="vieworder.aspx.cs" Inherits="Admin_vieworder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            SearchKeywordOrderid();
            SearchKeywordCustomer();
        });

        function SearchKeywordOrderid() {
            $("#ContentPlaceHolder1_txtsearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/SearchKeywordOrderid",
                        data: "{'SearchKey':'" + document.getElementById('ContentPlaceHolder1_txtsearch').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("No Match");
                        }
                    });
                },
                select: function (event, ui) {
                    var text = ui.item.value;
                    $('#ContentPlaceHolder1_txtsearch').val(text);
                    $('#ContentPlaceHolder1_txtsearch').trigger('click');
                }
            });
        }

        function SearchKeywordCustomer() {
            $("#ContentPlaceHolder1_txtcontact").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/SearchKeywordCustomer",
                        data: "{'SearchKey':'" + document.getElementById('ContentPlaceHolder1_txtcontact').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("No Match");
                        }
                    });
                },
                select: function (event, ui) {
                    var text = ui.item.value;
                    $('#ContentPlaceHolder1_txtcontact').val(text);
                    $('#ContentPlaceHolder1_txtcontact').trigger('click');
                }
            });
        }

        //On Page Load
        $(function () {
            $("#ContentPlaceHolder1_ddlsesrchCategory").select2();
            //$("#tabs").tabs();
            //$(".select2").select2();
        });

        //On UpdatePanel Refresh
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //alert(prm);
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        //alert('tst');
                        $("#ContentPlaceHolder1_ddlsesrchCategory").select2();
                        //$("#tabs").tabs();
                    }
                });
            };
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>



    <section class="content-header">

        <h1>Orders
            <small></small>
        </h1>

        <ol class="breadcrumb"></ol>
        <a href="add_order.aspx" class="btn btn-info pull-right">New Order</a>
    </section>



    <!-- Main content -->
    <section class="content">
        <div class="row">

            <div class="col-md-12">
                <div class="box-body">

                    <!-- Horizontal Form -->
                    <div class="alert alert-success alert-dismissible msgsucess" id="lblmsg" visible="false" runat="server">
                        <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                        <h4>
                            <i class="icon fa fa-check"></i>
                            <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>
                        </h4>
                    </div>
                </div>
            </div>

            <div class="col-xs-8">
                <div class="box">
                    <div class="box-header">
                        <div class="col-sm-12">
                            <h3 class="box-title">Search</h3>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12 search_bar">
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlstatus" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtsearch" CssClass="form-control " placeholder="Enter Order Number" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtcontact" runat="server" placeholder="Enter Customer Name" CssClass="form-control cstm_search"></asp:TextBox>
                                        <asp:Button ID="imgbtnSearch" runat="server" class="btn btn-info pull-right " OnClick="imgbtnSearch_Click" Text="Search" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlchangestatus" CssClass="form-control cstm_search_d" Width="200" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="Required" ID="reqddlchangestatus" ForeColor="Red" runat="server"
                                            ControlToValidate="ddlchangestatus" InitialValue="0" Display="Dynamic" ErrorMessage="Please select status"
                                            ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btnApplyStatus" OnClientClick="javascript:return CheckUserItemSelectionForOrderStatus();" ValidationGroup="btnsave" runat="server" OnClick="btnApplyStatus_Click" class="btn btn-info pull-right" Text="Apply" />
                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="col-xs-4">
                <div class="box">
                    <div class="box-header">
                        <div class="col-sm-12">
                            <h3 class="box-title">Timeframe</h3>
                        </div>
                        <div class="col-sm-12">
                            <div class="sec1">
                                <div class="spec">
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk2015" runat="server" AutoPostBack="true" OnCheckedChanged="chk2015_CheckedChanged" /><p id="PervYear" runat="server">
                                            <%--<label id="PervYear" runat="server"></label>--%>
                                        </p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk2016" runat="server" AutoPostBack="true" OnCheckedChanged="chk2016_CheckedChanged" /><p id="currYear" runat="server">
                                            <%--<label id="currYear" runat="server"></label>--%>
                                        </p>
                                    </label>
                                </div>
                            </div>
                            <div class="sec2">
                                <div class="spec">
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk1" runat="server" AutoPostBack="true" OnCheckedChanged="chk1_CheckedChanged" /><p>1</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk2" runat="server" AutoPostBack="true" OnCheckedChanged="chk2_CheckedChanged" /><p>2</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk3" runat="server" AutoPostBack="true" OnCheckedChanged="chk3_CheckedChanged" /><p>3</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk4" runat="server" AutoPostBack="true" OnCheckedChanged="chk4_CheckedChanged" /><p>4</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk5" runat="server" AutoPostBack="true" OnCheckedChanged="chk5_CheckedChanged" /><p>5</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk6" runat="server" AutoPostBack="true" OnCheckedChanged="chk6_CheckedChanged" /><p>6</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk7" runat="server" AutoPostBack="true" OnCheckedChanged="chk7_CheckedChanged" /><p>7</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk8" runat="server" AutoPostBack="true" OnCheckedChanged="chk8_CheckedChanged" /><p>8</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk9" runat="server" AutoPostBack="true" OnCheckedChanged="chk9_CheckedChanged" /><p>9</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk10" runat="server" AutoPostBack="true" OnCheckedChanged="chk10_CheckedChanged" /><p>10</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk11" runat="server" AutoPostBack="true" OnCheckedChanged="chk11_CheckedChanged" /><p>11</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk12" runat="server" AutoPostBack="true" OnCheckedChanged="chk12_CheckedChanged" /><p>12</p>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="col-xs-12">
                        <div class="box">
                            <div class="box-header order_box_border bdrbtm">
                                <div class="col-sm-10">
                                    <h3 class="box-title">Order List</h3>
                                </div>

                                <div class="col-sm-2 cmn_show">
                                    <label for="inputSkills" class="control-label">Show:</label>
                                    <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="orderid" PagerStyle-CssClass="paging-link" role="grid" OnSorting="gvAdmin_Sorting"
                                    OnPageIndexChanging="gvAdmin_PageIndexChanging"
                                    OnRowDataBound="gvAdmin_RowDataBound"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    PagerStyle-HorizontalAlign="Right" Width="100%">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for order list.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle Width="50" />
                                            <HeaderTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" Text=" " onclick="javascript:SelectAllCheckboxes_1();" runat="server"></asp:CheckBox>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" Text=" " onclick="javascript:UnSelectHeaderCheckbox();" runat="server"></asp:CheckBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Order Number" SortExpression="orderid">
                                            <ItemStyle Width="130" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderid" runat="server" Text='<%#Eval("orderid") %>' Visible="false"></asp:Label>
                                                <a id="A1" runat="server" href='<%# "vieworderdetails.aspx?orderid=" + Eval("orderid") + "&customer="+ Eval("customerid")%>'><%#Eval("orderid")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Created At" SortExpression="CreatedDate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lbldate" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>--%>
                                                <a id="A2" runat="server" href='<%# "vieworderdetails.aspx?orderid=" + Eval("orderid") + "&customer="+ Eval("customerid")%>'><%#Eval("CreatedDate")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer" SortExpression="contactname">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblcontactname" runat="server" Text='<%#Eval("contactname") %>'></asp:Label>--%>
                                                <a id="A3" runat="server" href='<%# "vieworderdetails.aspx?orderid=" + Eval("orderid") + "&customer="+ Eval("customerid")%>'><%#Eval("contactname")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" SortExpression="orderstatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblstatus" runat="server" Text='<%#Eval("orderstatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" SortExpression="totalammount">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotal" runat="server" Text='<%#Eval("totalammount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Visible="true" Position="Bottom" Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />
                                </asp:GridView>
                                <div class="pagi col-sm-12">
                                    <div class="row">
                                        <div class="col-sm-5">
                                            <div aria-live="polite" role="status" id="example1_info" class="dataTables_info">
                                                <asp:Literal ID="ltrcountrecord" runat="server"></asp:Literal>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button ID="imgbtnDelete" runat="server" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" Visible="false" Text="Delete" />

                                    <div class="pagi">
                                        <div class="row">
                                            <div class="col-sm-7">
                                                <div id="example1_paginate" class="dataTables_paginate paging_simple_numbers">
                                                    <ul class="pagination">
                                                        <asp:Literal ID="ltrpaggingbottom" runat="server"></asp:Literal>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="chk2015" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk2016" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk1" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk2" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk3" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk4" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk5" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk6" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk7" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk8" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk9" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk10" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk11" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chk12" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="imgbtnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </section>



    <script type="text/javascript">
        $(document).ready(function () {
            $('.label_check input:checkbox').on('click', function () {
                if ($(this).is(":checked")) {
                    $(this).parent().addClass("menuitemshow");
                } else {
                    $(this).parent().removeClass("menuitemshow");
                }
            });

            $('.label_check input:radio').click(function () {
                $('.label_check input:radio[name=' + $(this).attr('name') + ']').parent().removeClass('menuitemshow');
                $(this).parent().addClass('menuitemshow');
            });
        });
    </script>

    <script src="js/general.js"></script>

</asp:Content>

