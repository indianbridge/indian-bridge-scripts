<?php
/*
 * The options in the post list pages like archives, category, tag, author, search pages.
 */
if ( !function_exists( '_bootstrap_module_archives_options' ) ) {
	function _bootstrap_module_archives_options( $sections ) {
		$fields = array();
		$fields[] = array( 
	        'id'       => 'archives-raw-1',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'End of Excerpt Text', '_bootstrap' ) . '</h1>',
	    );		
		// Read more text
	    $fields[] = array(
	        'id'       => 'read_more_text',
	        'type'     => 'text',
	        'title'    => __( 'Read More Text', '_bootstrap' ),
	        'desc'     => __( 'When excerpts are shown what text should link to full content page.', '_bootstrap' ),
	        'default'  => __( 'Read More', '_bootstrap' ),
	    );	
	    // What html tag to surround the read more text
	 	$fields[] = array(
	        'id'       => 'read_more_text_tag',
	        'type'     => 'button_set',
	        'title'    => __( 'Read More Text Tag', '_bootstrap' ),
	        'desc'     => __( 'How should the Read More Text be presented', '_bootstrap' ),

	        //Must provide key => value pairs
	        'options'  => array(
	            'plain' 	=> 'Plain Text',
	            'label' 	=> 'Bootstrap Label',
	            'button' 	=> 'Bootstrap Button',
	        ), 
	        'default'  => 'label',
	    );	    	   
	    // What class to apply to the html tag for Read More text
	 	$fields[] = array(
	        'id'       => 'read_more_text_tag_class',
	        'type'     => 'button_set',
	        'title'    => __( 'Read More Text Class', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the Read More text. Applicable only for labels and buttons', '_bootstrap' ),

	        //Must provide key => value pairs
	        'options'  => array(
	            'default' 	=> 'Default',
	            'primary' 	=> 'Primary',
	            'success' 	=> 'Success',
	            'info' 		=> 'Info',
	            'warning' 	=> 'Warning',
	            'danger' 	=> 'Danger',
	        ), 
	        'default'  => 'primary',      
	    );	 
	    
		$fields[] = array( 
	        'id'       => 'archives-raw-2',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Excerpt Container', '_bootstrap' ) . '</h1>',
	    );		
	    // What html tag to excerpt
	 	$fields[] = array(
	        'id'       => 'excerpt_tag',
	        'type'     => 'button_set',
	        'title'    => __( 'Excerpt Container Tag', '_bootstrap' ),
	        'desc'     => __( 'How should the Excerpt be presented', '_bootstrap' ),

	        //Must provide key => value pairs
	        'options'  => array(
	            'plain' 	=> 'Plain Text',
	            'panel' 	=> 'Bootstrap Panel',
	            'well' 		=> 'Bootstrap Well',
	            'alert'		=> 'Bootstrap Alert',
	        ), 
	        'default'  => 'panel',
	    );	    	   
	    // What class to apply to the html tag for Excerpt container
	 	$fields[] = array(
	        'id'       => 'excerpt_tag_class',
	        'type'     => 'button_set',
	        'title'    => __( 'Excerpt Container Class', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the Excerpt container. Applicable only for panel and alert.', '_bootstrap' ),

	        //Must provide key => value pairs
	        'options'  => array(
	            'default' 	=> 'Default',
	            'primary' 	=> 'Primary',
	            'success' 	=> 'Success',
	            'info' 		=> 'Info',
	            'warning' 	=> 'Warning',
	            'danger' 	=> 'Danger',
	        ), 
	        'default'  => 'primary',      
	    );	        	    
	           	    

		$section = array(
			'title' => __( 'Archives Options', '_bootstrap' ),
			'icon' => 'el-icon-list icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_archives_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_archives_options', 50 );