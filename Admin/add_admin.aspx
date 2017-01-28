<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_admin.aspx.cs" Inherits="Admin_add_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdprevOrder" runat="server" />
    <asp:HiddenField ID="hdTotalMenu" runat="server" Value="0" />
    <asp:HiddenField ID="hdadminid" runat="server" Value="0" />
    <asp:HiddenField ID="hfpass" runat="server" />
    <asp:HiddenField ID="superadmin" runat="server" />

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>
            <asp:Label ID="ltrheading" runat="server"></asp:Label>
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
                        <h3 class="box-title"></h3>
                    </div>



                    <!-- /.box-header -->
                    <!-- form start -->
                    <form class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">First Name </label>

                                <div class="col-sm-10">
                                    <%--  <input type="email" class="form-control" id="Email1" placeholder="First Name">--%>
                                    <asp:TextBox ID="txtfname" runat="server" class="form-control" MaxLength="48"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtfname" ForeColor="Red" runat="server" ControlToValidate="txtfname" Display="Dynamic" ErrorMessage="Please enter first name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Last Name </label>

                                <div class="col-sm-10">
                                    <%--<input type="email" class="form-control" id="Email2" placeholder="Last Name">--%>
                                    <asp:TextBox ID="txtlname" runat="server" class="form-control" MaxLength="48"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtlname" runat="server" ForeColor="Red" ControlToValidate="txtlname" Display="Dynamic" ErrorMessage="Please enter last name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Email</label>

                                <div class="col-sm-10">
                                    <%--  <input type="email" class="form-control" id="inputEmail3" placeholder="Email">--%>
                                    <asp:TextBox ID="txtemail" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtemail" runat="server" ForeColor="Red" ControlToValidate="txtemail" Display="Dynamic" ErrorMessage="Please enter email" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revtxtemail" runat="server" ControlToValidate="txtemail" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="The email address you entered appears to be incorrect.(Example: yourscreename@aol.com)" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="btnsave" EnableViewState="False"></asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Username </label>

                                <div class="col-sm-10">
                                    <%--<input type="email" class="form-control" id="Email3" placeholder="Username">--%>
                                    <asp:TextBox ID="txtusername" runat="server" class="form-control" MaxLength="40"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtusername" runat="server" ForeColor="Red" ControlToValidate="txtusername" Display="Dynamic" ErrorMessage="Please enter username" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Password</label>

                                <div class="col-sm-10">
                                    <%--<input type="password" class="form-control" id="inputPassword3" placeholder="Password">--%>
                                    <asp:TextBox ID="txtpassword" TextMode="Password" MaxLength="20" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtpassword" runat="server" ForeColor="Red" ControlToValidate="txtpassword" Display="Dynamic" ErrorMessage="Please enter password" ValidationGroup="btnsave"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Access Type</label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="ddladmintype" runat="server" class="form-control">
                                        <%--<asp:ListItem Value="0">Select Admin Type</asp:ListItem>
                                        <asp:ListItem Value="1">Admin</asp:ListItem>
                                        <asp:ListItem Value="2">Super Admin</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqddladmintype" runat="server" ControlToValidate="ddladmintype" ForeColor="Red" InitialValue="0" ErrorMessage="Please select admin type" Display="Dynamic" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>

                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Mobile</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtmobile" runat="server" MaxLength="20" CssClass="form-control" onkeypress="return isPhoneNumber(event)"></asp:TextBox>
                                </div>
                            </div>

                            <%-- <div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Is Active </label>
                                <div class="col-sm-10">
                                    <div class="label_check">

                                        <asp:CheckBox ID="chkisactive" Checked="true" runat="server" Text=" NOTE:- Please tick this checkbox if you want to display on the website." />

                                    </div>
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Is Active</label>
                                <div class="col-sm-1">
                                    <div class="label_radio">
                                        <asp:RadioButton ID="rbtActive" Text="Yes" value="1" GroupName="rbt" Checked="true" runat="server" />
                                    </div>
                                </div>

                                <div class="col-sm-1">
                                    <div class="label_radio">
                                        <asp:RadioButton ID="rbtInactive" Text="No" value="0" GroupName="rbt" runat="server" />
                                    </div>
                                </div>
                                <div id="reqvarient" style="color: red"></div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <label for="inputPassword3" class="col-sm-2 control-label">Grant Access </label>
                                <div class="col-sm-10">
                                    <div class="checkbox">
                                        <%--<label>
                                            <asp:CheckBox ID="chkgrantaccess" runat="server" />
                                            Grant Access 
                                            <asp:Literal ID="ltrMenus" runat="server"></asp:Literal>
                                        </label>--%>
                                        <asp:Literal ID="ltrMenus" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <%--  <button type="submit" class="btn btn-default">Cancel</button>--%>
                            <%--  <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                            <asp:Button ID="btnsubmit" runat="server" class="btn btn-info pull-right" Text="Save" ValidationGroup="btnsave" OnClick="btnsubmit_Click" />
                            <asp:Button ID="btncancel" runat="server" class="btn btn-default" Text="Back" OnClick="btncancel_Click" />
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

