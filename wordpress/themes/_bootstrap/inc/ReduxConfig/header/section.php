<?php
/*
 * The header options for the _bootstrap theme
 */

function _bootstrap_module_header_options( $sections ) {
	$fields = array();
	$page_name = 'header';
	
	$section_name = 'layout';
	// The title for this section
	$fields[] = array( 
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
	    'type'     => 'raw',
	    'content'  => '<h1>' . __( 'Header Layout', '_bootstrap' ) . '</h1>',
	);	

	// Fixed or Full Width
	$fields[] = array(
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'width' ),
	    'type'     => 'button_set',
	    'title'    => __( 'Fixed or Full Width', '_bootstrap' ),
	    'desc'     => __( 'Fixed uses container class, full uses container-fluid class and none does not apply either of these classes', '_bootstrap' ),
	    'options'  => array(
	    	'container'			=> 'Fixed Width',
	    	'container-fluid'	=> 'Full Width',
	    	'container-none'	=> 'None',
	    ),                
	    'default'  => 'container-none',
	);	
	
    $prefix = __( 'Header', '_bootstrap' );
	// Container options
	_bootstrap_add_container_styling_options( $fields, $page_name, $section_name, $prefix, 'none', FALSE );    	
	
	// Surrounding Box		
	
	$section_name = 'navbar';
	// The title for this section
	$fields[] = array( 
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
	    'type'     => 'raw',
	    'content'  => '<h1>' . __( 'Navbar Options', '_bootstrap' ) . '</h1>',
	);	
	
    // Default or Inverse navbar.
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'color' ),
        'type'     => 'button_set',
        'title'    => __( 'Use Default or Inverse Navbar?', '_bootstrap' ),
        'desc'     => __( 'Choose the styling for navbar.', '_bootstrap' ),
        'options'  => array(
            'default'	=> 'Default',
            'inverse'	=> 'Inverse',
        ), 
        'default'  => 'default',
    );		    
    
    // Static or Fixed navbar.
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'style' ),
        'type'     => 'button_set',
        'title'    => __( 'Use Static or Fixed Navbar?', '_bootstrap' ),
        'desc'     => __( 'Fixed keeps the navbar on screen as you scroll.', '_bootstrap' ),
        'options'  => array(
            'static'	=> 'Static',
            'fixed'		=> 'Fixed',
        ), 
        'default'  => 'static',
    );	  
    
	// What type of padding to use on top for static navbar - constant or dynamic using jquery.
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'padding_style' ),
        'type'     => 'button_set',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'style' ), 'equals', 'fixed' ),
        'title'    => __( 'Constant or Dynamic Padding for Fixed Navbar', '_bootstrap' ),
        'desc'     => __( 'Fixed navbar needs some padding at top so that content does overlap under the navbar. Should we use a constant padding (use text field below to specify) or dynamic (javascript code will be used).', '_bootstrap' ),
        'options'  => array(
            'constant'	=> 'Constant',
            'dynamic'	=> 'Dynamic',
        ), 
        'default'  => 'dynamic',
    );	    
    
    // The javascript file that calculates height of header and sets dynamic padding
    // This will assume that header has id masthead
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'padding_constant' ),
        'type'     => 'text',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'style' ), 'equals', 'fixed' ),
        'validate' => 'numeric',
        'title'    => __( 'How much padding to use on top', '_bootstrap' ),
        'desc'     => __( 'This will add a constant padding on top for body when using Fixed navbar', '_bootstrap' ),
        'default'  => '60',
    );		         	    

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