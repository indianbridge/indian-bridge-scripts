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
	    // Content or Excerpt	
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
	    
		// Read more text
	    $fields[] = array(
	        'id'       => '_bootstrap_post_lists_read_more_text',
	        'type'     => 'text',
	        'required' => array( '_bootstrap_post_lists_content_or_excerpt', 'equals', 'excerpt' ),
	        'title'    => __( 'Read More Text', '_bootstrap' ),
	        'desc'     => __( 'When excerpts are shown, what text should link to full content page.', '_bootstrap' ),
	        'default'  => __( 'Read More', '_bootstrap' ),
	    );	
	    // What html tag to surround the read more text
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_read_more_text_container_style',
	        'type'     => 'button_set',
	        'required' => array( '_bootstrap_post_lists_content_or_excerpt', 'equals', 'excerpt' ),
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
	        'required' => array( '_bootstrap_post_lists_content_or_excerpt', 'equals', 'excerpt' ),
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
	        'id'       => '_bootstrap_post_lists_archives_raw_fi',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Featured Image Options', '_bootstrap' ) . '</h1>',
	    );		    
		$fields[] = array(
	        'id'       => '_bootstrap_post_lists_show_featured_image',
	        'type'     => 'switch', 
	        'title'    => __( 'Show Feature Image left of content', '_bootstrap' ),
	        'desc'     => __( 'This controls whether the featured image is shown or not', '_bootstrap' ),
	        'default'  => TRUE,
	    );	 
	    $fields[] = array(
	        'id'       => '_bootstrap_post_lists_show_featured_image_size',
	        'type'     => 'text',
	        'required' => array( '_bootstrap_post_lists_show_featured_image', 'equals', TRUE ),
	        'title'    => __( 'Which size image to use.', '_bootstrap' ),
	        'desc'     => __( 'Select one of the predefined sizes.', '_bootstrap' ),
	        'default'  => 'thumbnail'
	    );	
		$fields[] = array(
	        'id'       => '_bootstrap_post_lists_show_featured_image_container_size',
	        'type'     => 'switch', 
	        'required' => array( '_bootstrap_post_lists_show_featured_image', 'equals', TRUE ),
	        'title'    => __( 'Should the Featured Image Container be sized?', '_bootstrap' ),
	        'desc'     => __( 'This controls whether the featured image container is sized using dimensions specified below', '_bootstrap' ),
	        'default'  => FALSE,
	    );	    
 		$fields[] = array(
	        'id'       => '_bootstrap_post_lists_show_featured_image_container_dimensions',
	        'type'     => 'dimensions',
	        'required' => array( '_bootstrap_post_lists_show_featured_image', 'equals', TRUE ),
	        'units'    => array( 'em', 'px', '%' ),
	        'title'    => __( 'Dimensions (Width/Height) For Feature Image Container', '_bootstrap' ),
	        'desc'     => __( 'Use this to specify the width and height of img tag that will contain feature image.', '_bootstrap' ),
	        'default'  => array(
	            'Width'   => '100', 
	            'Height'  => '100'
	        ),
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
		$fields[] = array(
	        'id'       => '_bootstrap_post_lists_show_meta_information',
	        'type'     => 'switch', 
	        'title'    => __( 'Show Meta Information', '_bootstrap' ),
	        'desc'     => __( 'This controls whether the meta information for post is displayed in post lists or not.', '_bootstrap' ),
	        'default'  => TRUE,
	    );
	    $fields[] = array(
	        'id'       => '_bootstrap_post_lists_meta_information_location',
	        'type'     => 'button_set',
	        'title'    => __( 'Location of Meta Information', '_bootstrap' ),
	        'desc'     => __( 'Where should the meta information be displayed.', '_bootstrap' ),
			'required' => array( '_bootstrap_post_lists_show_meta_information', 'equals', TRUE ),
	        //Must provide key => value pairs
	        'options'  => array(
	            'top' 		=> __( 'Top (below title)', '_bootstrap' ),
	            'bottom' 	=> __( 'Bottom', '_bootstrap'),
	        ), 
	        'default'  => 'bottom',      
	    );
	    	    
		$fields[] = array(
	        'id'      => '_bootstrap_post_meta_information_control',
	        'type'    => 'sorter',
	        'required' => array( '_bootstrap_post_lists_show_meta_information', 'equals', TRUE ),
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
			    
	    // What style to use for meta information
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_post_meta_information_container_style',
	        'type'     => 'button_set',
	        'required' => array( '_bootstrap_post_lists_show_meta_information', 'equals', TRUE ),
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
	    // What class to apply to the meta information style
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_post_meta_information_container_class',
	        'type'     => 'button_set',
	        'required' => array( '_bootstrap_post_lists_show_meta_information', 'equals', TRUE ),
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
	        'id'       => '_bootstrap_post_lists_archives_raw_paging',
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Paging/Pagination Format', '_bootstrap' ) . '</h1>',
	    );			
		$fields[] = array(
	        'id'       => '_bootstrap_post_lists_show_paging',
	        'type'     => 'switch', 
	        'title'    => __( 'Show Paging/Pagination', '_bootstrap' ),
	        'desc'     => __( 'This controls whether the paging/pagination is displayed at the end of post lists or not.', '_bootstrap' ),
	        'default'  => TRUE,
	    );	    
	    // Paging Style
	 	$fields[] = array(
	        'id'       => '_bootstrap_post_lists_paging_style',
	        'type'     => 'button_set',
	        'title'    => __( 'Post Lists Paging Style', '_bootstrap' ),
	        'desc'     => __( 'What kind of paging style should be applied.', '_bootstrap' ),
	        'required' => array( '_bootstrap_post_lists_show_paging', 'equals', TRUE ),
	        //Must provide key => value pairs
	        'options'  => array(
	            'wp_paging' 			=> 'Wordpress Paging',
	            'wp_pagination' 		=> 'Wordpress Pagination',
	            'bootstrap_paging' 		=> 'Bootstrap Paging',
	            'bootstrap_pagination' 	=> 'Bootstrap Pagination',
	        ), 
	        'default'  => 'bootstrap_pagination',      
	    );		
	    //echo json_encode($fields);         	    	           	    
	           	    

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