<?php
/**
 * The template for displaying full width pages
 * @package bfi_bootstrap
 * Template Name: Full-Width
 */

get_header(); ?>

<div class="col-sm-12">
	<div id="primary" class="content-area">
		<main id="main" class="site-main" role="main">

			<?php while ( have_posts() ) : the_post(); ?>
				<?php
					$parent_id  = $post->post_parent;
					if ( $parent_id ) {
						global $post;
						$breadcrumbs = array();
						$breadcrumbs[] = '<li class="active">' . get_the_title() . '</li>';
						while ( $parent_id ) {
							$page = get_page($parent_id);
							$breadcrumbs[] = '<li><a href="' . get_permalink($page->ID) . '">' . get_the_title($page->ID) . '</a></li>';
							$parent_id  = $page->post_parent;
						}
						$breadcrumbs = array_reverse($breadcrumbs);
						echo '<ol class="breadcrumb">';
						for ($i = 0; $i < count($breadcrumbs); $i++) {
							echo $breadcrumbs[$i];
						}
						echo '</ol>';
					}
				?>
				<?php get_template_part( 'content', 'page' ); ?>

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
<?php get_footer(); ?>
