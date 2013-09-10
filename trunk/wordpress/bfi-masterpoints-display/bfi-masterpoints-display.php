<?php
/*
 Plugin Name: BFI Masterpoint Display
Plugin URI: http://bfi.net.in
Description: A plugin to manage all masterpoint capabilities for BFI site
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
License: GNU General Public License v3.0
License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'BFI_Masterpoint_Display' ) ) {

	class BFI_Masterpoint_Display {
		private $bfi_masterpoint_db;
		private $fieldNames;
		private $errorFlag;
		public function __construct() {
			$dataGridName = "datatables";

			$jsFilePath = 'js/jquery.'.$dataGridName.'.min.js';
			$cssFilePath = 'css/jquery.'.$dataGridName.'.css';
			$jsIdentifier = 'jquery_'.$dataGridName.'_js';
			$cssIdentifier = 'jquery_'.$dataGridName.'_css';

			// register the javascript
			wp_register_script( $jsIdentifier, plugins_url($jsFilePath, __FILE__ ), array( 'jquery' ) );
			wp_enqueue_script($jsIdentifier);

			// Register bfi master point javascript
			wp_register_script( 'bfi_masterpoints_display', plugins_url( 'bfi-masterpoints-display.js', __FILE__ ), array( 'jquery' ) );
			wp_enqueue_script('bfi_masterpoints_display');

			// register the css
			wp_register_style($cssIdentifier, plugins_url( 'css/jquery.datatables_themeroller.css', __FILE__ ) );
			wp_enqueue_style($cssIdentifier);
			wp_register_style( 'jquery_dataTables_redmond_css', plugins_url( 'css/jquery_ui/jquery-ui-1.10.3.custom.min.css', __FILE__ ) );
			wp_enqueue_style('jquery_dataTables_redmond_css');

			// Add the shortcode
			add_shortcode('bfi_masterpoint_display', array($this, 'bfi_masterpoint_display'));
			add_action( 'show_user_profile', array($this, 'bfi_add_custom_user_profile_fields') );
			add_action( 'edit_user_profile', array($this, 'bfi_add_custom_user_profile_fields') );
			add_action( 'personal_options_update', array($this, 'bfi_save_custom_user_profile_fields') );
			add_action( 'edit_user_profile_update', array($this, 'bfi_save_custom_user_profile_fields') );
			add_filter('user_contactmethods',array($this, 'remove_contactmethods'),10,1);
			add_filter( 'xmlrpc_methods', array( $this, 'add_xml_rpc_methods' ) );
			register_activation_hook( __FILE__, array( $this, 'activate' ) );
			register_deactivation_hook(__FILE__, array( $this, 'deactivate' ));
			global $wpdb;
			$this->bfi_masterpoint_db = $wpdb;
			$this->fieldNames = array('address_1','address_2','address_3','city','country','residence_phone','mobile_no','sex','dob');
			$this->errorFlag = false;
		}
			
		public function add_xml_rpc_methods( $methods ) {
			//$methods['bfi.getTableCount'] = array($this,'bfi_getTableCount');
			$methods['bfi.getTableData'] = array($this,'bfi_getTableData');
			$methods['bfi.addTableData'] = array($this,'bfi_addTableData');
			$methods['bfi.validateMasterpointCredentials'] = array($this,'bfi_checkManageMasterpointCredentials');
			$methods['bfi.addUsers'] = array($this,'bfi_addUsers');
			$methods['bfi.addMasterpoints'] = array($this,'bfi_addMasterpoints');
			return $methods;
		}
		
		public function createMessage($error,$message,$content) {
			$return_value = array("error"=>$error,"message"=>$message,"content"=>$content);
			return json_encode($return_value);			
		}
		
		public function createErrorMessage($message, $content='') {
			return $this->createMessage("true",$message,$content);
			/*$return_value = array("error"=>"true","message"=>$message,"content"=>$content);
			return json_encode($return_value);*/
		}
		
		public function createSuccessMessage($message,$content='') {
			return $this->createMessage("false",$message,$content);
			/*$return_value = array("error"=>"false","message"=>$message,"content"=>$content);
			return json_encode($return_value);*/
		}	
		
		public function bfi_checkManageMasterpointCredentials($params) {
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $params );
		
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];
		
			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $this->createErrorMessage($wp_xmlrpc_server->error->message);
			}
		
			// Check if user is allowed to manage masterpoints
			if ( ! current_user_can( 'manage_masterpoints' ) )
				return $this->createErrorMessage("User $username is not authorized to manage masterpoints.");
		
			// Check if database is valid
			if (!$this->bfi_masterpoint_db) {
				return $this->createErrorMessage('Invalid Masterpoint Database!');
			}
			
			return $this->createSuccessMessage("Validated Credentials");
		}		
		
		public function bfi_getTableData($params) {
			// Check credentials first		
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
			$result = json_decode($return_string,true);
			if ($result->error) return $return_string;
			
			do_action( 'xmlrpc_call', __FUNCTION__); // patterned on the core XML-RPC actions
			
			// Parse the parameters
			$args     = $params[3];
			$tableInfo = array("tableName"=>"","content"=>"","delimiter"=>",","where"=>"","orderBy"=>"","limit"=>"");
			foreach($tableInfo as $indexName=>$value) {
				if (!empty($args[$indexName])) $tableInfo[$indexName] = $args[$indexName];
			}
			if ( empty( $tableInfo['tableName'] ) ) {
				return $this->createErrorMessage("Missing parameter 'tableName'");
			}

			// Create Query
			$query = "SELECT * FROM ".$tableInfo['tableName'];
			if (!empty($tableInfo['where'])) $query .= " WHERE ".$tableInfo['where'];
			if (!empty($tableInfo['orderBy'])) $query .= " ORDER BY ".$tableInfo['orderBy'];
			if (!empty($tableInfo['limit'])) $query .= " LIMIT ".$tableInfo['limit'];
			
			// Fetch results
			try {
				$results = $this->bfi_masterpoint_db->get_results($query,ARRAY_A);
				if ($results == null) {
					return $this->createErrorMessage("Invalid Query : $query or no results found!");
				}
				$content = '';
				foreach($results as $index=>$result) {
					if ($index == 0) {
						$content .= implode($tableInfo['delimiter'],array_keys($result)).PHP_EOL;
					}
					$content .= implode($tableInfo['delimiter'],array_values($result)).PHP_EOL;
				}
			}
			catch (Exception $e) {
				return $this->createErrorMessage($e->getMessage());
			}
			return $this->createSuccessMessage("Retrieved Table Data Successfully",$content);
		}		
		
		public function bfi_addTableData($params) {
			
			// Check credentials first
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
			$result = json_decode($return_string,true);
			if ($result->error) return $return_string;
				
			do_action( 'xmlrpc_call', __FUNCTION__); // patterned on the core XML-RPC actions
				
			// Parse the parameters
			$args     = $params[3];
			$tableInfo = array("tableName"=>"","content"=>"","delimiter"=>",","where"=>"","orderBy"=>"","limit"=>"");
			foreach($tableInfo as $indexName=>$value) {
				if (!empty($args[$indexName])) $tableInfo[$indexName] = $args[$indexName];
			}
			if ( empty( $tableInfo['tableName'] ) ) {
				return $this->createErrorMessage("Missing parameter 'tableName'");
			}
			
			if ( empty( $tableInfo['content'] ) ) {
				return $this->createErrorMessage("Missing parameter 'content'");
			}			
				
			/*$tableName = $args['tableName'];
			$content = $args['content'];
			$delimiter = ",";
			if (!empty($args['delimiter'])) $delimiter = $args['delimiter'];*/
			$lines = explode(chr(10),$tableInfo['content']);
			$error = 'false';
			$content = '';
			$message = '';
			foreach($lines as $index=> $line) {
				if ($index == 0) {
					$fieldNameLine = $line;
					$fieldNames = explode($tableInfo['delimiter'],$line);
				}
				else {
					$fields = explode($tableInfo['delimiter'],$line);
					if(count($fields) < count($fieldNames)) {
						$message = 'Error trying to insert '.$line.' : Number of elements ('.strval(count($fields)).') is less than number of fieldNames ('.strval(count($fieldNames)).') specified in first line : '.$fieldNameLine;
						$error = 'true';
					}
					else {
						$values = array();
						foreach($fieldNames as $fieldIndex=>$fieldName) {
							$values[$fieldName] = $fields[$fieldIndex];
						}
						$result = $this->bfi_masterpoint_db->insert($tableInfo['tableName'],$values);
						if (false === $result) {
							$error = 'true';
							$message = "Some errors were found!";
							$content .= 'Error trying to insert '.$line.' : '.$this->bfi_masterpoint_db->last_error.PHP_EOL;
						}
						else {
							$content .= 'Successfully inserted '.$line.PHP_EOL;
						}
					}
				}
			}
			return $this->createMessage($error,$message,$content);
		}		
		
		public function bfi_addMasterpoints($params) {
			$this->errorFlag = false;
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
			do_action( 'xmlrpc_call', __FUNCTION__); // patterned on the core XML-RPC actions
			if ($this->errorFlag == true) return return_string;
			$this->errorFlag = false;
			$args     = $params[3];
			// required parameters
			if ( empty( $args['tableName'] ) ) return new IXR_Error( 500, __( "Missing parameter 'tableName'" ) );
			if ( empty( $args['content'] ) ) return new IXR_Error( 500, __( "Missing parameter 'content'" ) );
			$tableName = $args['tableName'];
			if($tableName != 'bfi_tournament_masterpoint') return new IXR_Error( 500, __( "TableName (".$tableName.") has to be bfi_tournament_masterpoint to add users!" ) );
			$content = $args['content'];
			$delimiter = ",";
			if (!empty($args['delimiter'])) $delimiter = $args['delimiter'];
			$lines = explode(chr(10),$content);
			foreach($lines as $index=> $line) {
				if ($index == 0) {
					$fieldNameLine = $line;
					$fieldNames = explode($delimiter,$line);
				}
				else {
					$fields = explode($delimiter,$line);
					if(count($fields) < count($fieldNames)) {
						$return_string .= 'Error trying to insert '.$line.' : Number of elements ('.strval(count($fields)).') is less than number of fieldNames ('.strval(count($fieldNames)).') specified in first line : '.$fieldNameLine;
					}
					else {
						$values = array();
						foreach($fieldNames as $fieldIndex=>$fieldName) {
							$values[$fieldName] = $fields[$fieldIndex];
						}
						$return_string .= $this->bfi_addMasterpoint($values).PHP_EOL;
					}
				}
			}
			return $return_string;
		}
		
		public function bfi_addMasterpoint($args) {
			$this->errorFlag = false;
			$return_string = '';
			$fields = array('tournament_code','event_code','member_id');
			foreach($fields as $field) {
				if ( empty( $args[$field] ) ) {
					$this->errorFlag = true;
					return 'bfi_addMasterpoint - Missing field : '.$field;
				}
			}
			$member_id = $args['member_id'];
			$tableName = 'bfi_tournament_masterpoint';
			$result = $this->bfi_masterpoint_db->insert($tableName,$args);
			if (false === $result) {
				$this->errorFlag = true;
				return 'Error trying to insert '.http_build_query($args).' into '.$tableName.' because : '.$this->bfi_masterpoint_db->last_error;
			}	
			// REcompute total for this member id	
			
			$localpoints_earned = $this->bfi_masterpoint_db->get_var( $this->bfi_masterpoint_db->prepare("SELECT sum(localpoints_earned) FROM $tableName WHERE member_id = %s",$member_id) );
			$fedpoints_earned = $this->bfi_masterpoint_db->get_var( $this->bfi_masterpoint_db->prepare("SELECT sum(fedpoints_earned) FROM $tableName WHERE member_id = %s",$member_id) );
			$tableName = 'bfi_member';
			$results = $this->bfi_masterpoint_db->update(
					$tableName,
					array(
							'total_current_lp' => $localpoints_earned,
							'total_current_fp' => $fedpoints_earned	
					),
					array( 'member_id' => $member_id ),
					array(
							'%f',	// value1
							'%f'	// value2
					)
			);
			if (false === $results) {
				$this->errorFlag = true;
				return 'Error trying to update '.$tableName.' for member_id : '.$member_id.' with local points : '.$localpoints_earned.' and fed points : '.$fedpoints_earned.' because : '.$this->bfi_masterpoint_db->last_error;
			}
			return 'Successfully inserted into masterpoint table and updated '.$results.' rows in member table for member id : '.$member_id.' with local points : '.$localpoints_earned.' and fed points : '.$fedpoints_earned;
		}
		
		
		public function bfi_addUsers($params) {
			$this->errorFlag = false;
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
			do_action( 'xmlrpc_call', __FUNCTION__); // patterned on the core XML-RPC actions
			if ($this->errorFlag == true) return return_string;
			$this->errorFlag = false;
			$args     = $params[3];
			// required parameters
			if ( empty( $args['tableName'] ) ) return new IXR_Error( 500, __( "Missing parameter 'tableName'" ) );
			if ( empty( $args['content'] ) ) return new IXR_Error( 500, __( "Missing parameter 'content'" ) );
			$tableName = $args['tableName'];
			if($tableName != 'bfi_member') return new IXR_Error( 500, __( "TableName (".$tableName.") has to be bfi_member to add users!" ) );
			$content = $args['content'];
			$delimiter = ",";
			if (!empty($args['delimiter'])) $delimiter = $args['delimiter'];
			$lines = explode(chr(10),$content);
			foreach($lines as $index=> $line) {
				if ($index == 0) {
					$fieldNameLine = $line;
					$fieldNames = explode($delimiter,$line);
				}
				else {
					$fields = explode($delimiter,$line);
					if(count($fields) < count($fieldNames)) {
						$return_string .= 'Error trying to insert '.$line.' : Number of elements ('.strval(count($fields)).') is less than number of fieldNames ('.strval(count($fieldNames)).') specified in first line : '.$fieldNameLine;
					}
					else {
						$values = array();
						foreach($fieldNames as $fieldIndex=>$fieldName) {
							$values[$fieldName] = $fields[$fieldIndex];
						}
						$result = $this->bfi_masterpoint_db->insert($tableName,$values);
						if (false === $result) {
							$return_string .= 'Error trying to insert '.$line.' : '.$this->bfi_masterpoint_db->last_error.PHP_EOL;
						}
						else {
							$return_string .= 'Successfully added member '.$line.PHP_EOL;
							// Add masterpoint data
							$masterpointdata = array();
							$masterpointdata['member_id'] = $values['member_id'];
							$masterpointdata['tournament_code'] = 'OP000';
							$masterpointdata['event_code'] = 'OP0';
							$date = new DateTime('now', new DateTimeZone('Asia/Calcutta'));
							$masterpointdata['event_date'] = $date->format('Y-m-d');
							$masterpointdata['localpoints_earned'] = empty($values['total_current_lp'])?0:$values['total_current_lp'];						
							$masterpointdata['fedpoints_earned'] = empty($values['total_current_fp'])?0:$values['total_current_fp'];
							$return_string .= $this->bfi_addMasterpoint($masterpointdata);						
							$userdata = array();
							$userdata['user_login'] = $values['member_id'];
							$userdata['user_pass'] = 'bridge';
							if (!empty($values['email'])) $userdata['user_email'] = $values['email'];
							if (!empty($values['first_name'])) $userdata['first_name'] = $values['first_name'];
							else $userdata['first_name'] = '';
							if (!empty($values['last_name'])) $userdata['last_name'] = $values['last_name'];
							else $userdata['last_name'] = '';
							$userdata['display_name'] = $userdata['first_name'].' '.$userdata['last_name'];
							$userdata['role'] = 'subscriber';
							$user_id = wp_insert_user( $userdata );
							if ( is_wp_error( $user_id ) ) {
								$return_string .= 'Cannot create user : '.$userdata['user_login'].' because '.$user_id->get_error_message().PHP_EOL;
							}
							else {
								$return_string .= 'Successfully created user : '.$userdata['user_login'].PHP_EOL;
							}							
						}
					}
				}
			}
			return $return_string;
		}
		
		
		
		public function bfi_getTableCount($params) {
			$this->errorFlag = false;
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
			do_action( 'xmlrpc_call', __FUNCTION__); // patterned on the core XML-RPC actions
			if ($errorFlag == true) return return_string;			
			$args     = $params[3];
			// required parameters
			$tableInfo = array("tableName"=>"","content"=>"","delimiter"=>"","where"=>"","orderBy"=>"","limit"=>"");
			foreach($tableInfo as $indexName=>$value) {
				if (!empty($args[$indexName])) $tableInfo[$indexName] = $args[$indexName];
			}
			if ( empty( $tableInfo['tableName'] ) ) return new IXR_Error( 500, __( "Missing parameter 'tableName'" ) );
				
			$query = "SELECT COUNT(*) FROM ".$tableInfo['tableName'];
			if (!empty($tableInfo['where'])) $query .= " WHERE ".$tableInfo['where'];
			if (!empty($tableInfo['orderBy'])) $query .= " ORDER BY ".$tableInfo['orderBy'];
			if (!empty($tableInfo['limit'])) $query .= " LIMIT ".$tableInfo['limit'];
			$return_value = $this->bfi_masterpoint_db->get_var($query);
			if ($return_value == null || false === $return_value) {
				$this->errorFlag = true;
				return 'Invalid Query : '.$query;
			}
			return $return_value;	
		}
		


		

		
		function import_subscribers($numberOfMembers) {
			$html = "<div>Import Subscribers : </div>";
			if ($this->bfi_masterpoint_db) {
				global $wpdb;
				//echo '<script>jQuery("#update-status").html("Importing "'.$numberOfMembers.' members);</script>';
				// Calculate where to start
				$count_last_imported = $wpdb->get_var( "SELECT COUNT(*) FROM sriram_member_count" );
				if ($count_last_imported == 0) {
					$last_imported = 0;
				}
				else {
					$last_imported = $wpdb->get_var( "SELECT last_imported FROM sriram_member_count LIMIT 0,1" );
				}
				$added = 0;
				$notAdded = 0;
				$alreadyExists = 0;
				$query = "SELECT member.member_id AS member_id, valid.password as password, member.email AS email, member.first_name AS first_name, member.last_name AS last_name FROM member ";
				//$query .= " JOIN valid ON valid.username= member.member_id ORDER BY (total_current_fp+total_current_lp) DESC LIMIT 10";
				$query .= " JOIN valid ON valid.username= member.member_id LIMIT ".$last_imported.",".$numberOfMembers;
				//$query .= " JOIN valid ON valid.username= member.member_id";
				$rows = $this->bfi_masterpoint_db->get_results( $this->bfi_masterpoint_db->prepare($query));
				$count = 0;
				$html .= '<p>Count = '.count($rows).'</p>';
				foreach($rows as $row) {
					if (username_exists($row->member_id)) {
						$alreadyExists++;
					}
					else {
						//$html .= '<p>Member '.$row->member_id.' not found! Trying to Add.</p>';
						$userdata = array();
						$userdata['user_login'] = $row->member_id;
						$userdata['user_pass'] = $row->password;
						$userdata['user_email'] = $row->email;
						$userdata['first_name'] = $row->first_name;
						$userdata['last_name'] = $row->last_name;
						$userdata['display_name'] = $row->first_name.' '.$row->last_name;
						$userdata['role'] = 'subscriber';
						$user_id = wp_insert_user( $userdata );
						if ( is_wp_error( $user_id ) ) {
							$html .= '<p>Not Added because : '.$user_id->get_error_message().'</p>';
							$notAdded++;
						}
						else {
							//$html .= '<p>Added</p>';
							$added++;
						}
					}
					/*$userdata = array();
					 $userdata['user_login'] = $row->member_id;
					$userdata['user_pass'] = $row->password;
					$userdata['user_email'] = $row->email;
					$userdata['first_name'] = $row->first_name;
					$userdata['last_name'] = $row->last_name;
					$userdata['display_name'] = $row->first_name.' '.$row->last_name;
					$userdata['role'] = 'subscriber';
					$user_id = wp_insert_user( $userdata );
					if ( is_wp_error( $user_id ) ) {
					$notAdded += 1;
					}
					else {
					$added += 1;
					}
					$count = $count+1;*/
					//echo '<script>jQuery("#update-status").html("Count : "'.$count.', Added : '.$added.', Not Added : '.$notAdded.'</script>';
				}
				$last_imported = $last_imported+$numberOfMembers;
				$wpdb->query( $wpdb->prepare( "UPDATE sriram_member_count SET last_imported=%d",$last_imported));
				$html .= '<p><strong>Already Exisits : '.$alreadyExist.', Added : '.$added.', Not Added: '.$notAdded.'</strong></p>';
			}
			else {
				$html .= '<p><strong>Database is not available for import</strong></p>';
			}
			return $html;
		}
		
		

		function deactivate() {
			global $wp_roles;
			$wp_roles->remove_cap( 'administrator', 'manage_masterpoints' );
			$wp_roles->remove_cap( 'editor', 'manage_masterpoints' );
			$this->deleteFile('user-panel.php');
			$this->deleteFile('profile-form.php');
		}

		function activate() {
			global $wp_roles;
			$wp_roles->add_cap( 'administrator', 'manage_masterpoints' );
			$wp_roles->add_cap( 'editor', 'manage_masterpoints' );
			$this->copyFile('user-panel.php');
			$this->copyFile('profile-form.php');
		}

		function deleteFile($fileName) {
			$destination = get_template_directory().DIRECTORY_SEPARATOR.$fileName;
			unlink($destination);
		}

		function copyFile($fileName) {
			$source = rtrim(plugin_dir_path(__FILE__),'/').DIRECTORY_SEPARATOR.'templates'.DIRECTORY_SEPARATOR.$fileName;
			$destination = get_template_directory().DIRECTORY_SEPARATOR.$fileName;
			copy($source,$destination);;
		}

		function remove_contactmethods( $contactmethods ) {
			unset($contactmethods['aim']);
			unset($contactmethods['jabber']);
			unset($contactmethods['yim']);
			return $contactmethods;
		}

		function getMemberSummary($current_user) {
			$html = "";
			$html .= '<div class="fl" style="padding-right:10px;" title="Member Photo"><em>'.get_avatar( $current_user->ID,'110' ).'</em></div>';
			$html .= '<div>Name : '.$current_user->display_name.'</div>';
			return $html;
		}

		function getBFIMembershipInfo($member_id) {
			$query = "SELECT bfi_member.member_id AS member_number, bfi_rank_master.description AS rank, bfi_zone_master.description AS zone, (bfi_member.total_current_lp+bfi_member.total_current_fp) AS total_points, bfi_member.total_current_fp AS fed_points, bfi_member.total_current_lp AS local_points FROM bfi_member ";
			$query .= "JOIN bfi_zone_master ON bfi_member.zone_code=bfi_zone_master.zone_code JOIN bfi_rank_master ON bfi_member.rank_code=bfi_rank_master.rank_code WHERE member_id=%s LIMIT 1";
			$mydb = $this->bfi_masterpoint_db;
			$rows = $mydb->get_results( $mydb->prepare($query,$member_id));
			if (count($rows) > 0) return $rows[0];
			else return null;
		}

		function showLoggedInShortCodeContent() {
			$current_user = wp_get_current_user();
			if ( 0 == $current_user->ID ) {
				return $this->showNotLoggedInShortcodeContent();
			}
			$member_id = $current_user->user_login;
			$membership_info = $this->getBFIMembershipInfo($member_id);
			if ($membership_info != null) {
				return $this->showMemberShortcodeContent($current_user,$membership_info);
			}
			else { return $this->showNonMemberShortcodeContent($current_user);
			}
		}

		function showMemberShortcodeContent($current_user,$membership_info) {
			ob_start();
			echo $this->getMemberSummary($current_user);
			echo '<div class="fl" style="padding-right:10px;" title="BFI Member Info">';
			echo '<span class="ico aut">Member Number: '.$membership_info->member_number.'</span><br/>';
			echo '<span class="ico ranking-icon">Rank: '.$membership_info->rank.'</span><br/>';
			echo '<span class="ico map-icon">Zone: '.$membership_info->zone.'</span>';
			echo '</div>';
			echo '<div class="fl" style="padding-right:10px;" title="BFI Member Points">';
			echo '<span class="ico trophy-icon">Total Points: '.$membership_info->total_points.'</span><br/>';
			echo '<span class="ico trophy-silver-icon">Fed Points: '.$membership_info->fed_points.'</span><br/>';
			echo '<span class="ico trophy-bronze-icon">Local Points: '.$membership_info->local_points.'</span>';
			echo '</div>';
			$tabs = array('mymasterpoint'=>"My Masterpoints",'leaderboard'=>"Masterpoint Leaderboard");
			$selectedTab = 'mymasterpoint';
			echo $this->getMasterpointTabs($tabs,$selectedTab,$current_user->user_login);
			$out = ob_get_clean();
			return $out;
		}

		function showNonMemberShortcodeContent($current_user) {
			ob_start();
			echo $this->getMemberSummary($current_user);
			echo '<h1>You have to be a member of BFI to see your Masterpoint Summary and Details</h1>';
			$tabs = array('leaderboard'=>"Masterpoint Leaderboard");
			$selectedTab = 'leaderboard';
			echo $this->getMasterpointTabs($tabs,$selectedTab,'');
			$out = ob_get_clean();
			return $out;
		}

		function showNotLoggedInShortcodeContent() {
			ob_start();
			echo '<h1>You have to be a member of BFI and be logged in to see your Masterpoint Summary and Details</h1>';
			$tabs = array('leaderboard'=>"Masterpoint Leaderboard");
			$selectedTab = 'leaderboard';
			echo $this->getMasterpointTabs($tabs,$selectedTab,'');
			$out = ob_get_clean();
			return $out;
		}

		function getMasterpointTabs($tabs,$selectedTab,$member_id) {
			$html = "";
			$html .= '<div class="tabber-widget-default">';
			$html .= '<ul class="tabber-widget-tabs">';
			$tabIDPrefix = 'masterpoint_page_tab_';
			foreach($tabs as $tab=>$tabName) {
				$html .= '<li><a id="'.$tabIDPrefix.$tab.'" onclick="switchMasterpointPageTab(\''.$tab.'\',\''.$tabIDPrefix.'\',\''.plugins_url( 'jquery.datatables.php', __FILE__ ).'\',\''.$member_id.'\');" href="javascript:void(0)">'.$tabName.'</a></li>';
			}
			$html .= '</ul>';
			$html .= '<div class="tabber-widget-content">';
			$html .= '<div class="tabber-widget">';
			$html .= '<div id="bfi_masterpoints_table_container">';
			$html .= '</div></div></div></div>';
			$html .= '<script type="text/javascript">';
			$html .= 'switchMasterpointPageTab(\''.$selectedTab.'\',\''.$tabIDPrefix.'\',\''.plugins_url( 'jquery.datatables.php', __FILE__ ).'\',\''.$member_id.'\');';
			$html .= '</script>';
			return $html;
		}

		/**
		 * Replace shortcode with posts
		 */
		function bfi_masterpoint_display ($atts, $content = null ) {
			if ( is_user_logged_in() ) {
				return $this->showLoggedInShortCodeContent();
			}
			else { return $this->showNotLoggedInShortcodeContent();
			}
		}

		function bfi_add_custom_user_profile_fields( $user ) {
			if ($this->bfi_masterpoint_db) {
				$query = "SELECT * FROM bfi_member WHERE member_id=%s";
				$member_id = $user->user_login;
				$rows = $this->bfi_masterpoint_db->get_results( $this->bfi_masterpoint_db->prepare($query,$member_id));
				if (count($rows) > 0) {
					$row = $rows[0]

					?>
<h3>
	<?php _e('Extra Profile Information', 'your_textdomain');?>
</h3>
<table class="form-table">
	<?php foreach ($this->fieldNames as $fieldName) 
	{
		?>
	<tr>
		<th><label for="<?php echo $fieldName; ?>"><?php _e($fieldName, 'your_textdomain'); ?>
		</label>
		</th>
		<td><input type="text" name="<?php echo $fieldName; ?>"
			id="<?php echo $fieldName; ?>"
			value="<?php echo esc_attr( $row->$fieldName ); ?>"
			class="regular-text" /><br /> <span class="description"><?php _e('Please enter '.$fieldName.'.', 'your_textdomain'); ?>
		</span>
		</td>
	</tr>
	<?php
	}
	?>

</table>
<?php 
				}
			}
		}

		function bfi_save_custom_user_profile_fields( $user_id ) {
			if ( !current_user_can( 'edit_user', $user_id ) )
				return FALSE;

			$user_info = get_userdata($user_id);
			$member_id = $user_info->user_login;
			$query = "UPDATE bfi_member SET ";
			$updateFields = array();
			$updateFields[] = "first_name='$user_info->user_firstname'";
			$updateFields[] = "last_name='$user_info->user_lastname'";
			foreach ($this->fieldNames as $fieldName) {
				$updateFields[] = "$fieldName='$_POST[$fieldName]'";
			}
			$query .= implode(',', $updateFields);
			$query .= " WHERE member_id=%s";
			$rows = $this->bfi_masterpoint_db->query( $this->bfi_masterpoint_db->prepare($query,$member_id));
			// Dont update password since masterpoint valid table uses a blob for pass.
			/*$query = "UPDATE valid SET password='$user_info->user_pass' WHERE member_id=%s";
			$rows = $this->bfi_masterpoint_db->query( $this->bfi_masterpoint_db->prepare($query,$member_id));
			*/
		}
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_masterpoint_display'] = new BFI_Masterpoint_Display();
}