<?php
/**
 * Sets up the options page for Bootstrap skin/theme (from Bootswatch and elsewhere).
 * Sets up options for location of core css an js files. 
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
* Sets up the skin and css/js options page.
* 
* @param array $sections the redux sections array to append this section to
* 
* @return void
*/
function _bootstrap_module_skin_options( $sections ) {
	$fields = array();	
	$page_name = 'style';
	$section_name = 'skins';
	
	// The header for this section
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'header' ),
        'type'     => 'raw',
        'content'  => '<h1>' . __( 'Boostrap CSS Selection', '_bootstrap' ) . '</h1>',
    );	
    
    $custom_skin_name = 'Custom_Internal';
	// Skins with screenshots
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'skin' ),
        'type'     => 'image_select',
        'title'    => __( 'Choose your Bootstrap skin', '_bootstrap' ), 
        'desc'     => sprintf( __( 'If you select Custom CSS File the you have to provide the path to the css file in the textbox that appears. Add a folder to css/skins directory if you want to your own skins to be listed as screenshot above. You can %s use this link %s to call the Bootswatch API to update/get new Bootswatch skins. ', '_bootstrap' ), '<a href="http://localhost/bfi/wp-admin/themes.php?page=get-bootswatch-themes">' , '</a>' ),
        'options'  => _bootstrap_get_local_skin_list( $custom_skin_name ),
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
    
    // Add Bootstrap JS options;
	$section_name = 'bootstrap_js';
	$properties = array(
		'page_name'		=> $page_name,
		'section_name'	=> $section_name,
		'name'			=> __( 'Bootstrap Javascript', '_bootstrap' ),
		'default_local'	=> '$THEME_URI/js/bootstrap.min.js',
		'default_cdn'	=> '//netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js',
	);
	_bootstrap_add_asset_location_options( $fields, $properties );
    
    // Add Font Awesome CSS options;
	$section_name = 'font_awesome_css';
	$properties = array(
		'page_name'		=> $page_name,
		'section_name'	=> $section_name,
		'name'			=> __( 'Font Awesome CSS', '_bootstrap' ),
		'default_local'	=> '$THEME_URI/css/font-awesome-4.0.3/css/font-awesome.min.css',
		'default_cdn'	=> '//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css',
	);
	_bootstrap_add_asset_location_options( $fields, $properties );         	    

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

/**
* Get the list of local skins listed in the THEME_DIR/css/skins folder
* 
* @param string $custom_skin_name the id for the custom skin option
* @return array of skin objects with each skin speciying folder name, screenshot and location to cdn file
*/
function _bootstrap_get_local_skin_list( $custom_skin_name = 'Custom_Internal' ) {
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
	
	// Add an option for user specified custom skin if a local skin is not sufficient
	$skin_uri = THEME_DIR_URI . '/css/images/custom_css_file_text.jpg';
	$options_array[$custom_skin_name] = array(
		'alt'	=> 'Custom CSS File',
		'img'	=> $skin_uri,
	);	
	
	return $options_array;
	
}