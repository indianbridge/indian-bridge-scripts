<?php
/**
 * @package _bootstrap
 */

// The type of the container to be used display each article.
$style = _bootstrap_get_option( '_bootstrap_post_lists_excerpt_container_style' );

$class_id = '';
switch ( $style ) {
	// Bootstrap panel container
	case 'panel' :
		$class_id = '_bootstrap_post_lists_excerpt_container_class_panel';
		break;
	
	// Bootstrap alert container
	case 'alert' :
		$class_id = '_bootstrap_post_lists_excerpt_container_class_alert';
		break;
		
	// Bootstrap well and plain text
	case 'well' :
	case 'plain' :
	default :
		$class_id = '_bootstrap_post_lists_excerpt_container_class_text';
		break;	
}

// The class of the container or the text within.
$class = _bootstrap_get_option( $class_id );

// Should the meta information be shown?
$show_meta = _bootstrap_get_option( '_bootstrap_post_lists_show_meta_information' );

// The location of the meta information display.
$meta_location = _bootstrap_get_option( '_bootstrap_post_lists_meta_information_location' );

// The style to be applied to title.
$title_tag = _bootstrap_get_option( '_bootstrap_post_lists_excerpt_title_style' );
if ( $title_tag === 'custom' ) {
	$title_tag = _bootstrap_get_option( '_bootstrap_post_lists_excerpt_title_custom' );
}

// Classes for article sections that will conditionalized based on type of container being used.
$container_class = '';
$header_class = '';
$title_class = '';
$body_class = '';
$footer_class = '';

// Setup classes for all sections of the article depending on type of container selected
switch ( $style ) {
	
	// Bootstrap panel container
	case 'panel' :
		$container_class = 'panel panel-' . $class; 
		$header_class = 'panel-heading';
		$title_class = 'panel-title';
		$body_class = 'panel-body';
		$footer_class = 'panel-footer';
		break;
	
	// Bootstrap alert container
	case 'alert' :
		$container_class = 'alert alert-' . $class;
		break;
		
	// Bootstrap well and text is assigned class
	case 'well' :
		$container_class = 'well text-' . $class;
		break;
	
	// No container but text is assigned class
	case 'plain' :
	default :
		$container_class = 'text-' . $class;
		break;
}	
?>
<article id="post-<?php the_ID(); ?>" <?php post_class( $container_class ); ?> >
	<header class="<?php echo $header_class; ?>" >
		<<?php echo $title_tag; ?> class="<?php echo $title_class; ?>" >
			<a href="<?php the_permalink(); ?>" rel="bookmark">
				<?php the_title(); ?> 
			</a>
			
			<?php 
				// Show the edit link for users that are allowed to edit post.
				$edit_url = get_edit_post_link();
				if ( $edit_url ) {
					?>
						<a href="<?php echo get_edit_post_link(); ?>" 
						class="btn btn-xs">
							<i class="fa fa-edit"></i> Edit	
						</a>
					<?php
				}
			?>			
		</<?php echo $title_tag; ?>>
	</header>			
	<!-- Show meta information on top -->
	<?php if ( $show_meta && $meta_location === 'top' ) { ?>
		<footer class="<?php echo $footer_class; ?>">
			<?php _bootstrap_display_meta_information(); ?>	
		</footer>
	<?php } ?>
	<!-- Show content/excerpt -->
	<section class="<?php echo $body_class; ?>">
		<?php _bootstrap_show_post_content(); ?>
	</section>
	<!-- Show meta information in the bottom -->
	<?php if ( $show_meta && $meta_location === 'bottom' ) { ?>
		<footer class="<?php echo $footer_class; ?>">
			<?php _bootstrap_display_meta_information(); ?>	
		</footer>
	<?php } ?>
</article><!-- #post-## -->
