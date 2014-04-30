<?php
/**
 * The main template file.
 *
 * This is the most generic template file in a WordPress theme
 * and one of the two required files for a theme (the other being style.css).
 * It is used to display a page when nothing more specific matches a query.
 * E.g., it puts together the home page when no home.php file exists.
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
	<div id="primary" class="content-area">
		<main id="main" class="site-main" role="main">

		<?php if ( have_posts() ) : ?>
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

			<?php get_template_part( 'content', 'none' ); ?>

		<?php endif; ?>

		</main><!-- #main -->
	</div><!-- #primary -->

</div>
<?php if ( $sidebar_location === 'right' ) { ?>
<div class="col-sm-4">
	<?php get_sidebar(); ?>
</div>
<?php } ?>
<?php get_footer(); ?>
