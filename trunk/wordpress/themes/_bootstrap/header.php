<?php
/**
* The Header for our theme.
*
* Displays all of the <head> section and everything up till <div id="content">
*
* @package _bootstrap
*/
?><!DOCTYPE html>
<html <?php language_attributes(); ?>>
<head>
	<meta charset="<?php bloginfo( 'charset' ); ?>">
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<title>
		<?php wp_title( '|', true, 'right' ); ?>
	</title>
	<link rel="profile" href="http://gmpg.org/xfn/11">
	<link rel="pingback" href="<?php bloginfo( 'pingback_url' ); ?>">
	<!--[if lt IE 9]>
	<script src="//cdnjs.cloudflare.com/ajax/libs/html5shiv/r29/html5.min.js"></script>
	<![endif]-->
	<?php wp_head(); ?>
</head>

<body <?php body_class(); ?>>
	<div id="page" class="hfeed site"> <!-- This will be closed in footer -->
	<?php
		$page_name = 'header';
		$section_name = 'container';
		$container_class = _bootstrap_get_redux_option( $page_name, $section_name, 'width' );
		$options = _bootstrap_get_container_options( $page_name, $section_name );
		$container_class .= ' ' . $options['container_class'];		
	?>
		<header id="masthead" class="site-header <?php echo $container_class; ?>" role="banner">
			<nav role="navigation">
				<div id="primary-menu" class="<?php echo _bootstrap_get_navbar_class( 'top', 'header' ); ?>">
						<!-- .navbar-toggle is used as the toggle for collapsed navbar content -->
						<div class="navbar-header">
							<button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target=".navbar-responsive-collapse">
								<span class="sr-only">
									Toggle navigation
								</span>
								<span class="icon-bar">
								</span>
								<span class="icon-bar">
								</span>
								<span class="icon-bar">
								</span>
							</button>
							<a class="navbar-brand" href="<?php bloginfo( 'url' ); ?>" title="Home">
								<?php bloginfo( 'name' ); ?>
							</a>
						</div>
						<div class="navbar-collapse navbar-responsive-collapse navbar-right collapse" style="height: 1px;">
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
						</div>
				</div>
			</nav>
		</header><!-- #masthead -->