<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="Admin_home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="content-header">

        <h1>Dashbord
            <small></small>
        </h1>
      <%--  <ol class="breadcrumb"></ol>--%>

        <div class="saleswrap"><span class="lifetime">Lifetime Sales</span><asp:Label ID="lblLifeTimeSale" class="control-label" runat="server"></asp:Label></div>
    </section>
    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="col-md-4 col-lg-3  col-lg-offset-3 col-md-offset-2">
                    <div class="box">
                        <div class="box-header topcol">
                            <div class="col-md-12">
                                <h3 class="box-title">Today's total sales</h3>
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lblTodaysSales" class="control-label" runat="server"></asp:Label>
                                <span class="clsTodayOrd">
                                    <img src="../images/icon1.jpg" /></span>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 col-lg-3 ">
                    <div class="box">
                        <div class="box-header topcol">
                            <div class="col-md-12">
                                <h3 class="box-title">Today's order count</h3>
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lbltodaysOrder" class="control-label" runat="server"></asp:Label>
                                <span class="clsTodayOrd">
                                    <img src="../images/icon2.jpg" /></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="col-md-6">
                    <div class="box">
                        <div class="box-header">
                            <div class="col-md-12">
                                <h3 class="box-title">New Customers</h3>
                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="gvNewCustomer" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped dataTable">
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for new customer.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <%#Eval("companyName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Primary Contact">
                                            <ItemTemplate>
                                                <%#Eval("contactName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile Number">
                                            <ItemTemplate>
                                                <%#Eval("mobile") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="box">
                        <div class="box-header">
                            <div class="col-md-12">
                                <h3 class="box-title">Recent Order</h3>
                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="gvRecentOrder" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped dataTable">
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for recent order.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Order Number">
                                            <ItemTemplate>
                                                <%#Eval("orderid") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <%#Eval("companyName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Amount">
                                            <ItemTemplate>
                                                <%#Eval("totalammount") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="col-md-6">
                    <div class="box">
                        <div class="box-header">
                            <div class="col-md-12">
                                <h3 class="box-title">Sales Chart</h3>
                            </div>
                            <div class="col-md-12">
                                <div class="box-body">
                                    <div class="chart">
                                        <canvas id="lineChart" style="height: 250px"></canvas>
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
                                <h3 class="box-title">Product Performance</h3>
                            </div>
                            <div class="col-md-12">
                                <asp:GridView runat="server" ID="gvProductPerformance" AutoGenerateColumns="false" CssClass="table table-bordered table-striped dataTable">
                                    <EmptyDataRowStyle CssClass="repeat Required" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <div class="message error">
                                            <h6>No record found for Product.</h6>
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <%# Server.HtmlDecode(Eval("productName").ToString()) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <%#Eval("price") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity Ordered">
                                            <ItemTemplate>
                                                <%#Eval("productOrderCount") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </section>

    <script src="plugins/chartjs/Chart.min.js"></script>

    <script>
        $(function () {


            var Salesdata;
            var months;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../WebService.asmx/GetMounthlyRecord",
                data: "{}",
                dataType: "json",
                success: function (data) {
                    var Years = null;
                    var Months = null;
                    var Sales = null;

                    //for (var i = 0; i < data; i++) {
                    //    $('#ContentPlaceHolder1_ddlcategoryleftside').val(newarray);
                    //}
                    months = '';
                    Salesdata = '';
                    $.each(data.d, function (i, v1) {
                        Years = v1.Years;
                        Months = v1.Months;
                        Sales = v1.Sales;
                        //alert("Months=" + Months);
                        if (Months == 1) { months += 'Jan,'; } if (Months == 2) { months += 'Feb,'; } if (Months == 3) { months += 'Mar,'; }
                        if (Months == 4) { months += 'Apr,'; } if (Months == 5) { months += 'May,'; } if (Months == 6) { months += 'Jun,'; }
                        if (Months == 7) { months += 'Jul,'; } if (Months == 8) { months += 'Aug,'; } if (Months == 9) { months += 'Sep,'; }
                        if (Months == 10) { months += 'Oct,'; } if (Months == 11) { months += 'Nov,'; } if (Months == 12) { months += 'Dec,'; }
                        Salesdata += Sales + ","
                        //testdata += '[]';   
                    });
                    // alert(months.replace(/,\s*$/, ""));
                    //alert(Salesdata.replace(/,\s*$/, ""));
                    var r = months.replace(/,\s*$/, "");
                    var strMonth = r.split(",");

                    var u = Salesdata.replace(/,\s*$/, "");
                    var strSales = u.split(",");

                    var lineChartCanvas = $("#lineChart").get(0).getContext("2d");
                    var lineChart = new Chart(lineChartCanvas);

                    var areaChartData = {
                        //labels: ['Jan', 'Feb', 'Mar'],
                        labels: strMonth,
                        datasets: [
                          {
                              label: "Digital Goods",
                              fillColor: "rgba(rgb(255,255,255))",
                              strokeColor: "rgba(60,141,188,0.8)",
                              pointColor: "#3b8bba",
                              pointStrokeColor: "rgba(60,141,188,1)",
                              pointHighlightFill: "#fff",
                              pointHighlightStroke: "rgba(60,141,188,1)",
                              data: strSales
                          }
                        ]
                    };

                    var areaChartOptions = {
                        //Boolean - If we should show the scale at all
                        showScale: true,
                        //Boolean - Whether grid lines are shown across the chart
                        scaleShowGridLines: false,
                        //String - Colour of the grid lines
                        scaleGridLineColor: "rgba(0,0,0,.05)",
                        //Number - Width of the grid lines
                        scaleGridLineWidth: 1,
                        //Boolean - Whether to show horizontal lines (except X axis)
                        scaleShowHorizontalLines: true,
                        //Boolean - Whether to show vertical lines (except Y axis)
                        scaleShowVerticalLines: true,
                        //Boolean - Whether the line is curved between points
                        bezierCurve: true,
                        //Number - Tension of the bezier curve between points
                        bezierCurveTension: 0.3,
                        //Boolean - Whether to show a dot for each point
                        pointDot: false,
                        //Number - Radius of each point dot in pixels
                        pointDotRadius: 4,
                        //Number - Pixel width of point dot stroke
                        pointDotStrokeWidth: 1,
                        //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
                        pointHitDetectionRadius: 20,
                        //Boolean - Whether to show a stroke for datasets
                        datasetStroke: true,
                        //Number - Pixel width of dataset stroke
                        datasetStrokeWidth: 2,
                        //Boolean - Whether to fill the dataset with a color
                        datasetFill: true,
                        //String - A legend template

                        //Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
                        maintainAspectRatio: true,
                        //Boolean - whether to make the chart responsive to window resizing
                        responsive: true
                    };

                    //Create the line chart
                    lineChart.Line(areaChartData, areaChartOptions);

                    //-------------
                    //- LINE CHART -
                    //--------------

                    var lineChartOptions = areaChartOptions;
                    lineChartOptions.datasetFill = false;
                    lineChart.Line(areaChartData, lineChartOptions);

                }, error: function (result) { }
            });

        });
    </script>

</asp:Content>

