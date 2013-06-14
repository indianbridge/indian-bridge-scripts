<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;


	global $pagenow;


	// Thickbox on admin page
	if ( is_admin() ) :
		wp_enqueue_script('thickbox', null, array('jquery'));
		wp_enqueue_style('thickbox.css', '/'.WPINC.'/js/thickbox/thickbox.css', false, null);
	endif;


	// Scripts on admin page
	if ( $pagenow == 'admin.php' || $pagenow == 'widgets.php' ) :
		wp_register_script('pa2', get_bloginfo('template_directory').'/admin/js/colorpicker/js/colorpicker.js', false, null);				wp_enqueue_script('pa2', false, null);
		wp_register_script('pa3', get_bloginfo('template_directory').'/admin/js/admin.js', false, null);									wp_enqueue_script('pa3', false, null);
	endif;


	// Control Panel
	class ControlPanel {
	
		var
			$default_settings,
			$options,
			$headerr,
			$sidebars,
			$slider,
			$products,
			$style,
			$typography,
			$buddypress,
			$misc;


		function ControlPanel() {

			add_action('admin_menu', array(&$this, 'add_menu'));
			add_action('admin_menu', array(&$this, 'add_headerr_menu'));
			add_action('admin_menu', array(&$this, 'add_sidebars_menu'));
			add_action('admin_menu', array(&$this, 'add_slider_menu'));
			add_action('admin_menu', array(&$this, 'add_products_menu'));
			add_action('admin_menu', array(&$this, 'add_style_menu'));
			add_action('admin_menu', array(&$this, 'add_typography_menu'));
			add_action('admin_menu', array(&$this, 'add_buddypress_menu'));
			add_action('admin_menu', array(&$this, 'add_misc_menu'));

			add_action('admin_head', array(&$this, 'admin_head'));


			// Theme settings page
			if (!is_array(get_option('ikarina'))) :
				add_option('ikarina', $this->default_settings);
				$this->options = get_option('ikarina');
			endif;


			// Header page
			if (!is_array(get_option('panda_he'))) :
				add_option('panda_he', $this->default_settings);
				$this->headerr = get_option('panda_he');
			endif;


			// Sidebars page
			if (!is_array(get_option('panda_sb'))) :
				add_option('panda_sb', $this->default_settings);
				$this->sidebars = get_option('panda_sb');
			endif;


			// Slider page
			if (!is_array(get_option('panda_sl'))) :
				add_option('panda_sl', $this->default_settings);
				$this->slider = get_option('panda_sl');
			endif;


			// Products page
			if (!is_array(get_option('panda_pr'))) :
				add_option('panda_pr', $this->default_settings);
				$this->products = get_option('panda_pr');
			endif;


			// Style page
			if (!is_array(get_option('panda_st'))) :
				add_option('panda_st', $this->default_settings);
				$this->style = get_option('panda_st');
			endif;


			// Typography page
			if (!is_array(get_option('panda_ty'))) :
				add_option('panda_ty', $this->default_settings);
				$this->typography = get_option('panda_ty');
			endif;


			// BuddyPress page
			if (!is_array(get_option('panda_bp'))) :
				add_option('panda_bp', $this->default_settings);
				$this->buddypress = get_option('panda_bp');
			endif;


			// Misc page
			if (!is_array(get_option('panda_mi'))) :
				add_option('panda_mi', $this->default_settings);
				$this->misc = get_option('panda_mi');
			endif;

		}


		function add_menu() {
			add_object_page(__('Theme settings','pandathemes'), __('Theme settings','pandathemes'), 'edit_theme_options', "theme-settings", array(&$this, 'options_menu'));
		}

		function add_headerr_menu() {
			add_submenu_page ( "theme-settings", __('Header','pandathemes'), __('Header','pandathemes'), 'edit_theme_options', "headerr", array(&$this, 'headerr_menu'));
		}

		function add_sidebars_menu() {
			add_submenu_page ( "theme-settings", __('Sidebars','pandathemes'), __('Sidebars','pandathemes'), 'edit_theme_options', "sidebars", array(&$this, 'sidebars_menu'));
		}

		function add_slider_menu() {
			add_submenu_page ( "theme-settings", __('Slider','pandathemes'), __('Slider','pandathemes'), 'edit_theme_options', "slider", array(&$this, 'slider_menu'));
		}

		function add_products_menu() {
			add_submenu_page ( "theme-settings", __('Products','pandathemes'), __('Products','pandathemes'), 'edit_theme_options', "products", array(&$this, 'products_menu'));
		}

		function add_style_menu() {
			add_submenu_page ( "theme-settings", __('Style','pandathemes'), __('Style','pandathemes'), 'edit_theme_options', "style", array(&$this, 'style_menu'));
		}

		function add_typography_menu() {
			add_submenu_page ( "theme-settings", __('Typography','pandathemes'), __('Typography','pandathemes'), 'edit_theme_options', "typography", array(&$this, 'typography_menu'));
		}

		function add_buddypress_menu() {
			if ( is_super_admin() ) :
				add_submenu_page ( "theme-settings", 'BuddyPress', 'BuddyPress', 'edit_theme_options', "buddypress", array(&$this, 'buddypress_menu'));
			endif;
		}

		function add_misc_menu() {
			add_submenu_page ( "theme-settings", 'Miscellaneous', 'Miscellaneous', 'edit_theme_options', "misc", array(&$this, 'misc_menu'));
		}


		function admin_head() {
			echo '
				<link rel="stylesheet" href="'.get_bloginfo('template_url').'/admin/css/admin.css" type="text/css" media="screen" />
				<link rel="stylesheet" media="screen" type="text/css" href="'.get_bloginfo('template_url').'/admin/js/colorpicker/css/colorpicker.css" />
			';
		}


		function options_menu() {

			if ($_POST['ikarina_action'] == 'save') :

				if (!$_POST['tab']) : $this->options["tab"] = 'general'; else : $this->options["tab"] = $_POST['tab']; endif;


				// G E N E R A L

				// Logo
				$this->options["logo_type"] = $_POST['logo_type'];
					$this->options["logo"] = $_POST['logo'];
					$this->options["sitename"] = $_POST['sitename'];
					$this->options["sitedesc"] = $_POST['sitedesc'];

				// Favicon
				$this->options["favicon"] = $_POST['favicon'];

				// GA				
				$this->options["google_analytics"] = stripslashes($_POST['google_analytics']);

				// Copyright
				$this->options["copyrights"] = stripslashes($_POST['copyrights']);
				$this->options["dev_copyright"] = $_POST['dev_copyright'];
					$this->options['dev_copyright_data'] = stripslashes($_POST['dev_copyright_data']);


				// P O S T S

				// Templates
				$this->options["blog_template"] = $_POST['blog_template'];
				$this->options["posts_qty"] = $_POST['posts_qty'];

				// Common
				$this->options["square_thumbs"] = $_POST['square_thumbs'];
					$this->options["thumbs_crop_top"] = $_POST['thumbs_crop_top'];

				$this->options["post_metas"] = $_POST['post_metas'];
					$this->options["author_info"] = $_POST['author_info'];

				// Post page
				$this->options["before_post"] = $_POST['before_post'];
					$this->options["before_post_data"] = stripslashes($_POST['before_post_data']);

				$this->options["after_title"] = $_POST['after_title'];
					$this->options["after_title_data"] = stripslashes($_POST['after_title_data']);

				$this->options["post_feat_image"] = $_POST['post_feat_image'];
					$this->options["post_feat_image_link"] = $_POST['post_feat_image_link'];

				$this->options["excerpt"] = $_POST['excerpt'];

				$this->options["after_post"] = $_POST['after_post'];
					$this->options["after_post_data"] = stripslashes($_POST['after_post_data']);

				// Comments
				$this->options["post_comments"] = $_POST['post_comments'];
					$this->options["gravatars_on_comments"] = $_POST['gravatars_on_comments'];
					$this->options["dates_on_comments"] = $_POST['dates_on_comments'];
					$this->options["website_on_comments"] = $_POST['website_on_comments'];
					$this->options["post_trackbacks"] = $_POST['post_trackbacks'];

				// Image placeholder
				$this->options["img_placeholder"] = $_POST['img_placeholder'];

				// Custom gravatar
				$this->options["gravatar"] = $_POST['gravatar'];


				// P A G E S

				// Comments on page
				$this->options["pages_metas"] = $_POST['pages_metas'];

				// Update settings
				update_option('ikarina', $this->options);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';

	
			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form.php');

		} // end options_menu()



		// ********************************************
		//
		//	H E A D E R
		//
		// ********************************************



		function headerr_menu() {

			if ($_POST['panda_he_action'] == 'save') :
	

				if (!$_POST['tab']) : $this->headerr["tab"] = 'general'; else : $this->headerr["tab"] = $_POST['tab']; endif;

				// Notice for visitors
				$this->headerr["notice"] = $_POST['notice'];
				$this->headerr["notice_data"] = stripslashes($_POST['notice_data']) ? stripslashes($_POST['notice_data']) : '<div style="background:#FFFADE; padding:6px 0px;"><div style="background:url('.get_bloginfo('template_url').'/images/icons/16/led-icons/lightbulb.png) 0 50% no-repeat; width:940px; margin:0 auto; padding:0 0 0 20px;">Hello World!</div></div>';

				// Custom data
				$this->headerr["hcustom"] = $_POST['hcustom'];
				$this->headerr["hcustom_data"] = stripslashes($_POST['hcustom_data']);

				// Lifestream
				$array = array (
					"lifestream",
					"lifestream_custom",
					"life_rss",
					"life_twitter",
					"life_deviantart",
					"life_digg",
					"life_facebook",
					"life_flickr",
					"life_lastfm",
					"life_linkedin",
					"life_myspace",
					"life_picasa",
					"life_reddit",
					"life_skype",
					"life_stumbleupon",
					"life_technorati",
					"life_youtube",
					"life_vimeo",
					"life_blogger",
					"life_delicious",
					"life_designfloat",
					"life_designmoo"
				);
				foreach ( $array as $a ) :
					$this->headerr[$a] = stripslashes($_POST[$a]);
				endforeach;

				// Search form
				$this->headerr["searchform"] = $_POST['searchform'];

				// Message on menu bar
				$this->headerr["topmessage"] = stripslashes($_POST['topmessage']);


				// Update settings
				update_option('panda_he', $this->headerr);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';


			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_headerr.php');

		}



		// ********************************************
		//
		//	S I D E B A R S
		//
		// ********************************************



		function sidebars_menu() {

			if ($_POST['panda_sb_action'] == 'save') :
	

				if (!$_POST['tab']) : $this->sidebars["tab"] = 'general'; else : $this->sidebars["tab"] = $_POST['tab']; endif;

				// Additional sidebars
				$this->sidebars["additional_sidebars"] = $_POST['additional_sidebars'];

				// Footer sidebars
				$this->sidebars["footer_sidebars"] = $_POST['footer_sidebars'];


				// Update settings
				update_option('panda_sb', $this->sidebars);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';


			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_sidebars.php');

		}



		// ********************************************
		//
		//	S L I D E R
		//
		// ********************************************



		function slider_menu() {

			if ($_POST['panda_sl_action'] == 'save') :


				if (!$_POST['tab']) : $this->slider["tab"] = 'general'; else : $this->slider["tab"] = $_POST['tab']; endif;

				// Slider
				$this->slider["slider"] = $_POST['slider'];

				// On pages
				$array = array (
					"slider_on_homepage",
					"slider_on_page",
					"slider_on_post",
					"slider_on_blog",
					"slider_on_archives",
					"slider_on_product",
					"slider_on_poducts_archives",
					"slider_on_search",
					"slider_on_404"
				);
				foreach ( $array as $a ) :
					$this->slider[$a] = $_POST[$a];
				endforeach;	


				// Update settings
				update_option('panda_sl', $this->slider);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';

	
			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_slider.php');

		}



		// ********************************************
		//
		//	P R O D U C T S
		//
		// ********************************************



		function products_menu() {

			if ($_POST['panda_pr_action'] == 'save') :


				if (!$_POST['tab']) : $this->products["tab"] = 'general'; else : $this->products["tab"] = $_POST['tab']; endif;

				// Activation
				$this->products["products"] = $_POST['products'];

				// Templates
				$this->products["product_template"] = $_POST['product_template'];

				// Catalog page
				$this->products["posts_per_page"] = $_POST['posts_per_page'];

				$this->products["after_catalog"] = $_POST['after_catalog'];
				$this->products["after_catalog_data"] = stripslashes($_POST['after_catalog_data']);

				// Product page
				$this->products["reviews"] = $_POST['reviews'];

				$this->products["related_products"] = $_POST['related_products'];
				$this->products["related_products_qty"] = $_POST['related_products_qty'];

				$this->products["after_product"] = $_POST['after_product'];
				$this->products["after_product_data"] = stripslashes($_POST['after_product_data']);

				$this->products["after_purchase"] = $_POST['after_purchase'];
				$this->products["after_purchase_data"] = stripslashes($_POST['after_purchase_data']);

				$this->products["tab_desc"] = stripslashes($_POST['tab_desc']);
				$this->products["tab_spec"] = stripslashes($_POST['tab_spec']);
				$this->products["tab_imgs"] = stripslashes($_POST['tab_imgs']);
				$this->products["tab_rews"] = stripslashes($_POST['tab_rews']);

				// Update settings
				update_option('panda_pr', $this->products);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';

	
			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_products.php');

		}



		// ********************************************
		//
		//	S T Y L E
		//
		// ********************************************



		function style_menu() {

			if ($_POST['panda_st_action'] == 'save') :


				if (!$_POST['tab']) : $this->style["tab"] = 'general'; else : $this->style["tab"] = $_POST['tab']; endif;

				// Basic style
				$this->style["style"] = $_POST['style'];

				// Layout
				$array = array (
					"background_color",
					"pattern",			// Patterns
					"pattern_current",	// Pattern ID (for pattern viewer)
					"background_image",	// Custom pattern
					"background_image_position",
					"background_repeat",
					"background_attachment"
				);
				foreach ( $array as $a ) :
					$this->style[$a] = $_POST[$a];
				endforeach;

				// Menu
				$array = array (
					"menu_color",
					"menu_bg_color",
					"menu_hover_color",
					"menu_hover_bg_color",
					"menu_selected_color",
					"menu_selected_bg_color",
					"separator"

				);
				foreach ( $array as $a ) :
					$this->style[$a] = $_POST[$a];
				endforeach;

				// Footer
				$array = array (
					"footer_bg",
					"footer_titles_color",
					"footer_text_color",
					"footer_links_color"

				);
				foreach ( $array as $a ) :
					$this->style[$a] = $_POST[$a];
				endforeach;

				// Custom CSS
				$this->style["custom_css"] = stripslashes($_POST['custom_css']);


				// Update settings
				update_option('panda_st', $this->style);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';

	
			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_style.php');

		}



		// ********************************************
		//
		//	T Y P O G R A P H Y
		//
		// ********************************************



		function typography_menu() {

			if ($_POST['panda_ty_action'] == 'save') :


				if (!$_POST['tab']) : $this->typography["tab"] = 'general'; else : $this->typography["tab"] = $_POST['tab']; endif;

				// Titles
				$this->typography["titles_font_group"] = stripslashes($_POST['titles_font_group']);
				$this->typography["titles_font"] = $_POST['titles_font'];

				if (!$_POST['cufon_panda_current']) : $this->typography["cufon_panda_current"] = 0; else : $this->typography["cufon_panda_current"] = $_POST['cufon_panda_current']; endif;
				if (!$_POST['cufon_panda']) : $this->typography["cufon_panda"] = '- select -'; else : $this->typography["cufon_panda"] = $_POST['cufon_panda']; endif;
				$this->typography["cufon_custom"] = $_POST['cufon_custom'];
				$this->typography["cufon_classes"] = $_POST['cufon_classes'];

				if (!$_POST['google_panda_current']) : $this->typography["google_panda_current"] = 0; else : $this->typography["google_panda_current"] = $_POST['google_panda_current']; endif;
				if (!$_POST['google_panda']) : $this->typography["google_panda"] = '- select -'; else : $this->typography["google_panda"] = $_POST['google_panda']; endif;

				$array = array (
					"google_custom",
					"google_panda_family",
					"google_panda_family_custom",
					"google_classes",
					"h1_color",
					"h2_color",
					"h3_color",
					"h4_color",
					"h5_color",
					"h6_color",
					"h1_size",
					"h2_size",
					"h3_size",
					"h4_size",
					"h5_size",
					"h6_size"
				);
				foreach ( $array as $a ) :
					$this->typography[$a] = $_POST[$a];
				endforeach;

				// Content
				$this->typography["font"] = $_POST['font'];
				$this->typography["font_size"] = $_POST['font_size'];
				$this->typography["links_color"] = $_POST['links_color'];
				$this->typography["links_hover_color"] = $_POST['links_hover_color'];


				// Update settings
				update_option('panda_ty', $this->typography);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';

	
			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_typography.php');

		}



		// ********************************************
		//
		//	B U D D Y  P R E S S
		//
		// ********************************************



		function buddypress_menu() {

			if ($_POST['panda_bp_action'] == 'save') :
	

				if (!$_POST['tab']) : $this->buddypress["tab"] = 'general'; else : $this->buddypress["tab"] = $_POST['tab']; endif;

				// Compatibility
				$this->buddypress["compatibility"] = $_POST['compatibility'];

				// Admin bar
				$this->buddypress["admin_bar"] = $_POST['admin_bar'];


				// Update settings
				update_option('panda_bp', $this->buddypress);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';


			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_buddypress.php');

		}



		// ********************************************
		//
		//	M I S C
		//
		// ********************************************



		function misc_menu() {

			if ($_POST['panda_mi_action'] == 'save') :
	

				if (!$_POST['tab']) : $this->misc["tab"] = 'general'; else : $this->misc["tab"] = $_POST['tab']; endif;

					// Admin bar
					$this->misc["admin_bar"] = $_POST['admin_bar'];

					// Buy themes
					$this->misc["buy_themes"] = $_POST['buy_themes'];

					// Autoformatting
					$this->misc["formatting"] = $_POST['formatting'];

					// Chmod
					$this->misc["chmod"] = $_POST['chmod'];


				// Update settings
				update_option('panda_mi', $this->misc);

				echo '<div class="updated fade" id="message" style="background-color: rgb(255, 251, 204);"><p>Your settings have been saved.</p></div>';


			endif;
	
			include(TEMPLATEPATH.'/admin/panel/admin_form_misc.php');

		}



	} // end ControlPanel



?>