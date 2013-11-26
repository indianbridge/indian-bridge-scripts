<?php

/**
 * Class to manage display masterpoints
 */

if (!class_exists('BFI_Masterpoint_Display_Shortcode')) {

	class BFI_Masterpoint_Display_Shortcode {
		
		private $bfi_masterpoint_db;
		private $table_prefix;
		private $member_id = null;
		private $tournament_code = null;
		private $event_code = null;
		
		
		public function __construct($db, $table_prefix) {
			$this->bfi_masterpoint_db = $db;
			$this->table_prefix = $table_prefix;
			// Add the shortcode
			add_shortcode('bfi_masterpoint_display', array($this, 'bfi_masterpoint_display'));	
			add_filter( 'query_vars', array($this, 'add_query_vars_filter' ));		
		}	
		
		public function add_query_vars_filter( $vars ){
			$vars[] = "member_id";
			$vars[] = "tournament_code";
			$vars[] = "event_code";
			return $vars;
		}
		
		
		/**
		 * Replace shortcode with posts
		 */
		function bfi_masterpoint_display($atts, $content = null) {
			$this->member_id = get_query_var('member_id');
			$this->tournament_code = get_query_var('tournament_code');
			$this->event_code = get_query_var('event_code');
			if (is_user_logged_in()) {
				return $this -> showLoggedInShortCodeContent();
			} else {
				return $this -> showNotLoggedInShortcodeContent();
			}
		}
		
		function showNotLoggedInShortcodeContent() {
			ob_start();
			echo '<h1>You have to be a member of BFI and be logged in to see Masterpoint Summary and Details</h1>';
			$tabs = array('lookupmemberid' => 'Find Your BFI Member ID', 'leaderboard' => "Masterpoint Leaderboard");
			$selectedTab = 'lookupmemberid';
			echo $this -> getMasterpointTabs($tabs, $selectedTab, '');
			$out = ob_get_clean();
			return $out;
		}	
		
		function showLoggedInShortCodeContent() {
			$current_user = wp_get_current_user();
			if (0 == $current_user -> ID) {
				return $this -> showNotLoggedInShortcodeContent();
			}
			$member_id = $current_user -> user_login;
			$membership_info = $this -> getBFIMembershipInfo($member_id);
			if ($membership_info != null) {
				return $this -> showMemberShortcodeContent($current_user, $membership_info);
			} else {
				return $this -> showNonMemberShortcodeContent($current_user);
			}
		}

		function showMemberShortcodeContent($current_user, $membership_info) {
			ob_start();
			echo $this -> getMemberSummary($current_user);
			echo '<div class="fl" style="padding-right:10px;" title="BFI Member Info">';
			echo '<span class="ico aut">Member Number: ' . $membership_info -> member_number . '</span><br/>';
			echo '<span class="ico ranking-icon">Rank: ' . $membership_info -> rank . '</span><br/>';
			echo '<span class="ico map-icon">Zone: ' . $membership_info -> zone . '</span>';
			echo '</div>';
			echo '<div class="fl" style="padding-right:10px;" title="BFI Member Points">';
			echo '<span class="ico trophy-icon">Total Points: ' . $membership_info -> total_points . '</span><br/>';
			echo '<span class="ico trophy-silver-icon">Fed Points: ' . $membership_info -> fed_points . '</span><br/>';
			echo '<span class="ico trophy-bronze-icon">Local Points: ' . $membership_info -> local_points . '</span>';
			echo '</div>';
			echo '<div class="clear h10"><!-- --></div>';
			$tabs = array('mymasterpoint' => "My Masterpoints", 'leaderboard' => "Masterpoint Leaderboard", "allmasterpoint" => "All Masterpoints");
			$selectedTab = 'mymasterpoint';
			echo $this -> getMasterpointTabs($tabs, $selectedTab, $current_user -> user_login);
			$out = ob_get_clean();
			return $out;
		}
				

		function showNonMemberShortcodeContent($current_user) {
			ob_start();
			echo $this -> getMemberSummary($current_user);
			//$member_id = get_query_var('bfi_member_id');
			echo '<h1>You have to be a member of BFI to see your Masterpoint Summary and Details</h1>';
			//if (empty($member_id)) {
				$tabs = array('leaderboard' => "Masterpoint Leaderboard", "allmasterpoint" => "All Masterpoints");
			/*}
			else {
				$tabs = array('mymasterpoint' => $member_id."'s Masterpoints", 'leaderboard' => "Masterpoint Leaderboard", "allmasterpoint" => "All Masterpoints");
				$selectedTab = 'mymasterpoint';	
			}*/
			$selectedTab = 'leaderboard';
			//echo $this -> getMasterpointTabs($tabs, $selectedTab, $member_id);
			echo $this -> getMasterpointTabs($tabs, $selectedTab, '');
			$out = ob_get_clean();
			return $out;
		}
		
		function getMasterpointTabs($tabs, $selectedTab, $member_id) {
			$html = "";
			$html .= '<div class="tabber-widget-default">';
			$html .= '<ul class="tabber-widget-tabs">';
			$tabIDPrefix = 'masterpoint_page_tab_';
			foreach ($tabs as $tab => $tabName) {
				$html .= '<li><a id="' . $tabIDPrefix . $tab . '" onclick="switchMasterpointPageTab(\'' . $tab . '\',\'' . $tabIDPrefix . '\',\'' . plugins_url('jquery.datatables.php', __FILE__) . '\',\'' . $member_id . '\',\'' . DB_NAME . '\');" href="javascript:void(0)">' . $tabName . '</a></li>';
			}
			$html .= '</ul>';
			$html .= '<div class="tabber-widget-content">';
			$html .= '<div class="tabber-widget">';
			$html .= '<div id="bfi_masterpoints_table_container">';
			$html .= '</div></div></div></div>';
			$html .= '<script type="text/javascript">';
			$html .= 'switchMasterpointPageTab(\'' . $selectedTab . '\',\'' . $tabIDPrefix . '\',\'' . plugins_url('jquery.datatables.php', __FILE__) . '\',\'' . $member_id . '\',\'' . DB_NAME . '\');';
			$html .= '</script>';
			return $html;
		}

		function getMemberSummary($current_user) {
			$html = "";
			$html .= '<div class="fl" style="padding-right:10px;" title="Member Photo"><em>' . get_avatar($current_user -> ID, '110') . '</em></div>';
			$html .= '<div>Name : ' . $current_user -> display_name . '</div>';
			return $html;
		}
		
		function getBFIMembershipInfo($member_id) {
			$table_prefix = $this -> table_prefix;
			$member_tableName = $table_prefix . 'member';
			$zone_tableName = $table_prefix . 'zone_master';
			$rank_tableName = $table_prefix . 'rank_master';
			$query = "SELECT " . $member_tableName . ".member_id AS member_number, " . $rank_tableName . ".description AS rank, " . $zone_tableName . ".description AS zone, (" . $member_tableName . ".total_current_lp+" . $member_tableName . ".total_current_fp) AS total_points, " . $member_tableName . ".total_current_fp AS fed_points, " . $member_tableName . ".total_current_lp AS local_points FROM " . $member_tableName . " ";
			$query .= "JOIN " . $zone_tableName . " ON " . $member_tableName . ".zone_code=" . $zone_tableName . ".zone_code JOIN " . $rank_tableName . " ON " . $member_tableName . ".rank_code=" . $rank_tableName . ".rank_code WHERE member_id=%s LIMIT 1";
			$mydb = $this -> bfi_masterpoint_db;
			$rows = $mydb -> get_results($mydb -> prepare($query, $member_id));
			if (count($rows) > 0)
				return $rows[0];
			else
				return null;
		}	

	}
}
?>