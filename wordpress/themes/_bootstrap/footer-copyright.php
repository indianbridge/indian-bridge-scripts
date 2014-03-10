<?php
/**
 * The template part for showing a copyrights navbar in the footer.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
 *
 * @package _bootstrap_html
 */
// Get options for footer copyright area containter
$page_name = 'footer';
$section_name = 'copyright_area_container';
$options = _bootstrap_get_area_container_options( $page_name, $section_name );
$section_name = 'copyright_area';
if ( $options['show'] ) {
?>
	<div class="<?php echo $options['container_class']; ?>">
		<nav id="footer-menu" class="<?php echo _bootstrap_get_navbar_class( 'bottom', 'footer' ); ?>">
	  		<div class="container">
		  		<p class="navbar-text navbar-left">
		  			<?php echo _bootstrap_get_redux_option( $page_name, $section_name, 'left' ); ?>
		  		</p>
		  		<p class="navbar-text navbar-right">
		  			<?php echo _bootstrap_get_redux_option( $page_name, $section_name, 'right' ); ?>
		  		</p>
			</div>
		</nav>
	</div>
<?php 
}
?>