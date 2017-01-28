<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="viewadmin.aspx.cs" Inherits="Admin_viewadmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            SearchKeywordManageUser();
        });

        function SearchKeywordManageUser() {
            $("#ContentPlaceHolder1_txtsearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/SearchKeywordManageUser",
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <section class="content-header">
        <h1>Manage User
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <asp:Button ID="btnadd" runat="server" class="btn btn-info pull-right" Text="Add New" OnClick="btnadd_Click" />
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



            <div class="col-xs-12">
                <div class="box">
                    <div class="box-header">
                        <h3 class="box-title">Search</h3>
                    </div>



                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">

                                <div class="col-sm-12 search_bar">
                                    <div class="col-sm-11">
                                        <asp:TextBox ID="txtsearch" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Button ID="imgbtnSearch" runat="server" Text="Search" class="btn btn-info pull-right" OnClick="imgbtnSearch_Click" />
                                    </div>

                                </div>

                            </div>

                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="box">
                            <div class="box-header bdrbtm">
                                <div class="col-sm-10">
                                    <h3 class="box-title">User List</h3>
                                </div>

                                <div class="col-sm-2 cmn_show">
                                    <label for="inputSkills" class="control-label">Show:</label>
                                    <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-12">

                                <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" OnSorting="gvAdmin_Sorting" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="adminid" PagerStyle-CssClass="paging-link" role="grid"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    OnPageIndexChanging="gvAdmin_PageIndexChanging"
                                    PagerStyle-HorizontalAlign="Right" Width="100%"
                                    OnRowDataBound="gvAdmin_RowDataBound">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for admin.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle Width="50" />
                                            <HeaderTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" runat="server" Text=" "></asp:CheckBox>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" runat="server" Text=" "></asp:CheckBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="firstname">

                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <a runat="server" href='<%# "add_admin.aspx?flag=edit&id=" + Eval("adminid") + ""%>'><%# Server.HtmlEncode(Eval("firstname").ToString()) + " " + Server.HtmlEncode(Eval("lastname").ToString())%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email Address" SortExpression="emailaddress">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <a runat="server" href='<%# "add_admin.aspx?flag=edit&id=" + Eval("adminid") + ""%>'><%#Eval("emailaddress")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile Number" SortExpression="mobile">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Literal ID="ltruserid" runat="server" Text='<%#Eval("mobile")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Access Type" SortExpression="typeName">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Literal ID="ltruseridesstype" runat="server" Text='<%#Eval("typeName")%>'></asp:Literal>
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
                                    <asp:Button ID="imgbtnDelete" runat="server" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="imgbtnDelete_Click" Text="Delete" />
                                    <%--<asp:Button ID="btnadd" runat="server" class="btn btn-info pull-right" Text="Add New" OnClick="btnadd_Click" />--%>

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
                        <asp:AsyncPostBackTrigger ControlID="imgbtnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
    </section>
    <script src="js/general.js"></script>
</asp:Content>

