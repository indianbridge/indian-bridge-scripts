<?php
/*
 Plugin Name: BFI Masterpoint Display
 Plugin URI: http://bfi.net.in
 Description: A plugin to manage all masterpoint capabilities for BFI site
 Author: Sriram Narasimhan
 Author URI: ...
 Version: 1.0

 Copyright: � 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
 License: GNU General Public License v3.0
 License URI: http://www.gnu.org/licenses/gpl-3.0.html
 */

require_once (plugin_dir_path(__FILE__) . 'includes' . DIRECTORY_SEPARATOR . 'custom_admin_bar_css.php');
require_once (plugin_dir_path(__FILE__) . 'includes' . DIRECTORY_SEPARATOR . 'custom_user_profile.php');
require_once (plugin_dir_path(__FILE__) . 'includes' . DIRECTORY_SEPARATOR . 'masterpoint_management.php');
require_once (plugin_dir_path(__FILE__) . 'includes' . DIRECTORY_SEPARATOR . 'masterpoint_display.php');
require_once( ABSPATH . '/wp-admin/includes/user.php');

if (!class_exists('BFI_Masterpoint_Display')) {

	class BFI_Masterpoint_Display {
		private $bfi_masterpoint_db;
		private $errorFlag;
		private $table_prefix;
		private $custom_admin_bar_css;
		private $custom_user_profile;
		private $jsonDelimiter;
		private $bfi_masterpoint_manager;
		
		public function __construct() {
			$dataGridName = "datatables";

			$jsFilePath = 'js/jquery.' . $dataGridName . '.min.js';
			$cssFilePath = 'css/jquery.' . $dataGridName . '.css';
			$jsIdentifier = 'jquery_' . $dataGridName . '_js';
			$cssIdentifier = 'jquery_' . $dataGridName . '_css';

			// register the javascript
			wp_register_script($jsIdentifier, plugins_url($jsFilePath, __FILE__), array('jquery'));
			wp_enqueue_script($jsIdentifier);
			/*$js_columnFilterIdentifier = 'columnFilter';
			$js_columnFilterFile = 'js/jquery.dataTables.columnFilter.js';
			wp_register_script($js_columnFilterIdentifier, plugins_url($js_columnFilterFile, __FILE__), array('jquery'));
			wp_enqueue_script($js_columnFilterIdentifier);*/
			
			// Register bfi master point javascript
			wp_register_script('bfi_masterpoints_display', plugins_url('bfi-masterpoints-display.js', __FILE__), array('jquery'));
			wp_enqueue_script('bfi_masterpoints_display');

			// register the css
			wp_register_style($cssIdentifier, plugins_url('css/jquery.datatables_themeroller.css', __FILE__));
			wp_enqueue_style($cssIdentifier);
			wp_register_style('jquery_dataTables_redmond_css', plugins_url('css/jquery_ui/jquery-ui-1.10.3.custom.min.css', __FILE__));
			wp_enqueue_style('jquery_dataTables_redmond_css');

			
			add_action('admin_bar_menu', array($this, 'customize_admin_bar'));
			//add_filter( 'query_vars', array($this, 'add_query_vars_filter' ));

			register_activation_hook(__FILE__, array($this, 'activate'));
			register_deactivation_hook(__FILE__, array($this, 'deactivate'));
			global $wpdb;
			$this -> bfi_masterpoint_db = $wpdb;
			$this -> table_prefix = 'bfi_';
			//$table_prefix = 'bfi_';

			// Customize the user profile
			$this -> custom_user_profile = new BFI_Custom_User_Profile($wpdb, $this -> table_prefix);

			// Customize the admin bar
			$this -> custom_admin_bar_css = new BFI_Custom_Admin_Bar();
			$this->bfi_masterpoint_manager = new BFI_Masterpoint_Manager($wpdb,$this->table_prefix);
			
			// Some constants
			$this -> errorFlag = false;	
			$this->jsonDelimiter = "!@#";
			
			$bfi_masterpoint_display_shortcode = new BFI_Masterpoint_Display_Shortcode($wpdb,$this->table_prefix);
		}

		public function add_masterpoint_admin_bar($wp_admin_bar) {
			$wp_admin_bar -> add_menu(array('parent' => 'top-secondary', "id" => "my-masterpoints", "title" => __('myMasterpoints'), "href" => home_url("/member_services/masterpoints"), ));
			$wp_admin_bar -> add_group(array('parent' => 'my-masterpoints', 'id' => 'my-masterpoint-summary', ));
			$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-summary', 'id' => 'masterpoints-summary', 'title' => __('<h6>Masterpoint Summary</h6>'), ));
			$wp_admin_bar -> add_group(array('parent' => 'my-masterpoints', 'id' => 'my-masterpoint-recent', ));
			$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-recent', 'id' => 'masterpoints-recent', 'title' => __('<h6>Recent Masterpoints</h6>'), ));

			$my_masterpoint_html = '';
			$my_recent_masterpoint_html = '';
			$current_user = wp_get_current_user();
			$member_id = $current_user -> user_login;
			$table_prefix = $this -> table_prefix;
			$mydb = $this -> bfi_masterpoint_db;
			$member_tableName = $table_prefix . 'member';
			$zone_tableName = $table_prefix . 'zone_master';
			$rank_tableName = $table_prefix . 'rank_master';
			$query = "SELECT " . $member_tableName . ".member_id AS member_number, " . $rank_tableName . ".description AS rank, " . $zone_tableName . ".description AS zone, (" . $member_tableName . ".total_current_lp+" . $member_tableName . ".total_current_fp) AS total_points, " . $member_tableName . ".total_current_fp AS fed_points, " . $member_tableName . ".total_current_lp AS local_points FROM " . $member_tableName . " ";
			$query .= "JOIN " . $zone_tableName . " ON " . $member_tableName . ".zone_code=" . $zone_tableName . ".zone_code JOIN " . $rank_tableName . " ON " . $member_tableName . ".rank_code=" . $rank_tableName . ".rank_code WHERE member_id=%s LIMIT 1";
			$rows = $mydb -> get_results($mydb -> prepare($query, $member_id));
			$numItems = count($rows);
			if ($numItems < 1) {
				$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-summary', 'id' => 'masterpoint-summary-list-item1', 'title' => '<div class="ab-item ab-empty-item">No Information Found!</div>'));
			} else {
				$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-summary', 'id' => 'masterpoint-summary-list-item2', 'title' => '<span class="ico ranking-icon">Rank: ' . $rows[0] -> rank . '</span>'));
				$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-summary', 'id' => 'masterpoint-summary-list-item3', 'title' => '<span class="ico map-icon">Zone: ' . $rows[0] -> zone . '</span>'));
				$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-summary', 'id' => 'masterpoint-summary-list-item4', 'title' => '<span class="ico trophy-icon">Total Points: ' . $rows[0] -> total_points . '</span>'));
				$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-summary', 'id' => 'masterpoint-summary-list-item5', 'title' => '<span class="ico trophy-silver-icon">Fed Points: ' . $rows[0] -> fed_points . '</span>'));
				$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-summary', 'id' => 'masterpoint-summary-list-item1', 'title' => '<span class="ico trophy-bronze-icon">Local Points: ' . $rows[0] -> local_points . '</span>'));
			}
			$masterpoint_tableName = $table_prefix . 'tournament_masterpoint';
			$tournament_tableName = $table_prefix . 'tournament_master';
			$event_tableName = $table_prefix . 'event_master';
			$query = "SELECT " . $masterpoint_tableName . ".event_date as event_date," . $tournament_tableName . ".description as tournament_name," . $event_tableName . ".description as event_name,";
			$query .= $masterpoint_tableName . ".localpoints_earned as local," . $masterpoint_tableName . ".fedpoints_earned as fed";
			$query .= " FROM " . $masterpoint_tableName;
			$query .= " JOIN " . $tournament_tableName . " ON " . $masterpoint_tableName . ".tournament_code=" . $tournament_tableName . ".tournament_code";
			$query .= " JOIN " . $event_tableName . " ON " . $masterpoint_tableName . ".event_code=" . $event_tableName . ".event_code";
			$query .= " WHERE member_id=%s ORDER BY event_date DESC LIMIT 0,5";
			$rows = $mydb -> get_results($mydb -> prepare($query, $member_id));
			$numItems = count($rows);
			if ($numItems < 1) {
				$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-recent', 'id' => 'masterpoint-recent-list-item1', 'title' => '<div class="ab-item ab-empty-item">No Masterpoint Entries Found!</div>'));
			} else {
				$my_recent_masterpoint_html .= '<div class="set-width">';
				foreach ($rows as $index => $row) {
					$points = $row -> local;
					if ($points <= 0)
						$points = $row -> fed;
					$wp_admin_bar -> add_menu(array('parent' => 'my-masterpoint-recent', 'id' => 'masterpoint-recent-list-item' . $index, 'title' => '<span class="ico profile-icon">' . $row -> event_name . ' - ' . $points . '</span>'));
				}
				$my_recent_masterpoint_html .= '</div>';
			}
		}

		public function customize_admin_bar($wp_admin_bar) {
			if (is_user_logged_in() && current_user_can('subscriber')) {
				$this -> add_masterpoint_admin_bar($wp_admin_bar);
			}
		}

		function deactivate() {
			global $wp_roles;
			$wp_roles -> remove_cap('administrator', 'manage_masterpoints');
			$wp_roles -> remove_cap('editor', 'manage_masterpoints');
			$this -> deleteFile('user-panel.php');
			$this -> deleteFile('profile-form.php');
			$this -> deleteFile('login-form.php');
		}

		function activate() {
			global $wp_roles;
			$wp_roles -> add_cap('administrator', 'manage_masterpoints');
			$wp_roles -> add_cap('editor', 'manage_masterpoints');
			$this -> copyFile('user-panel.php');
			$this -> copyFile('profile-form.php');
			$this -> copyFile('login-form.php');
		}

		function deleteFile($fileName) {
			$destination = get_template_directory() . DIRECTORY_SEPARATOR . $fileName;
			unlink($destination);
		}

		function copyFile($fileName) {
			$source = rtrim(plugin_dir_path(__FILE__), '/') . DIRECTORY_SEPARATOR . 'templates' . DIRECTORY_SEPARATOR . $fileName;
			$destination = get_template_directory() . DIRECTORY_SEPARATOR . $fileName;
			copy($source, $destination); ;
		}

	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_masterpoint_display'] = new BFI_Masterpoint_Display();
}
