<?php
/**
 * @package bfi_bootstrap
 */
?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	<div class="panel panel-default">
		<div class="panel-heading">		
			<span class="panel-title"><?php the_title(); ?></span> <?php edit_post_link( 'Edit this Page', '(', ')' ); ?>
		</div>
		<div class="panel-body">
			<?php _bootstrap_show_post_content( true, true ); ?>
		</div>
		<div class="panel-footer">
			<?php _bootstrap_display_meta_information(); ?>
		</div>		
	</div>	
</article><!-- #post-## -->
