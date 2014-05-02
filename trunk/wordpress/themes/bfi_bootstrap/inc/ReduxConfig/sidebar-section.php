<?php
function bfi_bootstrap_module_sidebar_options( $sections ) {
	$fields = array();		  
    $page_name = 'sidebar';
    $section_name = 'widgets';
    $title = __( 'Sidebar', 'bfi_bootstrap' );
    $option_name = 'location';
    $fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => __( 'Location of Sidebar', 'bfi_bootstrap' ),
        'desc'     => __( 'Where should the sidebar be displayed.', 'bfi_bootstrap' ),
        'options'  => array(
            'left' 		=> __( 'Left of Content', 'bfi_bootstrap' ),
            'right' 	=> __( 'Right of Content', 'bfi_bootstrap'),
        ), 
        'default'  => 'right',      
    );    
	bfi_bootstrap_get_container_config( &$fields, $page_name, $title );	    
	$section = array(
		'title' => $title . __( ' Options', 'bfi_bootstrap' ),
		'icon' => 'fa fa-hand-o-right'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_sidebar_options', 50 );
?>