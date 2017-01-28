<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="viewcontent.aspx.cs" Inherits="Admin_viewcontent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <section class="content-header">
        <h1>CMS
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="box">



                            <div class="box-header">
                                <h3 class="box-title">Search</h3>
                            </div>

                            <div class="box-body">
                                <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                                    <div class="row">

                                        <div class="col-sm-12 search_bar">
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtsearch" CssClass="form-control" placeholder=" Name" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button ID="imgbtnSearch" runat="server" class="btn btn-info pull-right" Text="Search" OnClick="imgbtnSearch_Click" />
                                            </div>
                                            <label for="inputSkills" class="col-sm-1 control-label">Show:</label>
                                            <div class="col-sm-1">
                                                <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Content List</h3>
                            </div>
                            <div class="col-sm-12 ">

                                <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" OnSorting="gvAdmin_Sorting" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="Id" PagerStyle-CssClass="paging-link" role="grid"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    OnPageIndexChanging="gvAdmin_PageIndexChanging"
                                    PagerStyle-HorizontalAlign="Right" Width="100%"
                                    OnRowDataBound="gvAdmin_RowDataBound" OnRowEditing="gvAdmin_RowEditing" OnRowUpdating="gvAdmin_RowUpdating" OnRowCancelingEdit="gvAdmin_RowCancelingEdit">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for CMS.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle Width="50" />
                                            <HeaderTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" Text=" " runat="server"></asp:CheckBox>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" Text=" " runat="server"></asp:CheckBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Content Name" SortExpression="ContentTitle">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <a id="an" runat="server" href='<%# "add_content.aspx?flag=edit&id=" + Eval("Id") + ""%>'><%#Eval("ContentTitle")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Statue" SortExpression="DisplayStatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%--<%# Server.HtmlEncode(Eval("DisplayStatus").ToString())%>--%>
                                                <asp:LinkButton ID="lnkstatus" runat="server" Text='<%# Convert.ToInt32(Eval("DisplayStatus"))==1?"Active":"Inactive" %>'
                                                    CommandArgument='<%# Convert.ToInt32(Eval("DisplayStatus"))%>' OnClick="lnkStatus_click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SortOrder" SortExpression="DisplayRank">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("DisplayRank").ToString())%>
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
                                        <div class="col-sm-7">
                                            <div id="example1_paginate" class="dataTables_paginate paging_simple_numbers">
                                                <ul class="pagination">
                                                    <asp:Literal ID="ltrpaggingbottom" runat="server"></asp:Literal>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button ID="imgbtnDelete" runat="server" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="imgbtnDelete_Click" Text="Delete" />
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
    <script src="js/general.js"></script>
</asp:Content>

