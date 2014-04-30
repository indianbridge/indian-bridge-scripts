<?php
/**
 * The template for displaying all pages.
 *
 * This is the template that displays all pages by default.
 * Please note that this is the WordPress construct of pages
 * and that other 'pages' on your WordPress site will use a
 * different template.
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
<?php if ( $sidebar_location === 'right' ) { ?>
<div class="col-sm-4">
	<?php get_sidebar(); ?>
</div>
<?php } ?>
<?php get_footer(); ?>
