<?php
/**
 * The template part for showing widgets in the footer.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
 *
 * @package _bootstrap_html
 */
// Get footer widget area options
$page_name = 'footer';
$section_name = 'widget_area_container';
$options = _bootstrap_get_area_container_options( $page_name, $section_name );
if ( $options['show'] ) {
	?>
	<div class="<?php echo $options['container_class']; ?>">
		<?php		
			// Get footer widget options
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
?>