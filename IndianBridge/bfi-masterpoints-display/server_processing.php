<?php
			$newdb = new wpdb('indianbridge', 'kibitzer', 'masterpoints', 'localhost');
			$newdb->show_errors();
			$retrieve_data = $newdb->get_results( "SELECT * FROM tournament_masterpoint WHERE member_id = 'TM000277'" );

			
			$iTotal = ''.count($retrieve_data);
			$iFilteredTotal = ''.count($retrieve_data);

	
	/*
	 * Output
	 */
	$output = array(
		"sEcho" => intval($_GET['sEcho']),
		"iTotalRecords" => $iTotal,
		"iTotalDisplayRecords" => $iFilteredTotal,
		"aaData" => array()
	);
	/*foreach ($retrieve_data as $retrieved_data) {
		$row = array();
		$row[] = 'Test';
		$row[] = 'Sriram';
		//$row[] = $retrieved_data->event_code;
		//$row[] = $retrieved_data->fedpoints_earned;
		$output['aaData'][] = $row;
	}*/
	
	echo json_encode( $output );
?>