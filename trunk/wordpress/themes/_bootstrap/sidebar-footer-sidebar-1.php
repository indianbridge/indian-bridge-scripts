<?php
/**
 * The Sidebar containing sidebar-1 widgets.
 *
 * @package _bootstrap
 */

	// Get the options
	if ( ! dynamic_sidebar( 'footer-sidebar-1' ) ) {
		// Set the no widget in sidebar contents
		get_template_part( 'sidebar', 'none' );
	} // end sidebar widget area
