<?php
/*
 * The sidebar options for the _bootstrap theme
 */
if ( !function_exists( '_bootstrap_module_sidebar_options' ) ) {
	function _bootstrap_module_sidebar_options( $sections ) {
		$fields = array();
		$page_name = 'sidebar';
		$section_name = 'sidebar';
		$prefix = __( 'Content Area Widgets/Sidebars', '_bootstrap' );
		
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
			'title' => __( 'Sidebar/Widgets', '_bootstrap' ),
			'icon' => 'el-icon-indent-right icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_sidebar_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_sidebar_options', 50 );
