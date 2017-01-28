<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="viewusertype.aspx.cs" Inherits="Admin_viewusertype" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <section class="content-header">
        <h1>User Type
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
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
                       
                    </div>

                   

                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12 search_bar">

                                    <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" OnSorting="gvAdmin_Sorting" CssClass="table table-bordered table-striped dataTable"
                                        GridLines="None" DataKeyNames="adminTypeId" PagerStyle-CssClass="paging-link" role="grid"
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
                                                <h6>No record found for user type.</h6>
                                            </div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle Width="50"  />
                                                <HeaderTemplate>
                                                      <div class="label_check">
                                                    <asp:CheckBox ID="chkHeader" Text=" " CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" runat="server"></asp:CheckBox>
                                                          </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                      <div class="label_check">
                                                    <asp:CheckBox ID="chkDelete" Text=" " CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" runat="server"></asp:CheckBox>
                                                          </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User Type" SortExpression="typeName">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                   <a id="an" runat="server"  href='<%# "add_usertype.aspx?flag=edit&id=" + Eval("adminTypeId") + ""%>'> <%# Server.HtmlEncode(Eval("typeName").ToString()) %></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Sort Order" SortExpression="sortorder ">
                                                <HeaderStyle HorizontalAlign="Left" />

                                                <ItemTemplate><%#Eval("sortorder")%></ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="IsActive" SortExpression="isactive">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkstatus" runat="server" Text='<%# Convert.ToInt32(Eval("isActive"))==1?"Active":"Inactive" %>'
                                                        CommandArgument='<%# Convert.ToInt32(Eval("isActive"))%>' OnClick="lnkStatus_click"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            
                                        </Columns>
                                        <PagerSettings Visible="true" Position="Bottom" Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />
                                    </asp:GridView>

                                </div>
                            </div>
                        </div>
                    </div>
                      <div class="pagi">
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
                        <asp:Button ID="btnadd" runat="server" class="btn btn-info pull-right" Text="Add New" OnClick="btnadd_Click" />
                    </div>

                </div>
            </div>
        </div>
    </section>
    <script src="js/general.js"></script>
</asp:Content>

