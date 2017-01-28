<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="add_category.aspx.cs" Inherits="Admin_add_category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>
    <script language="javascript">
        function showsubmenu(id, cid) {
            var subcat = document.getElementById(id);
            if (subcat.style.display == 'none') {
                subcat.style.display = 'block';
                $("#" + cid).addClass("open_cat");
            }
            else {
                subcat.style.display = 'none';
                $("#" + cid).removeClass("open_cat");
            }

        }
    </script>
    <script type="text/javascript">
        function LanguageTxt(name) {
            var flag = GetParameterValues('flag');
            if (flag == 'undefined' || flag == 'edit') {
            }
            else {
                var Total = document.getElementById("ContentPlaceHolder1_hdtotallanguage").value;
                for (var i = 0; i < Total; i++) {
                    document.getElementById("txtcountryname" + (i + 1)).value = name;
                }
            }
        }

        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }
    </script>

    <script id="imageTemplate" type="text/x-jquery-tmpl">
        <div class="imageholder" id="divimg${fileNamewithoutspace}">
            <figure>
                <img class="imgnew" src="${filePath}" alt="${fileName}" />
                <figcaption class="equal"><span></span>
                    <p>${fileName}</p>
                </figcaption>
            </figure>
            <a onclick="deleteimg(this.id);" href="javascript:void(0);" id="${fileNamewithoutspace}">delete</a>
        </div>

    </script>
    <%-- <script type="text/javascript">
        function deleteimg(imgid) {
            //alert(imgid);
            //alert($("#divimg" + imgid).val());
            $("#divimg" + imgid).remove();
            var filename = "newr" + imgid;
            var hiddenfl = "<input type='hidden' value='" + filename + "' name='" + filename + "'>";
            //alert(hiddenfl);
            $("#dltimgtags").append(hiddenfl);
        }
    </script>--%>

    <%-------------------------- Drag drop end--------------------------%>

    <script type="text/javascript" src="../fancybox/jquery.mousewheel-3.0.4.pack.js"></script>
    <script type="text/javascript" src="../fancybox/jquery.fancybox-1.3.4.pack.js"></script>
    <script type="text/javascript" src="../fancybox/at-jquery.js"></script>
    <link rel="stylesheet" type="text/css" href="../fancybox/jquery.fancybox-1.3.4.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfprevsort" runat="server" />
    <asp:HiddenField ID="hdtotallanguage" runat="server" Value="0" />
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>
            <asp:Label ID="ltrheading" runat="server"></asp:Label>
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">

            <div class="col-md-12">
                <div class="box-body">

                    <!-- Horizontal Form -->
                    <div class="" id="lblp" visible="false" runat="server">
                        <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                        <h4>
                            <i class="icon fa fa-check" id="iconemsg" runat="server"></i>
                            <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>
                        </h4>
                    </div>

                </div>
            </div>

            <div class="col-md-3">
                <!-- Horizontal Form -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                        <h3 class="box-title">Categories</h3>
                        <div class="col-md-12 nw">
                            <a href="add_category.aspx" class="btn btn-info pull-right">New</a>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->


                    <div class="box-body">

                        <asp:DataList ID="DataList1" runat="server" CssClass="data_cat" OnItemDataBound="DataList1_ItemDataBound">
                            <ItemTemplate>
                                <asp:Label ID="id" runat="server" Text='<%# Eval("categoryId") %>' Visible="false"></asp:Label>
                                <a href='<%# Eval("categoryId","add_category.aspx?flag=edit&id={0}") %>'>
                                    <%# Eval("categoryName") %>
                                </a>
                                <asp:HyperLink ID="MainCat" Text='+' CssClass="anc_cat" runat="server" NavigateUrl='javascript:void(0)'></asp:HyperLink>

                                <asp:Panel ID="panelsubcat" runat="server" CssClass="subpro" Style="display: none;">
                                    <asp:DataList ID="subcat" runat="server">
                                        <ItemTemplate>
                                            <a href='<%# Eval("categoryId","add_category.aspx?flag=edit&id={0}") %>'>
                                                <%# Eval("categoryName") %>
                                            </a>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </asp:Panel>

                            </ItemTemplate>

                        </asp:DataList>

                    </div>
                    <!-- /.box-body -->


                </div>
                <!-- /.box -->

            </div>

            <!-- right column -->
            <div class="col-md-5">
                <!-- Horizontal Form -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                        <h3 class="box-title">Add Category</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->

                    <form role="form">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="exampleInputEmail1">Category  Name </label>

                                <asp:TextBox ID="txtcategoryname" runat="server" class="form-control" MaxLength="90"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Required" ID="rfvtitle" runat="server" ForeColor="Red" ControlToValidate="txtcategoryname" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please enter category name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>

                            </div>
                            <div class="form-group">
                                <label for="exampleInputEmail1">Parent Category </label>

                                <asp:DropDownList ID="ddlParentcategory" runat="server" class="form-control">
                                </asp:DropDownList>

                            </div>

                            <div class="form-group">
                                <label for="exampleInputEmail1">Category Description  </label>

                                <asp:TextBox ID="txtcategorydes" TextMode="MultiLine" runat="server" class="form-control" MaxLength="500"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtPageName" runat="server" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtcategorydes" Display="Dynamic" ErrorMessage="Please enter category description" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>

                            </div>

                            <div class="form-group">
                                <label for="exampleInputEmail1">Sort Order</label>

                                <asp:TextBox ID="txtsortorder" runat="server" class="form-control" MaxLength="6"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Required" ID="rfvsortorder" ForeColor="Red" runat="server" SetFocusOnError="true"
                                    ControlToValidate="txtsortorder" Display="Dynamic" ErrorMessage="Please enter sort order"
                                    ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                <asp:CompareValidator CssClass="Required" ID="CVsortorder" ForeColor="Red" Operator="DataTypeCheck" SetFocusOnError="true"
                                    runat="server" Type="Integer" ControlToValidate="txtsortorder" ErrorMessage="Please enter numeric value for sort order"
                                    Display="Dynamic" ValidationGroup="btnsave" EnableViewState="false"></asp:CompareValidator>


                            </div>

                            <div class="form-group">



                                <label for="exampleInputEmail1">Is Active? </label>

                                <asp:DropDownList ID="ddlactive" runat="server" class="form-control">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>

                            </div>

                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">

                            <asp:Button ID="btncancel" runat="server" class="btn btn-default" Text="Delete Category" OnClientClick="return confirm('Do you want to delete this record?');" OnClick="btncancel_Click" />
                            <asp:Button ID="btnsubmit" runat="server" class="btn btn-info pull-right" Text="Save" ValidationGroup="btnsave" OnClick="btnsubmit_Click" />
                        </div>
                        <!-- /.box-footer -->
                    </form>
                </div>
                <!-- /.box -->

            </div>
            <!--/.col (right) -->

            <!-- right column -->
            <div class="col-md-4">
                <!-- Horizontal Form -->
                <div class="box box-info">

                    <!-- /.box-header -->
                    <!-- form start -->
                    <div class="box-body">

                        <div class="form-group">
                            <label for="exampleInputFile">Image Name </label>

                            <div id="drop-zone">
                                Drop files here...
                                <div id="clickHere">
                                    or click here..
                                    <input type="file" name="file[]" id="filenew" multiple />
                                </div>

                                <div id="result">
                                </div>
                            </div>
                            <div id="dltimgtags"></div>

                            <span id="spimg" runat="server" visible="false" style="display: table-row-group; float: right; padding-top: 8px; width: 50%">
                                <a id="ancImage" runat="server" class="pop3" rel="group1">Preview </a>|
                                        <asp:LinkButton ID="lnkdelimg" runat="server" CssClass="login-tab" OnClientClick="javascript:return confirm('Are you sure you want to delete image ?');"
                                            OnClick="lnkdelimg_Click"> Delete Image</asp:LinkButton>
                            </span>

                            <asp:HiddenField ID="hdImage" runat="server" Value=""></asp:HiddenField>
                            <asp:HiddenField ID="hdImage2" runat="server" Value=""></asp:HiddenField>

                        </div>

                        <div class="form-group" id="tdlanguages">
                            <label for="exampleInputEmail1">Arabic Name  </label>

                            <asp:TextBox ID="txtarbicname" runat="server" class="form-control" MaxLength="90"></asp:TextBox>

                        </div>
                        <div class="form-group">
                            <label for="exampleInputEmail1">Arabic Description  </label>

                            <asp:TextBox ID="txtarbicdes" runat="server" TextMode="MultiLine" MaxLength="500" class="form-control"></asp:TextBox>

                        </div>

                    </div>
                    <!-- /.box-body -->


                </div>
                <!-- /.box -->

            </div>
            <!--/.col (right) -->
        </div>

    </section>
    <!-- /.content -->

    <script src="js/jquery.min.js"></script>
    <%--<script src="http://ajax.microsoft.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js"></script>--%>
    <script src="DragDrop/jquery.tmpl.min.js"></script>
    <script src="DragDrop/modernizr.custom.js"></script>
    <script src="DragDrop/newjs1.js"></script>

</asp:Content>

