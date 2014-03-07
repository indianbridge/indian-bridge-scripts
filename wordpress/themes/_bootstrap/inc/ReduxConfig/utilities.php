<?php
/**
 * Utility functions to retrieve option values.
 * If redux is installed it will use the retreived values.
 * If not then a pre-defined set of values will be used (this needs to be implemented).
 *
 * @package _bootstrap
 */
 
/**
* Build the options from prefixes. 
* 
* @param string $page_name the page in which this option occurs.
* @param string $section_name the section in which this option occurs.
* @param string $option_name the actual option name.
* 
* @return string full option name
*/
function _bootstrap_get_option_name( $page_name, $section_name, $option_name ) {
	return THEME_NAME . '_' . $page_name . '_' . $section_name . '_' . $option_name;
}

/**
 * Get the value of speficied option.
 *
 * @param $name the string name of option.
 * @param key the string key whose value is needed.
 * @return mixed the value of the option.
 */ 
if ( !function_exists( '_bootstrap_get_option' ) ) {

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
}

/**
* Create the options layout for a container (could be for content or sidebar etc.)
* 
* @param array $fields reference to array of fields to which we will add styling options.
* @param string $page_name the name of the page this will go into.
* @param string $section_name the name of the section this will go into.
* @param string $prefix the name of thing we are styling
* 
* @return nothing
*/
function _bootstrap_add_container_styling_options( &$fields, $page_name, $section_name, $prefix ) {
	
	$fields[] = array( 
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title' ),
        'type'     => 'raw',
        'content'  => '<h1>' . sprintf( __( '%s Title, Container and Text Styling', '_bootstrap' ), $prefix ) . '</h1>',
    );	
    
    // The container title style
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'title_style' ),
        'type'     => 'button_set',
        'title'    => $prefix . __( ' Title Style', '_bootstrap' ),
        'desc'     => sprintf( __( 'How should the %s Title be styled. If you choose custom enter tag name in the text field that appears.', '_bootstrap' ), $prefix ),
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
        'title'    => sprintf( __( 'Custom tag for %s Title', '_bootstrap'), $prefix ),
        'desc'     => __( 'This will be used only when custom option is selected above.', '_bootstrap' ),
        'default'  => 'div'
    );	     
    	
    // The container style
 	$fields[] = array(
        'id'       => _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ),
        'type'     => 'button_set',
        'title'    => $prefix . __( ' Container Style', '_bootstrap' ),
        'desc'     => sprintf( __('How should the %s Container be styled', '_bootstrap' ), $prefix ),
        'options'  => array(
            'plain' 	=> 'Plain Text',
            'panel' 	=> 'Bootstrap Panel',
            'well' 		=> 'Bootstrap Well',
            'alert'		=> 'Bootstrap Alert',
        ), 
        'default'  => 'panel',
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
* 
* @param string $page_name the name of page where options will be found.
* @param string $section_name the name of section where options will be found.
* 
* @return associative array of classes for styling content
*/
function _bootstrap_get_container_options( $page_name, $section_name ) {
	// The style for the container
	$output['style'] = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ) );

	$class_id = '';
	switch ( $output['style'] ) {
		// Bootstrap panel container
		case 'panel' :
			$class_id = _bootstrap_get_option_name( $page_name, $section_name, 'class_panel' );
			break;
		
		// Bootstrap alert container
		case 'alert' :
			$class_id = _bootstrap_get_option_name( $page_name, $section_name, 'class_alert' );
			break;
			
		// Bootstrap well and plain text
		case 'well' :
		case 'plain' :
		default :
			$class_id = _bootstrap_get_option_name( $page_name, $section_name, 'class_text' );
			break;	
	}

	// The class of the container or the text within.
	$class = _bootstrap_get_option( $class_id );

	// The style to be applied to title.
	$output['title_tag'] = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'title_style' ) );
	if ( $output['title_tag'] === 'custom' ) {
		$output['title_tag'] = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'title_custom' ) );
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
		default :
			$output['container_class'] = 'text-' . $class;
			break;
	}		
	return $output;	
}
