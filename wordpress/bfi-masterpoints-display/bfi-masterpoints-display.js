var previous_masterpoint_page_tab = null;
var filterFromDate="";
var filterToDate="";
var server_side_url="";
var member_id="";
function setFilter() {
	bfi_masterpoint_renderTable_dataTables(previous_masterpoint_page_tab);
}
function switchMasterpointPageTab(newTab, tabIDPrefix) {
	var selectedClass = 'selected';
	if (previous_masterpoint_page_tab != null)
		jQuery("#" + tabIDPrefix + previous_masterpoint_page_tab).removeClass(selectedClass);
	jQuery("#" + tabIDPrefix + newTab).addClass(selectedClass);
	previous_masterpoint_page_tab = newTab;
	bfi_masterpoint_renderTable_dataTables(newTab, member_id);
}

function bfi_masterpoint_renderTable_dataTables(tableType) {
	var html = '<table id="bfi_masterpoints_table"><thead><tr>';
	if (tableType == "leaderboard") {
		var columns = new Array("ID", "NAME", "Total", "Fed", "Local","Zone","State");
		var sortColumn = "Total";
		var SortOrder = "desc";
	} else if (tableType == "mymasterpoint") {
		var columns = new Array("Event Date", "Tourney", "Tourney Type", "Event", "Local Points", "Fed Points", "Total Points");
		var sortColumn = "Event Date";
		var SortOrder = "desc";
	} else if (tableType == "tournamentlist") {
		var columns = new Array("Tournament Name", "Tournament Level");
		var sortColumn = "Tournament Name";
		var SortOrder = "desc";		
	} else if (tableType == "allmasterpoint") {
		var columns = new Array("Event Date", "Tourney", "Tourney Type", "Event", "Member ID", "Member Name", "Local Points", "Fed Points", "Total Points");
		var sortColumn = "Event Date";
		var SortOrder = "desc";
	}
	else if (tableType == "lookupmemberid") {
		var columns = new Array("ID", "NAME");
		var sortColumn = "NAME";
		var SortOrder = "asc";		
	}
	else {
		alert("Unknown Table Type : " + tableType);
		return;
	}
	var sortParameters = new Array();
	sortParameters[0] = new Array();
	sortParameters[0][0] = columns.indexOf(sortColumn);
	sortParameters[0][1] = SortOrder;
	jQuery.each(columns, function(index, value) {
		html += '<th>' + value + '</th>';
	});
	html += '</tr></thead></table>';

	var tableID = '#bfi_masterpoints_table';
	jQuery('#bfi_masterpoints_table_container').html(html);
	var myTable = jQuery(tableID).dataTable({
		"bProcessing" : true,
		"bJQueryUI" : true,
		"bServerSide" : true,
		"sPaginationType" : "full_numbers",
		"aaSorting" : sortParameters,
		"bAutoWidth" : true,
		"fnServerParams" : function(aoData) {
			aoData.push({
				"name" : "BFI_Table_Type",
				"value" : tableType
			});
			aoData.push({
				"name" : "member_id",
				"value" : member_id
			});
			aoData.push({
				"name" : "from_date",
				"value" : filterFromDate
			});
			aoData.push({
				"name" : "to_date",
				"value" : filterToDate
			});						
		},
		"sAjaxSource" : server_side_url
	});
	jQuery( tableID+'_filter input').unbind();
	jQuery( tableID+'_filter input').bind('keyup', function(e) {
		if(e.keyCode == 13) {
			myTable.fnFilter(this.value);
		}
	});
}