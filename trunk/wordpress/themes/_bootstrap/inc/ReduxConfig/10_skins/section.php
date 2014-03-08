<?php
/**
 * The skin and bootstrap files options.
 * 
 * @package _bootstrap
 */
 
function _bootstrap_module_skin_options( $sections ) {
	$fields = array();
	
	// Get all the folders in the skins folder
	// Each folder should have a css file, a screenshot file and options link to cdn hosted css
	$skins_folder = THEME_DIR . '/css/skins';

	foreach( glob( $skins_folder . '/*', GLOB_ONLYDIR ) as $folder ) {
		$skin_name = basename( $folder );
		$skin_uri = THEME_DIR_URI . '/css/skins/' . $skin_name . '/screenshot.png';
		$options_array[$skin_name] = array(
			'alt'	=> $skin_name,
			'img'	=> $skin_uri,
		);
	}  
	$custom_skin_name = 'Custom_Internal';
	$skin_uri = THEME_DIR_URI . '/css/images/custom_css_file_text.jpg';
	$options_array[$custom_skin_name] = array(
		'alt'	=> 'Custom CSS File',
		'img'	=> $skin_uri,
	);	
	
	$page_name = 'style';
	$section_name = 'skins';
	// The header for this section
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'header' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Boostrap CSS Selection', '_bootstrap' ) . '</h1>',
    );	
	// Skins with screenshots
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'skin' ),
        'type'     => 'image_select',
        'title'    => __( 'Choose your Bootstrap skin', '_bootstrap' ), 
        'desc'     => __( 'Add a folder to css/skins directory if you want to supply your own skins. If you select Custom CSS File the you have to provide the path to the css file in the textbox that appears.', '_bootstrap' ),
        'options'  => $options_array,
        'default' => 'Default'
    );	
    //Switch to select cdn hosted css instead of local
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'bootstrap_css' ),
        'type'     => 'button_set',
        'title'    => __( 'Use Local or CDN hosted Bootstrap CSS', '_bootstrap' ),
        'desc'     => __( 'For CDN a file named cdn.link with link to the cdn css file should be available in the skin folder. If not the local will be used even if CDN is selected', '_bootstrap' ),
                'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'skin' ), 'not', $custom_skin_name ),
        'options'  => array(
        	'local'	=> 'Use Local CSS',
        	'cdn'	=> 'Use CDN Hosted CSS',
        ),                
        'default'  => 'cdn',
    );	 
    
    // Custom css file location
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'bootstrap_custom_css_location' ),
        'type'     => 'text',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'skin' ), 'equals', $custom_skin_name ),
        'title'    => __( 'Path to Custom Bootstrap CSS File', '_bootstrap' ),
        'desc'     => __( 'This will be used only when Custom CSS File option is selected above.', '_bootstrap' ),
        'default'  => '//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css'
    );	
    
	$section_name = 'bootstrap_js';
	
	// The header for this section
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'header' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Bootstrap JS', '_bootstrap' ) . '</h1>',
    );  	    
	
    // Select Bootstrap js option
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'bootstrap_js' ),
        'type'     => 'button_set',
        'title'    => __( 'Use No, Local or CDN hosted Bootstrap Javascript', '_bootstrap' ),
        'desc'     => __( 'If CDN is selected then a text box to specify location of CDN hosted js will appear.', '_bootstrap' ),
        'options'  => array(
        	'no'	=> 'Don\'t Include JS',
        	'local'	=> 'Use Local JS',
        	'cdn'	=> 'Use CDN Hosted JS',
        ),
        'default'  => 'cdn',
    );	      
    // The bootstrap js cdn location
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'bootstrap_js_cdn_location' ),
        'type'     => 'text',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'bootstrap_js' ), 'equals', 'cdn' ),
        'title'    => __( 'Path to CDN hosted Boostrap js', '_bootstrap' ),
        'desc'     => __( 'This will be used only when CDN option above is selected.', '_bootstrap' ),
        'default'  => '//netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js'
    );	
    
    $section_name = 'font_awesome_css';
    	
	// The header for this section
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'header' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Font Awesome CSS', '_bootstrap' ) . '</h1>',
    );   
    
    // Select Font Awesome css option
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'font_awesome_css' ),
        'type'     => 'button_set',
        'title'    => __( 'Use No, Local or CDN hosted Font Awesome CSS', '_bootstrap' ),
        'desc'     => __( 'If CDN is selected then a text box to specify location of CDN hosted CSS will appear.', '_bootstrap' ),
        'options'  => array(
        	'no'	=> 'Don\'t Include CSS',
        	'local'	=> 'Use Local CSS',
        	'cdn'	=> 'Use CDN Hosted CSS',
        ),
        'default'  => 'cdn',
    );        	   
    // The bootstrap js cdn location
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'font_awesome_css_cdn_location' ),
        'type'     => 'text',
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'font_awesome_css' ), 'equals', 'cdn' ),
        'title'    => __( 'Path to CDN hosted Font Awesome icons and CSS', '_bootstrap' ),
        'desc'     => __( 'This will be used only when CDN option above is selected.', '_bootstrap' ),
        'default'  => '//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css'
    );	            	    
        	    

	$section = array(
		'title' => __( 'Skin & Bootstrap CSS/JS Location', '_bootstrap' ),
		'icon' => 'fa fa-cloud fa-lg'
	);
	
	$section['fields'] = $fields;

	$section = apply_filters( '_bootstrap_module_skin_options_modifier', $section );
	
	$sections[] = $section;
	return $sections;
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_skin_options', 50 );