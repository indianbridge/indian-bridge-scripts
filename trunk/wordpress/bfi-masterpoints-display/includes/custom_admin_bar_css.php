<?php

/**
 * Class to customize admin bar including adding some styles
 */

if (!class_exists('BFI_Custom_Admin_Bar')) {

	class BFI_Custom_Admin_Bar {
		public function __construct() {
			// Add some custom styles to admin bar drop down text otherwise myForums and myMasterpoints look weird
			add_action('wp_head', array($this, 'add_custom_wpadminbar_css'));
			// Customize the admin bar
			add_action('wp_before_admin_bar_render', array($this, 'customize_admin_bar'));
		}

		public function customize_admin_bar() {
			global $wp_admin_bar;
			if (!is_user_logged_in() || current_user_can('subscriber')) {
				$wp_admin_bar -> remove_menu('site-name');
			}
			if (is_user_logged_in()) {
				$my_account = $wp_admin_bar -> get_node('my-account');
				$newtitle = str_replace('Howdy,', 'Namaste', $my_account -> title);
				$wp_admin_bar -> add_node(array('id' => 'my-account', 'title' => $newtitle));
			}
		}

		public function add_custom_wpadminbar_css() {
			echo '<style type="text/css">
				#wpadminbar .ico {
					padding: 0 0 0 20px;
					background-position: left 3px;
					background-repeat: no-repeat;
					color: #000;
				}

				#wpadminbar .set-width {
					width: 300px;
				}

				#wpadminbar h6 {
					text-decoration: none;
					border: none !important;
					background: #333;
					color: #FFF;
					white-space: nowrap;
					line-height: 1;
					font-size: 11px;
					font-weight: bold;
					border-radius: 2px !important;
					margin: 0 !important;
					padding: 6px !important;
				}
			</style>';
		}

	}

}
?>