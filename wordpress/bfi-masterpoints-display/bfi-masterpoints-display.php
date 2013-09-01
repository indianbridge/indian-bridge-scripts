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
			$methods['bfi.getTableCount'] = array($this,'bfi_getTableCount');
			$methods['bfi.getTableData'] = array($this,'bfi_getTableData');
			$methods['bfi.addTableData'] = array($this,'bfi_addTableData');
			$methods['bfi.addUsers'] = array($this,'bfi_addUsers');
			return $methods;
		}
		
		
		public function bfi_addUsers($params) {
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $args );
		
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];
			$args     = $params[3];
		
			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $wp_xmlrpc_server->error;
			}
		
			if ( ! current_user_can( 'manage_masterpoints' ) )
				return new IXR_Error( 403, __( 'User '.$username.' is not allowed to manage masterpoints and so cannot add users.' ) );
		
			do_action( 'xmlrpc_call', 'bfi.addUsers' ); // patterned on the core XML-RPC actions
		
			// required parameters
			if ( empty( $args['content'] ) ) return new IXR_Error( 500, __( "Missing parameter 'content'" ) );
			$delimiter = "\t";
			if (!empty($args['delimiter'])) $delimiter = $args['delimiter'];
			$content = $args['content'];
			$lines = explode(PHP_EOL,$content);
			foreach($lines as $line) {
				$fields = explode($delimiter,$line);
				// Expected order is member id, name, local, national, total
				$userdata['user_login'] = $fields[0];
				$userdata['display_name'] = $fields[1];
				$nameSplit = explode(' ',$userdata['display_name'],2);
				$userdata['first_name'] = $nameSplit[0];
				$userdata['last_name'] = count($nameSplit) > 1?$nameSplit[1]:'';
				$userdata['user_pass'] = 'bridge';
				$local = $fields[2];
				$national = $fields[3];
				$total = $fields[4];
		
				// Insert into members table
				$this->bfi_masterpoint_db->insert();
		
				// Insert into users table
			}
		
			$output = '';
		
			return $output;
		}
		
		
		
		public function bfi_getTableCount($params) {
			/*global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $args );
			
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];
			
			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $wp_xmlrpc_server->error;
			}
			
			if ( ! current_user_can( 'manage_masterpoints' ) )
				return new IXR_Error( 403, __( 'User '.$username.' is not allowed to manage masterpoints and so cannot request tournament types information.' ) );
			
			do_action( 'xmlrpc_call', 'bfi.getTableData' ); // patterned on the core XML-RPC actions
			if (!$this->bfi_masterpoint_db) {
				return new IXR_Error( 403, __("Invalid Masterpoint Database!"));
			}*/
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
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
			if (!return_value) return 0;
			return $return_value;	
		}
		
		public function bfi_checkManageMasterpointCredentials($params) {
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $params );
			$errorFlag = true;
				
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];
				
			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $wp_xmlrpc_server->error;
			}
				
			if ( ! current_user_can( 'manage_masterpoints' ) )
				return new IXR_Error( 403, __( 'User '.$username.' is not allowed to manage masterpoints and so cannot request tournament types information.' ) );
				
			do_action( 'xmlrpc_call', 'bfi.getTableData' ); // patterned on the core XML-RPC actions
			if (!$this->bfi_masterpoint_db) {
				return new IXR_Error( 403, __("Invalid Masterpoint Database!"));
			}
			$errorFlag = false;
			return '';	
		}
		
		public function bfi_addTableData($params) {
			/*global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $args );
			
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];
			
			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $wp_xmlrpc_server->error;
			}
			
			if ( ! current_user_can( 'manage_masterpoints' ) )
				return new IXR_Error( 403, __( 'User '.$username.' is not allowed to manage masterpoints and so cannot request tournament types information.' ) );
			
			do_action( 'xmlrpc_call', 'bfi.getTableData' ); // patterned on the core XML-RPC actions
			if (!$this->bfi_masterpoint_db) {
				return new IXR_Error( 403, __("Invalid Masterpoint Database!"));
			}*/
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
			if ($errorFlag == true) return return_string;
			$args     = $params[3];
			// required parameters
			if ( empty( $args['tableName'] ) ) return new IXR_Error( 500, __( "Missing parameter 'tableName" ) );
			if ( empty( $args['content'] ) ) return new IXR_Error( 500, __( "Missing parameter 'content" ) );
			$tableName = $args['tableName'];
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
						return $return_string;
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
							$return_string .= 'Successfully inserted '.$line.PHP_EOL;
						}
					}
				}
			}
			return $return_string;
		}
		
		public function bfi_getTableData($params) {
			/*global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $args );
				
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];
				
			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $wp_xmlrpc_server->error;
			}
				
			if ( ! current_user_can( 'manage_masterpoints' ) )
				return new IXR_Error( 403, __( 'User '.$username.' is not allowed to manage masterpoints and so cannot request tournament types information.' ) );
				
			do_action( 'xmlrpc_call', 'bfi.getTableData' ); // patterned on the core XML-RPC actions	
			if (!$this->bfi_masterpoint_db) {
				return new IXR_Error( 403, __("Invalid Masterpoint Database!"));
			}*/
			$return_string = $this->bfi_checkManageMasterpointCredentials($params);
			if ($errorFlag == true) return return_string;		
			$errorFlag = true;	
			$args     = $params[3];
			// required parameters
			$tableInfo = array("tableName"=>"","content"=>"","delimiter"=>"","where"=>"","orderBy"=>"","limit"=>"");
			foreach($tableInfo as $indexName=>$value) {
				if (!empty($args[$indexName])) $tableInfo[$indexName] = $args[$indexName];
			}
			if ( empty( $tableInfo['tableName'] ) ) return new IXR_Error( 500, __( "Missing parameter 'tableName'" ) );
			
			$query = "SELECT * FROM ".$tableInfo['tableName'];
			if (!empty($tableInfo['where'])) $query .= " WHERE ".$tableInfo['where'];
			if (!empty($tableInfo['orderBy'])) $query .= " ORDER BY ".$tableInfo['orderBy'];
			if (!empty($tableInfo['limit'])) $query .= " LIMIT ".$tableInfo['limit'];
			try {
				$results = $this->bfi_masterpoint_db->get_results($query,ARRAY_A);
				if ($results == null) {
					return 'Invalid Query : '.$query.' or no results found!';
				}
				$return_string = '';
				foreach($results as $index=>$result) {
					if ($index == 0) {
						$return_string .= implode(",",array_keys($result)).PHP_EOL;
					}
					$return_string .= implode(",",array_values($result)).PHP_EOL;
				}
			}
			catch (Exception $e) {
				return $e->getMessage();
			}
			$errorFlag = false;
			return $return_string;
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