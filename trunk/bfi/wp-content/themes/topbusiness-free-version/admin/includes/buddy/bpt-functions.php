<?php if ( !defined( 'ABSPATH' ) ) exit;



// ********************************************
//
//	B U D D Y P R E S S   F U N C T I O N S
//
// ********************************************



	// Check to make sure the active theme is not bp-default
	if ( 'bp-default' == get_option( 'template' ) ) :
		return;
	endif;



	// Sets up WordPress theme for BuddyPress support / since 1.2
	function bp_tpack_theme_setup() {

		global $bp;
	
		// Load the default BuddyPress AJAX functions if it isn't explicitly disabled
		if ( !(int)get_option( 'bp_tpack_disable_js' ) ) :
			require_once( BP_PLUGIN_DIR . '/bp-themes/bp-default/_inc/ajax.php' );
		endif;
	
		if ( !is_admin() ) :

			// Register buttons for the relevant component templates

			// Friends button
			if ( bp_is_active( 'friends' ) ) :
				add_action( 'bp_member_header_actions',    'bp_add_friend_button' );
			endif;
	
			// Activity button
			if ( bp_is_active( 'activity' ) ) :
				add_action( 'bp_member_header_actions',    'bp_send_public_message_button' );
			endif;
	
			// Messages button
			if ( bp_is_active( 'messages' ) ) :
				add_action( 'bp_member_header_actions',    'bp_send_private_message_button' );
			endif;
	
			// Group buttons
			if ( bp_is_active( 'groups' ) ) :
				add_action( 'bp_group_header_actions',     'bp_group_join_button' );
				add_action( 'bp_group_header_actions',     'bp_group_new_topic_button' );
				add_action( 'bp_directory_groups_actions', 'bp_group_join_button' );
			endif;
	
			// Blog button
			if ( bp_is_active( 'blogs' ) ) :
				add_action( 'bp_directory_blogs_actions',  'bp_blogs_visit_blog_button' );
			endif;

		endif;

	}

	add_action( 'after_setup_theme', 'bp_tpack_theme_setup', 11 );



	// Enqueues BuddyPress JS and related AJAX functions / since 1.2
	function bp_tpack_enqueue_scripts() {

		// Do not enqueue JS if it's disabled
		if ( get_option( 'bp_tpack_disable_js' ) ) :
			return;
		endif;
	
		// Add words that we need to use in JS to the end of the page so they can be translated and still used.
		$params = array(
			'my_favs'           => __( 'My Favorites', 'pandathemes' ),
			'accepted'          => __( 'Accepted', 'pandathemes' ),
			'rejected'          => __( 'Rejected', 'pandathemes' ),
			'show_all_comments' => __( 'Show all comments for this thread', 'pandathemes' ),
			'show_all'          => __( 'Show all', 'pandathemes' ),
			'comments'          => __( 'comments', 'pandathemes' ),
			'close'             => __( 'Close', 'pandathemes' )
		);
	
		// BP 1.5+
		if ( version_compare( BP_VERSION, '1.3', '>' ) ) :
			// Bump this when changes are made to bust cache
			$version            = '20110818';
			$params['view']     = __( 'View', 'pandathemes' );
		endif;
	
		// Enqueue the global JS - Ajax will not work without it
		wp_enqueue_script( 'dtheme-ajax-js', BP_PLUGIN_URL . '/bp-themes/bp-default/_inc/global.js', array( 'jquery' ), $version );
	
		// Localize the JS strings
		wp_localize_script( 'dtheme-ajax-js', 'BP_DTheme', $params );
	}

	add_action( 'wp_enqueue_scripts', 'bp_tpack_enqueue_scripts' );



	// Enqueues BuddyPress basic styles / since 1.2
	if ( !function_exists( 'bp_tpack_use_wplogin' ) ) :

		function bp_tpack_use_wplogin() {
			// returning 2 will automatically use wp-login
			return 2;
		}

		add_filter( 'bp_no_access_mode', 'bp_tpack_use_wplogin' );

	endif;



	// Hooks into the 'bp_get_activity_action_pre_meta' action to add secondary activity avatar support / since 1.2
	function bp_tpack_activity_secondary_avatars( $action, $activity ) {
		// sanity check - some older versions of BP do not utilize secondary activity avatars
		if ( function_exists( 'bp_get_activity_secondary_avatar' ) ) :

			switch ( $activity->component ) {
				case 'groups' :
				case 'friends' :
					// Only insert avatar if one exists
					if ( $secondary_avatar = bp_get_activity_secondary_avatar() ) {
						$reverse_content = strrev( $action );
						$position        = strpos( $reverse_content, 'a<' );
						$action          = substr_replace( $action, $secondary_avatar, -$position - 2, 0 );
					}
					break;
			}

		endif;
	
		return $action;

	}

	add_filter( 'bp_get_activity_action_pre_meta', 'bp_tpack_activity_secondary_avatars', 10, 2 );



	// Custom size of buddy avatars
	define('BP_AVATAR_THUMB_HEIGHT', 50);
	define('BP_AVATAR_THUMB_WIDTH', 50);
	define('BP_AVATAR_FULL_HEIGHT', 150);
	define('BP_AVATAR_FULL_WIDTH', 150);



?>