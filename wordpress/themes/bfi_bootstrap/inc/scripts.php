<?php
/**
 * Functions to enqueue all necessary stylesheets and javascript files.
 *
 */
function bfi_bootstrap_scripts() {
	// Load our main stylesheet. (this should be mostly empty)
	wp_enqueue_style( 'bfi_bootstrap-style', get_stylesheet_uri() );	
	
	// Enqueue the bootstrap css
	$skin = bfi_bootstrap_get_redux_option( 'bootswatch' );
	if ( ! isset( $skin ) ) {
		$stylesheet = 'http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css';
	}
	else {
		// Get the appropriate path depending on whether local or cdn option is specified
		$skin_folder = THEME_DIR . '/css/skins/' . $skin;
		$skin_uri = THEME_DIR_URI . '/css/skins/' . $skin;
		$cdn_file = $skin_folder . '/cdn.link';
		if ( is_file( $cdn_file ) ) {
			$stylesheet = file_get_contents( $cdn_file );
		}
		else {
			$stylesheet = $skin_uri . '/bootstrap.min.css';
		}
	}
	wp_enqueue_style( 'bfi_bootstrap-css-core', $stylesheet );	
	
	// Enqueue bootstrap js
	$bootstrap_js = 'http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js';
	//$bootstrap_js = THEME_DIR_URI . '/js/bootstrap.min.js';
	wp_enqueue_script( 'bfi_bootstrap-js-core', $bootstrap_js, '20120206', true );
	
	// Enqueue fontawesome css
	$fontawesome_css = 'http://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css';
	wp_enqueue_style( 'bfi_bootstrap-fontawesome-css', $fontawesome_css );	
	
	
	// Add the padding to body top for fixed navbar
	$header_alignment = bfi_bootstrap_get_redux_option( 'header-alignment' );
	if ( $header_alignment === 'fixed' ) {
		wp_enqueue_script( 'bfi_bootstrap-js-padding-top', THEME_DIR_URI . '/js/jquery.bootstrap.fixed.top.navbar.js', array('jquery'), '20140219', true );
	}
	
	
	// Smartmenus bootstrap addon css
	wp_enqueue_style( '_bootstrap_smartmenus_addon_css', THEME_DIR_URI . '/css/jquery.smartmenus.bootstrap.css' );

	// Smartmenus js
	wp_enqueue_script( '_bootstrap_smartmenus_js', THEME_DIR_URI . '/js/jquery.smartmenus.bootstrap.min.js', array('jquery'), '20140219', true );
	
	// Smartmenus bootstrap addon js
	wp_enqueue_script( '_bootstrap_smartmenus_addon_js', THEME_DIR_URI . '/js/jquery.smartmenus.min.js', array('_bootstrap_smartmenus_js'), '20140219', true );

	// Enable threaded comments
	if ( is_singular() && comments_open() && get_option( 'thread_comments' ) ) {
		wp_enqueue_script( 'comment-reply' );
	}
	
}
add_action( 'wp_enqueue_scripts', 'bfi_bootstrap_scripts' );