//$(function () {
//    var count = 0;
//    $("#sortable1").sortable({
//        connectWith: ".connectedSortable"
//    }).disableSelection();
//    $("#sortable1").sortable({
//        connectWith: ".connectedSortable",
//        receive: function (event, ui) {
//            if (++count > 1)
//                $("#sortable1").sortable("option", { connectWith: "" });
//        },
//        remove: function (event, ui) {
//            if (count > 0 && --count <= 1)
//                $("#sortable1").sortable("option", { connectWith: ".connectedSortable" });
//        }
//    }).disableSelection();
//});

$('#ContentPlaceHolder1_lstDropCatSku').draggable({
    stop: function (event, ui) {
        $(this).css('left', 0);
        $(this).css('top', 0);
    }
});