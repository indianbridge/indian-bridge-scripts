<?php
/**
 * The template for displaying Search Results pages.
 *
 * @package bfi_bootstrap
 */

get_header(); ?>

	<section id="primary" class="content-area">
		<main id="main" class="site-main" role="main">
		
		<?php if ( have_posts() ) : ?>

			<div class="col-sm-12">
			<div class="page-header">
				<h1 class="page-title"><?php printf( __( 'Search Results for: %s', 'bfi_bootstrap' ), '<span>' . get_search_query() . '</span>' ); ?></h1>
			</div><!-- .page-header -->
			

			<?php /* Start the Loop */ ?>
			<?php while ( have_posts() ) : the_post(); ?>

				<?php
					/* Include the Post-Format-specific template for the content.
					 * If you want to override this in a child theme, then include a file
					 * called content-___.php (where ___ is the Post Format name) and that will be used instead.
					 */
					get_template_part( 'content', get_post_format() );
				?>

			<?php endwhile; ?>

			<?php _bootstrap_paginate(); ?>

		<?php else : ?>

			<?php get_template_part( 'content', '404' ); ?>

		<?php endif; ?>		
			</div>
		</main><!-- #main -->
	</section><!-- #primary -->
<?php get_footer(); ?>
