<?php
/**
 * Scripts Enqueuer.
 *
 * Enqueues all css and js files
 *
 * @package _bootstrap
 */

if ( ! function_exists( '_bootstrap_scripts' ) ) {
	function _bootstrap_scripts() {
		// Load our main stylesheet. (this should be mostly empty)
		wp_enqueue_style( '_bootstrap-style', get_stylesheet_uri() );	
		
		// Load the bootstrap style based on options
		$skin_folder = THEME_DIR . '/css/skins/' . _bootstrap_get_option( 'bootstrap_skins' );
		$skin_uri = THEME_DIR_URI . '/css/skins/' . _bootstrap_get_option( 'bootstrap_skins' );
		$stylesheet = $skin_uri . '/bootstrap.min.css';
		$use_cdn = _bootstrap_get_option( 'local_or_cdn_css' );
		if ( $use_cdn ) {
			// Check if cdn link is provided
			$cdn_file = $skin_folder . '/cdn.link';
			if ( is_file( $cdn_file ) ) {
				$stylesheet = file_get_contents( $cdn_file );
			}
		}
		wp_enqueue_style( '_bootstrap-css', $stylesheet );
		
		// Smartmenus bootstrap addon css
		wp_enqueue_style( '_bootstrap_smartmenus_addon_css', THEME_DIR_URI . '/css/jquery.smartmenus.bootstrap.css' );
		
		// Enqueue bootstrap js.
		$use_cdn = _bootstrap_get_option( 'local_or_cdn_js' );
		$bootstrap_js = $use_cdn?_bootstrap_get_option( 'cdn_js_location' ):THEME_DIR_URI . '/js/bootstrap.min.js';
		wp_enqueue_script( '_bootstrap_js', $bootstrap_js, array('jquery'), '20140219', true );

		// Smartmenus js
		wp_enqueue_script( '_bootstrap_smartmenus_js', THEME_DIR_URI . '/js/jquery.smartmenus.bootstrap.min.js', array('jquery'), '20140219', true );
		
		// Smartmenus bootstrap addon js
		wp_enqueue_script( '_bootstrap_smartmenus_addon_js', THEME_DIR_URI . '/js/jquery.smartmenus.min.js', array('_bootstrap_smartmenus_js'), '20140219', true );
		
		// underscores navigation addon
		wp_enqueue_script( '_bootstrap-navigation', THEME_DIR_URI . '/js/navigation.js', array(), '20120206', true );

		// underscores addon
		wp_enqueue_script( '_bootstrap-skip-link-focus-fix', THEME_DIR_URI . '/js/skip-link-focus-fix.js', array(), '20130115', true );

		// Enable threaded comments
		if ( is_singular() && comments_open() && get_option( 'thread_comments' ) ) {
			wp_enqueue_script( 'comment-reply' );
		}
	}
}
add_action( 'wp_enqueue_scripts', '_bootstrap_scripts' );
 
 ?>
