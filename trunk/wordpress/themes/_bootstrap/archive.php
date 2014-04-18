<?php
/**
 * The template for displaying Archive pages.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
 *
 * @package _bootstrap
 */
 
get_header(); 
$options = _bootstrap_get_area_container_options( 'header' );
if ( $options['show'] ) {
?>
<div id="content" class="site-content <?php echo $options['container_class']; ?>">
	<div class="row">
		<?php _bootstrap_echo_archives_page_header(); ?>
		<?php get_template_part( 'content', 'archive' ); ?>
	</div><!-- .row -->
</div><!-- #content .container -->
<?php
}
get_footer(); ?>