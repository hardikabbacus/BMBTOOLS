//Check Text Box Value Number and Dash
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 45 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
//check textbox value numeric value with single dot
function validateQty(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode != 45 && charCode != 8 && (charCode != 46) && (charCode < 48 || charCode > 57))
        return false;
    if (charCode == 46) {
        if ((el.value) && (el.value.indexOf('.') >= 0))
            return false;
        else
            return true;
    }
    return true;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = evt.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
};
//textbox value enter number only
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
//validation on phone number
function isPhoneNumber(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 43 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
//bind PopImages
function BindPopImagesProduct(productid) {



    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindPopImages",
        data: "{'productid':'" + productid + "'}",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            $("#showimg").empty();
            var strdata = response.d;
            //var imagesname = strdata[0].imageName;
            //var urlimages = "<a id='ancImage' class='pop3' rel='group1' href='../resources/product/full/" + imagesname + "'>Preview</a>";
            //$('#ContentPlaceHolder1_images').append(urlimages);
            $('#showimg').append(strdata);

        },
        error: function (result) {
            alert("Error");
        }
    });
}
// master product add from product page
function addMasterProduct() {
    //alert('in');
    var flag = false;
    var productName = $("#ContentPlaceHolder1_txtpopProduct").val();
    var sku = $("#ContentPlaceHolder1_txtpopSKU").val();
    var productDescription = $("#ContentPlaceHolder1_txtpopDescriprion").val();
    var ddlPopCategoryProduct = $("#ContentPlaceHolder1_ddlPOPCategoryProduct").val();
    var ddlPopBrandProduct = $("#ContentPlaceHolder1_ddlPOPBrandProduct").val();
    //var imagePopBrandProduct = $("#ContentPlaceHolder1_uploadProductMasterImage").val();
    var ArabicName = $("#ContentPlaceHolder1_txtArabicName").val();
    var ArabicDiscription = $("#ContentPlaceHolder1_txtArabicDiscription").val();

    var strdata;
    if (sku == "" || sku == null) {
        //alert("Please Enter Sku");
        document.getElementById("skumsg").innerHTML = "Please enter sku";
        // return false;
    }
    else {
        //if (!isNaN(sku)) {
        //    document.getElementById("skumsg").innerHTML = "";
        //} else {
        //    document.getElementById("skumsg").innerHTML = "Please enter sku values in number formate";
        //}
        document.getElementById("skumsg").innerHTML = "";
    }
    if (productName.trim() == "" || productName == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productmastermsg").innerHTML = "Please enter master product name";
        // return false;
    }
    else {
        document.getElementById("productmastermsg").innerHTML = "";
    }

    if (ddlPopCategoryProduct == "" || ddlPopCategoryProduct == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productcategorymsg").innerHTML = "Please select product category";
        // return false;
    }
    else {
        document.getElementById("productcategorymsg").innerHTML = "";
    }
    if (ddlPopBrandProduct == "" || ddlPopBrandProduct == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productbrandmsg").innerHTML = "Please select product brand";
        // return false;
    }
    else {
        document.getElementById("productbrandmsg").innerHTML = "";
    }

    if (sku != "" && productName != "" && sku != "" && ddlPopCategoryProduct != "" && ddlPopBrandProduct != "") {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/addMasterProduct",
            //data: "{'name':'" + document.getElementById('txtname').value + "','country':'" + document.getElementById('txtcname').value + "','city':'" + document.getElementById('txtctname').value + "'}",
            data: "{'productName':'" + productName + "','sku':'" + sku + "','productDescription':'" + productDescription + "','ddlPopCategoryProduct':'" + ddlPopCategoryProduct + "','ddlPopBrandProduct':'" + ddlPopBrandProduct + "','ArabicName':'" + ArabicName + "','ArabicDiscription':'" + ArabicDiscription + "'}",
            dataType: "json",
            async: false,
            //global: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                strdata = response.d;
                //alert(strdata);
                flag = true;
                if (strdata == "Master product inserted successfully") {
                    //alert(strdata);
                    BindRecordFromSku(sku);
                    BindRecorFromMasterProductName(productName);
                    flag = uploadedImages();
                    //setTimeout("uploadedImages()", 2000);


                }
                else {
                    alert(strdata);
                    flag = false;
                }
                //window.location = "dashboard.aspx";

            },
            error: function (result) {
                //alert("Error");
            }
        });
        return flag;
    }
    else {
        return false;
    }
}

// master product add from product page
function addMasterProductPopUp() {
    //alert('in');
    var flag = false;
    var productName = $("#ContentPlaceHolder1_txtpopProduct").val();
    var sku = $("#ContentPlaceHolder1_txtpopSKU").val();
    var productDescription = $("#ContentPlaceHolder1_txtpopDescriprion").val();
    var ddlPopCategoryProduct = $("#ContentPlaceHolder1_ddlPOPCategoryProduct").val();
    var ddlPopBrandProduct = $("#ContentPlaceHolder1_ddlPOPBrandProduct").val();
    //var imagePopBrandProduct = $("#ContentPlaceHolder1_uploadProductMasterImage").val();
    var ArabicName = $("#ContentPlaceHolder1_txtArabicName").val();
    var ArabicDiscription = $("#ContentPlaceHolder1_txtArabicDiscription").val();

    var strdata;
    if (sku == "" || sku == null) {
        //alert("Please Enter Sku");
        document.getElementById("skumsg").innerHTML = "Please enter sku";
        // return false;
    }
    else {

        document.getElementById("skumsg").innerHTML = "";
    }
    if (productName.trim() == "" || productName == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productmastermsg").innerHTML = "Please enter master product name";
        // return false;
    }
    else {
        document.getElementById("productmastermsg").innerHTML = "";
    }

    if (ddlPopCategoryProduct == "" || ddlPopCategoryProduct == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productcategorymsg").innerHTML = "Please select product category";
        // return false;
    }
    else {
        document.getElementById("productcategorymsg").innerHTML = "";
    }
    if (ddlPopBrandProduct == "" || ddlPopBrandProduct == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productbrandmsg").innerHTML = "Please select product brand";
        // return false;
    }
    else {
        document.getElementById("productbrandmsg").innerHTML = "";
    }

    if (sku != "" && productName != "" && sku != "" && ddlPopCategoryProduct != "" && ddlPopBrandProduct != "") {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/addMasterProduct",
            data: "{'productName':'" + productName + "','sku':'" + sku + "','productDescription':'" + productDescription + "','ddlPopCategoryProduct':'" + ddlPopCategoryProduct + "','ddlPopBrandProduct':'" + ddlPopBrandProduct + "','ArabicName':'" + ArabicName + "','ArabicDiscription':'" + ArabicDiscription + "'}",
            dataType: "json",
            async: false,
            //global: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                strdata = response.d;
                //alert(strdata);
                flag = true;
                if (strdata == "Master product inserted successfully") {
                    //alert(strdata);
                    BindRecordFromSku(sku);
                    BindRecorFromMasterProductName(productName);
                    //flag = uploadedImages();
                    if (uploadedImages()) {
                        window.location.href = 'viewmasterproduct.aspx?flag=add';
                    }
                    window.location.href = 'viewmasterproduct.aspx?flag=add';
                    //setTimeout("uploadedImages()", 2000);


                }
                else {
                    alert(strdata);
                    flag = false;
                }
                //window.location = "dashboard.aspx";

            },
            error: function (result) {
                //alert("Error");
            }
        });
        return flag;
    }
    else {
        return false;
    }
}

function uploadedImages() {
    var uploadfiles = $("#filedrgdrop").get(0);
    var uploadedfiles = uploadfiles.files;
    var fromdata = new FormData();
    var id;
    for (var x = 0; x < uploadfiles.files.length; x++) {
        fromdata.append(uploadedfiles[x].name, uploadedfiles[x]);
        // alert(uploadedfiles[x].name);
    }

    var choice = {};
    choice.url = "ImagesHandler.ashx";
    choice.type = "POST";
    choice.data = fromdata;
    choice.contentType = false;
    choice.processData = false;
    choice.success = function (result) { };
    choice.error = function (err) { };
    $.ajax(choice);
    //event.preventDefault();
    return true;
}

// category add from product page
function addCategoryProduct() {
    //  alert('in');categorynamemsg
    var parentid = $("#ContentPlaceHolder1_ddlPOPProductParentCategory").val();
    var categoryName = $("#ContentPlaceHolder1_txtcateName").val();
    var categoryDescription = $("#ContentPlaceHolder1_txtdescription").val();
    var isactive = $("#ContentPlaceHolder1_ddlisactive").val();
    var strdata;
    var flag = false;
    //parentid = 0;

    if (categoryName == "" || categoryName == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("categorynamemsg").innerHTML = "Please enter category name";
        // return false;
    }
    else {
        document.getElementById("categorynamemsg").innerHTML = "";
    }
    if (categoryName != "") {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/addCategoryProduct",
            //data: "{'name':'" + document.getElementById('txtname').value + "','country':'" + document.getElementById('txtcname').value + "','city':'" + document.getElementById('txtctname').value + "'}",
            data: "{'parentid':'" + parentid + "','categoryName':'" + categoryName + "','categoryDescription':'" + categoryDescription + "','isactive':'" + isactive + "'}",
            dataType: "json",
            async: false,
            //global: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                strdata = response.d;
                if (strdata == "Category added successfully") {
                    BindPopCategory();
                    BindPopBrand();
                    alert("category added successfully");
                    $('#ModelCategory').hide();
                    clearCategoryform();
                    flag = false;
                    //flag = true;
                }
                else {
                    alert(strdata);
                    flag = false;
                }
                //window.location = "dashboard.aspx";

            },
            error: function (result) {
                //alert("Error");
            }
        });
        return flag;

    }
    else {
        return false;
    }
}


function singleproductdisply() {
    if (document.getElementById('ContentPlaceHolder1_rbtsingleprod').checked) {

        document.getElementById('idAddMasterPro').style.display = 'none';
        document.getElementById('idSku_PRoduct').style.display = 'none';
        document.getElementById('idSku_PRoduct1').style.display = 'none';
        document.getElementById('categoryproduct').style.display = '';
        document.getElementById('divOnlySingle').style.display = '';
        document.getElementById('categoryname').style.display = '';
        document.getElementById('brandname').style.display = '';
        document.getElementById('ContentPlaceHolder1_lblskubind').style.display = 'none';
    }
    else if (document.getElementById('ContentPlaceHolder1_rbtvariant').checked) {

        // document.getElementById('ifYes').style.visibility = 'hidden';
        document.getElementById('idAddMasterPro').style.display = '';
        document.getElementById('idSku_PRoduct').style.display = '';
        document.getElementById('idSku_PRoduct1').style.display = '';
        document.getElementById('categoryproduct').style.display = 'none';
        document.getElementById('divOnlySingle').style.display = 'none';
        document.getElementById('categoryname').style.display = 'none';
        document.getElementById('brandname').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_lblskubind').style.display = '';
    }
}



function GetProductBrandValue() {

    ShowProgress();

    var leftCategoryIdProduct = $("#ContentPlaceHolder1_ddlcategoryleftside").val();
    var leftBrandIdProduct = $("#ContentPlaceHolder1_ddlBrandleftside").val();

    var LeftTagsProduct = $("#ContentPlaceHolder1_txttags").val();

    if (leftCategoryIdProduct != "" || leftCategoryIdProduct != null) {
        $("#ContentPlaceHolder1_hidLeftCategoryIdProduct").val(leftCategoryIdProduct);
    }
    if (leftBrandIdProduct != "" || leftBrandIdProduct != null) {
        $("#ContentPlaceHolder1_hidLeftBrandIdProduct").val(leftBrandIdProduct);
    }
    if (LeftTagsProduct != "" || LeftTagsProduct != null) {
        $("#ContentPlaceHolder1_hidLeftTagsProduct").val(LeftTagsProduct);
    }

}


// GET ID OF last row and increment it by one
$(document).ready(function () {
    //BindSKU();
    //BindProductName();
    BindUserRole();
    SearchTextSKU();
    SearchMasterProductName();
    BindCustomerName();

    //******* code for state bind with countryid…………..

    //$("#ContentPlaceHolder1_ddlmastersku").change(function () {

    //    var productId = $(this).val();
    //    $.ajax({
    //        type: "POST",
    //        contentType: "application/json; charset=utf-8",
    //        url: "../WebService.asmx/MasterproductBind",
    //        data: "{'productId':'" + productId + "'}",
    //        dataType: "json",
    //        success: function (data) {
    //            var v = null;
    //            var a = null;
    //            var b = null;
    //            //var c = $("#ContentPlaceHolder1_mastercount").val();
    //            //c++;
    //            //alert("c"+c);
    //            var counter = $("#ContentPlaceHolder1_mastercount").val();
    //            counter++;
    //            // alert(counter);
    //            $.each(data.d, function (i, v1) {

    //                //  v = "<option value='" + v1.productId + "' selected>" + v1.productName + "</option>";
    //                //a = "<input type='hidden' name='lblskuid' id=lbl" + v1.productId + "  value=" + v1.productId + "><label>" + v1.sku + "</label><br/>";
    //                //b = "<label>" + v1.productName + "</label><br/>";
    //                v = v1.productId;
    //                var flag = true;
    //                var countr = 0;
    //                $(".ddlpropducts").each(function () {
    //                    countr++;
    //                    var dupvalue = $(this).find('input[type=hidden]:first').val();
    //                    if (dupvalue == v || countr == 1)
    //                    { flag = false; }
    //                });

    //                if (flag) {
    //                    a = "<div class='form-group ddlpropducts' id='div" + counter + "'><label for='inputName' class='col-sm-2 control-label'>SKU</label><div class='col-sm-4'><input type='hidden' name=lbl" + counter + " id=lbl" + counter + "  value=" + v1.productId + "><label>" + v1.sku + "</label></div><label for='inputName' class='col-sm-2 control-label'></label><div class='col-sm-4 close_btn'><label>" + v1.productName + "</label><input type='button' value='x' class='del_ExpenseRow add_btn' id=div" + counter + " onclick='RemoveRow(this.id);' /></div></div>";
    //                    //alert(a);
    //                    $("#ContentPlaceHolder1_mastercount").val(counter);
    //                }
    //                else { alert('you can not select multiple master products'); }
    //            });


    //            // alert(v);
    //            $("#ContentPlaceHolder1_ddlproductname").val(v);
    //            // $("#ContentPlaceHolder1_lblskuname").append(v);
    //            $("#ContentPlaceHolder1_lblskubind").append(a);
    //            //  $("#ContentPlaceHolder1_lblskuname").append(b);

    //        },
    //        error: function (result) {
    //            //alert("Error");
    //        }

    //    });
    //});

    // $("#ContentPlaceHolder1_ddlproductname").change(function () {


    // var sku = $(this).val();
    // // alert(productId);
    // $.ajax({
    // type: "POST",
    // contentType: "application/json; charset=utf-8",
    // url: "../WebService.asmx/MasterproductBind",
    // data: "{'sku':'" + sku + "'}",
    // dataType: "json",
    // success: function (data) {
    // var v = null;
    // var v3 = null;
    // var a = null;
    // var b = null;

    // var counter = $("#ContentPlaceHolder1_mastercount").val();
    // counter++;
    // // alert(counter);
    // $.each(data.d, function (i, v1) {

    // // v = "<option value='" + v1.productId + "' selected>" + v1.sku + "</option>";
    // v = v1.sku;
    // v3 = v1.productId;
    // var flag = true;
    // var countr = 0;
    // $(".ddlpropducts").each(function () {
    // countr++;
    // var dupvalue = $(this).find('input[type=hidden]:first').val();
    // if (dupvalue == v3 || countr == 1)
    // { flag = false; }
    // });

    // if (flag) {
    // a = "<div class='form-group ddlpropducts' id='div" + counter + "'><label for='inputName' class='col-sm-2 control-label'>SKU</label><div class='col-sm-4'><input type='hidden' name=lbl" + counter + " id=lbl" + counter + "  value=" + v1.productId + "><label>" + v1.sku + "</label></div><label for='inputName' class='col-sm-2 control-label'></label><div class='col-sm-4 close_btn'><label>" + v1.productName + "</label><input type='button' value='x' class='del_ExpenseRow add_btn' id=div" + counter + " onclick='RemoveRow(this.id);' /></div></div>";
    // $("#ContentPlaceHolder1_mastercount").val(counter);
    // }
    // else { alert('you can not select multiple master products'); }


    // });


    // //$('#select2-ContentPlaceHolder1_ddlmastersku-container').text(v);
    // //$('#select2-ContentPlaceHolder1_ddlmastersku-container').attr("title", v);

    // //$("#ContentPlaceHolder1_ddlmastersku").val(v3);
    // //$("#ContentPlaceHolder1_lblskubind").append(a);

    // $("#ContentPlaceHolder1_txtskumaster").val(v);
    // $("#ContentPlaceHolder1_lblskubind").append(a);


    // },
    // error: function (result) {
    // //alert("Error");
    // }

    // });
    // });

    function SearchTextSKU() {
        $("#ContentPlaceHolder1_txtskumaster").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../WebService.asmx/MasterproductBindSKU",
                    //data: "{'empName':'" + document.getElementById('txtEmpName').value + "'}",
                    data: "{'sku':'" + document.getElementById('ContentPlaceHolder1_txtskumaster').value + "'}",
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
                //alert(text);
                FindSkurecord(text);

            }
        });
    }

    function FindSkurecord(sku) {
        // alert('Sku val :' + sku);
        // var productId = $(this).val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/MasterproductBind",
            data: "{'sku':'" + sku + "'}",
            dataType: "json",
            success: function (data) {
                var v = null;
                var a = null;
                var b = null;
                //alert('hhh');
                var counter = $("#ContentPlaceHolder1_mastercount").val();
                counter++;
                $.each(data.d, function (i, v1) {
                    v = v1.sku;
                    v3 = v1.productName;
                    var flag = true;
                    var countr = 0;
                    $(".ddlpropducts").each(function () {
                        countr++;
                        // alert('Counter :' + countr);
                        var dupvalue = $(this).find('input[type=hidden]:first').val();
                        if (dupvalue == v || countr == 1)
                        { flag = false; }
                    });

                    if (flag) {
                        a = "<div class='form-group ddlpropducts' id='div" + counter + "'><label for='inputName' class='col-sm-2 control-label'>SKU</label><div class='col-sm-4'><input type='hidden' name=lbl" + counter + " id=lbl" + counter + "  value=" + v1.productId + "><label>" + v1.sku + "</label></div><label for='inputName' class='col-sm-2 control-label'></label><div class='col-sm-4 close_btn'><label>" + v1.productName + "</label><input type='button' value='x' class='del_ExpenseRow add_btn' id=div" + counter + " onclick='RemoveRow(this.id);' /></div></div>";
                        $("#ContentPlaceHolder1_mastercount").val(counter);
                    }
                    else { alert('you can not select multiple master products'); }
                });
                //$("#ContentPlaceHolder1_ddlproductname").val(v);
                // $('#select2-ContentPlaceHolder1_ddlproductname-container').text(v3);
                // $('#select2-ContentPlaceHolder1_ddlproductname-container').attr("title", v);
                // $("#ContentPlaceHolder1_ddlproductname").val(v);

                $("#ContentPlaceHolder1_txtmasterproductname").val(v3);
                $("#ContentPlaceHolder1_lblskubind").append(a);
            },
            error: function (result) { }
        });
    }

    function SearchMasterProductName() {
        $("#ContentPlaceHolder1_txtmasterproductname").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../WebService.asmx/MasterproductName",
                    data: "{'productname':'" + document.getElementById('ContentPlaceHolder1_txtmasterproductname').value + "'}",
                    dataType: "json",
                    success: function (data) {
                        response(data.d);
                    },
                    error: function (result) { alert("No Match"); }
                });
            },
            select: function (event, ui) {
                var text = ui.item.value;
                $('#ContentPlaceHolder1_txtmasterproductname').val(text);
                $('#ContentPlaceHolder1_txtmasterproductname').trigger('click');
                FindMasterProductName(text);
            }
        });
    }

    function FindMasterProductName(productname) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/MasterproductNameBind",
            data: "{'productname':'" + productname + "'}",
            dataType: "json",
            success: function (data) {
                var v = null;
                var v3 = null;
                var a = null;
                var b = null;
                var counter = $("#ContentPlaceHolder1_mastercount").val();
                counter++;
                $.each(data.d, function (i, v1) {
                    v = v1.sku;
                    v3 = v1.productId;
                    var flag = true;
                    var countr = 0;
                    $(".ddlpropducts").each(function () {
                        countr++;
                        var dupvalue = $(this).find('input[type=hidden]:first').val();
                        if (dupvalue == v3 || countr == 1)
                        { flag = false; }
                    });
                    if (flag) {
                        a = "<div class='form-group ddlpropducts' id='div" + counter + "'><label for='inputName' class='col-sm-2 control-label'>SKU</label><div class='col-sm-4'><input type='hidden' name=lbl" + counter + " id=lbl" + counter + "  value=" + v1.productId + "><label>" + v1.sku + "</label></div><label for='inputName' class='col-sm-2 control-label'></label><div class='col-sm-4 close_btn'><label>" + v1.productName + "</label><input type='button' value='x' class='del_ExpenseRow add_btn' id=div" + counter + " onclick='RemoveRow(this.id);' /></div></div>";
                        $("#ContentPlaceHolder1_mastercount").val(counter);
                    }
                    else { alert('you can not select multiple master products'); }
                });
                $("#ContentPlaceHolder1_txtskumaster").val(v);
                $("#ContentPlaceHolder1_lblskubind").append(a);
            },
            error: function (result) { }
        });
    }

    function BindSKU() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/SKUBind",
            dataType: "json",
            success: function (data) {
                var v = '';
                v += "<option value='0'>Enter SKU Values</option>";
                $.each(data.d, function (i, v1) {

                    v += "<option value='" + v1.productId + "'>" + v1.sku + "</option>";

                });

                $("#ContentPlaceHolder1_ddlmastersku").append(v);
            }
            ,
            error: function (result) {
                //alert("Error");
            }
        });
    }

    function BindProductName() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/SKUProductNameBind",
            dataType: "json",
            success: function (data) {
                var v = '';
                v += "<option value='0'>None - this is searchable field</option>";
                $.each(data.d, function (i, v1) {

                    v += "<option value='" + v1.sku + "'>" + v1.productName + "</option>";

                });

                $("#ContentPlaceHolder1_ddlproductname").append(v);
            }
            ,
            error: function (result) {
                //alert("Error");
            }
        });
    }

});

// bind left product category in product page
function BindLeftCategoryProduct(pro_id) {
    //alert('int');
    var productid = pro_id;
    //alert(productid);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindLeftCategoryProduct",
        data: "{'productid':'" + productid + "'}",
        dataType: "json",
        async: false,
        //global: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            strdata = response.d;
            //alert(strdata);
            var newarray = strdata.split(',');
            for (var i = 0; i < newarray.length; i++) {
                $('#ContentPlaceHolder1_ddlcategoryleftside').val(newarray);
                // alert("array :" + newarray[i]);
            }
        },
        error: function (result) {
            //alert("Error");
        }
    });
}

// bind left brand for product
function BindLeftBrandProduct(pro_id) {
    //alert('int');
    var productid = pro_id;
    //alert(productid);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindLeftBrandProduct",
        data: "{'productid':'" + productid + "'}",
        dataType: "json",
        async: false,
        //global: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            strdata = response.d;
            //alert(strdata);
            var newarray = strdata.split(',');
            for (var i = 0; i < newarray.length; i++) {
                $('#ContentPlaceHolder1_ddlBrandleftside').val(newarray);
            }
        },
        error: function (result) {
            //alert("Error");
        }
    });
}

// bind left Tags for product
function BindLeftTagsProduct(pro_id) {
    //alert('int');
    var productid = pro_id;
    //alert(productid);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindLeftTagsProduct",
        data: "{'productid':'" + productid + "'}",
        dataType: "json",
        async: false,
        //global: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            strdata = response.d;
            //alert(strdata);

            //window.location = "dashboard.aspx";
            $('#ContentPlaceHolder1_txttags').val(strdata);
        },
        error: function (result) {
            //alert("Error");
        }
    });
}

//  bind master product for product
function BindMasterProductForProduct(pro_id) {

    var productId = pro_id;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindMasterProduct_Product",
        data: "{'productId':'" + productId + "'}",
        dataType: "json",
        success: function (data) {
            var a = '';
            var counter = $("#ContentPlaceHolder1_mastercount").val();
            //alert(data.d);
            $.each(data.d, function (i, v1) {
                a += "<div class='form-group ddlpropducts' id='div" + counter + "'><label for='inputName' class='col-sm-2 control-label'>SKU</label><div class='col-sm-4'><input type='hidden' name=lbl" + counter + " id=lbl" + counter + "  value=" + v1.productId + "><label>" + v1.sku + "</label></div><label for='inputName' class='col-sm-2 control-label'></label><div class='col-sm-4 close_btn'><label>" + v1.productName + "</label><input type='button' value='x' class='del_ExpenseRow add_btn' id=div" + counter + " onclick='RemoveRow(this.id);' /></div></div>";
                //alert(a);
                //alert(counter);
                counter++;
                $("#ContentPlaceHolder1_mastercount").val(counter);
            });
            $("#ContentPlaceHolder1_lblskubind").append(a);
        },
        error: function (result) {
            //alert("Error");
        }
    });
}


function RemoveRow(r) {
    // alert(r);
    $("#" + r).remove();
    var counter = parseInt($("#ContentPlaceHolder1_mastercount").val());
    counter--;
    $('#ContentPlaceHolder1_mastercount').val(counter);
    changeRowsId(r);
    $('#ContentPlaceHolder1_txtmasterproductname').val('');
    $('#ContentPlaceHolder1_txtskumaster').val('');
    //return false;
}


function changeRowsId(removerowxounter) {
    //var counter = $("#ContentPlaceHolder1_mastercount").val();
    //  var ii = removerowxounter.substring(3);
    //del_ExpenseRow
    var countr = 0;
    $(".ddlpropducts").each(function () {
        countr++;
        $(this).attr("id", 'div' + countr).attr("name", 'ddlpropducts' + countr);
        $(this).find('input[type=hidden]:first').attr("id", "lbl" + countr);
    });
    var countr2 = 0;
    $(".del_ExpenseRow").each(function () {
        countr2++;
        $(this).attr("id", 'div' + countr2).attr("name", 'del_ExpenseRow' + countr2);
    });
    $("#ContentPlaceHolder1_mastercount").val(countr);
}

// bind user role
function BindUserRole() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindUserType",
        dataType: "json",
        success: function (data) {
            var v = '';
            $.each(data.d, function (i, v1) {
                v += "<option value='" + v1.adminTypeId + "'>" + v1.typeName + "</option>";
            });

            $("#ContentPlaceHolder1_listusertype").append(v);
        }
        ,
        error: function (result) {
            //alert("Error");
        }
    });
}

// add user type
function addUserType() {
    //alert('in');
    var typeName = $("#ContentPlaceHolder1_txtusertype").val();
    var isactive = $("#ContentPlaceHolder1_ddlUserActive").val();

    var strdata;

    if (typeName == "" || typeName == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("usermessage").innerHTML = "Please enter type name";
        // return false;
    }
    else {
        document.getElementById("usermessage").innerHTML = "";
    }

    if (typeName != "" && isactive != "") {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/addUserType",
            data: "{'typeName':'" + typeName + "','isactive':'" + isactive + "'}",
            dataType: "json",
            async: false,
            //global: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                strdata = response.d;
                if (strdata == "success") {
                    BindUserRole();
                    alert(strdata);
                }

                //window.location = "dashboard.aspx";

            },
            error: function (result) {
                //alert("Error");
            }
        });
        return true;
    }
    else {
        return false;
    }
}

function clearform() {
    $('#ContentPlaceHolder1_hdproductid').val(null);
    $("#ContentPlaceHolder1_txtpopProduct").val(null);
    $("#ContentPlaceHolder1_txtpopSKU").val(null);
    $("#ContentPlaceHolder1_txtpopDescriprion").val(null);
    $("#ContentPlaceHolder1_ddlPOPCategoryProduct").select2('val', 0);

    $("#ContentPlaceHolder1_ddlPOPBrandProduct").select2('val', 0);
    $("#ContentPlaceHolder1_uploadProductMasterImage").val(null);
    $("#ContentPlaceHolder1_images").empty();

    $("#ContentPlaceHolder1_images").val(null);
    $("#showimg").empty();

    document.getElementById('btnPopMasterProupdate').style.display = 'none';
    document.getElementById('btnPopMasterPro').style.display = '';
}



function fetchData() {

    // var id = $(this).attr("id");
    var id = $('#ContentPlaceHolder1_hdproductid').val();
    //  alert(id);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindMasterProductdata",
        data: "{'id':'" + id + "'}",
        dataType: "json",
        asycn: false,
        success: function (data) {
            var Operation = data.d;
            var sku = Operation[0].sku;
            var productname = Operation[0].productName;
            var productdes = Operation[0].productDescription;
            var ArabicName = Operation[0].ArabicName;
            var ArabicDescription = Operation[0].ArabicDescription;

            BindPopCategoryProduct(id);
            BindPopBrandProduct(id);


            $('#ContentPlaceHolder1_txtpopSKU').val(sku);
            $('#ContentPlaceHolder1_txtpopProduct').val(productname);
            $('#ContentPlaceHolder1_txtpopDescriprion').val(productdes);
            $('#ContentPlaceHolder1_txtArabicName').val(ArabicName);
            $('#ContentPlaceHolder1_txtArabicDiscription').val(ArabicDescription);
            BindPopImagesProduct(id);

        },
        error: function (result) {
            alert("error in Update");
            //alert("Error");
            return false;
        }
    });
    return false;
}


function updateMasterProduct() {
    //alert('in');
    var id = $('#ContentPlaceHolder1_hdproductid').val();
    var productName = $("#ContentPlaceHolder1_txtpopProduct").val();
    var sku = $("#ContentPlaceHolder1_txtpopSKU").val();
    var productDescription = $("#ContentPlaceHolder1_txtpopDescriprion").val();
    var ddlPopCategoryProduct = $("#ContentPlaceHolder1_ddlPOPCategoryProduct").val();
    var ddlPopBrandProduct = $("#ContentPlaceHolder1_ddlPOPBrandProduct").val();
    //var imagePopBrandProduct = $("#ContentPlaceHolder1_uploadProductMasterImage").val();
    var ArabicName = $("#ContentPlaceHolder1_txtArabicName").val();
    var ArabicDiscription = $("#ContentPlaceHolder1_txtArabicDiscription").val();

    var flag = false;

    var strdata;
    if (sku == "" || sku == null) {
        //alert("Please Enter Sku");
        document.getElementById("skumsg").innerHTML = "Please enter sku update";
        // return false;
    }
    else {

        document.getElementById("skumsg").innerHTML = "";
    }
    if (productName.trim() == "" || productName == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productmastermsg").innerHTML = "Please enter master product name";
        // return false;
    }
    else {
        document.getElementById("productmastermsg").innerHTML = "";
    }

    if (ddlPopCategoryProduct == "" || ddlPopCategoryProduct == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productcategorymsg").innerHTML = "Please select product category";
        // return false;
    }
    else {
        document.getElementById("productcategorymsg").innerHTML = "";
    }
    if (ddlPopBrandProduct == "" || ddlPopBrandProduct == null) {
        //alert("Please Enter Producdesction");
        document.getElementById("productbrandmsg").innerHTML = "Please select product brand";
        // return false;
    }
    else {
        document.getElementById("productbrandmsg").innerHTML = "";
    }
    // var fileUpload = $("#ContentPlaceHolder1_uploadProductMasterImage").get(0);
    // var files = fileUpload.files;
    // var docname = $('#ContentPlaceHolder1_uploadProductMasterImage').val();

    if (sku != "" && productName != "" && sku != "" && ddlPopCategoryProduct != "" && ddlPopBrandProduct != "") {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/updateMasterProduct",
            data: "{'productid':'" + id + "','productName':'" + productName + "','sku':'" + sku + "','productDescription':'" + productDescription + "','ddlPopCategoryProduct':'" + ddlPopCategoryProduct + "','ddlPopBrandProduct':'" + ddlPopBrandProduct + "','ArabicName':'" + ArabicName + "','ArabicDiscription':'" + ArabicDiscription + "'}",
            dataType: "json",
            async: false,
            //global: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                strdata = response.d;
                //alert(strdata);
                flag = true;
                if (strdata == "Master product updated successfully") {
                    if (updateImages(id)) {
                        //alert("You will now be redirected.");
                        window.location.href = 'viewmasterproduct.aspx?flag=edit';
                    }
                    //setTimeout("updateImages()", 10000);
                    //alert(strdata);
                    window.location.href = 'viewmasterproduct.aspx?flag=edit';
                }
                else {
                    alert(strdata);
                    flag = false;
                }

            },
            error: function (result) {
                alert("Error");
            }
        });
        return flag;
    }
    else {
        return false;
    }
}

function updateImages(id) {
    var uploadfiles = $("#filedrgdrop").get(0);
    var uploadedfiles = uploadfiles.files;
    var fromdata = new FormData();
    var id;
    for (var x = 0; x < uploadfiles.files.length; x++) {
        fromdata.append(uploadedfiles[x].name, uploadedfiles[x]);
        // alert(uploadedfiles[x].name);
    }

    var choice = {};

    choice.url = "ImagesHandler.ashx?id=" + id + "";
    choice.type = "POST";
    choice.data = fromdata;
    choice.contentType = false;
    choice.processData = false;
    choice.success = function (result) { };
    choice.error = function (err) { };
    $.ajax(choice);
    //event.preventDefault();
    return true;
}

function BindPopCategoryProduct(pro_id) {
    //alert('int');
    var productid = pro_id;
    //alert(productid);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindLeftCategoryProduct",
        data: "{'productid':'" + productid + "'}",
        dataType: "json",
        async: false,
        //global: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            // strdata = response.d;
            // // alert(strdata);
            // //var obj = $('#ContentPlaceHolder1_ddlPOPCategoryProduct');
            // var newarray = strdata.split(',');
            // for (var i = 0; i < newarray.length; i++) {
            // //$('#ContentPlaceHolder1_ddlPOPCategoryProduct').val(newarray);
            // //$('#ContentPlaceHolder1_ddlPOPCategoryProduct').val(newarray[i]).attr("selected", "selected");
            // //alert(newarray[i]);
            // // alert("array :" + newarray[i]);
            // }
            // //alert(newarray);
            // $('#ContentPlaceHolder1_ddlPOPCategoryProduct').val(newarray);

            // saddam code
            strdata = response.d;
            // var newarray=[];
            var newarray = strdata.split(',');
            // alert("Category "+newarray);
            // alert("Leangth "+newarray.length);
            //  $('#select2-selection select2-selection--multiple').text(newarray);
            //$('.select2-selection__choice').attr("title", newarray);
            if (newarray.length == 1) {
                $("#ContentPlaceHolder1_ddlPOPCategoryProduct").select2().select2('val', newarray);
            }
            else {
                for (var i = 0; i < newarray.length; i++) {

                    // alert("Category Val" + newarray[i]);

                    // var newval = newarray[i];
                    //   var selectedItems =[];
                    //  selectedItems.push("your selected items");

                    // $('#drp_Books_Ill_Illustrations').select2('val',selectedItems );
                    $("#ContentPlaceHolder1_ddlPOPCategoryProduct").select2().val(newarray);
                }
            }

        },
        error: function (result) {
            alert("Error");
        }
    });
}

// bind Pop brand for product
function BindPopBrandProduct(pro_id) {
    //alert('int');
    var productid = pro_id;
    //alert(productid);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindLeftBrandProduct",
        data: "{'productid':'" + productid + "'}",
        dataType: "json",
        async: false,
        //global: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            // strdata = response.d;
            // //alert(strdata);
            // var newarray = strdata.split(',');
            // //for (var i = 0; i < newarray.length; i++) {
            // //    $('#ContentPlaceHolder1_ddlPOPBrandProduct').val(newarray[i]);
            // //}
            // //alert(newarray);
            // $('#ContentPlaceHolder1_ddlPOPBrandProduct').val(newarray);

            // saddam code
            strdata = response.d;
            var newarray = strdata.split(',');
            //  alert(newarray);
            if (newarray.length == 1) {
                $('#ContentPlaceHolder1_ddlPOPBrandProduct').select2().select2('val', newarray);
            }
            else {
                for (var i = 0; i < newarray.length; i++) {

                    $('#ContentPlaceHolder1_ddlPOPBrandProduct').select2().val(newarray);
                }
            }

        },
        error: function (result) {
            alert("Error");
        }
    });
}


$(document).ready(function () {
    BindPopCategory();
    BindPopBrand();

    $('#ContentPlaceHolder1_btnProductsubmit').click(function () {
        var flag = true;
        var engproductname = $("#txtbrandname1").val();
        var skuname = $("#txtsku").val();
        var varient = $("#ContentPlaceHolder1_rbtvariant").is(':checked') ? 1 : 0;
        var CategoryProductname = $("#ContentPlaceHolder1_ddlcategoryleftside").val();
        var BrandProductname = $("#ContentPlaceHolder1_ddlBrandleftside").val();
        var prodid = $("#ContentPlaceHolder1_hidprodID").val();

        if (skuname != "") {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../WebService.asmx/SkuIsExist",
                data: "{'prodid':'" + prodid + "','skuname':'" + skuname + "'}",
                dataType: "json",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    var strdata = response.d;
                    //alert(strdata);
                    if (strdata == "Yes") {

                        alert("SKU already exist");
                        flag = false;
                    }
                    else {
                        flag = true;
                    }
                },
                error: function (result) {
                    alert("Error");
                }
            });
        }

        if (engproductname == "") { document.getElementById("productnamemsg").innerHTML = "Please enter product name"; flag = false; }
        else { document.getElementById("productnamemsg").innerHTML = ""; }
        if (skuname == "") { document.getElementById("skumsg").innerHTML = "Please enter sku"; flag = false; }
        else { document.getElementById("skumsg").innerHTML = ""; }
        if (document.getElementById('ContentPlaceHolder1_rbtsingleprod').checked) {
            if (CategoryProductname == "" || CategoryProductname == null) { document.getElementById("reqddlcategory").innerHTML = "Please select category"; flag = false; }
            else { document.getElementById("reqddlcategory").innerHTML = ""; }
            if (BrandProductname == "" || BrandProductname == null) { document.getElementById("reqddlbrand").innerHTML = "Please select brand"; flag = false; }
            else { document.getElementById("reqddlbrand").innerHTML = ""; }
        }
        else if (document.getElementById('ContentPlaceHolder1_rbtvariant').checked) {
            document.getElementById("reqddlcategory").innerHTML = "";
            document.getElementById("reqddlbrand").innerHTML = "";
        }
        //alert(varient);
        if (varient == 1) {
            var skucheck = $('.skudiv span').text();
            //alert(skucheck);
            if (skucheck == '') {
                document.getElementById("reqvarient").innerHTML = "Please select masterproduct"; flag = false;
            }
            else {
                document.getElementById("reqvarient").innerHTML = "";
            }
        }
        return flag;

    });

});
function BindPopCategory() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindPopCategory",
        dataType: "json",
        success: function (data) {
            var v = '';
            //v += "<option value='0'>None - this is searchable field</option>";
            $.each(data.d, function (i, v1) {

                v += "<option value='" + v1.categoryId + "'>" + v1.categoryName + "</option>";

            });

            $("#ContentPlaceHolder1_ddlPOPCategoryProduct").append(v);
            $("#ContentPlaceHolder1_ddlcategoryleftside").append(v);
        }
        ,
        error: function (result) {
            //alert("Error");
        }
    });
}

function BindPopBrand() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindPopBrand",
        dataType: "json",
        success: function (data) {
            var v = '';
            //v += "<option value='0'>None - this is searchable field</option>";
            $.each(data.d, function (i, v1) {
                v += "<option value='" + v1.idbrand + "'>" + v1.brandName + "</option>";
            });

            $("#ContentPlaceHolder1_ddlPOPBrandProduct").append(v);
            $("#ContentPlaceHolder1_ddlBrandleftside").append(v);
        }
        ,
        error: function (result) {
            //alert("Error");
        }
    });
}


function validationbrand() {
    var brandname = $("#txtbrandname1").val();
    var txtsortorder = $("#ContentPlaceHolder1_txtsortorder").val();
    //var imgname = $("#ContentPlaceHolder1_txtImageName").val();
    //var alreadyexist = $("#ContentPlaceHolder1_spimg").text();

    var flag = true;
    if (brandname == "") {
        document.getElementById("brandmessage").innerHTML = "Please enter brand name";
        flag = false;
    }
    else {
        document.getElementById("brandmessage").innerHTML = "";
    }

    if (txtsortorder == "") {
        document.getElementById("reqtxtsortorder").innerHTML = "Please enter sort order";
        flag = false;
    }
    else {
        if (isNaN(txtsortorder)) {
            document.getElementById("reqtxtsortorder").innerHTML = "Please enter numeric value for sort order";
            flag = false;
        }
        else {
            document.getElementById("reqtxtsortorder").innerHTML = "";
        }
    }

    // if (imgname == "" && alreadyexist == "") {
    // document.getElementById("regimagenamemsg").innerHTML = "Please browse image";
    // flag = false;
    // }
    // else {
    // if (alreadyexist == "") {
    // var arr1 = new Array;
    // arr1 = imgname.split("\\");
    // var len = arr1.length;
    // var img1 = arr1[len - 1];
    // var filext = img1.substring(img1.lastIndexOf(".") + 1);
    // // Checking Extension
    // if (filext == "bmp" || filext == "gif" || filext == "png" || filext == "jpg" || filext == "jpeg") {
    // document.getElementById("regimagenamemsg").innerHTML = "";
    // }
    // else {
    // document.getElementById("regimagenamemsg").innerHTML = " ! Invalid image type only (.jpeg,.gif,.png) image types are allowed.";
    // flag = false;
    // }
    // }
    // else {
    // document.getElementById("regimagenamemsg").innerHTML = "";
    // }
    // }
    return flag;
}

// get address longitude and letitude
function GetLongitudeLatitude(address) {
    address = address.replace(/,/g, "coma1")
    //alert(address);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/FindLongitudeLetitude",
        data: "{'Address':'" + address + "'}",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var strdata = response.d;
            //alert(strdata);
            $('#ContentPlaceHolder1_hidaddresslonglet').val(strdata);
        },
        error: function (result) {
            alert("Error");
        }
    });
}

//Bind record from Sku Record
function BindRecordFromSku(sku) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/MasterproductBind",
        data: "{'sku':'" + sku + "'}",
        dataType: "json",
        success: function (data) {
            var v = null;
            var a = null;
            var b = null;
            var counter = $("#ContentPlaceHolder1_mastercount").val();
            counter++;
            $.each(data.d, function (i, v1) {
                v = v1.sku;
                v3 = v1.productName;
                var flag = true;
                var countr = 0;
                $(".ddlpropducts").each(function () {
                    countr++;
                    var dupvalue = $(this).find('input[type=hidden]:first').val();
                    if (dupvalue == v || countr == 1)
                    { flag = false; }
                });

                if (flag) {
                    a = "<div class='form-group ddlpropducts' id='div" + counter + "'><label for='inputName' class='col-sm-2 control-label'>SKU</label><div class='col-sm-4'><input type='hidden' name=lbl" + counter + " id=lbl" + counter + "  value=" + v1.productId + "><label>" + v1.sku + "</label></div><label for='inputName' class='col-sm-2 control-label'></label><div class='col-sm-4 close_btn'><label>" + v1.productName + "</label><input type='button' value='x' class='del_ExpenseRow add_btn' id=div" + counter + " onclick='RemoveRow(this.id);' /></div></div>";
                    $("#ContentPlaceHolder1_mastercount").val(counter);
                }
                else { alert('you can not select multiple master products'); }
            });
            $("#ContentPlaceHolder1_txtmasterproductname").val(v3);
            $("#ContentPlaceHolder1_lblskubind").append(a);
        },
        error: function (result) { }
    });
}

//Bind record from Master Product Name
function BindRecorFromMasterProductName(productname) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/MasterproductNameBind",
        data: "{'productname':'" + productname + "'}",
        dataType: "json",
        success: function (data) {
            var v = null;
            var v3 = null;
            var a = null;
            var b = null;
            var counter = $("#ContentPlaceHolder1_mastercount").val();
            counter++;
            $.each(data.d, function (i, v1) {
                v = v1.sku;
                v3 = v1.productId;
                var flag = true;
                var countr = 0;
                $(".ddlpropducts").each(function () {
                    countr++;
                    var dupvalue = $(this).find('input[type=hidden]:first').val();
                    if (dupvalue == v3 || countr == 1)
                    { flag = false; }
                });
            });
            $("#ContentPlaceHolder1_txtskumaster").val(v);

        }, error: function (result) { }
    });
}

//delete product image
function DeleteProductImage(productimageid) {
    var result = confirm("Are you sure you want to delete image ?");
    if (result) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/DeleteProductImage",
            data: "{'productimageid':'" + productimageid + "'}",
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var strdata = response.d;
                //alert(strdata);
                if (strdata == "success") {
                    $("#" + productimageid).remove();
                }
            },
            error: function (result) {
                alert("Error");
            }
        });
    }
}

////Search Text contact person
function BindCustomerName() {
    $("#ContentPlaceHolder1_txtCustomer").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../WebService.asmx/BindCustomerName",
                data: "{'companyName':'" + document.getElementById('ContentPlaceHolder1_txtCustomer').value + "'}",
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
            $('#ContentPlaceHolder1_txtCustomer').val(text);
            $('#ContentPlaceHolder1_txtCustomer').trigger('click');
            $('#DivGrid').show();
            $('#DivTotal').show();
        }
    });
}

// credits 
function calcCOD() {
    if ($('#ContentPlaceHolder1_rbtCOD').is(':checked') == true) {
        $("#ContentPlaceHolder1_lblDelivertType").text("Cash on Delivery");
        //$("#ContentPlaceHolder1_lblCreditLimits").text("Cash on Delivery");
    }
}


function calcCREDIT() {
    if ($('#ContentPlaceHolder1_rbtCredit').is(':checked') == true) {
        $("#ContentPlaceHolder1_lblDelivertType").text("Credit");
        BindCreditLimits();
    }
}

function BindCreditLimits() {
    var companyName = $('#ContentPlaceHolder1_txtCustomer').val();
    var TotalPayAmmount = $('#ContentPlaceHolder1_lblTotalPayAmmount').text();

    if (companyName == "" || companyName == null) {
        alert("Please select company name");
        $('#ContentPlaceHolder1_rbtCOD').prop('checked', true);
        $("#ContentPlaceHolder1_lblDelivertType").text("Cash on Delivery");
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/GetCustomerCreditFromName",
            data: "{'companyName':'" + companyName + "'}",
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var strdata = response.d;
                if (TotalPayAmmount != "" || TotalPayAmmount != null) {
                    if (parseInt(TotalPayAmmount.replace('SAR', '')) >= parseInt(strdata.replace('.00', ''))) {
                        alert(TotalPayAmmount.replace('SAR', '')); alert(strdata.replace('.00', ''));
                        alert("your credit is lower than total payment.");
                        $('#ContentPlaceHolder1_rbtCOD').prop('checked', true);
                        $("#ContentPlaceHolder1_lblDelivertType").text("Cash on Delivery");
                    }
                    else { $("#ContentPlaceHolder1_lblCreditLimits").text("Your credit is " + strdata); }
                }
                else {
                    $("#ContentPlaceHolder1_lblCreditLimits").text("Your credit is " + strdata);
                }
            },
            error: function (result) {
                alert("Error");
            }
        });
    }
}
// credits 
function calcCOD() {
    if ($('#ContentPlaceHolder1_rbtCOD').is(':checked') == true) {
        $("#ContentPlaceHolder1_lblDelivertType").text("Cash on Delivery");
    }
}


function calcCREDIT() {
    if ($('#ContentPlaceHolder1_rbtCredit').is(':checked') == true) {
        $("#ContentPlaceHolder1_lblDelivertType").text("Credit");
        BindCreditLimits();
    }
}

function BindCreditLimits() {
    var companyName = $('#ContentPlaceHolder1_txtCustomer').val();
    var TotalPayAmmount = $('#ContentPlaceHolder1_lblTotalPayAmmount').text();

    if (companyName == "" || companyName == null) {
        alert("Please select company name");
        $('#ContentPlaceHolder1_rbtCOD').prop('checked', true);
        $("#ContentPlaceHolder1_lblDelivertType").text("Cash on Delivery");
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/GetCustomerCreditFromName",
            data: "{'companyName':'" + companyName + "'}",
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var strdata = response.d;
                if (TotalPayAmmount != "" || TotalPayAmmount != null) {
                    if (parseInt(TotalPayAmmount.replace('SAR', '')) >= parseInt(strdata.replace('.00', ''))) {
                        //alert(TotalPayAmmount.replace('SAR', '')); alert(strdata.replace('.00', ''));
                        //alert("Your credit limits is lower than total payment.");
                        //$('#ContentPlaceHolder1_rbtCOD').prop('checked', true);
                        //$("#ContentPlaceHolder1_lblDelivertType").text("Cash on Delivery");

                        fetchCreditPopUpData();

                    }
                    else { $("#ContentPlaceHolder1_lblCreditLimits").text("Your credit is " + strdata); }
                }
                else {
                    $("#ContentPlaceHolder1_lblCreditLimits").text("Your credit is " + strdata);
                }
            },
            error: function (result) {
                alert("Error");
            }
        });
    }
}

//Fetch Data on credit limits popup Box
function fetchCreditPopUpData() {
    var companyName = $('#ContentPlaceHolder1_txtCustomer').val();
    //alert(companyName);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/GetCustomerCreditFromName",
        data: "{'companyName':'" + companyName + "'}",
        dataType: "json",
        asycn: false,
        success: function (data) {
            var Credits = data.d;
            //alert(Credits);
            $('#ContentPlaceHolder1_txtCredits').val(Credits);
            $("#ContentPlaceHolder1_lblerrormessgae").text("Your credit limits is lower than total payment. Do you want to update your credit limits?");
            $('#ModelCategory').modal();

        }, error: function (result) {
            alert("error in Update");
            return false;
        }
    });
    return false;
}

function UpdateCredit() {
    var customerid = $('#ContentPlaceHolder1_hidCustomerId').val();
    var Credits = $("#ContentPlaceHolder1_txtCredits").val();
    var YesNo = $('#ContentPlaceHolder1_ddlCredits').val();

    var flag = false;

    if (YesNo == "0") {
        $('#ContentPlaceHolder1_rbtCOD').prop('checked', true);
        $("#ContentPlaceHolder1_lblDelivertType").text("Cash on Delivery");

        $(".modal").css("display", "none");
        $(".modal-backdrop").css("display", "none");
        $("div").removeClass("in");
        flag = true;
        return flag;
    }

    var strdata;
    if (Credits == "" || Credits == null) {
        document.getElementById("msgCredit").innerHTML = "Please enter credit limits.";
    }
    else {
        document.getElementById("msgCredit").innerHTML = "";
    }

    if (Credits != "") {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/UpdateCustomerCreditsLimits",
            data: "{'customerid':'" + customerid + "','Credits':'" + Credits + "'}",
            dataType: "json",
            async: false,
            //global: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                strdata = response.d;
                flag = true;
                if (strdata == "Credit limits update successfully.") {
                    //return flag;
                    $("#ContentPlaceHolder1_lblerrMsg").text("Credit limits update successfully.");
                    $(".modal").css("display", "none");
                    $(".modal-backdrop").css("display", "none");
                    $("div").removeClass("in");
                    flag = true;
                    return flag;
                }
                else {
                    //alert(strdata);
                    flag = false;
                }
            }, error: function (result) { alert("Error"); }
        });
        return flag;
    }
    else {
        return false;
    }
    return flag;
}

//19_01_2017

function GetBrandValues() {

    var BrandId = $("#ContentPlaceHolder1_ddlcatalogsBrand").val();

    if (BrandId != "" || BrandId != null) {
        $("#ContentPlaceHolder1_hidbrandid").val(BrandId);
    }
}

function ShowProgress() {
    var modal = $('<div />');
    modal.addClass("Mymodal");
    $('body').append(modal);
    var loading = $(".loading");
    loading.show();
    var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
    var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
    loading.css({ top: top, left: left });
}



function onTxtSkuChange() {
    var skuname = $("#txtsku").val();
    var prodid = $("#ContentPlaceHolder1_hidprodID").val();

    if (skuname != "") {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../WebService.asmx/SkuIsExist",
            data: "{'prodid':'" + prodid + "','skuname':'" + skuname + "'}",
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var strdata = response.d;
                //alert(strdata);
                if (strdata == "Yes") {
                    if (confirm("SKU is alrady exist. Do you want to continue?")) {
                        //alert('yes');
                        GetProdutctidCount();
                    }
                    else {
                        //alert('no');
                        flag = false;
                    }
                    flag = false;
                }
                else {
                    flag = true;
                }
            },
            error: function (result) {
                alert("Error");
            }
        });
    }
}

function GetProdutctidCount() {
    var skuname = $("#txtsku").val();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/GetProdutctidCount",
        data: "{'skuname':'" + skuname + "'}",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var strdata = response.d;
            window.location.href = 'add_product.aspx?flag=edit&id=' + strdata;
        },
        error: function (result) {
            alert("Error");
        }
    });
}


//Update product image set main 
function UpdateProductImageSetMain(productimageid, ids) {
    //alert(ids);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/UpdateProductImageSetMain",
        data: "{'productimageid':'" + productimageid + "'}",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var strdata = response.d;
            //alert(strdata);
            if (strdata != "fail") {
                $("#" + ids).text("Main");

                for (i = 0; i < parseInt(strdata) ; i++) {
                    var ID = "img_" + i;
                    if (ids != ID) {
                        $("#img_" + i).text("Set");
                    }
                }
            }
        },
        error: function (result) {
            alert("Error");
        }
    });
}


