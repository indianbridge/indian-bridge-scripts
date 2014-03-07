<?php
/**
 * @package _bootstrap
 */

// The type of the container to be used display each article.
$page_name = 'archives';
$section_name = 'content';
$options = _bootstrap_get_container_options( $page_name, $section_name );
$options['container_class'] .= ' item';

$section_name = 'meta';
// Should the meta information be shown?
$show_meta = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'show' ) );

// The location of the meta information display.
$meta_location = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'location' ) );
?>
<article id="post-<?php the_ID(); ?>" <?php post_class( $options['container_class'] ); ?> >
	<header class="<?php echo $options['header_class']; ?>" >
		<<?php echo $options['title_tag']; ?> class="<?php echo $options['title_class']; ?>" >
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
		</<?php echo $options['title_tag']; ?>>
	</header>			
	<!-- Show meta information on top -->
	<?php if ( $show_meta && $meta_location === 'top' ) { ?>
		<footer class="<?php echo $options['footer_class']; ?>">
			<?php _bootstrap_display_meta_information(); ?>	
		</footer>
	<?php } ?>
	<!-- Show content/excerpt -->
	<section class="<?php echo $options['body_class']; ?>">
		<?php _bootstrap_show_post_content(); ?>
	</section>
	<!-- Show meta information in the bottom -->
	<?php if ( $show_meta && $meta_location === 'bottom' ) { ?>
		<footer class="<?php echo $options['footer_class']; ?>">
			<?php _bootstrap_display_meta_information(); ?>	
		</footer>
	<?php } ?>
</article><!-- #post-## -->
