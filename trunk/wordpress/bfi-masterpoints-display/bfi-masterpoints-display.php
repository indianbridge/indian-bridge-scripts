<?php
/*
Plugin Name: Wordpress BFI Masterpoint Display
Plugin URI: http://bfi.net.in
Description: A plugin to show posts filtered by categories using ajax
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
			private $dataGridName;
			public function __construct() {
				$this->dataGridName = "jqGrid";
				$dataGridName = $this->dataGridName;
				//$dataGridName = "dataTables";
				//$dataGridName = "flexiGrid";
				$jsFilePath = 'js/jquery.'.$dataGridName.'.min.js';
				$cssFilePath = 'css/jquery.'.$dataGridName.'.css';
				$jsIdentifier = 'jquery_'.$dataGridName.'_js';
				$cssIdentifier = 'jquery_'.$dataGridName.'_css';
				//$use_dataTables = false;
				// register the javascript
				wp_register_script( $jsIdentifier, plugins_url($jsFilePath, __FILE__ ), array( 'jquery' ) );	
				wp_enqueue_script($jsIdentifier);	
				/*if ($use_dataTables) {
					wp_register_script( 'jquery_dataTables', plugins_url( 'js/jquery.dataTables.min.js', __FILE__ ), array( 'jquery' ) );	
					wp_enqueue_script('jquery_dataTables');							
				}
				else {
					wp_register_script( 'jquery_flexigrid', plugins_url( 'js/flexigrid.js', __FILE__ ), array( 'jquery' ) );	
					wp_enqueue_script('jquery_flexigrid');
				}*/

				// Register bfi master point javascript

				wp_register_script( 'bfi_masterpoints_display', plugins_url( 'bfi-masterpoints-display.js', __FILE__ ), array( 'jquery' ) );	
				wp_enqueue_script('bfi_masterpoints_display');
				// register the css
				wp_register_style($cssIdentifier, plugins_url($cssFilePath, __FILE__ ) );	
				wp_enqueue_style($cssIdentifier);				

				/*if ($use_dataTables) {
					wp_register_style( 'jquery_dataTables_css', plugins_url( 'css/jquery.dataTables.css', __FILE__ ) );	
					wp_enqueue_style('jquery_dataTables_css');
					//wp_register_style( 'jquery_dataTables_themeroller_css', plugins_url( 'css/jquery.dataTables_themeroller.css', __FILE__ ) );	
					//wp_enqueue_style('jquery_dataTables_themeroller_css');
					wp_register_style( 'jquery_dataTables_redmond_css', plugins_url( 'css/jquery_ui/jquery-ui-1.10.3.custom.min.css', __FILE__ ) );	
					wp_enqueue_style('jquery_dataTables_redmond_css');
					
				}
				else {
					wp_register_style( 'jquery_flexigrid_css', plugins_url( 'css/jquery.flexigrid.css', __FILE__ ) );	
					wp_enqueue_style('jquery_flexigrid_css');			
				}*/

				// Add the shortcode
				add_shortcode('bfi_masterpoint_display', array($this, 'bfi_masterpoint_display'));
				add_action( 'show_user_profile', array($this, 'bfi_add_custom_user_profile_fields') );
				add_action( 'edit_user_profile', array($this, 'bfi_add_custom_user_profile_fields') );
				add_action( 'personal_options_update', array($this, 'bfi_save_custom_user_profile_fields') );
				add_action( 'edit_user_profile_update', array($this, 'bfi_save_custom_user_profile_fields') );	
				add_action('admin_menu', array($this, 'bfi_database_import_menu'));				
				$this->bfi_masterpoint_db = new wpdb('indianbridge', 'kibitzer', 'masterpoints', 'localhost');
				$this->fieldNames = array('address_1', 'address_2', 'address_3', 'city', 'state', 'country','residence_phone','mobile_no','sex','dob');
				//$this->bfi_masterpoint_db->show_errors();
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


    			// See if the user has posted us some information
    			// If they did, this hidden field will be set to 'Y'
				if( isset($_POST[ $hidden_field_name ]) && $_POST[ $hidden_field_name ] == 'Y' ) {
					if ($this->bfi_masterpoint_db) {
						$added = 0;
						$notAdded = 0;
						$query = "SELECT * FROM member LIMIT 0,10";
						$rows = $this->bfi_masterpoint_db->get_results( $this->bfi_masterpoint_db->prepare($query));
						foreach($rows as $row) {
							$userdata = array();
							$userdata['user_login'] = $row->member_id;
							$query = "SELECT password FROM valid WHERE username=%s";
							$passwordRow = $this->bfi_masterpoint_db->get_row($this->bfi_masterpoint_db->prepare($query,$member_id));
							if ($passwordRow != null) $userdata['user_pass'] = $passwordRow->password;
							else $userdata['user_pass'] = 'bridge';
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
						}
						?>
						<div class="updated"><p><strong>Added : <?php echo $added; ?>, Not Added: <?php echo $notAdded; ?></strong></p></div>
						<?php
					}
        			else {
					?>
					<div class="updated"><p><strong><?php _e('Database is not available for import', 'menu-test' ); ?></strong></p></div>
					<?php
					}

				}

    			// Now display the settings editing screen

				echo '<div class="wrap">';

   				 // header

				echo "<h2>" . __( 'Import BFI Members from Database', 'menu-test' ) . "</h2>";

    			// settings form

				?>

				<form name="form1" method="post" action="">
					<input type="hidden" name="<?php echo $hidden_field_name; ?>" value="Y">

					<p class="submit">
						<input type="submit" name="Submit" class="button-primary" value="<?php esc_attr_e('Import BFI Members') ?>" />
					</p>

				</form>
			</div>

			<?php

		}

		function remove_subscribers() {
			$args = array( 'role' => 'subscriber' );
			$subscribers = get_users( $args );
			if( !empty($subscribers) ) {
				$i = 0;
				foreach( $subscribers as $subscriber ) {
					if( wp_delete_user( $subscriber->ID ) ) {
						$i++;
					}
				}
				echo $i.' Subscribers deleted';
			} else {
				echo 'No Subscribers deleted';
			}
		}		

		/**
		 * Replace shortcode with posts
		 */
		function bfi_masterpoint_display ($atts, $content = null ) {
			ob_start();
			?>
			<div class="datagrid1">
				<table id="bfi_masterpoints_table"></table>
				<div id="bfi_masterpoints_pager"></div>
				</table>
			</div>
			<script type="text/javascript">
				bfi_masterpoint_renderTable(<?php echo "'".$this->dataGridName."'"; ?>);
				//bfi_masterpointTable_flexigrid();
			</script>
			<?php
			$out = ob_get_clean();
			return $out;		 
		}

		function bfi_add_custom_user_profile_fields( $user ) {
			if ($this->bfi_masterpoint_db) {
				$query = "SELECT * FROM member WHERE member_id=%s";
				$member_id = $user->user_login;
				//$member_id = 'WB000777';
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