
/* window 'load' attachment */
function addLoadEvent(func) { var oldonload = window.onload; if (typeof window.onload != 'function') { window.onload = func; } else { window.onload = function () { oldonload(); func; } } }

/* grab Elements from the DOM by className */
function getElementsByClass(searchClass, node, tag) { var classElements = new Array(); if (node == null) node = document; if (tag == null) tag = '*'; var els = node.getElementsByTagName(tag); var elsLen = els.length; var pattern = new RegExp("(^|\\s)" + searchClass + "(\\s|$)"); for (i = 0, j = 0; i < elsLen; i++) { if (pattern.test(els[i].className)) { classElements[j] = els[i]; j++; } } return classElements; }

/* Counts the number of character enters and remaining characters in the text area to be enter*/
function CheckFieldLength(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining").innerHTML = mc - len;

}
function radioUserItemSelection() {
    var count = 0; $(".main-tab tr").find("input:radiobutton").each(function () {
        if (this.checked) { count++; }
    });
    if (count == 0) {
        alert('Please select at least one Customer');
        return false;
    }
    if (ValidateTextbox() == true) {
        if (document.getElementById("ctl00_ContentPlaceHolder1_hfsubtotal").value == "") {
            alert("Please calculate total price by clicking calculate button.");
            return false;
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_txtpoints").value == "") {
            alert("Please enter Points");
            return false;
        }
        else {
            //            if (isNaN("ctl00_ContentPlaceHolder1_txtpoints".value)) {
            //                alert("Please enter numeric value only for Points to be used");
            //                return false;
            //            }
            return isDecimal(document.getElementById("ctl00_ContentPlaceHolder1_txtpoints").value);
        }
    }
    else {
        return false;
    }
}

function CheckFieldLengthProd(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining").innerHTML = mc - len;

}

function CheckFieldLengthProd2(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount2").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining2").innerHTML = mc - len;

}

function CheckFieldLengthProd3(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount3").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining3").innerHTML = mc - len;

}

function CheckFieldLengthProd4(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount4").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining4").innerHTML = mc - len;

}

function CheckFieldLengthProd5(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount5").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining5").innerHTML = mc - len;

}

function CheckFieldLengthProd6(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount6").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining6").innerHTML = mc - len;
}

function CheckFieldLengthProd7(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount7").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining7").innerHTML = mc - len;
}

function CheckFieldLengthProd8(fn, mc) {
    var len = fn.value.length;
    if (len > mc) { fn.value = fn.value.substring(0, mc); len = mc; }

    document.getElementById("ctl00_ContentPlaceHolder1_charcount8").innerHTML = len;
    document.getElementById("ctl00_ContentPlaceHolder1_remaining8").innerHTML = mc - len;
}


function radioUserSelection() {
    var count = 0; $(".main-tab tr").find("input:radiobutton").each(function () {
        if (this.checked) { count++; }
    });
    if (count == 0) {
        alert('Please select at least one Customer');
        return false;
    }
    if (ValidateTextbox() == true) {
        if (document.getElementById("ctl00_ContentPlaceHolder1_hfsubtotal").value == "") {
            alert("Please calculate total price by clicking calculate button.");
            return false;
        }
    }
    else {
        return false;
    }
    if (document.getElementById("ctl00_ContentPlaceHolder1_txtpoints").value != "") {
        //        if (isNaN("ctl00_ContentPlaceHolder1_txtpoints".value)) {
        //            alert("Please enter numeric value only for Points to be used");
        //            return false;
        //        }
        var stat = isDecimal(document.getElementById("ctl00_ContentPlaceHolder1_txtpoints").value);
        if (stat == false)
            return false
    }
    if (document.getElementById("ctl00_ContentPlaceHolder1_txtdiscount").value != "") {
        //        if (isNaN("ctl00_ContentPlaceHolder1_txtdiscount".value)) {
        //            alert("Please enter numeric value only for Rewarded Points");
        //            return false;
        //        }
        var stat = isDecimal(document.getElementById("ctl00_ContentPlaceHolder1_txtdiscount").value);
        if (stat == false)
            return false
    }
    return true;
}
function isDecimal(argvalue) {
    var count = 0
    argvalue = argvalue.toString();
    if (argvalue.length == 0) {
        return true;
    }
    for (var n = 0; n < argvalue.length; n++) {
        if (argvalue.substring(n, n + 1) == "0")
            continue;

        else if (argvalue.substring(n, n + 1) == "1")
            continue;

        else if (argvalue.substring(n, n + 1) == "2")
            continue;

        else if (argvalue.substring(n, n + 1) == "3")
            continue;

        else if (argvalue.substring(n, n + 1) == "4")
            continue;

        else if (argvalue.substring(n, n + 1) == "5")
            continue;

        else if (argvalue.substring(n, n + 1) == "6")
            continue;

        else if (argvalue.substring(n, n + 1) == "7")
            continue;

        else if (argvalue.substring(n, n + 1) == "8")
            continue;

        else if (argvalue.substring(n, n + 1) == "9")
            continue;

            //else if(argvalue.substring(n, n+1) == " ")
            //continue;

        else if (argvalue.substring(n, n + 1) == ".") {
            count += 1;
            if (count < 1) {
                continue;
            }
            else if (count == 1) {
                if (argvalue.length - argvalue.indexOf(".") > 3) {
                    alert("Please enter upto 2 decimal places only ");
                    return false;
                }
            }
            else {
                alert("Please enter valid decimal value ");
                return false;
            }
            continue;
        }
        else {
            alert("Please enter decimal value only");
            return false;
        }

    }
    return true;
}

function checkheader1(name) {
    var test = 0;
    var con = 0;
    for (var i = 0; i < document.aspnetForm.length; i++) {

        if (document.aspnetForm.elements[i].type == "checkbox") {

            var chkname = document.aspnetForm.elements[i].name
            var ch = chkname.split("$");

            //var ch = chkname.split(":");
            if (ch[2] == name) {

                if (document.aspnetForm.elements[i].checked == true) {
                    con = 1;
                    i = document.aspnetForm.length;
                }
            }
        }
    }

    if (con == 1) {
        for (var i = 0; i < document.aspnetForm.length; i++) {

            if (document.aspnetForm.elements[i].type == "checkbox") {
                test = document.aspnetForm.elements[i].name.indexOf("chkGroups")
                if (test > 0) {
                    document.aspnetForm.elements[i].checked = true;
                }
            }
        }
    }
    else {
        for (var i = 0; i < document.aspnetForm.length; i++) {
            if (document.aspnetForm.elements[i].type == "checkbox") {
                test = document.aspnetForm.elements[i].name.indexOf("chkGroups")
                if (test > 0) {
                    document.aspnetForm.elements[i].checked = false;
                }
            }
        }
    }
}

function checkother1(name) {
    var con = 0;
    for (var i = 0; i < document.aspnetForm.length; i++) {
        if (document.aspnetForm.elements[i].type == "checkbox") {
            var chkname = document.aspnetForm.elements[i].name
            var ch = chkname.split("$");
            //var ch = chkname.split(":");
            if (ch[2] != name) {
                if (document.aspnetForm.elements[i].checked == false) {
                    con = 1;
                    i = document.aspnetForm.length;
                }
            }
        }
    }
    if (con == 1) {
        for (var i = 0; i < document.aspnetForm.length; i++) {
            if (document.aspnetForm.elements[i].type == "checkbox") {
                var chkname = document.aspnetForm.elements[i].name
                var ch = chkname.split("$");
                //var ch = chkname.split(":");
                if (ch[2] == name) {
                    document.aspnetForm.elements[i].checked = false;
                    i = document.aspnetForm.length;
                }
            }
        }
    }

}

/* toggle an element's display */
function toggle(obj) { var el = document.getElementById(obj); if (el.style.display != 'none') { el.style.display = 'none'; } else { el.style.display = ''; } }

/* grid selected row color */
$(document).ready(function () { $(".main-tab tr").hover(function () { $(this).css("background-color", "#e5e5e5"); }, function () { $(this).css("background-color", "#fcfcfc"); }); });

// select  all check box in grid /table if header is checked
function SelectAllCheckboxes() { var count = 0; $(".main-tab tr").find("input:checkbox").each(function () { if ($(this).parent().attr('class') == 'headercheckbox') { if (this.checked) { count = 1; } } if (count == 1) { $(this).attr('checked', true); } else { $(this).attr('checked', false); } }) }

function SelectAllCheckboxes_1() {
    var count = 0;
    $(".table tr").find("input:checkbox").each(function () {
        if ($(this).parent().attr('class') == 'headercheckbox') {
            if (this.checked)
            { count = 1; }
        }
        if (count == 1) {
            if ($(this).attr('disabled') == true) {
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', true);
            }
        }
        else {
            $(this).attr('checked', false);
        }
    }
     )
}


// Un check header if one of the child is unchecked in grid /table 
function UnSelectHeaderCheckbox() { $(".table tr").find("input:checkbox").each(function () { if (!this.checked) { $(".headercheckbox > input:checkbox").attr('checked', false); } }) }

// alert / confirmation for check box selection in grid/table
function CheckUserItemSelection() { var count = 0; $(".table tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record to delete'); return false; } else { return confirm('Are you sure you want to delete your selections?'); } }

//alert / confirmation for check box selection in grid/table multipale update
function CheckUserItemSelectionForUpdate() { var count = 0; $(".table tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record to update'); return false; } }

function CheckUserItemSelectionForDelete() { var count = 0; $(".table tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record to cancel job'); return false; } else { return confirm('Are you sure you want to cancel your selections?'); } }

function CheckUserItemSelectionForOrderStatus() { var count = 0; $(".table tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record for change to status'); return false; } else { return confirm('Are you sure you want to change status to your selections?'); } }

// alert / confirmation for check box selection in grid/table
function CheckUserItemSelectionForMail() { var count = 0; $(".data tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record to delete'); return false; } else { return confirm('Are you sure you want to send this mail your selections?'); } }
// alert / confirmation for check box selection in Question grid/table
function CheckUserItemSelectionQuestion() { var count = 0; $(".data tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record to delete'); return false; } else { return confirm('Are you sure you want to delete your selections and related record in comment,answer and report table section?'); } }
// alert / confirmation for check box selection in Question grid/table
function CheckUserItemSelectiongroup() { var count = 0; $(".data tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record to delete'); return false; } else { return confirm('Are you sure you want to delete your selections and related record in group table section?'); } }
// select  all check box in email check box list if header is checked
function SelectAllEmailCheckboxes() { var count = 0; if ($("#ctl00_ContentPlaceHolder1_chkall").is(':checked')) { count = 1; } $(".email-tab tr").find("input:checkbox").each(function () { if (count == 1) { $(this).attr('checked', true); } else { $(this).attr('checked', false); } }) }

// Un check header if one of the child is unchecked in grid /table 
function UnSelectHeaderEmailCheckbox() { var count = 0; $(".email-tab tr").find("input:checkbox").each(function () { if (!this.checked) { $("#ctl00_ContentPlaceHolder1_chkall").attr('checked', false); } }) }

// This functions check for numeric value //if validationtype=decimal (check decimal)
function checkNumeric(objectid, validationtype) { var d = validationtype; var value = $("#" + objectid + "").val(); var orignalValue = value; val = val.replace(/[0-9]*/g, ""); var msg = "Only Integer Values allowed."; if (d == 'decimal') { value = value.replace(/\./, ""); msg = "Only Numeric Values allowed."; } if (val != '') { orignalValue = orignalValue.replace(/([^0-9].*)/g, ""); $(this).val(orignalValue); alert(msg); } }

//Check custom date
function isDateCustom(dateStr) {
    var datePat = /^(\d{1,2})(\/)(\d{1,2})(\/)(\d{4})$/; //var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var matchArray = dateStr.match(datePat); // is the format ok?
    if (matchArray == null) { return 'Please enter date as dd/mm/yyyy. Your current selection reads: ' + dateStr; }
    month = matchArray[3]; day = matchArray[1]; year = matchArray[5]; // p@rse date into variables
    // check month range
    if (month < 1 || month > 12) { return 'Month must be between 1 and 12.'; }
    if (day < 1 || day > 31) { return 'Day must be between 1 and 31.'; }
    if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) { return 'Month ' + month + ' doesn`t have 31 days!'; }
    // check for february 29th
    if (month == 2) { var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)); if (day > 29 || (day == 29 && !isleap)) { return 'February ' + year + ' doesn`t have ' + day + ' days!'; } }
    return ''; // date is valid
}

//Check Date Validaton
function ValidaDate(dateStr, args) {
    var datestr1 = args.Value;
    var datePat = /^(\d{1,2})(\/)(\d{1,2})(\/)(\d{4})$/; //var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var matchArray = (datestr1).match(datePat); // is the format ok?
    if (matchArray == null) { return false }
    month = matchArray[1]; day = matchArray[3]; year = matchArray[5]; // p@rse date into variables
    // check month range
    if (month < 1 || month > 12) { return false }
    if (day < 1 || day > 31) { return false }
    if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) { return false }
    // check for february 29th
    if (month == 2) { var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)); if (day > 29 || (day == 29 && !isleap)) { return false } }
    return true; // date is valid
}

// Compare dates
function CompareDates(Fromdate, Todate) {
    var str1 = Fromdate; var str2 = Todate; var dt1 = parseInt(str1.substring(0, 2), 10); var mon1 = parseInt(str1.substring(3, 5), 10); var yr1 = parseInt(str1.substring(6, 10), 10); var dt2 = parseInt(str2.substring(0, 2), 10); var mon2 = parseInt(str2.substring(3, 5), 10); var yr2 = parseInt(str2.substring(6, 10), 10); var date1 = new Date(yr1, mon1, dt1); var date2 = new Date(yr2, mon2, dt2);
    if (date2 < date1) { return false; } else { return true; }
}


//-----------------------------Array Push and popup----------------------------    

function add_item(array1, item) {
    var dynamic_array = new Array();
    dynamic_array = array1;
    dynamic_array.push(item);
    return dynamic_array;
}

function remove_item() {
    dynamic_array.pop();
    document.frm.outputbox.value = dynamic_array;
}

function removeItems(array1, item) {
    var array = new Array(); var i = 0;
    array = array1;
    while (i < array.length) { if (array[i] == item) { array.splice(i, 1); } else { i++; } }
    array.sort();
    return array;
}

// clock 
function showclock() {
    if (!document.layers && !document.all && !document.getElementById)
        return
    var Digital = new Date()
    var hours = Digital.getHours()
    var minutes = Digital.getMinutes()
    var seconds = Digital.getSeconds()
    var dn = "AM"
    if (hours > 12) { dn = "PM"; hours = hours - 12; }
    if (hours == 0) hours = 12;
    if (minutes <= 9) minutes = "0" + minutes;
    if (seconds <= 9) seconds = "0" + seconds;
    //change font size here to your desire
    myclock = "  " + hours + ":" + minutes + ":" + seconds + " " + dn + ""
    if (document.layers) {
        document.layers.liveclock.document.write(myclock)
        document.layers.liveclock.document.close()
    }
    else if (document.all)
        liveclock.innerHTML = myclock
    else if (document.getElementById)
        document.getElementById("liveclock").innerHTML = myclock
    window.status = "Current Time " + hours + ":" + minutes + ":" + seconds + " " + dn;
    setTimeout("showclock()", 1000)
}


//load editor
function BasicToolbar(id) {
    CKEDITOR.replace(id, { language: 'es', uiColor: '#3399FF', toolbar: 'Basic' });
}
//function BasicToolbar(){}
function CustomToolbar(id) { CKEDITOR.replace(id, { language: 'es', uiColor: '#3399FF' }); }
function DefaultToolbar(id) { CKEDITOR.replace(id, { language: 'es', uiColor: '#3399FF' }); }

function PopulateMenu() {
    $.ajax({
        type: "GET",
        url: "http://localhost/cmsbuilder/admin/menu/admin.txt?" + Math.floor(Math.random() * 200),
        //url: "http://comp18/cmsbuilder/admin/menu/admin.txt?" + Math.floor(Math.random() * 200),
        success: function (msg) {
            $(".container").html(msg);
            setTimeout("showMenu();", 500);
        }
    });
}

//addLoadEvent(showMenu);
//----------------------Load admin menu------------------------------
function showMenu() {
    function megaHoverOver() {
        $(this).find(".sub").stop().fadeTo('fast', 1).show();

        //Calculate width of all ul's
        (function ($) {
            jQuery.fn.calcSubWidth = function () {
                rowWidth = 0;
                //Calculate row
                $(this).find("ul").each(function () {
                    rowWidth += $(this).width();
                });
            };
        })(jQuery);

        if ($(this).find(".row").length > 0) { //If row exists...
            var biggestRow = 0;
            //Calculate each row
            $(this).find(".row").each(function () {
                $(this).calcSubWidth();
                //Find biggest row
                if (rowWidth > biggestRow) {
                    biggestRow = rowWidth;
                }
            });
            //Set width
            $(this).find(".sub").css({ 'width': biggestRow });
            $(this).find(".row:last").css({ 'margin': '0' });

        } else { //If row does not exist...

            $(this).calcSubWidth();
            //Set Width
            $(this).find(".sub").css({ 'width': rowWidth });

        }
    }

    function megaHoverOut() {
        $(this).find(".sub").stop().fadeTo('fast', 0, function () {
            $(this).hide();
        });
    }

    var config = {
        sensitivity: 2, // number = sensitivity threshold (must be 1 or higher)    
        interval: 25, // number = milliseconds for onMouseOver polling interval    
        over: megaHoverOver, // function = onMouseOver callback (REQUIRED)    
        timeout: 125, // number = milliseconds delay before onMouseOut    
        out: megaHoverOut // function = onMouseOut callback (REQUIRED)    
    };

    $("ul#topnav li .sub").css({ 'opacity': '0' });
    $("ul#topnav li").hoverIntent(config);
}
//---------------------- End Loading admin menu------------------------------


//---------------------Image Preview-------------------------------
function imagePreview(imageSource, imagevalue) {
    if (imagevalue != "") {
        var el = $('<div id="popupPreview"><a id="popupPreviewClose"><img src="images/fileclose.png" /></a><h1>Preview</h1><p id="previewArea" style="overflow:scroll;width:100%;height:470px;"><img src="' + imageSource + '" alt=""/></p></div><div id="backgroundPopup"></div>');
        $("body").append(el);

    }
    else { $("#button").css({ 'display': 'none' }); }

}
function openPreview(imagename) {
    window.open("viewImage.aspx?name=" + imagename, "Actual Image", "height=400,width=600,left=150,top=200,scrollbars=yes");
}
function openImagePreview(imagename, flag) {
    window.open("viewactualimage.aspx?name=" + imagename + "&parent=" + flag, "Actual Image", "height=400,width=600,left=150,top=200,scrollbars=yes");
}


/*handle selection of radiobutton */
function fnCheckSel(intObjId) {
    var strSceTypeId;
    strSceTypeId = intObjId + "1";
    for (var i = 1; i < document.aspnetForm.length; i++) {
        if (document.aspnetForm.elements[i].id) {
            if (document.aspnetForm.elements[i].id.indexOf("rdbtndefault") != -1) {
                if (document.aspnetForm.elements[i].id.indexOf("rdbtndefault1") == -1) {
                    document.aspnetForm.elements[i].checked = false;
                }
            }
        }
    }
    document.getElementById(intObjId).checked = true;
}






// Un check header if one of the child is unchecked in grid /table 
function UnSelectHeaderCheckbox2() { $(".main-tab1 tr").find("input:checkbox").each(function () { if (!this.checked) { $(".headercheckbox > input:checkbox").attr('checked', false); } }) }

// alert / confirmation for check box selection in grid/table
function CheckUserItemSelection2() { var count = 0; $(".main-tab1 tr").find("input:checkbox").each(function () { if (this.checked) { count++; } }); if (count == 0) { alert('Please select at least a single record to delete'); return false; } else { return confirm('Are you sure you want to delete your selections?'); } }

function SelectAllCheckboxes_2() {
    var count = 0;
    $(".main-tab1 tr").find("input:checkbox").each(function () {
        if ($(this).parent().attr('class') == 'headercheckbox') {
            if (this.checked)
            { count = 1; }
        }
        if (count == 1) {
            if ($(this).attr('disabled') == true) {
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', true);
            }
        }
        else {
            $(this).attr('checked', false);
        }
    }
 )
}

function getres(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 35 && charCode != 36 && charCode != 38 && charCode != 37 && charCode != 39 && charCode != 40) {
        $("#ContentPlaceHolder1_imgbtnSearch").trigger("click");
        var input = document.getElementById('ctl00_ContentPlaceHolder1_txtsearch');
        input.addEventListener('focus', reset, false);
        evt.preventDefault();
        return false;
    }
    else {
        return false;
    }
}

function getcountry(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 35 && charCode != 36 && charCode != 38 && charCode != 37 && charCode != 39 && charCode != 40) {
        $("#ctl00_ContentPlaceHolder1_txtcountry").trigger("click");
        var input = document.getElementById('ctl00_ContentPlaceHolder1_txtcountry');
        input.addEventListener('focus', reset, false);
        evt.preventDefault();
        return false;
    }
    else {
        return false;
    }
}


function SetCursorToTextEnd(elem) {
    var elemLen = elem.value.length;

    // For IE Only
    if (document.selection) {
        // Set focus
        elem.focus();
        // Use IE Ranges
        var oSel = document.selection.createRange();
        // Reset position to 0 & then set at end
        oSel.moveStart('character', -elemLen);
        oSel.moveStart('character', elemLen);
        oSel.moveEnd('character', 0);
        oSel.select();
    }
    else if (elem.selectionStart || elem.selectionStart == '0') {
        // Firefox/Chrome
        elem.selectionStart = elemLen;
        elem.selectionEnd = elemLen;
        elem.focus();
    } // if
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function clickgo() {
    $("#ctl00_ContentPlaceHolder1_imgbtnSearch").trigger("click");
    return false;
}

function isDate(txtDate) {
    var objDate,  // date object initialized from the txtDate string
        mSeconds, // txtDate in milliseconds
        day,      // day
        month,    // month
        year;     // year
    // date length should be 10 characters (no more no less)
    //var 
    if (txtDate.length !== 10) {
        return false;
        alert("Please select Date in Valid form.");
    }
    // third and sixth character should be '/'
    if (txtDate.substring(2, 3) !== '/' || txtDate.substring(5, 6) !== '/') {
        return false;
    }
    // extract month, day and year from the txtDate (expected format is mm/dd/yyyy)
    // subtraction will cast variables to integer implicitly (needed
    // for !== comparing)
    month = txtDate.substring(3, 5) - 1; // because months in JS start from 0
    day = txtDate.substring(0, 2) - 0;
    year = txtDate.substring(6, 10) - 0;
    // test year range
    if (year < 1000 || year > 3000) {
        return false;
    }
    // convert txtDate to milliseconds
    mSeconds = (new Date(year, month, day)).getTime();
    // initialize Date() object from calculated milliseconds
    objDate = new Date();
    objDate.setTime(mSeconds);
    // compare input date and parts from Date() object
    // if difference exists then date isn't valid
    if (objDate.getFullYear() !== year ||
        objDate.getMonth() !== month ||
        objDate.getDate() !== day) {
        return false;
    }
    // otherwise return true
    return true;
}


