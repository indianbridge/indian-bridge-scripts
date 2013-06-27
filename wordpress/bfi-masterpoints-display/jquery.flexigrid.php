<?php
// Connect to MySQL database
mysql_connect('localhost', 'indianbridge', 'kibitzer');
mysql_select_db('masterpoints');
$page = 1; // The current page
$sortname = 'event_date'; // Sort column
$sortorder = 'desc'; // Sort order
$qtype = ''; // Search column
$query = ''; // Search string
// Get posted data
if (isset($_POST['page'])) {
	$page = mysql_real_escape_string($_POST['page']);
}
if (isset($_POST['sortname'])) {
	$sortname = mysql_real_escape_string($_POST['sortname']);
}
if (isset($_POST['sortorder'])) {
	$sortorder = mysql_real_escape_string($_POST['sortorder']);
}
if (isset($_POST['qtype'])) {
	$qtype = mysql_real_escape_string($_POST['qtype']);
}
if (isset($_POST['query'])) {
	$query = mysql_real_escape_string($_POST['query']);
}
if (isset($_POST['rp'])) {
	$rp = mysql_real_escape_string($_POST['rp']);
}
if (isset($_POST['member_id'])) {
	$member_id = mysql_real_escape_string($_POST['member_id']);
}
// Setup sort and search SQL using posted data
$sortSql = "order by $sortname $sortorder";
$searchSql = ($qtype != '' && $query != '') ? "where $qtype = '$query'" : "where member_id = '$member_id'";
// Get total count of records
$sql = "select count(*)
from tournament_masterpoint
$searchSql";
$result = mysql_query($sql);
$row = mysql_fetch_array($result);
$total = $row[0];
// Setup paging SQL
$pageStart = ($page-1)*$rp;
$limitSql = "limit $pageStart, $rp";
// Return JSON data
$data = array();
$data['page'] = $page;
$data['total'] = $total;
$data['rows'] = array();
$sql = "select tournament_code, event_code, member_id, event_date, localpoints_earned, fedpoints_earned
from tournament_masterpoint
$searchSql
$sortSql
$limitSql";
$results = mysql_query($sql);
$count = 1;
while ($row = mysql_fetch_assoc($results)) {
	$data['rows'][] = array(
		'id' => $count,
		'cell' => array(getTournamentName($row['tournament_code']), getEventName($row['tournament_code']), getMemberName($row['member_id']), $row['event_date'], $row['localpoints_earned'], $row['fedpoints_earned'])
		);
	$count = $count + 1;
}
echo json_encode($data);


function getTournamentName($tournament_code) {
	$searchSql = "where tournament_code = '$tournament_code'";
	$sql = "select * from tournament_master $searchSql";
	$result = mysql_query($sql);
	$row = mysql_fetch_assoc($result);
	if ($row) {
		return $row['description'];
	} else { return '-';}

}

function getEventName($tournament_code) {
	$searchSql = "where tournament_code = '$tournament_code'";
	$sql = "select * from tournament_master $searchSql";
	$result = mysql_query($sql);
	$row = mysql_fetch_assoc($result);
	if ($row) {
		$tournament_level_code = $row['tournament_level_code'];
		$searchSql = "where tournament_level_code = '$tournament_level_code'";
		$sql = "select * from tournament_level_master $searchSql";	
		$result = mysql_query($sql);
		$row = mysql_fetch_assoc($result);	
		if ($row) {
			return $row['description'];
		} else { return '-';}	
	} else { return '-';}
}

function getMemberName($member_id) {
	$searchSql = "where member_id = '$member_id'";
	$sql = "select * from member $searchSql";
	$result = mysql_query($sql);
	$row = mysql_fetch_assoc($result);
	if ($row) {
		return $row['first_name'].' '.$row['last_name'];
	} else { return '-';}			
}	
?>