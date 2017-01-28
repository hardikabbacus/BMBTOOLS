<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="viewpayment.aspx.cs" Inherits="Admin_viewpayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../js/PaymentAuto.js"></script>
    <link href="css/jquery-ui.css" rel="stylesheet" />

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

    <script type="text/javascript">
        //On Page Load
        $(function () {
            $("#ContentPlaceHolder1_ddlsesrchCategory").select2();

        });

        $(document).ready(function () {
            SearchKeywordPayment();
        });

        function SearchKeywordPayment() {
            $("#ContentPlaceHolder1_txtsearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/SearchKeywordPayment",
                        data: "{'SearchKey':'" + document.getElementById('ContentPlaceHolder1_txtsearch').value + "'}",
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
                    $('#ContentPlaceHolder1_txtsearch').val(text);
                    $('#ContentPlaceHolder1_txtsearch').trigger('click');
                }
            });
        }

        //On UpdatePanel Refresh
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //alert(prm);
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        //alert('tst');
                        $("#ContentPlaceHolder1_ddlsesrchCategory").select2();
                        //$("#tabs").tabs();
                    }
                });
            };
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hidCustid" runat="server" Value="0" />
    <asp:HiddenField ID="hidPaymentId" runat="server" Value="0" />

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <section class="content-header">
        <h1>Manage Payments
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <input type="button" id="btnNewPayment" title="New Payment" value="New Payment" class="btn btn-info pull-right" />
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

            <div class="col-xs-8">
                <div class="box">
                    <div class="box-header">
                        <h3 class="box-title">Search</h3>
                    </div>

                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12 search_bar">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtsearch" CssClass="form-control cstm_search_payment" placeholder="Company name,payment id,order number" runat="server"></asp:TextBox>
                                        <asp:Button ID="imgbtnSearch" runat="server" class="btn btn-info pull-right" OnClick="imgbtnSearch_Click" Text="Search" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-4">
                <div class="box">
                    <div class="box-header">
                        <div class="col-sm-12">
                            <h3 class="box-title">Time Frame</h3>
                        </div>
                        <div class="col-sm-12">
                            <div class="sec3">
                                <div class="spec">
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk2015" runat="server" AutoPostBack="true" OnCheckedChanged="chk2015_CheckedChanged" /><p id="PervYear" runat="server">
                                            <%--<label id="PervYear" runat="server"></label>--%>
                                        </p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk2016" runat="server" AutoPostBack="true" OnCheckedChanged="chk2016_CheckedChanged" /><p id="currYear" runat="server">
                                            <%--<label id="currYear" runat="server"></label>--%>
                                        </p>
                                    </label>
                                </div>
                            </div>
                            <div class="sec4">
                                <div class="spec">
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk1" runat="server" AutoPostBack="true" OnCheckedChanged="chk1_CheckedChanged" /><p>1</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk2" runat="server" AutoPostBack="true" OnCheckedChanged="chk2_CheckedChanged" /><p>2</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk3" runat="server" AutoPostBack="true" OnCheckedChanged="chk3_CheckedChanged" /><p>3</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk4" runat="server" AutoPostBack="true" OnCheckedChanged="chk4_CheckedChanged" /><p>4</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk5" runat="server" AutoPostBack="true" OnCheckedChanged="chk5_CheckedChanged" /><p>5</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk6" runat="server" AutoPostBack="true" OnCheckedChanged="chk6_CheckedChanged" /><p>6</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk7" runat="server" AutoPostBack="true" OnCheckedChanged="chk7_CheckedChanged" /><p>7</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk8" runat="server" AutoPostBack="true" OnCheckedChanged="chk8_CheckedChanged" /><p>8</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk9" runat="server" AutoPostBack="true" OnCheckedChanged="chk9_CheckedChanged" /><p>9</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk10" runat="server" AutoPostBack="true" OnCheckedChanged="chk10_CheckedChanged" /><p>10</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk11" runat="server" AutoPostBack="true" OnCheckedChanged="chk11_CheckedChanged" /><p>11</p>
                                    </label>
                                    <label class="label_check">
                                        <asp:CheckBox ID="chk12" runat="server" AutoPostBack="true" OnCheckedChanged="chk12_CheckedChanged" /><p>12</p>
                                    </label>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-12">

                <div class="box">
                    <div class="box-header bdrbtm">
                        <div class="col-sm-10">
                            <h3 class="box-title">Payment History</h3>
                        </div>

                        <div class="col-sm-2 cmn_show">
                            <label for="inputSkills" class="control-label">Show:</label>
                            <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-12">


                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="paymentid" PagerStyle-CssClass="paging-link" role="grid" OnSorting="gvAdmin_Sorting"
                                    OnPageIndexChanging="gvAdmin_PageIndexChanging"
                                    OnRowDataBound="gvAdmin_RowDataBound"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    PagerStyle-HorizontalAlign="Right" Width="100%">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for order list.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle Width="50" />
                                            <HeaderTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" Text=" " onclick="javascript:SelectAllCheckboxes_1();" runat="server"></asp:CheckBox>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" Text=" " onclick="javascript:UnSelectHeaderCheckbox();" runat="server"></asp:CheckBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Order Number" SortExpression="orderid">
                                            <ItemStyle Width="130" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblpaymentid" runat="server" Text='<%#Eval("paymentid") %>' Visible="false"></asp:Label>
                                                <input type="button" class="updatepay no_btn" id='<%# Eval("paymentid") %>' value="<%#Eval("orderid")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Created At" SortExpression="CreatedDate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lbldate" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>--%>
                                                <input type="button" class="updatepay no_btn" id='<%# Eval("paymentid") %>' value="<%#Eval("CreatedDate")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name" SortExpression="companyname">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblcontactname" runat="server" Text='<%#Eval("contactname") %>'></asp:Label>--%>
                                                <input type="button" class="updatepay no_btn" id='<%# Eval("paymentid") %>' value="<%#Eval("companyname")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" SortExpression="paystatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblstatus" runat="server" Text='<%#Eval("paystatus").ToString() == "1" ? "Paid" : "Unpaid" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" SortExpression="payammount">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotal" runat="server" Text='<%#Eval("payammount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                    <PagerSettings Visible="true" Position="Bottom" Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />
                                </asp:GridView>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chk2015" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk2016" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk1" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk2" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk3" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk4" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk5" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk6" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk7" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk8" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk9" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk10" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk11" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chk12" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="imgbtnSearch" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <div class="pagi col-sm-12">
                            <div class="row">
                                <div class="col-sm-5">
                                    <div aria-live="polite" role="status" id="example1_info" class="dataTables_info">
                                        <asp:Literal ID="ltrcountrecord" runat="server"></asp:Literal>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="box-footer">
                            <asp:Button ID="imgbtnDelete" runat="server" Visible="false" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="imgbtnDelete_Click" Text="Delete" />
                            <div class="pagi">
                                <div class="row">
                                    <div class="col-sm-7">
                                        <div id="example1_paginate" class="dataTables_paginate paging_simple_numbers">
                                            <ul class="pagination">
                                                <asp:Literal ID="ltrpaggingbottom" runat="server"></asp:Literal>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>




    <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close">×</span>
                <h2>New Payment</h2>
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
                                            <label for="inputEmail3" class="col-sm-3 control-label">Choose Customer:</label>

                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtPOPCustomer" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                                <div id="customernamemsg" runat="server"></div>
                                                <%--<asp:RequiredFieldValidator ID="reqtxtPOPCustomer" ControlToValidate="txtPOPCustomer" ErrorMessage="Please enter customer name" EnableViewState="true" ForeColor="Red" Display="Dynamic" runat="server" ValidationGroup="btnsave"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">For Order Number:</label>

                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtPopOrderno" runat="server" MaxLength="6" onkeypress="return isNumberKey(event);" CssClass="form-control"></asp:TextBox>
                                                <div id="orderid" runat="server"></div>
                                                <%--<asp:RequiredFieldValidator ID="reqtxtPopOrderno" ControlToValidate="txtPopOrderno" ErrorMessage="Please enter order number" EnableViewState="true" ForeColor="Red" Display="Dynamic" runat="server" ValidationGroup="btnsave"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Notes :</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtPopNots" runat="server" TextMode="MultiLine" MaxLength="200" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group drp_arrw">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Amount Recieved :</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtPopAmmount" runat="server" MaxLength="8" Width="200" onkeypress='return validateQty(this,event);' CssClass="form-control"></asp:TextBox>
                                                <div id="ammount" runat="server"></div>
                                                <%--<asp:RequiredFieldValidator ID="reqtxtPopAmmount" ControlToValidate="txtPopAmmount" ErrorMessage="Please enter ammount" EnableViewState="true" ForeColor="Red" Display="Dynamic" runat="server" ValidationGroup="btnsave"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-footer">
                                        <asp:Button ID="btnpay" runat="server" Text="Confirm Payment" OnClientClick="addPayment()" ValidationGroup="btnsave" OnClick="btnpay_Click" class="btn btn-info pull-right" />
                                        <asp:Button ID="btnpayupdate" runat="server" Text="Update Payment" OnClientClick="addPayment()" ValidationGroup="btnsave" OnClick="btnpayupdate_Click" class="btn btn-info pull-right" />
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
        var btn = document.getElementById("btnNewPayment");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
        btn.onclick = function () {
            modal.style.display = "block";
            clearPaymentform();
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

    <script>
        $('.updatepay').click(function (e) {
            // e.preventDefault(); // <-- don't follow the link
            //$('#id-of-hidden-field').val($(this).attr('title'));
            clearPaymentform();

            var id = $(this).attr("id");
            $('#ContentPlaceHolder1_hidPaymentId').val(id);
            //alert(id);
            fetchPaymentData();

            var modal = document.getElementById('myModal');
            modal.style.display = "block";
            document.getElementById('ContentPlaceHolder1_btnpay').style.display = 'none';  // add button
            document.getElementById('ContentPlaceHolder1_btnpayupdate').style.display = ''; // update button
        });
    </script>



    <script type="text/javascript">
        $(document).ready(function () {
            $('.label_check input:checkbox').on('click', function () {
                if ($(this).is(":checked")) {
                    $(this).parent().addClass("menuitemshow");
                } else {
                    $(this).parent().removeClass("menuitemshow");
                }
            });

            $('.label_check input:radio').click(function () {
                $('.label_check input:radio[name=' + $(this).attr('name') + ']').parent().removeClass('menuitemshow');
                $(this).parent().addClass('menuitemshow');
            });
        });
    </script>

    <script src="js/general.js" type="text/javascript"></script>
</asp:Content>

