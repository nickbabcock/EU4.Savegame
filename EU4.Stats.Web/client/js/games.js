$(function() {

    var fullOptions = {
        dom: 'T<"clear">frtip',
        tableTools: {
            sSwfPath: '//cdn.jsdelivr.net/jquery.datatables/1.10.0/plugins/tabletools/swf/copy_csv_xls.swf',
            aButtons: ['copy', 'csv', 'xls']
        }
    };

    var sortTable = $.extend({}, fullOptions, {
        paging: false,
        searching: false,
        info: false,
        order: []
    });

    $('table.full-table').dataTable(fullOptions);
    $('table.sort-table').dataTable(sortTable);
    $('table.plain-table').dataTable($.extend({}, sortTable, {order:[]}));
});
