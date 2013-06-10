<?php // by PandaThemes.com


// ***********************************
//
// 	P O S T S
//
// ***********************************


	function posts( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'w'			=> '',
			'm'			=> '',
			'align'		=> '',
			'grid'		=> '',
			'type'		=> 'recent',
			'cat'		=> '',
			'qty'		=> '3',
			'orderby'	=> 'date',
			'order'		=> 'DESC',
			'title'		=> 'yes',
			'titletag'	=> 'h5',
			'date'		=> '',
			'author'    => '',
			'postedin'	=> '',
			'tags'		=> '',
			'comments'	=> '',
			'excerpt'	=> '',
			't'			=> 'none',
			'tw'		=> '100',
			'th'		=> '100',
			'crop'		=> '',
			'offset'	=> '0',
			'button'	=> 'Read More',
			'resp'		=> ''
		), $atts));
		
		// Theme settings
		global $theme_options;

		// Data
		$template_url = get_bloginfo('template_url');

		$margin = ($m) ? ' style="margin:' . $m . '"' : '';

		$wWrap = ($w) ? ' style="width:' . $w . ';"' : '';

			if ( $align == 'left' ) :
	
				$alignWrap = ' fl';
	
			elseif ( $align == 'right' ) :
	
				$alignWrap = ' fr';
	
			elseif ( $align == 'center' ) :
	
				$alignWrap = ' fc';
	
			else :
	
				$alignWrap = '';
	
			endif;

		$count = 0;
		$last_count = 0;

		$clear = '';

		if ($grid) :

			$width = 100 / $grid;

			$width = ' style="width:' . floor($width) . '%;"';

			$gridAuto = ' grid-auto';

		else :

			$width = ' style="width:100%;"';

		endif;

		$resp = ($resp) ? ' no-resp' : '';


		// Wrap all posts
		$out = '<div class="block relative' . $gridAuto . $alignWrap . '"' . $wWrap . '>';




			// R E C E N T   P O S T S

			if ($type == 'recent') :

				$args = array(
					'numberposts'	=> $qty,
					'category'		=> $cat,
					'orderby'		=> $orderby,
					'order'			=> $order,
					'offset'		=> $offset
				);

				$recentposts = get_posts($args);



				foreach($recentposts as $post) :

					setup_postdata($post);


					if ($grid) :
	
						$class = ' grid-post fl';
	
						$count++;
	
						if ($count == $grid) :
	
							$count = 0;
	
							$clear = '<div class="clear"><!-- --></div>';

						else :

							$clear = '';
	
						endif;
	
					else :

						$last_count++;
						
						$last = ( $last_count == $qty ) ? ' class="posts-last"' : '';

					endif;


					// Wrap this post
					$out .= '

						<div'.$width.' class="posts-post' . $class . $resp . '">

							<div'.$margin.$last.'>';


								// Image
								if ($t != 'none') :

									$c = ($crop == 'top') ? '&a=t' : '';

									if ($t == 'left') :

										$a = ' fl mfl';

									elseif ($t == 'right') :

										$a = ' fr mfr';

									elseif ($t == 'center') :

										$a = ' fc1e block';

									else :

										$a = '';

									endif;
									$src = get_the_post_thumbnail($post->ID,'full');
									$src = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'Full Size');
									$src = ($src) ? $src[0] : $theme_options['img_placeholder'];

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
										}

										// Display image
										if ($src) : $out .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.$path.'&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
										else : $out .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
										endif;

								endif;



								// Title
								if ($title == 'yes') :

									$edit = '';

									if (current_user_can('manage_options')) : $edit = ' - <a href="'.get_edit_post_link($post->ID).'"><small>'.__('Edit','pandathemes').'</small></a>'; endif;

									$out .= '<'.$titletag.' class="lh13"><a href="'.get_permalink($post->ID).'">'.get_the_title($post->ID).'</a>'.$edit.'</'.$titletag.'>';

								endif;



								// Date
								if ($date == 'yes') :

									$out .= '<div class="f11 ico tim" title="Post Date"><em>'.get_the_time( get_option('date_format'), $post->ID ).'</em></div>';

								endif;

								// author
								if ($author == 'yes') :
									$out .= '<div><!-- divider --></div>';
									$author_id = $post->post_author;
									$out .= '<div class="f11" title="Author"><em>'.get_avatar( $author_id,'16' ).' <a href="'.get_author_posts_url( $author_id ).'">'.get_the_author_meta( 'display_name',$author_id).'</a>'.'</em></div>';

								endif;

								// Posted in
								if ($postedin == 'yes') :
									$out .= '<div><!-- divider --></div>';
									$out .= '<div class="f11 ico cat" title="Categories">'.__('','pandathemes').' '.get_the_category_list(', ','', $post_id = $post->ID).'</div>';

								endif;



								// Comments
								if ($comments == 'yes' && get_comments_number($post->ID) != 0) :
									$out .= '<div><!-- divider --></div>';
									$out .= '<div class="f11 ico com" title="Comments">'.__('','pandathemes').' <a href="'.get_comments_link($post->ID).'">'.get_comments_number($post->ID).'</a></div>';

								endif;



								// Tags
								if ($tags == 'yes') :

									$posttags = get_the_tags($post->ID);

										if ($posttags) :

											$tag = '';

											foreach($posttags as $tagg) {

												$tag .= '<a href="'.get_tag_link($tagg->term_id).'">'.$tagg->name.'</a>, ';

											}

										endif;
									$out .= '<div><!-- divider --></div>';
									$out .= '<div class="f11 ico tag" title="Tags">'.__('','pandathemes').' '.$tag.'</div>';

								endif;



								// Excerpt
								if ($excerpt == 'yes') :

									$out .= ($post->post_excerpt) ? '<p>'.$post->post_excerpt.'</p>' : '<p>'.get_the_excerpt($post->ID).'</p>';

									if ($button != 'no') :

										$out .= '<a href="'.get_permalink($post->ID).'" class="button"><span>'.$button.'</span></a>';

									endif;

								endif;



								$out .= '

								<div class="clear"><!-- --></div>

							</div>

						</div>

					'.$clear;



				endforeach;




			// R E L A T E D   P O S T S

			elseif ($type == 'related') :

				$tgs = wp_get_post_tags(get_the_ID());

				$tagsarray = array();

				foreach ($tgs as $tg) {
					$tagsarray[] = $tg->term_id;
				};

				$args = array(
					'showposts' 	=> $qty,
					'tag__in' 		=> $tagsarray,
					'post__not_in'	=> array(get_the_ID()),
					'orderby' 		=> $orderby
				);

				query_posts($args);

				if ($grid) :

					$class = ' grid-post fl';

					$count++;

					if ($count == $grid) :

						$count = 0;

						$clear = '<div class="clear"><!-- --></div>';

					else :

						$clear = '';

					endif;

				else :

					$last_count++;
					
					$last = ( $last_count == $qty ) ? ' class="posts-last"' : '';

				endif;

				global $post;

				while (have_posts()) : the_post();


					if (!$grid) :

						$last_count++;
						
						$last = ( $last_count == $qty ) ? ' class="posts-last"' : '';

					endif;


					// Wrap this post
					$out .= '

						<div'.$width.' class="posts-post'.$class.'">

							<div'.$margin.$last.'>';


								// Image
								if ($t != 'none') :

									$c = ($crop == 'top') ? '&a=t' : '';

									if ($t == 'left') :

										$a = ' fl mfl';

									elseif ($t == 'right') :

										$a = ' fr mfr';

									elseif ($t == 'center') :

										$a = ' fc1e block';

									else :

										$a = '';

									endif;

									$src = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'Full Size');
									$src = ($src) ? $src[0] : $theme_options['img_placeholder'];

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
										}

										// Display image
										if ($src) : $out .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.$path.'&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
										else : $out .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
										endif;

								endif;



								// Title
								if ($title == 'yes') :

									if (current_user_can('manage_options')) : $edit = ' - <a href="'.get_edit_post_link($post->ID).'"><small>'.__('Edit','pandathemes').'</small></a>'; endif;

									$out .= '<'.$titletag.' class="lh13"><a href="'.get_permalink($post->ID).'">'.get_the_title($post->ID).'</a>'.$edit.'</'.$titletag.'>';

								endif;



								// Date
								if ($date == 'yes') :

									$out .= '<div class="f11 ico tim" title="Post Date"><em>'.get_the_time( get_option('date_format'), $post->ID ).'</em></div>';

								endif;

								// author
								if ($author == 'yes') :
									$out .= '<div><!-- divider --></div>';
									$author_id = $post->post_author;
									$out .= '<div class="f11" title="Author"><em>'.get_avatar( $author_id,'16' ).' <a href="'.get_author_posts_url( $author_id ).'">'.get_the_author_meta( 'display_name',$author_id).'</a>'.'</em></div>';

								endif;

								// Posted in
								if ($postedin == 'yes') :
									$out .= '<div><!-- divider --></div>';
									$out .= '<div class="f11 ico cat" title="Categories">'.__('','pandathemes').' '.get_the_category_list(', ','', $post_id = $post->ID).'</div>';

								endif;



								// Comments
								if ($comments == 'yes' && get_comments_number($post->ID) != 0) :
									$out .= '<div><!-- divider --></div>';
									$out .= '<div class="f11 ico com" title="Comments>'.__('','pandathemes').' <a href="'.get_comments_link($post->ID).'">'.get_comments_number($post->ID).'</a></div>';

								endif;



								// Tags
								if ($tags == 'yes') :

									$posttags = get_the_tags($post->ID);

										if ($posttags) :

											$tag = '';

											foreach($posttags as $tagg) {

												$tag .= '<a href="'.get_tag_link($tagg->term_id).'">'.$tagg->name.'</a>, ';

											}

										endif;
									$out .= '<div><!-- divider --></div>';
									$out .= '<div class="f11 ico tag" title="Tags">'.__('','pandathemes').' '.$tag.'</div>';

								endif;



								// Excerpt
								if ($excerpt == 'yes') :

									$out .= ($post->post_excerpt) ? '<p>'.$post->post_excerpt.'</p>' : '<p>'.get_the_excerpt($post->ID).'</p>';

									if ($button != 'no') :

										$out .= '<a href="'.get_permalink($post->ID).'" class="button"><span>'.$button.'</span></a>';

									endif;

								endif;



								$out .= '

							</div>

						</div>

					'.$clear;


				endwhile; wp_reset_query();

			endif;




		$out .= '<div class="clear"><!-- --></div></div>';


	return $out; }

	add_shortcode('posts', 'posts');



?>