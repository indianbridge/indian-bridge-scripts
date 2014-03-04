<?php
/**
 * @package _bootstrap
 */
?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	<?php
		$style = _bootstrap_get_option( '_bootstrap_post_lists_excerpt_container_style' );
		$class = _bootstrap_get_option( '_bootstrap_post_lists_excerpt_container_class' );
		$meta_location = _bootstrap_get_option( '_bootstrap_post_lists_meta_information_location' );
		$show_meta = _bootstrap_get_option( '_bootstrap_post_lists_show_meta_information');
		if ( $style == 'panel' ) {
	?>
			<div class="panel panel-<?php echo $class; ?>">
			  <div class="panel-heading">
			    <h3 class="panel-title">
			    	<a href="<?php the_permalink(); ?>" rel="bookmark"><?php the_title(); ?> </a>
			    	<?php edit_post_link( __( 'Edit', '_bootstrap' ), '<span class="edit-link">', '</span>' ); ?>
			    </h3>
			  </div>
			  <?php if ( $show_meta && $meta_location === 'top' ) { ?>
				  <div class="panel-footer">
				  	<?php _bootstrap_display_meta_information(); ?>
				  </div>			  
			  <?php } ?>
			  <div class="panel-body">
			  	<?php _bootstrap_show_post_content(); ?>
			  </div>
			  <?php if ( $show_meta && $meta_location === 'bottom' ) { ?>
				  <div class="panel-footer">
				  	<?php _bootstrap_display_meta_information(); ?>
				  </div>
			  <?php } ?>
			</div>	
	<?php
		}
		else if ( $style == 'alert' ) {
	?>
			<div class="alert alert-<?php echo $class; ?>">
			  <div>
			    <h3>
			    	<a href="<?php the_permalink(); ?>" rel="bookmark"><?php the_title(); ?> </a>
			    	<?php edit_post_link( __( 'Edit', '_bootstrap' ), '<span class="edit-link">', '</span>' ); ?>
			    </h3>		    
			  </div>
			  <?php if ( $show_meta && $meta_location === 'top' ) { ?>
				  <div>
					<?php _bootstrap_display_meta_information(); ?>
				  </div>			  
			  <?php } ?>
			  <div>
			  	<?php _bootstrap_show_post_content(); ?>
			  </div>
			  <?php if ( $show_meta && $meta_location === 'bottom' ) { ?>
				  <div>
					<?php _bootstrap_display_meta_information(); ?>
				  </div>
			  <?php } ?>
			</div>	
	<?php	
		}
		else {
			$container_class = ( $style === 'well' ? 'well' : 'row' );
			$container_class .= ' text-' . $class;
	?>	
			<div class="<?php echo $container_class; ?>">
			  <div>
			    <h3>
			    	<a href="<?php the_permalink(); ?>" rel="bookmark"><?php the_title(); ?> </a>
			    	<?php edit_post_link( __( 'Edit', '_bootstrap' ), '<span class="edit-link">', '</span>' ); ?>
			    </h3>
			  </div>
			  <?php if ( $show_meta && $meta_location === 'top' ) { ?>
				  <div>
					<?php _bootstrap_display_meta_information(); ?>
				  </div>			  
			  <div>
			  	<?php _bootstrap_show_post_content(); ?>
			  </div>
			  <?php } ?>
			  <?php if ( $show_meta && $meta_location === 'bottom' ) { ?>
				  <div>
					<?php _bootstrap_display_meta_information(); ?>
				  </div>	
			  <?php } ?>		
			</div>
	<?php
		}
	?>	
</article><!-- #post-## -->
