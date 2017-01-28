<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="add_smartcatelogs.aspx.cs" Inherits="Admin_add_smartcatelogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="DragDropList/ListDragDrop.js"></script>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.1/themes/base/jquery-ui.css" />
    <%-- <script src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script src="http://code.jquery.com/ui/1.9.1/jquery-ui.js"></script>--%>

    <style>
        .form-group {
            float: left;
            width: 100%;
        }
    </style>
    <script>
        $(function () {
            //Initialize Select2 Elements
            $(".select2").select2();
        });

        $(document).ready(function () {
            singleDisplay();
        });

        function singleDisplay() {
            if (document.getElementById('ContentPlaceHolder1_rbtCategory').checked) {
                //alert("category");
                document.getElementById('SkuList').style.display = 'none';
                document.getElementById('CategoryList').style.display = '';
            }
            else if (document.getElementById('ContentPlaceHolder1_rbtsku').checked) {
                //alert("sku");
                document.getElementById('SkuList').style.display = '';
                document.getElementById('CategoryList').style.display = 'none';
            }
        }

        //drag drop list
        $(document).ready(function () {
            $('#ContentPlaceHolder1_lstDropCatSku').children().draggable();
        });

    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>


    <asp:HiddenField ID="hidbrandid" runat="server" Value="" />
    <asp:HiddenField ID="hidlogo" Value="" runat="server" />
    <section class="content-header">

        <h1>Smart Catalog
            <small></small>
        </h1>
        <%--<a href="add_order.aspx" class="btn btn-info pull-right">New Order</a>--%>
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

            <div class="col-md-6">
                <div class="box">
                    <div class="box-header">
                        <div class="col-md-12">
                            <h3 class="box-title">Catalogs Name</h3>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-md-12 search_bar">
                                    <asp:TextBox ID="txtcatalogname" CssClass="form-control" placeholder="Choose name to save for this catalogs" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="Required" ID="reqtxtcatalogname" ForeColor="Red" runat="server"
                                        ControlToValidate="txtcatalogname" Display="Dynamic" ErrorMessage="Please enter catalogs name"
                                        ValidationGroup="btnsave" EnableViewState="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="col-md-6">
                <div class="box">
                    <div class="box-header">
                        <div class="col-md-12">
                            <h3 class="box-title">Prepared For (Optional)</h3>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-md-12 search_bar">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtpreparedfor" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:FileUpload ID="uploadlogo" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="box">
                    <div class="box-header bdrbtm">
                        <div class="col-md-12">
                            <h3 class="box-title"></h3>
                            <div class="label_radio">
                                <asp:RadioButton ID="rbtCategory" GroupName="rbt" Checked="true" OnClick="javascript:singleDisplay();" runat="server" Text="By Category" />
                            </div>
                            <div class="label_radio">
                                <asp:RadioButton ID="rbtsku" GroupName="rbt" runat="server" OnClick="javascript:singleDisplay();" Text="By SKU" />
                            </div>
                        </div>
                    </div>


                    <div class="form-horizontal" id="CategoryList">

                        <div class="box-body">
                            <asp:UpdatePanel ID="UpdatePanelListBox" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="col-md-6">
                                        <label for="category" class="col-md-12 control-label">Categories</label>
                                        <div class="form-group">
                                            <%--<asp:TextBox ID="txttags" runat="server" data-role="tagsinput" CssClass="form-control"></asp:TextBox>--%>
                                            <asp:ListBox ID="lstCategory" runat="server" Width="100%" Height="150px" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnAddtoArrange" runat="server" Text="Add to Arrange" OnClick="btnAddtoArrange_Click" class="btn btn-info pull-right" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="arrangecategory" class="col-md-12 control-label">Arrange Categories</label>
                                        <div class="form-group">
                                            <asp:ListBox ID="lstDropCatSku" runat="server" Width="100%" Height="200px" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                        </div>
                                        <ul id="sortable1" class="connectedSortable" style="display: none;">
                                            <asp:Literal ID="ltrCat" runat="server"></asp:Literal>
                                        </ul>
                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddtoArrange" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>

                    </div>

                    <div class="form-horizontal" id="SkuList" style="display: none;">
                        <div class="box-body">
                            <div class="col-md-6">
                                <label for="category" class="col-md-12 control-label">SKU</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtSKuList" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                    <div class="form-group">
                                        <asp:Button ID="btnCheck" runat="server" Text="Add to Arrange" OnClick="btnCheck_Click" class="btn btn-info pull-right" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>

            <div class="col-md-4">
                <div class="box">
                    <div class="box-header">
                        <div class="col-md-12">
                            <h3 class="box-title">Options</h3>
                        </div>
                    </div>
                    <div class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="productphoto" class="col-md-8 control-label">Only Product with photo</label>
                                <div class="col-md-4">
                                    <div class="label_check">
                                        <asp:CheckBox ID="chkProductPhoto" runat="server" Text=" " />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="Brand" class="col-md-4 control-label">Brands</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlcatalogsBrand" runat="server" class="form-control select2" multiple="multiple" data-placeholder="Select a brand"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="pricelavel" class="col-md-4 control-label">Price Level</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlpricelavel" runat="server" class="form-control">
                                        <asp:ListItem Value="0">No Price</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="30">30</asp:ListItem>
                                        <asp:ListItem Value="40">40</asp:ListItem>
                                        <asp:ListItem Value="50">50</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="pricerangs" class="col-md-4 control-label">Price Range</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlpricerange" runat="server" class="form-control">
                                        <asp:ListItem Value="L">Less Than</asp:ListItem>
                                        <asp:ListItem Value="G">Greater Than</asp:ListItem>
                                        <asp:ListItem Value="E">Equal To</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtpricerange" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Button ID="btnGeneratecatalogs" runat="server" Text="Generate Catalogs" ValidationGroup="btnsave" OnClick="btnGeneratecatalogs_Click" OnClientClick="GetBrandValues();" class="btn btn-info pull-right" />
                                </div>
                                <div class="col-md-6">
                                    <asp:Button ID="btnpreviewcataloga" runat="server" Text="Preview Catalogs" OnClick="btnpreviewcataloga_Click" class="btn btn-info pull-right" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="box">
                    <div class="box-header bdrbtm">
                        <div class="col-sm-10">
                            <h3 class="box-title">Product Order Arrangement</h3>
                        </div>

                        <div class="col-sm-2 cmn_show">
                            <label for="inputSkills" class="control-label">Show:</label>
                            <asp:DropDownList ID="ddlpageSize" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="upd2" runat="server">
                        <ContentTemplate>

                            <div class="col-sm-12">
                                <asp:GridView ID="gvcatelogs" runat="server" AllowSorting="true" OnSorting="gvcatelogs_Sorting" CssClass="table table-bordered table-striped dataTable"
                                    GridLines="None" DataKeyNames="productid" PagerStyle-CssClass="paging-link" role="grid"
                                    AutoGenerateColumns="false" ShowFooter="false"
                                    OnPageIndexChanging="gvcatelogs_PageIndexChanging" OnRowDataBound="gvcatelogs_RowDataBound"
                                    PagerStyle-HorizontalAlign="Right" Width="100%">
                                    <HeaderStyle CssClass="gridheader" />
                                    <RowStyle CssClass="roweven" />
                                    <AlternatingRowStyle CssClass="roweven" />
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for product order arrangement.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="SKU" SortExpression="sku">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("sku").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name" SortExpression="productname">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("productname").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Master/single" SortExpression="materProduct">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("materProduct").ToString() == "False" ? "Single" : "Master")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category" SortExpression="categoryname">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("categoryname").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price" SortExpression="price">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Server.HtmlEncode(Eval("price").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action">
                                            <ItemStyle Width="100" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ButtonRemove" runat="server" CssClass="btn btn-info  btnimpexp" OnClick="ButtonRemove_Click" CommandName="Remove">Remove</asp:LinkButton>
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
                                    <asp:Button ID="imgbtnDelete" runat="server" Visible="false" class="btn btn-default" OnClientClick="javascript:return CheckUserItemSelection();" Text="Delete" />
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddtoArrange" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>



        </div>

    </section>



    <%--<link href="js/jquery.tagsinput.css" rel="stylesheet" />
    <script src="js/jquery.tagsinput.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            KeyWordAutoPopuletSearchProduct();
        });


        $(function () {
            $('#ContentPlaceHolder1_txttags').tagsInput({ width: 'auto', height: 'auto' });
        });

    </script>--%>
</asp:Content>

