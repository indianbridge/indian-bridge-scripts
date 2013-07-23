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
				add_action('admin_menu', array($this, 'bfi_database_import_menu'));   
				add_action( 'edit_user_profile_update', array($this, 'bfi_save_custom_user_profile_fields') );	
				add_filter('user_contactmethods',array($this, 'remove_contactmethods'),10,1);	
				add_filter( 'xmlrpc_methods', array( $this, 'add_xml_rpc_methods' ) );
				register_activation_hook( __FILE__, array( $this, 'activate' ) );	
				register_deactivation_hook(__FILE__, array( $this, 'deactivate' ));
				$this->bfi_masterpoint_db = new wpdb('bfinem7l_sriram', 'kibitzer', 'bfinem7l_masterpoints', 'localhost');
				$this->fieldNames = array('address_1','address_2','address_3','city','country','residence_phone','mobile_no','sex','dob');
			}
			
			public function add_xml_rpc_methods( $methods ) {
				$methods['bfi.postMasterpoints'] = array( $this, 'bfi_post_masterpoints' );
				return $methods;
			}	
			
			public function bfi_post_masterpoints( $params ) {
				
				/*global $wp_xmlrpc_server;
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
					return new IXR_Error( 403, __( 'User '.$username.' is not allowed to manage masterpoints and so cannot run this software.' ) );
				
				do_action( 'xmlrpc_call', 'bfi.postMasterpoints' ); // patterned on the core XML-RPC actions
				
				// required parameters
				if ( empty( $args['content'] ) ) return new IXR_Error( 500, __( "Missing parameter 'content'" ) );
				
				$content = $args['content'];
				// Update post 37
				$my_post = array();
				$my_post['ID'] = 1536;
				$my_post['post_content'] = $content;

				// Update the post into the database
				wp_update_post( $my_post );				

				return strval($my_post['ID']);*/
				return '';
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
			
                        function bfi_database_import_menu() {
                                add_users_page( 'Import Members from Masterpoint Table', 'Import BFI Members', 'manage_masterpoints', 'bfi-database-import',array($this, 'bfi_database_import'));
                        }

                        function bfi_database_import() {

                        //must check that the user has the required capability 
                                if (!current_user_can('manage_masterpoints'))
                                {
                                        wp_die( __('You do not have sufficient permissions to access this page.') );
                                }

                        // variables for the field and option names 
                                $hidden_field_name = 'mt_submit_hidden';

                                ?>
                                <div id="update-status" class="updated"></div>
                                <?php
                        // See if the user has posted us some information
                        // If they did, this hidden field will be set to 'I' or 'D'
                                if( isset($_POST[ $hidden_field_name ]) && ($_POST[ $hidden_field_name ] == 'I' || $_POST[ $hidden_field_name ] == 'D')) {
                                        $numberOfMembers = 0;
                                        if (isset($_POST[ 'number_of_members'])) {
                                                $numberOfMembers = intval($_POST[ 'number_of_members']);
                                        }
                                        if ($numberOfMembers==0) {
                                                $numberOfMembers = 500;
                                        }
                                        if ($_POST[ $hidden_field_name ] == 'I') {
                                                $updatedHTML = $this->import_subscribers($numberOfMembers);
                                        }
                                        else if ($_POST[ $hidden_field_name ] == 'D') {
                                                $updatedHTML = $this->remove_subscribers($numberOfMembers);
                                        }
                                        ?>
                                        <div class="updated">Finished : <?php echo $updatedHTML; ?></div>
                                        <?php
                                }

                        // Now display the settings editing screen

                                echo '<div class="wrap">';

                                 // header

                                echo "<h2>" . __( 'Manage BFI Members from Database', 'menu-test' ) . "</h2>";

                        // settings form

                                ?>

                                <form name="form1" method="post" action="">
                                        <input type="hidden" name="<?php echo $hidden_field_name; ?>" value="I">
                                        <span>Number of Members to import : </span><input name="number_of_members" value="500">
                                        <p class="submit">
                                                <input type="submit" name="Submit" class="button-primary" value="<?php esc_attr_e('Import BFI Members') ?>" />
                                        </p>

                                </form>
                                <form name="form2" method="post" action="">
                                        <input type="hidden" name="<?php echo $hidden_field_name; ?>" value="D">
                                        <span>Number of Members to delete : </span><input name="number_of_members" value="500">
                                        <p class="submit">
                                                <input type="submit" name="Submit" class="button-primary" value="<?php esc_attr_e('Remove Subscribers') ?>" />
                                        </p>

                                </form>                         
                                </div>

                        <?php

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

                function remove_subscribers($numberOfMembers) {
                        $html = "Remove Subscribers : ";
                        $args = array( 'role' => 'subscriber' );
                        $subscribers = get_users( $args );
                        if( !empty($subscribers) ) {
                                $i = 0;
                                foreach( $subscribers as $subscriber ) {
                                        if( wp_delete_user( $subscriber->ID ) ) {
                                                $i++;
                                        }
                                }
                                $html .= ''.$i.' Subscribers deleted';
                        } else {
                                $html .= 'No Subscribers deleted';
                        }
                        return $html;
                }               			



		function getMemberSummary($current_user) {
			$html = "";
			$html .= '<div class="fl" style="padding-right:10px;" title="Member Photo"><em>'.get_avatar( $current_user->ID,'110' ).'</em></div>';
			$html .= '<div>Name : '.$current_user->display_name.'</div>';
			return $html;
		}

		function getBFIMembershipInfo($member_id) {
			$query = "SELECT member.member_id AS member_number, rank_master.description AS rank, zone_master.description AS zone, (member.total_current_lp+member.total_current_fp) AS total_points, member.total_current_fp AS fed_points, member.total_current_lp AS local_points FROM member ";
			$query .= "JOIN zone_master ON member.zone_code=zone_master.zone_code JOIN rank_master ON member.rank_code=rank_master.rank_code WHERE member_id=%s LIMIT 1";
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
			if ($membership_info != null) { return $this->showMemberShortcodeContent($current_user,$membership_info); }
			else { return $this->showNonMemberShortcodeContent($current_user); }		
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
			if ( is_user_logged_in() ) { return $this->showLoggedInShortCodeContent(); }
			else { return $this->showNotLoggedInShortcodeContent(); }
		}

		function bfi_add_custom_user_profile_fields( $user ) {
			if ($this->bfi_masterpoint_db) {
				$query = "SELECT * FROM member WHERE member_id=%s";
				$member_id = $user->user_login;
				$rows = $this->bfi_masterpoint_db->get_results( $this->bfi_masterpoint_db->prepare($query,$member_id));
				if (count($rows) > 0) {
					$row = $rows[0]
					
					?>
					<h3><?php _e('Extra Profile Information', 'your_textdomain');?></h3>
					<table class="form-table">
						<?php foreach ($this->fieldNames as $fieldName) 
						{
							?>
							<tr>
								<th>
									<label for="<?php echo $fieldName; ?>"><?php _e($fieldName, 'your_textdomain'); ?></label>
								</th>							
								<td>
									<input type="text" name="<?php echo $fieldName; ?>" id="<?php echo $fieldName; ?>" value="<?php echo esc_attr( $row->$fieldName ); ?>" class="regular-text" /><br />
									<span class="description"><?php _e('Please enter '.$fieldName.'.', 'your_textdomain'); ?></span>
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
			$query = "UPDATE member SET ";
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