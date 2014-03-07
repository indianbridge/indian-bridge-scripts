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
$elements = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'layout' ), 'enabled' );
//$elements = $layout['enabled'];
$total_width = 0;
foreach( $elements as $key => $value ) {
	$width[ $key ] = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, $key . '_width' ) );
	$total_width += $width[ $key ];
}

if ( $total_width > 12 ) {
	?>
		<div class="alert alert-danger alert-dismissable">
		<button type="button" class="close" data-dismiss="alert">&times;</button>
		  <?php printf( __( 'Total width of content and sidebar columns (%d) exceeds 12. Content or Sidebar will wrap to the next row.', '_bootstrap' ), $total_width ); ?>
		</div>
	<?php	
}

foreach( $elements as $key => $value ) {
	if ( stristr( $key, 'content') !== FALSE ) {
		if ( $width[ $key ] > 0 && $width[ $key ] < 12 ) {
		?>
		<div class="col-sm-<?php echo $width[ $key ]; ?>">
			<div id="articles">
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
			</div>
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
	else if ( stristr( $key, 'sidebar') !== FALSE ) {
		if ( $width[ $key ] > 0 && $width[ $key ] < 12 ) {
		?>
		<div class="col-sm-<?php echo $width[ $key ]; ?>">
			<?php get_sidebar( $key ); ?>
		</div>
		<?php
		}
	}
}
?>

