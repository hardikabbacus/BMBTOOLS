<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_brand.aspx.cs" Inherits="Admin_add_brand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js"></script>
    <%--<script src="http://ajax.microsoft.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js"></script>--%>
    <script src="DragDrop/jquery.tmpl.min.js"></script>
    <script src="DragDrop/modernizr.custom.js"></script>
    <script src="DragDrop/newjs1.js" id="dragjs" type="text/javascript"></script>

    <script src="ckeditor/ckeditor.js" type="text/javascript"></script>

    <script id="imageTemplate" type="text/x-jquery-tmpl">
        <div class="imageholder" id="divimg${fileNamewithoutspace}">
            <figure>
                <img class="imgnew" src="${filePath}" alt="${fileName}" />
                <figcaption class="equal"><span></span><p>${fileName}</p></figcaption>
            </figure>
            <a onclick="deleteimg(this.id);" href="javascript:void(0);" id="${fileNamewithoutspace}">delete</a>
        </div>

    </script>
  <%--  <script type="text/javascript">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfprevsort" runat="server" />
    <asp:HiddenField ID="hdtotallanguage" runat="server" Value="0" />
    <input type="hidden" id="hiddeletecount" runat="server" />
    <!-- Content Header (Page header) -->

    <!-- Main content -->

    <section class="content-header">
        <h1>
            <asp:Label ID="ltrheading" runat="server"></asp:Label>
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
    </section>

    <section class="content">

        <div class="row">

            <div class="col-md-12">
                <!-- Horizontal Form -->

                <div class="box-body">
                    <div class="alert alert-success alert-dismissible msgsucess" id="lblmsg" runat="server" visible="false">
                        <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                        <h4>
                            <i class="icon fa fa-check"></i>
                            <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>
                        </h4>
                    </div>
                    <div class="alert alert-danger alert-dismissible msgsucess" id="lblmsgdiv" runat="server" visible="false">
                        <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                        <h4>
                            <i class="icon fa fa-ban"></i>
                            <asp:Literal ID="lblalert" runat="server"></asp:Literal>
                        </h4>
                    </div>
                </div>
            </div>


            <div class="col-md-3">
                <!-- Horizontal Form -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                        <h3 class="box-title">Brand</h3>
                        <a href="add_brand.aspx" class="btn btn-info pull-right">New</a>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->


                    <div class="box-body">

                        <asp:DataList ID="ddlbrandlist" runat="server" class="data_cat">
                            <ItemTemplate>
                                <asp:Label ID="id" runat="server" Text='<%# Eval("brandId") %>' Visible="false"></asp:Label>
                                <a href='<%# Eval("brandId","add_brand.aspx?flag=edit&id={0}") %>'>
                                    <%# Eval("brandName") %>
                                </a>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->

            </div>
            <!-- /.col -->
            <div class="col-md-5">
                <div class="nav-tabs-custom">
                    <ul class="nav nav-tabs">

                        <asp:Literal ID="ltrtab" runat="server"></asp:Literal>
                    </ul>
                    <div class="tab-content">

                        <asp:Literal ID="ltrcategorylanguages" runat="server"></asp:Literal>


                        <div class="form-group">
                            <label for="inputName" class="col-sm-12 control-label">Sort Order</label>

                            <div class="col-sm-12">
                                <asp:TextBox ID="txtsortorder" runat="server" MaxLength="6" class="form-control"></asp:TextBox>
                                <div id="reqtxtsortorder" style="color: red"></div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputSkills" class="col-sm-12 control-label">Is Active? </label>

                            <div class="col-sm-12">
                                <asp:DropDownList ID="ddlactive" runat="server" class="form-control">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-offset-0 col-sm-12">
                                <asp:Button ID="btncancel" runat="server" class="btn btn-default" Text="Delete Brand" OnClientClick="return confirm('Do you want to delete this record?');" OnClick="btncancel_Click" />
                                <asp:Button ID="btnsubmit" runat="server" class="btn btn-info pull-right" Text="Save" CausesValidation="true" OnClientClick="return validationbrand();" ValidationGroup="btnsave" OnClick="btnsubmit_Click" />
                            </div>
                        </div>


                        <!-- /.tab-pane -->
                    </div>
                    <!-- /.tab-content -->
                </div>
                <!-- /.nav-tabs-custom -->
            </div>

            <div class="col-md-4">
                <!-- Horizontal Form -->
                <div class="box box-info">

                    <!-- /.box-header -->
                    <!-- form start -->


                    <div class="box-body">

                        <div class="form-group">
                            <label for="exampleInputFile">Image Name </label>


                            <%--<asp:FileUpload ID="txtImageName" runat="server" CssClass="input1" />--%>
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
                                            OnClick="lnkdelimg_Click"> Delete</asp:LinkButton>
                            </span>
                            <br />

                            <div id="regimagenamemsg" style="color: red"></div>

                            <asp:HiddenField ID="hdImage" runat="server" Value=""></asp:HiddenField>
                            <asp:HiddenField ID="hdImage2" runat="server" Value=""></asp:HiddenField>

                        </div>
                    </div>
                    <!-- /.box-body -->

                </div>
                <!-- /.box -->

            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->

    </section>
    <!-- /.content -->

    <!-- /.content-wrapper -->


</asp:Content>

