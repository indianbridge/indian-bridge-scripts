<?php
function bfi_bootstrap_module_main_options( $sections ) {
	$fields = array();	
	
	// Skins with screenshots
    $fields[] = array(
        'id'       => 'bootswatch',
        'type'     => 'image_select',
        'title'    => 'Choose your Bootstrap skin', 
        'desc'     => 'Select the bootswatch you want to use',
        'options'  => bfi_bootstrap_get_local_skin_list( $custom_skin_name ),
        'default' => 'Default'
    );	         	    

	$section = array(
		'title' => __( 'Main Options', '_bootstrap' ),
		'icon' => 'fa fa-cog'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_main_options', 50 );
?>