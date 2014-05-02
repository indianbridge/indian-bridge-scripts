<?php
function bfi_bootstrap_module_footer_options( $sections ) {
	$fields = array();		  
    $page_name = 'footer';
    $section_name = 'widgets';
    $title = __( 'Footer', 'bfi_bootstrap' );   
	bfi_bootstrap_get_container_config( &$fields, $page_name, $title );	
	$title = __('Footer Widgets', 'bfi_bootstrap' );
	$fields[] = array( 
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . $title . '</h1>',
    );	
	$option_name = 'show';
	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'switch', 
        'title'    => __( 'Show ', 'bfi_bootstrap' ). $title . __( 'Information?', 'bfi_bootstrap' ),
        'desc'     => __( 'This controls whether the ', 'bfi_bootstrap' ). $title . __( 'information for post is displayed in archives/post lists or not.', 'bfi_bootstrap' ),
        'on'		=> __( 'Show ', 'bfi_bootstrap' ). $title . __( '', 'bfi_bootstrap' ),
        'off'		=> __( 'Don\'t Show ', 'bfi_bootstrap' ). $title . __( '', 'bfi_bootstrap' ),
        'default'  => TRUE,
    );	    
	bfi_bootstrap_get_container_config( &$fields, $page_name . '-' .$section_name, $title );	 

	$section_name = 'copyright-area';
	$title = __('Footer Copyright Navbar', 'bfi_bootstrap' );
	$fields[] = array( 
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . $title . '</h1>',
    );	
	
	$option_name = 'show';
	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'switch', 
        'title'    => __( 'Show ', 'bfi_bootstrap' ). $title . __( 'Information?', 'bfi_bootstrap' ),
        'desc'     => __( 'This controls whether the ', 'bfi_bootstrap' ). $title . __( 'information for post is displayed in archives/post lists or not.', 'bfi_bootstrap' ),
        'on'		=> __( 'Show ', 'bfi_bootstrap' ). $title . __( '', 'bfi_bootstrap' ),
        'off'		=> __( 'Don\'t Show ', 'bfi_bootstrap' ). $title . __( '', 'bfi_bootstrap' ),
        'default'  => TRUE,
    );	
	// Default or Inverse navbar.
	$option_name = 'style';
 	$fields[] = array(
        'id'       =>  bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => $title . __( ' Style', 'bfi_bootstrap' ),
        'desc'     => __( 'Choose the Bootstrap style for the ', 'bfi_bootstrap' ) . $title . __( '. This will apply bootstrap classes navbar-default or navbar-inverse.', 'bfi_bootstrap' ),
        'options'  => array(
            'default'	=> 'Default',
            'inverse'	=> 'Inverse',
        ), 
        'default'  => 'default',
    );		    
    
    // Static or Fixed navbar. 
    $option_name = 'alignment';
 	$fields[] = array(
        'id'       =>  bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => $title . __( 'Alignment', 'bfi_bootstrap' ),
        'desc'     => __( 'Static navbar stays on top of page. Fixed navbar moves and stays on top of screen even as you scroll. Applies class navbar-static and navabar-fixed.', 'bfi_bootstrap' ),
        'options'  => array(
            'static'	=> 'Static',
            'fixed'		=> 'Fixed',
        ), 
        'default'  => 'fixed',
    );	  		
	// left
	$html = '<a href="http://wordpress.org/" rel="generator">' . sprintf( __( 'Proudly powered by %s', '_bootstrap' ), 'WordPress' ). '</a>';
    $fields[] = array(
        'id'			=> bfi_bootstrap_get_option_name( $page_name, $section_name, 'left' ),
        'type' 			=> 'textarea',
        'title' 		=> __( 'Copyright Area Left', '_bootstrap' ), 
        'desc' 			=> __( 'Enter content for left. HTML is allowed.', '_bootstrap'),
        'validate' 		=> 'html',
        'default' 		=> $html,
    );
    
	// right
	$html = __( 'Theme: ', '_bootstrap' ) . '_bootstrap ' . __( 'by', '_bootstrap' ) . ' Sriram Narasimhan';
    $fields[] = array(
        'id'			=> bfi_bootstrap_get_option_name( $page_name, $section_name, 'right' ),
        'type' 			=> 'textarea',
        'title' 		=> __( 'Copyright Area Right', '_bootstrap' ), 
        'desc' 			=> __( 'Enter content for right. HTML is allowed.', '_bootstrap'),
        'validate' 		=> 'html',
        'default' 		=> $html,
    ); 	  
	 
    
	$title = __( 'Footer', 'bfi_bootstrap' );   
	$section = array(
		'title' => $title . __( ' Options', 'bfi_bootstrap' ),
		'icon' => 'fa fa-hand-o-down'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_footer_options', 50 );
?>