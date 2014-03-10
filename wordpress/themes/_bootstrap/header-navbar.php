<?php
/**
 * The template part for showing a navbar in the header.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
 *
 * @package _bootstrap_html
 */
$options = _bootstrap_get_area_container_options( 'header' );
if ( $options['show'] ) {
?>
<nav role="navigation" class="<?php echo $options['container_class']; ?>">
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
<?php
}
?>