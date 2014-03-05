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