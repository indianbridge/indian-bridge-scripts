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
$section_name = 'copyright-area';
$show_copyright = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'show' );
if ( $show_copyright ) {
$footer_style = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'style' );
$footer_alignment = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'alignment' );	
?>
	<nav id="footer-menu" class="navbar navbar-<?php echo $footer_style; ?> navbar-<?php echo $footer_alignment; ?>-bottom" role="navigation">
  		<div>
	  		<p class="navbar-text navbar-left">
	  			<?php echo bfi_bootstrap_get_redux_option( $page_name, $section_name, 'left' ); ?>
	  		</p>
	  		<p class="navbar-text navbar-right">
	  			<?php echo bfi_bootstrap_get_redux_option( $page_name, $section_name, 'right' ); ?>
	  		</p>
		</div>
	</nav>

<?php } ?>