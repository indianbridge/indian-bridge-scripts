<?php
/*
 * The footer options
 */

function _bootstrap_module_footer_options( $sections ) {
	$fields = array();
	$page_name = 'footer';
	
	$section_name = 'layout';
	$prefix = __( 'Footer', '_bootstrap' );
	$number_of_sidebars = 3;
	$area = 'Footer';
	for( $i = 1; $i <= $number_of_sidebars; $i++ ) {
		$id = sprintf( '%s-sidebar-%d', strtolower( $area ), $i );
		$name = sprintf( __( 'Sidebar %d', '_bootstrap' ), $i );
		$items[] = array(
			'id'		=> $id,
			'name'		=> $name,
			'enabled'	=> TRUE,
			'width'		=> 4,
		);			
	}
	_bootstrap_add_sidebar_options( $fields, $page_name, $section_name, $prefix, $items );	         	    

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