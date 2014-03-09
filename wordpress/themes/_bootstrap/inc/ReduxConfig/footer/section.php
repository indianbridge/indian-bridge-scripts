<?php
/*
 * The footer options
 */

function _bootstrap_module_footer_options( $sections ) {
	$fields = array();
	
	$page_name = 'footer';
	$section_name = 'widget_area_container';
	$prefix = __( 'Footer Widget', '_bootstrap' );
	// Footer Widget Area Container Options
	$properties = array (
		'page_name' 	=> $page_name,
		'section_name'	=> 'widget_area_container',
		'name'			=> 'Footer Widget',
		'default_width'	=> 'container-fluid',
	); 
	_bootstrap_add_area_container_options( $fields, $properties );    
		
	// Setup the widgets
	$section_name = 'widgets';
	$prefix = __( 'Footer Widgets', '_bootstrap' );
	$number_of_sidebars = 3;
	$area = 'Footer';
	for( $i = 1; $i <= $number_of_sidebars; $i++ ) {
		$id = sprintf( '%s-sidebar-%d', strtolower( $area ), $i );
		$name = sprintf( __( 'Widget %d', '_bootstrap' ), $i );
		$items[] = array(
			'id'		=> $id,
			'name'		=> $name,
			'enabled'	=> TRUE,
			'width'		=> 4,
		);			
	}
	$properties = array(
		'page_name' 	=> $page_name,
		'section_name'	=> $section_name,
		'name'			=> $prefix,
		'items'			=> $items,
	);	
	_bootstrap_add_sidebar_options( $fields, $properties );	 
	
	// Widget Container
	$section_name = 'widget_container';
	$properties = array (
		'page_name' 	=> $page_name,
		'section_name'	=> $section_name,
		'name'			=> $prefix,
		'default_container' => 'alert',
		'include_title' => TRUE,
	);	
	_bootstrap_add_container_styling_options( $fields, $properties );
	
	// Footer Copyright Area Container Options
	$properties = array (
		'page_name' 	=> $page_name,
		'section_name'	=> 'copyright_area_container',
		'name'			=> 'Footer Copyright',
		'default_width'	=> 'none',
	); 
	_bootstrap_add_area_container_options( $fields, $properties );  		
    
    // Navbar Options
	$properties = array (
		'page_name' 	=> $page_name,
		'name'			=> 'Footer',
		'js_location'	=> '/js/jquery.bootstrap.fixed.bottom.navbar.js',
	);     	
	_bootstrap_add_navbar_options( $fields, $properties );
	
	$section_name = 'copyright_area';	
	
	// left
	$html = '<a href="http://wordpress.org/" rel="generator">' . sprintf( __( 'Proudly powered by %s', '_bootstrap' ), 'WordPress' ). '</a>';
    $fields[] = array(
        'id'			=> _bootstrap_get_option_name( $page_name, $section_name, 'left' ),
        'type' 			=> 'textarea',
        'title' 		=> __( 'Copyright Area Left', '_bootstrap' ), 
        'desc' 			=> __( 'Enter content for left. HTML is allowed.', '_bootstrap'),
        'validate' 		=> 'html',
        'default' 		=> $html,
    );
    
	// right
	$html = __( 'Theme: ', '_bootstrap' ) . '_bootstrap ' . __( 'by', '_bootstrap' ) . ' Sriram Narasimhan';
    $fields[] = array(
        'id'			=> _bootstrap_get_option_name( $page_name, $section_name, 'right' ),
        'type' 			=> 'textarea',
        'title' 		=> __( 'Copyright Area Right', '_bootstrap' ), 
        'desc' 			=> __( 'Enter content for right. HTML is allowed.', '_bootstrap'),
        'validate' 		=> 'html',
        'default' 		=> $html,
    );    
	     	   
	$section = array(
		'title' => __( 'Footer', '_bootstrap' ),
		'icon' => 'fa fa-hand-o-down fa-lg'
	);
	
	$section['fields'] = $fields;

	$section = apply_filters( '_bootstrap_module_footer_options_modifier', $section );
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/' . REDUX_OPT_NAME . '/sections', '_bootstrap_module_footer_options', 50 );