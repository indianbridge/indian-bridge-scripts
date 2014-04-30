<?php
/**
 * The template for displaying the footer.
 *
 * This needs to close any tags opened before the header
 *
 * @package _bootstrap_html
 */
?>
</div>
<div class="row">
<?php
get_template_part( 'footer', 'widgets' );
?>
</div>
<div class="row">
<?php
get_template_part( 'footer', 'copyright' );
?> 
</div>
</div><!-- #page -->
<?php wp_footer(); ?>
</body>
</html>
