// for payment section



$(document).ready(function () {
    SearchOrderId();
    SearchCustomerName();
});

function SearchOrderId() {

    $("#ContentPlaceHolder1_txtPopOrderno").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../WebService.asmx/searchOrderId",
                data: "{'orderid':'" + document.getElementById('ContentPlaceHolder1_txtPopOrderno').value + "','contactname':'" + document.getElementById('ContentPlaceHolder1_txtPOPCustomer').value + "'}",
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
            $('#ContentPlaceHolder1_txtPopOrderno').val(text);
            $('#ContentPlaceHolder1_txtPopOrderno').trigger('click');
            FindCustomerFromOrderID(text);
        }
    });
}
//Find Sku Record
function FindCustomerFromOrderID(orderid) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/FindCustomerFromOrderID",
        data: "{'orderid':'" + orderid + "'}",
        dataType: "json",
        success: function (data) {
            var v = null;
            var v2 = null;
            var v4 = null;
            var v3 = null;
            $.each(data.d, function (i, v1) {
                v = v1.orderid;
                v3 = v1.contactName;
                v2 = v1.customerid;
                v4 = v1.totalammount;
            });
            $("#ContentPlaceHolder1_txtPOPCustomer").val(v3);
            $("#ContentPlaceHolder1_hidCustid").val(v2);
            $("#ContentPlaceHolder1_txtPopAmmount").val(v4);
            //$("#ContentPlaceHolder1_lblskubind").append(a);
        },
        error: function (result) { }
    });
}

//Search Customer Name
function SearchCustomerName() {
    $("#ContentPlaceHolder1_txtPOPCustomer").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../WebService.asmx/searchCustomerName",
                data: "{'contactName':'" + document.getElementById('ContentPlaceHolder1_txtPOPCustomer').value + "'}",
                dataType: "json",
                success: function (data) {
                    response(data.d);
                },
                error: function (result) { alert("No Match"); }
            });
        },
        select: function (event, ui) {
            var text = ui.item.value;
            $('#ContentPlaceHolder1_txtPOPCustomer').val(text);
            $('#ContentPlaceHolder1_txtPOPCustomer').trigger('click');
            //FindOrderidFromCustomerName(text);
        }
    });
}

//Find Order id from customerName
function FindOrderidFromCustomerName(contactName) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/FindOrderidFromCustomername",
        data: "{'contactName':'" + contactName + "'}",
        dataType: "json",
        success: function (data) {
            var v = null;
            var v3 = null;

            $.each(data.d, function (i, v1) {
                v = v1.orderid;
                v3 = v1.customerid;
            });
            $("#ContentPlaceHolder1_txtPopOrderno").val(v);
            //$("#ContentPlaceHolder1_lblskubind").append(a);
        }, error: function (result) { }
    });
}

function addPayment() {
    var flag = false;
    var customername = $("#ContentPlaceHolder1_txtPOPCustomer").val();
    var orderid = $("#ContentPlaceHolder1_txtPopOrderno").val();
    var nots = $("#ContentPlaceHolder1_txtPopNots").val();
    var ammount = $("#ContentPlaceHolder1_txtPopAmmount").val();

    var strdata;
    if (customername.trim() == "" || customername.trim() == null) {
        document.getElementById("ContentPlaceHolder1_customernamemsg").innerHTML = "Please enter customer name"; return false;
    }
    else {
        document.getElementById("ContentPlaceHolder1_customernamemsg").innerHTML = "";
    }
    if (orderid.trim() == "" || orderid == null) {
        document.getElementById("ContentPlaceHolder1_orderidmsg").innerHTML = "Please enter order no"; return false;
    }
    else {
        document.getElementById("ContentPlaceHolder1_orderidmsg").innerHTML = "";
    }
    if (ammount.trim() == "" || ammount == null) {
        document.getElementById("ContentPlaceHolder1_ammountmsg").innerHTML = "Please enter ammount"; return false;
    }
    else {
        document.getElementById("ContentPlaceHolder1_ammountmsg").innerHTML = "";
    }

}

//Clear Dialog Box
function clearPaymentform() {
    //$('#ContentPlaceHolder1_hdproductid').val(null);
    $("#ContentPlaceHolder1_txtPOPCustomer").val(null);
    $("#ContentPlaceHolder1_txtPopOrderno").val(null);
    $("#ContentPlaceHolder1_txtPopNots").val(null);
    $("#ContentPlaceHolder1_txtPopAmmount").val(null);

    document.getElementById('ContentPlaceHolder1_btnpayupdate').style.display = 'none';
    document.getElementById('ContentPlaceHolder1_btnpay').style.display = '';
}

//Fetch Data on Product Diloag Box
function fetchPaymentData() {
    var id = $('#ContentPlaceHolder1_hidPaymentId').val();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../WebService.asmx/BindPaymentData",
        data: "{'id':'" + id + "'}",
        dataType: "json",
        asycn: false,
        success: function (data) {
            var Operation = data.d;

            var paymentid = Operation[0].paymentid;
            var orderid = Operation[0].orderid;
            var payammount = Operation[0].payammount
            var paynotes = Operation[0].paynotes;
            var contactName = Operation[0].contactName;

            $('#ContentPlaceHolder1_txtPOPCustomer').val(contactName);
            $('#ContentPlaceHolder1_txtPopOrderno').val(orderid);
            $('#ContentPlaceHolder1_txtPopNots').val(paynotes);
            $('#ContentPlaceHolder1_txtPopAmmount').val(payammount);

        }, error: function (result) {
            alert("error in Update");
            return false;
        }
    });
    return false;
}