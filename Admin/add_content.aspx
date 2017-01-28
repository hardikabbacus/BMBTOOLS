<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_content.aspx.cs" Inherits="Admin_add_content" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="ckeditor/ckeditor.js" type="text/javascript"></script>
    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfprevsort" runat="server" />

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
                    <div class="alert alert-danger alert-dismissible msgsucess" id="lblmsg" visible="false" runat="server">
                        <button class="close" aria-hidden="true" data-dismiss="alert" type="button">×</button>
                        <h4>
                            <i class="icon fa fa-ban"></i>
                            <asp:Literal ID="lblmsgs" runat="server"></asp:Literal>
                        </h4>
                    </div>

                </div>
            </div>
            <!-- left column -->

            <!--/.col (left) -->
            <!-- right column -->
            <div class="col-md-12">
                <!-- Horizontal Form -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title"></h3>
                    </div>


                    <!-- /.box-header -->
                    <!-- form start -->
                    <form class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Content Name </label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtcontentname" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqtxtcontentname" runat="server" ControlToValidate="txtcontentname" Display="Dynamic" ErrorMessage="Please enter content name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Content Description</label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtdesc" runat="server" MaxLength="500" TextMode="MultiLine" class="form-control ckeditor"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Meta Title </label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtmetatitle" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqtxtmetatitle" runat="server" ControlToValidate="txtmetatitle" Display="Dynamic" ErrorMessage="Please enter meta title" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Meta Keyword </label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtmetakey" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqtxtmetakey" runat="server" ControlToValidate="txtmetakey" Display="Dynamic" ErrorMessage="Please enter meta keyword" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Meta Description </label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtmetadesc" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqtxtmetadesc" runat="server" ControlToValidate="txtmetadesc" Display="Dynamic" ErrorMessage="Please enter meta description" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <%--<div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Is Active </label>
                                <div class="col-sm-10">
                                    <div class="label_check">

                                        <asp:CheckBox ID="chkisactive" Checked="true" runat="server" Text=" NOTE:- Please tick this checkbox if you want to display on the website." />

                                    </div>
                                </div>
                            </div>--%>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Sort Order</label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtsortorder" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="rfvsortorder" runat="server"
                                        ControlToValidate="txtsortorder" Display="Dynamic" ErrorMessage="Please enter sort order"
                                        ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="Required" ForeColor="Red" ID="CVsortorder" Operator="DataTypeCheck"
                                        runat="server" Type="Integer" ControlToValidate="txtsortorder" ErrorMessage="Please Enter numeric value for Sort Order"
                                        Display="Dynamic" ValidationGroup="btnsave" EnableViewState="false"></asp:CompareValidator>

                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Is Active </label>
                                <div class="col-sm-10">
                                    <div class="label_check">
                                        <asp:CheckBox ID="chkvisible" Checked="true" runat="server" Text="NOTE:- Please tick this checkbox if you want to display on the website." />


                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <asp:Button ID="btncancel" runat="server" class="btn btn-default" Text="Back" OnClick="btncancel_Click" />
                            <asp:Button ID="btnsubmit" runat="server" class="btn btn-info pull-right" Text="Save" ValidationGroup="btnsave" OnClick="btnsubmit_Click" />
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

