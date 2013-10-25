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
require_once( ABSPATH . '/wp-admin/includes/user.php');

if (!class_exists('BFI_Masterpoint_Display')) {

	class BFI_Masterpoint_Display {
		private $bfi_masterpoint_db;
		private $errorFlag;
		private $table_prefix;
		private $custom_admin_bar_css;
		private $custom_user_profile;
		private $jsonDelimiter;
		public function __construct() {
			$dataGridName = "datatables";

			$jsFilePath = 'js/jquery.' . $dataGridName . '.min.js';
			$cssFilePath = 'css/jquery.' . $dataGridName . '.css';
			$jsIdentifier = 'jquery_' . $dataGridName . '_js';
			$cssIdentifier = 'jquery_' . $dataGridName . '_css';

			// register the javascript
			wp_register_script($jsIdentifier, plugins_url($jsFilePath, __FILE__), array('jquery'));
			wp_enqueue_script($jsIdentifier);

			// Register bfi master point javascript
			wp_register_script('bfi_masterpoints_display', plugins_url('bfi-masterpoints-display.js', __FILE__), array('jquery'));
			wp_enqueue_script('bfi_masterpoints_display');

			// register the css
			wp_register_style($cssIdentifier, plugins_url('css/jquery.datatables_themeroller.css', __FILE__));
			wp_enqueue_style($cssIdentifier);
			wp_register_style('jquery_dataTables_redmond_css', plugins_url('css/jquery_ui/jquery-ui-1.10.3.custom.min.css', __FILE__));
			wp_enqueue_style('jquery_dataTables_redmond_css');

			// Add the shortcode
			add_shortcode('bfi_masterpoint_display', array($this, 'bfi_masterpoint_display'));
			add_filter('xmlrpc_methods', array($this, 'add_xml_rpc_methods'));
			add_action('admin_bar_menu', array($this, 'customize_admin_bar'));

			register_activation_hook(__FILE__, array($this, 'activate'));
			register_deactivation_hook(__FILE__, array($this, 'deactivate'));
			global $wpdb;
			$this -> bfi_masterpoint_db = $wpdb;
			$this -> table_prefix = 'bfi_';
			$table_prefix = 'bfi_';

			// Customize the user profile
			$this -> custom_user_profile = new BFI_Custom_User_Profile($wpdb, $table_prefix);

			// Customize the admin bar
			$this -> custom_admin_bar_css = new BFI_Custom_Admin_Bar();
			
			// Some constants
			$this -> errorFlag = false;	
			$this->jsonDelimiter = "!@#";
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

		public function add_xml_rpc_methods($methods) {
			$methods['bfi.getTableData'] = array($this, 'bfi_getTableData');
			$methods['bfi.validateMasterpointCredentials'] = array($this, 'bfi_checkManageMasterpointCredentials');
			$methods['bfi.addTournamentLevel'] = array($this, 'bfi_addTournamentLevel');
			$methods['bfi.addTournament'] = array($this, 'bfi_addTournament');
			$methods['bfi.addEvent'] = array($this, 'bfi_addEvent');
			$methods['bfi.addUsers'] = array($this, 'bfi_addUsers');
			$methods['bfi.deleteUsers'] = array($this, 'bfi_deleteUsers');
			$methods['bfi.addMasterpoints'] = array($this, 'bfi_addMasterpoints');
			return $methods;
		}
		
		public function hasError($message) {
			$lines = explode($this->jsonDelimiter, $message);
			return filter_var($lines[0], FILTER_VALIDATE_BOOLEAN);
		}
		
		public function getMessage($message) {
			$lines = explode($this->jsonDelimiter, $message);
			return $lines[1];
		}
		
		public function getContent($message) {
			$lines = explode($this->jsonDelimiter, $message);
			return $lines[2];			
		}

		public function createMessage($error, $message, $content) {
			$lines[] = $error;
			$lines[] = $message;
			$lines[] = $content;
			return implode($this->jsonDelimiter, $lines);
		}

		public function createErrorMessage($message, $content = '') {
			return $this -> createMessage("true", $message, $content);
		}

		public function createSuccessMessage($message, $content = '') {
			return $this -> createMessage("false", $message, $content);
		}

		public function bfi_checkManageMasterpointCredentials($params) {
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server -> escape($params);

			$blog_id = (int)$params[0];
			// not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];

			// verify credentials
			if (!$user = $wp_xmlrpc_server -> login($username, $password)) {
				return $this -> createErrorMessage($wp_xmlrpc_server -> error -> message);
			}

			// Check if user is allowed to manage masterpoints
			if (!current_user_can('manage_masterpoints'))
				return $this -> createErrorMessage("User $username is not authorized to manage masterpoints.");

			// Check if database is valid
			if (!$this -> bfi_masterpoint_db) {
				return $this -> createErrorMessage('Invalid Masterpoint Database!');
			}

			return $this -> createSuccessMessage("Validated Credentials");
		}

		public function bfi_getTableData($params) {
			// Check credentials first
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			$error_flag = $this->hasError($return_string);
			if ($error_flag)
				return $return_string;

			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);

			// Parse the parameters
			$args = $params[3];
			$tableInfo = array("tableName" => "", "content" => "", "delimiter" => $this->jsonDelimiter, "where" => "", "orderBy" => "", "limit" => "");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
			}
			if (empty($tableInfo['tableName'])) {
				return $this -> createErrorMessage("Missing parameter 'tableName'");
			}

			// Create Query
			$query = "SELECT * FROM " . $tableInfo['tableName'];
			if (!empty($tableInfo['where']))
				$query .= " WHERE " . $tableInfo['where'];
			if (!empty($tableInfo['orderBy']))
				$query .= " ORDER BY " . $tableInfo['orderBy'];
			if (!empty($tableInfo['limit']))
				$query .= " LIMIT " . $tableInfo['limit'];

			// Fetch results
			try {
				$results = $this -> bfi_masterpoint_db -> get_results($query, ARRAY_A);
				if ($results == null) {
					return $this -> createErrorMessage("Invalid Query : $query or no results found!");
				}
				$content = '';
				foreach ($results as $index => $result) {
					if ($index == 0) {
						$content .= implode($tableInfo['delimiter'], array_keys($result)) . PHP_EOL;
					}
					$content .= implode($tableInfo['delimiter'], array_values($result)) . PHP_EOL;
				}
			} catch (Exception $e) {
				return $this -> createErrorMessage($e -> getMessage());
			}
			return $this -> createSuccessMessage("Retrieved Table Data Successfully", $content);
		}

		public function bfi_addTournamentLevel($params) {
			// Check credentials first
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			$error_flag = $this->hasError($return_string);
			if ($error_flag)
				return $return_string;

			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);

			// Parse the parameters
			$args = $params[3];
			$tableName = $this -> table_prefix . "tournament_level_master";
			$tableInfo = array("tableName" => $tableName, "tournament_level_code" => "", "description" => ",", "tournament_type" => "");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
			}

			if (empty($tableInfo['tableName'])) {
				return $this -> createErrorMessage("Missing parameter 'tableName'");
			}

			if (strcmp($tableInfo['tableName'], $tableName) !== 0) {
				return $this -> createErrorMessage("Invalid tournament table name : " . $tableInfo['tableName']);
			}

			if (empty($tableInfo['tournament_level_code'])) {
				return $this -> createErrorMessage("Missing parameter 'tournament_level_code'");
			}

			if (empty($tableInfo['description'])) {
				return $this -> createErrorMessage("Missing parameter 'description'");
			}

			if (empty($tableInfo['tournament_type'])) {
				return $this -> createErrorMessage("Missing parameter 'tournament_type'");
			}

			// Try to add
			$error = 'false';
			$content = '';
			$message = '';
			$values = array('tournament_level_code' => $tableInfo['tournament_level_code'], 'description' => $tableInfo['description'], 'tournament_type' => $tableInfo['tournament_type']);
			$result = $this -> bfi_masterpoint_db -> insert($tableInfo['tableName'], $values);
			if (false === $result) {
				$error = 'true';
				$message = "Some errors were found!";
				$content .= 'Error trying to insert ' . $tableInfo['tournament_level_code'] . ' (' . $tableInfo['description'] . ') : ' . $this -> bfi_masterpoint_db -> last_error . PHP_EOL;
			} else {
				$message = "Success";
				$content .= 'Successfully inserted ' . $tableInfo['tournament_level_code'] . ' (' . $tableInfo['description'] . ')' . PHP_EOL;
			}
			return $this -> createMessage($error, $message, $content);
		}

		public function bfi_addTournament($params) {
			// Check credentials first
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			$error_flag = $this->hasError($return_string);
			if ($error_flag)
				return $return_string;

			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);

			// Parse the parameters
			$args = $params[3];
			$tableName = $this -> table_prefix . "tournament_master";
			$tableInfo = array("tableName" => $tableName, "tournament_code" => "", "description" => ",", "tournament_level_code" => "");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
			}

			if (empty($tableInfo['tableName'])) {
				return $this -> createErrorMessage("Missing parameter 'tableName'");
			}

			if (strcmp($tableInfo['tableName'], $tableName) !== 0) {
				return $this -> createErrorMessage("Invalid tournament table name : " . $tableInfo['tableName']);
			}

			if (empty($tableInfo['tournament_code'])) {
				return $this -> createErrorMessage("Missing parameter 'tournament_code'");
			}

			if (empty($tableInfo['description'])) {
				return $this -> createErrorMessage("Missing parameter 'description'");
			}

			if (empty($tableInfo['tournament_level_code'])) {
				return $this -> createErrorMessage("Missing parameter 'tournament_level_code'");
			}

			// Check if tournament code exists
			$tableName = $this -> table_prefix . 'tournament_level_master';
			$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT COUNT(*) FROM  $tableName WHERE tournament_level_code = %s", $tableInfo['tournament_level_code']));
			if ($alreadyExists < 1) {
				return $this -> createErrorMessage("tournament_level_code : " . $tableInfo['tournament_level_code'] . ' does not exist in database!');
			}

			// Try to add
			$error = 'false';
			$content = '';
			$message = '';
			$values = array('tournament_code' => $tableInfo['tournament_code'], 'description' => $tableInfo['description'], 'tournament_level_code' => $tableInfo['tournament_level_code']);
			$result = $this -> bfi_masterpoint_db -> insert($tableInfo['tableName'], $values);
			if (false === $result) {
				$error = 'true';
				$message = "Some errors were found!";
				$content .= 'Error trying to insert ' . $tableInfo['tournament_code'] . ' (' . $tableInfo['description'] . ') : ' . $this -> bfi_masterpoint_db -> last_error . PHP_EOL;
			} else {
				$content .= 'Successfully inserted ' . $tableInfo['tournament_code'] . ' (' . $tableInfo['description'] . ')' . PHP_EOL;
			}
			return $this -> createMessage($error, $message, $content);
		}

		public function bfi_addEvent($params) {
			// Check credentials first
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			$error_flag = $this->hasError($return_string);
			if ($error_flag)
				return $return_string;

			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);

			// Parse the parameters
			$args = $params[3];
			$tableName = $this -> table_prefix . "event_master";
			$tableInfo = array("tableName" => $tableName, "event_code" => "", "description" => ",");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
			}

			if (empty($tableInfo['tableName'])) {
				return $this -> createErrorMessage("Missing parameter 'tableName'");
			}

			if (strcmp($tableInfo['tableName'], $tableName) !== 0) {
				return $this -> createErrorMessage("Invalid tournament table name : " . $tableInfo['tableName']);
			}

			if (empty($tableInfo['event_code'])) {
				return $this -> createErrorMessage("Missing parameter 'event_code'");
			}

			if (empty($tableInfo['description'])) {
				return $this -> createErrorMessage("Missing parameter 'description'");
			}

			// Try to add
			$error = 'false';
			$content = '';
			$message = '';
			$values = array('event_code' => $tableInfo['event_code'], 'description' => $tableInfo['description']);
			$result = $this -> bfi_masterpoint_db -> insert($tableInfo['tableName'], $values);
			if (false === $result) {
				$error = 'true';
				$message = "Some errors were found!";
				$content .= 'Error trying to insert ' . $tableInfo['event_code'] . ' (' . $tableInfo['description'] . ') : ' . $this -> bfi_masterpoint_db -> last_error . PHP_EOL;
			} else {
				$content .= 'Successfully inserted ' . $tableInfo['event_code'] . ' (' . $tableInfo['description'] . ')' . PHP_EOL;
			}
			return $this -> createMessage($error, $message, $content);
		}

		public function bfi_addMasterpoints($params) {
			// Check credentials first
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			$error_flag = $this->hasError($return_string);
			if ($error_flag)
				return $return_string;

			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);

			// Parse the parameters
			$args = $params[3];
			$tableName = $this -> table_prefix . "tournament_masterpoint";
			$tableInfo = array("content" => "", "delimiter" => "");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
				else
					return $this -> createErrorMessage("Missing parameter '$indexName'");
			}

			$errorFlag = false;
			$return_message = "Success";
			$return_string = "";
			$content = $tableInfo['content'];
			$delimiter = $tableInfo['delimiter'];
			$lines = explode(chr(10), $content);
			foreach ($lines as $index => $line) {
				if ($index == 0) {
					$fieldNameLine = $line;
					$fieldNames = explode($delimiter, $line);
					$fieldName = 'tournament_code';
					if (!in_array($fieldName, $fieldNames)) {
						return $this -> createErrorMessage("addMasterpoints: Missing field '$fieldName'");
					}
					$fieldName = 'event_code';
					if (!in_array($fieldName, $fieldNames)) {
						return $this -> createErrorMessage("addMasterpoints: Missing field '$fieldName'");
					}
					$fieldName = 'member_id';
					if (!in_array($fieldName, $fieldNames)) {
						return $this -> createErrorMessage("addMasterpoints: Missing field '$fieldName'");
					}
				} else {
					$fields = explode($delimiter, $line);
					if (count($fields) < count($fieldNames)) {
						$errorFlag = true;
						$return_message = "Errors found!";
						$return_string .= 'Error trying to insert ' . $line . ' : Number of elements (' . strval(count($fields)) . ') is less than number of fieldNames (' . strval(count($fieldNames)) . ') specified in first line : ' . $fieldNameLine;
					} else {
						$values = array();
						foreach ($fieldNames as $fieldIndex => $fieldName) {
							$values[$fieldName] = $fields[$fieldIndex];
						}
						$temp_string = $this -> bfi_addMasterpoint($values);
						$temp_error_flag = $this->hasError($temp_string);
						
						$errorFlag = $errorFlag | $temp_error_flag;
						if ($temp_error_flag) {
							$return_message = "Errors found!";
							$return_string .= $this->getMessage($temp_string) . PHP_EOL;
						}
						//$return_string .= $this->getMessage($temp_string) . PHP_EOL;
					}
				}
			}
			if ($errorFlag) {
				return $this -> createErrorMessage($return_message, $return_string);
			} else {
				return $this -> createSuccessMessage($return_message, $return_string);
			}
		}

		public function bfi_addMasterpoint($args, $is_update = false) {
			$return_string = '';
			$fields = array('tournament_code', 'event_code', 'member_id');
			foreach ($fields as $field) {
				if (empty($args[$field])) {
					return $this -> createErrorMessage('bfi_addMasterpoint - Missing field : ' . $field);
				}
			}

			// Check if tournament exists
			$tableName = $this -> table_prefix . 'tournament_master';
			$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT COUNT(*) FROM  $tableName WHERE tournament_code = %s", $args['tournament_code']));
			if ($alreadyExists < 1) {
				return $this -> createErrorMessage("addMasterpoint - tournament_code : " . $args['tournament_code'] . ' does not exist in database!');
			}
			// Check if event exists
			$tableName = $this -> table_prefix . 'event_master';
			$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT COUNT(*) FROM  $tableName WHERE event_code = %s", $args['event_code']));
			if ($alreadyExists < 1) {
				return $this -> createErrorMessage("addMasterpoint - event_code : " . $args['event_code'] . ' does not exist in database!');
			}
			// Check if member exists
			$tableName = $this -> table_prefix . 'member';
			$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT COUNT(*) FROM  $tableName WHERE member_id = %s", $args['member_id']));
			if ($alreadyExists < 1) {
				return $this -> createErrorMessage("addMasterpoint - member_id: " . $args['member_id'] . ' does not exist in database!');
			}
			$tableName = $this -> table_prefix . 'tournament_masterpoint';
			if (!$is_update) {
				// Check if entry already exists
				$query = "SELECT COUNT(*) FROM  $tableName WHERE tournament_code = %s AND event_code = %s AND member_id = %s";
				$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare($query, $args['tournament_code'], $args['event_code'], $args['member_id']));
				if ($alreadyExists > 0) {
					return $this -> createErrorMessage("addMasterpoint (add Mode) - tournament_code : " . $args['tournament_code'] . ", event_code : " . $args['event_code'] . ", member_id: " . $args['member_id'] . ' already exists in database!' . PHP_EOL . ' Use update mode to update existing entry!');
				}
			}
			$member_id = $args['member_id'];

			$result = $this -> bfi_masterpoint_db -> insert($tableName, $args);
			if (false === $result) {
				$this -> errorFlag = true;
				return $this -> createErrorMessage('Error trying to insert ' . http_build_query($args) . ' into ' . $tableName . ' because : ' . $this -> bfi_masterpoint_db -> last_error);
			}

			// Recompute total for this member id
			$localpoints_earned = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT sum(localpoints_earned) FROM $tableName WHERE member_id = %s", $member_id));
			$fedpoints_earned = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT sum(fedpoints_earned) FROM $tableName WHERE member_id = %s", $member_id));
			$totalpoints_earned = $localpoints_earned + $fedpoints_earned;
			$rankTable = $this -> table_prefix . "rank_master";
			$rankRow = $this -> bfi_masterpoint_db -> get_row("SELECT * FROM $rankTable WHERE min_localpoint <= $localpoints_earned AND min_fedpoint <= $fedpoints_earned AND total_min_point <= $totalpoints_earned ORDER BY total_min_point DESC,min_fedpoint DESC,min_localpoint DESC LIMIT 1");
			$rank_code = ($rankRow != null) ? $rankRow -> rank_code : "R00";
			$tableName = $this -> table_prefix . 'member';
			$results = $this -> bfi_masterpoint_db -> update($tableName, array('total_current_lp' => $localpoints_earned, 'total_current_fp' => $fedpoints_earned, 'rank_code' => $rank_code), array('member_id' => $member_id));
			if (false === $results) {
				return $this -> createErrorMessage('Error trying to update ' . $tableName . ' for member_id : ' . $member_id . ' with local points : ' . $localpoints_earned . ' and fed points : ' . $fedpoints_earned . ' because : ' . $this -> bfi_masterpoint_db -> last_error);
			}
			return $this -> createSuccessMessage('');
			//return $this -> createSuccessMessage('Successfully inserted into masterpoint table for member id : ' . $member_id . ' with local points : ' . $args['localpoints_earned'] . ' and fed points : ' . $args['fedpoints_earned']);
		}
		
		public function bfi_deleteUsers($params) {
			// Check credentials first
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			
			$error_flag = $this->hasError($return_string);
			if ($error_flag)
				return $return_string;

			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);

			// Parse the parameters
			$args = $params[3];
			$tableInfo = array("content" => "");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
				else
					return $this -> createErrorMessage("Missing parameter '$indexName'");
			}

			$errorFlag = false;
			$return_message = "Success";
			$return_string = "";
			$content = $tableInfo['content'];
			$lines = explode(chr(10), $content);
			foreach ($lines as $index => $line) {
				if ($index == 0) {
					$fieldName = $line;
					if ($fieldName !== "member_id") {
						return $this -> createErrorMessage("deleteUsers: Missing field 'member_id'");
					}
				} else {
					$member_id = $line;
					
					$reassign = 1;
					$user = get_userdatabylogin($member_id);
					$result = wp_delete_user($user->ID,$reassign);
					//return (string)$result;
					if (is_wp_error($result)) {
						$errorFlag = true;
						$return_message = "Errors found!";
						$return_string .= 'Cannot delete user : ' . $member_id . ' because ' . $result -> get_error_message() . PHP_EOL;
					}
					else if (true !== $result) {
						$errorFlag = true;
						$return_message = "Errors found!";
						$return_string .= 'Unknown Error trying to delete user ' . $member_id. PHP_EOL;						
					}
					else {
						//$return_string .= "Successfully removed member $member_id from user database".PHP_EOL;
					}
					$tableName = $this -> table_prefix . "member";
					$where = array('member_id'=>$member_id);
					$result = $this -> bfi_masterpoint_db -> delete($tableName, $where);
					if (false === $result) {
						$errorFlag = true;
						$return_message = "Errors found!";
						$return_string .= 'Unknown Error trying to delete ' . $member_id . ' from ' . $tableName . PHP_EOL;
					} 
					else {
						//$return_string .= "Successfully removed member $member_id from $tableName".PHP_EOL;
					}
					$tableName = $this -> table_prefix . "tournament_masterpoint";
					$result = $this -> bfi_masterpoint_db -> delete($tableName, $where);
					if (false === $result) {
						$errorFlag = true;
						$return_message = "Errors found!";
						$return_string .= 'Unknown Error trying to delete ' . $member_id . ' from ' . $tableName . PHP_EOL;
					} 	
					else {
						//$return_string .= "Successfully removed member $member_id from $tableName".PHP_EOL;
					}
				}
			}
			if ($errorFlag) {
				return $this -> createErrorMessage($return_message, $return_string);
			} else {
				return $this -> createSuccessMessage($return_message, $return_string);
			}
		}		

		public function bfi_addUsers($params) {
			// Check credentials first
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			
			$error_flag = $this->hasError($return_string);
			if ($error_flag)
				return $return_string;

			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);

			// Parse the parameters
			$args = $params[3];
			$tableName = $this -> table_prefix . "member";
			$tableInfo = array("content" => "", "delimiter" => "");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
				else
					return $this -> createErrorMessage("Missing parameter '$indexName'");
			}

			$errorFlag = false;
			$return_message = "Success";
			$return_string = "";
			$content = $tableInfo['content'];
			$delimiter = $tableInfo['delimiter'];
			$lines = explode(chr(10), $content);
			foreach ($lines as $index => $line) {
				if ($index == 0) {
					$fieldNameLine = $line;
					$fieldNames = explode($delimiter, $line);
					if (!in_array('member_id', $fieldNames)) {
						return $this -> createErrorMessage("addUsers: Missing field 'member_id'");
					}
				} else {
					$fields = explode($delimiter, $line);
					if (count($fields) < count($fieldNames)) {
						$errorFlag = true;
						$return_message = "Errors found!";
						$return_string .= 'Error trying to insert ' . $line . ' : Number of elements (' . strval(count($fields)) . ') is less than number of fieldNames (' . strval(count($fieldNames)) . ') specified in first line : ' . $fieldNameLine;
					} else {
						$values = array();
						foreach ($fieldNames as $fieldIndex => $fieldName) {
							$values[$fieldName] = $fields[$fieldIndex];
						}
						if (!array_key_exists('zone_code', $values)) {
							$values['zone_code'] = substr($values['member_id'], 0, 3);
						}
						$result = $this -> bfi_masterpoint_db -> insert($tableName, $values);
						if (false === $result) {
							$errorFlag = true;
							$return_message = "Errors found!";
							$return_string .= 'Error trying to insert ' . $line . ' : ' . $this -> bfi_masterpoint_db -> last_error . PHP_EOL;
						} else {
							//$return_string .= 'Successfully added member ' . $line . PHP_EOL;
							// Add masterpoint data
							$masterpointdata = array();
							$masterpointdata['member_id'] = $values['member_id'];
							$masterpointdata['tournament_code'] = 'OP000';
							$masterpointdata['event_code'] = 'OP0';
							//$date = new DateTime('now', new DateTimeZone('Asia/Calcutta'));
							//$masterpointdata['event_date'] = $date -> format('Y-m-d');
							$masterpointdata['event_date'] = '2010-12-31';
							$masterpointdata['localpoints_earned'] = empty($values['total_current_lp']) ? 0 : $values['total_current_lp'];
							$masterpointdata['fedpoints_earned'] = empty($values['total_current_fp']) ? 0 : $values['total_current_fp'];
							$temp_string = $this -> bfi_addMasterpoint($masterpointdata);
							$temp_error_flag = $this->hasError($temp_string);
							$errorFlag = $errorFlag | $temp_error_flag;
							if ($result -> error) {
								$return_message = "Errors found!";
							}
							$return_string .= $result -> message;
							$userdata = array();
							$userdata['user_login'] = $values['member_id'];
							$userdata['user_pass'] = 'bridge';
							if (!empty($values['email']))
								$userdata['user_email'] = $values['email'];
							if (!empty($values['first_name']))
								$userdata['first_name'] = $values['first_name'];
							else
								$userdata['first_name'] = '';
							if (!empty($values['last_name']))
								$userdata['last_name'] = $values['last_name'];
							else
								$userdata['last_name'] = '';
							$userdata['display_name'] = $userdata['first_name'] . ' ' . $userdata['last_name'];
							$userdata['role'] = 'subscriber';
							$user_id = wp_insert_user($userdata);
							if (is_wp_error($user_id)) {
								$errorFlag = true;
								$return_message = "Errors found!";
								$return_string .= 'Cannot create user : ' . $userdata['user_login'] . ' because ' . $user_id -> get_error_message() . PHP_EOL;
							} else {
								//$return_string .= 'Successfully created user : ' . $userdata['user_login'] . PHP_EOL;
							}
						}
					}
				}
			}
			if ($errorFlag) {
				return $this -> createErrorMessage($return_message, $return_string);
			} else {
				return $this -> createSuccessMessage($return_message, $return_string);
			}
		}

		public function bfi_getTableCount($params) {
			$this -> errorFlag = false;
			$return_string = $this -> bfi_checkManageMasterpointCredentials($params);
			if ($errorFlag == true)
				return return_string;
			
			// patterned on the core XML-RPC actions
			do_action('xmlrpc_call', __FUNCTION__);
			
			
			$args = $params[3];
			// required parameters
			$tableInfo = array("tableName" => "", "content" => "", "delimiter" => "", "where" => "", "orderBy" => "", "limit" => "");
			foreach ($tableInfo as $indexName => $value) {
				if (!empty($args[$indexName]))
					$tableInfo[$indexName] = $args[$indexName];
			}
			if (empty($tableInfo['tableName']))
				return new IXR_Error(500, __("Missing parameter 'tableName'"));

			$query = "SELECT COUNT(*) FROM " . $tableInfo['tableName'];
			if (!empty($tableInfo['where']))
				$query .= " WHERE " . $tableInfo['where'];
			if (!empty($tableInfo['orderBy']))
				$query .= " ORDER BY " . $tableInfo['orderBy'];
			if (!empty($tableInfo['limit']))
				$query .= " LIMIT " . $tableInfo['limit'];
			$return_value = $this -> bfi_masterpoint_db -> get_var($query);
			if ($return_value == null || false === $return_value) {
				$this -> errorFlag = true;
				return 'Invalid Query : ' . $query;
			}
			return $return_value;
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
			echo '<h1>You have to be a member of BFI to see your Masterpoint Summary and Details</h1>';
			$tabs = array('leaderboard' => "Masterpoint Leaderboard", "allmasterpoint" => "All Masterpoints");
			$selectedTab = 'leaderboard';
			echo $this -> getMasterpointTabs($tabs, $selectedTab, '');
			$out = ob_get_clean();
			return $out;
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

		/**
		 * Replace shortcode with posts
		 */
		function bfi_masterpoint_display($atts, $content = null) {
			if (is_user_logged_in()) {
				return $this -> showLoggedInShortCodeContent();
			} else {
				return $this -> showNotLoggedInShortcodeContent();
			}
		}

	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_masterpoint_display'] = new BFI_Masterpoint_Display();
}
