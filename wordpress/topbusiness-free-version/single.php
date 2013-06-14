<?php

	get_header();

	if (have_posts()) : while (have_posts()) : the_post(); 

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true);

	?>

	<div id="content">

		<div class="contentbox <?php
			if ($sidebar == 'No sidebar') :

				echo'wfull';

			elseif ($sidebar == 'Buddy Sidebar') :

				echo'w720';

			else :

				echo'w620';

			endif; ?>">

			<?php
				// BREADCRUMBS
				if ( function_exists( 'breadcrumb_trail' ) ) {
					breadcrumb_trail(	array(
						'before'		=> __('Browse:','pandathemes'),
						'show_home'		=> __('Home','pandathemes'),
						'front_page'	=> true,
						'separator'		=> '>'
					));
				}
			?>

			<div class="clear h10"><!-- --></div>

			<?php
			// FEATURED IMAGE
			
			if ( $theme_options['post_feat_image']=='enable' && $post->post_type == 'post') :

				// Dimentions
				//$w = '300';
				//$h = $theme_options['square_thumbs']=='enable' ? $w : 'auto';
				$w='240';
				$h='240';
				$a = $theme_options['thumbs_crop_top']=='enable' ? '&a=t' : '';

				// Check featured image from URL
				$feat_img = get_post_meta($post->ID, 'feat_img_value', true);

				if ($feat_img) :

					$src = $feat_img;
					$path = $src;

				else :

					// Get image source
					$src = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'Full Size');
					$src = ($src) ? $src[0] : get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg';
			
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
					if ($src) :

						if ( $theme_options['post_feat_image_link']=='enable' ) :

							$out = '<a rel="prettyPhoto" href="' . $path . '" title="' . $post->post_excerpt . '"><img id="single-feat-img" class="fr ml10 br3" src="' . get_bloginfo('template_url') . '/timthumb.php?src=' . $path . '&amp;h=' . $h . '&amp;w=' . $w . '&amp;zc=3&amp;q=90'. $a . '" width="' . $w . '" alt="' . get_the_title($post->ID) . '" /></a>';

						else :

							$out = '<img id="single-feat-img" class="fr ml10 br3" src="' . get_bloginfo('template_url') . '/timthumb.php?src=' . $path . '&amp;h=' . $h . '&amp;w=' . $w . '&amp;zc=3&amp;q=90'. $a . '" width="' . $w . '" alt="' . get_the_title($post->ID) . '" />';

						endif;
				
					endif;
				
				echo $out;

			endif;
			?>

			<h2><?php the_title(); edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' ) ?></h2>

			<?php

				// Info about post
				// Date
				//$out = '<em class="ico tim" title="Post Date">'.'Test'.'</em>';
				if ($post->post_type == 'post') {
					$out = '<em class="ico tim" title="Post Date">'.get_the_time( get_option('date_format'), $post->ID ).'</em>';

					// author
					$out .= '<div><!-- divider --></div>';
					$author_id = $post->post_author;
					$out .= '<div class="f11" title="Author"><em>'.get_avatar( $author_id,'16' ).' <a href="'.get_author_posts_url( $author_id ).'">'.get_the_author_meta( 'display_name',$author_id).'</a>'.'</em></div>';

					// Posted in
					$out .= '<div><!-- divider --></div>';
					$out .= '<div class="f11 ico cat" title="Categories">'.__('','pandathemes').' '.get_the_category_list(', ','', $post_id = $post->ID).'</div>';

					// Comments
					if ($theme_options['post_comments'] == 'enable') :
						$out .= '<div><!-- divider --></div>';
						$out .= '<div class="f11 ico com" title="Comments">'.__('','pandathemes').' <a href="'.get_comments_link($post->ID).'">'.get_comments_number($post->ID).'</a></div>';
					endif;

					// Tags
					$posttags = get_the_tags($post->ID);
					if ($posttags) :
						$tag = '';
						foreach($posttags as $tagg) {
							$tag .= '<a href="'.get_tag_link($tagg->term_id).'">'.$tagg->name.'</a>, ';
						}
					endif;
					$out .= '<div><!-- divider --></div>';
					$out .= '<div class="f11 ico tag" title="Tags">'.__('','pandathemes').' '.$tag.'</div>';
					$out .= '<hr/>';
					echo $out;
				}
				
				// AFTER TITLE
				if ($theme_options['after_title']=='enable') { echo '<div id="after_title" style="width:250px; float:left; overflow:hidden;">'.do_shortcode($theme_options['after_title_data']).'</div><div class="clear h10"><!-- --></div>'; }

				// EXCERPT
				if ($theme_options['excerpt']=='enable') { $out = $post->post_excerpt ? '<div class="clear"><!-- --></div><p class="f120 excerpt">' . $post->post_excerpt . '</p>' : ''; echo $out; }

				// BEFORE POST
				if ($theme_options['before_post']=='enable') { echo '<div id="before_post">'.do_shortcode($theme_options['before_post_data']).'</div>'; }

				// CONTENT
				the_content();

				// AFTER POST
				if ($theme_options['after_post']=='enable') { echo '<div class="clear"><!-- --></div><div id="after_post">'.do_shortcode($theme_options['after_post_data']).'</div>'; }
				
				// PAGINATION
				wp_link_pages(array('before' => '<p><strong>Pages:</strong> ', 'after' => '</p>', 'next_or_number' => 'number'));

				// COMMENTS ETC.
				comments_template();

	endwhile;

		else : echo '<div class="contentbox w620"><h2>404</h2><p>'.__('Sorry, no posts matched your criteria.','pandathemes').'</p>';

	endif; ?>

	</div> <!-- end_of_contentbox -->

		<?php
			// SIDEBAR
			if ($sidebar == 'Buddy Sidebar') :
				include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
			else :
				include(TEMPLATEPATH.'/inc/sidebar.php');
			endif;

	get_footer();

?>