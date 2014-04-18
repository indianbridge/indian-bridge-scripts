<?php
function bfi_bootstrap_module_content_options( $sections ) {
	$fields = array();		  
    
	bfi_bootstrap_get_container_config( &$fields, 'content' );	    

	$section = array(
		'title' => __( 'Content Container Options', '_bootstrap' ),
		'icon' => 'fa fa-hand-o-left'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_content_options', 50 );
?>