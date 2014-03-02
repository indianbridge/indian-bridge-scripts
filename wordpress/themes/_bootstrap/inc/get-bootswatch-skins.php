<?php
/**
 * Retreive bootswatch skins using bootswatch api.
 *
 * @param $url the url for the bootswatch api (expected to be http://api.bootswatch.com/3/)
 * @param $skins_folder the local folder where you want to copy the swatches
 * @param $overwrite if a folder already exists should the contents be overwritten,
 * 					 optional defaults to FALSE
 * @package _bootstrap
 */
if ( ! function_exists( '_bootstrap_get_bootswatch_skins' ) ) {
	function _bootstrap_get_bootswatch_skins( $url, $skins_folder, $overwrite = FALSE ) {
		// Retreive the swatch list
		$response = wp_remote_get( $url );
		
		if ( is_wp_error( $response ) ) {
			// Report error if there is an error
			$error_message = $response->get_error_message();
			_e( 'Something went wrong: ', '_bootstrap' );
			echo $error_message;
		} 
		else {
			// Copy to local folders
   			_bootstrap_populate_bootswatch_folder( json_decode( $response['body'] ), $skins_folder, $overwrite );
		}
	}
}

/**
 * Copy retreived bootswatches to local folders.
 *
 * @param $skins_json the retreived swatches as an object decoded from json
 * @param $skins_folder the local folder where you want to copy the swatches
 * @param $overwrite if a folder already exists should the contents be overwritten,
 * 					 optional defaults to FALSE
 * @package _bootstrap
 */
if ( ! function_exists( '_bootstrap_populate_bootswatch_folder' ) ) {
	function _bootstrap_populate_bootswatch_folder( $skins_json, $skins_folder, $overwrite = FALSE ) {
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
 * @package _bootstrap
 */
if ( ! function_exists( '_bootstrap_copy_one_swatch' ) ) {
	function _bootstrap_copy_one_swatch( $swatch, $folder, $overwrite = FALSE ) {
		_e( 'Processing Swatch : ', '_bootstrap' );
		echo $swatch->name;
		// Check if folder needs to be overwritten
		if ( is_dir( $folder) && ! $overwrite ) {
			_e( 'Folder already exists and no overwrite option selected.', '_bootstrap' );
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
		
		// Copy the min css file
		copy( $swatch->cssMin, $folder . '/bootstrap.min.css' );
		
		// Copy cdn link
		file_put_contents( $folder . '/cdn.link', $swatch->cssCdn );
		_e( 'Processed', '_bootstrap' );
	}
}
?>