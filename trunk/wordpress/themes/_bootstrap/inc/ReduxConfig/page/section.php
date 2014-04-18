<?php
/*
 * The options in the pages.
 */
if ( !function_exists( '_bootstrap_module_page_options' ) ) {
	function _bootstrap_module_page_options( $sections ) {
		$page_name = 'page';
		$fields = array();
		
		// Archives Area Container Options
		$properties = array (
			'page_name' 	=> $page_name,
			'name'			=> 'Page Content',
			'default_width'	=> 'container-fluid',
		); 
		_bootstrap_add_area_container_options( $fields, $properties );			
		
		// Layout Options
		_bootstrap_page_add_layout_options( $fields, $page_name );		

    	// Container Styling
		$section_name = 'content';
		$prefix = __( 'Content/Excerpt Container', '_bootstrap' );
		
		// Container options
		$properties = array (
			'page_name' 	=> $page_name,
			'section_name'	=> $section_name,
			'name'			=> $prefix,
			'default_container' => 'panel',
			'include_title' => TRUE,
		);		
		_bootstrap_add_container_styling_options( $fields, $properties );    			  	    	           	       

		$section = array(
			'title' => __( 'Page Styling', '_bootstrap' ),
			'icon' => 'el-icon-file icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_page_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_page_options', 50 );

/**
* Add layout related options on pages.
* @param array $fields reference to array of options to which the new options will be added.
* @param string $page_name the name of the page used in forming the option id.
* 
* @return nothing
*/
function _bootstrap_page_add_layout_options( &$fields, $page_name ) {
	$section_name = 'layout';
	$prefix = __( 'Content Area Widgets/Sidebars', '_bootstrap' );
	$items[] = array(
		'id'		=> 'content',
		'name'		=> __( 'Content', '_bootstrap' ),
		'width'		=> 8,
		'enabled'	=> TRUE,
	);
	$number_of_sidebars = 2;
	$area = 'Content';
	for( $i = 1; $i <= $number_of_sidebars; $i++ ) {
		$id = sprintf( '%s-sidebar-%d', strtolower( $area ), $i );
		$name = sprintf( __( 'Sidebar %d', '_bootstrap' ), $i );
		if ( $i === 1 ) {
			$items[] = array(
				'id'		=> $id,
				'name'		=> $name,
				'enabled'	=> TRUE,
				'width'		=> 4,
			);			
		} 
		else {
			$items[] = array(
				'id'		=> $id,
				'name'		=> $name,
				'enabled'	=> FALSE,
				'width'		=> 4,
			);			
		}
	}
	$properties = array(
		'page_name' 	=> $page_name,
		'section_name'	=> $section_name,
		'name'			=> $prefix,
		'items'			=> $items,
	);
	_bootstrap_add_sidebar_options( $fields, $properties );
}