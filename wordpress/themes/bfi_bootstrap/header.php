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
<?php
	$page_name  = 'main';
	$section_name = 'width';
	$container_width = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'width' )
?>
<div id="page" class="hfeed site <?php echo $container_width; ?>">
	
		<header id="header" class="site-header" role="banner">	
			<div class="row">
				<?php get_template_part( 'header', 'navbar' ); ?>
			</div>
			<div class="row">
				<?php get_template_part( 'header', 'jumbotron' ); ?>
			</div>
			<div class="row">
				<?php get_template_part( 'header', 'carousel' ); ?>
			</div>
		</header><!-- #masthead -->
		<p></p>
	<div class="row">
