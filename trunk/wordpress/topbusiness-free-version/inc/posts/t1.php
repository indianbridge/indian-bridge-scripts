<div class="item it1">

	<?php 

	// T H U M B N A I L

		// Dimentions
		$w = '100';
		$h = $theme_options['square_thumbs']=='enable' ? $w : 'auto';
		$a = $theme_options['thumbs_crop_top']=='enable' ? '&a=t' : '';

		// Check featured image from URL
		$feat_img = get_post_meta($post->ID, 'feat_img_value', true);

		if ($feat_img) :

			$src = $feat_img;
			$path = $src;

		else :

			// Get image source
			$src = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'thumbnail');
			$src = ($src) ? $src[0] : '';
	
			// For multisite WordPress
			global $blog_id;
			if (isset($blog_id) && $blog_id > 1) {
				$imageParts = explode('/files/', $src);
				if (isset($imageParts[1])) {
					$path = '/blogs.dir/'.$blog_id.'/files/'.$imageParts[1];
				}
			}
	
			// For default WordPress
			else {
				$path = $src;
			};

		endif;

			// Display image
			if ($src) : $out = '<a href="' . get_permalink($post->ID) . '"><img class="t1 fl br3" src="' . $path . '"width="' . $w . '" alt="' . get_the_title($post->ID) . '" /></a>';

			// Else default
			else : $out = '<a href="' . get_permalink($post->ID) . '"><img class="t1 fl br3" src="' . get_bloginfo('template_url') . '/images/blog-thumb-100x100.jpg" width="100" height="100" alt="no image"/></a>';
		
			endif;
		
		echo $out;

	?>

	<div class="t1-right fr">

		<h3><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a><?php edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' ) ?></h3>

		<?php	// METAS
		if ($theme_options['post_metas']=='enable') : ?>
			<div class="l">
				<span title="Author"><?php echo get_avatar( get_the_author_email(), '16'); ?> <?php the_author_posts_link(); ?></span>
				<span class="ico"><!-- divider --></span>
				<span class="ico tim" title="Post Date"><?php the_time(get_option('date_format')); ?></span>
				<span class="ico"><!-- divider --></span>
				<span class="ico cat" title="Categories"><?php the_category(', '); ?></span>
				<span class="ico"><!-- divider --></span>
				<span class="ico tag" title="Tags"><?php the_tags('',',',''); ?></span>
				<?php	// COMMENTS
				if ($theme_options['post_comments']=='enable') : ?>
						<span class="ico"><!-- divider --></span>
						<span class="ico com" title="Comments"><?php comments_popup_link(__('Leave a reply','pandathemes'), __('1 Comment','pandathemes'), __('% Comments','pandathemes')); ?></span>
					<?php
				endif; ?>
			</div>
			<?php
		endif;
		echo '<p>'.get_the_excerpt().'<span class="btwrap"><a class="button" href="'.get_permalink().'"><span>'.__('Read More','pandathemes').'</span></a></span></p>';

		echo '<div class="h30"><!-- --></div>';?>

	</div>

	<div class="clear"><!-- --></div>
</div><!-- end item -->