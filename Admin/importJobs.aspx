<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="importJobs.aspx.cs" Inherits="Admin_importJobs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/jquery-ui.css" rel="stylesheet" />
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
            $("#btnproductExportImport").click();
        }

        $(document).ready(function () {
            SearchKeywordImportJobs();
        });

        function SearchKeywordImportJobs() {
            $("#ContentPlaceHolder1_txtsearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/SearchKeywordImportJobs",
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Import Jobs
            <small></small>
        </h1>
        <input type="button" id="btnproductExportImport" class="btn btn-info pull-right" title="Export Import" value="Import" />
        <ol class="breadcrumb"></ol>
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

                <div class="new_structure">

                    <div class="srch_bar">
                        <div class="col-sm-8 bg_box">
                            <h4>Search</h4>
                            <div class="txt_div">
                                <asp:TextBox ID="txtsearch" CssClass="form-control" placeholder="Name" runat="server"></asp:TextBox>
                                <asp:Button ID="imgbtnSearch" runat="server" class="btn btn-info pull-right" Text="Search" OnClick="imgbtnSearch_Click" />
                            </div>
                        </div>

                        <div class="col-sm-4 bg_box">
                            <h4>Filter</h4>
                            <div class="txt_div">
                                <asp:DropDownList ID="ddlsearch" class="form-control" runat="server" OnSelectedIndexChanged="ddlsearch_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Product</asp:ListItem>
                                    <asp:ListItem Value="2">Inventory</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="btnApply" Text="Apply" runat="server" class="btn btn-info pull-right" OnClick="btnApply_Click" />
                            </div>
                        </div>
                    </div>

                </div>

                <div class="box">
                    <div class="box-header bdrbtm">
                        <div class="col-sm-10 ">
                            <h3 class="box-title">Import History</h3>
                        </div>
                        
                        <div class="col-sm-2 cmn_show">
                            <label for="inputSkills" class="control-label">Show:</label>
                            <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">

                                <div class="col">
                                    <label for="inputSkills" class="col-sm-4 control-label sel_job_lbl"></label>
                                    <div class="col-sm-3 imp_wrp">
                                    </div>
                                </div>

                                <div class="col-sm-12 ">

                                    <asp:GridView ID="gvImportjob" runat="server" AllowSorting="true" OnSorting="gvImportjob_Sorting" CssClass="table table-bordered table-striped dataTable"
                                        GridLines="None" DataKeyNames="id" PagerStyle-CssClass="paging-link" role="grid" OnRowDataBound="gvImportjob_RowDataBound"
                                        AutoGenerateColumns="false" ShowFooter="false"
                                        PagerStyle-HorizontalAlign="Right" Width="100%" OnRowCommand="gvImportjob_RowCommand">
                                        <HeaderStyle CssClass="gridheader" />
                                        <RowStyle CssClass="roweven" />
                                        <AlternatingRowStyle CssClass="roweven" />
                                        <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            <div class="message error">
                                                <h6>No record found for jobs.</h6>
                                            </div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle Width="50" />
                                                <HeaderTemplate>
                                                    <div class="label_check">
                                                        <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" Text=" " runat="server"></asp:CheckBox>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="label_check">
                                                        <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" Text=" " runat="server"></asp:CheckBox>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="File Name" SortExpression="importfilename">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfilename" runat="server" Text='<%#Eval("importfilename") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblimpfile" runat="server" Text='<%#Eval("ImportType") %>' Visible="false"></asp:Label>
                                                    <%--<a id="BtnRedirect" runat="server"><%#Eval("importfilename")%></a>--%>

                                                    <a id="BtnRedirect" runat="server" href='<%# "viewImportJobDetails.aspx?id=" + Eval("id") +"&filename="+Eval("importfilename")+ "&iptp="+ Eval("ImportType") + ""%>'><%#Eval("importfilename")%></a>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" SortExpression="filestatus">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%--<%#Eval("filestatus")%>--%>
                                                    <asp:Label ID="lblAvailData" runat="server" Text='<%#Eval("FileDataAvaliable") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblCompleteIncomplete" runat="server" Text='<%#Eval("FileCompleteIncompleteStatus") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblfilestatus" runat="server" Text=" "></asp:Label>

                                                    <%--<asp:Label ID="lblfilestatus" runat="server" Text='<%# Convert.ToInt32(Eval("FileCompleteIncompleteStatus"))!=0?"For Review":"Complete" %>'></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Type" SortExpression="ImportType">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblImportType" runat="server" Text='<%# Convert.ToInt32(Eval("ImportType"))==1?"Product":"Inventory" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date" SortExpression="createDate">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%# Server.HtmlEncode(Eval("createDate").ToString())%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="Buttonremove" runat="server" CssClass="btn btn-info  btnimpexp" CommandArgument='<%#Eval("id") %>' CommandName="Edit">Remove</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Visible="true" Position="Bottom" Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />
                                    </asp:GridView>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pagi col-sm-12">
                        <div class="row">
                            <div class="col-sm-12 import_job_label">
                                <div aria-live="polite" role="status" id="example1_info" class="dataTables_info">
                                    <asp:Literal ID="ltrcountrecord" runat="server"></asp:Literal>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="imgbtnDelete" runat="server" class="btn ftr_btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="imgbtnDelete_Click" Text="Delete" />
                        <div class="pagi">
                            <div class="row">

                                <div class="col-sm-7">
                                    <div id="example1_paginate" class="dataTables_paginate paging_simple_numbers">
                                        <ul class="pagination btn">
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
    </section>

    <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close">×</span>
                <h2>Export Import Wizard</h2>
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
                                            <label for="inputEmail3" class="col-sm-3 control-label">Select Jobs :</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="ddlselecttype" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="0">-Select-</asp:ListItem>
                                                    <asp:ListItem Value="1">Product</asp:ListItem>
                                                    <asp:ListItem Value="2">Inventory</asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:RequiredFieldValidator CssClass="Required" ID="reqddlselecttype" ForeColor="Red" runat="server"
                                                    ControlToValidate="ddlselecttype" InitialValue="0" Display="Dynamic" ErrorMessage="Please select type"
                                                    ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label">Select File :</label>
                                            <div class="col-sm-9">
                                                <asp:FileUpload ID="file" runat="server" CssClass="file_upld" /><br />
                                                <asp:RequiredFieldValidator CssClass="Required" ID="reqfile" ForeColor="Red" runat="server"
                                                    ControlToValidate="file" Display="Dynamic" ErrorMessage="Please select type"
                                                    ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-3 control-label"></label>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnimport" runat="server" class="btn btn-info pull-right" OnClick="btnimport_Click" ValidationGroup="btnsave" Text="Import" />
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
        var span = document.getElementsByClassName("close");

        // When the user clicks the button, open the modal
        btn.onclick = function () {
            modal.style.display = "block";
        }

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
            window.location.href = "importjobs.aspx";
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

