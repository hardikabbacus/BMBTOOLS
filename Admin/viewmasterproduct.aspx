<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="viewmasterproduct.aspx.cs" Inherits="Admin_viewmasterproduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="DragDrop/style.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script>
        $(function () {
            //Initialize Select2 Elements
            $(".select2").select2();


        });

        function isNumberKeySKU(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 45 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

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
            width: 65%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s;
        }

        .modal-content-exp {
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

        $(document).ready(function () {
            KeyWordAutoPopuletSearchMasterProduct();
        });

        function KeyWordAutoPopuletSearchMasterProduct() {
            $("#ContentPlaceHolder1_txtsearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/KeyWordAutoPopuletSearchMasterProduct",
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

    </script>

    <%-------------------------- Drag drop --------------------------%>


    <script id="imageTemplate1" type="text/x-jquery-tmpl">
        <div class="imageholder" id="divimg1${fileNamewithoutspace1}">
            <figure>
                <a class="pop3" href="${filePath}" rel="group1">
                    <img class="imgnew" src="${filePath}" alt="${fileName}" />
                </a>
                <figcaption class="equal"><span></span>
                    <p>${fileName}</p>
                </figcaption>
            </figure>
            <a onclick="deleteimg1(this.id);" href="javascript:void(0);" id="${fileNamewithoutspace1}">delete</a>
        </div>

    </script>
    <script>
        function deleteimg1(imgid1) {
            //alert(imgid);
            //alert($("#divimg" + imgid).val());
            $("#divimg1" + imgid1).remove();
            var filename = "newr1" + imgid1;
            var hiddenfl2 = "<input type='hidden' value='" + filename + "' name='" + filename + "'>";
            //alert(hiddenfl);
            $("#dltimgtags1").append(hiddenfl2);

            var uploadfiles = $("#filedrgdrop").get(0);
            var uploadedfiles = uploadfiles.files;
            var fromdata = new FormData();
            var id;
            for (var x = 0; x < uploadfiles.files.length; x++) {
                fromdata.append(uploadedfiles[x].name, uploadedfiles[x]);
                // alert(uploadedfiles[x].name);
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <asp:HiddenField ID="hdproductid" Value="0" runat="server" />
    <asp:HiddenField ID="hidflag" runat="server" Value="0" />

    <section class="content-header">
        <h1>Master Product Details
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <input type="button" id="btnproductExportImport" title="Export Import" class="btn btn-info pull-right btnimpexp" value="Export Import" />
        <input type="button" id="btnMasterproduct" class="btn btn-info pull-right" title="Add New" value="Add New" />

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

                    <div class="box-header">
                        <h3 class="box-title">Search</h3>
                    </div>

                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">

                                <div class="col-sm-12 search_bar">
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtsearch" CssClass="form-control" placeholder="Name,SKU,Brand" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Button ID="imgbtnSearch" runat="server" class="btn btn-info pull-right" Text="Search" OnClick="imgbtnSearch_Click" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlsesrchCategory" CssClass="form-control select2" Width="200" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlsesrchCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="box">
                    <div class="box-header">
                        <h3 class="box-title">Master Product List</h3>
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
                                    <h6>No record found for master products.</h6>
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

                                <asp:TemplateField HeaderText="Image" SortExpression="imageName">
                                    <ItemStyle Width="100" />
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdmenuimage" runat="server" Value='<%# Eval("imageName")%>' Visible="false" />
                                        <a id="ancImage" runat="server" class="pop3" rel="group1">
                                            <img id="imgMenu" runat="server" width="50" height="50" title='<%# Eval("imageName") %>' /></a>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SKU" SortExpression="sku">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <input type="button" class="update no_btn" id='<%# Eval("productid") %>' value="<%#Eval("sku")%>" />

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Master Product Name" SortExpression="productName">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <input type="button" class="update no_btn" id='<%# Eval("productid") %>' value="<%# Server.HtmlDecode(Eval("productName").ToString())%>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price" SortExpression="lowprices">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("lowprices").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category" SortExpression="Category">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("Category").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Brand" SortExpression="Brand">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("Brand").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="IsActive" SortExpression="isactive" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkstatus" runat="server" Text='<%# Convert.ToInt32(Eval("isactive"))==1?"Active":"Inactive" %>'
                                            CommandArgument='<%# Convert.ToInt32(Eval("isActive"))%>' OnClick="lnkStatus_click"></asp:LinkButton>
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
                            <%--<asp:Button ID="btnadd" runat="server" class="btn btn-info pull-right" Text="Add New" />--%>
                            <%--<input type="button" id="btnMasterproduct" class="btn btn-info pull-right" title="+" value="Add New" />--%>
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
                <h2>Create Master Product</h2>
            </div>
            <div class="modal-body">

                <section class="content">
                    <div class="row">

                        <!-- right column -->
                        <div class="col-md-6">
                            <div class="row">

                                <!-- Horizontal Form -->
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="nav-tabs-custom popup">
                                    <ul class="nav nav-tabs">
                                        <li class="active"><a href="#General" data-toggle="tab" aria-expanded="true">English</a></li>
                                        <li><a href="#tab" data-toggle="tab">Arabic</a></li>
                                    </ul>
                                    <form class="form-horizontal">
                                        <div class="tab-content">

                                            <!-- /.Genral tab-pane -->

                                            <div class="tab-pane fade in active" id="General">

                                                <div class="form-group">
                                                    <label for="inputEmail3" class="col-sm-12 control-label">SKU*:</label>

                                                    <div class="col-sm-12">
                                                        <input type="text" class="form-control" id="txtpopSKU" maxlength="50" runat="server" onkeypress="return isNumberKeySKU(event);" />
                                                        <div id="skumsg" style="color: red"></div>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="inputEmail3" class="col-sm-12 control-label">Master Product Name*:</label>

                                                    <div class="col-sm-12">
                                                        <input type="text" class="form-control" id="txtpopProduct" maxlength="500" runat="server" />
                                                        <div id="productmastermsg" style="color: red"></div>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="inputEmail3" class="col-sm-12 control-label">Description:</label>

                                                    <div class="col-sm-12">
                                                        <%--<input type="text" class="form-control" id="Text2" >--%>
                                                        <textarea rows="3" class="form-control" id="txtpopDescriprion" runat="server"></textarea>
                                                    </div>
                                                </div>

                                                <div class="form-group drp_arrw">
                                                    <label for="inputEmail3" class="col-sm-12 control-label">Categories*:</label>

                                                    <div class="col-sm-12">
                                                        <asp:DropDownList ID="ddlPOPCategoryProduct" runat="server" class="form-control select2" multiple="multiple" data-placeholder="Select a category" Style="width: 100%;">
                                                        </asp:DropDownList>
                                                        <div id="productcategorymsg" style="color: red"></div>
                                                    </div>
                                                </div>

                                                <div class="form-group drp_arrw">
                                                    <label for="inputPassword3" class="col-sm-12 control-label">Brand*:</label>

                                                    <div class="col-sm-12">
                                                        <asp:DropDownList ID="ddlPOPBrandProduct" runat="server" class="form-control select2" multiple="multiple" data-placeholder="Select a brand" Style="width: 100%;">
                                                        </asp:DropDownList>
                                                        <div id="productbrandmsg" style="color: red"></div>
                                                    </div>
                                                </div>
                                                <!-- /.box-body -->

                                                <!-- /.box-footer -->

                                            </div>
                                            <div class="tab-pane fade" id="tab">

                                                <div class="form-group">
                                                    <label for="inputEmail3" class="col-sm-12 control-label">Arabic Name:</label>

                                                    <div class="col-sm-12">
                                                        <input type="text" class="form-control" id="txtArabicName" maxlength="100" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="inputEmail3" class="col-sm-12 control-label">Arabic Discription:</label>

                                                    <div class="col-sm-12">
                                                        <textarea class="form-control" id="txtArabicDiscription" maxlength="500" runat="server"></textarea>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </form>
                                </div>
                                <!-- /.box -->
                                <!-- general form elements disabled -->

                                <!-- /.box -->
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="box padb">
                                <div class="form-group">
                                    <div class="box-header">
                                        <div class="col-sm-12">
                                            <h3 class="box-title">Dropzone</h3>
                                        </div>
                                    </div>
                                    <div class="form-group col-sm-12">

                                        <div id="drop-zone" class="fullwidth">
                                            Drop files here...
                                                        <div id="clickHere">
                                                            or click here..
                                                                <input type="file" name="file[]" id="filedrgdrop" multiple />
                                                        </div>
                                        </div>
                                        <div id="result1" class="resultbox">
                                        </div>
                                        <div id="dltimgtags1"></div>
                                        <div id="showimg" class="master_prod_span"></div>
                                    </div>

                                </div>
                            </div>

                            <button id="btnPopMasterProupdate" onclick="return updateMasterProduct();" class="btn btn-info pull-right">Update</button>
                            <button id="btnPopMasterPro" onclick="return addMasterProductPopUp();" class="btn btn-info pull-right">Create</button>
                            <%--<asp:Button ID="btnPopMasterPro" runat="server" OnClientClick="return addmasterProduct();" Text="Create" CssClass="btn btn-info pull-right" />--%>
                        </div>
                        <!--/.col (right) -->

                    </div>
                    <!-- /.row -->
                </section>

            </div>

        </div>

    </div>

    <!-- Category Dilog Box -->
    <div id="ModelCategory" class="modal">

        <!-- Modal content -->
        <div class="modal-content-exp">
            <div class="modal-header">
                <span class="close1">×</span>
                <h2>Export Import Master Product</h2>
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
                                                <asp:Button ID="btnupload" CssClass="btn btn-xs btn-info" runat="server" OnClick="btnupload_Click" Text="Download" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-3 control-label">Step-2</label>

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
                                                <asp:Button ID="btnimport" runat="server" CssClass="btn btn-xs btn-info" OnClick="btnimport_Click" ValidationGroup="btnsaveimport" Text="Import" />
                                        </div>
                                    </div>




                                </div>
                                <!-- /.box-body -->

                                <!-- /.box-footer -->

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
        var btn = document.getElementById("btnMasterproduct");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
        btn.onclick = function () {
            modal.style.display = "block";
            document.getElementById('btnPopMasterProupdate').style.display = 'none';
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

    <script>
        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
        btnupdate.onclick = function () {
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

    <script>
        $('.update').click(function (e) {
            // e.preventDefault(); // <-- don't follow the link
            //$('#id-of-hidden-field').val($(this).attr('title'));
            clearform();

            var id = $(this).attr("id");
            $('#ContentPlaceHolder1_hdproductid').val(id);
            //alert(id);
            fetchData();

            //BindPopCategoryProduct(id);
            //BindPopBrandProduct(id);

            var modal = document.getElementById('myModal');
            modal.style.display = "block";
            document.getElementById('btnPopMasterPro').style.display = 'none';
            document.getElementById('btnPopMasterProupdate').style.display = '';
        });
    </script>

    <script>
        // Get the modal
        var modalcategory = document.getElementById('ModelCategory');

        // Get the button that opens the modal
        var btnproductExportImport = document.getElementById("btnproductExportImport");

        // Get the <span> element that closes the modal
        var spancateory = document.getElementsByClassName("close1")[0];

        // When the user clicks the button, open the modal
        btnproductExportImport.onclick = function () {
            modalcategory.style.display = "block";
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

    <script src="js/jquery.min.js"></script>
    <script src="DragDrop/jquery.tmpl.min.js"></script>
    <script src="DragDrop/modernizr.custom.js"></script>
    <script src="DragDrop/newjs2.js"></script>

    <script src="js/general.js"></script>
</asp:Content>

