<?php
/*
 * The header options for the _bootstrap theme
 */

function _bootstrap_module_header_options( $sections ) {
	$fields = array();
	
	// Header container options
	$page_name = 'header';
	$section_name = 'container';
	$prefix = __( 'Header', '_bootstrap' );
	_bootstrap_add_area_container_options( $fields, $page_name, $section_name, $prefix );
	
	
	// Navbar Options
	$properties = array (
		'page_name' 	=> $page_name,
		'name'			=> 'Header',
		'js_location'	=> '/js/jquery.bootstrap.fixed.top.navbar.js',
	); 
	_bootstrap_add_navbar_options( $fields, $properties );

	$section = array(
		'title' => __( 'Header', '_bootstrap' ),
		'icon' => 'fa fa-hand-o-up fa-lg'
	);
	
	$section['fields'] = $fields;

	$section = apply_filters( '_bootstrap_module_header_options_modifier', $section );
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/' . REDUX_OPT_NAME . '/sections', '_bootstrap_module_header_options', 50 );