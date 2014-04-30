<?php

/**
 * Include the Plugin Dependency Checker
 */
require_once dirname( __FILE__ ) . '/plugin-dependencies.php';


/*
 * Require the framework class before doing anything else, so we can use the defined urls and dirs
 * Also if running on windows you may have url problems, which can be fixed by defining the framework url first
 */
if ( class_exists( 'ReduxFramework' ) ) {
	function _bootstrap_redux_init() {	
		$args = array();
	
		$args['opt_name']               = REDUX_OPT_NAME;
		$args['customizer']             = true;
		$args['forced_edd_license']     = true;
		$args['google_api_key']         = 'AIzaSyCDiOc36EIOmwdwspLG3LYwCg9avqC5YLs';
		$args['global_variable']        = 'redux';
		$args['default_show']           = true;
		$args['default_mark']           = '*';
		$args['page_slug']              = REDUX_OPT_NAME;
		$theme                          = wp_get_theme();
		$args['display_name']           = $theme->get( 'Name' );
		$args['menu_title']             = 'BFI_Bootstrap Theme Options';
		$args['display_version']        = $theme->get( 'Version' );    
		$args['page_position']          = 99;
		$args['dev_mode']               = false;
		$args['page_type']              = 'submenu';
		$args['page_parent']            = 'themes.php';
	
		$args['help_tabs'][] = array(
			'id'      => 'redux-options-1',
			'title'   => __( 'Theme Information 1', 'bfi_bootstrap' ),
			'content' => __('<p>This is the tab content, HTML is allowed.</p>', 'bfi_bootstrap' )
		);
		$args['help_tabs'][] = array(
			'id'      => 'redux-options-2',
			'title'   => __( 'Theme Information 2', 'bfi_bootstrap' ),
			'content' => __( '<p>This is the tab content, HTML is allowed. Tab2</p>', 'bfi_bootstrap' )
		);
	
		//Set the Help Sidebar for the options page - no sidebar by default                   
		$args['help_sidebar'] = __( '<p>This is the sidebar content, HTML is allowed.</p>', 'bfi_bootstrap' );
	
		$sections = array();
		$sections = apply_filters( '_bootstrap_add_sections', $sections );
	
		$ReduxFramework = new ReduxFramework( $sections, $args );
	
		if ( !empty( $redux['dev_mode'] ) && $redux['dev_mode'] == 1 ) :
			$ReduxFramework->args['dev_mode']     = true;
			$ReduxFramework->args['system_info']  = true;
		endif;
	}
	add_action('init', '_bootstrap_redux_init');
	require_once dirname( __FILE__ ) . '/main-section.php';
	require_once dirname( __FILE__ ) . '/header-section.php';
	require_once dirname( __FILE__ ) . '/content-section.php';
	require_once dirname( __FILE__ ) . '/sidebar-section.php';
	require_once dirname( __FILE__ ) . '/comments-section.php';
	require_once dirname( __FILE__ ) . '/footer-section.php';
	
}
else {
	// We will probably load defaults here.
}

/**
* Use font-awesome icons and remove elusive icons.
*/
function bfi_bootstrap_use_font_awesome_in_redux() {
    // remove elusive icon from the panel completely
    wp_deregister_style( 'redux-elusive-icon' );
    wp_deregister_style( 'redux-elusive-icon-ie7' );

    wp_register_style(
        'redux-font-awesome',
        'http://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css',
        array(),
        time(),
        'all'
    );  
    wp_enqueue_style( 'redux-font-awesome' );
}
// Register the font change actions.
add_action( 'redux/page/' . REDUX_OPT_NAME . '/enqueue', 'bfi_bootstrap_use_font_awesome_in_redux' );

function bfi_bootstrap_get_local_skin_list( ) {
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
	
	return $options_array;
	
}

function bfi_bootstrap_get_container_config( &$fields, $page_name, $title ) {
	$section_name = 'container';
	
	$option_name = 'title';
	$fields[] = array( 
	    'id'       =>  bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
	    'type'     => 'raw',
	    'content'  => '<h1>' . $title . __( ' Container Options', 'bfi_bootstrap' ) . '</h1>',
	);		  
    // The container style
    $option_name = 'style';
    $field_name = bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name );
 	$fields[] = array(
        'id'       => $field_name,
        'type'     => 'button_set',
        'title'    => $title . __( ' Container Style', 'bfi_bootstrap' ),
        'desc'     => __( 'How should the ', 'bfi_bootstrap' ). $area_name . __( ' Container be styled', 'bfi_bootstrap' ),
        'options'  => array(
            'panel' 	=> 'Bootstrap Panel',
            'well' 		=> 'Bootstrap Well',
            'alert'		=> 'Bootstrap Alert',
            'none'		=> 'None',
        ), 
        'default'  => 'panel',
    );	
        	   
    // What class to apply to the container when panel is used.
    $option_name = 'panel-style';
 	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => $title . __( ' Container Bootstrap Panel Class', 'bfi_bootstrap' ),
        'desc'     => __( 'What class should be applied to the Panel.', 'bfi_bootstrap' ),
        'required' => array( $field_name, 'equals', 'panel' ),
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
    
    // What class to apply to the container when alert is used.
    $option_name = 'alert-style';
 	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => $title . __( ' Container Bootstrap Alert Class', 'bfi_bootstrap' ),
        'desc'     => __( 'What class should be applied to the Alert.', 'bfi_bootstrap' ),
        'required' => array( $field_name, 'equals', 'alert' ),       
        'options'  => array(
            'success' 	=> 'Success',
            'info' 		=> 'Info',
            'warning' 	=> 'Warning',
            'danger' 	=> 'Danger',
            'none'		=> 'None',
        ), 
        'default'  => 'info',      
    );
    
    // What class to apply to the  text of the container
    $option_name = 'text-style';
 	$fields[] = array(
        'id'       => bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ),
        'type'     => 'button_set',
        'title'    => __( 'Text Class for the ', 'bfi_bootstrap' ) . $title . __( ' Container', 'bfi_bootstrap' ),
        'desc'     => __( 'What class should be applied to the text.', 'bfi_bootstrap' ),      
        'options'  => array(
            'muted' 	=> 'Muted',
            'primary' 	=> 'Primary',
            'success' 	=> 'Success',
            'info' 		=> 'Info',
            'warning' 	=> 'Warning',
            'danger' 	=> 'Danger',
            'none'		=> 'None',
        ),
        'default'  => 'none',      
    );	  	
}

function bfi_bootstrap_get_container_options( $page_name ) {
	$section_name = 'container';
	$container_style = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'style' );
	$panel_style = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'panel-style' );
	$alert_style = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'alert-style' );
	$text_style = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'text-style' );
	$container_class = '';
	if ( $container_style === 'panel' ) {
	$container_class = 'panel panel-' . $panel_style;
	}
	else if ( $container_style === 'alert' ) {
	$container_class = 'alert alert-' . $alert_style;
	}
	else if ( $container_style === 'well' ) {
	$container_class = 'well';
	}
	$container_class .= ' text-' . $text_style;
	return $container_class;	
}

function bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name ) {
	return THEME_NAME . '_' . $page_name . '_' . $section_name . '_' . $option_name;
}

function bfi_bootstrap_get_redux_option( $page_name, $section_name, $option_name, $key = false ) {
	global $redux;
	$options = $redux;
	
	$name = bfi_bootstrap_get_option_name( $page_name, $section_name, $option_name );
	// Set this to your preferred default value
	$var = '';

	if ( empty( $name ) && !empty( $options ) ) {
		$var = $options;
	} else {
		if ( !empty( $options[$name] ) ) {
			$var = ( !empty( $key ) && !empty( $options[$name][$key] ) && $key !== true ) ? $options[$name][$key] : $var = $options[$name];;
		}
	}
	return $var;
}

/*function bfi_bootstrap_get_redux_option( $name, $key = false ) {
	global $redux;
	$options = $redux;
	if ( ! isset( $options ) ) return NULL;
	// Set this to your preferred default value
	$var = '';

	if ( empty( $name ) && !empty( $options ) ) {
		$var = $options;
	} else {
		if ( !empty( $options[$name] ) ) {
			$var = ( !empty( $key ) && !empty( $options[$name][$key] ) && $key !== true ) ? $options[$name][$key] : $var = $options[$name];;
		}
	}
	return $var;
}*/

?>