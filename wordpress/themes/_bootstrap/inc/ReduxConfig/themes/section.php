<?php
/*
 * The skins core options for the _bootstrap theme
 */
if ( !function_exists( '_bootstrap_module_bootswatch_options' ) ) {
	function _bootstrap_module_bootswatch_options( $sections ) {
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
		
		// Themes with screenshots
	    $fields[] = array(
	        'id'       => 'bootstrap_skins',
	        'type'     => 'image_select',
	        'title'    => __( 'Choose your bootstrap theme', '_bootstrap' ), 
	        'desc'     => __( 'Add a folder to css/skins directory if you want your own theme', '_bootstrap' ),
	        'options'  => $options_array,
	        'default' => 'Default'
	    );	
	    
	    //Switch to select cdn hosted css instead of local
		$fields[] = array(
	        'id'       => 'local_or_cdn_css',
	        'type'     => 'switch', 
	        'title'    => __( 'Use CDN Hosted Bootstrap CSS instead of Local', '_bootstrap' ),
	        'desc'     => __( 'For CDN a file names cdn.link with link to the cdn css ', '_bootstrap' ) .
	        			  __( 'file should be available. If not the local will be used ', '_bootstrap' ) .
	        			  __( 'even if CDN is select', '_bootstrap' ),
	        'default'  => FALSE,
	    );	 
	    
	    //Switch to select cdn hosted js instead of local
		$fields[] = array(
	        'id'       => 'local_or_cdn_js',
	        'type'     => 'switch', 
	        'title'    => __( 'Use CDN Hosted Bootstrap JS instead of Local', '_bootstrap' ),
	        'desc'     => __( 'If selected then the js hosted on from link in the next text field will be used.', '_bootstrap' ),
	        'default'  => FALSE,
	    );	   
	    // The bootstrap js cdn location
	    $fields[] = array(
	        'id'       => 'cdn_js_location',
	        'type'     => 'text',
	        'title'    => __('Path to CDN hosted Boostrap js', '_bootstrap'),
	        'desc'     => __('This will be used only when use CDN above is switched on.', '_bootstrap'),
	        'default'  => '//netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js'
	    );	        	    
	        	    

		$section = array(
			'title' => __( 'Skins', '_bootstrap' ),
			'icon' => 'el-icon-css icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_bootswatch_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_bootswatch_options', 50 );