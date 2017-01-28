<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>AdminBMB | Log in</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.6 -->
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css">
    <!-- Theme style -->
    <link href="dist/css/AdminLTE.min.css" rel="stylesheet" />
    <!-- iCheck -->
    <link href="plugins/iCheck/square/blue.css" rel="stylesheet" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->
</head>
<body class="hold-transition login-page">
    <form id="form1" runat="server">
        <input type="hidden" name="prevurl" id="prevurl" runat="server" />
        <div class="login-box">
            <div class="login-logo">
                <a href="home.aspx">
                    <img src="../images/main_logo.png" /></a>
            </div>
            <!-- /.login-logo -->
            <div class="login-box-body">
                <p class="login-box-msg">Sign in to start your session</p>

                <p class="login-box-msg" style="color: red;" id="lblmsg">
                    <asp:Literal ID="ltrmessage" runat="server"></asp:Literal>
                </p>

                <form method="post">
                    <div class="form-group has-feedback">
                        <asp:TextBox ID="txtemail" runat="server" class="form-control" placeholder="Username" MaxLength="100"></asp:TextBox>
                        <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
                        <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtUser" runat="server" Font-Bold="false" ForeColor="Red" ControlToValidate="txtemail" ErrorMessage="Please enter username" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group has-feedback">
                        <asp:TextBox ID="txtpassword" runat="server" class="form-control" TextMode="Password" placeholder="Password" MaxLength="20"></asp:TextBox>
                        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                        <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtpassword" runat="server" ForeColor="Red" Font-Bold="false" ControlToValidate="txtpassword" ErrorMessage="Please enter password" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="checkbox icheck">
                                <label>
                                    <input type="checkbox">
                                    Remember Me
                                </label>
                            </div>
                        </div>
                        <!-- /.col -->
                        <div class="col-xs-6 clslogin">
                            <asp:Button ID="btnlogin" runat="server" CssClass="btn btn-primary btn-block btn-flat" Text="Sign In" OnClick="btnlogin_Click" />
                            <a href="forgotpassword.aspx" class="frgt_pass">I forgot my password</a>
                        </div>
                        <!-- /.col -->
                    </div>
                </form>

                <%--<div class="social-auth-links text-center">
                <p>- OR -</p>
                <a href="#" class="btn btn-block btn-social btn-facebook btn-flat"><i class="fa fa-facebook"></i>Sign in using
        Facebook</a>
                <a href="#" class="btn btn-block btn-social btn-google btn-flat"><i class="fa fa-google-plus"></i>Sign in using
        Google+</a>
            </div>
            <!-- /.social-auth-links -->--%>

                <%--<a href="forgotpassword.aspx">I forgot my password</a>--%>
                <%--<asp:LinkButton ID="lnkforgotpassword" runat="server" OnClick="lnkforgotpassword_Click">I forgot my password</asp:LinkButton>--%>
                <br>
                <%--<a href="register.html" class="text-center">Register a new membership</a>--%>
            </div>
            <!-- /.login-box-body -->
        </div>
        <!-- /.login-box -->
    </form>


    <!-- jQuery 2.2.3 -->
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <!-- Bootstrap 3.3.6 -->
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="plugins/iCheck/icheck.min.js"></script>
    <script>
        $(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '20%' // optional
            });
        });
    </script>
</body>
</html>
