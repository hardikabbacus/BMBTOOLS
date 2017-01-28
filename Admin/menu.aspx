<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="Admin_menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <section class="content-header">
        <h1>Menu Details
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

                                    <label for="inputSkills" class="col-sm-1 control-label">Search:</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtsearch" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Button ID="imgbtnSearch" runat="server" Text="Search" class="btn btn-info pull-right" OnClick="imgbtnSearch_Click" />
                                    </div>

                                </div>

                                <div class="col-sm-12">
                                    <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" OnSorting="gvAdmin_Sorting" CssClass="table table-bordered table-striped dataTable"
                                        GridLines="None" DataKeyNames="idmenu" PagerStyle-CssClass="paging-link" role="grid"
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
                                                <h6>No records found for menu.</h6>
                                            </div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle  />
                                                <HeaderTemplate >
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
                                            <asp:TemplateField HeaderText="Menu Name" SortExpression="title">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                   <a id="an" runat="server"  href='<%# "add_menu.aspx?flag=edit&id=" + Eval("idmenu") + ""%>'> <%#Eval("title")%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Parent Menu" SortExpression="parentMenu" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" />

                                                <ItemTemplate><%#Eval("parentMenu")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="URL" SortExpression="pageurl">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%#Eval("pageurl")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Image" SortExpression="imagepath">
                                                
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdmenuimage" runat="server" Value='<%# Eval("imagepath")%>'
                                                        Visible="false" />
                                                    <img id="imgMenu" runat="server" title='<%# Eval("title") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Sort Order" SortExpression="sortorder">

                                                <ItemTemplate>
                                                    <%#Eval("sortorder")%>
                                                </ItemTemplate>
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
                                    <%--<li id="example1_previous" class="paginate_button previous disabled"><a tabindex="0" data-dt-idx="0" aria-controls="example1" href="#">Previous</a></li>
                                    <li class="paginate_button active"><a tabindex="0" data-dt-idx="1" aria-controls="example1" href="#">1</a></li>
                                    <li class="paginate_button "><a tabindex="0" data-dt-idx="2" aria-controls="example1" href="#">2</a></li>
                                    <li class="paginate_button "><a tabindex="0" data-dt-idx="3" aria-controls="example1" href="#">3</a></li>
                                    <li class="paginate_button "><a tabindex="0" data-dt-idx="4" aria-controls="example1" href="#">4</a></li>
                                    <li class="paginate_button "><a tabindex="0" data-dt-idx="5" aria-controls="example1" href="#">5</a></li>
                                    <li class="paginate_button "><a tabindex="0" data-dt-idx="6" aria-controls="example1" href="#">6</a></li>
                                    <li id="example1_next" class="paginate_button next"><a tabindex="0" data-dt-idx="7" aria-controls="example1" href="#">Next</a></li>--%>
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

