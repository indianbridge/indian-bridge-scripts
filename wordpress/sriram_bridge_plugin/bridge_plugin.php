<?php

/*
  Plugin Name: Sriram Bridge Plugin
  Plugin URI: http://bfi.net.in
  Description: A plugin to display suit symbols and bridge hands and auctions
  Author: Sriram Narasimhan
  Author URI: ...
  Version: 1.0

  Copyright: � 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
  License: GNU General Public License v3.0
  License URI: http://www.gnu.org/licenses/gpl-3.0.html
 */


if (!class_exists('Sriram_Bridge_Plugin')) {


    class Sriram_Bridge_Plugin {
    	
		private $images_directory;
        public function __construct() {
        	add_filter('the_content',array($this,'replace_suit_symbols'));
        }      

		public function replace_suit_symbols($content) {
			$content = str_ireplace('!s', '<img src="' . plugins_url( 'images/s.png' , __FILE__ ) . '" > ', $content);
			$content = str_ireplace('!h', '<img src="' . plugins_url( 'images/h.png' , __FILE__ ) . '" > ', $content);
			$content = str_ireplace('!d', '<img src="' . plugins_url( 'images/d.png' , __FILE__ ) . '" > ', $content);
			$content = str_ireplace('!c', '<img src="' . plugins_url( 'images/c.png' , __FILE__ ) . '" > ', $content);
			return $content;
		}
    }

    // finally instantiate our plugin class and add it to the set of globals
    $GLOBALS['sriram_bridge_plugin'] = new Sriram_Bridge_Plugin();
}