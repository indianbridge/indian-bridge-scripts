<?php
/*
Plugin Name: BFI Custom User Fields
Plugin URI: http://bfi.net.in
Description: A plugin to manage custom fields in user progiel
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'BFI_Custom_User_Fields' ) ) {
	

	class BFI_Custom_User_Fields {
		
		public function __construct() {
			add_action( 'show_user_profile', array($this, 'bfi_add_custom_user_profile_fields') );
			add_action( 'edit_user_profile', array($this, 'bfi_add_custom_user_profile_fields') );
			add_action( 'personal_options_update', array($this, 'bfi_save_custom_user_profile_fields') );
			add_action( 'edit_user_profile_update', array($this, 'bfi_save_custom_user_profile_fields') );	
			add_filter('user_contactmethods',array($this, 'add_twitter_contactmethod'),10,1);				
		}		
		function add_twitter_contactmethod( $contactmethods ) {
		  unset($contactmethods['aim']);
		  unset($contactmethods['jabber']);
		  unset($contactmethods['yim']);
		  return $contactmethods;
		}
	
		function bfi_add_custom_user_profile_fields( $user ) {
			?>
			<h3><?php _e('Extra Profile Information', 'your_textdomain'); ?></h3>
			<table class="form-table">
				<tr>
					<th>
						<label for="address"><?php _e('Address', 'your_textdomain'); ?></label>
					</th>
					<td>
						<input type="text" name="address" id="address" value="<?php echo esc_attr( get_the_author_meta( 'address', $user->ID ) ); ?>" class="regular-text" /><br />
						<span class="description"><?php _e('Please enter your address.', 'your_textdomain'); ?></span>
					</td>
				</tr>
			</table>
			<?php 
		}
		
		function bfi_save_custom_user_profile_fields( $user_id ) {
    
			if ( !current_user_can( 'edit_user', $user_id ) )
				return FALSE;
    
			update_usermeta( $user_id, 'address', $_POST['address'] );
		}		
		
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_custom_user_fields'] = new BFI_Custom_User_Fields();
}