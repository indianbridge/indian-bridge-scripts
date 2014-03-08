<?php
/**
 * The template part for displaying lists of posts.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
 *
 * @package _bootstrap
 */
 
// Get laypout options for post lists/archives pages
$page_name = 'archives';
$section_name = 'layout';
$options = _bootstrap_get_sidebar_options( $page_name, $section_name );
if ( $options['total_width'] > 12 ) {
	?>
		<div class="alert alert-danger alert-dismissable">
		<?php if ( _bootstrap_js_enabled() ) { ?>
			<button type="button" class="close" data-dismiss="alert">&times;</button>
		<?php } 
		printf( __( 'Total width of content and sidebar columns (%d) exceeds 12. Content or Sidebar will wrap to the next row.', '_bootstrap' ), $options['total_width'] ); ?>
		</div>
	<?php	
}

foreach( $options['items'] as $key => $value ) {
	
	if ( ! stristr( $key, 'sidebar') ) {
		if ( $value > 0 && $value < 12 ) {
		?>
		<div class="col-sm-<?php echo $value; ?>">
			<section id="articles">
				<?php
				if ( have_posts() ) {
					/* Start the Loop */ 
					while ( have_posts() ) {
						the_post(); 
						/* Include the Post-Format-specific template for the content.
						* If you want to override this in a child theme, then include a file
						* called content-___.php (where ___ is the Post Format name) and that will be used instead.
						*/
						get_template_part( 'content', get_post_format() );
					} // end while
				}
				else {
					get_template_part( 'content', 'none' );
				}
				?>
			</section>
			<?php
				if ( have_posts() ) {
					// Show pagination if enabled
					_bootstrap_paginate();
				}
			?>
		</div>	
		<?php	
		}
	}
	if ( stristr( $key, 'sidebar') ) {
		if ($value > 0 && $value < 12 ) {
		?>
		<div class="col-sm-<?php echo $value; ?>">
			<?php get_sidebar( $key ); ?>
		</div>
		<?php
		}
	}
}
?>

