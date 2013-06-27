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
		echo '<span class="ico profile-icon"><a href="http://localhost/bfi/wp-admin/profile.php">' . self::get_title( 'profile' ) . '</a></span><br/>';
		echo '<span class="ico logout-icon"><a href="' . wp_logout_url() . '">' . self::get_title( 'logout' ) . '</a></span>';
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
	$mydb= new wpdb('indianbridge','kibitzer','masterpoints','localhost');
	$current_user = wp_get_current_user();
	//$member_id = 'WB000777';
	$member_id = $current_user->user_login;
	?>
	<h5>Masterpoints</h5>
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
						getMasterpointSummary($mydb,$member_id);
					?>
				</div>
				<div id="masterpoint_content_2" style="display:none;">
				<?php
					getRecentMasterpointList($mydb,$member_id)
				?>
				</div>
				<div id="masterpoint_content_3" style="display:none;">
				<?php
					getMasterpointLeaders($mydb,$member_id)
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
function getMasterpointSummary($mydb,$member_id) {	
		$query = "SELECT * FROM member WHERE member_id=%s";
		$rows = $mydb->get_results( $mydb->prepare($query,$member_id));
		$numItems = count($rows);
		if ($numItems < 1) {
			echo '<h6>Not a member of BFI</h6>';
		}
		else {
			$row=$rows[0];
			$query = "SELECT description FROM rank_master WHERE rank_code=%s";
			$rows = $mydb->get_results( $mydb->prepare($query,$row->rank_code));
			if (count($rows) > 0) {
				echo '<span class="ico ranking-icon">Rank: '.$rows[0]->description.'</span>';
			}
			else {
				echo '<span class="ico ranking-icon">Rank: Unknown</span>';
			}
			echo '<br/>';
			$query = "SELECT description FROM zone_master WHERE zone_code=%s";
			$rows = $mydb->get_results( $mydb->prepare($query,$row->zone_code));
			if (count($rows) > 0) {
				echo '<span class="ico map-icon">Zone: '.$rows[0]->description.'</span>';
			}
			else {
				echo '<span class="ico map-icon">Zone: Unknown</span>';
			}		
			echo '<br/>';	
			$sumField = 'localpoints_earned';
			$tableName = 'tournament_masterpoint';
			$query = "SELECT SUM(".$sumField.") FROM ".$tableName." WHERE member_id = %s";
			$localTotal = $mydb->get_var( $mydb->prepare($query,$member_id) );
			
			$sumField = 'fedpoints_earned';
			$query = "SELECT SUM(".$sumField.") FROM ".$tableName." WHERE member_id = %s";
			$fedTotal = $mydb->get_var( $mydb->prepare($query,$member_id) );
			echo '<span><a href="#">Total Points: '.($localTotal+$fedTotal).'</a></span><br/>';
			echo '<span><a href="#">Fed Points: '.$fedTotal.'</a></span><br/>';
			echo '<span><a href="#">Local Points: '.$localTotal.'</a></span><br/>';
		}	
}

function showTable($rows,$fields) {
	if(count($rows) <= 0) {
		echo '<h6>No Entries Found!</h6>';
		return;
	}	
	echo '<div class="datagrid">';
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

function getRecentMasterpointList($mydb,$member_id) {
	$query = "SELECT * FROM tournament_masterpoint WHERE member_id=%s ORDER BY event_date DESC LIMIT 0,10";
	$rows = $mydb->get_results( $mydb->prepare($query,$member_id));	
	$fields = array('event_date' => 'Date','fedpoints_earned' => 'Fed','localpoints_earned' => 'Local');
	showTable($rows,$fields);
}

function getMasterpointLeaders($mydb,$member_id) {
	//$query = "SELECT *,(fedpoints_earned+localpoints_earned) AS totalpoints_earned FROM member WHERE member_id=%s ORDER BY totalpoints_earned DESC LIMIT 0,10";
	$query = "SELECT CONCAT(first_name,' ',last_name) AS full_name,(total_current_fp+total_current_lp) AS total FROM member ORDER BY total DESC LIMIT 0,10";
	$rows = $mydb->get_results( $mydb->prepare($query,$member_id));	
	$fields = array('full_name' => 'Name','total' => 'Total');
	showTable($rows,$fields);	
}
?>
