<?php
function bfi_bootstrap_module_header_options( $sections ) {
	$fields = array();	
	
	$title = __( 'Header', 'bfi_bootstrap' );
	$page_name = 'header';
	$section_name = '';
	// Default or Inverse navbar.
	$option_name = 'style';
 	$fields[] = array(
        'id'       =>  bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => $title . __( ' Style', 'bfi_bootstrap' ),
        'desc'     => __( 'Choose the Bootstrap style for the ', 'bfi_bootstrap' ) . $title . __( '. This will apply bootstrap classes navbar-default or navbar-inverse.', 'bfi_bootstrap' ),
        'options'  => array(
            'default'	=> 'Default',
            'inverse'	=> 'Inverse',
        ), 
        'default'  => 'default',
    );		    
    
    // Static or Fixed navbar. 
    $option_name = 'alignment';
 	$fields[] = array(
        'id'       =>  bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => $title . __( 'Alignment', 'bfi_bootstrap' ),
        'desc'     => __( 'Static navbar stays on top of page. Fixed navbar moves and stays on top of screen even as you scroll. Applies class navbar-static and navabar-fixed.', 'bfi_bootstrap' ),
        'options'  => array(
            'static'	=> 'Static',
            'fixed'		=> 'Fixed',
        ), 
        'default'  => 'fixed',
    );	  	
	
	// header image
	$option_name = 'image';
    $fields[] = array(
	    'id'       =>  bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
	    'type'     => 'media', 
	    'title'    => $title . __( ' Image', 'bfi_bootstrap' ),
	    'desc'     => __( 'The image to be displayed in header bar', 'bfi_bootstrap' ),
	);  
	
	//Jumbotron
	$section_name = 'jumbotron';
	$fields[] = array( 
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Jumbotron Options', 'bfi_bootstrap' ) . '</h1>',
    );	
	
	$show_jumbotron = bfi_bootstrap_get_option_name( $page_name, $section_name, 'show' );
	$fields[] = array(
        'id'       => $show_jumbotron,
        'type'     => 'button_set', 
        'title'    => __( 'Show Jumbotron?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the Jumbotron is not shown or shown in front page only or shown everywhere', 'bfi_bootstrap' ),
        'options'  => array(
        	'no'		=> 'Don\'t Show Jumbotron anywhere',
            'front' 	=> 'Show Jumbotron in Front Page Only',
            'all' 		=> 'Show Jumbotron everywhere',
        ), 
        'default'  => 'front',
    );	
	/*$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'show-where' ),
        'type'     => 'switch', 
        'title'    => __( 'Where to show the Jumbotron?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the jumbotron is shown only in front page or all pages.', 'bfi_bootstrap' ),
        'required' => array( $show_jumbotron, '!=', 'no' ),
        'on'		=> __( 'Show in Front Page Only', 'bfi_bootstrap' ),
        'off'		=> __( 'Show in All Pages', 'bfi_bootstrap' ),
        'default'  => FALSE,
    );   */   
    
	$html = __( '<h1>Welcome to landing page!</h1><p>This is an example for jumbotron.</p><p><a class="btn btn-primary btn-lg" role="button">Learn more</a></p>', 'bfi_bootstrap' );
    $fields[] = array(
        'id'			=> bfi_bootstrap_get_option_name( $page_name, $section_name, 'html' ),
        'type' 			=> 'textarea',
        'title' 		=> __( 'Jumbotron Contents', '_bootstrap' ), 
        'desc' 			=> __( 'Enter content for Jumbotron. HTML is allowed.', '_bootstrap'),
        'required' => array( $show_jumbotron, '!=', 'no' ),
        'validate' 		=> 'html',
        'default' 		=> $html,
    ); 	   
    
     //Carousel
	$section_name = 'carousel';
	$fields[] = array( 
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Carousel Options', 'bfi_bootstrap' ) . '</h1>',
    );	
	
	$show_carousel = bfi_bootstrap_get_option_name( $page_name, $section_name, 'show' );
	/*$fields[] = array(
        'id'       => $show_carousel,
        'type'     => 'switch', 
        'title'    => __( 'Show Carousel?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the carousel is shown or not', 'bfi_bootstrap' ),
        'on'		=> __( 'Show Carousel', 'bfi_bootstrap' ),
        'off'		=> __( 'Don\'t Show carousel', 'bfi_bootstrap' ),
        'default'  => TRUE,
    );*/
    
	$fields[] = array(
        'id'       => $show_carousel,
        'type'     => 'button_set', 
        'title'    => __( 'Show Carousel?', '_bootstrap' ),
        'desc'     => __( 'This controls whether the carousel is not shown or shown only in front page or all pages.', 'bfi_bootstrap' ),
        'options'  => array(
        	'no'		=> 'Don\'t Show carousel anywhere',
            'front' 	=> 'Show Carousel in Front Page Only',
            'all' 		=> 'Show Carousel everywhere',
        ), 
        'default'  => 'no',
    );       	
    
    $fields[] = array(
    'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'categories' ),
    'type'     => 'select',
    'multi'    => true,
    'title'    => __('Categories to show in slider', 'bfi_bootstrap'), 
    'desc'     => __('The categories from which posts are showin carousel.', 'bfi_bootstrap'),
    'required' => array( $show_carousel, '!=', 'no' ),
    'data'  => 'categories',
	);
	
	$fields[] = array(
	    'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, 'slide-count' ),
	    'type'     => 'spinner', 
	    'title'    => __('Number of Slides', 'bfi_bootstrap'),
	    'desc'     => __('The number of posts to be shown in the carousel', 'bfi_bootstrap'),
	    'required' => array( $show_carousel, '!=', 'no' ),
	    'default'  => '5',
	    'min'      => '1',
	    'step'     => '1',
	    'max'      => '10',
	);	

	$section = array(
		'title' => $title . __( ' Options', 'bfi_bootstrap' ),
		'icon' => 'fa fa-hand-o-up'
	);
	
	$section['fields'] = $fields;
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', 'bfi_bootstrap_module_header_options', 50 );
?>