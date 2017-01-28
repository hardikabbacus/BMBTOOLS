<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_menu.aspx.cs" Inherits="Admin_add_menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfprevsort" runat="server" />
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>     <asp:Label ID="ltrheading" runat="server"></asp:Label>
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
    </section>
    <!-- Main content -->
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
  <div class="col-md-12">
                <div class="box-body">

                    <!-- Horizontal Form -->
                    <div class="alert alert-danger alert-dismissible msgsucess" id="lblmsg" visible="false" runat="server">
                        <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                        <h4>
                            <i class="icon fa fa-ban"></i>
                            <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>
                        </h4>
                    </div>

                </div>
            </div>
            <!-- left column -->

            <!--/.col (left) -->
            <!-- right column -->
            <div class="col-md-12">
                <!-- Horizontal Form -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                           
                        </h3>
                    </div>

                  

                    <!-- /.box-header -->
                    <!-- form start -->
                    <form class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Menu Name </label>

                                <div class="col-sm-10">
                                    <%--  <input type="email" class="form-control" id="Email1" placeholder="First Name">--%>
                                    <asp:TextBox ID="txtmenuname" runat="server" class="form-control" ></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="rfvtitle" runat="server" ControlToValidate="txtmenuname" Display="Dynamic" ErrorMessage="Please enter menu name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group" style="display:block;">
                                <label for="inputPassword3" class="col-sm-2 control-label">Parent Menu </label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="ddlParentMenu" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Page Url </label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtPageName" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqtxtPageName" runat="server" ControlToValidate="txtmenuname" Display="Dynamic" ErrorMessage="Please enter page url" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                    <%--<asp:CustomValidator ID="CVpageurl" ControlToValidate="txtPageName" CssClass="Required"
                                        ClientValidationFunction="ValidPageUrl" ErrorMessage="CustomValidator" runat="server"
                                        ValidationGroup="btnsave" SetFocusOnError="true" Display="Dynamic" ValidateEmptyText="true" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Sort Order</label>

                                <div class="col-sm-10">
                                    <%--  <input type="email" class="form-control" id="inputEmail3" placeholder="Email">--%>
                                    <asp:TextBox ID="txtsortorder" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="rfvsortorder" runat="server"
                                        ControlToValidate="txtsortorder" Display="Dynamic" ErrorMessage="Please enter sort order"
                                        ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="Required" ForeColor="Red" ID="CVsortorder" Operator="DataTypeCheck"
                                        runat="server" Type="Integer" ControlToValidate="txtsortorder" ErrorMessage="Please Enter numeric value for Sort Order"
                                        Display="Dynamic" ValidationGroup="btnsave" EnableViewState="false"></asp:CompareValidator>

                                </div>
                            </div>

                            <%--<div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Images </label>
                                <div class="col-sm-10">
                                    <asp:FileUpload ID="flmenuimg" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="btnsave" EnableViewState="false" ErrorMessage="Please Select Image" ControlToValidate="flmenuimg" runat="server" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$"
                                        ControlToValidate="flmenuimg" runat="server" ValidationGroup="btnsave" ErrorMessage="Please select a valid Image File file."
                                        Display="Dynamic" />
                                </div>
                            </div>--%>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Image Name </label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtImageName" runat="server" class="form-control" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Is Active </label>
                                <div class="col-sm-10">
                                   <div class="label_check">
                                            <asp:CheckBox ID="chkvisible" Checked="true" runat="server" Text="NOTE:- Please tick this checkbox if you want to display on the website." />
                                            
                                      
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <%--  <button type="submit" class="btn btn-default">Cancel</button>--%>
                            <asp:Button ID="btncancel" runat="server" class="btn btn-default" Text="Back" OnClick="btncancel_Click" />
                            <%--  <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                            <asp:Button ID="btnsubmit" runat="server" class="btn btn-info pull-right" Text="Save" ValidationGroup="btnsave" OnClick="btnsubmit_Click" />
                        </div>
                        <!-- /.box-footer -->
                    </form>
                </div>
                <!-- /.box -->

            </div>
            <!--/.col (right) -->

        </div>

    </section>
    <!-- /.content -->
</asp:Content>

