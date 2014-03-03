<?php
/*
 * The options in the post list pages like archives, category, tag, author, search pages.
 */
if ( !function_exists( '_bootstrap_module_post_lists_options' ) ) {
	function _bootstrap_module_post_lists_options( $sections ) {
		$fields = array();
		$fields[] = array( 
	        'id'       => '_bootstrap_post_lists_raw_1',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Content and Excerpt', '_bootstrap' ) . '</h1>',
	    );	
	    // Content or Exceprt	
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_content_or_excerpt',
	        'type'     => 'button_set',
	        'title'    => __( 'Show Excerpt or Content in Post', '_bootstrap' ),
	        'desc'     => __( 'Controls what is shown in the body of single post in the list of posts.', '_bootstrap' ),
	        //Must provide key => value pairs
	        'options'  => array(
	            'excerpt' => 'Excerpt',
	            'content' => 'Content',
	        ), 
	        'default'  => 'excerpt',
	    );	    
		$fields[] = array(
	        'id'       => '_bootstrap_post_lists_show_featured_image',
	        'type'     => 'switch', 
	        'title'    => __( 'Show Feature Image left of content', '_bootstrap' ),
	        'desc'     => __( 'This controls whether the featured image is shown or not', '_bootstrap' ),
	        'default'  => TRUE,
	    );	 	    
		// Read more text
	    $fields[] = array(
	        'id'       => '_bootstrap_post_lists_read_more_text',
	        'type'     => 'text',
	        'title'    => __( 'Read More Text', '_bootstrap' ),
	        'desc'     => __( 'When excerpts are shown, what text should link to full content page.', '_bootstrap' ),
	        'default'  => __( 'Read More', '_bootstrap' ),
	    );	
	    // What html tag to surround the read more text
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_read_more_text_container_style',
	        'type'     => 'button_set',
	        'title'    => __( 'Read More Text Container', '_bootstrap' ),
	        'desc'     => __( 'How should the Read More Text be styled', '_bootstrap' ),

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
	        'id'       => '_bootstrap_post_lists_read_more_text_container_class',
	        'type'     => 'button_set',
	        'title'    => __( 'Read More Text Container Class', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the Read More text Container.', '_bootstrap' ),

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
	        'id'       => '_bootstrap_post_lists_archives_raw_2',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Excerpt/Content Container', '_bootstrap' ) . '</h1>',
	    );		
	    // What html tag to excerpt
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_excerpt_container_style',
	        'type'     => 'button_set',
	        'title'    => __( 'Excerpt Container Style', '_bootstrap' ),
	        'desc'     => __( 'How should the Excerpt/Content Container be styled', '_bootstrap' ),

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
	        'id'       => '_bootstrap_post_lists_excerpt_container_class',
	        'type'     => 'button_set',
	        'title'    => __( 'Excerpt Container Class', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the Excerpt/Content container.', '_bootstrap' ),

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
	        'id'       => '_bootstrap_post_lists_archives_raw_3',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Post Meta Information Styles', '_bootstrap' ) . '</h1>',
	    );		
	    // What html tag to excerpt
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_post_meta_information_container_style',
	        'type'     => 'button_set',
	        'title'    => __( 'Post Meta Information Container Style', '_bootstrap' ),
	        'desc'     => __( 'How should the Post Meta Information items be styled', '_bootstrap' ),

	        //Must provide key => value pairs
	        'options'  => array(
	            'plain' 	=> 'Plain Text',
	            'label' 	=> 'Bootstrap Label',
	            'button' 	=> 'Bootstrap Button',
	        ), 
	        'default'  => 'plain',
	    );	    	   
	    // What class to apply to the html tag for Excerpt container
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_post_meta_information_container_class',
	        'type'     => 'button_set',
	        'title'    => __( 'Post Meta Information Container Class', '_bootstrap' ),
	        'desc'     => __( 'What class should be applied to the Post Meta Information container.', '_bootstrap' ),

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
	        'id'       => '_bootstrap_post_lists_archives_raw_4',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Post Information Items Control', '_bootstrap' ) . '</h1>',
	    );		    
		$fields[] = array(
	        'id'      => '_bootstrap_post_meta_information_control',
	        'type'    => 'sorter',
	        'title'   => __( 'Post Information Display Control', '_bootstrap' ),
	        'desc'    => __( 'Organize what post information you want displayed and in what order on the post lists page', '_bootstrap' ),
	        'options' => array(
	            'enabled'  => array(
	                'placebo'    	=> 'placebo', //REQUIRED!
	                'publish_date' 	=> __( 'Publish Date', '_bootstrap' ),
	                'author'     	=> __( 'Author', '_bootstrap' ),
	                'categories' 	=> __( 'Categories', '_bootstrap' ),
	                'comments'		=> __( 'Comments', '_bootstrap' ),
	            ),
	            'disabled' => array(
	                'placebo'   		=> 'placebo', //REQUIRED!
	                'last_updated_date'	=> __( 'Last Updated Date', '_bootstrap' ),
	                'tags'   			=> __( 'Tags', '_bootstrap' ),
	            )
	        ),
		);	           	    	           	    
	           	    

		$section = array(
			'title' => __( 'Post Lists Options', '_bootstrap' ),
			'icon' => 'el-icon-list icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_post_lists_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_post_lists_options', 50 );