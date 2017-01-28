<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="smsnotifaction.aspx.cs" Inherits="Admin_smsnotifaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="ckeditor/ckeditor.js" type="text/javascript"></script>
    <style>
        .msg {
            color: red;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdtotallemailnotification" runat="server" Value="0" />
    <asp:HiddenField ID="hdTotalMenu" runat="server" Value="0" />

    <!-- Main content -->
    <section class="content">

        <div class="row">
            <!-- /.col -->
            <div class="col-md-12">
                <div class="nav-tabs-custom">
                    <ul class="nav nav-tabs">
                        <li><a href="setup.aspx">General</a></li>
                        <li class="active"><a href="smsnotifaction.aspx" data-toggle="tab">SMS Notifications</a></li>
                        <li><a href="emailnotifaction.aspx">Email Notifications</a></li>
                        <li><a href="userrole.aspx">User Roles</a></li>
                        <li><a href="#regionsetting">Region Settings</a></li>
                        <li><a href="services.aspx">Services</a></li>
                        <li><a href="#storesetting">Store Settings</a></li>
                    </ul>
                    <div class="tab-content">

                        <!-- /.Genral tab-pane -->
                        <div class="tab-pane" id="General">
                            General

                        </div>

                        <!-- /.Sms notification tab-pane -->
                        <div class="active tab-pane" id="smsnotification">
                            <section class="content-header">
                                <h3>SMS Configurations<small></small></h3>
                                <ol class="breadcrumb"></ol>
                                <div class="label_radio">
                                    <asp:RadioButton ID="rbtenable" Text="Enable" GroupName="rbt" runat="server" />
                                    <asp:RadioButton ID="rbtdisabled" Text="Disabled" GroupName="rbt" runat="server" />
                                </div>
                            </section>
                            <div class="form-group">
                            </div>

                            <div class="form-group">
                                <label for="inputName" class="col-sm-2 control-label">Twilio SID :</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtSid" runat="server" class="form-control"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputName" class="col-sm-2 control-label">Twilio Auth Token :</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtToken" runat="server" class="form-control"></asp:TextBox>

                                </div>
                            </div>

                            <section class="content-header">
                                <h1>SMS Templates<small></small></h1>
                                <ol class="breadcrumb"></ol>

                            </section>
                            <div class="form-group">
                            </div>
                            <div class="col-md-12">

                                <div class="nav-tabs-custom">
                                    <asp:Literal ID="ltrtabbind" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrtabbinddata" runat="server"></asp:Literal>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-offset-0 col-sm-9">
                                        <asp:Button ID="btnsmsnotification" runat="server" class="btn btn-danger" Text="Confirm" OnClick="btnsmsnotification_Click" />
                                    </div>
                                </div>


                            </div>
                        </div>

                        <!-- /.Email Notification tab-pane -->
                        <div class="tab-pane" id="Email">
                            email notifaction
                        </div>

                        <!-- /.user roles tab-pane -->
                        <div class="tab-pane" id="userroles">
                            User role
                        </div>

                        <!-- /.region setting tab-pane -->
                        <div class="tab-pane" id="regionsetting">
                            regionsetting
                        </div>

                        <!-- /.Services tab-pane -->
                        <div class="tab-pane" id="Services">
                            Services
                        </div>

                        <!-- /.store setting tab-pane -->
                        <div class="tab-pane" id="storesetting">
                            storesetting
                        </div>

                    </div>
                    <!-- /.tab-content -->
                </div>
                <!-- /.nav-tabs-custom -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->

    </section>
    <!-- /.content -->
</asp:Content>

