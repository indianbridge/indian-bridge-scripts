<?php
/**
 * @package _bootstrap
 */
?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	<div class="panel panel-primary">
	  <div class="panel-heading">
	    <h3 class="panel-title">
	    	<a href="<?php the_permalink(); ?>" rel="bookmark"><?php the_title(); ?> </a>
	    	<?php edit_post_link( __( 'Edit', '_bootstrap' ), '<span class="edit-link">', '</span>' ); ?>
	    </h3>
	    
	  </div>
	  <div class="panel-body">
	    <?php the_excerpt(); ?>

	  </div>
	  <div class="panel-footer">
	  	<?php _bootstrap_get_published_date( ); ?>
	  	<?php _bootstrap_get_updated_date( ); ?>
	  	<?php _bootstrap_posted_by(); ?>
	  	<?php _bootstrap_post_categories(); ?>
	  	<?php _bootstrap_post_tags(); ?>
	  	<?php 
	  		if ( comments_open() ) {
				_bootstrap_post_comments();
			}
		?>
	  </div>
	</div>	
</article><!-- #post-## -->
