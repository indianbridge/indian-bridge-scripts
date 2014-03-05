<?php
/*
 * The sidebar options for the _bootstrap theme
 */
if ( !function_exists( '_bootstrap_module_sidebar_options' ) ) {
	function _bootstrap_module_sidebar_options( $sections ) {
		$fields = array();
		
		$fields[] = array( 
	        'id'       => '_bootstrap_sidebar_raw_styling',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Sidebar/Widget Container and Text Styling', '_bootstrap' ) . '</h1>',
	    );	
	    
	    // What type of styling for sidebar/widget title.
	 	$fields[] = array(
	        'id'       => '_bootstrap_sidebar_title_style',
	        'type'     => 'button_set',
	        'title'    => __( 'Sidebar/Widget Title Style', '_bootstrap' ),
	        'desc'     => __( 'How should the Sidebar/Widget Title be styled. If you choose custom enter tag name in the text field that appears.', '_bootstrap' ),
	        'options'  => array(
	            'h1' 		=> 'h1',
	            'h2' 		=> 'h2',
	            'h3' 		=> 'h3',
	            'h4' 		=> 'h4',
	            'h5' 		=> 'h5',
	            'h6' 		=> 'h6',
	            'custom'	=> 'Custom',
	        ), 
	        'default'  => 'h3',
	    );	   
	    // Custom tag for title
	    $fields[] = array(
	        'id'       => '_bootstrap_sidebar_title_custom',
	        'type'     => 'text',
	        'required' => array( '_bootstrap_sidebar_title_style', 'equals', 'custom' ),
	        'title'    => __( 'Custom tag for Sidebar/Widget Title', '_bootstrap'),
	        'desc'     => __( 'This will be used only when custom option is selected above.', '_bootstrap' ),
	        'default'  => 'div'
	    );	     
	    	
	    // What type of container should the content/excerpt be in.
	 	$fields[] = array(
	        'id'       => '_bootstrap_sidebar_style',
	        'type'     => 'button_set',
	        'title'    => __( 'Sidebar/Widget Container Style', '_bootstrap' ),
	        'desc'     => __( 'How should the Sidebar/Widget Container be styled', '_bootstrap' ),
	        'options'  => array(
	            'plain' 	=> 'Plain Text',
	            'panel' 	=> 'Bootstrap Panel',
	            'well' 		=> 'Bootstrap Well',
	            'alert'		=> 'Bootstrap Alert',
	        ), 
	        'default'  => 'panel',
	    );	
	        	   
	    // What class to apply to the container when panel is used.
	 	$fields[] = array(
	        'id'       => '_bootstrap_sidebar_class_panel',
	        'type'     => 'button_set',
	        'title'    => __( 'Bootstrap Panel Styling', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the Panel.', '_bootstrap' ),
	        'required' => array( '_bootstrap_sidebar_style', 'equals', 'panel' ),
	        'options'  => array(
	            'default' 	=> 'Default',
	            'primary' 	=> 'Primary',
	            'success' 	=> 'Success',
	            'info' 		=> 'Info',
	            'warning' 	=> 'Warning',
	            'danger' 	=> 'Danger',
	            'none'		=> 'None',
	        ), 
	        'default'  => 'primary',      
	    );	 
	    
	    // What class to apply to the container when alert is used.
	 	$fields[] = array(
	        'id'       => '_bootstrap_sidebar_class_alert',
	        'type'     => 'button_set',
	        'title'    => __( 'Bootstrap Alert Styling', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the Alert.', '_bootstrap' ),
	        'required' => array( '_bootstrap_sidebar_style', 'equals', 'alert' ),       
	        'options'  => array(
	            'success' 	=> 'Success',
	            'info' 		=> 'Info',
	            'warning' 	=> 'Warning',
	            'danger' 	=> 'Danger',
	            'none'		=> 'None',
	        ), 
	        'default'  => 'info',      
	    );
	    
	    // What class to apply to the  text when plain text or well is used.
	 	$fields[] = array(
	        'id'       => '_bootstrap_sidebar_class_text',
	        'type'     => 'button_set',
	        'title'    => __( 'Text Styling for Well or Plain Text option', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the text when Well or Plain Text is used.', '_bootstrap' ),
	        'required' => array( '_bootstrap_sidebar_style', 'equals', array( 'well', 'plain' ) ),       
	        'options'  => array(
	            'muted' 	=> 'Muted',
	            'primary' 	=> 'Primary',
	            'success' 	=> 'Success',
	            'info' 		=> 'Info',
	            'warning' 	=> 'Warning',
	            'danger' 	=> 'Danger',
	            'none'		=> 'None',
	        ),
	        'default'  => 'primary',      
	    );	    		         	    

		$section = array(
			'title' => __( 'Sidebar/Widgets', '_bootstrap' ),
			'icon' => 'el-icon-home icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_sidebar_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_sidebar_options', 50 );