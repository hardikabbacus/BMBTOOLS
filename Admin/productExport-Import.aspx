<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="productExport-Import.aspx.cs" Inherits="Admin_productExport_Import" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <%--<h1>Dashboard
                    <small>Control panel</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Dashboard</li>
        </ol>--%>
    </section>

    <!-- Main content -->
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">

            <!-- left column -->

            <!--/.col (left) -->
            <!-- right column -->
            <div class="col-md-12">
                <!-- Horizontal Form -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label ID="ltrheading" runat="server"></asp:Label>
                        </h3>
                    </div>

                    <p class="login-box-msg" id="lblmsg" style="color: red;" runat="server">
                        <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>
                        <asp:Label ID="lblcustom" runat="server" CssClass="Required" Text=""></asp:Label>
                        <asp:Label ID="lbltotal" runat="server" CssClass="Required"></asp:Label>
                        <asp:Label ID="lbltotalcount" runat="server" CssClass="Required"></asp:Label>
                        <asp:Label ID="lbltotalsuccesscount" runat="server" CssClass="Required"></asp:Label>
                        <asp:Label ID="lblerrorinrow" runat="server" CssClass="Required"></asp:Label>
                        <asp:Label ID="lbltotalerrorcount" runat="server" CssClass="Required"></asp:Label>
                        <asp:Label ID="lblError" runat="server" CssClass="Required"></asp:Label>
                    </p>

                    <!-- /.box-header -->
                    <!-- form start -->
                    <form class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label"></label>
                                <div class="col-sm-10">
                                    <asp:Button ID="btngeneratecsv" runat="server" Text="Generate Product.CSV" OnClick="btngeneratecsv_Click" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-12 control-label">OR</label>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label"></label>

                                <div class="col-sm-5">
                                    <asp:FileUpload ID="fluUploadCsv" runat="server" />
                                </div>
                                <div class="col-sm-5">
                                    <asp:Button ID="btnUpload" runat="server" Text="Update Products" OnClick="btnUpload_Click" ValidationGroup="btnsave" />
                                </div>
                            </div>

                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <%--  <button type="submit" class="btn btn-default">Cancel</button>--%>
                        </div>
                        <!-- /.box-footer -->
                    </form>
                </div>
                <!-- /.box -->

            </div>
            <!--/.col (right) -->

        </div>

    </section>
    <!-- /.content -->
</asp:Content>

