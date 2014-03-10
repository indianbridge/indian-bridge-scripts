<?php
/**
 * The template for displaying the footer.
 *
 * This needs to close any tags opened before the header
 *
 * @package _bootstrap_html
 */
?>
	<footer role="contentinfo">
		<?php
		get_template_part( 'footer', 'widgets' );
		get_template_part( 'footer', 'copyright' );
		?> 
	</footer>
</div><!-- #page -->
<?php wp_footer(); ?>
</body>
</html>