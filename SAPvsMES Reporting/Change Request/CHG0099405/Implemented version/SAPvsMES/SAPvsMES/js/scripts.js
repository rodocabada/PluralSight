$(document).ready(function () {
    var tbl = $('#MainContent_tblSAPvsMES');
    if (tbl.length !== 0) {
        var table = tbl.DataTable({
            lengthChange: false,
            buttons: [
                {
                    extend: 'excel',
                    text: '<i class="fa fa-file-excel-o"></i> Excel',
                    title: 'Excel example'
                },
                  {
                      extend: 'pdf',
                      //message: 'This is an example from DataTables PDF Option',
                      text: '<i class="fa fa-file-pdf-o"></i> PDF',
                      title: 'PDF example'
                  },
                  {
                      extend: 'print',
                      //message: 'Tesla Packed Serials by List of Boxes',
                      text: '<i class="fa fa-print"></i> Print',
                      title: 'Print example'
                  }
            ],
            dom:
                   "<'row'<'col-sm-6'l><'col-sm-6'f>>" +
                   "<'row'<'col-sm-12'tr>>" +
                   "<'row'<'col-sm-6'i><'col-sm-6'p>>",
            renderer: 'bootstrap',
            oLanguage: {
                sLengthMenu: "_MENU_",
                sInfo: "Showing <strong>_START_</strong>-<strong>_END_</strong> of <strong>_TOTAL_</strong>",
                oPaginate: {
                    sPrevious: '<i class="fa fa-angle-left"></i>',
                    sNext: '<i class="fa fa-angle-right"></i>'
                }
            },
            iDisplayLength: 10,
            //"bPaginate": false
        });
        //load table buttons
        table.buttons().container().appendTo('#' + tbl.attr('id') + '_wrapper .col-sm-6:eq(0)');
    }

    if ($('.flatpickr').length > 0) {
        flatpickr(".flatpickr", {
            altInput: true,
            enableTime: false,
            altFormat: "m/d/Y"
        });
    }
});