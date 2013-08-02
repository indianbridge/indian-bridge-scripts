<?php

get_header();
$rootDir = get_template_directory_uri();
$imagePath = '/images/icons/16/led-icons/';
echo '<div id="content"><div class="contentbox wfull">';
echo '<div class="clear h10"><!-- --></div>';
if (have_posts()) {
	while (have_posts()) {
		the_post();


		
		echo '<div class="item">';
		$w='120';
		$h='120';
		$a = $theme_options['thumbs_crop_top']=='enable' ? '&a=t' : '';
		// Check featured image from URL
		$feat_img = get_post_meta($post->ID, 'feat_img_value', true);

		if ($feat_img) {
			$src = $feat_img;
			$path = $src;
		}
		else {
			// Get image source
			$src = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'Full Size');
			$src = ($src) ? $src[0] : get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg';
			$path = $src;
		}


		// Display image
		if ($src) {
			$out = '<img id="single-feat-img" class="t1 fl br3" src="' . get_bloginfo('template_url') . '/timthumb.php?src=' . $path . '&amp;h=' . $h . '&amp;w=' . $w . '&amp;zc=3&amp;q=90'. $a . '" width="' . $w . '" alt="' . get_the_title($post->ID) . '" />';
		}

		echo $out;

		echo '<div class="t1-right fr">';
		echo'<h3>';
		the_title();
		edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' );
		echo '</h3>';
		foreach($customFields as $field=>$image) {
			$value = get_post_meta($post->ID,$registerTypeName.'_'.$field,true);
			if (!empty($value)) {
				$imageURL = $rootDir.$imagePath.$image;
				if ($field == 'email') {
					$value = '<a href=mailto:'.$value.'>'.$value.'</a>';
				}
				if ($value == 'true') {
					$value = '<img src="'.$rootDir.$imagePath.'accept.png'.'"/>';
				}
				else if ($value == 'false') {
					$value = '<img src="'.$rootDir.$imagePath.'cross.png'.'"/>';
				}				
				$out = '<div class="icon16" style="background-image:url('.$imageURL.') !important;">'.ucfirst($field).' : '.$value.' </div><br/>';
				echo $out;
			}
		}
		echo '</div><div class="clear"></div><div class="h30"></div>';
		// CONTENT
		the_content();
		echo '</div>';
	}
}

else {
	echo '<h2>404</h2><p>'.__('Sorry, no posts matched your criteria.','pandathemes').'</p>';
}
echo '</div></div><!-- end_of_contentbox -->';


get_footer();

?>