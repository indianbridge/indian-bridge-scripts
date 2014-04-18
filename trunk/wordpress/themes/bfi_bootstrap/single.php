<?php
/**
 * The Template for displaying all single posts.
 *
 * @package bfi_bootstrap
 */

get_header(); 
$container_class = bfi_bootstrap_get_container_options( 'content' );

?>
<div class="col-sm-8">
	<div id="primary" class="content-area">
		<main id="main" class="site-main" role="main">

		<?php while ( have_posts() ) : the_post(); ?>

			<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
				<div class="<?php echo $container_class; ?>">
					<div class="panel-heading">		
						<span class="panel-title"><?php the_title(); ?></span> <?php edit_post_link( 'Edit this Page', '(', ')' ); ?>
					</div>
					<div class="panel-footer">
						<?php _bootstrap_display_meta_information(); ?>
					</div>
					<div class="panel-body">
						<?php _bootstrap_show_post_content( true, false); ?>
					</div>
				</div>	

			</article><!-- #post-## -->

			<?php bfi_bootstrap_post_nav(); ?>

			<?php
				// If comments are open or we have at least one comment, load up the comment template
				if ( comments_open() || '0' != get_comments_number() ) :
					comments_template();
				endif;
			?>

		<?php endwhile; // end of the loop. ?>

		</main><!-- #main -->
	</div><!-- #primary -->

	</div>
<div class="col-sm-4">
<?php get_sidebar(); ?>
</div>
<?php get_footer(); ?>