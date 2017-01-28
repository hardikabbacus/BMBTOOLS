<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_language.aspx.cs" Inherits="Admin_add_language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Content Header (Page header) -->
   <section class="content-header">
        <h1>    <asp:Label ID="ltrheading" runat="server"></asp:Label>
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
                        <h3 class="box-title">
                           
                        </h3>
                    </div>

                   

                    <!-- /.box-header -->
                    <!-- form start -->
                    <form class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Language Name </label>

                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtlanguadename" runat="server" class="form-control" ></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="rfvtitle" runat="server" ControlToValidate="txtlanguadename" Display="Dynamic" ErrorMessage="Please enter language name" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Text Align </label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtalign" runat="server" class="form-control" MaxLength="1" ></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ForeColor="Red" ID="reqtxtalign" runat="server" ControlToValidate="txtalign" Display="Dynamic" ErrorMessage="Please enter text align" ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-2 control-label">Is Active </label>
                                <div class="col-sm-10">
                                         <div class="label_check">
                                            <asp:CheckBox ID="chkvisible" Checked="true" runat="server" Text="  NOTE:- Please tick this checkbox if you want to display on the website." />
                                          
                                      
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <%--  <button type="submit" class="btn btn-default">Cancel</button>--%>
                            <asp:Button ID="btncancel" runat="server" class="btn btn-default" Text="Back" OnClick="btncancel_Click" />
                            <%--  <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
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

