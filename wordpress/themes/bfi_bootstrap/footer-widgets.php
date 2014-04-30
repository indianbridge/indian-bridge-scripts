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
$section_name = 'widgets';
$show_copyright = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'show' );
if ( $show_copyright ) {
	$container_class = bfi_bootstrap_get_container_options( $page_name );
	$area = 'Footer';
	$number_of_sidebars = 3;
	?>
	<div class="<?php echo $container_class; ?>">
		<div class="panel-body">
		<?php
			for ( $i = 1; $i <= $number_of_sidebars; $i++ ) {
			$key = sprintf( '%s-sidebar-%d', strtolower( $area ), $i );
		?>
			<div class="col-sm-4">
				<?php get_sidebar( $key ); ?>
			</div>	
		<?php
			}
		?>	
		</div>	
	</div>	
<?php
}
?>