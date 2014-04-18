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
			'title'   => __( 'Theme Information 1', '_bootstrap' ),
			'content' => __('<p>This is the tab content, HTML is allowed.</p>', '_bootstrap' )
		);
		$args['help_tabs'][] = array(
			'id'      => 'redux-options-2',
			'title'   => __( 'Theme Information 2', '_bootstrap' ),
			'content' => __( '<p>This is the tab content, HTML is allowed. Tab2</p>', '_bootstrap' )
		);
	
		//Set the Help Sidebar for the options page - no sidebar by default                   
		$args['help_sidebar'] = __( '<p>This is the sidebar content, HTML is allowed.</p>', '_bootstrap' );
	
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

function bfi_bootstrap_get_container_config( &$fields, $area_name ) {
    // The container style
 	$fields[] = array(
        'id'       => $area_name . '-container-style',
        'type'     => 'button_set',
        'title'    => $area_name . ' Container Style',
        'desc'     => 'How should the '. $area_name . ' Container be styled',
        'options'  => array(
            'panel' 	=> 'Bootstrap Panel',
            'well' 		=> 'Bootstrap Well',
            'alert'		=> 'Bootstrap Alert',
            'none'		=> 'None',
        ), 
        'default'  => 'panel',
    );	
        	   
    // What class to apply to the container when panel is used.
 	$fields[] = array(
        'id'       => $area_name . '-container-panel-style',
        'type'     => 'button_set',
        'title'    => $area_name . ' Container Bootstrap Panel Class',
        'desc'     => 'What class should be applied to the Panel.',
        'required' => array( $area_name . '-container-style', 'equals', 'panel' ),
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
 	$fields[] = array(
        'id'       => $area_name . '-container-alert-style',
        'type'     => 'button_set',
        'title'    => $area_name . ' Container Bootstrap Alert Class',
        'desc'     => 'What class should be applied to the Alert.',
        'required' => array( $area_name . '-container-style', 'equals', 'alert' ),       
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
 	$fields[] = array(
        'id'       => $area_name . '-container-text-style',
        'type'     => 'button_set',
        'title'    => 'Text Class for the ' . $area_name . ' Container',
        'desc'     => 'What class should be applied to the text.',      
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

function bfi_bootstrap_get_container_options( $prefix ) {
	 $container_style = bfi_bootstrap_get_redux_option( $prefix . '-container-style' );
	 $panel_style = bfi_bootstrap_get_redux_option( $prefix . '-container-panel-style' );
	 $alert_style = bfi_bootstrap_get_redux_option( $prefix . '-container-alert-style' );
	 $text_style = bfi_bootstrap_get_redux_option( $prefix . '-container-text-style' );
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

function bfi_bootstrap_get_redux_option( $name, $key = false ) {
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
}

?>