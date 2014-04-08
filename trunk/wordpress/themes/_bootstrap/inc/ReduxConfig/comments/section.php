<?php
/**
 * Sets up the options page for comments section
 *
 * LICENSE: #LICENSE#
 *
 * @package    _bootstrap_redux_config
 * @author     Sriram Narasimhan
 * @copyright  #COPYRIGHT#
 * @version    SVN: $Id$
 * @since      File available since Release 1.0.0
 *
 */

/**
* Sets up the comments options page.
* 
* @param array $sections the redux sections array to append this section to
* 
* @return void
*/
function _bootstrap_module_comments_options( $sections ) {
	$fields = array();	
	$page_name = 'comments';
	
	// Area container
	$properties = array (
		'page_name' 	=> $page_name,
		'name'			=> 'Comments',
		'default_width'	=> 'container-fluid',
		'include_title'	=> TRUE,
	); 	
	_bootstrap_add_area_container_options( $fields, $properties );
	$section = array(
		'title' => __( 'Comments', '_bootstrap' ),
		'icon' => 'fa fa-comments-o fa-lg'
	);
	
	// Single Comment Container
	$properties = array (
		'page_name' 	=> $page_name,
		'name'			=> 'Single Comment',
		'default_container'	=> 'well',
		'include_title'	=> TRUE,
	);
	_bootstrap_add_container_styling_options( $fields, $properties );	
	
	$section_name = 'paging';
		$fields[] = array( 
	        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
	        'type'     => 'raw',
	        'content'  => '<h1>' . __( 'Paging/Pagination Format', '_bootstrap' ) . '</h1>',
	    );			
    
	    // Paging Style
	 	$fields[] = array(
	        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'style' ),
	        'type'     => 'button_set',
	        'title'    => __( 'Comments Paging Style', '_bootstrap' ),
	        'desc'     => __( 'What kind of paging style should be applied when their are multiple pages of comments.', '_bootstrap' ),
	        'options'  => array(
	            'wp_paging' 			=> __( 'Wordpress Paging', '_bootstrap' ),
	            'bootstrap_paging' 		=> __( 'Bootstrap Paging', '_bootstrap' ),
	        ), 
	        'default'  => 'bootstrap_paging',      
	    );	
	    
	    // Location of paging
		$fields[] = array(
	        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'location' ),
	        'type'     => 'button_set',
	        'title'    => __( 'Comments Paging Location', '_bootstrap' ),
	        'desc'     => __( 'Where should the comments paging shown.', '_bootstrap' ),
	        'options'  => array(
	            'above' => __( 'Above Comments', '_bootstrap' ),
	            'below' => __( 'Below Comments', '_bootstrap' ),
	            'both'	=> __( 'Above and Below Comments', '_bootstrap' ),
	        ), 
	        'default'  => 'bootstrap_paging',      
	    );		    
	
	$section['fields'] = $fields;

	$section = apply_filters( '_bootstrap_module_comments_options_modifier', $section );
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_comments_options', 50 );
