<?php
/**
 * @package bfi_bootstrap
 */

$page_name = 'content';
$container_class = bfi_bootstrap_get_container_options( $page_name );
$section_name = 'meta';
$show_meta = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'show' );
$meta_location = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'location' );
?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	<div class="<?php echo $container_class; ?>">
		<div class="panel-heading">		
			<span class="panel-title"><a href="<?php echo get_permalink(); ?>"><?php the_title(); ?></a></span> <?php edit_post_link( 'Edit this Page', '(', ')' ); ?>
		</div>
		<?php if ( $show_meta && $meta_location === 'top' ) { ?>
		<div class="panel-footer">
			<?php _bootstrap_display_meta_information(); ?>
		</div>
		<?php } ?>		
		<div class="panel-body">
			<?php _bootstrap_show_post_content( true, true ); ?>
		</div>
		<?php if ( $show_meta && $meta_location === 'bottom' ) { ?>
		<div class="panel-footer">
			<?php _bootstrap_display_meta_information(); ?>
		</div>
		<?php } ?>		
	</div>	
</article><!-- #post-## -->
