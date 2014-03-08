<?php

/**
 * Include the Plugin Dependency Checker
 */
require_once dirname( __FILE__ ) . '/plugin-dependencies.php';


/**
 * Add some utilities for settin up options sections and getting options.
 */
require_once dirname( __FILE__ ) . '/utilities.php'; 

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
		$args['menu_title']             = __( 'Theme Options', '_bootstrap' );
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

	/**
	* Use font-awesome icons and remove elusive icons.
	* 
	* @return nothing
	*/
	function _bootstrap_use_font_awesome_in_redux() {
	    // remove elusive icon from the panel completely
	    //wp_deregister_style( 'redux-elusive-icon' );
	    //wp_deregister_style( 'redux-elusive-icon-ie7' );

	    wp_register_style(
	        'redux-font-awesome',
	        '//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css',
	        array(),
	        time(),
	        'all'
	    );  
	    wp_enqueue_style( 'redux-font-awesome' );
	}
	// Register the font change actions.
	add_action( 'redux/page/' . REDUX_OPT_NAME . '/enqueue', '_bootstrap_use_font_awesome_in_redux' );



	/**
	 * Add all the sections
	 * Each folder represents an options module.
	 * Each folder should include a section.php which defines the options for that module.
	 */
	foreach( glob( dirname( __FILE__ ) . '/*/section.php' ) as $module ) {
		require_once $module;
	}
}
else {
	// We will probably load defaults here.
}

?>