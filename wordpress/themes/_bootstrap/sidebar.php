<?php
/**
 * The Sidebar containing the main widget areas.
 *
 * @package _bootstrap
 */

	// Get the options
	if ( ! dynamic_sidebar( 'sidebar-1' ) ) {
		// Set the no widget in sidebar contents
		get_template_part( 'sidebar', 'none' );
	} // end sidebar widget area
