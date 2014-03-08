<?php
/**
 * The template for displaying the footer.
 *
 * Contains the closing of the #content div and all content after
 *
 * @package _bootstrap
 */
?>

<footer id="colophon" class="site-footer" role="contentinfo">
	
	<?php
	// Get laypout options for post lists/archives pages
	$page_name = 'footer';
	$section_name = 'widget_area';
	$show_widget_area = _bootstrap_get_redux_option( $page_name, $section_name, 'show' );
	if ( $show_widget_area ) {
		$section_name = 'widget_area_container';
		$container_class = _bootstrap_get_redux_option( $page_name, $section_name, 'width' );
		$options = _bootstrap_get_container_options( $page_name, $section_name );
		$container_class .= ' ' . $options['container_class'];	
		?>
		<div class="<?php echo $container_class; ?>">
		<?php		
		$section_name = 'widgets';
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
	<?php 
	} 
	$page_name = 'footer';
	$section_name = 'copyright_area';
	$show_copyright_area = _bootstrap_get_redux_option( $page_name, $section_name, 'show' );
	if ( $show_copyright_area ) {
		$options = _bootstrap_get_navbar_options( $page_name );
		?>
		<div class="site-info row">
			<nav id="footer-menu" class="navbar navbar-<?php echo $options['navbar_color']; ?> navbar-<?php echo $options['navbar_style']; ?>-bottom">
		  		<div class="container">
		  		<p class="navbar-text navbar-left"><?php echo _bootstrap_get_redux_option( $page_name, $section_name, 'left' ); ?></p>
		  		<p class="navbar-text navbar-right"><?php echo _bootstrap_get_redux_option( $page_name, $section_name, 'right' ); ?></p>
				</div>
			</nav>
		</div><!-- .site-info -->
	<?php } ?>
</footer><!-- #colophon -->
	
</div><!-- #page -->

<?php wp_footer(); ?>

</body>
</html>