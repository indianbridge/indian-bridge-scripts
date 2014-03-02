<?php
/**
 * _bootstrap functions and definitions
 *
 * @package _bootstrap
 */

/**
 * Set the content width based on the theme's design and stylesheet.
 */
if ( ! isset( $content_width ) ) {
	$content_width = 640; /* pixels */
}

/**
 * Start of CONSTANTS section
 * Define some constants to be used throughout
 */

// The option name used by Redux Framework
if ( ! defined( 'REDUX_OPT_NAME' ) ) {
	define( 'REDUX_OPT_NAME', '_bootstrap' );
}

// Absolute path to theme directory
if ( ! defined( 'THEME_DIR' ) ) {
	define( 'THEME_DIR', get_template_directory() );
}

// Absolute path to theme directory URI
if ( ! defined( 'THEME_DIR_URI' ) ) {
	define( 'THEME_DIR_URI', get_template_directory_uri() );
}

/**
 * End of CONSTANTS section
 */

if ( ! function_exists( '_bootstrap_setup' ) ) :
/**
 * Sets up theme defaults and registers support for various WordPress features.
 *
 * Note that this function is hooked into the after_setup_theme hook, which
 * runs before the init hook. The init hook is too late for some features, such
 * as indicating support for post thumbnails.
 */
function _bootstrap_setup() {

	/*
	 * Make theme available for translation.
	 * Translations can be filed in the /languages/ directory.
	 * If you're building a theme based on _bootstrap, use a find and replace
	 * to change '_bootstrap' to the name of your theme in all the template files
	 */
	load_theme_textdomain( '_bootstrap', THEME_DIR . '/languages' );

	// Add default posts and comments RSS feed links to head.
	add_theme_support( 'automatic-feed-links' );

	/*
	 * Enable support for Post Thumbnails on posts and pages.
	 *
	 * @link http://codex.wordpress.org/Function_Reference/add_theme_support#Post_Thumbnails
	 */
	add_theme_support( 'post-thumbnails' );

	// This theme uses wp_nav_menu() in one location.
	register_nav_menus( array(
		'primary' => __( 'Primary Menu', '_bootstrap' ),
	) );

	// Enable support for Post Formats.
	add_theme_support( 'post-formats', array( 'aside', 'image', 'video', 'quote', 'link' ) );

	// Setup the WordPress core custom background feature.
	add_theme_support( 'custom-background', apply_filters( '_bootstrap_custom_background_args', array(
		'default-color' => 'ffffff',
		'default-image' => '',
	) ) );

	// Enable support for HTML5 markup.
	add_theme_support( 'html5', array( 'comment-list', 'search-form', 'comment-form', ) );
	
	// 
	
}
endif; // _bootstrap_setup
add_action( 'after_setup_theme', '_bootstrap_setup' );

/**
* Remove text at end of excerpt. We will add custom buttons later.
* 
* @return void
*/
function new_excerpt_more( $more ) {
	return ' <a class="" href="'. get_permalink( get_the_ID() ) . '"><span class="label label-primary">' .
	__( 'Read More', '_bootstrap' ) . '</span></a>';
	/*return '<a href="<?php the_permalink(); ?>" class="btn btn-info btn-xs" role="button">' . 
	__( 'Read More', '_bootstrap') . '</a>';*/
}
add_filter('excerpt_more', 'new_excerpt_more');

/**
 * Register widgetized area and update sidebar with default widgets.
 */
function _bootstrap_widgets_init() {
	register_sidebar( array(
		'name'          => __( 'Sidebar', '_bootstrap' ),
		'id'            => 'sidebar-1',
		'before_widget' => '<aside id="%1$s" class="well widget %2$s">',
		'after_widget'  => '</aside>',
		'before_title'  => '<h1 class="widget-title">',
		'after_title'   => '</h1>',
	) );
}
add_action( 'widgets_init', '_bootstrap_widgets_init' );


/**
 * Functions to retrieve bootswatch skins and populate local folder.
 */
require THEME_DIR . '/inc/get-bootswatch-skins.php';

// This should be added as a subpage in a theme option
//_bootstrap_get_bootswatch_skins( 'http://api.bootswatch.com/3/', THEME_DIR . '/css/skins', TRUE );

/**
 * Enqueue all the necessary css and js files.
 */
require THEME_DIR . '/inc/scripts.php';

/**
 * Implement the Custom Header feature.
 */
require THEME_DIR . '/inc/custom-header.php';

/**
 * Custom template tags for this theme.
 */
require THEME_DIR . '/inc/template-tags.php';

/**
 * Custom post information retreival functions.
 */
require THEME_DIR . '/inc/post-information.php';

/**
 * Custom functions that act independently of the theme templates.
 */
require THEME_DIR . '/inc/extras.php';

/**
 * Customizer additions.
 */
require THEME_DIR . '/inc/customizer.php';

/**
 * Load Jetpack compatibility file.
 */
require THEME_DIR . '/inc/jetpack.php';

/**
 * Load all Redux Admin Options
 */
require THEME_DIR . '/inc/ReduxConfig/config.php';

/**
 * Bootrap walker for menu
 */
require THEME_DIR . '/inc/bootstrap-wp-navwalker.php';
require THEME_DIR . '/inc/bootstrap-wp-list-pages-navwalker.php';
