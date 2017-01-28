<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="productImageUpload.aspx.cs" Inherits="Admin_productImageUpload" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/jquery-ui.css" rel="stylesheet" />

    <script>
        $(function () {
            //Initialize Select2 Elements
            $(".select2").select2();
        });
    </script>

    <%-------------------------- Drag drop --------------------------%>

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
    </script>

    <script type="text/javascript">

        //On Page Load
        $(function () {
            $(".select2").select2();
            $("#ContentPlaceHolder1_gvAdmin_ddlsku").select2();

        });

        //On UpdatePanel Refresh
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //alert(prm);
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        $(".select2").select2();
                        $("#ContentPlaceHolder1_gvAdmin_ddlsku").select2();
                    }
                });
            };
        });

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            SearchTextSKUAutocomplete();
        });

        function SearchTextSKUAutocomplete() {
            $("#ContentPlaceHolder1_txtsearch").autocomplete({
                //alert('in');
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../WebService.asmx/BindAllSKU",
                        data: "{'sku':'" + document.getElementById('ContentPlaceHolder1_txtsearch').value + "'}",
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
                    $('#ContentPlaceHolder1_txtskumaster').val(text);
                    $('#ContentPlaceHolder1_txtskumaster').trigger('click');
                    //FindSkurecord(text);
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <section class="content-header">
        <h1>Manage Image 
            <small></small>
        </h1>
        <ol class="breadcrumb"></ol>
        <%--<asp:Button ID="btnUpload" runat="server" class="btn btn-info pull-right" Text="Upload" OnClick="btnUpload_Click" />--%>
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Search Image</h3>
                            </div>



                            <div class="box-body">
                                <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                                    <div class="row">

                                        <div class="col-sm-12 search_bar">


                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtsearch" CssClass="form-control" placeholder="Search" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button ID="imgbtnSearch" runat="server" Text="Search" class="btn btn-info pull-right" OnClick="imgbtnSearch_Click" />
                                            </div>

                                            <div style="display: none;">
                                                <%--<asp:DropDownList ID="ddlpage" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"></asp:DropDownList>--%>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged"></asp:DropDownList>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Image List</h3>

                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="gvAdmin" runat="server" AllowSorting="true" OnSorting="gvAdmin_Sorting" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="productImagesId" PagerStyle-CssClass="paging-link" role="grid"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    OnPageIndexChanging="gvAdmin_PageIndexChanging" OnRowEditing="gvAdmin_RowEditing" OnRowUpdating="gvAdmin_RowUpdating" OnRowCancelingEdit="gvAdmin_RowCancelingEdit"
                                    PagerStyle-HorizontalAlign="Right" Width="100%"
                                    OnRowDataBound="gvAdmin_RowDataBound">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for product images.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="50" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle Width="50" />
                                            <HeaderTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" runat="server" Text=" "></asp:CheckBox>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="label_check">
                                                    <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" runat="server" Text=" "></asp:CheckBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image" SortExpression="imagename">
                                            <ItemStyle Width="200" />
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdmenuimage" runat="server" Value='<%# Eval("imagename")%>' Visible="false" />
                                                <a id="ancImage" runat="server" class="pop3" rel="group1">
                                                    <img id="imgMenu" runat="server" width="50" height="50" title='<%# Eval("imagename") %>' />
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SKU" SortExpression="sku">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%# Eval("sku")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%# Eval("sku")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="ddlsku" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                <br />
                                                <asp:RequiredFieldValidator ID="reqddlsku" ForeColor="Red" runat="server" ErrorMessage="Please search sku."
                                                    ControlToValidate="ddlsku" InitialValue="0" ValidationGroup='<%# "ImgBtnUpdate" + Container.DataItemIndex %>'></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image Name" SortExpression="imagename">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%#Eval("imagename") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Image Label" SortExpression="imgLabel">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblImgLabl" Text='<%#Eval("imgLabel") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblImgLabl" runat="server" Text='<%# Eval("imgLabel")%>' Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtImgLabl" runat="server" Text='<%# Eval("imgLabel")%>' MaxLength="90" CssClass="form-control"></asp:TextBox>
                                                <br />
                                                <%--<asp:RequiredFieldValidator ForeColor="Red" ID="reqddlsku" runat="server"
                                                        ControlToValidate="ddlsku" Display="Dynamic" ErrorMessage="Please select sku" InitialValue="0"
                                                        ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>--%>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action">
                                            <%--<HeaderStyle Width="90" HorizontalAlign="Center" />--%>
                                            <ItemStyle Width="90" HorizontalAlign="Center" />
                                            <EditItemTemplate>
                                                <%--<asp:LinkButton ID="ButtonUpdate" runat="server" CssClass="btn btn-info btn-md mar" ValidationGroup="btnsave" CommandName="Update">Confirm</asp:LinkButton>
                                                            <asp:LinkButton ID="ButtonCancel" runat="server" CssClass="btn btn-info btn-md mar" CommandName="Cancel">Cancel</asp:LinkButton>--%>
                                                <asp:ImageButton ID="ImgBtnUpdate" runat="server" CommandName="Update" ValidationGroup='<%# "ImgBtnUpdate" + Container.DataItemIndex %>' ImageUrl="~/images/ok.jpg" />
                                                <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/cancel.jpg" />

                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="ButtonEdit" runat="server" CssClass="btn btn-info" CommandName="Edit">Modify</asp:LinkButton>--%>
                                                <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                                                <asp:ImageButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("productImagesId")%>' OnClientClick="return confirm('Are you sure you want to delete image?')" OnClick="ImgBtnDelete_Click" ImageUrl="~/images/delete.jpg" />
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
                                    <asp:Button ID="btnDelete" runat="server" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="btnDelete_Click" Text="Delete" />

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

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>

    <script src="js/jquery.min.js"></script>

    <%--<script src="http://ajax.microsoft.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js"></script>--%>
    <script src="DragDrop/jquery.tmpl.min.js"></script>
    <script src="DragDrop/modernizr.custom.js"></script>
    <%--<script src="DragDrop/newjs.js"></script>--%>

    <script src="js/general.js"></script>
</asp:Content>

