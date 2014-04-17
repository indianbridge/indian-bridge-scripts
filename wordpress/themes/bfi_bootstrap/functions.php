<?php
/**
 * bfi_bootstrap functions and definitions
 *
 * @package bfi_bootstrap
 */

/**
 * Set the content width based on the theme's design and stylesheet.
 */
if ( ! isset( $content_width ) ) {
	$content_width = 640; /* pixels */
}

// The option name used by Redux Framework
if ( ! defined( 'REDUX_OPT_NAME' ) ) {
	define( 'REDUX_OPT_NAME', 'bfi_bootstrap' );
}

// The name of this theme
if ( ! defined( 'THEME_NAME' ) ) {
	define( 'THEME_NAME', 'bfi_bootstrap' );
}

// Absolute path to theme directory
if ( ! defined( 'THEME_DIR' ) ) {
	define( 'THEME_DIR', get_template_directory() );
}

// Absolute path to theme directory URI
if ( ! defined( 'THEME_DIR_URI' ) ) {
	define( 'THEME_DIR_URI', get_template_directory_uri() );
}

if ( ! function_exists( 'bfi_bootstrap_setup' ) ) :
/**
 * Sets up theme defaults and registers support for various WordPress features.
 *
 * Note that this function is hooked into the after_setup_theme hook, which
 * runs before the init hook. The init hook is too late for some features, such
 * as indicating support for post thumbnails.
 */
function bfi_bootstrap_setup() {

	/*
	 * Make theme available for translation.
	 * Translations can be filed in the /languages/ directory.
	 * If you're building a theme based on bfi_bootstrap, use a find and replace
	 * to change 'bfi_bootstrap' to the name of your theme in all the template files
	 */
	load_theme_textdomain( 'bfi_bootstrap', get_template_directory() . '/languages' );

	// Add default posts and comments RSS feed links to head.
	add_theme_support( 'automatic-feed-links' );

	/*
	 * Enable support for Post Thumbnails on posts and pages.
	 *
	 * @link http://codex.wordpress.org/Function_Reference/add_theme_support#Post_Thumbnails
	 */
	add_theme_support( 'post-thumbnails' );
	// set the default thumbnail size
	set_post_thumbnail_size( 150, 150 );	

	// This theme uses wp_nav_menu() in one location.
	register_nav_menus( array(
		'primary' => __( 'Primary Menu', 'bfi_bootstrap' ),
	) );

	// Enable support for Post Formats.
	//add_theme_support( 'post-formats', array( 'aside', 'image', 'video', 'quote', 'link' ) );

	// Setup the WordPress core custom background feature.
	/*add_theme_support( 'custom-background', apply_filters( 'bfi_bootstrap_custom_background_args', array(
		'default-color' => 'ffffff',
		'default-image' => '',
	) ) );*/

	// Enable support for HTML5 markup.
	add_theme_support( 'html5', array(
		'comment-list',
		'search-form',
		'comment-form',
		'gallery',
	) );
}
endif; // bfi_bootstrap_setup
add_action( 'after_setup_theme', 'bfi_bootstrap_setup' );

// Hide admin bar
add_filter('show_admin_bar', '__return_false');

/**
 * Register widget area.
 *
 * @link http://codex.wordpress.org/Function_Reference/register_sidebar
 */
function bfi_bootstrap_widgets_init() {
	register_sidebar( array(
		'name'          => __( 'Sidebar', 'bfi_bootstrap' ),
		'id'            => 'sidebar-1',
		'description'   => '',
		'before_widget' => '<aside id="%1$s" class="panel panel-primary widget %2$s">',
		'after_widget'  => '</section></aside>',
		'before_title'  => '<header class="panel-heading"><h3 class="panel-title widget-title">',
		'after_title'   => '</h1></header><section class="panel-body">',
	) );
}
add_action( 'widgets_init', 'bfi_bootstrap_widgets_init' );


/**
 * Enqueue all the necessary css and js files.
 */
require THEME_DIR . '/inc/scripts.php';

/**
 * Custom template tags for this theme.
 */
require get_template_directory() . '/inc/template-tags.php';

/**
 * Custom functions that act independently of the theme templates.
 */
require get_template_directory() . '/inc/extras.php';


/**
 * Load Jetpack compatibility file.
 */
require get_template_directory() . '/inc/jetpack.php';

/**
 * Bootrap walker for menu
 */
require THEME_DIR . '/inc/bootstrap-wp-navwalker.php';
require THEME_DIR . '/inc/bootstrap-wp-list-pages-navwalker.php';

/**
 * Custom post information retreival functions.
 */
require THEME_DIR . '/inc/post-information.php';

/** 
 * Add Bootstrap pagination support.
 */
require THEME_DIR . '/inc/pagination.php';

/** 
 * Customize comment form
 */
require THEME_DIR . '/inc/custom-comments.php';