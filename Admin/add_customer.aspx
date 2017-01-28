<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_customer.aspx.cs" Inherits="Admin_add_customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>

    <%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk">
    </script>--%>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBDN1IxVt1PYVCozy6Gi42uBY4GmGTl6So">
    </script>
    <%--<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>--%>
    <script type="text/javascript">
        function initialize() {

            var st = '';
            var flag = false;
            if (document.getElementById('ContentPlaceHolder1_txtgpslocation').value != '' && document.getElementById('ContentPlaceHolder1_txtgpslocation').value != null) {
                st = document.getElementById('ContentPlaceHolder1_txtgpslocation').value;
                GetMap(st);
            }
            else if (document.getElementById('ContentPlaceHolder1_txtstreetaddress').value != '' && document.getElementById('ContentPlaceHolder1_txtstreetaddress').value != null) {
                GetLongitudeLatitude(document.getElementById('ContentPlaceHolder1_txtstreetaddress').value);
                st = document.getElementById('ContentPlaceHolder1_hidaddresslonglet').value;
                $("#ContentPlaceHolder1_txtgpslocation").val(st);
                GetMap(st);
            }
            else if (document.getElementById('ContentPlaceHolder1_hidlongleti').value != '' && document.getElementById('ContentPlaceHolder1_hidlongleti').value != null) {
                st = document.getElementById('ContentPlaceHolder1_hidlongleti').value;
                GetMap(st);
            }
            else {
                //var latitu = '23.0225050';
                //var longtu = '72.5713621';
                //st = latitu + ',' + longtu;

                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (p) {
                        //st = new google.maps.LatLng(p.coords.latitude, p.coords.longitude);
                        st = p.coords.latitude + "," + p.coords.longitude;
                        //alert(st);
                        GetMap(st);
                    });
                }
                //alert(st);
                flag = true;
            }

        }

        function GetMap(st) {
            var longlat = st.split(",");
            //alert("long" + longlat[0]);
            //alert("long" + longlat[1]);
            var lat = longlat[0];
            var lon = longlat[1];
            //alert(lat);
            //alert(lon);
            var myLatlng = new google.maps.LatLng(lat, lon) // This is used to center the map to show our markers
            var mapOptions = {
                center: myLatlng,
                zoom: 12,
                mapTypeId: google.maps.MapTypeId.ROADMAP, //HYBRID // ROADMAP
                marker: true
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
            var marker = new google.maps.Marker({
                position: myLatlng,
                draggable: true
            });

            // adds a listener to the marker
            // gets the coords when drag event ends
            // then updates the input with the new coords
            var geocoder = geocoder = new google.maps.Geocoder();

            //address from longitude and letitude
            if (document.getElementById('ContentPlaceHolder1_hidlongleti').value == '') {
                if (document.getElementById('ContentPlaceHolder1_hidlongleti').value != '') {
                    if (flag == false) {
                        geocoder.geocode({ 'latLng': myLatlng }, function (results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                if (results[1]) {
                                    //alert("Location: " + results[1].formatted_address);
                                    $("#ContentPlaceHolder1_txtstreetaddress").val(results[1].formatted_address);
                                    //alert(results[0].address_components.length);
                                    for (var i = 0; i < results[0].address_components.length; i++) {
                                        if (results[0].address_components[i].types[0] == "country") {
                                            //alert(results[0].address_components[i].long_name);
                                            $("#ContentPlaceHolder1_txtcountry").val(results[0].address_components[i].long_name);
                                        }
                                        if (results[0].address_components[i].types[0] == "locality") {
                                            //alert(results[0].address_components[i].long_name);
                                            $("#ContentPlaceHolder1_txtcity").val(results[0].address_components[i].long_name);
                                        }
                                        if (results[0].address_components[i].types[0] == "administrative_area_level_1") {
                                            //alert(results[0].address_components[i].long_name);
                                        }
                                    }

                                }
                            }
                        });
                    }
                }
            }

            // adds a listener to the marker
            // gets the coords when drag event ends
            // then updates the input with the new coords
            google.maps.event.addListener(marker, 'dragend', function (evt) {
                geocoder.geocode({ 'latLng': marker.getPosition() }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {

                        lat = marker.getPosition().lat().toFixed(6);
                        lng = marker.getPosition().lng().toFixed(6);
                        $("#ContentPlaceHolder1_txtgpslocation").val(marker.getPosition().lat().toFixed(6) + ',' + marker.getPosition().lng().toFixed(6));
                        $("#ContentPlaceHolder1_txtstreetaddress").val(results[0].formatted_address);
                        address = results[0].formatted_address;

                        for (var i = 0; i < results[0].address_components.length; i++) {
                            if (results[0].address_components[i].types[0] == "country") {
                                //alert(results[0].address_components[i].long_name);
                                $("#ContentPlaceHolder1_txtcountry").val(results[0].address_components[i].long_name);
                            }
                            if (results[0].address_components[i].types[0] == "locality") {
                                //alert(results[0].address_components[i].long_name);
                                $("#ContentPlaceHolder1_txtcity").val(results[0].address_components[i].long_name);
                            }
                            if (results[0].address_components[i].types[0] == "administrative_area_level_1") {
                                //alert(results[0].address_components[i].long_name);  // state
                                //$("#ContentPlaceHolder1_txtcity").val(results[0].address_components[i].long_name);
                            }
                        }

                        //alert("Latitude: " + lat + "\nLongitude: " + lng + "\nAddress: " + address);

                    }
                });
            });

            ////// centers the map on markers coords
            ////alert(marker.position);
            //map.setCenter(marker.position);
            document.getElementById('ContentPlaceHolder1_hidlongleti').value = '';
            //// set map
            marker.setMap(map);
        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtglobelrate").blur(function () {
                var GlobleRate = $("#ContentPlaceHolder1_txtglobelrate").val();
                $("#ContentPlaceHolder1_txtglobelrate").val(GlobleRate + '%');
            });

            $('#ContentPlaceHolder1_txtcredit').blur(function () {
                var CreditRate = $('#ContentPlaceHolder1_txtcredit').val();
                $('#ContentPlaceHolder1_txtcredit').val(CreditRate + '.00');
            });

        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hidlongleti" runat="server" />
    <asp:HiddenField ID="hidpass" runat="server" />
    <asp:HiddenField ID="hidaddresslonglet" runat="server" />
    <!-- Content Header (Page header) -->
    <section class="content-header add_cust">
        <h1>
            <asp:Label ID="ltrheading" runat="server"></asp:Label>
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <asp:Button ID="btnsubmit" runat="server" class="btn btn-info pull-right" Text="Save" ValidationGroup="btnsave" OnClick="btnsubmit_Click" />
    </section>

    <!-- Main content -->
    <section class="content add_cust">
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
            <form class="form-horizontal">

                <div class="col-md-12" id="divCustomerSales" runat="server" visible="false">
                    <!-- Horizontal Form -->
                    <div class="box box-info">
                        <div class="box-header bdrbtm">
                            <div class="col-md-7">
                                <h3 class="box-title tphdr mtp">Date Joined :
                                <asp:Literal ID="ltrJoineddate" runat="server"></asp:Literal>
                                </h3>
                            </div>
                            <div class="rightwrapheader">
                                <h3 class="box-title tphdr"><span>Number Of Orders :</span><label id="lblnoOrders" runat="server">0</label></h3>
                                <h3 class="box-title tphdr"><span class="blk">Total Sale :</span><label id="lblTotalSale" runat="server">0</label></h3>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-6">
                    <!-- Horizontal Form -->
                    <div class="box box-info">
                        <div class="box-header">
                            <h3 class="box-title">Company Details</h3>
                        </div>

                        <!-- /.box-header -->
                        <!-- form start -->

                        <div class="box-body">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Company Name </label>

                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtcompanyname" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtcompanyname" ForeColor="Red" SetFocusOnError="true" runat="server" ControlToValidate="txtcompanyname" Display="Dynamic" ErrorMessage="Please enter company name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Street Address </label>

                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtstreetaddress" runat="server" MaxLength="500" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtstreetaddress" runat="server" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtstreetaddress" Display="Dynamic" ErrorMessage="Please enter street address" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">City </label>

                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtcity" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator CssClass="Required" ID="reqtxtcity" runat="server" ForeColor="Red" ControlToValidate="txtcity" Display="Dynamic" ErrorMessage="Please enter city" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-12 control-label">Country </label>

                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtcountry" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator CssClass="Required" ID="reqtxtcountry" runat="server" ForeColor="Red" ControlToValidate="txtcountry" Display="Dynamic" ErrorMessage="Please enter country" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Store Phone Number</label>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtstorenumber" runat="server" class="form-control" MaxLength="20" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtstorenumber" runat="server" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtstorenumber" Display="Dynamic" ErrorMessage="Please enter store phone number" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">GPS Location</label>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtgpslocation" runat="server" class="form-control" MaxLength="50"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator CssClass="Required" ID="reqtxtgpslocation" runat="server" ForeColor="Red" ControlToValidate="txtgpslocation" Display="Dynamic" ErrorMessage="Please enter gps location" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-9"> Format : LATITUDE,LONGITUDE e.g. 40.64523,-73.7840008</div>
                                <div class="col-md-3">                                   
                                    <%--<asp:Button ID="btnviewmap" runat="server" CssClass="btn btn-info pull-right" Text="View Map" />--%>
                                    <input type="button" value="View Map" class="btn btn-info pull-right" onclick="javascript: initialize();" />
                                </div>
                            </div>

                            <!-- Div for map -->
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div id="map_canvas" style="width: 100%; height: 223px"></div>
                                </div>
                            </div>

                        </div>


                    </div>
                    <!-- /.box -->

                </div>
                <!--/.col (right) -->

                <!-- right column -->
                <div class="col-md-6">
                    <!-- Horizontal Form -->
                    <div class="box box-info">
                        <div class="box-header">
                            <h3 class="box-title">Primary Contact</h3>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->

                        <div class="box-body">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Contact Name </label>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtcontactname" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtcontactname" SetFocusOnError="true" ForeColor="Red" runat="server" ControlToValidate="txtcontactname" Display="Dynamic" ErrorMessage="Please enter contact name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Mobile </label>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtmobile" runat="server" class="form-control" MaxLength="20" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtmobile" ForeColor="Red" SetFocusOnError="true" runat="server" ControlToValidate="txtmobile" Display="Dynamic" ErrorMessage="Please enter mobile" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Customer Type </label>
                                <div class="col-sm-12">
                                    <asp:DropDownList ID="ddlcustomertype" runat="server" class="form-control">
                                        <asp:ListItem Value="1">Wholesale</asp:ListItem>
                                        <asp:ListItem Value="2">Super Market</asp:ListItem>
                                        <asp:ListItem Value="3">Convenient Store</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Email</label>

                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtemail" runat="server" class="form-control" MaxLength="150"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator CssClass="Required" ID="reqtxtemail" runat="server" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtemail" Display="Dynamic" ErrorMessage="Please enter email" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="revtxtemail" runat="server" ControlToValidate="txtemail" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="The email address you entered appears to be incorrect.(Example: abc@aol.com)" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="btnsave" EnableViewState="False"></asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">New Password </label>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" class="form-control" MaxLength="20"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator CssClass="Required" ID="reqtxtpassword" ForeColor="Red" SetFocusOnError="true" runat="server" ControlToValidate="txtpassword" Display="Dynamic" ErrorMessage="Please enter password" ValidationGroup="btnsave"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">
                                    <h4>Customer Account Setting</h4>
                                </label>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Language Preference </label>
                                <div class="col-sm-12">
                                    <div class="label_radio">
                                        <asp:RadioButton ID="rbtenglish" Text="English" Checked="true" runat="server" GroupName="rbtlung" />
                                        <asp:RadioButton ID="rbtaribic" Text="Arabic" runat="server" GroupName="rbtlung" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Global Discount Rate</label>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtglobelrate" runat="server" class="form-control" onkeypress='return validateQty(this,event);' MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtglobelrate" ForeColor="Red" SetFocusOnError="true" runat="server" ControlToValidate="txtglobelrate" Display="Dynamic" ErrorMessage="Please enter global discount rate" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="label_check">
                                        <asp:CheckBox ID="chkcod" Text="Cash On Delivery" Checked="true" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-4  col-xs-12 ">
                                    <div class="label_check">
                                        <asp:CheckBox ID="chkallowcradit" Text="Allow Cradit" runat="server" />
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-6 ">
                                    <asp:DropDownList ID="ddldays" class="form-control" runat="server">
                                        <asp:ListItem Value="10 Days">10 Days</asp:ListItem>
                                        <asp:ListItem Value="20 Days">20 days</asp:ListItem>
                                        <asp:ListItem Value="30 Days">30 days</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-4 col-xs-6 ">
                                    <asp:TextBox ID="txtreducepercent" class="form-control" MaxLength="6" onkeypress="return isNumber(event)" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtreducepercent" ForeColor="Red" SetFocusOnError="true" runat="server" ControlToValidate="txtreducepercent" Display="Dynamic" ErrorMessage="Reduce percent" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">Credit Limit</label>
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtcredit" runat="server" class="form-control" onkeypress="return isNumber(event)" MaxLength="10"></asp:TextBox>

                                </div>
                            </div>

                        </div>

                    </div>
                </div>

                <!-- Div for map -->
                <%--<div class="col-sm-12">
                    <div class="box bdr-none">
                        <div class="box-body">
                            <div id="map_canvas" style="width: 100%; height: 400px"></div>
                        </div>
                    </div>
                </div>--%>

                <!-- /.box-body -->
                <div class="box-footer">
                    <%--  <button type="submit" class="btn btn-default">Cancel</button>--%>
                    <%--  <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>

                    <asp:Button ID="btncancel" runat="server" class="btn btn-default" Text="Back" OnClick="btncancel_Click" />
                </div>
                <!-- /.box-footer -->

            </form>
            <!--/.col (right) -->





        </div>

    </section>
    <!-- /.content -->
</asp:Content>

