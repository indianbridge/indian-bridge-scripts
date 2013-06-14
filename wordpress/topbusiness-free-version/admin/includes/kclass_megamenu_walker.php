<?php

class kclass_megamenu_walker extends Walker_Nav_Menu 
{
	function start_el(&$output, $item, $depth, $args) 
	{
		global $wp_query;
		$indent = ( $depth ) ? str_repeat( "\t", $depth ) : '';

		$class_names = $value = '';
		$basic_title = '';

		$classes = empty( $item->classes ) ? array() : (array) $item->classes;

		$class_names = join( ' ', apply_filters( 'nav_menu_css_class', array_filter( $classes ), $item ) );
		$class_names = ' class="' . esc_attr( $class_names );

			$attr = $item->attr_title;
			if (!($attr)) {
				$class_names .= " basic";
			}
			elseif (
				($attr == "mega-3") ||
				($attr == "mega-4") ||
				($attr == "mega-5") ||
				($attr == "mega-6") ) {
					$class_names .= " basic-mega ".$attr;
			}
			elseif (!(is_numeric($attr))) {
				$class_names .= " basic";
				$basic_title = ' title="'.$attr.'"';
			}

		$class_names .= '"';

		$output .= $indent . '<li' . $value . $class_names .'>';

		$attributes = ! empty( $item->target ) ? ' target="' . esc_attr( $item->target ) .'"' : '';
		$attributes .= ! empty( $item->xfn ) ? ' rel="' . esc_attr( $item->xfn ) .'"' : '';
		$attributes .= ! empty( $item->url ) ? ' href="' . esc_attr( $item->url ) .'"' : '';

		$megamenu = '';

		if ($attr) {
			if (is_numeric($attr)) {
				$megamenu = '<ul class="megamenu"><li>';
					$page_data = get_page($attr);
					$content = $page_data->post_content;
				$megamenu .= do_shortcode($content);
				$megamenu .= '</li></ul>';
			} else {

			}
		}
		
		if ($depth != 0) {$megamenu = '';}

		$item_output = '';
		$item_output .= '<a'.$attributes.$basic_title.'>';
		$item_output .= apply_filters( 'the_title', $item->title, $item->ID );
		$item_output .= '</a>';
		$item_output .= $megamenu;
	
		$output .= apply_filters( 'walker_nav_menu_start_el', $item_output, $item, $depth, $args );
	}
}
?>