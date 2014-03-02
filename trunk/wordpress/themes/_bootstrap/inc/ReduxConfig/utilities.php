<?php

// Retreive a saved Redux option
if ( !function_exists( '_bootstrap_get_option' ) ) :
/*
 * Gets the current values from REDUX, and if not there, grabs the defaults
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
endif;
