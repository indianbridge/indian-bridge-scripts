<?php
/**
 * Convenience functions to setup and retreive Redux Framework option sections. 
 *
 * Defines functions to add repeatedly used option sections as well as
 * functions to retreive all the values in option sections as an associative array.
 *
 * LICENSE: #LICENSE#
 *
 * @package    _bootstrap_redux_framework
 * @author     Sriram Narasimhan
 * @copyright  #COPYRIGHT#
 * @version    SVN: $Id$
 * @since      File available since Release 1.0.0
 *
 */

/**
* Construct option name based on page, section and option name. 
* Uses a constant THEME_NAME which required to be set ( in functions.php ).
* 
* @param string $page_name the page this option belongs to
* @param string $section_name page section this option belongs to
* @param string $option_name the actual option name.
* 
* @return string full option name
*/
function _bootstrap_get_option_name( $page_name, $section_name, $option_name ) {
	return THEME_NAME . '_' . $page_name . '_' . $section_name . '_' . $option_name;
}

/**
* Check if Font Awesome CSS has been included.
* 
* @return true if included false otherwise
*/
function _bootstrap_font_awesome_enabled() {
	$page_name = 'style';
	$section_name = 'font_awesome_css';
	$fa_location =  _bootstrap_get_redux_option( $page_name, $section_name, 'font_awesome_css' );	
	return ( $fa_location !== 'no' );
}

/**
* Return a font awesome icon or place holder text if not enabled.
* 
* @param string $icon_name name of icon
* @param string $icon_title place holder text
* 
* @return string html markup to display icon or name.
*/
function _bootstrap_get_font_awesome_icon( $icon_name, $icon_title, $class = '' ) {
	if ( _bootstrap_font_awesome_enabled() ) {
	 	return '<i title="' . $icon_title . '" class="fa fa-' . $icon_name . ' ' . $class . '"></i>';
 	}
 	else {
 		return $icon_title;
		
	}	
}

/**
* Check if Bootstrap JS has been included.
* 
* @return true if included false otherwise
*/
function _bootstrap_js_enabled() {
	$page_name = 'style';
	$section_name = 'bootstrap_js';
	$js_location =  _bootstrap_get_redux_option( $page_name, $section_name, 'bootstrap_js' );	
	return ( $js_location !== 'no' );
}

/**
 * Get the value of speficied option.
 *
 * @param $name the string name of option.
 * @param key the string key whose value is needed.
 * @return mixed the value of the option.
 */ 
function _bootstrap_get_option( $name, $key = false ) {
	global $redux;
	$options = $redux;

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

/**
* Get the redux option with specified parameters.
* 
* @param string $page_name the options page in which this option appears.
* @param string $section_name the section in which this option appears.
* @param string $option_name the name of the option.
* @param string $key an optional key to a sub property of this option.
* 
* @return mixed the value of the option.
*/
function _bootstrap_get_redux_option( $page_name, $section_name, $option_name, $key = false ) {
	global $redux;
	$options = $redux;
	
	$name = _bootstrap_get_option_name( $page_name, $section_name, $option_name );
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

/**
* Create the options layout for a container (could be for content or sidebar etc.)
* 
* @param array $fields reference to array of fields to which we will add styling options.
* @param array $properties associative array of options used to customize the options
* 						   example : array ( 
										'page_name' : 'header', 
 										'section_name' : 'area_container', 
 										'name' : 'Header',
 										'default_container' : 'panel',
 										'include_title' : TRUE,
 									 )
* 
* @return void
*/
function _bootstrap_add_container_styling_options( &$fields, $properties ) {
	
	extract( $properties );
	
	if ( ! isset( $default_container ) ) {
		$default_container = 'none';
	}
	
	if ( ! isset( $include_title) ) {
		$include_title = TRUE;
	}

	$title = $name . ' ';
	if ( $include_title ) {
		$title .= __( 'Title, ', '_bootstrap' );
	}
	$title .= __( 'Container and Text Styling', '_bootstrap' );
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . $title . '</h1>',
    );	
    
    if ( $include_title ) {
	    // The container title style
	 	$fields[] = array(
	        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title_style' ),
	        'type'     => 'button_set',
	        'title'    => $name . __( ' Title Style', '_bootstrap' ),
	        'desc'     => sprintf( __( 'How should the %s Title be styled. If you choose custom enter tag name in the text field that appears.', '_bootstrap' ), $name ),
	        'options'  => array(
	            'h1' 		=> 'h1',
	            'h2' 		=> 'h2',
	            'h3' 		=> 'h3',
	            'h4' 		=> 'h4',
	            'h5' 		=> 'h5',
	            'h6' 		=> 'h6',
	            'custom'	=> 'Custom',
	        ), 
	        'default'  => 'h3',
	    );	   
	    
	    // Custom tag for title
	    $fields[] = array(
	        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title_custom' ),
	        'type'     => 'text',
	        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'title_style' ), 'equals', 'custom' ),
	        'title'    => sprintf( __( 'Custom tag for %s Title', '_bootstrap'), $name ),
	        'desc'     => __( 'This will be used only when custom option is selected above.', '_bootstrap' ),
	        'default'  => 'div'
	    );	     
    }
    	
    // The container style
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ),
        'type'     => 'button_set',
        'title'    => $name . __( ' Container Style', '_bootstrap' ),
        'desc'     => sprintf( __('How should the %s Container be styled', '_bootstrap' ), $name ),
        'options'  => array(
            'plain' 	=> 'Plain Text',
            'panel' 	=> 'Bootstrap Panel',
            'well' 		=> 'Bootstrap Well',
            'alert'		=> 'Bootstrap Alert',
            'none'		=> 'None',
        ), 
        'default'  => $default_container,
    );	
        	   
    // What class to apply to the container when panel is used.
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'class_panel' ),
        'type'     => 'button_set',
        'title'    => __( 'Bootstrap Panel Class', '_bootstrap' ),
        'desc'     => __( 'What class should be applied to the Panel.', '_bootstrap' ),
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ), 'equals', 'panel' ),
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
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'class_alert' ),
        'type'     => 'button_set',
        'title'    => __( 'Bootstrap Alert Class', '_bootstrap' ),
        'desc'     => __( 'What class should be applied to the Alert.', '_bootstrap' ),
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ), 'equals', 'alert' ),       
        'options'  => array(
            'success' 	=> 'Success',
            'info' 		=> 'Info',
            'warning' 	=> 'Warning',
            'danger' 	=> 'Danger',
            'none'		=> 'None',
        ), 
        'default'  => 'info',      
    );
    
    // What class to apply to the  text when plain text or well is used.
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'class_text' ),
        'type'     => 'button_set',
        'title'    => __( 'Text Class for Well or Plain Text option', '_bootstrap' ),
        'desc'     => __( 'What class should be applied to the text when Well or Plain Text is used.', '_bootstrap' ),
        'required' => array( _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ), 'equals', array( 'well', 'plain' ) ),       
        'options'  => array(
            'muted' 	=> 'Muted',
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

/**
* Retreive the container styling options for specified page and section.
* 
* @param string $page_name the name of page where options will be found.
* @param string $section_name the name of section where options will be found.
* @param boolean $include_title should styling for title be included.
* 
* @return associative array of classes for styling content
*/
function _bootstrap_get_container_options( $page_name, $section_name ) {
	
	// The style to be applied to title.
	$output['title_tag'] = _bootstrap_get_redux_option( $page_name, $section_name, 'title_style' );
	if ( $output['title_tag'] === 'custom' ) {
		$output['title_tag'] = _bootstrap_get_redux_option( $page_name, $section_name, 'title_custom' );
	}	
	
	// The style for the container
	$output['style'] = _bootstrap_get_redux_option( $page_name, $section_name, 'container_style' );

	$class = '';
	switch ( $output['style'] ) {
		// Bootstrap panel container
		case 'panel' :
			$class = _bootstrap_get_redux_option( $page_name, $section_name, 'class_panel' );
			break;
		
		// Bootstrap alert container
		case 'alert' :
			$class = _bootstrap_get_redux_option( $page_name, $section_name, 'class_alert' );
			break;
			
		// Bootstrap well and plain text
		case 'well' :
		case 'plain' :
			$class = _bootstrap_get_redux_option( $page_name, $section_name, 'class_text' );
			break;	
		default :
			break;
	}

	// Classes for article sections that will conditionalized based on type of container being used.
	$output['container_class'] = '';
	$output['header_class'] = '';
	$output['title_class'] = '';
	$output['body_class'] = '';
	$output['footer_class'] = '';

	// Setup classes for all sections of the article depending on type of container selected
	switch ( $output['style'] ) {
		
		// Bootstrap panel container
		case 'panel' :
			$output['container_class'] = 'panel panel-' . $class; 
			$output['header_class'] = 'panel-heading';
			$output['title_class'] = 'panel-title';
			$output['body_class'] = 'panel-body';
			$output['footer_class'] = 'panel-footer';
			break;
		
		// Bootstrap alert container
		case 'alert' :
			$output['container_class'] = 'alert alert-' . $class;
			break;
			
		// Bootstrap well and text is assigned class
		case 'well' :
			$output['container_class'] = 'well text-' . $class;
			break;
		
		// No container but text is assigned class
		case 'plain' :
			$output['container_class'] = 'text-' . $class;
			break;
		default :
			break;
	}		
	return $output;	
}

/**
* Add sidebar layout options in the page
* 
* @param array $fields reference to array of options to which to append.
* @param array $properties associative array of options used to customize the options
* 						   example : array ( 
										'page_name' : 'header', 
 										'section_name' : 'area_container', 
 										'name' : 'Header',
 										'items' : array (),
 									 )
* 
* @return nothing
*/
function _bootstrap_add_sidebar_options( &$fields, $properties ) {
	
	extract( $properties );
	// The header for this section
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'header' ),
        'type'     => 'raw',
        'content'  => '<h1>' . sprintf( __( '%s Layout Options', '_bootstrap' ), $name ) . '</h1>',
    );
    
    $enabled['placebo'] = 'placebo';
    $disabled['placebo'] = 'placebo';
    $count = 0;
    foreach ( $items as $item ) {
		if ( $item['enabled'] ) {
			$enabled[ $item['id'] ] = $item['name'];
			$count++;
		}
		else {
			$disabled[ $item['id'] ] = $item['name'];
		}
	}
    // Sidebar layout
	$fields[] = array(
        'id'      => _bootstrap_get_option_name( $page_name, $section_name, 'layout' ),
        'type'    => 'sorter',
        'title'   => sprintf( __( '%s Sidebar Layout', '_bootstrap' ), $name ),
        'desc'    => __( 'How do you want to layout the sidebars. Top to bottom will represent left to right', '_bootstrap' ),
        'options' => array( 'enabled'  => $enabled, 'disabled' => $disabled ),
	);	
	
	// widths
	foreach ( $items as $item ) {
		$fields[] = array(
	        'id' => _bootstrap_get_option_name( $page_name, $section_name, $item['id']. '_width' ),
	        'type' => 'slider',
	        'title' => sprintf( __( '%s Width', '_bootstrap' ), $item['name'] ),
	        'desc' => __( 'How many columns of Bootstrap 12 column grid should content occupy. If content is disabled above then this is ignored.', '_bootstrap' ),
	        "default" => $item['width'],
	        "min" => 0,
	        "step" => 1,
	        "max" => 12,
	        'display_value' => 'select',
	    ); 
    }
}

/**
* Get sidebar options.
* 
* @param string $page_name the page where the sidebar options are specified
* @param string $section_name the section of page where options are
* 
* @return array of sidebar options
*/
function _bootstrap_get_sidebar_options( $page_name, $section_name ) {
	$elements = _bootstrap_get_redux_option( $page_name, $section_name, 'layout', 'enabled' );
	$total_width = 0;
	foreach( $elements as $key => $value ) {
		$width[ $key ] = _bootstrap_get_redux_option( $page_name, $section_name, $key . '_width' );
		$total_width += $width[ $key ];
		$output['items'][ $key ] = $width[ $key ];
	}
	$output['total_width'] = $total_width;
	return $output;
}

/**
* Add the Bootstrap Area Container section.
* Includes width (fixed vs full) and container type (plain, panel, well, alert).
* 
* @param array $fields reference to the array of options to which new navbar options will be appended
* @param array $properties associative array of options used to customize the options
* 						   example : array ( 
										'page_name' : 'header', 
 										'section_name' : 'area_container', 
 										'name' : 'Header',
 									 )
* 
* @return void
*/
function _bootstrap_add_area_container_options( &$fields, $properties ) {
	extract( $properties );
	
	if ( ! isset( $section_name ) ) {
		$section_name = 'area_container';
	}
	
	// The title for this section
	$title = $name . __( ' Area', '_bootstrap' );
	
	// Title
	$fields[] = array( 
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
	    'type'     => 'raw',
	    'content'  => '<h1>' . $title . __( ' Enable and Width', '_bootstrap' ) . '</h1>',
	);	
	
    // Show Area or Not
	$fields[] = array(
        'id'		=>  _bootstrap_get_option_name( $page_name, $section_name, 'show' ),
        'type'		=> 'switch', 
        'title'		=> __( 'Show ', '_bootstrap' ) . $title,
        'desc'		=> sprintf( __( 'This controls whether the %s is shown or not', '_bootstrap' ), $title ),
        'on'		=> __( 'Show Area', '_bootstrap' ),
        'off'		=> __( 'Don\'t Show Area', '_bootstrap' ),
        'default'	=> TRUE,
    );	
		
	// Fixed or Full Width
	$fields[] = array(
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'width' ),
	    'type'     => 'button_set',
	    'title'    => $title . __( ' Container Width', '_bootstrap' ),
	    'desc'     => __( 'Fixed Width uses Bootstrap container class and Full Width uses Bootstrap container-fluid class. None is recommended only for header and footer navbars so that they stretch across the entire width.', '_bootstrap' ),
	    'options'  => array(
	    	'container'			=> 'Fixed Width',
	    	'container-fluid'	=> 'Full Width',
	    	'none'				=> 'None',
	    ),                
	    'default'  => $default_width,
	);	
	
	$properties['section_name'] = $section_name;
	$properties['include_title'] = FALSE;
	$properties['default_container'] = 'none';
	
	// Container options
	_bootstrap_add_container_styling_options( $fields, $properties );    	
}

/**
* Retrieve the options set for an area container.
* 
* @param string $page_name the name of the page whose options are being retreived
* @param string $section_name the optional section whose options are being retreived
* 							  If not specified then the 'area_container' is used as default
* 
* @return array of option values.
*/
function _bootstrap_get_area_container_options( $page_name, $section_name = 'area_container' ) {
	$options = _bootstrap_get_container_options( $page_name, $section_name );
	$options['show'] = _bootstrap_get_redux_option( $page_name, $section_name, 'show' );
	$options['container_class'] .= ' ' . _bootstrap_get_redux_option( $page_name, $section_name, 'width' );
	return $options;
}

/**
* Determines the Bootstrap classes for an area container.
* 
* @param string $page_name the page of options where the options are set
* @param string $section_name the section of the page where the options are set
* 
* @return string class to be applied to the area container div
*/
function _bootstrap_get_area_container_class( $page_name, $section_name = 'area_container' ) {
	$options = _bootstrap_get_area_container_options( $page_name, $section_name );
	return $options['container_class'];
}

/**
* Add the Bootstrap navbar option section.
* Includes style (default vs inverse), alignment (static vs fixed), and padding options when using fixed alignment navbar.
* 
* @param array $fields reference to the array of options to which new navbar options will be appended
* @param array $properties associative array of options used to customize the options
* 						   example : array ( 
										'page_name' : 'header', 
 										'section_name' : 'navbar', 
 										'name' : 'Header',
 										'js_location' : '/js/navbar.js',
 									 )
* 
* @return void
*/
function _bootstrap_add_navbar_options( &$fields, $properties ) {
	
	// Get the variable names
	extract( $properties );
	
	// Use default section name if not specified
	if ( ! isset( $section_name ) ) {
		$section_name = 'navbar';
	}
	
	// The title for this section
	$title = $name . __( ' Navbar', '_bootstrap' );
	$fields[] = array( 
	    'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
	    'type'     => 'raw',
	    'content'  => '<h1>' . $title . __( ' Options', '_bootstrap' ) . '</h1>',
	);	
	
    // Default or Inverse navbar.
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'style' ),
        'type'     => 'button_set',
        'title'    => $title . __( ' Style', '_bootstrap' ),
        'desc'     => __( 'Choose the Bootstrap style for the ', '_bootstrap' ) . $title . __( '. This will apply bootstrap classes navbar-default or navbar-inverse.', '_bootstrap' ),
        'options'  => array(
            'default'	=> 'Default',
            'inverse'	=> 'Inverse',
        ), 
        'default'  => 'default',
    );		    
    
    // Static or Fixed navbar.
    $alignment_field = _bootstrap_get_option_name( $page_name, $section_name, 'alignment' ); 
 	$fields[] = array(
        'id'       => $alignment_field,
        'type'     => 'button_set',
        'title'    => $title . __( 'Alignment', '_bootstrap' ),
        'desc'     => __( 'Static navbar stays on top of page. Fixed navbar moves and stays on top of screen even as you scroll. Applies class navbar-static and navabar-fixed.', '_bootstrap' ),
        'options'  => array(
            'static'	=> 'Static',
            'fixed'		=> 'Fixed',
        ), 
        'default'  => 'static',
    );	  
    
	// What type of padding to use on top for static navbar - constant or dynamic using jquery.
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'padding_style' ),
        'type'     => 'button_set',
        'required' => array( $alignment_field, 'equals', 'fixed' ),
        'title'    => __( 'Constant or Dynamic Padding for Fixed Navbar', '_bootstrap' ),
        'desc'     => __( 'Fixed navbar needs some padding at top so that content does overlap under the navbar. Should we use a constant padding (use text field below to specify) or dynamic (javascript code will be used).', '_bootstrap' ),
        'options'  => array(
            'constant'	=> 'Constant',
            'dynamic'	=> 'Dynamic',
        ), 
        'default'  => 'dynamic',
    );	    
    
    // The constant padding to use when constant padding is selected above
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'padding_constant' ),
        'type'     => 'text',
        'required' => array( $alignment_field, 'equals', 'fixed' ),
        'validate' => 'numeric',
        'title'    => __( 'How much padding to use on top', '_bootstrap' ),
        'desc'     => __( 'This will add a constant padding on top for body when using Fixed navbar. Used only when Constant Padding is selected above.', '_bootstrap' ),
        'default'  => '60',
    );		
    
    // Location of js file that calculates padding
    $fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'padding_js_location' ),
        'type'     => 'text',
        'required' => array( $alignment_field, 'equals', 'fixed' ),
        'title'    => __( 'Path to Custom Javascript File', '_bootstrap' ),
        'subtitle' => __( 'Relative to the theme root directory', '_bootstrap' ),
        'desc'     => __( 'If Dynamic Padding is chose above then the file specified here will loaded to calculate dynamic padding to apply.', '_bootstrap' ),
        'default'  => $js_location,
    );             	    	
}

/**
* Retrieve the options set for navbar in the given page name.
* 
* @param string $page_name the name of the page whose options are being retreived
* @param string $section_name the optional section whose options are being retreived
* 							  If not specified then the 'navbar' is used as default
* 
* @return array of option values.
*/
function _bootstrap_get_navbar_options( $page_name, $section_name = 'navbar' ) {
	$fields = array( 'style', 'alignment', 'padding_style', 'padding_constant', 'padding_js_location' );
	foreach( $fields as $field ) {
		$options[ $field] = _bootstrap_get_redux_option( $page_name, $section_name, $field);
	}
	return $options;
}

/**
* Determines the Bootstrap class for the navbar based on options set.
* 
* @param string $location the location of the navbar (top or bottom)
* @param string $page_name the page of options where the options are set
* @param string $section_name the section of the page where the options are set
* 
* @return string class to be applied to the navbar
*/
function _bootstrap_get_navbar_class ( $location, $page_name, $section_name = 'navbar' ) {
	$options = _bootstrap_get_navbar_options( $page_name, $section_name );
	return 'navbar navbar-' . $options['style'] . ' navbar-' . $options['alignment'] . '-' . $location;
}

function _bootstrap_show_footer_widgets() {
	// Get footer widget area options
	$page_name = 'footer';
	$section_name = 'widget_area_container';
	$options = _bootstrap_get_area_container_options( $page_name, $section_name );
	if ( $options['show'] ) {
		?>
		<div class="<?php echo $options['container_class']; ?>">
			<?php		
				// Get footer widget options
				$section_name = 'widgets';
				$options = _bootstrap_get_sidebar_options( $page_name, $section_name );
				if ( $options['total_width'] > 12 ) {
					?>
						<div class="alert alert-danger alert-dismissable">
						<?php if ( _bootstrap_js_enabled() ) { ?>
							<button type="button" class="close" data-dismiss="alert">&times;</button>
						<?php } 
						printf( __( 'Total width of content and sidebar columns (%d) exceeds 12. Content or Sidebar will wrap to the next row.', '_bootstrap' ), $options['total_width'] ); ?>
						</div>
					<?php	
				}
				foreach( $options['items'] as $key => $value ) {		
					if ( stristr( $key, 'sidebar') ) {
						if ($value > 0 && $value < 12 ) {
						?>
						<div class="col-sm-<?php echo $value; ?>">
							<?php get_sidebar( $key ); ?>
						</div>
						<?php
						}
					}
				}	
			?>		
		</div>	
	<?php
	}
}

function _bootstrap_show_copyrights() {
	// Get options for footer copyright area containter
	$page_name = 'footer';
	$section_name = 'copyright_area_container';
	$options = _bootstrap_get_area_container_options( $page_name, $section_name );
	$section_name = 'copyright_area';
	if ( $options['show'] ) {
	?>
		<div class="<?php echo $options['container_class']; ?>">
			<nav id="footer-menu" class="<?php echo _bootstrap_get_navbar_class( 'bottom', 'footer' ); ?>">
		  		<div class="container">
			  		<p class="navbar-text navbar-left">
			  			<?php echo _bootstrap_get_redux_option( $page_name, $section_name, 'left' ); ?>
			  		</p>
			  		<p class="navbar-text navbar-right">
			  			<?php echo _bootstrap_get_redux_option( $page_name, $section_name, 'right' ); ?>
			  		</p>
				</div>
			</nav>
		</div>
	<?php 
	}
}

function _bootstrap_show_header_contents() {
	$options = _bootstrap_get_area_container_options( 'header' );
	if ( $options['show'] ) {
	?>
	<nav role="navigation" class="<?php echo $options['container_class']; ?>">
		<div id="primary-menu" class="<?php echo _bootstrap_get_navbar_class( 'top', 'header' ); ?>">
				<!-- .navbar-toggle is used as the toggle for collapsed navbar content -->
				<div class="navbar-header">
					<button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target=".navbar-responsive-collapse">
						<span class="sr-only">
							Toggle navigation
						</span>
						<span class="icon-bar">
						</span>
						<span class="icon-bar">
						</span>
						<span class="icon-bar">
						</span>
					</button>
					<a class="navbar-brand" href="<?php bloginfo( 'url' ); ?>" title="Home">
						<?php bloginfo( 'name' ); ?>
					</a>
				</div>
				<div class="navbar-collapse navbar-responsive-collapse navbar-right collapse" style="height: 1px;">
					<?php
					wp_nav_menu( array(
						'menu'           => 'primary',
						'theme_location' => 'primary',
						'depth'          => 0,
						'container'      => 'div',
						'container_class'=> 'collapse navbar-collapse navbar-ex1-collapse',
						'menu_class'     => 'nav navbar-nav pull-right',
						'fallback_cb'=> 'wp_list_pages_bootstrap_navwalker::fallback',
						'walker'         => new wp_bootstrap_navwalker('NO_CARET'),)
					);
					?>
				</div>
		</div>
	</nav>	
	<?php
	}
}
