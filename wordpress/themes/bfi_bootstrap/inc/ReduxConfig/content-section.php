<?php
function bfi_bootstrap_module_content_options( $sections ) {
	$fields = array();		  
    $page_name = 'content';
    $title = __( 'Content', 'bfi_bootstrap' );
	bfi_bootstrap_get_container_config( &$fields, $page_name, $title );	    
	bfi_bootstrap_archives_add_meta_options( &$fields, $page_name );
	$section = array(
		'title' => $title . __( ' Options', 'bfi_bootstrap' ),
		'icon' => 'fa fa-hand-o-left'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_content_options', 50 );


function bfi_bootstrap_archives_add_meta_options( &$fields, $page_name ) {	
	$section_name = 'meta';
	
    // Section Title   
    $option_name = 'title';
	$fields[] = array( 
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Post Meta Information Styles', 'bfi_bootstrap' ) . '</h1>',
    );	
    
    // Show meta or not
    $option_name = 'show';
	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'switch', 
        'title'    => __( 'Show Meta Information?', 'bfi_bootstrap' ),
        'desc'     => __( 'This controls whether the meta information for post is displayed in archives/post lists or not.', 'bfi_bootstrap' ),
        'on'		=> __( 'Show Meta', 'bfi_bootstrap' ),
        'off'		=> __( 'Don\'t Show Meta', 'bfi_bootstrap' ),
        'default'  => TRUE,
    );
    
    // Location of meta
    $option_name = 'location';
    $fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => __( 'Location of Meta Information', 'bfi_bootstrap' ),
        'desc'     => __( 'Where should the meta information be displayed.', 'bfi_bootstrap' ),
		'required' => array( bfi_bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
        'options'  => array(
            'top' 		=> __( 'Top (below title)', 'bfi_bootstrap' ),
            'bottom' 	=> __( 'Bottom', 'bfi_bootstrap'),
        ), 
        'default'  => 'bottom',      
    );
    
    // Which items to show and in what order	
    $option_name = 'items';    
	$fields[] = array(
        'id'      => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'    => 'sorter',
        'required' => array( bfi_bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
        'title'   => __( 'Meta Information Enable and Order', 'bfi_bootstrap' ),
        'desc'    => __( 'Organize what meta information you want displayed and in what order on the archives/post lists page', 'bfi_bootstrap' ),
        'options' => array(
            'enabled'  => array(
                'placebo'    	=> 'placebo', //REQUIRED!
                'publish_date' 	=> __( 'Publish Date', 'bfi_bootstrap' ),
                'author'     	=> __( 'Author', 'bfi_bootstrap' ),
                'categories' 	=> __( 'Categories', 'bfi_bootstrap' ),
                'comments'		=> __( 'Comments', 'bfi_bootstrap' ),
            ),
            'disabled' => array(
                'placebo'   		=> 'placebo', //REQUIRED!
                'last_updated_date'	=> __( 'Last Updated Date', 'bfi_bootstrap' ),
                'tags'   			=> __( 'Tags', 'bfi_bootstrap' ),
            )
        ),
	);	
		    
    // How should items be displayed
    $option_name = 'container_style';
 	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'required' => array( bfi_bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
        'title'    => __( 'Post Meta Information Container Style', 'bfi_bootstrap' ),
        'desc'     => __( 'How should the Post Meta Information items be styled', 'bfi_bootstrap' ),

        //Must provide key => value pairs
        'options'  => array(
            'plain' 	=> 'Plain Text',
            'label' 	=> 'Bootstrap Label',
            'button' 	=> 'Bootstrap Button',
        ), 
        'default'  => 'plain',
    );	    	   
    // What class to apply to the meta information style
    $option_name = 'container_class';
 	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'required' => array( bfi_bootstrap_get_option_name( $page_name, $section_name, 'show' ), 'equals', TRUE ),
        'title'    => __( 'Post Meta Information Container Class', 'bfi_bootstrap' ),
        'desc'     => __( 'What class should be applied to the Post Meta Information container.', 'bfi_bootstrap' ),
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

?>