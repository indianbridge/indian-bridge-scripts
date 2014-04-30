<?php
/**
 * The template for displaying Archive pages.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
 *
 * @package bfi_bootstrap
 */

get_header(); 
$page_name = 'sidebar';
$section_name = 'widgets';
$sidebar_location = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'location' );
?>
<?php if ( $sidebar_location === 'left' ) { ?>
<div class="col-sm-4">
	<?php get_sidebar(); ?>
</div>
<?php } ?>
<div class="col-sm-8">
	<section id="primary" class="content-area">
		<main id="main" class="site-main" role="main">

		<?php if ( have_posts() ) : ?>

			<?php _bootstrap_echo_archives_page_header(); ?>
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

		</main><!-- #main -->
	</section><!-- #primary -->
</div>
<?php if ( $sidebar_location === 'right' ) { ?>
<div class="col-sm-4">
	<?php get_sidebar(); ?>
</div>
<?php } ?>
<?php get_footer(); ?>
