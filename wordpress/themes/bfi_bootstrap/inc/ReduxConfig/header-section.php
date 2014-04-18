<?php
function bfi_bootstrap_module_header_options( $sections ) {
	$fields = array();	
	
	$title = 'Header';
// Default or Inverse navbar.
 	$fields[] = array(
        'id'       => 'header-style',
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
 	$fields[] = array(
        'id'       => 'header-alignment',
        'type'     => 'button_set',
        'title'    => $title . __( 'Alignment', 'bfi_bootstrap' ),
        'desc'     => __( 'Static navbar stays on top of page. Fixed navbar moves and stays on top of screen even as you scroll. Applies class navbar-static and navabar-fixed.', 'bfi_bootstrap' ),
        'options'  => array(
            'static'	=> 'Static',
            'fixed'		=> 'Fixed',
        ), 
        'default'  => 'fixed',
    );	  	
	
	// Skins with screenshots
    $fields[] = array(
	    'id'       => 'header-image',
	    'type'     => 'media', 
	    'title'    => __( 'Header Image', 'bfi_bootstrap' ),
	    'desc'     => __( 'The header image to be displayed in header bar', 'bfi_bootstrap' ),
	);  

	$section = array(
		'title' => __( 'Header Options', 'bfi_bootstrap' ),
		'icon' => 'fa fa-hand-o-up'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_header_options', 50 );
?>