<?php
/**
 * The template for displaying Archive pages.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
 *
 * @package _bootstrap
 */
 
get_header(); ?>
<div id="content" class="site-content container">
	<div class="row">
		<?php _bootstrap_echo_archives_page_header(); ?>
		<?php get_template_part( 'content', 'archive' ); ?>
	</div><!-- .row -->
</div><!-- #content .container -->
<?php get_footer(); ?>