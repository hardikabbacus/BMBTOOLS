// Equalheight jquery
equalheight = function (container) {

    var currentTallest = 0,
         currentRowStart = 0,
         rowDivs = new Array(),
         $el,
         topPosition = 0;
    $(container).each(function () {

        $el = $(this);
        $($el).height('auto')
        topPostion = $el.position().top;

        if (currentRowStart != topPostion) {
            for (currentDiv = 0 ; currentDiv < rowDivs.length ; currentDiv++) {
                rowDivs[currentDiv].height(currentTallest);
            }
            rowDivs.length = 0; // empty the array
            currentRowStart = topPostion;
            currentTallest = $el.height();
            rowDivs.push($el);
        } else {
            rowDivs.push($el);
            currentTallest = (currentTallest < $el.height()) ? ($el.height()) : (currentTallest);
        }
        for (currentDiv = 0 ; currentDiv < rowDivs.length ; currentDiv++) {
            rowDivs[currentDiv].height(currentTallest);
        }
    });
}

$(window).load(function () {
    equalheight('.equal');
 
});


$(window).resize(function () {
    equalheight('.equal');
 });





  //$(function () {
  //  //Initialize Select2 Elements
  //  $(".select2").select2();

  //  //Datemask dd/mm/yyyy
  //  $("#datemask").inputmask("dd/mm/yyyy", {"placeholder": "dd/mm/yyyy"});
  //  //Datemask2 mm/dd/yyyy
  //  $("#datemask2").inputmask("mm/dd/yyyy", {"placeholder": "mm/dd/yyyy"});
  //  //Money Euro
  //  $("[data-mask]").inputmask();

  //  //Date range picker
  //  $('#reservation').daterangepicker();
  //  //Date range picker with time picker
  //  $('#reservationtime').daterangepicker({timePicker: true, timePickerIncrement: 30, format: 'MM/DD/YYYY h:mm A'});
  //  //Date range as a button
  //  $('#daterange-btn').daterangepicker(
  //      {
  //        ranges: {
  //          'Today': [moment(), moment()],
  //          'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
  //          'Last 7 Days': [moment().subtract(6, 'days'), moment()],
  //          'Last 30 Days': [moment().subtract(29, 'days'), moment()],
  //          'This Month': [moment().startOf('month'), moment().endOf('month')],
  //          'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
  //        },
  //        startDate: moment().subtract(29, 'days'),
  //        endDate: moment()
  //      },
  //      function (start, end) {
  //        $('#daterange-btn span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
  //      }
  //  );

  //  //Date picker
  //  $('#datepicker').datepicker({
  //    autoclose: true
  //  });

  //  //iCheck for checkbox and radio inputs
  //  $('.minimal input[type="checkbox"],.minimal input[type="radio"]').iCheck({
  //    checkboxClass: 'icheckbox_minimal-blue',
  //    radioClass: 'iradio_minimal-blue'
  //  });
  //  //Red color scheme for iCheck
  //  $('.minimal-red input[type="checkbox"],.minimal-red input[type="radio"]').iCheck({
  //    checkboxClass: 'icheckbox_minimal-red',
  //    radioClass: 'iradio_minimal-red'
  //  });
  //  //Flat red color scheme for iCheck
  //  $('.flat-red input[type="checkbox"],.flat-red input[type="radio"]').iCheck({
  //    checkboxClass: 'icheckbox_flat-green',
  //    radioClass: 'iradio_flat-green'
  //  });

  //  //Colorpicker
  //  $(".my-colorpicker1").colorpicker();
  //  //color picker with addon
  //  $(".my-colorpicker2").colorpicker();

  //  //Timepicker
  //  $(".timepicker").timepicker({
  //    showInputs: false
  //  });
  //});
