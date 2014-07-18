var tableTools = {
    sSwfPath: '//cdn.jsdelivr.net/jquery.datatables/1.10.0/plugins/tabletools/swf/copy_csv_xls.swf',
    aButtons: ['copy', 'csv', 'xls']
};

$(function() {

    $('#leaderReport').dataTable({
        dom: 'T<"clear">frtip',
        tableTools: tableTools
    });

    $('#navalWars,#landWars,#rivalries,#trade').dataTable({
        paging: false,
        searching: false,
        info: false,
        order: [],
        dom: 'T<"clear">frtip',
        tableTools: tableTools 
    });

    $('#correlation,#scores,#debt').dataTable({
        paging: false,
        searching: false,
        ordering: false,
        info: false,
        order: [],
        dom: 'T<"clear">frtip',
        tableTools: tableTools 
    });
});
