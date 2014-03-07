<?php
/**
 * Functions to setup settings page and scripts to retreive bootswatch skins.
 * @package _bootstrap
 */

/**
 * Add an admin page to menu.
 * 
  * @return nothing
 */
if ( ! function_exists( '_bootstrap_setup_bootswatch_admin_page' ) ) {
	function _bootstrap_setup_bootswatch_admin_page() {
		$title = __( 'Get Bootswatch Themes', '_bootstrap' );
		add_theme_page( $title, $title, 'manage_options', 
		'get-bootswatch-themes', '_bootstrap_get_bootswatch_themes_settings' );
	}
}
add_action("admin_menu", "_bootstrap_setup_bootswatch_admin_page");

/**
 * Setup the admin page to load bootswatch themes using api.
 * 
 * @return nothing
 *
 */
if ( ! function_exists( '_bootstrap_get_bootswatch_themes_settings' ) ) {
	function _bootstrap_get_bootswatch_themes_settings() {
		// Check that the user is allowed to update options
		if (!current_user_can('manage_options')) {
			wp_die( __('You do not have sufficient permissions to access this page.', '_bootstrap' ) );
		}	
		?>
		<div id="get_bootswatch_themes_progress" class="updated"></div>
		<?php
		if ( isset( $_POST["get_bootswatch_themes"] ) && $_POST["get_bootswatch_themes"] === 'Y' ) {
			$url = $_POST["bootswatch_api_url"];
			$skins_folder = $_POST["local_folder"];
			$overwrite = isset( $_POST["overwrite"] );	
			_bootstrap_get_bootswatch_skins( $url, THEME_DIR . $skins_folder, $overwrite );
			?>

			<?php
		}
		?>
		    <div class="wrap">
		        <h2><?php _e( 'Copy Bootswatch themes to local folder', '_bootstrap' ); ?></h2>
		 		
		        <form method="POST" action="">
		        	<input type="hidden" name="get_bootswatch_themes" value="Y" />
		            <table class="form-table">
		            	<tr>
		            		<th colspan=2>
		            			<h3><?php _e( 'Do not change any options unless you are absolutely sure about what you are doing!!!', '_bootstrap' ); ?></h3>
		            		</th>
		            	</tr>
		                <tr valign="top">
		                    <th scope="row">
		                        <label for="bootswatch_api_url">
		                            <?php _e( 'Bootswatch API URL', '_bootstrap' ); ?>:
		                        </label> 
		                    </th>
		                    <td>
		                        <input type="text" name="bootswatch_api_url" size="55" value="http://api.bootswatch.com/3/" />
		                    </td>
		                </tr>
		                <tr valign="top">
		                    <th scope="row">
		                        <label for="local_folder">
		                            <?php _e( 'Path to Local Folder Relative to Theme Root:', '_bootstrap' ); ?>
		                        </label> 
		                    </th>
		                    <td>
		                        <input type="text" name="local_folder" size="55" value="/css/skins" />
		                    </td>
		                </tr>	
		            	<tr>
		            		<th colspan=2>
		            			<h3><?php _e( 'Enable this option if some of the existing themes have updates.', '_bootstrap' ); ?></h3>
		            		</th>
		            	</tr>		                
		                <tr valign="top">
		                    <th scope="row">
		                        <label for="overwrite">
		                            <?php _e( 'Overwrite Contents if Folder already exists:', '_bootstrap' ); ?>
		                        </label> 
		                    </th>
		                    <td>
		                        <input type="checkbox" name="overwrite" />
		                    </td>
		                </tr>	                	                
		            </table>
		            <input class='button-primary' type='submit' name='submit_button' value='<?php _e( 'Get Bootswatch Themes', '_bootstrap' ); ?>' />
		            <h2>Once you click the above button, the retreival of themes might take some time (depending on number of themes to retreive) during which you will not see any updtes. Please be patient. When the script finishes you will get a summary of what happened.</h2>
		        </form>
		    </div>
		<?php
	}
}

/**
 * Retreive bootswatch skins using bootswatch api.
 *
 * @param $url the url for the bootswatch api (expected to be http://api.bootswatch.com/3/)
 * @param $skins_folder the local folder where you want to copy the swatches
 * @param $overwrite if a folder already exists should the contents be overwritten,
 * 					 optional defaults to FALSE
 * @return nothing
 */
if ( ! function_exists( '_bootstrap_get_bootswatch_skins' ) ) {
	function _bootstrap_get_bootswatch_skins( $url, $skins_folder, $overwrite = FALSE ) {
		$message = _( 'Progress Update', '_bootstrap' );
		_bootstrap_display_message( $message );
		// Retreive the swatch list
		$response = wp_remote_get( $url );
		
		if ( is_wp_error( $response ) ) {
			// Report error if there is an error
			$message = sprintf( __('Something went wrong: %s', '_bootstrap', $response->get_error_message() ) );
			_bootstrap_display_message( $message, TRUE );
		} 
		else {
			// Copy to local folders
   			_bootstrap_populate_bootswatch_folder( json_decode( $response['body'] ), $skins_folder, $overwrite );
		}
	}
}

/**
 * Function to display a message in a div using jQuery.
 *
 * @param $message the string message to display.
 * @return nothing
 */
function _bootstrap_display_message( $message, $append = FALSE ) {
	?>
	<script>		
		jQuery('#get_bootswatch_themes_progress').<?php echo $append ? 'append' : 'html'; ?>( '<?php echo $message; ?><br/>' );
	</script>	
	<?php
}

/**
 * Copy retreived bootswatches to local folders.
 *
 * @param $skins_json the retreived swatches as an object decoded from json
 * @param $skins_folder the local folder where you want to copy the swatches
 * @param $overwrite if a folder already exists should the contents be overwritten,
 * 					 optional defaults to FALSE
  * @return nothing
 */
if ( ! function_exists( '_bootstrap_populate_bootswatch_folder' ) ) {
	function _bootstrap_populate_bootswatch_folder( $skins_json, $skins_folder, $overwrite = FALSE ) {
		$total_themes = count( $skins_json->themes );
		if ( $total_themes < 1 ) {
			$message = __( 'No themes retreived from Bootswatch API. Nothing to do.', '_bootstrap' );
			_bootstrap_display_message( $message, TRUE );
			return;
		}
		$message = sprintf( _n( 'Processing 1 Theme', 'Processing %d Themes', $total_themes, '_bootstrap' ), $total_themes );
		_bootstrap_display_message( $message, TRUE );
		// Iterate over the swatches list and process each one in turn
		foreach($skins_json->themes as $item) {
			$skin_folder = $skins_folder . '/' . $item->name;
			_bootstrap_copy_one_swatch( $item, $skin_folder, $overwrite );
		}
	}
}

/**
 * Copy one bootswatch to local folder if necessary.
 *
 * @param $swatch one of the retreived swatches
 * @param $folder the local folder where you want to copy the swatch
 * @param $overwrite if a folder already exists should the contents be overwritten,
 * 					 optional defaults to FALSE
 * @return nothing
 */
if ( ! function_exists( '_bootstrap_copy_one_swatch' ) ) {
	function _bootstrap_copy_one_swatch( $swatch, $folder, $overwrite = FALSE ) {
		$message = sprintf ( __( 'Processing Swatch : %s', '_bootstrap' ), $swatch->name );
		_bootstrap_display_message( $message, TRUE );
		// Check if folder needs to be overwritten
		if ( is_dir( $folder) && ! $overwrite ) {
			$message = __( 'Folder already exists and no overwrite option selected. Skipping...', '_bootstrap' );
			_bootstrap_display_message( $message, TRUE );
			return;
		}
		
		// Create the folder if it does not exist
		if ( ! is_dir( $folder ) ) {
			mkdir( $folder );
		}
		
		// Copy the screenshot
		$image = wp_get_image_editor( $swatch->thumbnail );
		$image->resize( 100, 100, FALSE );
		$image->save( $folder . '/screenshot.png' );
		$message = 	__( 'Copied Screenshot', '_bootstrap' );	
		_bootstrap_display_message( $message, TRUE );
		
		// Copy the min css file
		copy( $swatch->cssMin, $folder . '/bootstrap.min.css' );
		$message = 	__( 'Copied css file', '_bootstrap' );	
		_bootstrap_display_message( $message, TRUE );
		
		// Copy cdn link
		file_put_contents( $folder . '/cdn.link', $swatch->cssCdn );
		$message = 	__( 'Copied cdn link', '_bootstrap' );	
		_bootstrap_display_message( $message, TRUE );	
		sleep( 0.1 ); // Let Browser refresh?
	}
}
?>