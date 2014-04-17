<?php
/**
 * The template used for displaying page content in page.php
 *
 * @package bfi_bootstrap
 */
?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	<div class="panel panel-success">
		<div class="panel-heading">		
			<span class="panel-title"><?php the_title(); ?></span> <?php edit_post_link( 'Edit this Page', '(', ')' ); ?>
		</div>
		<div class="panel-body">
			<?php the_content(); ?>
		</div>
	</div>	
</article><!-- #post-## -->
