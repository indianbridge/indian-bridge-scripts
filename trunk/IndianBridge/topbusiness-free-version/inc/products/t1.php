<?php

	// P R O D U C T S   A R C H I V E

		$counter = 0;

		echo '<ul class="no prdcts col4">';

		if ( $wp_query->have_posts() ) : while ( $wp_query->have_posts() ) : $wp_query->the_post();   

			// GET CUSTOM DATA
			$custom = get_post_custom($post->ID);

			$price = $custom['price'][0];
			$price_old = $custom['price_old'][0];

			$demo_button = ($custom['demo_button'][0]) ? ($custom['demo_button'][0]) : __('Live demo','pandathemes');
			$demo_url = $custom['demo_url'][0];
			$label = $custom['label'][0];

			$desc = $custom['desc'][0];

			$src = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'Full Size');
			$src = $src[0];
			
			$counter++;


			// DISPLAY ITEM


				$out = '';
	
				if ($counter == 4) : $counter = 0; $out .= '<li class="last">';
				else : $out .= '<li>'; endif;


					// Title
					$out .= '<h3><a href="'.get_permalink().'">'.get_the_title().'</a></h3>';


					// Label
					if ($label) : $out .= '<span class="lbl absolute f11 lh10 t1-lbl">'.$label.'</span>'; endif;

					
					// IMAGE

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
						$prettyDesc = ($desc) ? '&lt;span class=&quot;block pb10&quot;> ' . $desc . ' &lt;/span>' : '';
						if ($src) : $out .= '<a class="hidetitle t1-thumb" rel="prettyPhoto[gallery]" title="'.$prettyDesc.' &lt;a class=&quot;button&quot; href=&quot;'.get_permalink().'&quot;>'.__('Learn more','pandathemes').'&lt;/a>" href="'.$path.'"><div><!-- hover --></div><img src="'.$template_url.'/timthumb.php?src='.$path.'&h=211&w=211&zc=1&q=90&a=t" width="211" height="211" alt="'.get_the_title().'" /></a>'; endif;


					// Product metas
					$out .= '<div class="prdcts-excerpt">';


						// Catalogs
						$out .= '<p>';
	
							$count = 1;
							
							$terms = wp_get_post_terms( $post->ID, 'catalog');
							
							foreach ($terms as $term) :
								$comma = ($count == 1) ? '' : ', '; $count++;
								$out .= $comma . '<a href="' . get_term_link($term->slug, 'catalog') . '">' . $term->name . '</a>';
							endforeach;
						
						$out .= '</p>';


						// Short description
						$out .= '<p class="t1-desc">'.$desc.'</p>';
	

						// Price & Rating
						$out .= '<p>';
	
							if ($price) : $out .= '<span class="f16 dark">' . $price . '</span>'; endif;
							if ($price_old) : $out .= '&nbsp; <span class="f16 lgray"><strike>' . $price_old . '</strike></span>'; endif;
	
							if ($reviews == 'yes') :
								$out .= (get_average_ratings($post->ID)) ? '&nbsp;<span id="rating-total" class="h20 inline-block fr" style="margin:1px 0 0 0; background-position:0 -'.get_average_ratings($post->ID).'px;">&nbsp;</span>' : '';
							endif;
	
						$out .= '</p>';
	
					$out .= '<div class="clear"><!-- --></div></div>';

	
					// Buttons
					$out .= '<div class="t1-button-holder">';
						if ($demo_url) : $out .= '<a href="' . $template_url . '/go.php?' . $demo_url . '" class="t1-livedemo" target="_blank"><span>' . $demo_button . '</span></a>'; endif;
						$w = ($demo_url) ? '' : ' w100i';
						$out .= '<a href="' . get_permalink() . '" class="t1-details' . $w . '"><span>' . __('Learn more','pandathemes') . '</span></a>';
					$out .= '<div class="clear"><!-- --></div></div>';

	
				$out .= '</li>';


			echo $out;


		endwhile;
		endif;

		echo '</ul><div class="clear"><!-- --></div>';

?>