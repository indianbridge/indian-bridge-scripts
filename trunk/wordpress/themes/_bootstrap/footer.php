<?php
/**
 * The template for displaying the footer.
 *
 * Contains the closing of the #content div and all content after
 *
 * @package _bootstrap
 */
?>

	<footer id="colophon" class="site-footer alert alert-info" role="contentinfo">
	<div class="row">
<?php
// Get laypout options for post lists/archives pages
$page_name = 'footer';
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
	</div>
		<div class="site-info row">
			<a href="http://wordpress.org/" rel="generator"><?php printf( __( 'Proudly powered by %s', '_bootstrap' ), 'WordPress' ); ?></a>
			<span class="sep"> | </span>
			<?php printf( __( 'Theme: %1$s by %2$s.', '_bootstrap' ), '_bootstrap', '<a href="http://nsriram71.appspot.com" rel="designer">Sriram Narasimhan</a>' ); ?>
		</div><!-- .site-info -->
	</footer><!-- #colophon -->
	
</div><!-- #page -->

<?php wp_footer(); ?>

</body>
</html>