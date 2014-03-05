<?php
/**
 * Sidebar related code.
 * Register sidebars and style them for presentation.
 *
 * @package _bootstrap
 */

if ( ! function_exists( '_bootstrap_get_sidebar_options' ) ) {
	/**
	 * Retrieve options specified for sidebar.
	 *
	 * @return an associative array of options
	 */	
	function _bootstrap_get_sidebar_options() {
		// The type of the container to be used display each widget.
		$output['style'] = _bootstrap_get_option( '_bootstrap_sidebar_style' );

		$class_id = '';
		switch ( $output['style'] ) {
			// Bootstrap panel container
			case 'panel' :
				$class_id = '_bootstrap_sidebar_class_panel';
				break;
			
			// Bootstrap alert container
			case 'alert' :
				$class_id = '_bootstrap_sidebar_class_alert';
				break;
				
			// Bootstrap well and plain text
			case 'well' :
			case 'plain' :
			default :
				$class_id = '_bootstrap_sidebar_class_text';
				break;	
		}

		// The class of the container or the text within.
		$class = _bootstrap_get_option( $class_id );

		// The style to be applied to title.
		$output['title_tag'] = _bootstrap_get_option( '_bootstrap_sidebar_title_style' );
		if ( $output['title_tag'] === 'custom' ) {
			$output['title_tag'] = _bootstrap_get_option( '_bootstrap_sidebar_title_custom' );
		}

		// Classes for article sections that will conditionalized based on type of container being used.
		$output['container_class'] = '';
		$output['header_class'] = '';
		$output['title_class'] = '';
		$output['body_class'] = '';

		// Setup classes for all sections of the article depending on type of container selected
		switch ( $output['style'] ) {
			
			// Bootstrap panel container
			case 'panel' :
				$output['container_class'] = 'panel panel-' . $class; 
				$output['header_class'] = 'panel-heading';
				$output['title_class'] = 'panel-title';
				$output['body_class'] = 'panel-body';
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
}

/**
 * Register widgetized area and update sidebar with default widgets.
 */
function _bootstrap_widgets_init() {
	$options = _bootstrap_get_sidebar_options();
	$before_widget = '<aside id="%1$s" class="' . $options['container_class'] .' widget %2$s">';
	$after_widget = '</section></aside>';
	$before_title = '<header class="' . $options['header_class'] . '" >';
	$before_title .= '<' . $options['title_tag'] . ' class="' . $options['title_class'] . '" >';
	$after_title = '</' . $options['title_tag'] . '></header>';
	$after_title .= '<section class="' . $options['body_class'] . '">';
	register_sidebar( array(
		'name'          => __( 'Sidebar', '_bootstrap' ),
		'id'            => 'sidebar-1',
		'before_widget' => $before_widget,
		'after_widget'  => $after_widget,
		'before_title'  => $before_title,
		'after_title'   => $after_title,
	) );
}
add_action( 'init', '_bootstrap_widgets_init' );
?>