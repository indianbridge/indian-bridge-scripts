<?php
/**
 * The template for displaying 404 pages (Not Found).
 *
 * @package _bootstrap
 */

get_header(); ?>

	<div id="primary" class="content-area">
		<main id="main" class="site-main" role="main">

			<section class="error-404 not-found">
				<div class="alert alert-warning alert-dismissable">
				<?php if ( _bootstrap_js_enabled() ) { ?>
				<button type="button" class="close" data-dismiss="alert">&times;</button>
				<?php } ?>
				  <?php _e( 'Oops! That page can&rsquo;t be found. Try one of these other options.', '_bootstrap' ); ?>
				  <strong>Warning!</strong> Better check yourself, you're not looking too good.
				</div>			
				<header class="page-header">
					<h1 class="page-title"><?php _e( 'Oops! That page can&rsquo;t be found.', '_bootstrap' ); ?></h1>
				</header><!-- .page-header -->

				<div class="page-content">
					<p><?php _e( 'It looks like nothing was found at this location. Maybe try one of the links below or a search?', '_bootstrap' ); ?></p>

					<?php get_search_form(); ?>

					<?php the_widget( 'WP_Widget_Recent_Posts' ); ?>

					<?php if ( _bootstrap_categorized_blog() ) : // Only show the widget if site has multiple categories. ?>
					<div class="widget widget_categories">
						<h2 class="widgettitle"><?php _e( 'Most Used Categories', '_bootstrap' ); ?></h2>
						<ul>
						<?php
							wp_list_categories( array(
								'orderby'    => 'count',
								'order'      => 'DESC',
								'show_count' => 1,
								'title_li'   => '',
								'number'     => 10,
							) );
						?>
						</ul>
					</div><!-- .widget -->
					<?php endif; ?>

					<?php
					/* translators: %1$s: smiley */
					$archive_content = '<p>' . sprintf( __( 'Try looking in the monthly archives. %1$s', '_bootstrap' ), convert_smilies( ':)' ) ) . '</p>';
					the_widget( 'WP_Widget_Archives', 'dropdown=1', "after_title=</h2>$archive_content" );
					?>

					<?php the_widget( 'WP_Widget_Tag_Cloud' ); ?>

				</div><!-- .page-content -->
			</section><!-- .error-404 -->

		</main><!-- #main -->
	</div><!-- #primary -->

<?php get_footer(); ?>