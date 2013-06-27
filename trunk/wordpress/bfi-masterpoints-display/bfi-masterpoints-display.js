function bfi_masterpoint_renderTable(dataGridName) {
	if (dataGridName == "dataTables") {
		bfi_masterpoint_renderTable_dataTables();
	}
	else if (dataGridName == "flexiGrid") {
		bfi_masterpoint_renderTable_flexiGrid();
	}
	else if (dataGridName == "jqGrid") {
		bfi_masterpoint_renderTable_jqGrid();
	}
}

function bfi_masterpoint_renderTable_jqGrid() {
}

function bfi_masterpoint_renderTable_dataTables() {
    jQuery('#bfi_masterpoints_table').dataTable( {
        "bProcessing": true,
        "bJQueryUI": true,
        "bServerSide": true,
        "sAjaxSource": "http://localhost/bfi/wp-content/plugins/bfi-masterpoints-display/jquery.datatables.php"
    } );        
}

function bfi_masterpoint_renderTable_flexiGrid() {
    jQuery("#bfi_masterpoints_table").flexigrid({
        url: 'http://localhost/bfi/wp-content/plugins/bfi-masterpoints-display/jquery.flexigrid.php',
        dataType: 'json',
        colModel : [
        {display: 'Tourney Code', name : 'tournament_code', width:'auto',  sortable : true, align: 'left'},
        {display: 'Event_Code', name : 'event_code',  width:'auto', sortable : true, align: 'left'},
        {display: 'Member', name : 'member_id',   width:'auto', sortable : true, align: 'left'},
        {display: 'Event Date', name : 'event_date',  width:'auto', sortable : true, align: 'left'},
        {display: 'Local', name : 'localpoints_earned',  width:'auto', sortable : true, align: 'left'},
        {display: 'Fed', name : 'fedpoints_earned', width:'auto', sortable : true, align: 'left'}
        ],
        params : [{name:'member_id', value: 'WB007531'}],
        searchitems : [
        {display: 'Tourney Code', name : 'tourney_code'},
        {display: 'Event Code', name : 'event_code'},
        {display: 'Member ID', name : 'member_id', isdefault: true}
        ],
        sortname: "event_date",
        sortorder: "desc",
        usepager: true,
        title: "Masterpoints",
        useRp: true,
        rp: 20,
        showTableToggleBtn: false,
        resizable: true,
        height: 500,
        onSuccess: function() {format();},
        singleSelect: true
    });    
}

function format() {
    var gridContainer = jQuery('.flexigrid');
    var headers = gridContainer.find('div.hDiv table tr:first th:not(:hidden)');
    var drags = gridContainer.find('div.cDrag div');
    var offset = 0;
    var firstDataRow = gridContainer.find('tr:first td:not(:hidden)');
    var columnWidths = new Array( firstDataRow.length );
    gridContainer.find( 'tr' ).each( function() {
        gridContainer.find('td:not(:hidden)').each( function(i) {
            var colWidth = jQuery(this).outerWidth();
            if (!columnWidths[i] || columnWidths[i] < colWidth) {
                columnWidths[i] = colWidth;
            }
        });
    });
    for (var i = 0; i < columnWidths.length; ++i) {
        var bodyWidth = columnWidths[i];

        var header = headers.eq(i);
        var headerWidth = header.outerWidth();

        var realWidth = bodyWidth > headerWidth ? bodyWidth : headerWidth;
        firstDataRow.eq(i).css('width',realWidth);
        header.css('width',realWidth);            
        drags.eq(i).css('left',  offset + realWidth );
        offset += realWidth;
    }
}