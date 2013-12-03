<?php
/*
Plugin Name: Bridge Federation of India Plugins
Plugin URI: http://bfi.net.in
Description: A plugin to setup a variety of Bridge Federation of India related functions
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: � 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/

require_once (plugin_dir_path(__FILE__) . 'includes' . DIRECTORY_SEPARATOR . 'utilities.php');
if (!class_exists('BridgeFederationOfIndia_Plugins')) {
	

	class BridgeFederationOfIndia_Plugins {
		
		public function __construct() {
		}		
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['BridgeFederationOfIndia_Plugins'] = new BridgeFederationOfIndia_Plugins();
}