<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="viewImportJobDetails.aspx.cs" Inherits="Admin_viewImportJobDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Job Details
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
                    <%--<div class="box-header">
                        <h3 class="box-title"></h3>
                    </div>--%>
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">



                                <div class="col-sm-12 search_bar">
                                    <div class="col-sm-10 jobdetails_head">
                                        <h1>
                                            <asp:Label ID="lblfilename" runat="server"></asp:Label></h1>
                                        <p>
                                            <asp:Label ID="lblinsertfilecount" runat="server"></asp:Label>
                                        </p>
                                    </div>

                                    <%--<div class="col-sm-2">
                                        <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>--%>
                                </div>

                                <div class="col-sm-12">
                                    <div class="col-sm-12 job_error">
                                        <asp:Literal ID="ltrunread" runat="server"></asp:Literal>
                                        <%--<asp:Label ID="lblunread" runat="server" Text="Unreadable Data Found :"></asp:Label>--%>
                                        <asp:Label ID="lblerrorlist" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-sm-12 search_bar">
                                    <div class="col-sm-4 job_dtls_btns">
                                        <asp:Button ID="imgbtnApproveJob" runat="server" Text="Approve Job" class="btn btn-info " OnClientClick="javascript:return CheckUserItemSelectionForUpdate();" OnClick="imgbtnApproveJob_Click" />
                                        <asp:Button ID="imgbtnCancelJob" runat="server" Text="Cancel Job" OnClick="imgbtnCancelJob_Click" class="btn btn-info " />
                                        <asp:Button ID="imgbtnBack" runat="server" Text="Back" OnClick="imgbtnBack_Click" class="btn btn-info " />
                                    </div>
                                    <%--<div class="col-sm-1">
                                        <asp:Button ID="imgbtnCancelJob" runat="server" Text="Cancel Job" class="btn btn-info pull-right" />
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Button ID="imgbtnBack" runat="server" Text="Back" class="btn btn-info pull-right" />
                                    </div>--%>
                                    <div class="col-sm-6">
                                    </div>
                                    <div class="col-sm-2">
                                        
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-sm-12 ">
                <div class="box">
                    <div class="box-header bdrbtm">
                        <div class="col-sm-10">
                            <h3 class="box-title">Import History</h3>
                        </div>

                        <div class="col-sm-2 cmn_show">
                            <label for="inputSkills" class="control-label">Show:</label>
                            <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>


                    <asp:GridView ID="gvImportjob" runat="server" AllowSorting="true" OnSorting="gvImportjob_Sorting" CssClass="table table-bordered table-striped dataTable"
                        GridLines="None" DataKeyNames="id" PagerStyle-CssClass="paging-link" role="grid"
                        AutoGenerateColumns="false" ShowFooter="false" OnRowDataBound="gvImportjob_RowDataBound"
                        PagerStyle-HorizontalAlign="Right" Width="100%">
                        <HeaderStyle CssClass="gridheader" />
                        <RowStyle CssClass="roweven" />
                        <AlternatingRowStyle CssClass="roweven" />
                        <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <div class="message error">
                                <h6>No record found for jobs.</h6>
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
                                    <asp:Label ID="lblErrorType" runat="server" Text='<%#Eval("FileErrorLineNumber") %>' Visible="false"></asp:Label>
                                    <div class="label_check">
                                        <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" Text=" " runat="server"></asp:CheckBox>
                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product Name" SortExpression="productname">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblsku" runat="server" Text='<%#Eval("sku") %>' Visible="false"></asp:Label>
                                    <%#Eval("productname")%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pending Action">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("id") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblAction" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField ID="hidsku" runat="server" Value='<%#Eval("sku") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Line status">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hidstatus" runat="server" Value='<%# Convert.ToInt32(Eval("isStatus")) %>' />
                                    <asp:Label ID="lblstatus" runat="server" Text=''></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Description">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hiderrno" runat="server" Value='<%#Eval("FileErrorLineNumber") %>' />
                                    <asp:HiddenField ID="hiddescp" runat="server" Value='<%#Eval("FileError") %>' />
                                    <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerSettings Visible="true" Position="Bottom" Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />
                    </asp:GridView>

                    <div class="col-sm-12">
                        <div class="row">
                            <div class="col-sm-5">
                                <div aria-live="polite" role="status" id="example1_info" class="dataTables_info">
                                    <asp:Literal ID="ltrcountrecord" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="col-sm-7">
                                <div id="example1_paginate" class="dataTables_paginate paging_simple_numbers">
                                    <ul class="pagination pull-right">
                                        <asp:Literal ID="ltrpaggingbottom" runat="server"></asp:Literal>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="imgbtnDelete" runat="server" Visible="false" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" Text="Delete" />
                    </div>
                </div>

            </div>




        </div>
    </section>

    <script src="js/general.js"></script>
</asp:Content>

