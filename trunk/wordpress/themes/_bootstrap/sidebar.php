<?php
/**
 * The Sidebar containing the main widget areas.
 *
 * @package _bootstrap
 */

	// Get the options
	if ( ! dynamic_sidebar( 'sidebar-1' ) ) {
		$options = _bootstrap_get_sidebar_options();	
		?>
		<div id="secondary" class="col-sm-4 widget-area" role="complementary">
		<aside id="post-<?php the_ID(); ?>" <?php post_class( $options['container_class'] ); ?> >
			<header class="<?php echo $options['header_class']; ?>" >
				<<?php echo $options['title_tag']; ?> class="<?php echo $options['title_class']; ?>" >
					No Widgets Found!
				</<?php echo $options['title_tag']; ?>>
			</header>			
			<!-- Show content/excerpt -->
			<section class="<?php echo $options['body_class']; ?>">
				<p>You can either add some widgets <a href="<?php echo admin_url( 'widgets.php' ); ?>">here</a> or change the layout in theme options to not show a sidebar.</p>
			</section>

		</aside><!-- #post-## -->
		</div><!-- #secondary .col-sm-4 -->
	<?php
	} // end sidebar widget area
	?>
