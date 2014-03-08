<?php
/*
 * The sidebar options for the _bootstrap theme
 */
if ( !function_exists( '_bootstrap_module_sidebar_options' ) ) {
	function _bootstrap_module_sidebar_options( $sections ) {
		$fields = array();
		$page_name = 'sidebar';
		$section_name = 'sidebar';
		$prefix = __( 'Widgets/Sidebars', '_bootstrap' );
		
		// Container options
		_bootstrap_add_container_styling_options( $fields, $page_name, $section_name, $prefix );
	    		         	    
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
