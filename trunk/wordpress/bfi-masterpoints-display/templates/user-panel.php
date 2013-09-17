<?php
/*
If you would like to edit this file, copy it to your current theme's directory and edit it there.
Theme My Login will always look in your theme's directory first, before using this default template.
*/
?>
<div class="login" id="theme-my-login<?php $template->the_instance(); ?>">
	<?php if ( $template->options['show_gravatar'] ) : ?>
	<div class="one_half"><?php $template->the_user_avatar(); ?></div>
	<?php endif; ?>

	<div class="one_half">
		<?php 
		echo '<span class="ico profile-icon"><a href="'.admin_url("profile.php").'">Profile</a></span><br/>';
		echo '<span class="ico logout-icon"><a href="' . wp_logout_url() . '">Logout</a></span>';
		?>
	</div>
	<div class="clear"><!-- --></div>
	<script type="text/javascript">
		function user_panel_switchTabs(panelNumber) {
			var selectedClass = 'selected';
			for (var i=1;i<=3;i++) {
				if(i==panelNumber) {
					jQuery('#masterpoint_tab_'+i).addClass(selectedClass);
					jQuery('#masterpoint_content_'+i).show();
				}
				else {
					jQuery('#masterpoint_tab_'+i).removeClass(selectedClass)
					jQuery('#masterpoint_content_'+i).hide();
				}
			}
		}
	</script> 		
	<?php
	global $wpdb;
	$mydb = $wpdb;
	$table_prefix = 'bfi_';
	//$mydb= new wpdb('bfinem7l_sriram','kibitzer','bfinem7l_masterpoints','localhost');
	$current_user = wp_get_current_user();
	$member_id = $current_user->user_login;
	$masterpointURL = home_url("/meber_services/masterpoints");
	?>
	<h5>Masterpoints</h5>
	<h6><a href="<?php echo $masterpointURL; ?>">Click for Detailed Masterpoint Page</a></h6>
	<div class="tabber-widget-default">
		<ul class="tabber-widget-tabs">
			<li><a id="masterpoint_tab_1" onclick="user_panel_switchTabs(1);" href="javascript:void(0)">Summary</a></li>		
			<li><a id="masterpoint_tab_2" onclick="user_panel_switchTabs(2);" href="javascript:void(0)">Recent</a></li>
			<li><a id="masterpoint_tab_3" onclick="user_panel_switchTabs(3);" href="javascript:void(0)">Leaders</a></li>
		</ul>
		<div class="tabber-widget-content">
			<div class="tabber-widget">
				<div id="masterpoint_content_1" style="display:none;">
					<?php
						getMasterpointSummary($mydb,$member_id,$table_prefix);
					?>
				</div>
				<div id="masterpoint_content_2" style="display:none;">
				<?php
					getRecentMasterpointList($mydb,$member_id,$table_prefix)
				?>
				</div>
				<div id="masterpoint_content_3" style="display:none;">
				<?php
					getMasterpointLeaders($mydb,$member_id,$table_prefix)
				?>
				</div>
			</div>
		</div>
	</div>
	<script type="text/javascript">
		user_panel_switchTabs(1);
	</script>
	
	<?php do_action( 'tml_user_panel' ); ?>
</div>

<?php
function getMasterpointSummary($mydb,$member_id,$table_prefix) {
	$member_tableName = $table_prefix.'member';	
	$zone_tableName = $table_prefix.'zone_master';	
	$rank_tableName = $table_prefix.'rank_master';	
	$query = "SELECT ".$member_tableName.".member_id AS member_number, ".$rank_tableName.".description AS rank, ".$zone_tableName.".description AS zone, (".$member_tableName.".total_current_lp+".$member_tableName.".total_current_fp) AS total_points, ".$member_tableName.".total_current_fp AS fed_points, ".$member_tableName.".total_current_lp AS local_points FROM ".$member_tableName." ";
	$query .= "JOIN ".$zone_tableName." ON ".$member_tableName.".zone_code=".$zone_tableName.".zone_code JOIN ".$rank_tableName." ON ".$member_tableName.".rank_code=".$rank_tableName.".rank_code WHERE member_id=%s LIMIT 1";
	$rows = $mydb->get_results( $mydb->prepare($query,$member_id));
	$numItems = count($rows);
	if ($numItems < 1) {
		echo '<h6>Not a member of BFI</h6>';
	}
	else {
		$row=$rows[0];
		echo '<span class="ico ranking-icon">Rank: '.$rows[0]->rank.'</span><br/>';
		echo '<span class="ico map-icon">Zone: '.$rows[0]->zone.'</span><br/>';	
		echo '<span class="ico trophy-icon">Total Points: '.$rows[0]->total_points.'</span><br/>';
		echo '<span class="ico trophy-silver-icon">Fed Points: '.$rows[0]->fed_points.'</span><br/>';
		echo '<span class="ico trophy-bronze-icon">Local Points: '.$rows[0]->local_points.'</span><br/>';		
	}	
}

function showTable($rows,$fields) {
	if(count($rows) <= 0) {
		echo '<h6>No Entries Found!</h6>';
		return;
	}	
	echo '<div class="table-blue">';
	echo '<table class="stripeme"><thead><tr>';
	foreach($fields as $fieldKey => $fieldValue) {
		echo '<th>'.$fieldValue.'</th>';
	}
	echo '</tr></thead><tbody>';
	foreach($rows as $row) {
		echo '<tr>';
		foreach($fields as $fieldKey => $fieldValue) {
			echo '<td>'.$row->$fieldKey.'</td>';
		}
		echo '</tr>';
	}
	echo '</tbody></table></div>';		
}

function getRecentMasterpointList($mydb,$member_id,$table_prefix) {
	$tableName = $table_prefix.'tournament_masterpoint';
	$query = "SELECT * FROM ".$tableName." WHERE member_id=%s ORDER BY event_date DESC LIMIT 0,10";
	$rows = $mydb->get_results( $mydb->prepare($query,$member_id));	
	$fields = array('event_date' => 'Date','fedpoints_earned' => 'Fed','localpoints_earned' => 'Local');
	showTable($rows,$fields);
}

function getMasterpointLeaders($mydb,$member_id,$table_prefix) {
	$tableName = $table_prefix.'member';
	$query = "SELECT CONCAT(first_name,' ',last_name) AS full_name,(total_current_fp+total_current_lp) AS total FROM ".$tableName." ORDER BY total DESC LIMIT 0,10";
	$rows = $mydb->get_results( $mydb->prepare($query,$member_id));	
	$fields = array('full_name' => 'Name','total' => 'Total');
	showTable($rows,$fields);	
}
?>
