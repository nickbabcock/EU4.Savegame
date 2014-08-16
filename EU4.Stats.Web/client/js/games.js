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

    var xtable = $.extend({}, sortTable, { scrollX: true, ordering: false });

    $('table.full-table').dataTable(fullOptions);
    $('table.sort-table').dataTable(sortTable);
    $('table.plain-table').dataTable($.extend({}, sortTable, {ordering: false}));
    new $.fn.dataTable.FixedColumns($('table.x-table').dataTable(xtable));
});
