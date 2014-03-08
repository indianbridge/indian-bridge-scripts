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
		$page_name = 'style';
		$section_name = 'skins';
		
		// Load the bootstrap style based on options
		$skin = _bootstrap_get_redux_option( $page_name, $section_name, 'skin' );
		$custom_skin_name = 'Custom_Internal';
		if ( $skin === $custom_skin_name ) {
			$stylesheet = _bootstrap_get_redux_option( $page_name, $section_name, 'bootstrap_custom_css_location' );
		}
		else {
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
		
		// Add the selected bootstrap css file
		wp_enqueue_style( '_bootstrap-css', $stylesheet );
		
		// Add the padding to body if static navbar will be used
		$page_name = 'header';
		$options = _bootstrap_get_navbar_options( $page_name );
		if ( $options['navbar_style'] === 'fixed' ) {
			if ( $options['navbar_padding_style'] === 'constant' ) {
				// Set a constant padding as set in option
				$custom_css = 'body {  padding-top: ' . $options['navbar_padding_constant'] . 'px; }';
				wp_add_inline_style( '_bootstrap-css', $custom_css );
			}
			else {
				// Use jquery to compute height of header and set padding accordingly
				wp_enqueue_script( '_bootstrap_fixed_top_navbar_js', THEME_DIR_URI . '/js/jquery.bootstrap.fixed.top.navbar.js', array('jquery'), '20140219', true );
			}
		}
		
		$page_name = 'footer';
		$options = _bootstrap_get_navbar_options( $page_name );
		if ( $options['navbar_style'] === 'fixed' ) {
			if ( $options['navbar_padding_style'] === 'constant' ) {
				// Set a constant padding as set in option
				$custom_css = 'body {  padding-bottom: ' . $options['navbar_padding_constant'] . 'px; }';
				wp_add_inline_style( '_bootstrap-css', $custom_css );
			}
			else {
				// Use jquery to compute height of header and set padding accordingly
				wp_enqueue_script( '_bootstrap_fixed_bottom_navbar_js', THEME_DIR_URI . '/js/jquery.bootstrap.fixed.bottom.navbar.js', array('jquery'), '20140219', true );
			}
		}		
		
		// Enqueue Font awesome is requested
		$section_name = 'font_awesome_css';
		$fa_location =  _bootstrap_get_redux_option( $page_name, $section_name, 'font_awesome_css' );
		if ( $fa_location !== 'no' ) {
			$use_cdn = ( $fa_location === 'cdn' );
			$fa_css = $use_cdn ? _bootstrap_get_redux_option( $page_name, $section_name, 'font_awesome_css_cdn_location' ) : THEME_DIR_URI . '/css/font-awesome-4.0.3/css/font-awesome.min.css';
			wp_enqueue_style( '_bootstrap_font_awesome_css', $fa_css );
		}
		
		// Smartmenus bootstrap addon css
		wp_enqueue_style( '_bootstrap_smartmenus_addon_css', THEME_DIR_URI . '/css/jquery.smartmenus.bootstrap.css' );
		
		// Enqueue bootstrap js if requested
		$section_name = 'bootstrap_js';
		$bootstrap_js_location = _bootstrap_get_redux_option( $page_name, $section_name, 'bootstrap_js' );
		if ( $bootstrap_js_location !== 'no' ){
			$use_cdn = ( $bootstrap_js_location === 'cdn' );
			$bootstrap_js = $use_cdn ? _bootstrap_get_redux_option( $page_name, $section_name, 'bootstrap_js_cdn_location' ) : THEME_DIR_URI . '/js/bootstrap.min.js';
			wp_enqueue_script( '_bootstrap_js', $bootstrap_js, array('jquery'), '20140219', true );
		}

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
