<?php
/**
 * The Sidebar containing sidebar-1 widgets.
 *
 * @package _bootstrap
 */

	// Get the options
	if ( ! dynamic_sidebar( $name ) ) {
		// Set the no widget in sidebar contents
		get_template_part( 'sidebar', 'none' );
	} // end sidebar widget area
