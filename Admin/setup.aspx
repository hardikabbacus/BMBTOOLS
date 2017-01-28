<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="setup.aspx.cs" Inherits="Admin_setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="ckeditor/ckeditor.js" type="text/javascript"></script>
    <style>
        .msg {
            color: red;
            text-align: center;
        }
    </style>

  
    <script>
        $(document).ready(function () {
            var hdtab = $("#ContentPlaceHolder1_hdtab").val();
            //alert(hdtab);
            if (hdtab == "General") {
                $("#Generaltab").addClass("active");
                $("#General").addClass("active");
            }
            else if (hdtab == "Services") {
                $("#Servicestab").addClass("active");
                $("#Services").addClass("active");
            }
            else if (hdtab == "Email") {
                $("#Emailtab").addClass("active");
                $("#Email").addClass("active");
            }
            else if (hdtab == "smsnotification") {
                $("#smsnotificationtab").addClass("active");
                $("#smsnotification").addClass("active");
            }
            else if (hdtab == "userrole") {
                $("#userrolestab").addClass("active");
                $("#userroles").addClass("active");
            }
            else if (hdtab == "storesetting") {
                $("#storesettingtab").addClass("active");
                $("#storesetting").addClass("active");
            }

        });

        function checkCheckedBox() {
            $('input:checkbox.class').each(function () {
                var sThisVal = (this.checked ? $(this).val() : "");
            });
        }

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#ContentPlaceHolder1_imgMenu')
                        .attr('src', e.target.result)
                        .width(100)
                        .height(100);
                };

                reader.readAsDataURL(input.files[0]);
            }
        }

        function readURLStore(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#ContentPlaceHolder1_notfoundimg')
                        .attr('src', e.target.result)
                        .width(100)
                        .height(100);
                };

                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdtotallemailnotification" runat="server" Value="0" />
    <asp:HiddenField ID="hdtab" runat="server" Value="General" />
    <asp:HiddenField ID="hdTotalMenu" runat="server" Value="0" />

    <!-- Main content -->
    <section class="content setup_pg">

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

            <!-- /.col -->
            <div class="col-md-12">
                <div class="nav-tabs-custom">
                    <ul class="nav nav-tabs">
                        <li id="Generaltab"><a href="#General" data-toggle="tab">General</a></li>
                        <li id="smsnotificationtab"><a href="#smsnotification" data-toggle="tab">SMS Notifications</a></li>
                        <li id="Emailtab"><a href="#Email" data-toggle="tab">Email Notifications</a></li>
                        <li id="userrolestab"><a href="#userroles" data-toggle="tab">User Roles</a></li>
                        <li id="regionsettingtab"><a href="#regionsetting" data-toggle="tab">Region Settings</a></li>
                        <li id="Servicestab"><a href="#Services" data-toggle="tab">Services</a></li>
                        <li id="storesettingtab"><a href="#storesetting" data-toggle="tab">Store Settings</a></li>
                    </ul>
                    <div class="tab-content">

                        <!-- /.Genral tab-pane -->
                        <div class="tab-pane" id="General">
                            <div class="row">

                                <div class="col-md-6">

                                    <section class="content-header">
                                        <h1>Company Information<small></small></h1>
                                        <ol class="breadcrumb"></ol>
                                    </section>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Company Name</label>

                                        <div class="col-sm-9">
                                            <%--<input type="email" class="form-control" id="Email1" >--%>
                                            <asp:TextBox ID="txtcomanyname" runat="server" class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqcomname" runat="server" ForeColor="Red" ControlToValidate="txtcomanyname" Display="Dynamic" ErrorMessage="Please enter company name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail" class="col-sm-3 control-label">Street Address</label>

                                        <div class="col-sm-9">
                                            <%--<input type="email" class="form-control" id="Email2" >--%>
                                            <asp:TextBox ID="txtstreetaddress" runat="server" class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqstreetaddress" runat="server" ForeColor="Red" ControlToValidate="txtstreetaddress" Display="Dynamic" ErrorMessage="Please enter street address" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">City</label>

                                        <div class="col-sm-9">
                                            <%--  <input type="text" class="form-control" id="Text2" >--%>
                                            <asp:TextBox ID="txtcity" runat="server" class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqcity" runat="server" ForeColor="Red" ControlToValidate="txtcity" Display="Dynamic" ErrorMessage="Please enter city" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputExperience" class="col-sm-3 control-label">Country</label>

                                        <div class="col-sm-9">
                                            <%--<textarea class="form-control" id="Textarea1" ></textarea>--%>
                                            <asp:TextBox ID="txtcountry" runat="server" class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqcountry" runat="server" ForeColor="Red" ControlToValidate="txtcountry" Display="Dynamic" ErrorMessage="Please enter country" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputSkills" class="col-sm-3 control-label">Telephone</label>

                                        <div class="col-sm-9">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            <asp:TextBox ID="txttelephone" runat="server" class="form-control" onkeypress="return isPhoneNumber(event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqtelephone" runat="server" ForeColor="Red" ControlToValidate="txttelephone" Display="Dynamic" ErrorMessage="Please enter telephone" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputSkills" class="col-sm-3 control-label">Fax</label>

                                        <div class="col-sm-9">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            <asp:TextBox ID="txtfax" runat="server" class="form-control" onkeypress="return isPhoneNumber(event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqfax" runat="server" ForeColor="Red" ControlToValidate="txtfax" Display="Dynamic" ErrorMessage="Please enter fax" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputSkills" class="col-sm-3 control-label">Support Email</label>

                                        <div class="col-sm-9">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            <asp:TextBox ID="txtsupportemail" runat="server" class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqsupportemail" runat="server" ForeColor="Red" ControlToValidate="txtsupportemail" Display="Dynamic" ErrorMessage="Please enter support email" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group" >
                                        <label for="inputSkills" class="col-sm-3 control-label">Logo</label>

                                        <div class="col-sm-6">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            <asp:FileUpload ID="logoimg" onchange="readURL(this);" runat="server" />
                                        </div>
                                        <div class="col-sm-3" >
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            
                                                        <img id="imgMenu"  runat="server" width="100" height="100" />
                                              

                                     
                                           
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputSkills" class="col-sm-12 control-label abt_comp">About Company</label>

                                        <div class="col-sm-12">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            <asp:TextBox ID="txtaboutcompany" TextMode="MultiLine" runat="server" class="form-control ckeditor"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-offset-3 col-sm-9">
                                            <%--<button type="submit" class="btn btn-danger">Submit</button>--%>
                                            <asp:Button ID="btnsave" runat="server" class="btn btn-info pull-right" Text="Save" ValidationGroup="btnsave" OnClick="btnsave_Click" />
                                        </div>
                                    </div>

                                </div>

                            </div>

                        </div>

                        <!-- /.Sms notification tab-pane -->
                        <div class="tab-pane" id="smsnotification">
                            <!-- The timeline -->
                            <div class="row">

                                <div class="col-md-6">
                                    <section class="content-header sms_conf col-sm-12">
                                        <h3 class="col-sm-6">SMS Configurations<small></small></h3>
                                        <ol class="breadcrumb"></ol>
                                        <div class="label_radio col-sm-6">
                                            <asp:RadioButton ID="rbtenable" class="col-xs-6" Text="Enable" GroupName="rbt" runat="server" />
                                            <asp:RadioButton ID="rbtdisabled" class="col-xs-6" Text="Disabled" GroupName="rbt" runat="server" />
                                        </div>
                                    </section>
                                    <div class="form-group">
                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Twilio SID :</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtSid" runat="server" MaxLength="100" class="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Twilio Auth Token :</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtToken" runat="server"  MaxLength="100"  class="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                    <section class="content-header">
                                        <h1>SMS Templates<small></small></h1>
                                        <ol class="breadcrumb"></ol>

                                    </section>
                                    <div class="form-group">
                                    </div>

                                    <div class="nav-tabs-custom">
                                        <asp:Literal ID="ltrsmsTab" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrSmsTabbind" runat="server"></asp:Literal>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-offset-0 col-sm-12">
                                            <asp:Button ID="btnsmsnotification" runat="server" class="btn btn-info" Text="Confirm" OnClick="btnsmsnotification_Click" />
                                        </div>
                                    </div>


                                </div>
                            </div>
                        </div>

                        <!-- /.Email Notification tab-pane -->
                        <div class="tab-pane" id="Email">
                            <div class="row">

                                <div class="col-md-6">
                                    <section class="content-header">
                                        <h1>Email Configurations<small></small></h1>
                                        <ol class="breadcrumb"></ol>
                                    </section>
                                    <div class="form-group">
                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">From Email :</label>

                                        <div class="col-sm-9">
                                            <%--<input type="email" class="form-control" id="Email1" >--%>
                                            <asp:TextBox ID="txtfromemail" runat="server" MaxLength="100" class="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-md-12">
                                    <section class="content-header">
                                        <h1>Email Templates<small></small></h1>
                                        <ol class="breadcrumb"></ol>
                                    </section>
                                </div>
                                <div class="form-group">
                                </div>
                                <div class="col-md-12">

                                    <div class="nav-tabs-custom">
                                        <asp:Literal ID="ltrtabbind" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrtabbinddata" runat="server"></asp:Literal>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-offset-0 col-sm-9">
                                            <%--<button type="submit" class="btn btn-danger">Submit</button>--%>
                                            <asp:Button ID="btnconfirmemailnotification" runat="server" class="btn btn-info" Text="Confirm" OnClick="btnconfirmemailnotification_Click" />
                                        </div>
                                    </div>


                                </div>


                            </div>

                        </div>

                        <!-- /.user roles tab-pane -->
                        <div class="tab-pane" id="userroles">
                            <div class="row">
                                <div class="col-md-3">
                                    <!-- Horizontal Form -->
                                    <div class="box box-info">
                                        <div class="box-header with-border">
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                            <h3 class="box-title">Access Type</h3>
                                            <input type="button" id="btnAccess" class="btn btn-info pull-right" title="New Access Type" value="New Access Type" />
                                        </div>
                                        <!-- /.box-header -->
                                        <!-- form start -->

                                        <div class="box-body">

                                            <asp:ListBox ID="listusertype" runat="server" Visible="false"></asp:ListBox>

                                            <asp:DataList ID="UserTypelist" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="id" runat="server" Text='<%# Eval("adminTypeId") %>' Visible="false"></asp:Label>
                                                    <a href='<%# Eval("adminTypeId","setup.aspx?flag=edit&id={0}") %>'>
                                                        <%# Eval("typeName") %>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:DataList>


                                        </div>
                                        <!-- /.box-body -->


                                    </div>
                                    <!-- /.box -->

                                </div>

                                <!-- right column -->
                                <div class="col-md-6" id="divmenu" runat="server">
                                    <!-- Horizontal Form -->
                                    <div class="box box-info">
                                        <div class="box-header with-border">
                                            <asp:Label ID="Label2" runat="server"></asp:Label>
                                            <h3 class="box-title">User Access</h3>
                                        </div>
                                        <!-- /.box-header -->

                                        <!-- form start -->

                                        <form role="form">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-sm-12">
                                                        <asp:Literal ID="ltrMenus" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-footer">
                                                <%--<button id="btnsavechanges" runat="server" class="btn btn-info pull-left" title="SaveChanges">Save Chnages</button>--%>
                                                <asp:Button ID="btnsavechange" runat="server" CssClass="update" Text="Save Change" OnClick="btnsavechange_Click" />
                                            </div>
                                            <!-- /.box-footer -->
                                        </form>
                                    </div>
                                    <!-- /.box -->

                                </div>
                                <!--/.col (right) -->
                            </div>
                        </div>

                        <!-- /.region setting tab-pane -->
                        <div class="tab-pane" id="regionsetting">
                            regionsetting
                        </div>

                        <!-- /.Services tab-pane -->
                        <div class="tab-pane" id="Services">
                            <div class="row">

                                <div class="col-md-6">
                                    <section class="content-header">
                                        <h1>Social Media<small></small></h1>
                                        <ol class="breadcrumb"></ol>
                                    </section>
                                    <div class="form-group">
                                        <asp:Label ID="lblservicesmsg" runat="server" class="col-sm-12 control-label msg"></asp:Label>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Facebook :</label>

                                        <div class="col-sm-9">
                                            <%--<input type="email" class="form-control" id="Email1" >--%>
                                            <asp:TextBox ID="txtfacebook" runat="server" class="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail" class="col-sm-3 control-label">Twitter :</label>

                                        <div class="col-sm-9">
                                            <%--<input type="email" class="form-control" id="Email2" >--%>
                                            <asp:TextBox ID="txttwitter" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <section class="content-header">
                                        <h1>Adroll Settings<small></small></h1>
                                        <ol class="breadcrumb"></ol>
                                    </section>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Adroll adv_id :</label>

                                        <div class="col-sm-9">
                                            <%--  <input type="text" class="form-control" id="Text2" >--%>
                                            <asp:TextBox ID="txtadvid" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputExperience" class="col-sm-3 control-label">Adroll pix_id :</label>

                                        <div class="col-sm-9">
                                            <%--<textarea class="form-control" id="Textarea1" ></textarea>--%>
                                            <asp:TextBox ID="txtpixid" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <section class="content-header">
                                        <h1>Google Settings<small></small></h1>
                                        <ol class="breadcrumb"></ol>
                                    </section>

                                    <div class="form-group">
                                        <label for="inputSkills" class="col-sm-3 control-label">Google Analytics</label>

                                        <div class="col-sm-9">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            <asp:TextBox ID="txtgoogleanalytics" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputSkills" class="col-sm-3 control-label">Google maps :</label>

                                        <div class="col-sm-9">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            <asp:TextBox ID="txtgooglemaps" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <%--<button type="submit" class="btn btn-danger">Submit</button>--%>
                                            <asp:Button ID="btnupdateservices" runat="server" class="btn btn-info col-sm-2" Text="Update" OnClick="btnupdateservices_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <!-- /.store setting tab-pane -->
                        <div class="tab-pane" id="storesetting">
                           
                             <div class="row">

                                <div class="col-md-8">
                                    <section class="content-header sms_conf col-sm-12">
                                        <h3 class="col-sm-6">Store Settings<small></small></h3>
                                    </section>
                                    <div class="form-group">
                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Product Listing:</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtproductlisting" runat="server" class="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Categories :</label>
                                       
                                           
                                        <div class="label_radio col-sm-9">
                                            <div class="setup_radio"><asp:RadioButton ID="rbtwithimg" value="1"  Text="With Image Thumbneil" GroupName="rbtcat" runat="server" /></div>
                                            <div class="setup_radio"><asp:RadioButton ID="rbtwithoutimg" value="2"   Text="Without Thumbneil" GroupName="rbtcat" runat="server" /></div>
                                            <div class="setup_radio"><asp:RadioButton ID="rbtcommanimg" value="3" Text="Common Image" GroupName="rbtcat" runat="server" /></div>
                                        </div>

                                    
                                    </div>

                                     <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Sorting Option :</label>
                                       
                                           
                                        <div class="label_check col-sm-9">
                                            <asp:CheckBox ID="chknewpopular" value="1" runat="server" Text="New & Popular"></asp:CheckBox>
                                            <asp:CheckBox ID="chkpricelowhigh" value="2" runat="server" Text="Price Low - High"></asp:CheckBox>
                                            <asp:CheckBox ID="chkpricehighlow" value="3" runat="server" Text="Price High - Low"></asp:CheckBox>
                                            <asp:CheckBox ID="chkoldestfirst" value="4" runat="server" Text="Oldest First"></asp:CheckBox>
                                            
                                        </div>

                                    
                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Filter Options:</label>
                                       
                                           
                                        <div class="label_radio col-sm-9">
                                            <div class="setup_radio_full"><asp:RadioButton ID="rbtpricerange"   value="1" Text="Price Range" GroupName="rbtfilter" runat="server" /></div>
                                            <div class="setup_radio_full"><asp:RadioButton ID="rbtcategory"  value="2" Text="Category" GroupName="rbtfilter" runat="server" /></div>
                                        </div>

                                    
                                    </div>
                                      <div class="form-group">
                                        <label for="inputName" class="col-sm-3 control-label">Image Not Found:</label>
                                        <div class="col-sm-6">
                                            <asp:FileUpload ID="imgnotfound" onchange="readURLStore(this);" runat="server" />
                                        </div>
                                          <div class="col-sm-3">
                                            <%--<input type="text" class="form-control" id="Text3" >--%>
                                            
                                                        <img id="notfoundimg" runat="server" width="100" height="100" />
                                                
                                        </div>
                                    </div>
                                    

                                    <div class="form-group">
                                        <div class="col-sm-offset-0 col-sm-12" >
                                            <asp:Button ID="btnsoresetting"  runat="server" class="btn btn-info" Text="Confirm" OnClick="btnsoresetting_Click" />
                                        </div>
                                    </div>


                                </div>
                            </div>

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

    <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close">×</span>
                <h2>Add Role</h2>
            </div>
            <div class="modal-body">

                <section class="content">
                    <div class="row">

                        <!-- right column -->
                        <div class="col-md-12">
                            <!-- Horizontal Form -->
                            <div class="box box-info">
                                <div class="box-header with-border">
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <form class="form-horizontal">
                                    <div class="box-body">
                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">User Type:</label>

                                            <div class="col-sm-9">
                                                <input type="text" class="form-control" id="txtusertype" runat="server" />
                                                <div id="usermessage"></div>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputPassword3" class="col-sm-3 control-label">IsActive:</label>

                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="ddlUserActive" runat="server" class="form-control" data-placeholder="Select a Type">
                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                </asp:DropDownList>
                                                <div id="productbrandmsg"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-footer">
                                        <button id="btnPopAddUsertype" onclick="return addUserType();" class="btn btn-info pull-right">Add</button>
                                    </div>
                                    <!-- /.box-footer -->
                                </form>
                            </div>
                            <!-- /.box -->
                            <!-- general form elements disabled -->

                            <!-- /.box -->
                        </div>
                        <!--/.col (right) -->
                    </div>
                    <!-- /.row -->
                </section>

            </div>

        </div>

    </div>

    <script>
        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the button that opens the modal
        var btn = document.getElementById("btnAccess");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
        btn.onclick = function () {
            modal.style.display = "block";
        }

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }
    </script>


</asp:Content>

