<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="services.aspx.cs" Inherits="Admin_services" %>

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
                        <li><a href="smsnotifaction.aspx">SMS Notifications</a></li>
                        <li><a href="emailnotifaction.aspx">Email Notifications</a></li>
                        <li><a href="userroles.aspx">User Roles</a></li>
                        <li><a href="#regionsetting">Region Settings</a></li>
                        <li class="active"><a href="services.aspx" data-toggle="tab">Services</a></li>
                        <li><a href="#storesetting">Store Settings</a></li>
                    </ul>
                    <div class="tab-content">

                        <!-- /.Genral tab-pane -->
                        <div class="tab-pane" id="General">
                            General

                        </div>

                        <!-- /.Sms notification tab-pane -->
                        <div class="tab-pane" id="smsnotification">
                            <!-- The timeline -->
                            SMS Notifications
                        </div>

                        <!-- /.Email Notification tab-pane -->
                        <div class="tab-pane" id="Email">
                            email
                        </div>

                        <!-- /.user roles tab-pane -->
                        <div class="tab-pane" id="userroles">
                            User Role
                        </div>

                        <!-- /.region setting tab-pane -->
                        <div class="tab-pane" id="regionsetting">
                            regionsetting
                        </div>

                        <!-- /.Services tab-pane -->
                        <div class="active tab-pane" id="Services">
                            <section class="content-header">
                                <h1>Social Media<small></small></h1>
                                <ol class="breadcrumb"></ol>
                            </section>
                            <div class="form-group">
                                <asp:Label ID="lblservicesmsg" runat="server" class="col-sm-12 control-label msg"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label for="inputName" class="col-sm-2 control-label">Facebook :</label>

                                <div class="col-sm-6">
                                    <%--<input type="email" class="form-control" id="Email1" placeholder="Name">--%>
                                    <asp:TextBox ID="txtfacebook" runat="server" class="form-control"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputEmail" class="col-sm-2 control-label">Twitter :</label>

                                <div class="col-sm-6">
                                    <%--<input type="email" class="form-control" id="Email2" placeholder="Email">--%>
                                    <asp:TextBox ID="txttwitter" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <section class="content-header">
                                <h1>Adroll Settings<small></small></h1>
                                <ol class="breadcrumb"></ol>
                            </section>

                            <div class="form-group">
                                <label for="inputName" class="col-sm-2 control-label">Adroll adv_id :</label>

                                <div class="col-sm-6">
                                    <%--  <input type="text" class="form-control" id="Text2" placeholder="Name">--%>
                                    <asp:TextBox ID="txtadvid" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputExperience" class="col-sm-2 control-label">Adroll pix_id :</label>

                                <div class="col-sm-6">
                                    <%--<textarea class="form-control" id="Textarea1" placeholder="Experience"></textarea>--%>
                                    <asp:TextBox ID="txtpixid" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <section class="content-header">
                                <h1>Google Settings<small></small></h1>
                                <ol class="breadcrumb"></ol>
                            </section>

                            <div class="form-group">
                                <label for="inputSkills" class="col-sm-2 control-label">Google Analytics</label>

                                <div class="col-sm-6">
                                    <%--<input type="text" class="form-control" id="Text3" placeholder="Skills">--%>
                                    <asp:TextBox ID="txtgoogleanalytics" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputSkills" class="col-sm-2 control-label">Google maps :</label>

                                <div class="col-sm-6">
                                    <%--<input type="text" class="form-control" id="Text3" placeholder="Skills">--%>
                                    <asp:TextBox ID="txtgooglemaps" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>





                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-10">
                                    <%--<button type="submit" class="btn btn-danger">Submit</button>--%>
                                    <asp:Button ID="btnupdateservices" runat="server" class="btn btn-danger" Text="update" OnClick="btnupdateservices_Click" />
                                </div>
                            </div>


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

