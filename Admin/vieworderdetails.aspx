<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="vieworderdetails.aspx.cs" Inherits="Admin_vieworderdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>

    <style>
        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
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
            }
    </style>

    <%--<style>
        /*form styles*/
        #msform {
            width: 400px;
            margin: 50px auto;
            text-align: center;
            position: relative;
        }

            #msform fieldset {
                background: white;
                border: 0 none;
                border-radius: 3px;
                box-shadow: 0 0 15px 1px rgba(0, 0, 0, 0.4);
                padding: 20px 30px;
                box-sizing: border-box;
                width: 80%;
                margin: 0 10%;
                /*stacking fieldsets above each other*/
                position: absolute;
            }
                /*Hide all except first fieldset*/
                #msform fieldset:not(:first-of-type) {
                    display: none;
                }
            /*inputs*/
            #msform input, #msform textarea {
                padding: 15px;
                border: 1px solid #ccc;
                border-radius: 3px;
                margin-bottom: 10px;
                width: 100%;
                box-sizing: border-box;
                font-family: montserrat;
                color: #2C3E50;
                font-size: 13px;
            }
            /*buttons*/
            #msform .action-button {
                width: 100px;
                background: #27AE60;
                font-weight: bold;
                color: white;
                border: 0 none;
                border-radius: 1px;
                cursor: pointer;
                padding: 10px 5px;
                margin: 10px 5px;
            }

                #msform .action-button:hover, #msform .action-button:focus {
                    box-shadow: 0 0 0 2px white, 0 0 0 3px #27AE60;
                }
        /*headings*/
        .fs-title {
            font-size: 15px;
            text-transform: uppercase;
            color: #2C3E50;
            margin-bottom: 10px;
        }

        .fs-subtitle {
            font-weight: normal;
            font-size: 13px;
            color: #666;
            margin-bottom: 20px;
        }
        /*progressbar*/
        #progressbar {
            margin-bottom: 30px;
            overflow: hidden;
            /*CSS counters to number the steps*/
            counter-reset: step;
        }

            #progressbar li {
                list-style-type: none;
                color: white;
                text-transform: uppercase;
                font-size: 12px;
                width: 24.33%;
                float: left;
                position: relative;
            }

                #progressbar li:before {
                    content: counter(step);
                    counter-increment: step;
                    width: 20px;
                    line-height: 20px;
                    display: block;
                    font-size: 10px;
                    color: #333;
                    background: white;
                    border-radius: 3px;
                    margin: 0 auto 5px auto;
                }
                /*progressbar connectors*/
                #progressbar li:after {
                    content: '';
                    width: 100%;
                    height: 2px;
                    background: white;
                    position: absolute;
                    left: -50%;
                    top: 9px;
                    z-index: -1; /*put it behind the numbers*/
                }

                #progressbar li:first-child:after {
                    /*connector not needed before the first step*/
                    content: none;
                }
                /*marking active/completed steps green*/
                /*The number of the step and the connector before it = green*/
                #progressbar li.active:before, #progressbar li.active:after {
                    background: #27AE60;
                    color: white;
                }
    </style>--%>

    <%--<script type="text/javascript">
        function invokeButtonClick() {
            $("#span").click();
        }
    </script>--%>

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

    </script>

    <script type="text/javascript">

        //On UpdatePanel Refresh
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //alert(prm);
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        //$("#tabs").tabs();
                    }
                });
            };
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <asp:HiddenField ID="hidorderid" runat="server" Value="0" />
    <asp:HiddenField ID="hidPayType" runat="server" />

    <section class="content-header">
        <h1>
            <asp:Label ID="ltrheading" runat="server"></asp:Label>
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <asp:Label ID="lblOrderNo" runat="server" CssClass="oeder_no" Text="0"></asp:Label>

        <asp:Button ID="btnMakepaid" runat="server" class="btn btn-info pull-right" OnClick="btnMakepaid_Click" Text="Mark as Paid" />
        <asp:Button ID="btnOrderpdf" runat="server" class="btn btn-info pull-right  btnimpexp" Text="Invoice PDF" OnClick="btnOrderpdf_Click" />
        <asp:Button ID="btnSendOrderMail" runat="server" class="btn btn-info pull-right  btnimpexp" Text="Send Email" OnClick="btnSendOrderMail_Click" />
        <asp:Button ID="btnCancelorder" runat="server" class="btn btn-info pull-right" OnClick="btnCancelorder_Click" Text="Cancel Order" />
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
                    <div id="Div1" class="dataTables_wrapper form-inline dt-bootstrap">
                        <div class="row">
                            <div class="col-sm-12 search_bar">
                                <div class="col-sm-12">
                                    <div class="col-sm-6 ord_comp_detls">
                                        <%--<span>Company Name :</span> <asp:la
                                        <span>Shipping Address :</span>
                                        <span>Contact Person :</span>
                                        <span>Contact Phone :</span>--%>
                                        <asp:Literal ID="ltrContact" runat="server"></asp:Literal>
                                    </div>
                                    <div class="col-sm-6">

                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>

                                                <ul id="progressbar">
                                                    <li class="active" id="lnkrecive" runat="server">
                                                        <asp:LinkButton ID="lnkorderreceived" Text="Order Receieved" class="next action-button" ForeColor="#cccccc" runat="server" OnClick="lnkorderreceived_Click" /></li>
                                                    <li class="" id="lnkpreorder" runat="server">
                                                        <asp:LinkButton ID="lnkpreparingorder" Text="Preparing Order" class="next action-button" ForeColor="#cccccc" runat="server" OnClick="lnkpreparingorder_Click" /></li>
                                                    <li class="" id="lnkship" runat="server">
                                                        <asp:LinkButton ID="lnkshipped" Text="Shipped" ForeColor="#cccccc" class="next action-button" runat="server" OnClick="lnkshipped_Click" /></li>
                                                    <li class="" id="lnkdelivery" runat="server">
                                                        <asp:LinkButton ID="lnkdelivered" Text="Delivered" ForeColor="#cccccc" class="next action-button" runat="server" OnClick="lnkdelivered_Click" /></li>
                                                </ul>


                                                <%--<asp:LinkButton ID="lnkorderreceived" Text="Order Recieved" class="next action-button" ForeColor="#cccccc" runat="server" OnClick="lnkorderreceived_Click" />
                                    <asp:LinkButton ID="lnkpreparingorder" Text="Preparing Order" class="next action-button" ForeColor="#cccccc" runat="server" OnClick="lnkpreparingorder_Click" />
                                    <asp:LinkButton ID="lnkshipped" Text="Shipped" ForeColor="#cccccc" class="next action-button" runat="server" OnClick="lnkshipped_Click" />
                                    <asp:LinkButton ID="lnkdelivered" Text="Delivered" ForeColor="#cccccc" class="next action-button" runat="server" OnClick="lnkdelivered_Click" />--%>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lnkorderreceived" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkpreparingorder" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkshipped" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkdelivered" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="col-xs-9">
                        <div class="box">
                            <div class="box-body btnOrd">


                                <div class="box-header bdrbtm">
                                    <div class="col-sm-6">
                                        <h3 class="box-title">Order details</h3>
                                    </div>
                                    <div class="col-sm-6">
                                        <input type="button" id="btnAddOrder" title="+" value="+" class="btn btn-info pull-right add_btn" style="display: none;" />
                                        <asp:Button ID="btnEditOrder" runat="server" class="btn btn-info pull-right" OnClick="btnEditOrder_Click" Text="Edit Order" />
                                        <asp:Button ID="btnRemoveItem" runat="server" class="btn btn-info pull-right" OnClick="btnRemoveItem_Click" Visible="false" OnClientClick="javascript:return CheckUserItemSelection();" Text="Remove Item" />
                                        <asp:Button ID="btnSaveChanges" runat="server" class="btn btn-info pull-right" OnClick="btnSaveChanges_Click" Visible="false" Text="Save Changes" />
                                    </div>
                                </div>

                                <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                                    <div class="row">
                                        <div class="col-sm-12">
                                        </div>

                                        <div class="col-sm-12">
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
                                                    <asp:TemplateField ControlStyle-CssClass="DisNone">
                                                        <HeaderStyle Width="1" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle Width="1" HorizontalAlign="left" />
                                                        <HeaderTemplate>
                                                            <div class="label_check">
                                                                <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" runat="server" Text=" " Visible="false" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"></asp:CheckBox>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div class="label_check">
                                                                <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" runat="server" Text=" " AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"></asp:CheckBox>
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
                                                            <asp:Label ID="lblProName" runat="server" Visible="false" Text='<%# Server.HtmlDecode(Eval("productname").ToString()) %>'></asp:Label>
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
                                                            <asp:TextBox ID="txtQty" onkeypress="return isNumber(event)" runat="server" Width="50" class="form-control" MaxLength="6"
                                                                Text='<%# Eval("qty")%>' Visible="false"></asp:TextBox>
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
                                                            <%--<asp:TextBox ID="txtDiscount" onkeypress="return isNumber(event)" runat="server" Width="50" class="form-control" MaxLength="6"
                                                                Text='' Visible="false"></asp:TextBox>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Final Price" SortExpression="netprice">
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

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCst" runat="server" Text='<%# Eval("cost")%>' Visible="false"></asp:Label>
                                                            <asp:HiddenField ID="hidOrderStatus" runat="server" Value='<%#Eval("orderstatus") %>' />
                                                            <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.jpg" CommandName="Delete" CommandArgument='<%# Eval("orderdetailid")%>' OnClick="imgDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this item?')" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>

                                            <div class="pagi" style="display: none;">
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
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="box">
                            <div class="box-body">
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
                </ContentTemplate>
            </asp:UpdatePanel>
    </section>

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
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <!-- right column -->
                                <div class="col-md-12">
                                    <!-- Horizontal Form -->
                                    <div class="box box-info">
                                        <div class="box-header with-border">
                                            <div class="col-md-11">
                                                <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control" placeholder="Search by Product and SKU"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Button ID="btnsearch" runat="server" class="btn btn-info pull-right" OnClick="btnsearch_Click" Text="search" />
                                            </div>
                                        </div>
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
                                                                                <asp:CheckBox ID="chkHeader" Visible="false" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" runat="server" Text=" "></asp:CheckBox>
                                                                            </div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div class="label_check">
                                                                                <asp:CheckBox ID="chkDelete" Enabled="false" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" runat="server" Text=" "></asp:CheckBox>
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
                                    <!-- /.box-footer -->

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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

    <%-- <script src="js/jquery.easing.min.js"></script>--%>
    <%--  <script type="text/javascript">
        //jQuery time
        var current_fs, next_fs, previous_fs; //fieldsets
        var left, opacity, scale; //fieldset properties which we will animate
        var animating; //flag to prevent quick multi-click glitches

        $(".next").click(function () {
            if (animating) return false;
            animating = true;

            current_fs = $(this).parent();
            next_fs = $(this).parent().next();

            //activate next step on progressbar using the index of next_fs
            $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

            //show the next fieldset
            next_fs.show();
            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {
                    //as the opacity of current_fs reduces to 0 - stored in "now"
                    //1. scale current_fs down to 80%
                    scale = 1 - (1 - now) * 0.2;
                    //2. bring next_fs from the right(50%)
                    left = (now * 50) + "%";
                    //3. increase opacity of next_fs to 1 as it moves in
                    opacity = 1 - now;
                    current_fs.css({ 'transform': 'scale(' + scale + ')' });
                    next_fs.css({ 'left': left, 'opacity': opacity });
                },
                duration: 800,
                complete: function () {
                    current_fs.hide();
                    animating = false;
                },
                //this comes from the custom easing plugin
                easing: 'easeInOutBack'
            });
        });

        $(".previous").click(function () {
            if (animating) return false;
            animating = true;

            current_fs = $(this).parent();
            previous_fs = $(this).parent().prev();

            //de-activate current step on progressbar
            $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

            //show the previous fieldset
            previous_fs.show();
            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {
                    //as the opacity of current_fs reduces to 0 - stored in "now"
                    //1. scale previous_fs from 80% to 100%
                    scale = 0.8 + (1 - now) * 0.2;
                    //2. take current_fs to the right(50%) - from 0%
                    left = ((1 - now) * 50) + "%";
                    //3. increase opacity of previous_fs to 1 as it moves in
                    opacity = 1 - now;
                    current_fs.css({ 'left': left });
                    previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
                },
                duration: 800,
                complete: function () {
                    current_fs.hide();
                    animating = false;
                },
                //this comes from the custom easing plugin
                easing: 'easeInOutBack'
            });
        });

        $(".submit").click(function () {
            return false;
        })
    </script>--%>



    <script>
        // Get the modal
        var modal = document.getElementById('ContentPlaceHolder1_myModal');

        // Get the button that opens the modal
        var btn = document.getElementById("btnAddOrder");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
        btn.onclick = function () {
            //$('#ContentPlaceHolder1_GVOrder_chkHeader').attr('checked', false);

            //$('#ContentPlaceHolder1_GVOrder_chkDelete').attr('checked', false);

            modal.style.display = "block";
            clearform();

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

    <script src="js/general.js"></script>
</asp:Content>

