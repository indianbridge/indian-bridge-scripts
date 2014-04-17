<?php
/**
 * The Header for our theme.
 *
 * Displays all of the <head> section and everything up till <div id="content">
 *
 * @package bfi_bootstrap
 */
?><!DOCTYPE html>
<html <?php language_attributes(); ?>>
<head>
<meta charset="<?php bloginfo( 'charset' ); ?>">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title><?php wp_title( '|', true, 'right' ); ?></title>
<link rel="profile" href="http://gmpg.org/xfn/11">
<link rel="pingback" href="<?php bloginfo( 'pingback_url' ); ?>">
	<!--[if lt IE 9]>
	<script src="//cdnjs.cloudflare.com/ajax/libs/html5shiv/r29/html5.min.js"></script>
	<![endif]-->
<?php wp_head(); ?>
</head>

<body <?php body_class(); ?>>
<div id="page" class="hfeed site">

	<header id="header" class="site-header" role="banner">	
		<nav id="header-navbar" class="navbar navbar-default navbar-fixed-top" role="navigation">
			
		   <div class="navbar-header">
		      <button type="button" class="navbar-toggle" data-toggle="collapse" 
		         data-target="#example-navbar-collapse">
		         <span class="sr-only">Toggle navigation</span>
		         <span class="icon-bar"></span>
		         <span class="icon-bar"></span>
		         <span class="icon-bar"></span>
		      </button>
		      <a href="<?php echo esc_url( home_url( '/' ) ); ?>"><img align="middle" class="pull-left" height="50" src=" <?php echo THEME_DIR_URI . '/bfi_logo.png'; ?> "></img></a>
		      <a class="navbar-brand" href="<?php echo esc_url( home_url( '/' ) ); ?>">Bridge Federation of India</a>
		   </div>
		   <div class="navbar-responsive-collapse navbar-collapse" id="example-navbar-collapse">
				<?php
				wp_nav_menu( array(
					'menu'           => 'primary',
					'theme_location' => 'primary',
					'depth'          => 0,
					'container'      => 'div',
					'container_class'=> 'collapse navbar-collapse navbar-ex1-collapse',
					'menu_class'     => 'nav navbar-nav pull-right',
					'fallback_cb'=> 'wp_list_pages_bootstrap_navwalker::fallback',
					'walker'         => new wp_bootstrap_navwalker('NO_CARET'),)
				);
				?>
				<ul class="nav navbar-nav">
		         <li class="dropdown">
		            <a href="#" class="dropdown-toggle" data-toggle="dropdown">
		               Logged In as Sriram
		            </a>
		            <ul class="dropdown-menu">
		            <h1>Testing</h1>
		            <a href="www.google.com">Google</a>
		            </ul>
		         </li>
		      </ul>
		   </div>
		</nav>	

	</header><!-- #masthead -->
