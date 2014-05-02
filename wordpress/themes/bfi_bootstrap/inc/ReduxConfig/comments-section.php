<?php
function bfi_bootstrap_module_comments_options( $sections ) {
	
	$fields = array();	
	$page_name = 'comments';
    $title = __( 'Comments', 'bfi_bootstrap' );
	bfi_bootstrap_get_container_config( &$fields, $page_name, $title );	    
	$section_name = 'single-comment';
	$title = __( 'Single Comment', 'bfi_bootstrap' );
	bfi_bootstrap_get_container_config( &$fields, $page_name + '-' + $section_name, $title );	 
	$section = array(
		'title' => __( 'Comments Section Options', 'bfi_bootstrap' ),
		'icon' => 'fa fa-comments-o'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_comments_options', 50 );
?>