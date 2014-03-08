<?php
/*
 * The footer options
 */

function _bootstrap_module_footer_options( $sections ) {
	$fields = array();
	$page_name = 'footer';
	$section_name = 'widget_area';
	
	// Enable widget area
	// The title for this section
	$fields[] = array( 
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
	    'type'     => 'raw',
	    'content'  => '<h1>' . __( 'Footer Widget Area Enable', '_bootstrap' ) . '</h1>',
	);	
    $show_fi_field = _bootstrap_get_option_name( $page_name, $section_name, 'show' );
    // Show Widget Area or Not
	$fields[] = array(
        'id'       => $show_fi_field,
        'type'     => 'switch', 
        'title'    => __( 'Show Widget Area?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the footer widget area is shown or not', '_bootstrap' ),
        'on'		=> __( 'Show Widget Area', '_bootstrap' ),
        'off'		=> __( 'Don\'t Show Widget Area', '_bootstrap' ),
        'default'  => TRUE,
    );	
    
	$section_name = 'widget_area_container';
	$prefix = __( 'Footer Widget', '_bootstrap' );
	_bootstrap_add_area_container_options( $fields, $page_name, $section_name, $prefix );    
		
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
	_bootstrap_add_sidebar_options( $fields, $page_name, $section_name, $prefix, $items );	 
	
	// Widget Container
	$section_name = 'widget_container';
	_bootstrap_add_container_styling_options( $fields, $page_name, $section_name, $prefix, 'alert' );
	
	$section_name = 'copyright_area';
	// Enable copyright area
	// The title for this section
	$fields[] = array( 
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
	    'type'     => 'raw',
	    'content'  => '<h1>' . __( 'Copyright Area Enable', '_bootstrap' ) . '</h1>',
	);	
    $show_fi_field = _bootstrap_get_option_name( $page_name, $section_name, 'show' );
    // Show Copyright Area or Not
	$fields[] = array(
        'id'       => $show_fi_field,
        'type'     => 'switch', 
        'title'    => __( 'Show Copyright Area?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the footer copyright area is shown or not', '_bootstrap' ),
        'on'		=> __( 'Show Copyright Area', '_bootstrap' ),
        'off'		=> __( 'Don\'t Show Copyright Area', '_bootstrap' ),
        'default'  => TRUE,
    );	
    
    // Navbar Options
	$properties = array (
		'page_name' 	=> $page_name,
		'name'			=> 'Footer',
		'js_location'	=> '/js/jquery.bootstrap.fixed.bottom.navbar.js',
	);     	
	_bootstrap_add_navbar_options( $fields, $properties );
	
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