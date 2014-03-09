<?php
/**
 * The template for displaying the footer.
 *
 * Contains the closing of the #content div and all content after
 *
 * @package _bootstrap_html
 */
?>
	<footer id="colophon" class="site-footer" role="contentinfo">
	<?php
		_bootstrap_show_footer_widgets();
		_bootstrap_show_copyrights();
	?> 
	</footer><!-- #colophon -->
</div><!-- #page -->
<?php wp_footer(); ?>
</body>
</html>