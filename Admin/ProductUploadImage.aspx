<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="ProductUploadImage.aspx.cs" Inherits="Admin_ProductUploadImage" %>

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
                <a class="pop3" href="${filePath}" rel="group1">
                    <img class="imgnew" src="${filePath}" alt="${fileName}" /></a>
                <figcaption><span></span>
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

    <script type="text/javascript">
        //On Page Load
        $(function () {
            $(".select2").select2();
            $("#ContentPlaceHolder1_gvAdmin_ddlsku").select2();
            //$("#tabs").tabs();
            //$(".select2").select2();
        });

        //On UpdatePanel Refresh
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //alert(prm);
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        //alert('tst');
                        $(".select2").select2();
                        $("#ContentPlaceHolder1_gvAdmin_ddlsku").select2();
                        //$("#tabs").tabs();
                    }
                });
            };
        });
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
                <div class=" ">
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>



                                        <div class="col-md-8 leftbox">
                                            <div class="row fwherad">
                                                <div class="col-sm-12 search_bar">
                                                    <div style="float: left;">
                                                        <label for="inputSkills" class="control-label">Recent Images</label>
                                                    </div>

                                                    <div class="" style="display: none">
                                                        <asp:TextBox ID="txtsearch" CssClass="form-control" placeholder="Search" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="" style="display: none">
                                                        <asp:Button ID="imgbtnSearch" runat="server" Text="Search" class="btn btn-info pull-right" OnClick="imgbtnSearch_Click" />
                                                    </div>


                                                    <div class="" style="display: none">
                                                        <%--<asp:DropDownList ID="ddlpage" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"></asp:DropDownList>--%>
                                                    </div>
                                                    <div class="show">
                                                        <label for="inputSkills" class="control-label">Show:</label>
                                                        <div class=" img_value">
                                                            <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control " AutoPostBack="true" OnSelectedIndexChanged="ddlpageSize_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                            <asp:GridView ID="gvAdmin" runat="server" CssClass="table table-bordered table-striped dataTable" GridLines="None"
                                                DataKeyNames="productImagesId" PagerStyle-CssClass="paging-link" AutoGenerateColumns="false"
                                                ShowFooter="false" PagerStyle-HorizontalAlign="Right" Width="100%" OnDataBound="gvAdmin_DataBound"
                                                OnRowCommand="gvAdmin_RowCommand" OnRowDataBound="gvAdmin_RowDataBound">
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
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle />
                                                        <HeaderTemplate>
                                                            <div class="label_check">
                                                                <asp:CheckBox ID="chkHeader" CssClass="headercheckbox" onclick="javascript:SelectAllCheckboxes_1();" runat="server" Text=" "></asp:CheckBox>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hidProductImageIds" Value='<%# Eval("productImagesId")%>' runat="server" />
                                                            <div class="label_check">
                                                                <asp:CheckBox ID="chkDelete" CssClass="innercheckbox" onclick="javascript:UnSelectHeaderCheckbox();" runat="server" Text=" "></asp:CheckBox>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Image" SortExpression="imagename">
                                                        <ItemStyle />
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdmenuimage" runat="server" Value='<%# Eval("imagename")%>' Visible="false" />
                                                            <a id="ancImage" runat="server" class="pop3" rel="group1">
                                                                <img id="imgMenu" runat="server" width="50" height="50" title='<%# Eval("imagename") %>' />
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="SKU">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle />
                                                        <ItemTemplate>
                                                            <asp:Label ID="hidsku" runat="server" Visible="false" Text='<%#Eval("sku") %>'></asp:Label>
                                                            <asp:DropDownList ID="ddlsku" CssClass="form-control select2" runat="server">
                                                            </asp:DropDownList><br />
                                                            <asp:RequiredFieldValidator ID="reqddlsku" ForeColor="Red" runat="server" ErrorMessage="Please select sku."
                                                                ControlToValidate="ddlsku" InitialValue="0" ValidationGroup='<%# "ImgBtnEdit" + Container.DataItemIndex %>' Display="Dynamic"></asp:RequiredFieldValidator>

                                                            <asp:RequiredFieldValidator ID="reqddlskuall" ForeColor="Red" runat="server" ErrorMessage="Please select sku."
                                                                ControlToValidate="ddlsku" InitialValue="0" ValidationGroup="btnupdate" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Image Name" SortExpression="imagename">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductid" runat="server" Visible="false" Text='<%#Eval("productid") %>'></asp:Label>
                                                            <%#Eval("imagename") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Image Label" SortExpression="imgLabel">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblImgLabl" Text='<%#Eval("imgLabel") %>' runat="server"></asp:Label>--%>
                                                            <asp:TextBox ID="txtImgLabl" runat="server" Text='<%# Eval("imgLabel")%>' MaxLength="90" CssClass="form-control"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="UpdateSku" ImageUrl="~/images/edit.jpg"
                                                                ValidationGroup='<%# "ImgBtnEdit" + Container.DataItemIndex %>' CommandArgument='<%# Container.DataItemIndex %>' />

                                                            <asp:ImageButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("productImagesId")%>'
                                                                OnClientClick="return confirm('Are you sure you want to delete image?')" OnClick="ImgBtnDelete_Click" ImageUrl="~/images/delete.jpg" />
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
                                            <div class="box-footer pad col-sm-12">
                                                <div class="col-sm-5">
                                                    <asp:Button ID="btnDelete" runat="server" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" OnClick="btnDelete_Click" Text="Delete" />
                                                    <asp:Button ID="BtnMultiUpload" runat="server" CssClass="btn btn-default" ValidationGroup="btnupdate" OnClientClick="javascript:return CheckUserItemSelectionForUpdate();" OnClick="BtnMultiUpload_Click" Text="Update" />
                                                    <asp:Button ID="BtnShowAllImages" runat="server" CssClass="btn btn-default" OnClick="BtnShowAllImages_Click" Text="View All" />
                                                </div>
                                                <div class="col-sm-7">
                                                    <div id="example1_paginate" class="dataTables_paginate paging_simple_numbers">
                                                        <ul class="pagination ftr">
                                                            <asp:Literal ID="ltrpaggingbottom" runat="server"></asp:Literal>
                                                        </ul>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="col-md-4">
                                    <div class="box box-info padb">
                                        <div class="box-body">
                                            <div class="form-group  fgroup">

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
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnUpload" runat="server" class="btn btn-info pull-right" Text="Upload" OnClick="btnUpload_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>



                </div>
            </div>
        </div>
    </section>

    <script src="js/jquery.min.js"></script>
    <script src="DragDrop/jquery.tmpl.min.js"></script>
    <script src="DragDrop/modernizr.custom.js"></script>
    <script src="DragDrop/newjs.js"></script>

    <script src="js/general.js"></script>
</asp:Content>

