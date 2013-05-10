<?php // by PandaThemes.com


// ***********************************
//
// 	P O S T S  #2
//
// ***********************************


	function posts2( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'w'				=> '560',
			'cat'			=> '',
			'tags'			=> '',
			'author'		=> '',
			'comments'		=> '',
			'qty'			=> '3',
			'orderby'		=> 'date',
			'order'			=> 'DESC',
			'titletag'		=> 'h4',
			'crop'			=> '',
			't'				=> 'left',
			'float'			=> '',
			'date'			=> '',
			'excerpt'		=> '',
			'button'		=> __('Read More','pandathemes'),
			'transition'	=> 'ptf',
			'autoplay'		=> '',
			'offset'		=> '0'
		), $atts));


		// Theme settings
		global $theme_options;


		// Data
		$template_url = get_bloginfo('template_url');


		// Float
		if ($float == 'left') : $float = ' fl';

		elseif ($float == 'right') : $float = ' fr';

		endif;


		// Thumb sizes
		$tw = $w / 2 - 10;
		$th = $tw;


		// Style
		$style = ' style="';

			// Width
			$style .= $w ? 'width:' . $w . 'px;' : 'width:100%;';

			// Margin
			if ( $float == 'right' ) :

				$style .= 'margin-left:20px';

			elseif ( $float == 'left' ) :

				$style .= 'margin-right:20px';

			endif;

		$style .= '"';


		// Autoplay
		$autoplay = $autoplay ? ' title="'.$autoplay.'000"' : '';


		$tabs = '';

		$out = '
			<div class="pslider posts2 ps-3 ' . $transition . $float . '"' . $style . $autoplay . '>
			
				<div class="ploading"><!-- --></div>
			
				<div class="pslides">';



			// R E C E N T   P O S T S

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

					$out .= '<div style="width:100%; height:' . $th . 'px;">';
			

						// Image
						$c = ($crop == 'top') ? '&a=t' : '';
	
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

							// Image float
							$if = ( $t == 'left' ) ? 'fl' : 'fr';

							// Display image
							if ($src) : $out .= '<a class="block posts2-img '.$if.'" href="'.get_permalink($post->ID).'"><img src="'.$template_url.'/timthumb.php?src='.$path.'&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>';

							// Else default
							//else : $out .= '<a class="block '.$if.'" href="'.get_permalink($post->ID).'"><img src="'.get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg" alt="no image"/></a>';
							else : $out .= '<a class="block posts2-img '.$if.'" href="'.get_permalink($post->ID).'"><img src="'.$template_url.'/timthumb.php?src='.get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="no image"/></a>';

							endif;



						// M e t a s

							// Metas float
							$mf = ( $t == 'left' ) ? 'fr' : 'fl';

							$out .= '<div class="posts2-meta ' . $mf . ' lh13" style="width:' . $tw . 'px">';


								// Categories
								$categories = get_the_category($post->ID);
	
								foreach($categories as $cat) :
	
									$out .= '<a class="feat-link f11 lh11 inline-block nowrap" href="' . get_category_link($cat->term_id ) . '">' . $cat->cat_name . '</a>';
	
								endforeach;
	
	
								// Title
								$out .= '<' . $titletag . '><a href="' . get_permalink($post->ID) . '">' . get_the_title($post->ID) . '</a></' . $titletag . '>';
	
	
								// Date
								//$out .= $date != 'no' ? '<em class="f11 ico tim" title="Post Date">' . get_the_time( get_option('date_format'), //$post->ID ) . '</em><div class="clear h10"><!-- divider --></div>' : '<div class="clear h5"><!-- divider --></div>';
								if ($date == 'yes') :
									$out .= '<em class="f11 ico tim" title="Post Date">' . get_the_time( get_option('date_format'), $post->ID ) . '</em><div class="clear h5"><!-- divider --></div>';

								endif;

								// author
								if ($author == 'yes') :
									$author_id = $post->post_author;
									$out .= '<em class="f11" title="Author">'.str_replace('<img','<img align="left" style="padding-right:5px;"',get_avatar( $author_id,'16' )).' <a href="'.get_author_posts_url( $author_id ).'">'.get_the_author_meta( 'display_name',$author_id).'</a>'.'</em><div class="clear h5"><!-- divider --></div>';

								endif;



								// Comments
								if ($comments == 'yes' && get_comments_number($post->ID) != 0) :
									$out .= '<em class="f11 ico com" title="Comments"> <a href="'.get_comments_link($post->ID).'">'.get_comments_number($post->ID).'</a></em><div class="clear h5"><!-- divider --></div>';

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
									$out .= '<em class="f11 ico tag" title="Tags"> '.$tag.'</em><div class="clear h5"><!-- divider --></div>';

								endif;	
	
								// Excerpt
								if ( $excerpt != 'no' ) :

									$out .= ($post->post_excerpt) ? '<p>'.$post->post_excerpt.'</p>' : '<p>'.get_the_excerpt($post->ID).'</p>';

								endif;

								// Button
								if ( $button != 'no' ) :

									$out .= '<a class="button" href="' . get_permalink($post->ID) . '"><span>' . $button . '</span></a>';

								endif;

	
	
							$out .= '<div class="clear h17"><!-- --></div></div>';
						
			
					$out .= '<div class="clear"><!-- --></div></div>';

				endforeach;



		$out .= '
				</div>
			
				<div class="pnavbar">
					<div class="pnav">
						<div>
							<div>' . $tabs . '</div>
						</div>
					</div>
				</div>
			
			</div>';



	return $out; }

	add_shortcode('posts2', 'posts2');



?>