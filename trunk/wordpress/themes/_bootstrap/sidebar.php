<?php
/**
 * The Sidebar containing the main widget areas.
 *
 * @package _bootstrap
 */
?>
	<div class="col-sm-4">
	<div id="secondary" class="widget-area" role="complementary">
		<?php if ( ! dynamic_sidebar( 'sidebar-1' ) ) : ?>

			<aside id="search" class="well widget widget_search">
				<?php get_search_form(); ?>
			</aside>

			<aside id="archives" class="well widget">
				<h1 class="widget-title"><?php _e( 'Archives', '_bootstrap' ); ?></h1>
				<ul>
					<?php wp_get_archives( array( 'type' => 'monthly' ) ); ?>
				</ul>
			</aside>

			<aside id="meta" class="well widget">
				<h1 class="widget-title"><?php _e( 'Meta', '_bootstrap' ); ?></h1>
				<ul>
					<?php wp_register(); ?>
					<li><?php wp_loginout(); ?></li>
					<?php wp_meta(); ?>
				</ul>
			</aside>

		<?php endif; // end sidebar widget area ?>
	</div><!-- #secondary -->
	</div><!-- .col-sm-4 -->
