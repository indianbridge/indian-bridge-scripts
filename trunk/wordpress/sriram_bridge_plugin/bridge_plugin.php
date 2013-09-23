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
			add_shortcode( 'bbo', array($this,'display_bbo_hand_viewer') );
        }      
		
		public function display_bbo_hand_viewer($atts, $content="" ) {
			return '<iframe height="300" src="'.$content.'">Your browser does not support iFrames</iframe>';
		}

		public function replace_suit_symbols($content) {
			$content = str_ireplace('!s', '&spades;',$content);
			$content = str_ireplace('!h', '<span style="color:#CB0000;">&hearts;</span>',$content);
			$content = str_ireplace('!d', '<span style="color:#CB0000;">&diams;</span>',$content);
			$content = str_ireplace('!c', '&clubs;',$content);
			return $content;
		}
    }

    // finally instantiate our plugin class and add it to the set of globals
    $GLOBALS['sriram_bridge_plugin'] = new Sriram_Bridge_Plugin();
}