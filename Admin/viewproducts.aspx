<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="viewproducts.aspx.cs" Inherits="Admin_viewproducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script>
        $(function () {
            //Initialize Select2 Elements
            $(".select2").select2();
        });
    </script>
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
            width: 40%;
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
            width: 220px;
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
        function invokeButtonClick() {
            $("#btnproductExportImport").click();
        }


    </script>

    <script type="text/javascript">
        //On Page Load
        $(function () {
            $("#ContentPlaceHolder1_ddlsesrchCategory").select2();

        });

        $(document).ready(function () {
            KeyWordAutoPopuletSearchProduct();
        });

        function KeyWordAutoPopuletSearchProduct() {
            $("#ContentPlaceHolder1_txtsearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/KeyWordAutoPopuletSearchProduct",
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

                    }
                });
            };
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <asp:HiddenField ID="hidflag" runat="server" Value="0" />

    <section class="content-header">
        <h1>Product Details
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>

        <asp:Button ID="btnManageMaster" runat="server" class="btn btn-info pull-right" Text="Manage Master Products" OnClick="btnManageMaster_Click" />
        <input type="button" id="btnproductExportImport" class="btn btn-info pull-right  btnimpexp" title="Export Import" value="Export Import" />
        <asp:Button ID="btnadd" runat="server" class="btn btn-info pull-right" Text="Add New" OnClick="btnadd_Click" />
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

                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelCheckBoxes">
                    <ProgressTemplate>
                        <div class="Updatemodal">
                            <div class="Updatecenter">
                                <img alt="" src="../images/ajax-loader.gif" />
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div class="box">
                    <div class="box-header btmnone">
                        <h3 class="box-title">Search</h3>
                        <%--<button type="submit" id="btnproductExportImport" class="btn btn-info pull-right">Export Import</button>--%>
                    </div>

                    <div class=" ">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap col-sm-12">
                            <div class="row">

                                <div class="col-sm-12 search_bar">

                                    <label for="inputSkills" class="col-sm-1 control-label">Search</label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtsearch" CssClass="form-control" placeholder="name,SKU,brand" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanelCheckBoxes" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="col-sm-1">
                                                <asp:Button ID="imgbtnSearch" runat="server" class="btn btn-info pull-right" Text="Search" OnClick="imgbtnSearch_Click" />
                                            </div>
                                            <label for="inputSkills" class="col-sm-1 control-label">Filter</label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlsesrchCategory" CssClass="form-control  select2" Width="160" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlsesrchCategory_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-sm-3">
                                                <div class="radio">
                                                    <div class="label_radio">
                                                        <asp:RadioButton ID="rbtall" runat="server" GroupName="rbt" Checked="true" AutoPostBack="true" OnCheckedChanged="rbtAll_CheckedChanged" Text="All" />
                                                    </div>
                                                    <div class="label_radio">
                                                        <asp:RadioButton ID="rbtActive" runat="server" GroupName="rbt" OnCheckedChanged="rbtActive_CheckedChanged" AutoPostBack="true" Text="Active" />
                                                    </div>
                                                    <div class="label_radio">
                                                        <asp:RadioButton ID="rbtInactive" runat="server" GroupName="rbt" OnCheckedChanged="rbtInactive_CheckedChanged" AutoPostBack="true" Text="InActive" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="label_check featured">
                                                    <asp:CheckBox ID="chkfeatured" runat="server" Text="Featured Only" OnCheckedChanged="chkfeatured_CheckedChanged" AutoPostBack="true" />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="imgbtnSearch" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="rbtall" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rbtActive" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rbtInactive" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="chkfeatured" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlsesrchCategory" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Product List</h3>

                            </div>
                            <div class="col-sm-12">
                                <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" OnSorting="gvAdmin_Sorting" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="productId" PagerStyle-CssClass="paging-link" role="grid"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    OnPageIndexChanging="gvAdmin_PageIndexChanging"
                                    PagerStyle-HorizontalAlign="Right" Width="100%"
                                    OnRowDataBound="gvAdmin_RowDataBound">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for products.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
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
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Image" SortExpression="imageName">
                                            <ItemStyle Width="100" />
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdmenuimage" runat="server" Value='<%# Eval("imageName")%>'
                                                    Visible="false" />

                                                <a id="ancImage" runat="server" class="pop3" rel="group1">
                                                    <img id="imgMenu" runat="server" width="50" height="50" title='<%# Eval("imageName") %>' />
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SKU" SortExpression="sku">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <a runat="server" href='<%# "add_product.aspx?flag=edit&id=" + Eval("productid") + ""%>'><%#Eval("sku")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name" SortExpression="productName">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <a id="an" runat="server" href='<%# "add_product.aspx?flag=edit&id=" + Eval("productid") + ""%>'><%# Server.HtmlDecode(Eval("productName").ToString())%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price" SortExpression="price">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Literal ID="ltruserid" runat="server" Text='<%#Eval("price")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category" SortExpression="Category">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>

                                                <%--<%# Server.HtmlEncode(Eval("Category").ToString())%>--%>
                                                <asp:Label ID="lblcategory" Text='<%# Eval("Category").ToString() != "" ? Eval("Category").ToString() : Eval("MasterCategory").ToString() %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Brand" SortExpression="Brand">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%--<%# Server.HtmlEncode(Eval("Brand").ToString())%>--%>
                                                <asp:Label ID="lblBrand" Text='<%# Eval("Brand").ToString() != "" ? Eval("Brand").ToString() : Eval("MasterBrand").ToString() %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Inventory" SortExpression="inventory">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("inventory").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Visible="true" Position="Bottom" Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />
                                </asp:GridView>

                                <div class="pagi col-sm-12">
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

                                <div class="box-footer">
                                    <asp:Button ID="imgbtnDelete" runat="server" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="imgbtnDelete_Click" Text="Delete" />
                                    <%--<asp:Button ID="btnadd" runat="server" class="btn btn-info pull-right" Text="Add New" OnClick="btnadd_Click" />--%>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="imgbtnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="rbtall" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="rbtActive" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="rbtInactive" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="chkfeatured" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlsesrchCategory" EventName="SelectedIndexChanged" />--%>
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>
    </section>

    <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close">×</span>
                <h2>Export Import Product Wizard</h2>
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
                                            <p class="login-box-msg" id="P1" style="color: red;" runat="server">
                                                <%-- <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>--%>
                                                <asp:Label ID="lblcustom" runat="server" CssClass="Required" Text=""></asp:Label>
                                                <asp:Label ID="lbltotal" runat="server" CssClass="Required"></asp:Label>
                                                <asp:Label ID="lbltotalcount" runat="server" CssClass="Required"></asp:Label>
                                                <asp:Label ID="lbltotalsuccesscount" runat="server" CssClass="Required"></asp:Label>
                                                <asp:Label ID="lblerrorinrow" runat="server" CssClass="Required"></asp:Label>
                                                <asp:Label ID="lbltotalerrorcount" runat="server" CssClass="Required"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" CssClass="Required"></asp:Label>
                                            </p>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Step-1:</label>

                                            <div class="col-sm-9">
                                                Download the product data sheet
                                                <asp:Button ID="btnupload" runat="server" class="btn btn-xs btn-info" OnClick="btnupload_Click" Text="Download" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Strp-2</label>

                                            <div class="col-sm-9">
                                                Edit data sheet with your current product data
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Step-3</label>

                                            <div class="col-sm-9">
                                                Upload new data sheet
                                                <asp:FileUpload ID="file" runat="server" />
                                                <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqimageg" runat="server"
                                                    ControlToValidate="file" ErrorMessage="Please select file" Display="Dynamic"
                                                    ValidationGroup="btnsaveimport"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Step-4</label>

                                            <div class="col-sm-9">
                                                Import data sheet
                                                <asp:Button ID="btnimport" runat="server" class="btn btn-xs btn-info" OnClick="btnimport_Click" ValidationGroup="btnsaveimport" Text="Import" />
                                            </div>
                                        </div>




                                    </div>
                                    <!-- /.box-body -->

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
        var btn = document.getElementById("btnproductExportImport");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
        btn.onclick = function () {
            modal.style.display = "block";
        }

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
            window.location.href = "viewproducts.aspx";
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

