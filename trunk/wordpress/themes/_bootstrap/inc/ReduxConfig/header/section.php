<?php
/*
 * The header options for the _bootstrap theme
 */
if ( !function_exists( '_bootstrap_module_header_options' ) ) {
	function _bootstrap_module_header_options( $sections ) {
		$fields = array();
		
	    // Default or Inverse navbar.
	 	$fields[] = array(
	        'id'       => '_bootstrap_header_navbar_color',
	        'type'     => 'button_set',
	        'title'    => __( 'Use Default or Inverse Navbar', '_bootstrap' ),
	        'desc'     => __( 'Choose the styling for navbar.', '_bootstrap' ),
	        'options'  => array(
	            'default'	=> 'Default',
	            'inverse'	=> 'Inverse',
	        ), 
	        'default'  => 'default',
	    );		    
	    
	    // Static or Fixed navbar.
	 	$fields[] = array(
	        'id'       => '_bootstrap_header_navbar_style',
	        'type'     => 'button_set',
	        'title'    => __( 'Use Static or Fixed Navbar', '_bootstrap' ),
	        'desc'     => __( 'Fixed keeps the navbar on screen as you scroll.', '_bootstrap' ),
	        'options'  => array(
	            'static'	=> 'Static',
	            'fixed'		=> 'Fixed',
	        ), 
	        'default'  => 'static',
	    );	  
	    
		// What type of padding to use on top for static navbar - constant or dynamic using jquery.
	 	$fields[] = array(
	        'id'       => '_bootstrap_header_fixed_navbar_padding_style',
	        'type'     => 'button_set',
	        'required' => array( '_bootstrap_header_navbar_style', 'equals', 'fixed' ),
	        'title'    => __( 'Constant or Dynamic Padding for Fixed Navbar', '_bootstrap' ),
	        'desc'     => __( 'Fixed navbar needs some padding at top so that content does go under navbar to start with. Should we use a constant padding (use text field below to specify) or dynamic (javascript code will be used).', '_bootstrap' ),
	        'options'  => array(
	            'constant'	=> 'Constant',
	            'dynamic'	=> 'Dynamic',
	        ), 
	        'default'  => 'dynamic',
	    );	    
	    
	    // The javascript file that calculates height of header and sets dynamic padding
	    // This will assume that header has id masthead
	    $fields[] = array(
	        'id'       => '_bootstrap_header_fixed_navbar_padding_constant',
	        'type'     => 'text',
	        'required' => array( '_bootstrap_header_navbar_style', 'equals', 'fixed' ),
	        'validate' => 'numeric',
	        'title'    => __( 'How much padding to use on top', '_bootstrap' ),
	        'desc'     => __( 'This will add a padding on top for body when using Fixed navbar', '_bootstrap' ),
	        'default'  => '60',
	    );		         	    

		$section = array(
			'title' => __( 'Header', '_bootstrap' ),
			'icon' => 'el-icon-home icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_header_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_header_options', 50 );