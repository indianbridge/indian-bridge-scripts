<?php
/**
 * Functions to enqueue all necessary stylesheets and javascript files.
 *
 * Defines functions to add repeatedly used option sections as well as
 * functions to retreive all the values in option sections as an associative array.
 *
 * LICENSE: #LICENSE#
 *
 * @package    _bootstrap_core
 * @author     Sriram Narasimhan
 * @copyright  #COPYRIGHT#
 * @version    SVN: $Id$
 * @since      File available since Release 1.0.0
 *
 */
function _bootstrap_scripts() {
	// Load our main stylesheet. (this should be mostly empty)
	wp_enqueue_style( '_bootstrap-style', get_stylesheet_uri() );	
	
	$page_name = 'style';
	// Enqueue the bootstrap css
	_bootstrap_enqueue_bootstrap_css( $page_name );
	
	// Add the padding to body top if fixed navbar will be used in header
	_bootstrap_navbar_padding( 'top', 'header' );
	
	// Add the padding to body bottom if fixed navbar will be used in header
	_bootstrap_navbar_padding( 'bottom', 'footer' );	
	
	// Enqueue Font awesome is requested
	$section_name = 'font_awesome_css';
	_bootstrap_enqueue_asset( $page_name, $section_name, 'stylesheet' );
	
	// Smartmenus bootstrap addon css
	wp_enqueue_style( '_bootstrap_smartmenus_addon_css', THEME_DIR_URI . '/css/jquery.smartmenus.bootstrap.css' );
	
	// Enqueue bootstrap js if requested
	$section_name = 'bootstrap_js';
	_bootstrap_enqueue_asset( $page_name, $section_name, 'script' );

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
add_action( 'wp_enqueue_scripts', '_bootstrap_scripts' );

/**
* Add css or js for body padding based on options set for navbars.
* 
* @param string $location the location of the navbar
* @param string $page_name the page of options where the navbar options are specified
* @param string $section_name the section of the page where the options are specified
* 
* @return void
*/
function _bootstrap_navbar_padding( $location, $page_name, $section_name = 'navbar' ) {
	
	// Retreive the options
	$options = _bootstrap_get_navbar_options( $page_name, $section_name );
	
	//  Add the padding if fixed alignment is chosen
	if ( $options['alignment'] === 'fixed' ) {
		if ( $options['padding_style'] === 'constant' ) {
			// Set a constant padding as set in option
			$custom_css = 'body {  padding-' . $location . ': ' . $options['padding_constant'] . 'px; }';
			// wp_add_inline_style has to be added to existing enqueued style
			wp_add_inline_style( '_bootstrap-css', $custom_css );
		}
		else {
			// Use jquery to compute height of header and set padding accordingly
			wp_enqueue_script( '_bootstrap_navbar_' . $location . '_js', THEME_DIR_URI . $options['padding_js_location'], array('jquery'), '20140219', true );
		}
	}
}

/**
* Enqueue a css or js asset using options specified
* 
* @param string $page_name the options page where the options are specified
* @param string $section_name the section of the page where options are specified
* @param string $asset_type the kind of asset (stylesheet or script)
* 
* @return void
*/
function _bootstrap_enqueue_asset( $page_name, $section_name, $asset_type ) {
	$options = _bootstrap_get_asset_location_options( $page_name, $section_name );
	if ( $options['location'] !== 'no' ) {
		$asset_url = $options['location'] === 'cdn' ? $options['cdn_location'] : str_replace( '$THEME_URI', THEME_DIR_URI, $options['local_location'] );
		if ( $asset_type === 'stylesheet' ) {
			wp_enqueue_style( $section_name, $asset_url);
		}
		else if ( $asset_type === 'script' ) {
			wp_enqueue_script(  $section_name, $asset_url, array(), null, TRUE );
		}
	}	
}

/**
* Enqueue the bootstrap css file
*
* @param string $page_name the page where the options are specified
* 
* @return void
*/
function _bootstrap_enqueue_bootstrap_css( $page_name ) {
	$section_name = 'skins';
	// Retreive the selected skin
	$skin = _bootstrap_get_redux_option( $page_name, $section_name, 'skin' );
	$custom_skin_name = 'Custom_Internal';
	if ( $skin === $custom_skin_name ) {
		// If custom selected then get the path specified in textbox
		$stylesheet = _bootstrap_get_redux_option( $page_name, $section_name, 'bootstrap_custom_css_location' );
	}
	else {
		// Get the appropriate path depending on whether local or cdn option is specified
		$skin_folder = THEME_DIR . '/css/skins/' . $skin;
		$skin_uri = THEME_DIR_URI . '/css/skins/' . $skin;
		$bootstrap_css_location = _bootstrap_get_redux_option( $page_name, $section_name, 'bootstrap_css' );
		$use_cdn = ( $bootstrap_css_location == 'cdn' );
		if ( $use_cdn ) {
			// Check if cdn link is provided
			$cdn_file = $skin_folder . '/cdn.link';
			if ( is_file( $cdn_file ) ) {
				$stylesheet = file_get_contents( $cdn_file );
			}
			else {
				$stylesheet = $skin_uri . '/bootstrap.min.css';
			}
		}
		else {
			$stylesheet = $skin_uri . '/bootstrap.min.css';
		}
	}	
	
	// Enqueue the selected bootstrap css file
	wp_enqueue_style( '_bootstrap-css', $stylesheet );	
}
