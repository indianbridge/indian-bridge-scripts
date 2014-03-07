<?php
/**
 * Sidebar related code.
 * Register sidebars and style them for presentation.
 *
 * @package _bootstrap
 */

/**
 * Register widgetized area and update sidebar with default widgets.
 */
function _bootstrap_widgets_init() {
	$page_name = 'sidebar';
	$section_name = 'sidebar';
	$options = _bootstrap_get_container_options( $page_name, $section_name );
	$before_widget = '<aside id="%1$s" class="' . $options['container_class'] .' widget %2$s">';
	$after_widget = '</section></aside>';
	$before_title = '<header class="' . $options['header_class'] . '" >';
	$before_title .= '<' . $options['title_tag'] . ' class="' . $options['title_class'] . '" >';
	$after_title = '</' . $options['title_tag'] . '></header>';
	$after_title .= '<section class="' . $options['body_class'] . '">';
	register_sidebar( array(
		'name'          => __( 'Content Sidebar 1', '_bootstrap' ),
		'id'            => 'sidebar-1',
		'before_widget' => $before_widget,
		'after_widget'  => $after_widget,
		'before_title'  => $before_title,
		'after_title'   => $after_title,
	) );
	register_sidebar( array(
		'name'          => __( 'Content Sidebar 2', '_bootstrap' ),
		'id'            => 'sidebar-2',
		'before_widget' => $before_widget,
		'after_widget'  => $after_widget,
		'before_title'  => $before_title,
		'after_title'   => $after_title,
	) );	
}
add_action( 'init', '_bootstrap_widgets_init' );
?>