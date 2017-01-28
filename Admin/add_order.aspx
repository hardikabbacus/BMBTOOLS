<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_order.aspx.cs" Inherits="Admin_add_order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="css/jquery-ui.css" rel="stylesheet" />

    <style>
        .form-group {
            float: left;
            width: 100%;
        }

        #ui-id-2 {
            z-index: 99999999999;
        }
    </style>

    <style>
        /* The Modal (background) */
        .modal-backdrop.in {
            display: none;
        }

        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 999999; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .modal-content {
            position: relative;
            background-color: #fefefe;
            margin: auto;
            padding: 0;
            border: 1px solid #888;
            width: 65%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s;
        }

        /* Modal Content */
        .modal-contents {
            position: relative;
            background-color: #fefefe;
            margin: auto;
            padding: 0;
            border: 1px solid #888;
            width: 27%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s;
        }

        /* Add Animation */
        @-webkit-keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        @keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        /* The Close Button */
        .close {
            color: #000;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .modal-header {
            padding: 2px 16px;
            background-color: #FEFEFE;
            color: #000;
        }

        .modal-body {
            padding: 2px 16px;
        }

        .modal-footer {
            padding: 2px 16px;
            background-color: #FEFEFE;
            color: #000;
        }

        .close1 {
            opacity: 0.2;
            color: #000;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close1:hover,
            .close1:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
                opacity: 0.5;
            }

        /* for update panel lodder */
        .Updatemodal {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 94%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .Updatecenter {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .Updatecenter img {
                height: 30px;
                width: 200px;
            }
    </style>

    <script type="text/javascript">

        function ValidatePassKey(tb) {

            //alert(tb.id);
            var textid = tb.id;
            var txtvalue = $('#' + textid).val();

            var str = textid.split("_");
            //alert(str[3]);
            var createid = str[3];
            if (txtvalue != '' && txtvalue != '0') {
                document.getElementById("ContentPlaceHolder1_GVOrder_chkDelete_" + createid).checked = true;
            }
            else {
                document.getElementById("ContentPlaceHolder1_GVOrder_chkDelete_" + createid).checked = false;
            }
            createid++;
            //alert(createid);
            document.getElementById("ContentPlaceHolder1_GVOrder_txtqty_" + createid).focus();
        }

        function invokeButtonClick() {
            document.getElementById("btnCredits").click();
        }


    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            var cname = $('#ContentPlaceHolder1_txtCustomer').val();
            if (cname != "") {
                $('#DivGrid').show();
                $('#DivTotal').show();
            }
            else { $('#DivGrid').hide(); $('#DivTotal').hide(); }
        });

        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlCredits").change(function () {
                var valu = $("#ContentPlaceHolder1_ddlCredits").val();
                if (valu == 0) { $("#creditText").hide(); }
                if (valu == 1) { $("#creditText").show(); }
            });
        });

        //On UpdatePanel Refresh
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //alert(prm);
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        //$("#tabs").tabs();
                        $('#ContentPlaceHolder1_lblDelivertType').val();
                        $("#ContentPlaceHolder1_lblTotalPayAmmount").val();
                        //$("#ContentPlaceHolder1_rbtCredit").change();
                    }
                });
            };
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hidtxtQtyControlId" runat="server" Value="" />
    <asp:HiddenField ID="hidCustomerId" runat="server" />

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <asp:HiddenField ID="hidorderid" runat="server" Value="0" />

    <section class="content-header">
        <h1>
            <asp:Label ID="ltrheading" runat="server"></asp:Label>
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <asp:Button ID="btnConfirmOrder" runat="server" class="btn btn-info pull-right" OnClick="btnConfirmOrder_Click" CausesValidation="true" Text="Confirm Order" ValidationGroup="btnsave" />
        <asp:Button ID="btndiscard" runat="server" class="btn btn-info pull-right btnimpexp" Text="Discard" OnClick="btndiscard_Click" />
    </section>

    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <!-- Horizontal Form -->
                            <div class="alert alert-success alert-dismissible msgsucess" id="lblmsg" visible="false" runat="server">
                                <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                                <h4>
                                    <i class="icon fa fa-check"></i>
                                    <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>
                                </h4>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddProOrder" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>

            <div class="col-xs-9">
                <div class="box">

                    <div class="box-body">
                        <div id="Div2" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12 search_bar">
                                    <label for="inputSkills" class="col-sm-3 control-label">Choose Company:</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtCustomer" CssClass="form-control" placeholder="Enter company name" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-3" style="display: none;">

                <div class="box">
                    <div class="box-body">
                        <div id="Div3" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12 search_bar">
                                    <div class="col-sm-12">
                                        <div class="radio">
                                            <div class="label_radio">
                                                <asp:RadioButton ID="rbtCOD" runat="server" AutoPostBack="false" OnCheckedChanged="rbtCOD_CheckedChanged" Enabled="false" Checked="true" onclick="calcCOD();" GroupName="rbtpay" Text="Cash on delivery" />
                                                <input type="button" id="btnCredits" title="Export Import" class="btn btn-info pull-right btnimpexp" value="Credits PopUp" />
                                            </div>
                                        </div>
                                        <div class="radio">
                                            <div class="label_radio">
                                                <asp:RadioButton ID="rbtCredit" GroupName="rbtpay" AutoPostBack="false" OnCheckedChanged="rbtCredit_CheckedChanged" Enabled="false" onclick="calcCREDIT();" runat="server" Text="Credit" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div class="col-xs-9" id="DivGrid">
                <div class="box">
                    <div class="box-body btnOrd">

                        <div class="box-header bdrbtm">
                            <div class="col-sm-6">
                                <h3 class="box-title">Order details</h3>
                            </div>
                            <div class="col-sm-6">
                                <div class="col-md-11 text-center">
                                    <asp:Label ID="lblerrMsg" Text="" CssClass="control-label padtop" ForeColor="Red" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1">
                                    <input type="button" id="btnAddOrder" title="Add New" value="Add New" class="btn btn-info pull-right" />
                                </div>
                            </div>
                        </div>

                        <div id="Div4" class="dataTables_wrapper form-inline dt-bootstrap">

                            <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" CssClass="table table-bordered table-striped dataTable"
                                GridLines="None" DataKeyNames="orderdetailid" PagerStyle-CssClass="paging-link" role="grid" OnSorting="gvAdmin_Sorting"
                                AutoGenerateColumns="false" ShowFooter="false" OnRowDataBound="gvAdmin_RowDataBound"
                                PagerStyle-HorizontalAlign="Right" Width="100%">
                                <HeaderStyle CssClass="gridheader" />
                                <RowStyle CssClass="roweven" />
                                <AlternatingRowStyle CssClass="roweven" />
                                <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <div class="message error">
                                        <h6>No record found for order.</h6>
                                    </div>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle Width="50" HorizontalAlign="left" />
                                        <HeaderTemplate>
                                            <div class="label_check">
                                                <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" runat="server" Text=" "></asp:CheckBox>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="label_check">
                                                <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" runat="server" Text=" "></asp:CheckBox>
                                            </div>
                                            <asp:Label ID="lblproductid" runat="server" Text='<%#Eval("productid") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Image" SortExpression="imageName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblorderdetailids" runat="server" Text='<%#Eval("orderdetailid") %>' Visible="false"></asp:Label>
                                            <asp:HiddenField ID="hdmenuimageod" runat="server" Value='<%# Eval("imageName")%>'
                                                Visible="false" />
                                            <a id="ancImageod" runat="server" class="pop3" rel="group1">
                                                <img id="imgMenuod" runat="server" width="50" height="50" title='<%# Eval("imageName") %>' />
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SKU" SortExpression="sku">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%#Eval("sku")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="300" />
                                        <ItemTemplate>
                                            <asp:Label ID="lbldescp" runat="server" Text='<%# Server.HtmlDecode(Eval("productdescription").ToString().Replace(";amp",""))%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity" SortExpression="qty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblqty" runat="server" Text='<%# Eval("qty")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Price" SortExpression="price">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblprice" runat="server" Text='<%# Eval("price")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Discount %">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("globleDiscountRate").ToString() == "0" ? "0" : Eval("globleDiscountRate").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Final Price" SortExpression="finalprice">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblnetprice" runat="server" Text='<%# Eval("finalprice")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total Amount" SortExpression="netprice">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lbltotalammount" runat="server" Text='<%# Eval("netprice")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>

                            <div class="pagi  col-sm-12">
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

                            <div class="box-footer" style="display: none;">
                                <asp:Button ID="imgbtnDelete" runat="server" class="btn btn-default" OnClick="imgbtnDelete_Click" OnClientClick="javascript:return CheckUserItemSelection();" Text="Delete" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-3" id="DivTotal">
                <div class="box">
                    <div class="box-body">
                        <asp:Label ID="lblCreditLimits" CssClass="control-label" runat="server" Visible="false" Text=""></asp:Label>
                        <div class="box-header">
                            <h3 class="box-title text-center">Total Invoice Amount</h3>
                        </div>
                        <div class="invoicewrap">
                            <asp:Label ID="lblTotalPayAmmount" CssClass="control-label" runat="server" Text="0.00 SAR"></asp:Label>
                        </div>
                        <div class="invoicewrapbtm">
                            <asp:Label ID="Label1" CssClass="control-label" runat="server" Text="Payment"></asp:Label>
                            <asp:LinkButton ID="lnkEdit" CssClass="control-label-img" runat="server" OnClick="lnkEdit_Click"><img src="../images/edit.png" /></asp:LinkButton>
                            <asp:Label ID="lblDelivertType" CssClass="control-label-right" runat="server" Text="Cash on delivery"></asp:Label>
                        </div>
                    </div>



                </div>
            </div>
        </div>
    </section>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="Updatemodal">
                <div class="Updatecenter">
                    <img alt="" src="../images/ajax-loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <!-- The Modal -->
    <div id="myModal" class="modal" runat="server">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close">×</span>
                <h4>Add Product to Order</h4>
            </div>
            <div class="modal-body">

                <section class="content">
                    <div class="row">

                        <div class="col-md-12">
                            <!-- Horizontal Form -->
                            <div class="box box-info" style="margin-bottom: 0px;">
                                <div class="box-header" style="margin-top: 5px;">
                                    <div class="col-md-11">
                                        <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control" placeholder="Search by Product and SKU"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Button ID="btnsearch" runat="server" class="btn btn-info pull-right" OnClick="btnsearch_Click" Text="search" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- right column -->
                        <div class="col-md-12">


                            <div class="box-body">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <!-- Horizontal Form -->
                                        <div class="alert alert-success alert-dismissible msgsucess" id="lblerror" visible="false" runat="server">
                                            <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                                            <h4>
                                                <i class="icon fa fa-check"></i>
                                                <asp:Literal ID="lblerrors" runat="server"></asp:Literal>
                                            </h4>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAddProOrder" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>


                            <!-- Horizontal Form -->
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="box box-info ordPop">

                                        <!-- /.box-header -->
                                        <!-- form start -->

                                        <form class="form-horizontal">
                                            <div class="box-body">
                                                <div class="dataTables_wrapper form-inline dt-bootstrap">
                                                    <div class="row">
                                                        <div class="col-sm-12 product_new">
                                                            <asp:GridView ID="GVOrder" runat="server" AllowSorting="true" CssClass="table table-bordered table-striped dataTable"
                                                                GridLines="None" DataKeyNames="productId" PagerStyle-CssClass="paging-link" role="grid" OnSorting="GVOrder_Sorting"
                                                                OnRowDataBound="GVOrder_RowDataBound"
                                                                AutoGenerateColumns="false" ShowFooter="false"
                                                                PagerStyle-HorizontalAlign="Right" Width="100%">
                                                                <HeaderStyle CssClass="gridheader" />
                                                                <RowStyle CssClass="roweven" />
                                                                <AlternatingRowStyle CssClass="roweven" />
                                                                <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                                                <EmptyDataTemplate>
                                                                    <div class="message error">
                                                                        <h6>No record found for order .</h6>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle Width="10" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="10" HorizontalAlign="left" />
                                                                        <HeaderTemplate>
                                                                            <div class="label_check">
                                                                                <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" Visible="false" onclick="javascript:SelectAllCheckboxes_1();" runat="server" Text=" "></asp:CheckBox>
                                                                            </div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div class="label_check">
                                                                                <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" Enabled="false" onclick="javascript:UnSelectHeaderCheckbox();" runat="server" Text=" "></asp:CheckBox>
                                                                            </div>
                                                                            <asp:Label ID="lblproductid" runat="server" Text='<%#Eval("productid") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Image" SortExpression="imageName">
                                                                        <ItemStyle Width="50" />
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdmenuimage" runat="server" Value='<%# Eval("imageName")%>'
                                                                                Visible="false" />
                                                                            <a id="ancImage" runat="server" class="pop3" rel="group1">
                                                                                <img id="imgMenu" runat="server" width="50" height="50" title='<%# Eval("imageName") %>' />
                                                                            </a>
                                                                            <%--<asp:Label ID="lblOrderDetailId" runat="server" Text='<%#Eval("orderdetailid") %>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SKU" SortExpression="sku">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle Width="180" />
                                                                        <ItemTemplate>
                                                                            <%#Eval("sku")%>
                                                                            <asp:Label ID="lblCost" runat="server" Visible="false" Text='<%#Eval("cost") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="productName">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle Width="220" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblproName" runat="server" Text='<%#Server.HtmlDecode(Eval("productName").ToString()) %>'></asp:Label>
                                                                            <%--<%# Server.HtmlDecode(Eval("productName").ToString())%>--%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Price" SortExpression="price">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprice" runat="server" Text='<%# Eval("price")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Min Qty" SortExpression="minimumQuantity">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblminqty" runat="server" Text='<%# Eval("minimumQuantity")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Qty">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle Width="100" />
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtqty" Text="" runat="server" onchange="ValidatePassKey(this)" onkeypress="return isNumber(event)" Width="100" CssClass="form-control"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </form>

                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-footer">
                                        <asp:Button ID="btnAddProOrder" runat="server" CssClass="btn btn-info pull-right" OnClientClick="javascript:return CheckUserItemSelectionForUpdate();" Text="Add" OnClick="btnAddProOrder_Click" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnsearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <!-- /.box-footer -->

                        </div>

                        <!-- /.box -->
                        <!-- general form elements disabled -->

                        <!-- /.box -->
                    </div>
                    <!--/.col (right) -->
                </section>
            </div>
            <!-- /.row -->


        </div>

    </div>

    <script>
        // Get the modal
        var modal = document.getElementById('ContentPlaceHolder1_myModal');

        // Get the button that opens the modal
        var btn = document.getElementById("btnAddOrder");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
        btn.onclick = function () {

            var custname = $('#ContentPlaceHolder1_txtCustomer').val();
            //alert(custname);
            if (custname == '') {
                alert("Please select customer name.");
            }
            else {
                modal.style.display = "block";
                clearform();
            }
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

        $(document).ready(function () {
            KeyWordAutoPopuletProductName();
        });

        function KeyWordAutoPopuletProductName() {
            $("#ContentPlaceHolder1_txtSearchProduct").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/KeyWordAutoPopuletProductName",
                        data: "{'SearchKey':'" + document.getElementById('ContentPlaceHolder1_txtSearchProduct').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("No Match");
                        }
                    });
                },
                select: function (event, ui) {
                    var text = ui.item.value;
                    $('#ContentPlaceHolder1_txtSearchProduct').val(text);
                    $('#ContentPlaceHolder1_txtSearchProduct').trigger('click');
                }
            });
        }


    </script>


    <!-- Category Dilog Box -->
    <div id="ModelCategory" class="modal">

        <!-- Modal content -->
        <div class="modal-contents">
            <div class="modal-header">
                <span class="close1">×</span>
                <h2>Update Credit Limits</h2>
            </div>
            <div class="modal-body">

                <section class="content">
                    <div class="row">
                        <!-- right column -->
                        <div class="col-md-12">
                            <!-- general form elements -->
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <form role="form">
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="lblerrormessgae" Text="" CssClass="control-label padtop" ForeColor="Red" runat="server"></asp:Label>
                                            <select class="form-control" runat="server" id="ddlCredits">
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </div>
                                    </div>

                                    <div class="box-body" id="creditText">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Credit limits</label>
                                            <input type="text" class="form-control" id="txtCredits" maxlength="6" onkeypress="return isNumber(event)" runat="server" />
                                            <div id="msgCredit" style="color: red;"></div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->

                                    <div class="box-footer">
                                        <%--<button type="submit" id="btnCreditAdd" onclick="UpdateCredit();" class="btn btn-primary">Submit</button>--%>
                                        <input type="button" id="btnCreditAdd" onclick="UpdateCredit();" class="btn btn-primary" value="Submit" />
                                    </div>
                                </form>
                            </div>
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
        var modalcategory = document.getElementById('ModelCategory');

        //// Get the button that opens the modal
        var btncategory = document.getElementById("btnCredits");

        // Get the <span> element that closes the modal
        var spancateory = document.getElementsByClassName("close1")[0];

        //// When the user clicks the button, open the modal
        btncategory.onclick = function () {
            modalcategory.style.display = "block";
            fetchCreditPopUpData();
        }

        // When the user clicks on <span> (x), close the modal
        spancateory.onclick = function () {
            modalcategory.style.display = "none";

        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modalcategory.style.display = "none";
            }
        }
    </script>

    <script src="js/general.js"></script>

</asp:Content>

