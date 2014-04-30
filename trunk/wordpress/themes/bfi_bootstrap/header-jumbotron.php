<?php 
	$page_name = 'header';
	$section_name = 'jumbotron';
	$show_jumbotron = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'show' );
	if ( $show_jumbotron === 'all' || ( $show_jumbotron === 'front' && is_front_page() ) ) {
		$jumbotron_html = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'html' );
?>
<div class="jumbotron">
	<?php echo $jumbotron_html; ?>
</div>	
<?php } ?>	