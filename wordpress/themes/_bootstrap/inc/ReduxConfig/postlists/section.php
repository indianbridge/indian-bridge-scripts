<?php
/*
 * The options in the post list pages like archives, category, tag, author, search pages.
 */
if ( !function_exists( '_bootstrap_module_post_lists_options' ) ) {
	function _bootstrap_module_post_lists_options( $sections ) {
		$page_name = 'archives';
		$fields = array();
		
		// Layout Options
		_bootstrap_archives_add_layout_options( $fields, $page_name );
		
		// Excerpt Options
		_bootstrap_archives_add_excerpt_options( $fields, $page_name );
	    		

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
    	
    	// Container Styling
		$section_name = __( 'content', '_bootstrap' );
		$prefix = __( 'Content/Excerpt Container', '_bootstrap' );
		
		// Container options
		_bootstrap_add_container_styling_options( $fields, $page_name, $section_name, $prefix );    	
	    
	    
	    // Meta Information Control	    
		$fields[] = array( 
	        'id'       => '_bootstrap_post_lists_archives_raw_meta',
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
	        'title'   => __( 'Meta Information Display Control', '_bootstrap' ),
	        'desc'    => __( 'Organize what meta information you want displayed and in what order on the post lists page', '_bootstrap' ),
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
			'title' => __( 'Post Lists/Archives Page', '_bootstrap' ),
			'icon' => 'el-icon-list icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_post_lists_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_post_lists_options', 50 );


function _bootstrap_archives_add_layout_options( &$fields, $page_name ) {
	$section_name = 'layout';
	// The header for this section
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'header' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Archives/Post Lists Page Layout', '_bootstrap' ) . '</h1>',
    );
    
    $number_of_sidebars = 2;
    $sidebar_prefix = 'sidebar-';
    $enabled['placebo'] = 'placebo';
    $enabled['content'] = __( 'Content', '_bootstrap' );
    if ( $number_of_sidebars > 0 ) {
		$enabled[ $sidebar_prefix . '1' ] = __( 'Sidebar 1', '_bootstrap' );
	}
    $disabled['placebo'] = 'placebo';
    for ( $i = 2; $i <= $number_of_sidebars; $i++ ) {
		$disabled[ $sidebar_prefix . $i ] = sprintf( __( 'Sidebar %d', '_bootstrap' ), $i );
	}
    // Content sidebar layout
	$fields[] = array(
        'id'      => _bootstrap_get_option_name( $page_name, $section_name, 'layout' ),
        'type'    => 'sorter',
        'title'   => __( 'Content/Sidebar Layout', '_bootstrap' ),
        'desc'    => __( 'How do you want to layout the content and sidebars. Top to bottom will represent left to right', '_bootstrap' ),
        'options' => array( 'enabled'  => $enabled, 'disabled' => $disabled ),
	);	
	
	// content width
	$fields[] = array(
        'id' => _bootstrap_get_option_name( $page_name, $section_name, 'content_width' ),
        'type' => 'slider',
        'title' => __( 'Content Width', '_bootstrap' ),
        'desc' => __( 'How many columns of Bootstrap 12 column grid should content occupy. If content is disabled above then this is ignored.', '_bootstrap' ),
        "default" => 8,
        "min" => 0,
        "step" => 1,
        "max" => 12,
        'display_value' => 'select',
    ); 
    
    // sidebar widths
    for( $i = 1; $i <= $number_of_sidebars; $i++ ) {
    	$sidebar_name = $sidebar_prefix . $i;
		$fields[] = array(
	        'id' => _bootstrap_get_option_name( $page_name, $section_name, $sidebar_name . '_width' ),
	        'type' => 'slider',
	        'title' => sprintf(  __( 'Sidebar %d Column Width', '_bootstrap' ), $i ),
	        'desc' => sprintf( __( 'How many columns of Bootstrap 12 column grid should content occupy. If Sidebar %d is disabled above then this is ignored', '_bootstrap' ), $i ),
	        "default" => 4,
	        "min" => 0,
	        "step" => 1,
	        "max" => 12,
	        'display_value' => 'select',
	    );			
	}
}

function _bootstrap_archives_add_excerpt_options( &$fields, $page_name ) {
	$section_name = 'excerpt';
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'header' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Excerpt Options', '_bootstrap' ) . '</h1>',
    );	
   
    // Content or Excerpt	
    $use_expert_field = _bootstrap_get_option_name( $page_name, $section_name, 'use_excerpt' );
 	$fields[] = array(
        'id'		=> $use_expert_field,
        'type'      => 'switch',
        'title'     => __( 'Show Excerpt or Content in Archives/Post Lists', '_bootstrap' ),
        'desc'      => __( 'Controls what is shown in the body of single post in the archives/list of posts.', '_bootstrap' ),
        'on'		=> __( 'Excerpt', '_bootstrap' ),
        'off'		=> __( 'Content', '_bootstrap' ),
        'default'   => TRUE,
    );	   
    
    // Excerpt length
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'length' ),
        'type'     => 'text',
        'required' => array( $use_expert_field, 'equals', TRUE ),
        'validate' => 'numeric',
        'title'    => __( 'Excerpt Length', '_bootstrap' ),
        'desc'     => __( 'What should the excerpt length be?', '_bootstrap' ),
        'default'  => '55',
    );	
    
	// Read more text
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'read_more_text' ),
        'type'     => 'text',
        'required' => array( $use_expert_field, 'equals', TRUE ),
        'title'    => __( 'Read More Text', '_bootstrap' ),
        'desc'     => __( 'When excerpts are shown, what text should link to full content page.', '_bootstrap' ),
        'default'  => __( 'Read More', '_bootstrap' ),
    );	
    // What html tag to surround the read more text
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'read_more_text_container_style' ),
        'type'     => 'button_set',
        'required' => array( $use_expert_field, 'equals', TRUE ),
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
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'read_more_text_container_class' ),
        'type'     => 'button_set',
        'title'    => __( 'Read More Text Container Class', '_bootstrap' ),
        'required' => array( $use_expert_field, 'equals', TRUE ),
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
}	