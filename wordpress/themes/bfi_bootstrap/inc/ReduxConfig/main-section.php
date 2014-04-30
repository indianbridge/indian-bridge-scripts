<?php
function bfi_bootstrap_module_main_options( $sections ) {
	
	$fields = array();	
	$page_name = 'main';
	$section_name = 'skins';
	// Skins with screenshots
	$option_name = 'bootswatch';
    $fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'image_select',
        'title'    => 'Choose your Bootstrap skin', 
        'desc'     => 'Select the bootswatch you want to use',
        'options'  => bfi_bootstrap_get_local_skin_list( $custom_skin_name ),
        'default' => 'Default'
    );
    
   	// Fixed or Full Width
   	$section_name = 'width';
	$fields[] = array(
	    'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'width' ),
	    'type'     => 'button_set',
	    'title'    => __( 'Page Container Width', '_bootstrap' ),
	    'desc'     => __( 'Fixed Width uses Bootstrap container class and Full Width uses Bootstrap container-fluid class.', '_bootstrap' ),
	    'options'  => array(
	    	'container'			=> 'Fixed Width',
	    	'container-fluid'	=> 'Full Width',
	    ),                
	    'default'  => 'container',
	);	

	$section = array(
		'title' => __( 'Main Options', 'bfi_bootstrap' ),
		'icon' => 'fa fa-cog'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_main_options', 50 );
?>