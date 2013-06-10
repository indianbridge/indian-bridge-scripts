<?php

	class Walker_Nav_Menu_Dropdown extends Walker_Nav_Menu{

		function start_lvl(&$output, $depth) {
			// do not remove
		}

		function end_lvl(&$output, $depth) {
			// do not remove
		}

		function start_el(&$output, $item, $depth, $args){
	
			global $wp_query;

			$class_names = $value = '';

			$classes = empty( $item->classes ) ? array() : (array) $item->classes;

			$classes[] = 'menu-item-' . $item->ID;

			$selected = '';

			foreach ($classes as &$class) :

				if ( $class == 'current-menu-item' ) :
					$selected = ' selected';
				endif;

			endforeach;

			$output .= '<option value="' . $item->url . '"' . $selected . '>';
	
			$item->title = str_repeat("-&nbsp;", $depth * 1).$item->title;
	
			$item_output .= apply_filters( 'the_title', $item->title, $item->ID ) . "</option>\n";
	
			$output .= apply_filters( 'walker_nav_menu_start_el', $item_output, $item, $depth, $args );
	
		}

		function end_el(&$output, $item, $depth) {
			// do not remove
		}

	}

?>