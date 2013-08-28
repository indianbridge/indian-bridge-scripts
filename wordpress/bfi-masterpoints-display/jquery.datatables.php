<?php
$paramName = 'member_id';
if ( isset( $_GET[$paramName] ) && $_GET[$paramName] != '' ) {
$member_id = $_GET[$paramName];
}
else {
$member_id = '';
}

$paramName = 'BFI_Table_Type';
if ( isset( $_GET[$paramName] ) && $_GET[$paramName] != '' ) {
	$tableType = $_GET[$paramName];
}
else {
	$tableType = 'leaderboard';
}

if ($tableType == 'leaderboard' || $member_id == '') {
	doLeaderboard();
}
else if ($tableType == 'mymasterpoint') {
	doMyMasterpoint($member_id);
}

function connectToDB() {
	
	/* Database connection information */
	$gaSql['user']       = "bfinem7l_sriram";
	$gaSql['password']   = "kibitzer";
	$gaSql['db']         = "bfinem7l_bfitest";
	$gaSql['server']     = "localhost";
	
	
	/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
	 * If you just want to use the basic configuration for DataTables with PHP server-side, there is
	 * no need to edit below this line
	 */
	
	/* 
	 * MySQL connection
	 */
	$gaSql['link'] =  mysql_pconnect( $gaSql['server'], $gaSql['user'], $gaSql['password']  ) or
	die( 'Could not open connection to server' );
	
	mysql_select_db( $gaSql['db'], $gaSql['link'] ) or 
	die( 'Could not select database '. $gaSql['db'] );	

	return $gaSql['link'];
}

function getLimit() {
	$sLimit = "";
	if ( isset( $_GET['iDisplayStart'] ) && $_GET['iDisplayLength'] != '-1' )
	{
		$sLimit = "LIMIT ".mysql_real_escape_string( $_GET['iDisplayStart'] ).", ".
			mysql_real_escape_string( $_GET['iDisplayLength'] );
	}	
	return $sLimit;
}

function getOrder($aColumns) {
	$sOrder = "";
	if ( isset( $_GET['iSortCol_0'] ) )
	{
		$aValues = array_values($aColumns);
		$sOrder = "ORDER BY  ";
		for ( $i=0 ; $i<intval( $_GET['iSortingCols'] ) ; $i++ )
		{
			if ( $_GET[ 'bSortable_'.intval($_GET['iSortCol_'.$i]) ] == "true" )
			{
				$sOrder .= $aValues[ intval( $_GET['iSortCol_'.$i] ) ]."
				 	".mysql_real_escape_string( $_GET['sSortDir_'.$i] ) .", ";
			}
		}
		
		$sOrder = substr_replace( $sOrder, "", -2 );
		if ( $sOrder == "ORDER BY" )
		{
			$sOrder = "";
		}
	}	
	return $sOrder;
}

function doSearch($aColumns) {
	$sWhere = "";
	if ( $_GET['sSearch'] != "" )
	{
		$sWhere = "(";	
		foreach ($aColumns as $key => $value) {
			$sWhere .= $key." LIKE '%".mysql_real_escape_string( $_GET['sSearch'] )."%' OR ";
		}
		$sWhere = substr_replace( $sWhere, "", -3 );
		$sWhere .= ')';
	}
	else {
		$sWhere = "";
	}	
	return $sWhere;
}

function getResults($sqlLink, $sQuery_old, $sIndexColumn, $sTable, $aColumns) {
	$rResult = mysql_query( $sQuery_old, $sqlLink ) or die(mysql_error());
	
	/* Data set length after filtering */
	$sQuery = "
		SELECT FOUND_ROWS()
	";
	$rResultFilterTotal = mysql_query( $sQuery, $sqlLink) or die(mysql_error());
	$aResultFilterTotal = mysql_fetch_array($rResultFilterTotal);
	$iFilteredTotal = $aResultFilterTotal[0];
	
	/* Total data set length */
	$sQuery = "
		SELECT COUNT(".$sIndexColumn.")
		FROM   $sTable
	";
	$rResultTotal = mysql_query( $sQuery,$sqlLink) or die(mysql_error());
	$aResultTotal = mysql_fetch_array($rResultTotal);
	$iTotal = $aResultTotal[0];
	
	
	/*
	 * Output
	 */
	$output = array(
		"sEcho" => intval($_GET['sEcho']),
		"iTotalRecords" => $iTotal,
		"iTotalDisplayRecords" => $iFilteredTotal,
		"aaData" => array()
	);
	
	while ( $aRow = mysql_fetch_array( $rResult ) )
	{
		$row = array();
		foreach ($aColumns as $key => $value) {
			if ( $value == "version" )
			{
				// Special output formatting for 'version' column 
				$row[] = ($aRow[ $value ]=="0") ? '-' : $aRow[ $value ];
			}
			else if ( $value != ' ' )
			{
				// General output 
				$row[] = $aRow[ $value ];
			}			
		}
		$output['aaData'][] = $row;
	}
	return $output;
	//echo json_encode( $output );	
}

function doLeaderboard() {
	$sqlLink = connectToDB();
	$aColumns = array( 'member_id'=>'member_id',"CONCAT(first_name,' ',last_name)"=>'full_name','(total_current_fp+total_current_lp)'=>'total','total_current_fp'=>'total_current_fp','total_current_lp'=>'total_current_lp');

	// Indexed column (used for fast and accurate table cardinality) 
	$sIndexColumn = "member_id";
	$sTable = "bfi_member";
	$sqlLink = connectToDB();
	$sLimit = getLimit();
	//$sOrder = "";
	$sOrder = getOrder($aColumns);
	$sWhereSearch = doSearch($aColumns);
	$sWhere = "";
	if($sWhereSearch != "") {
		$sWhere .= " WHERE ".$sWhereSearch;
	}
	$sJoin = "";
	$sQuery = "
		SELECT SQL_CALC_FOUND_ROWS ";
	foreach ($aColumns as $key => $value) {
	  $sQuery .= "$key as $value,";
	}
	$sQuery = rtrim($sQuery,',');		
	$sQuery .= " FROM   $sTable
		$sJoin
		$sWhere
		$sOrder
		$sLimit
	";
	$output = getResults($sqlLink, $sQuery, $sIndexColumn, $sTable, $aColumns);
	echo json_encode( $output );
}



function doMyMasterpoint($member_id) {

	$aColumns = array( 'bfi_tournament_masterpoint.event_date'=>'event_date','bfi_tournament_master.description'=>'tournament_name','bfi_tournament_level_master.description'=>'tournament_type','bfi_event_master.description'=>'event_code','bfi_tournament_masterpoint.localpoints_earned'=>'localpoints_earned','bfi_tournament_masterpoint.fedpoints_earned'=>'fedpoints_earned','(bfi_tournament_masterpoint.localpoints_earned+bfi_tournament_masterpoint.fedpoints_earned)'=>'totalpoints');
	// Indexed column (used for fast and accurate table cardinality)
	$sIndexColumn = "*";
	$sTable = "bfi_tournament_masterpoint";

	//Connect
	$sqlLink = connectToDB();

	//Paging
	$sLimit = getLimit();

	//Ordering
	$sOrder = getOrder($aColumns);
	
	//Filtering
	$sWhereSearch = doSearch($aColumns);
	$sWhere = "WHERE (member_id = '".$member_id."'";
	if($sWhereSearch != "") {
		$sWhere .= " AND ".$sWhereSearch;
	}
	$sWhere .= ")";
	
	//Join
	$sJoin = "";
	$sJoin .= "JOIN bfi_tournament_master ON bfi_tournament_masterpoint.tournament_code = bfi_tournament_master.tournament_code ";
	$sJoin .= "JOIN bfi_event_master ON bfi_event_master.event_code = bfi_tournament_masterpoint.event_code ";
	$sJoin .= "JOIN bfi_tournament_level_master ON bfi_tournament_level_master.tournament_level_code = bfi_tournament_master.tournament_level_code";
	
	//Query
	$sQuery = "
		SELECT SQL_CALC_FOUND_ROWS ";
	foreach ($aColumns as $key => $value) {
	  $sQuery .= "$key as $value,";
	}
	$sQuery = rtrim($sQuery,',');		
	$sQuery .= " FROM   $sTable
		$sJoin
		$sWhere
		$sOrder
		$sLimit
	";

	//Results
	$output = getResults($sqlLink, $sQuery, $sIndexColumn, $sTable, $aColumns);
	echo json_encode( $output );
}

?>