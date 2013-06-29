    var previous_masterpoint_page_tab = null;
    function switchMasterpointPageTab(newTab, tabIDPrefix) {
        var selectedClass = 'selected';
        if (previous_masterpoint_page_tab != null)
            jQuery("#"+tabIDPrefix+previous_masterpoint_page_tab).removeClass(selectedClass);
        jQuery("#"+tabIDPrefix+newTab).addClass(selectedClass);
        previous_masterpoint_page_tab = newTab;
        bfi_masterpoint_renderTable_dataTables(newTab);
    } 

function bfi_masterpoint_renderTable(dataGridName,tableType) {
	if (dataGridName == "datatables") {
		bfi_masterpoint_renderTable_dataTables(tableType);
	}
	else if (dataGridName == "flexigrid") {
		bfi_masterpoint_renderTable_flexiGrid();
	}
	else if (dataGridName == "jqgrid") {
		bfi_masterpoint_renderTable_jqGrid();
	}
}

function bfi_masterpoint_renderTable_jqGrid() {
    var html = '<table id="bfi_masterpoints_table"><table>';
    html += ' <div id="bfi_masterpoints_pager"></div>';
    jQuery('#bfi_masterpoints_table_container').html(html);                 
    jQuery("#bfi_masterpoints_table").jqGrid({
        url:'http://localhost/bfi/wp-content/plugins/bfi-masterpoints-display/jquery.jqgrid.php?q=2',
        datatype: "json",
        colNames:['event_date','localpoints_earned', 'fedpoints_earned'],
        colModel:[
        {name:'event_date',index:'event_date'},
        {name:'localpoints_earned',index:'localpoints_earned'},    
        {name:'fedpoints_earned',index:'fedpoints_earned'}       
        ],
        rowNum:10,
        rowList:[10,20,30],
        pager: '#bfi_masterpoints_pager',
        sortname: 'event_date',
        viewrecords: true,
        sortorder: "desc",
        caption:"JSON Example"
    });
    jQuery("#bfi_masterpoints_table").jqGrid('navGrid','#bfi_masterpoints_pager',{edit:false,add:false,del:false});    
}

function bfi_masterpoint_renderTable_dataTables(tableType) {
    var html = '<table id="bfi_masterpoints_table"><thead><tr>';
    if (tableType == "leaderboard") {
        var columns=new Array("ID","NAME","Total","Fed", "Local");
        var sortColumn = "Total";
        var SortOrder = "desc";
    }
    else if (tableType == "mymasterpoint") {
        var columns=new Array("Event Date","Tourney","Tourney Type","Event","Local Points", "Fed Points","Total Points");
        var sortColumn = "Event Date";
        var SortOrder = "desc";
    }
    else {
        alert("Unknown Table Type : "+tableType);
        return;
    }
    var sortParameters = new Array();
    sortParameters[0] = new Array();
    sortParameters[0][0] = columns.indexOf(sortColumn);
    sortParameters[0][1] = SortOrder;
    //var columns=new Array("Event Date","Points");
    jQuery.each(columns,function (index,value) {
        html += '<th>'+value+'</th>';
    });
    html += '</tr></thead></table>';
    jQuery('#bfi_masterpoints_table_container').html(html);
          
    jQuery('#bfi_masterpoints_table').dataTable( {
        "bProcessing": true,
        "bJQueryUI": true,
        "bServerSide": true,
        "sPaginationType": "full_numbers",
        "aaSorting": sortParameters,
        "bAutoWidth": true,
        "fnServerParams": function ( aoData ) {
              aoData.push( { "name": "BFI_Table_Type", "value": tableType } );
        },
        "sAjaxSource": "http://localhost/bfi/wp-content/plugins/bfi-masterpoints-display/jquery.datatables.php"
    } );        
}

function bfi_masterpoint_renderTable_flexiGrid() {
    var html = '<table id="bfi_masterpoints_table"><table>';
    jQuery('#bfi_masterpoints_table_container').html(html);                 

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