$(function() {
    $('table.paging').dataTable();
    $('table').not('table.paging').dataTable({
        paging: false,
        searching: false,
        info: false,
        order: [],
        dom: 'T<"clear">lfrtip',
        tableTools: { 
            sSwfPath: "//cdn.jsdelivr.net/jquery.datatables/1.10.0/plugins/tabletools/swf/copy_csv_xls_pdf.swf"
        }
    });
});
