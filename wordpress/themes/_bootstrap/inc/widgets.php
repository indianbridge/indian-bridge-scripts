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
	_bootstrap_register_sidebars( $page_name, $section_name, 'Content', 2 );
	$page_name = 'footer';
	$section_name = 'sidebar';	
	_bootstrap_register_sidebars( $page_name, $section_name, 'Footer', 3 );

}
add_action( 'init', '_bootstrap_widgets_init' );

/**
* Register sidebars for any area.
* 
* @param int $number_of_sidebars the number of sidebars to register in the footer.
* 
* @return nothing
*/

/**
* Register sidebars for any area.
* 
* @param string $page_name the options page where the styling options for this panel will be.
* @param string $section_name the options section where styling will be.
* @param string $area the name of the area this sidbar will go to.
* @param int $number_of_sidebars the number of sidebars to register.
* 
* @return
*/
function _bootstrap_register_sidebars( $page_name, $section_name, $area, $number_of_sidebars ) {
	$options = _bootstrap_get_container_options( $page_name, $section_name );
	$before_widget = '<aside id="%1$s" class="' . $options['container_class'] .' widget %2$s">';
	$after_widget = '</section></aside>';
	$before_title = '<header class="' . $options['header_class'] . '" >';
	$before_title .= '<' . $options['title_tag'] . ' class="' . $options['title_class'] . '" >';
	$after_title = '</' . $options['title_tag'] . '></header>';
	$after_title .= '<section class="' . $options['body_class'] . '">';
	for ( $i = 1; $i <= $number_of_sidebars; $i++ ) {
		$id = sprintf( '%s-sidebar-%d', strtolower( $area ), $i );
		register_sidebar( array(
			'name'          => sprintf( __( '%s Sidebar %d', '_bootstrap' ), $area, $i ),
			'id'            => $id,
			'before_widget' => $before_widget,
			'after_widget'  => $after_widget,
			'before_title'  => $before_title,
			'after_title'   => $after_title,
		) );	
		$file_name = THEME_DIR . sprintf( '/sidebar-%s.php', $id );
		if ( ! file_exists( $file_name) ) {
			$template_file = THEME_DIR . '/sidebar-template.php';
			$contents = file_get_contents( $template_file );
			$contents = str_replace( '$name', "'" . $id . "'", $contents );
			file_put_contents( $file_name, $contents );
		}
	}	
}
?>