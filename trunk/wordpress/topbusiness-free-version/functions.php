<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

// by PandaThemes.com

	// IF YOU NEED TO ADD YOUR CUSTOM FUNCTIONS & HOOKS
	// YOU HAVE A BETTER WAY THAN EDIT THIS FILE.
	// JUST CREATE A topbusiness-custom.php FILE AT ../wp-content/themes/ DIRECTORY.
	// THIS IS AN ALTERNATIVE FOR A CHILD THEMES

$user = wp_get_current_user();			// $global for a current user

$theme_options = get_option('ikarina');	// $global for general
$panda_he = get_option('panda_he'); 	// $global for header
$panda_sl = get_option('panda_sl'); 	// $global for slider
$panda_pr = get_option('panda_pr'); 	// $global for products
$panda_st = get_option('panda_st'); 	// $global for style
$panda_ty = get_option('panda_ty'); 	// $global for typography
$panda_sb = get_option('panda_sb'); 	// $global for sidebars
$panda_bp = get_option('panda_bp'); 	// $global for buddypress
$panda_mi = get_option('panda_mi'); 	// $global for misc

require_once(TEMPLATEPATH.'/admin/panel/admin_panel.php');

$pandatheme_panel = new ControlPanel();

// DATA
$template_url = get_bloginfo('template_url');



// ********************************************
//
// 	C O M M O N
//
// ********************************************
	add_editor_style();


	// REGISTER EXTERNAL SCRIPTS
	if (!is_admin()) :

		// jQuery
		wp_deregister_script('jquery');
		wp_register_script('jquery', 'http://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.min.js', false, null); 				wp_enqueue_script('jquery', false, null);

		// another
		wp_register_script('k1', get_bloginfo('template_directory').'/js/jquery.menu.js', false, null);								wp_enqueue_script('k1', false, null);
		wp_register_script('k2', get_bloginfo('template_directory').'/js/jquery.panda.js', false, null);							wp_enqueue_script('k2', false, null);
		wp_register_script('k3', get_bloginfo('template_directory').'/js/jquery.animate-colors-min.js', false, null);				wp_enqueue_script('k3', false, null);
		wp_register_script('k4', get_bloginfo('template_directory').'/js/jquery.prettyPhoto.js', false, null, true, true);			wp_enqueue_script('k4', false, null, true, true);
		
		// Alternate row colors
		wp_register_script('alternaterow', get_bloginfo('template_directory').'/js/jquery.tableAlternateRowColor.js', false, null);
		wp_enqueue_script('alternaterow', false, null);
		

		// Cufon
		if (
				$panda_ty['titles_font_group'] == 'cufon' && $panda_ty['cufon_panda'] != '- select -' ||
				$panda_ty['titles_font_group'] == 'cufon' && $panda_ty['cufon_custom']
			) :

				// Register Cufon base
				wp_register_script('kcufon', get_bloginfo('template_directory').'/js/cufon-yui.js', false, null, true, true); 		wp_enqueue_script('kcufon', false, null, true, true);

				// Register Cufon font
				if ( $panda_ty['cufon_custom'] ) :

					wp_register_script('kcufon-font', $panda_ty['cufon_custom'], false, null, true, true); wp_enqueue_script('kcufon-font', false, null, true, true);

				else :

					wp_register_script('kcufon-font', get_bloginfo('template_directory').'/fonts/cufon/'.$panda_ty['cufon_panda'], false, null, true, true); wp_enqueue_script('kcufon-font', false, null, true, true);

				endif;

		endif;

	endif;

	// LOCALIZATION
	load_theme_textdomain( 'pandathemes', TEMPLATEPATH.'/lang' );

	// BREADCRUMBS
	require_once (TEMPLATEPATH.'/inc/breadcrumb-trail/breadcrumb-trail.php');

	// SIDEBARS REGISTRATION
	require_once (TEMPLATEPATH.'/admin/includes/sidebars.php');

	// CUSTOM WRITE PANEL
	require_once (TEMPLATEPATH.'/admin/includes/write_panel.php');

	// COMMENTS CALLBACK
	require_once (TEMPLATEPATH.'/inc/comments_callback.php');

	// PAGINATION
	require_once (TEMPLATEPATH.'/admin/includes/wp-pagenavi.php');



// ********************************************
//
// 	M E N U
//
// ********************************************



	// Menu registration
	function register_my_menus() {

		register_nav_menus(

			array(

				'primary-menu' => 'Menu'

			)

		);

	}

	add_action( 'init', 'register_my_menus' );

	// Menu walkers
	require_once (TEMPLATEPATH.'/admin/includes/kclass_megamenu_walker.php');
	require_once (TEMPLATEPATH.'/admin/includes/walker_nav_menu_dropdown.php');

	// Menu fallback
	function karina_fallback_menu() {

		$current = '';

		if (is_front_page()) : $current = ' class="current-menu-ancestor"'; endif;

		echo '<div class="menubox"><ul class="top-menu" id="menu-by-default">';

			echo '<li'.$current.'><a href="'.get_settings('home').'">'.__('Home','pandathemes').'</a></li>';

			wp_list_pages('title_li=&sort_column=menu_order');

		echo '</ul></div>';

	}

	// Drop-down menu fallback
	function dropdown_fallback_menu() {

		echo '<form action="' . get_bloginfo('url') . '" method="get">';

			wp_dropdown_pages();

		echo '</form>';

	}



// ********************************************
//
// 	W I D G E T S
//
// ********************************************



	$widgets = array (
		"flickr.php",
		"twitter.php",
		"recent_posts.php",
		"ads300.php",
		"ads125.php",
		"subscribe.php",
		"tabs.php",
		"contact_info.php",
		"demo_styles.php"
	);

	foreach ( $widgets as $w ) :

		require_once (TEMPLATEPATH.'/admin/includes/widgets/'.$w);
	
	endforeach;



// ********************************************
//
// 	F O R M A T T I N G
//
// ********************************************


	// RAW shortcode
	if ($panda_mi['formatting'] == 'raw') :

		function my_formatter($content) {
	
			$new_content = '';
	
			$pattern_full = '{(\[raw\].*?\[/raw\])}is';
	
			$pattern_contents = '{\[raw\](.*?)\[/raw\]}is';
	
			$pieces = preg_split($pattern_full, $content, -1, PREG_SPLIT_DELIM_CAPTURE);
	
			foreach ($pieces as $piece) {
	
				if (preg_match($pattern_contents, $piece, $matches)) :
	
					$new_content .= $matches[1];
	
				else :
	
					$new_content .= wptexturize(wpautop($piece));
	
				endif;
	
			};
	
		return $new_content; }
	
		remove_filter('the_content', 'wpautop');
		remove_filter('the_content', 'wptexturize');
		add_filter('the_content', 'my_formatter', 1);

	// Disabled at all
	elseif ($panda_mi['formatting'] == 'disabled') :

		remove_filter('the_content', 'wpautop');
		remove_filter('the_content', 'wptexturize');

	endif;



// ********************************************
//
// 	C U S T O M   T Y P E   P O S T S
//
// ********************************************



	// SLIDERS
	require_once (TEMPLATEPATH.'/admin/includes/custom_post_type/sliders.php');

	// PRODUCTS & RATING
	if ($panda_pr['products'] == 'enable') :
		
		require_once (TEMPLATEPATH.'/admin/includes/custom_post_type/products.php');
		require_once (TEMPLATEPATH.'/admin/includes/shortcodes/products.php');
		require_once (TEMPLATEPATH.'/inc/comments_rating.php');

	endif;
	
	// Disable autosaving of posts - DO NOT REMOVE THIS FILTER
	function disableAutoSave(){
		wp_deregister_script('autosave');
	}
	add_action( 'wp_print_scripts', 'disableAutoSave' );



// ********************************************
//
// 	S H O R T C O D E S
//
// ********************************************



	$codes = array (
		// An order of these files is a very important, because a visual shortcode editor.
		"buttons.php",
		"columns.php",
		"icons.php",
		"lists.php",
		"posts2.php",
		"posts.php",
		"shortcodes.php",
		"slider.php"
	);

	foreach ( $codes as $c ) :

		require_once (TEMPLATEPATH.'/admin/includes/shortcodes/'.$c);
	
	endforeach;



// ********************************************
//
// 	T I N Y M C E
//
// ********************************************



	if ( preg_match( "/(post-new|post)\.php/", basename( getenv('SCRIPT_FILENAME') ) ) ) {
		add_action('init', 'gse_init');
		add_action('admin_print_scripts', 'gse_admin_js');
	}
	
	function gse_init() {
		if ( get_user_option('rich_editing') == 'true' && current_user_can('edit_posts') ) {
			add_filter("mce_external_plugins", "gse_mce_plugin", 80);
			add_filter('mce_buttons', 'gse_mce_button', 80);
	   }
	}
	
	function gse_mce_button($buttons) {
		array_push($buttons, 'separator', 'shortcode_editor');
		return $buttons;
	}
	
	function gse_mce_plugin($plugin_array) {
		$plugin_array['shortcode_editor'] = get_bloginfo('template_url').'/admin/includes/tinymce/plugins/shortcode-editor/shortcode_editor.js';
		return $plugin_array;
	}
	
	function gse_admin_js($buttons) {
		global $shortcode_tags;
		?>
			<script type="text/javascript">
				<?php

					$codes = array (
						"line",
						"button",
						"bbutton",
						"toggle",
						"fieldset",
						"pbox",
						"note",
						"marker",
						"quote",
						"dropcap",
						"sidebar",
						"divider",
						"icon60",
						"icon",
						"email",
						"posts2",
						"posts",
						"products",
						"img",
						"hidden",
						"visible",
						"pages",
						"categories",
						"archives",
						"tweet",
						"like",
						"clear"
					);

					$out = '';
					$count = 0;

					foreach ( $codes as $c ) :

						if ($count == 0) :

							$out .=	$c;

						else :

							$out .=	'|' . $c;

						endif;

						$count++;

					endforeach;

				?>

				var gse_shortcode_er = /\[(<?php echo $out; // print join('|', array_keys($shortcode_tags)); ?>)\s?([^\]]*)(?:\s*\/)?\](([^\[\]]+)\[\/\1\])?/g;
					addLoadEvent(function() {
						jQuery('form#post').submit(function() {
							var c = this.content;
								c.value = c.value.replace(/(\[[^\]]+\S)(\s+sc_id="sc\d+")([^\]]*\])/g, '$1$3');
						});
					});
			</script>
		<?php
	}



// ********************************************
//
// 	H T M L   E D I T O R   B U T T O N S
//
// ********************************************



	function a_few_different_quicktags_buttons() {
		require_once (TEMPLATEPATH.'/admin/includes/tinymce/plugins/html-mode/buttons.php');
	}
	add_action( 'admin_print_footer_scripts', 'a_few_different_quicktags_buttons', 100 );



// ********************************************
//
// 	S T Y L E S
//
// ********************************************



	if (!is_admin()) :


		// Another styles
		function styles() {
			global $is_opera;
			$template_url = get_bloginfo('template_url');

				// Opera
				if ($is_opera) :

					wp_register_style('opera', get_bloginfo('template_directory').'/styles/opera.css', false, null); wp_enqueue_style('opera', false, null);

				endif;

		}
		add_filter('styles','styles');


		// BuddyPress
		if ( $panda_bp['compatibility'] == 'yes' )
			wp_register_style('bp-style', $template_url.'/styles/buddy/bp.css', false, null); wp_enqueue_style('bp-style', false, null);


		// Selected style
		$style = $panda_st['style'];
		
		// check to see if cookie is set.
		if (isset($_COOKIE['bfi_selected_style'])) :
			$bfi_cookie = $_COOKIE['bfi_selected_style']; 
			//var_dump($bfi_cookie);
			if ($bfi_cookie != 's1') :
				wp_register_style('predetermined', $template_url.'/styles/predetermined/'.$bfi_cookie.'.css', false, null); 
				wp_enqueue_style('predetermined', false, null);
			
			//else :
				//wp_register_style('predetermined', $template_url.'/styles/predetermined/s4.css', false, null); wp_enqueue_style('predetermined', //false, null);
			endif;
			
		else :
			if ($style && $style != '1') : wp_register_style('predetermined', $template_url.'/styles/predetermined/s'.$style.'.css', false, null); wp_enqueue_style('predetermined', false, null); endif;
		endif;

		// Responsive styles
		wp_register_style('responsive-style', $template_url.'/styles/responsive.css', false, null); wp_enqueue_style('responsive-style', false, null);


		// Custom styles
		wp_register_style('custom-style', $template_url.'/styles/custom_style.css', false, null); wp_enqueue_style('custom-style', false, null);


	endif;



// ********************************************
//
//	B U D D Y P R E S S
//
// ********************************************



	if ( $panda_bp['compatibility'] == 'yes' ) :

		include_once( ABSPATH . 'wp-admin/includes/plugin.php' );

		if ( is_plugin_active('buddypress/bp-loader.php') ) :

			// Buddy functions
			include( TEMPLATEPATH.'/admin/includes/buddy/bpt-functions.php' );

			if ( $panda_bp['admin_bar'] != 'yes' ) :

				define( 'BP_DISABLE_ADMIN_BAR', true );

			endif;

		endif;

	endif;



// ********************************************
//
// 	I N T E R N E T   E X P L O R E R
//
// ********************************************



	function ie() {
		global $is_IE;
		$template_url = get_bloginfo('template_url');
	
			// IE
			if ($is_IE) :
	
				echo '
					<!--[if lt IE 7]><link href="'.$template_url.'/styles/ie6.css" rel="stylesheet" type="text/css"><![endif]-->
					<!--[if IE 7]><link href="'.$template_url.'/styles/ie7.css" rel="stylesheet" type="text/css"><![endif]-->
					<!--[if IE 8]><link href="'.$template_url.'/styles/ie8.css" rel="stylesheet" type="text/css"><![endif]-->
					<!--[if IE 9]><link href="'.$template_url.'/styles/ie9.css" rel="stylesheet" type="text/css"><![endif]-->

					<div id="ie6" style="display:none;">
						<h3>'.__('Sorry, you can not browse this website.','pandathemes').'</h3>
						<p>'.__('Because you are using an outdated version of MS Internet Explorer. For a better experience using websites, please upgrade to a modern web browser.','pandathemes').'</p>
						<a href="http://firefox.com" title="Mozilla Firefox"><img src="'.$template_url.'/images/logo_ff.gif" alt="Mozilla Firefox"/></a>
						<a href="http://www.browserforthebetter.com/download.html" title="Microsoft Internet Explorer"><img src="'.$template_url.'/images/logo_ie.gif" alt="Microsoft Internet Explorer"/></a>
						<a href="http://www.apple.com/safari/download/" title="Apple Safari"><img src="'.$template_url.'/images/logo_sf.gif" alt="Apple Safari"/></a>
						<a href="http://www.google.com/chrome" title="Google Chrome"><img src="'.$template_url.'/images/logo_ch.gif" alt="Google Chrome"/></a>
					</div>
				';

			endif;

	}
	add_filter('ie','ie');



// ********************************************
//
// 	C U F O N
//
// ********************************************



	function cufon() {
		global $panda_ty;

			if ( $panda_ty['titles_font_group'] == 'cufon' && $panda_ty['cufon_panda'] != '- select -' ) :

				$cufon_classes = ( $panda_ty['cufon_classes'] ) ? ', '.$panda_ty['cufon_classes'] : '';
	
				echo '
					<script type="text/javascript">Cufon.replace(\'h1, h2, h3, h4, h5, h6, .pbox .pa, .font-replace, .dropcap, .dropcap2'.$cufon_classes.'\', { hover: true }, { fontFamily: \'Cufon Font\' });</script>
					<script type="text/javascript">Cufon.now();</script>
				';

			endif;

	}
	add_filter('cufon','cufon');



// ********************************************
//
// 	G O O G L E   W E B   F O N T S
//
// ********************************************



	function google_fonts() {
		global $panda_ty;

			if ( $panda_ty['titles_font_group'] == 'google' ) :

				// Custom font
				if ( $panda_ty['google_custom'] ) :

					echo stripslashes($panda_ty['google_custom']);

				// From library
				elseif ( $panda_ty['google_panda'] != '- select -' ) :

					echo "<link href='http://fonts.googleapis.com/css?family=".$panda_ty['google_panda']."' rel='stylesheet' type='text/css'>";

				endif;

			endif;

	}
	add_filter('google_fonts','google_fonts');



// ********************************************
//
// 	R E - O R D E R   M E T A B O X E S
//
// ********************************************



	function set_user_metaboxes($user_id=NULL) {
	
		// These are the metakeys we will need to update
		$meta_key['order'] = 'meta-box-order_post';
		$meta_key['hidden'] = 'metaboxhidden_post';
	
		// So this can be used without hooking into user_register
		if ( ! $user_id) : $user_id = get_current_user_id(); endif;
	
		// Set the default order if it has not been set yet
		if ( ! get_user_meta( $user_id, $meta_key['order'], false) ) :
	
			$meta_value = array(
				'side'		=> 'submitdiv,formatdiv,postimagediv,categorydiv,pageparentdiv',
				'normal'	=> 'postexcerpt,tagsdiv-post_tag,postcustom,commentstatusdiv,commentsdiv,trackbacksdiv,slugdiv,authordiv,revisionsdiv',
				'advanced'	=> '',
			);
	
			update_user_meta( $user_id, $meta_key['order'], $meta_value );
	
		endif;
	
		// Set the default hiddens if it has not been set yet
		if ( ! get_user_meta( $user_id, $meta_key['hidden'], true) ) :
	
			$meta_value = array('postcustom','trackbacksdiv','commentstatusdiv','commentsdiv','slugdiv','authordiv','revisionsdiv');
			update_user_meta( $user_id, $meta_key['hidden'], $meta_value );
	
		endif;
	
	}
	
	add_action('admin_init', 'set_user_metaboxes');



// ********************************************
//
// 	H I D E   A   P A R T   O F   M E T A B O X E S
//	F R O M   L O W   L E V E L   U S E R S
//
// ********************************************



	function hide_some_metaboxes() {

		global $user;

		if ( ! $user->has_cap('edit_pages') ) :

			remove_meta_box( 'slugdiv', 'post', 'normal' );
			remove_meta_box( 'postcustom', 'post', 'normal' );
			remove_meta_box( 'revisionsdiv', 'post', 'normal' );
			remove_meta_box( 'trackbacksdiv', 'post', 'normal' );
			remove_meta_box( 'authordiv', 'post', 'normal' );
			remove_meta_box( 'formatdiv', 'post', 'normal' );
			remove_meta_box( 'postimagediv', 'post', 'normal' );
			remove_meta_box( 'commentstatusdiv', 'post', 'normal' );

		endif;

	}
	
	add_action('admin_head', 'hide_some_metaboxes');



// ********************************************
//
// 	M I S C E L L A N E O U S
//
// ********************************************



	// Include excerpt into the search results
	function my_search_where( $where ) { 
	
		if ( is_search() ) { 
			$where = preg_replace( 
			"/post_title\s+LIKE\s*(\'[^\']+\')/", 
			"post_title LIKE $1) OR (post_excerpt LIKE $1", $where ); 
		} 

		return $where; }
	add_filter( 'posts_where', 'my_search_where' ); 


	// Custom gravatar
    function newgravatar ($avatar_defaults) {
	$theme_options = get_option('ikarina');
		$myavatar = $theme_options['gravatar'];
		$avatar_defaults[$myavatar] = __('Custom gravatar','pandathemes');
	    return $avatar_defaults; }
    add_filter( 'avatar_defaults', 'newgravatar' );


	// Set default thumbs
	if ( function_exists( 'add_theme_support' ) ) {
		add_theme_support( 'post-thumbnails' );
		set_post_thumbnail_size( 150, 150, true );
	}


	// Prevent stripe tags at descriptions
	$filters =
		array(
			'term_description',
			'category_description',
			'description',
			'pre_term_description',
			'pre_link_description',
			'pre_link_notes',
			'pre_user_description'
		);

	foreach ( $filters as $filter ) :
		remove_filter($filter, 'strip_tags');
		remove_filter($filter, 'wp_filter_kses');
	endforeach;

	foreach ( array('term_description') as $filter ) :
		remove_filter($filter, 'wp_kses_data');
	endforeach;


	// Shortcodes don't wrap by <p>..</p> tags
	add_filter('the_content', 'shortcode_unautop');


	// Do shortcodes at sidebars
	add_filter('widget_text', 'shortcode_unautop');
	add_filter('widget_text', 'do_shortcode');


	// Do shortcodes at Category, Tag, and Taxonomy Descriptions
	add_filter( 'term_description', 'shortcode_unautop');
	add_filter( 'term_description', 'do_shortcode' );


	// Custom lenght of excerpt
	function new_excerpt_length($length) { return 25; }
	add_filter('excerpt_length', 'new_excerpt_length');


	// Replace the [...] in excerpt
	function ik_excerpt_more($excerpt) {
		return str_replace('[...]', '...', $excerpt); }
	add_filter('wp_trim_excerpt', 'ik_excerpt_more');


	// Time delay for rss feed
	function publish_later_on_feed($where) {
		global $wpdb;
		if (is_feed()) :
			$now = gmdate('Y-m-d H:i:s');
			$wait = '10';
			$device = 'MINUTE';
			$where .= " AND TIMESTAMPDIFF($device, $wpdb->posts.post_date_gmt, '$now') > $wait ";
		endif;
		return $where;
	}
	add_filter('posts_where', 'publish_later_on_feed');


	// Automatic rss feeds
	add_theme_support('automatic-feed-links');


	// Activate wordpress thumbs
	if (function_exists('add_theme_support')) : add_theme_support('post-thumbnails'); set_post_thumbnail_size(300,9999);	endif;
	if (function_exists('add_image_size')) : add_image_size('thumb-150',150,9999); endif;
	add_theme_support('post-thumbnails');


	// Tag widget fix
	function tag_cloud($args = array()) {
		$args['smallest'] = 11;
		$args['largest'] = 11;
		$args['unit'] = 'px';
		$args['separator'] = '';
		$args['orderby'] = 'count';
		$args['order'] = 'DESC';
		return $args;
	}
	add_action('widget_tag_cloud_args', 'tag_cloud');


	// Set a wrapper for a default clod of tags
	function tag_wrapper($tags) { $cloud = '<div class="tag-cloud">'.$tags.'</div>'; return $cloud; }
	add_action('wp_tag_cloud', 'tag_wrapper');


	// Remove admin bar from the front-end
	if ($panda_mi['admin_bar'] == 'no') :
		function my_function_admin_bar() { return false; }
		add_filter( 'show_admin_bar' , 'my_function_admin_bar');
	endif;


	// Another themes
	if ($panda_mi['buy_themes'] != 'no') :
		function panda_themes_setup() { add_menu_page("Buy themes", "Buy themes", 'edit_themes', 'pandathemes', 'panda_themes_page', get_bloginfo('template_url').'/favicon.ico'); }
		function panda_themes_page(){
		?>
			<style type="text/css" media="all">#screen-meta, #footer, #wphead {display:none;} #wpbody-content {padding-bottom:0;} .wrap {margin:0;}</style>
			<script type="text/javascript">
			var ik = jQuery.noConflict();
				ik(document).ready(function() {
					var theFrame = ik("#panda-preview", parent.document.body);
					theFrame.height(ik(document.body).height());
				});
			</script>
			<iframe name="pandapreview" id="panda-preview" scrolling="auto" frameborder="0" style="width:100%; height:500px;" allowtransparency="true" src="http://pandathemes.com/wordpress-themes/"></iframe>
		<?php } 
			add_action('admin_menu', 'panda_themes_setup');
	endif;

	function pt_get_full_theme() {
		return '
			<fieldset class="panda-admin-fieldset fieldset-feat"><legend>' . __("Full version","pandathemes") . '</legend>
				<p>Install full version and you will get all possible <a target="_blank" href="http://topbusiness.pandathemes.com/about/">features</a> of this theme. <br>Also, you will get a lifetime access to <a target="_blank" href="http://support.pandathemes.com/">support forum</a> and all future updates for free.</p>
				<div class="clear h10"><!-- --></div>
				<a href="http://pandathemes.com/purchase-topbusiness-ex.htm" class="button-primary" target="_blank"> Extended License </a> &nbsp; &nbsp; 
				<a href="http://pandathemes.com/purchase-topbusiness.htm" class="button" target="_blank"> Regular License </a>
				<div class="clear h20"><!-- --></div>
			</fieldset>
			<div class="clear h10"><!-- --></div>';
	};



// ********************************************
//
// 	UPDATE NOTIFICATION
//
// ********************************************



	if ( is_admin() )
		include(TEMPLATEPATH.'/inc/update_notifier.php');



// ********************************************
//
// 	C U S T O M   S C R I P T S
//
// ********************************************



	if ( file_exists( WP_CONTENT_DIR . '/themes/topbusiness-custom.php' ) ) :

		require( WP_CONTENT_DIR . '/themes/topbusiness-custom.php' );

	endif;

?>
