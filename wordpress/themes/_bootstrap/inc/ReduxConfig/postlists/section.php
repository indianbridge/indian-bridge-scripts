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
	    		
	    // Feature Image
	    $prefix = __( 'Archives/Post Lists', '_bootstrap' );
	    _bootstrap_archives_add_featured_image_options( $fields, $page_name, $prefix );

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
	    
	    // Meta information
	    _bootstrap_archives_add_meta_options( $fields, $page_name );

  
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

/**
* Add layout related options on archives page.
* @param array $fields reference to array of options to which the new options will be added.
* @param string $page_name the name of the page used in forming the option id.
* 
* @return nothing
*/
function _bootstrap_archives_add_layout_options( &$fields, $page_name ) {
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

/**
* Add excerpt related options.
* 
* @param array $fields reference to array of options to which the new options will be added.
* @param string $page_name the name of the page used in forming the option id.
* 
* @return nothing
*/
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

/**
* Add featured image related options on archives page.
* 
* @param array $fields reference to array of options to which the new options will be added.
* @param string $page_name the name of the page used in forming the option id.
* 
* @return nothing
*/
function _bootstrap_archives_add_featured_image_options( &$fields, $page_name, $prefix ) {	
	$section_name = 'featured_image';
	
	// Title
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . $prefix . __( ' Featured Image Options', '_bootstrap' ) . '</h1>',
    );		    
    
    $show_fi_field = _bootstrap_get_option_name( $page_name, $section_name, 'show' );
    // Show Featured Image or Not
	$fields[] = array(
        'id'       => $show_fi_field,
        'type'     => 'switch', 
        'title'    => __( 'Show Feature Image?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the featured image is shown or not', '_bootstrap' ),
        'on'		=> __( 'Show Featured Image', '_bootstrap' ),
        'off'		=> __( 'Don\'t Show Featured Image', '_bootstrap' ),
        'default'  => TRUE,
    );	 
    
    // What to do if no featured image
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'place_holder' ),
        'type'     => 'text',
        'required' => array( $show_fi_field, 'equals', TRUE ),
        'title'    => __( 'What should happen if there is no featured image', '_bootstrap' ),
        'desc'     => __( 'Specify the path to a place holder image. Leave blank if you dont want to show anything. You can used $width and $height as placebolders for the size selected in the following fields.', '_bootstrap' ),
        'default'  => 'http://placehold.it/$widthx$height&text=No+Featured+Image',
    );	    
    
    // Location
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'location' ),
        'type'     => 'button_set',
        'required' => array( $show_fi_field, 'equals', TRUE ),
        'title'    => __( 'Location of Featured Image', '_bootstrap' ),
        'desc'     => __( 'Where should the Feature Image be placed?', '_bootstrap' ),
        'options'  => array(
        	'left'			=> 'Left of Text',
            'right' 		=> 'Right of Text',
            'above-left' 	=> 'Above Text to the Left',
            'above-center' 	=> 'Above Text Centered',
            'above-right' 	=> 'Above Text to the Right',
        ), 
        'default'  => 'left',
    );    
    
    // Bootstrap class
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'class' ),
        'type'     => 'button_set',
        'required' => array( $show_fi_field, 'equals', TRUE ),
        'title'    => __( 'Bootstrap Class of Image', '_bootstrap' ),
        'desc'     => __( 'What bootstrap class should be applied to the img tag.', '_bootstrap' ),
        'options'  => array(
        	'none'			=> 'None',
            'img-rounded' 	=> 'Rounded',
            'img-circle' 	=> 'Circle',
            'img-thumbnail' => 'Thumbnail',
        ), 
        'default'  => 'img-rounded',
    );      
    
    // The size to show
    // First get all built in and user defined sizes
    global $_wp_additional_image_sizes;
    $default = FALSE;
	foreach (get_intermediate_image_sizes() as $s) {

		if (isset($_wp_additional_image_sizes[$s])) {
			$width = intval($_wp_additional_image_sizes[$s]['width']);
			$height = intval($_wp_additional_image_sizes[$s]['height']);
		} else {
			$width = get_option($s.'_size_w');
			$height = get_option($s.'_size_h');
		}
		$option_name = $s . '~' . $width . '~' . $height;
		if ( ! $default ) {
			$default = $option_name;
		}		
		$options[ $option_name ] = $s . ' (' . $width . ' x ' . $height . ')';
	}   
	// Add option for specifying custom dimensions 
	$options['custom'] = 'Custom - Specify Dimensions Below';
	// Populate the select field
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'size' ),
        'type'     => 'select',
        'required' => array( $show_fi_field, 'equals', TRUE ),
		'title'    => __( 'Which size image to use.', '_bootstrap' ),
        'desc'     => __( 'Select one of the predefined sizes. This will be argument that is passed to the_post_thumbail', '_bootstrap' ),        
        'options'  => $options,
        'default'  => $default,
    );    
    
    // Custom Dimensions
$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'custom_dimensions' ),
        'type'     => 'dimensions',
        'required' => array( $show_fi_field, 'equals', TRUE ),
        'units'    => FALSE,
        'title'    => __( 'Custom Dimensions for Image', '_bootstrap' ),
        'desc'     => __( 'If you have chose Custom size above then use this to specify the custom width and height for featured image.', '_bootstrap' ),
        'default'  => array(
            'width'   => '100', 
            'height'  => '100',
            'units'	  => 'px',
        ),
);
}

/**
* Add post meta (dates, author, categories, tags etc.) related options on archives page.
* 
* @param array $fields reference to array of options to which the new options will be added.
* @param string $page_name the name of the page used in forming the option id.
* 
* @return nothing
*/
function _bootstrap_archives_add_meta_options( &$fields, $page_name ) {	
	$section_name = 'meta';
	
    // Section Title   
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Post Meta Information Styles', '_bootstrap' ) . '</h1>',
    );	
    
    // Show meta or not
	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'show' ),
        'type'     => 'switch', 
        'title'    => __( 'Show Meta Information?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the meta information for post is displayed in archives/post lists or not.', '_bootstrap' ),
        'on'		=> __( 'Show Meta', '_bootstrap' ),
        'off'		=> __( 'Don\'t Show Meta', '_bootstrap' ),
        'default'  => TRUE,
    );
    
    // Location of meta
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'location' ),
        'type'     => 'button_set',
        'title'    => __( 'Location of Meta Information', '_bootstrap' ),
        'desc'     => __( 'Where should the meta information be displayed.', '_bootstrap' ),
		'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
        'options'  => array(
            'top' 		=> __( 'Top (below title)', '_bootstrap' ),
            'bottom' 	=> __( 'Bottom', '_bootstrap'),
        ), 
        'default'  => 'bottom',      
    );
    
    // Which items to show and in what order	    
	$fields[] = array(
        'id'      => _bootstrap_get_option_name( $page_name, $section_name, 'items' ),
        'type'    => 'sorter',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
        'title'   => __( 'Meta Information Enable and Order', '_bootstrap' ),
        'desc'    => __( 'Organize what meta information you want displayed and in what order on the archives/post lists page', '_bootstrap' ),
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
		    
    // How should items be displayed
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ),
        'type'     => 'button_set',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
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
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'container_class' ),
        'type'     => 'button_set',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
        'title'    => __( 'Post Meta Information Container Class', '_bootstrap' ),
        'desc'     => __( 'What class should be applied to the Post Meta Information container.', '_bootstrap' ),
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
}