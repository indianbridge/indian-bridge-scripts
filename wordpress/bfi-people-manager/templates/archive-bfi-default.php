<?php
get_header();
echo '<div id="wrapper"><div id="content"><div class="contentbox wfull">';
$args=array(
		'post_type' => $registerTypeName,
		'post_status' => 'publish',
		'posts_per_page' => -1,
		'caller_get_posts'=> 1,
		'order' => 'ASC',
		'orderby' => 'title'
);
$query = new WP_Query( $args );
$rootDir = get_template_directory_uri();
$imagePath = '/images/icons/16/led-icons/';
// The Loop
if ( $query->have_posts() ) {
	echo '<div id="archive">';
	while ( $query->have_posts() ) {
		$query->the_post();
		$post_id = get_the_ID();
		echo '<div class="item">';
		$w='120';
		$h='120';
		$a = $theme_options['thumbs_crop_top']=='enable' ? '&a=t' : '';
			
		// Check featured image from URL
		$feat_img = get_post_meta($post_id, 'feat_img_value', true);
			
		if ($feat_img) :
			
		$src = $feat_img;
		$path = $src;
			
		else :
			
		// Get image source
		$src = wp_get_attachment_image_src(get_post_thumbnail_id($post_id), 'Full Size');
		$src = ($src) ? $src[0] : get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg';
		$path = $src;
			
		endif;
		// Display image
		if ($src) :
		$out = '<a href="'.get_permalink().'"><img id="single-feat-img" class="t1 fl br3" src="' . get_bloginfo('template_url') . '/timthumb.php?src=' . $path . '&amp;h=' . $h . '&amp;w=' . $w . '&amp;zc=3&amp;q=90'. $a . '" width="' . $w . '" alt="' . get_the_title() . '" /></a>';
		endif;
		echo $out;
		echo '<div class="t1-right fr">';
		echo '<h3><a href="'.get_permalink().'">';
		the_title();
		edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' );
		echo '</a></h3>';
		foreach($customFields as $field=>$image) {
			$value = get_post_meta($post_id,$registerTypeName.'_'.$field,true);
			$imageURL = $rootDir.$imagePath.$image;
			if ($field == 'email') {
				$value = '<a href=mailto:'.$value.'>'.$value.'</a>';
			}
			$out = '<div class="icon16" style="background-image:url('.$imageURL.') !important;">'.ucfirst($field).' : '.$value.' </div><br/>';
			echo $out;
		}
		echo '<div class="clear"></div><div class="h30"></div></div><div class="clear"></div>';
		echo '<p>'.get_the_excerpt().'<span class="btwrap"><a class="button" href="'.get_permalink().'"><span>'.__('Read More','pandathemes').'</span></a></span></p>';
		echo '<div class="clear"></div></div>';
	}
	echo '</div>';
} else {
	echo '<h1>No State Association information found</h1>';
}

/* Restore original Post Data */
wp_reset_postdata();

echo '</div>';
get_footer(); ?>