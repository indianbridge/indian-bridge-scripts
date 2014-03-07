<?php
/*
 * The skins core options for the _bootstrap theme
 */
if ( !function_exists( '_bootstrap_module_skin_options' ) ) {
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
		
		// Themes with screenshots
	    $fields[] = array(
	        'id'       => '_bootstrap_skin',
	        'type'     => 'image_select',
	        'title'    => __( 'Choose your bootstrap skin', '_bootstrap' ), 
	        'desc'     => __( 'Add a folder to css/skins directory if you want to supply your own skins.', '_bootstrap' ),
	        'options'  => $options_array,
	        'default' => 'Default'
	    );	
	    
	    //Switch to select cdn hosted css instead of local
	 	$fields[] = array(
	        'id'       => '_bootstrap_local_or_cdn_css',
	        'type'     => 'button_set',
	        'title'    => __( 'Use Local or CDN hosted Bootstrap CSS', '_bootstrap' ),
	        'desc'     => __( 'For CDN a file named cdn.link with link to the cdn css ', '_bootstrap' ) .
	        			  __( 'file should be available in the skin folder. If not the local will be used ', '_bootstrap' ) .
	        			  __( 'even if CDN is selected', '_bootstrap' ),
	        //Must provide key => value pairs
	        'options'  => array(
	            'local' => 'Local CSS',
	            'cdn' 	=> 'CDN Hosted CSS',
	        ), 
	        'default'  => 'local',
	    );	    
	    
	    //Switch to select cdn hosted font awesome icons instead of local
	 	$fields[] = array(
	        'id'       => '_bootstrap_local_or_cdn_fa',
	        'type'     => 'button_set',
	        'title'    => __( 'Use Local or CDN hosted Font Awesome CSS', '_bootstrap' ),
	        'desc'     => __( 'If CDN is selected then the font awesome css hosted on link in the next text field will be used.', '_bootstrap' ),
	        //Must provide key => value pairs
	        'options'  => array(
	            'local' => 'Local CSS',
	            'cdn' 	=> 'CDN Hosted CSS',
	        ), 
	        'default'  => 'local',
	    );	    	   
	    // The bootstrap js cdn location
	    $fields[] = array(
	        'id'       => '_boostrap_cdn_fa_location',
	        'type'     => 'text',
	        'required' => array( '_bootstrap_local_or_cdn_fa', 'equals', 'cdn' ),
	        'title'    => __( 'Path to CDN hosted Font Awesome icons and CSS', '_bootstrap' ),
	        'desc'     => __( 'This will be used only when CDN option above is selected.', '_bootstrap' ),
	        'default'  => '//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css'
	    );	     
	    
	    //Switch to select cdn hosted js instead of local
	 	$fields[] = array(
	        'id'       => '_bootstrap_local_or_cdn_js',
	        'type'     => 'button_set',
	        'title'    => __( 'Use Local or CDN hosted Bootstrap Javascript', '_bootstrap' ),
	        'desc'     => __( 'If CDN is selected then the Bootstrap JS hosted on link in the next text field will be used.', '_bootstrap' ),
	        //Must provide key => value pairs
	        'options'  => array(
	            'local' => 'Local JS',
	            'cdn' 	=> 'CDN Hosted JS',
	        ), 
	        'default'  => 'local',
	    );	      
	    // The bootstrap js cdn location
	    $fields[] = array(
	        'id'       => '_bootstrap_cdn_js_location',
	        'type'     => 'text',
	        'required' => array( '_bootstrap_local_or_cdn_js', 'equals', 'cdn' ),
	        'title'    => __( 'Path to CDN hosted Boostrap js', '_bootstrap' ),
	        'desc'     => __( 'This will be used only when CDN option above is selected.', '_bootstrap' ),
	        'default'  => '//netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js'
	    );	        	    
	        	    

		$section = array(
			'title' => __( 'Bootstrap CSS/JS', '_bootstrap' ),
			'icon' => 'el-icon-cloud icon-large'
		);
		
		$section['fields'] = $fields;

		$section = apply_filters( '_bootstrap_module_skin_options_modifier', $section );
		
		$sections[] = $section;
		return $sections;
	}
}

add_filter( 'redux/options/'.REDUX_OPT_NAME.'/sections', '_bootstrap_module_skin_options', 50 );