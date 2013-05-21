<?php
/*
Plugin Name: Wordpress BFI Masterpoint Display
Plugin URI: http://bfi.net.in
Description: A plugin to show posts filtered by categories using ajax
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'BFI_Masterpoint_Display' ) ) {
	

	class BFI_Masterpoint_Display {
		
		public function __construct() {		
			// register the javascript for the ajax function
			wp_register_script( 'jquery_datatables', plugins_url( 'jquery.dataTables.js', __FILE__ ), array( 'jquery' ) );	
			wp_enqueue_script('jquery_datatables');
			wp_register_script( 'bfi_masterpoint_display_js', plugins_url( 'bfi-masterpoints-display.js', __FILE__ ), array( 'jquery' ) );	
			wp_enqueue_script('bfi_masterpoint_display_js');			
			
			// register the css
			wp_register_style( 'jquery_datatables_css', plugins_url( 'css/jquery.dataTables.css', __FILE__ ) );	
			wp_enqueue_style('jquery_datatables_css');			
			
			// Add the shortcode
			add_shortcode('bfi_masterpoint_display', array($this, 'bfi_masterpoint_display'));
		}
		
		/**
		 * Replace shortcode with posts
		 */
		function bfi_masterpoint_display ($atts, $content = null ) {
			/*$newdb = new wpdb('indianbridge', 'kibitzer', 'masterpoints', 'localhost');
			$newdb->show_errors();
			echo 'Testing shortcode';
			$retrieve_data = $newdb->get_results( "SELECT * FROM tournament_masterpoint WHERE member_id = 'TM000277'" );
			echo '<table id="bfi_masterpoint_table" class="dataTable"><thead><tr><th>Event</th><th>Points</th></tr></thead><tbody>';
			foreach ($retrieve_data as $retrieved_data) {
				echo '<tr><td>'.$retrieved_data->event_code.'</td><td>'.$retrieved_data->fedpoints_earned.'</td></tr>';
				//echo '<li>'.$retrieved_data->fedpoints_earned.'</li>';
			}
			echo '</tbody></table>';
			echo '<script type="text/javascript">';
			echo "$('#bfi_masterpoint_table').dataTable({\"bSort\":true});";
			echo '</script>';*/
			echo '<h1>Testing shortcode</h1>';
			echo '<div id="demo"></div>';
			echo '<script type="text/javascript" language="javascript">renderTable("http://localhost/bfitest/wp-content/plugins/bfi-masterpoints-display/server_processing_old.php");</script>';
		}	
		
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_masterpoint_display'] = new BFI_Masterpoint_Display();
}