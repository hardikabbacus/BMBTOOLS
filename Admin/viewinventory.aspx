<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="viewinventory.aspx.cs" Inherits="Admin_viewinventory" %>

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
    </style>
    <script type="text/javascript">
        function invokeButtonClick() {
            document.getElementById("btnproductExportImport").click();
        }
        function SelectallButtonClick() {
            // alert('click');
            // Iterate each checkbox
            $(':checkbox').each(function () {
                this.checked = true;
            });
        }

        // change text box focus on TAB button click
        function ValidatePassKey(tb) {
            //alert(tb.id);
            var textid = tb.id;
            var str = textid.split("_");
            //alert(str[3]);
            var createid = str[3];
            createid++;
            //alert(createid);
            document.getElementById("ContentPlaceHolder1_gvAdmin_txtInventoryAll_" + createid).focus();

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hidflag" runat="server" Value="0" />

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <section class="content-header">
        <h1>Manage Inventory
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <input type="button" id="btnproductExportImport" class="btn btn-info pull-right btnimpexp invt_exp" title="Export Import" value="Export Import" />
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

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="col-xs-8">

                        <div class="box">

                            <div class="box-header">
                                <h3 class="box-title">Search by SKU</h3>
                            </div>

                            <div>
                                <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                                  
                                            <div class="txtarea">
                                                <asp:TextBox ID="txtsearch" CssClass="form-control" TextMode="MultiLine" placeholder="Enter sku" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="box-footer">
                                                <asp:Button ID="imgbtnSearch" runat="server" class="btn btn-info pull-right" Text="Search" OnClick="imgbtnSearch_Click" />
                                        

                                        <div style="display: none;">
                                            <div class="col-sm-3 inv_btns">
                                                <asp:Button ID="btnSelectAll" runat="server" Visible="false" class="btn btn-info inv_sel_all_btn" OnClientClick="SelectallButtonClick();" Text="Select All" />

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
                                <div class="">
                                    <h3 class="box-title">FIlter by rule</h3>
                                </div>
                                <div class="rightsecpad">
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlfilter" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="0">Select Filter</asp:ListItem>
                                            <asp:ListItem Value="1">Less Than</asp:ListItem>
                                            <asp:ListItem Value="2">More Than</asp:ListItem>
                                            <asp:ListItem Value="3">Equal to</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtfilter" runat="server" placeholder="Value" CssClass="form-control"></asp:TextBox>
                                </div>



                                <div class="box-footer">
                                    <asp:Button ID="btnApply" runat="server" class="btn btn-info pull-right" OnClick="btnApply_Click1" Text="Apply" />
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-xs-12">
                        <div class="box">
                            <div class="box-header bdrbtm">
                                <div class="col-sm-8">
                                    <h3 class="box-title">Inventory List</h3>
                                </div>
                                
                                <div class="col-sm-4 cmn_show">
                                    <label for="inputSkills" class="control-label inve">Show:</label>
                                    <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:Button ID="Button1" runat="server" class="btn btn-info pull-right" Text="Update Inventory" ValidationGroup="btnsave" OnClientClick="javascript:return CheckUserItemSelectionForUpdate();" OnClick="btnUpdateAll_Click" />
                                </div>
                                
                            </div>

                            <div class="col-sm-12">

                                <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" OnSorting="gvAdmin_Sorting" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="productId" PagerStyle-CssClass="paging-link" role="grid"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    OnPageIndexChanging="gvAdmin_PageIndexChanging"
                                    PagerStyle-HorizontalAlign="Right" Width="100%"
                                    OnRowDataBound="gvAdmin_RowDataBound" OnRowEditing="gvAdmin_RowEditing" OnRowUpdating="gvAdmin_RowUpdating" OnRowCancelingEdit="gvAdmin_RowCancelingEdit">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for master products.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle Width="50" />
                                            <HeaderTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" Text=" " runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"></asp:CheckBox>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" Text=" " runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"></asp:CheckBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Image" SortExpression="imageName">
                                            <ItemStyle Width="200" />
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
                                            <ItemTemplate><%#Eval("sku")%></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Product Name" SortExpression="productName">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("productName").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Inventory" SortExpression="inventory">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="200" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblinventory" runat="server" Text='<%# Server.HtmlEncode(Eval("inventory").ToString())%>'></asp:Label>

                                                <asp:HiddenField ID="hidProductId" runat="server" Value='<%# Eval("productId") %>' />
                                                <asp:TextBox ID="txtInventoryAll" onblur="ValidatePassKey(this)" runat="server" class="form-control" MaxLength="6"
                                                    Text='<%# Eval("inventory")%>' Visible="false"></asp:TextBox><br />
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="reqtxtInventoryAll" runat="server"
                                                    ControlToValidate="txtInventoryAll" Display="Dynamic" ErrorMessage="Please enter inventory"
                                                    ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ForeColor="Red" ID="comtxtInventoryAll" Operator="DataTypeCheck"
                                                    runat="server" Type="Integer" ControlToValidate="txtInventoryAll" ErrorMessage="Please enter numeric value"
                                                    Display="Dynamic" ValidationGroup="btnsave" EnableViewState="false"></asp:CompareValidator>

                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtInventory" runat="server" class="form-control" onblur="ValidatePassKey(this)" MaxLength="6"
                                                    Text='<%# Eval("inventory")%>'></asp:TextBox><br />
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="reqtxtInventory" runat="server"
                                                    ControlToValidate="txtInventory" Display="Dynamic" ErrorMessage="Please enter inventory"
                                                    ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ForeColor="Red" ID="comtxtInventory" Operator="DataTypeCheck"
                                                    runat="server" Type="Integer" ControlToValidate="txtInventory" ErrorMessage="Please enter numeric value"
                                                    Display="Dynamic" ValidationGroup="btnsave" EnableViewState="false"></asp:CompareValidator>

                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action">
                                            <%--<HeaderStyle Width="90" HorizontalAlign="Center" />--%>
                                            <ItemStyle Width="100" HorizontalAlign="Center" />
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="ButtonUpdate" runat="server" CssClass="btn btn-info btn-md mar" ValidationGroup="btnsave" CommandName="Update">Confirm</asp:LinkButton>
                                                <asp:LinkButton ID="ButtonCancel" runat="server" CssClass="btn btn-info btn-md mar" CommandName="Cancel">Cancel</asp:LinkButton>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ButtonEdit" runat="server" CssClass="btn btn-info  btnimpexp" CommandName="Edit">Modify</asp:LinkButton>
                                                <%--<a id="an" runat="server" class="btn btn-orange" href='<%# "add_product.aspx?flag=edit&id=" + Eval("productid") + ""%>'>Modify</a>--%>
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

                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button ID="imgbtnDelete" runat="server" Visible="false" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="imgbtnDelete_Click" Text="Delete" />
                                    <asp:Button ID="btnUpdateAll" runat="server" Text="Update" Visible="false" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelectionForUpdate();" OnClick="btnUpdateAll_Click" />

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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>

    <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close">×</span>
                <h2>Import Inventory Wizard</h2>
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
                                                <asp:Button ID="btndownload" CssClass="btn btn-xs btn-info" runat="server" OnClick="btndownload_Click" Text="Download" />
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
                                                <asp:FileUpload ID="fluUploadCsv" runat="server" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Step-4</label>

                                            <div class="col-sm-9">
                                                Import data sheet
                                                <asp:Button ID="btnimport" CssClass="btn btn-xs btn-info" runat="server" OnClick="btnimport_Click" Text="Import" />
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

