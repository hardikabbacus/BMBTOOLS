<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="add_product.aspx.cs" Inherits="Admin_add_product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="DragDrop/style.css" rel="stylesheet" />

    <script>
        $(document).ready(function () {
            singleproductdisply();
        });
    </script>

    <%--<link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-ui-1.8.24.js" type="text/javascript"></script>
    <script src="js/globalize/globalize.js"></script>
    <script src="js/globalize/cultures/globalize.culture.en-GB.js"></script>
    <link href="Content/tag-it/css/jquery.tagit.css" rel="stylesheet" />
    <link href="Content/tag-it/css/tagit.ui-zendesk.css" rel="stylesheet" />
    <script src="Content/tag-it/js/tag-it.js"></script>--%>

    <script src="ckeditor/ckeditor.js" type="text/javascript"></script>

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

    <style type="text/css">
        .Mymodal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-size: 10pt;
            border: 10px solid #FFFFFF;
            border-radius: 10px;
            width: 220px;
            border-radius: 10px;
            display: none;
            filter: alpha(opacity=100);
            position: fixed;
            background-color: White;
            z-index: 999;
        }

            .loading img {
                height: 30px;
                width: 200px;
            }
    </style>

    <%-------------------------- Drag drop --------------------------%>
    <style>
       
    </style>
    <script id="imageTemplate" type="text/x-jquery-tmpl">
        <div class="imageholder" id="divimg${fileNamewithoutspace}">
            <figure>
                <a class="pop3" href="${filePath}" rel="group1">
                    <img class="imgnew" src="${filePath}" alt="${fileName}" />
                </a>
                <figcaption class="equal"><span></span>
                    <p>${fileName}</p>
                </figcaption>
            </figure>
            <a onclick="deleteimg(this.id);" href="javascript:void(0);" id="${fileNamewithoutspace}">delete</a>
        </div>
    </script>

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

    <script type="text/javascript">
        function deleteimg(imgid) {
            //alert(imgid);
            //alert($("#divimg" + imgid).val());
            $("#divimg" + imgid).remove();
            var filename = "newr" + imgid;
            var hiddenfl = "<input type='hidden' value='" + filename + "' name='" + filename + "'>";
            //alert(hiddenfl);
            $("#dltimgtags").append(hiddenfl);
        }

        function deleteimg1(imgid1) {
            //alert(imgid);
            //alert($("#divimg" + imgid).val());
            $("#divimg1" + imgid1).remove();
            var filename = "newr1" + imgid1;
            var hiddenfl2 = "<input type='hidden' value='" + filename + "' name='" + filename + "'>";
            //alert(hiddenfl);
            $("#dltimgtags1").append(hiddenfl2);
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfprevsort" runat="server" />
    <asp:HiddenField ID="hdtotallanguage" runat="server" Value="0" />
    <asp:HiddenField ID="hidLeftCategoryIdProduct" Value="0" runat="server" />
    <asp:HiddenField ID="hidLeftBrandIdProduct" Value="0" runat="server" />
    <asp:HiddenField ID="hidLeftTagsProduct" Value="" runat="server" />
    <asp:HiddenField ID="hidSku" runat="server" Value="" />
    <asp:HiddenField ID="hidprodID" runat="server" Value="0" />


    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <!-- Content Header (Page header) -->
    <section class="content-header">

        <div class="loading" align="center">
            <img src="../images/ajax-loader.gif" alt="" />
        </div>

        <h1>
            <asp:Label ID="ltrheading" runat="server"></asp:Label>
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>

        <asp:Button ID="btnProductsubmit" runat="server" class="btn btn-info pull-right" CausesValidation="true" OnClientClick="GetProductBrandValue();" Text="Save" ValidationGroup="btnsave" OnClick="btnsubmit_Click" />
        <asp:Button ID="btnDuplicate" runat="server" class="btn btn-default dupli_btn pull-right" Text="Duplicate" OnClientClick="GetProductBrandValue();" OnClick="btnDuplicate_Click" />
        <div class="pagi_np">
            <a id="lnkPreview" class="btn btn-default prev_btn" runat="server">Prev</a>
            <a id="lnkNext" class="btn btn-default next_btn" runat="server">Next</a>
        </div>

    </section>
    <!-- Main content -->
    <section class="content add_prod">

        <div class="row">

            <div class="col-md-12">
                <!-- Horizontal Form -->
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

            <!-- /.col -->
            <div class="col-md-8">
                <div class="col-md-12">
                    <div class="row">
                        <div class="box bdrnone">
                            <div class="nav-tabs-custom">
                                <ul class="nav nav-tabs">

                                    <asp:Literal ID="ltrtab" runat="server"></asp:Literal>
                                </ul>
                                <div class="tab-content">

                                    <!-- /.tab-pane -->

                                    <asp:Literal ID="ltrcategorylanguages" runat="server"></asp:Literal>

                                    <!-- /.tab-pane -->
                                </div>
                                <!-- /.nav-tabs-custom -->
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="row">
                        <div class="box">
                            <div class="box-body">
                                <div class="form-group prod_varient">
                                    <div class="col-sm-4">
                                        <div class="label_radio">
                                            <asp:RadioButton ID="rbtsingleprod" Text="Single Product" Checked="true" GroupName="rbt" OnClick="javascript:singleproductdisply();" runat="server" />
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="label_radio">
                                            <asp:RadioButton ID="rbtvariant" Text="Variant" GroupName="rbt" OnClick="javascript:singleproductdisply();" runat="server" />
                                        </div>
                                    </div>
                                    <div id="reqvarient" style="color: red"></div>
                                </div>
                                <div class="skudiv">
                                    <asp:HiddenField ID="mastercount" runat="server" Value="0" />
                                    <asp:Label ID="lblskubind" runat="server"></asp:Label>
                                </div>


                                <div class="form-group" id="idSku_PRoduct">
                                    <label for="inputName" class="col-sm-2 control-label">Enter SKU</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtskumaster" runat="server" class="form-control" />
                                        <%--<asp:DropDownList ID="ddlmastersku" runat="server" class="form-control select2"></asp:DropDownList>--%>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtmasterproductname" runat="server" class="form-control" placeholder="None - this is searchable field" />
                                        <%--<asp:DropDownList ID="ddlproductname" runat="server" class="form-control select2"></asp:DropDownList>--%>
                                    </div>
                                    <div class="col-sm-1" id="idAddMasterPro">
                                        <input type="button" id="btnMasterproduct" title="+" value="+" class="add_btn" />
                                    </div>
                                </div>

                                <div class="form-group" id="idSku_PRoduct1">
                                    <label for="inputName" class="col-sm-2 control-label">Varient</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtVarient" runat="server" class="form-control" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>

                <div class="col-md-6">
                    <div class="row bpadr">
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Price</h3>
                            </div>

                            <div class="col-sm-12 search_bar">
                                <div class="col-sm-12">

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-7 control-label">Wholesale (Primery)</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtWholePrice" runat="server" class="form-control" MaxLength="12" onkeypress='return validateQty(this,event);'></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtWholePrice" ForeColor="Red" runat="server"
                                                ControlToValidate="txtWholePrice" Display="Dynamic" ErrorMessage="Please enter wholesale price"
                                                ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-7 control-label">Super Market (Tier-1)</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtSuperMarketPrice" runat="server" class="form-control" MaxLength="12" onkeypress='return validateQty(this,event);'></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-7 control-label">Convinient Store (Tier-2)</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtConvinitPrice" runat="server" class="form-control" MaxLength="12" onkeypress='return validateQty(this,event);'></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="row bpadl">
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Controls</h3>
                            </div>

                            <div class="col-sm-12 search_bar">
                                <div class="col-sm-12">

                                    <div class="form-group" style="display: none;">
                                        <label for="inputName" class="col-sm-7 control-label">Price</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtprice" runat="server" class="form-control" MaxLength="12" onkeypress='return validateQty(this,event);'></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator CssClass="Required" ID="reqtxtprice" ForeColor="Red" runat="server"
                                                ControlToValidate="txtprice" Display="Dynamic" ErrorMessage="Please enter price"
                                                ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>--%>
                                        </div>

                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-7 control-label">Cost</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtcost" runat="server" class="form-control" MaxLength="12" onkeypress='return validateQty(this,event);'></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-7 control-label">Minimum Quantity</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtminqty" runat="server" class="form-control" MaxLength="6" onkeypress="return isNumber(event)"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ID="reqinputName" runat="server"
                                                ControlToValidate="txtminqty" Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter minimum quantity"
                                                ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="inputName" class="col-sm-7 control-label">Inventory</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtinventry" runat="server" class="form-control" MaxLength="6" onkeypress="return isNumber(event)"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqtxtinventry" runat="server"
                                                ControlToValidate="txtminqty" Display="Dynamic" ErrorMessage="Please enter inventory"
                                                ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator CssClass="Required" ID="cmotxtinventry" ControlToValidate="txtinventry" ControlToCompare="txtminqty" Type="Integer" ForeColor="Red" Operator="GreaterThan"
                                                runat="server" ErrorMessage="Inventory should be greater than minimum quantity"
                                                Display="Dynamic" ValidationGroup="btnsave" EnableViewState="false"></asp:CompareValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="row ">
                        <div class="form-group">
                            <label for="inventory_log" id="inventory_log" runat="server" class="col-sm-12 control-label aln_rgt"></label>
                        </div>
                    </div>
                </div>
                <!-- /.tab-content -->
            </div>

            <div class="col-md-4">

                <div class="col-md-12">
                    <div class="row">
                        <div class="box">
                            <div class="form-group bpadT" id="tdlanguages">
                                <label for="inputSkills" class="col-sm-4 control-label">Is Active</label>
                                <div class="col-sm-4 minimal">
                                    <div class="label_check">
                                        <asp:CheckBox ID="chkactive" Checked="true" runat="server" Text="Active" />
                                    </div>
                                </div>
                                <div class="col-sm-4 minimal">
                                    <div class="label_check">
                                        <asp:CheckBox ID="chkfeatured" runat="server" Text="Featured" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="row">
                        <!-- Horizontal Form -->
                        <div class="box box-info">
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">

                                <div class="form-group">

                                    <div id="drop-zone">
                                        Drop files here...
                                            <div id="clickHere">
                                                or click here..
                                                <input type="file" name="file[0]" id="filenew" multiple />
                                            </div>
                                    </div>
                                    <div id="result">
                                    </div>
                                    <div id="dltimgtags"></div>

                                    <asp:Literal ID="imgltr" runat="server"></asp:Literal>

                                </div>







                            </div>
                            <!-- /.box-body -->

                        </div>
                        <!-- /.box -->
                    </div>
                </div>

                <div class="col-md-12" id="divOnlySingle" style="display: none;">
                    <div class="row">
                        <div class="box">
                            <div class="form-group" id="categoryproduct" style="display: none;">
                                <div class="col-sm-12">
                                </div>
                            </div>

                            <div class="form-group" id="categoryname" style="display: none;">
                                <div class="col-sm-5">
                                    <label for="inputSkills" class="control-label">Category</label><input type="button" id="btnCategory" title="+" value="+" class="add_btn pull-right abtn" />
                                </div>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlcategoryleftside" runat="server" class="form-control select2" multiple="multiple" data-placeholder="Select a category" Style="width: 100%;">
                                    </asp:DropDownList>
                                    <div id="reqddlcategory" style="color: red"></div>
                                </div>

                            </div>

                            <div class="form-group" id="brandname" style="display: none;">
                                <div class="col-sm-5">
                                    <label for="inputSkills" class="control-label">Brand</label>
                                </div>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBrandleftside" runat="server" class="form-control select2" multiple="multiple" data-placeholder="Select a brand" Style="width: 100%;">
                                    </asp:DropDownList>
                                    <div id="reqddlbrand" style="color: red"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="row">
                        <div class="box">
                            <div class="form-group bpadT">
                                <div class="col-sm-5">
                                    <label for="inputSkills" class="control-label">Tags</label>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txttags" runat="server" data-role="tagsinput" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

            </div>


            <!-- /.col -->
        </div>
        <!-- /.row -->
    </section>
    <!-- /.content -->

    <!-- /.content-wrapper -->


    <!-- The Modal -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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

                                        <div class="nav-tabs-custom popup">
                                            <ul class="nav nav-tabs">
                                                <li class="active"><a href="#General" data-toggle="tab" aria-expanded="true">English</a></li>
                                                <li><a href="#tab" data-toggle="tab">Arabic</a></li>
                                            </ul>
                                            <!-- /.box-header -->
                                            <!-- form start -->
                                            <form class="form-horizontal">
                                                <div class="tab-content">
                                                    <div class="tab-pane fade in active" id="General">
                                                        <div class="form-group">
                                                            <label for="inputEmail3" class="col-sm-12 control-label">SKU*:</label>

                                                            <div class="col-sm-12">
                                                                <input type="text" class="form-control" id="txtpopSKU" maxlength="50" runat="server" onkeypress="return isNumberKey(event);" />
                                                                <div id="skumsg" style="color: red"></div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="inputEmail3" class="col-sm-12 control-label">Master Product Name*:</label>

                                                            <div class="col-sm-12">
                                                                <input type="text" class="form-control" maxlength="500" id="txtpopProduct" runat="server" />
                                                                <div id="productmastermsg" style="color: red"></div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="inputEmail3" class="col-sm-12 control-label">Description:</label>

                                                            <div class="col-sm-12">
                                                                <%--<input type="text" class="form-control" id="Text2" placeholder="Description">--%>
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

                                            <div class="form-group  col-sm-12">

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

                                            </div>

                                            <!-- /.box-body -->
                                            <div class="box-footer">

                                                <button id="btnPopMasterPro" onclick="return addMasterProduct();" class="btn btn-info pull-right">Create</button>

                                            </div>
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


            <!-- Category Dilog Box -->
            <div id="ModelCategory" class="modal">

                <!-- Modal content -->
                <div class="modal-content">
                    <div class="modal-header">
                        <span class="close1">×</span>
                        <h2>Create New Category</h2>
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
                                                    <label for="exampleInputEmail1">Name*</label>
                                                    <input type="text" class="form-control" id="txtcateName" runat="server" />
                                                    <div id="categorynamemsg" style="color: red"></div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Parent Category</label>
                                                    <%-- <select class="form-control" runat="server" id="ddlPOPProductParentCategory">
                                                <%--<option value="1">Category 1</option>
                                                <option value="0">Category 1</option>--%>
                                                    <%--</select>--%>

                                                    <asp:DropDownList ID="ddlPOPProductParentCategory" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Description:</label>
                                                    <textarea class="form-control" runat="server" id="txtdescription"></textarea>
                                                </div>

                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Is Active?</label>
                                                    <select class="form-control" runat="server" id="ddlisactive">
                                                        <option value="1">Yes</option>
                                                        <option value="0">No</option>

                                                    </select>
                                                </div>


                                            </div>
                                            <!-- /.box-body -->

                                            <div class="box-footer">
                                                <button type="submit" id="btnCategoryAdd" onclick="return addCategoryProduct();" class="btn btn-primary">Create</button>
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
        </ContentTemplate>
    </asp:UpdatePanel>

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
        var modalcategory = document.getElementById('ModelCategory');

        // Get the button that opens the modal
        var btncategory = document.getElementById("btnCategory");

        // Get the <span> element that closes the modal
        var spancateory = document.getElementsByClassName("close1")[0];

        // When the user clicks the button, open the modal
        btncategory.onclick = function () {
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

    <%--<script src="http://ajax.microsoft.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js"></script>--%>
    <script src="DragDrop/jquery.tmpl.min.js"></script>
    <script src="DragDrop/modernizr.custom.js"></script>
    <script src="DragDrop/newjs.js"></script>
    <script src="DragDrop/newjs2.js"></script>

    <link href="js/jquery.tagsinput.css" rel="stylesheet" />
    <script src="js/jquery.tagsinput.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_txttags').tagsInput({ width: 'auto', height: 'auto' });
        });

    </script>

</asp:Content>

