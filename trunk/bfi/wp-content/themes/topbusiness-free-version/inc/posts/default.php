<div class="item itd">

	<h3><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a><?php edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' ) ?></h3>

	<?php	// METAS
	if ($theme_options['post_metas']=='enable') : ?>
		<div class="l">
			<span class="ico tim"><?php the_time(get_option('date_format')); ?></span>
			<?php	// COMMENTS
			if ($theme_options['post_comments']=='enable') : ?>
					<span class="ico"><!-- divider --></span>
					<span class="ico com"><?php comments_popup_link(__('Leave a reply','pandathemes'), __('1 Comment','pandathemes'), __('% Comments','pandathemes')); ?></span>
				<?php
			endif; ?>
			<span class="ico"><!-- divider --></span>
			<span class="ico cat"><?php the_category(', ') ?></span>
			<div class="clear"><!-- --></div>
		</div>
		<?php
	endif;

	// CONTENT
	the_content('<span>'.__('Read this post','pandathemes').'</span>'); ?>
	
	<div class="m">
		<?php // TAGS
			if ($theme_options['post_metas']=='enable') : the_tags('<span class="ico tag"> ', ', ', '</span>'); endif;
		?>
	</div>

	<div class="clear h5"><!-- --></div>

</div><!-- end item -->