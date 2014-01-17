<?php

	get_header();

	$template_url = get_bloginfo('template_url');
	
	// GET A PRODUCT SETTINGS
	$after_product = $panda_pr['after_product_data'];
	$reviews = $panda_pr['reviews'];
	$related_products = $panda_pr['related_products'];


		if ( have_posts() ) : while ( have_posts() ) : the_post();
		
		
			// GET CUSTOM DATA
			$custom = get_post_custom($post->ID);  
		
			$price = $custom['price'][0];
			$price_old = $custom['price_old'][0];
		
			$demo_url = $custom['demo_url'][0];
			$label = $custom['label'][0];
		
			$button_current = $custom['button_current'][0];
			$button = $custom['button'][0];
			$button_tagline = $custom['button_tagline'][0];
			$button_url = $custom['button_url'][0];
			$direct_url = $custom['direct_url'][0];
			$custom_button = $custom['custom_button'][0];
		
			$before_button = $custom['before_button'][0];
			$after_button = $custom['after_button'][0];

			$after_purchase = $panda_pr['after_purchase'];
			$after_purchase_data = $panda_pr['after_purchase_data'];

			$demo_button = ($custom['demo_button'][0]) ? ($custom['demo_button'][0]) : __('Live demo','pandathemes');
			$demo_button_tagline = $custom['demo_button_tagline'][0];
		
			$desc = $custom['desc'][0];

			$custom_tab_title = $custom['custom_tab_title'][0];
			$custom_tab_content = $custom['custom_tab_content'][0];

			if ($direct_url == 'yes') :

				$redirect = $target = '';

			else :

				$redirect = $template_url.'/go.php?';
				$target = ' target="_blank"';

			endif; ?>


			<div id="content">

	
				<?php

					// A LIST OF CUSTOM CATEGORIES
					$terms = get_terms('catalog');
		
					$count = count($terms);
		
					if ($count > 1) :
		
						$out = '<ul class="no relative ctabs">';
		
							// Catalog page (this page)
							$pges = get_pages(array(
								'meta_key'		=> '_wp_page_template',
								'meta_value'	=> 'products.php'
							));
		
							foreach($pges as $p) :
								$out .= '<li><a href="' . get_permalink( $p->ID ) . '"><span><!-- --></span>' . __( 'All','pandathemes' ) . '</a></li>';
							endforeach;

							// Custom categories
							$cats = get_the_terms( $post->ID, 'catalog' );

							foreach ($cats as $cat) :
								$current = $cat->slug;
							endforeach;

							foreach ($terms as $term) :
								$out .= ($current == $term->slug) ?
									'<li><a href="' . get_term_link($term->slug, 'catalog') . '" class="current"><span><!-- --></span>' . $term->name . '</a></li>' :
									'<li><a href="' . get_term_link($term->slug, 'catalog') . '"><span><!-- --></span>' . $term->name . '</a></li>';
							endforeach;
		
						$out .= '</ul>';
		
						echo $out;
		
					endif;

				?>


				<div class="contentbox wfull">


					<!-- C O M M O N -->
			
					<h2><?php the_title(); edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span> ' ); ?></h2>

					<?php
						// REVIEWS
						if ($reviews == 'yes') :
		
							if ( get_average_ratings($post->ID) && get_average_ratings($post->ID) != '0' ) :
		
								echo '
									<div style="margin:-8px 0 8px -3px;">
										<span id="rating-total" class="h20 inline-block" style="background-position:0 -'.get_average_ratings($post->ID).'px;">&nbsp;</span>
										<a id="reviews-total" class="ntd f11 gray" href="#tab-reviews">'.$panda_pr['tab_rews'].':&nbsp;</a>
									</div>
								';

							else :

								echo '<div class="mt-10"><!-- --></div>';

							endif;

						endif;

						// SHORT DESCRIPTION
						if ($desc) : echo '<div class="one_half"><p>'.$desc.'</p><div class="clear h10"></div></div>'; endif;
					?>

					<div id="pr-holder" class="fl w600">

						<?php
							
							// FIRST IMAGE
							$src = ($custom['i1'][0]) ? $custom['i1'][0] : $template_url.'/images/no_photo.gif';

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
							if ($src) { $out = '<img id="product-first-image" class="br3 block mb25" src="'.$path.'" width="600" alt="'.get_the_title().'" />'; echo $out; }
		
							// METAS
							include(TEMPLATEPATH.'/inc/products/metas.php');
							
						?>

						<div class="clear h5"><!-- --></div>

					</div><!-- end_of_w600 -->


					<!-- S I D E B A R -->

					<?php

						$out = '';


						// LIVE DEMO
						if ($demo_url) :

							$out .= '<div class="fr br3 relative mb25 pr-bar">';
	
								$out .= '<h3>'.$demo_button.'</h3>';

								$out .= '<div class="h20"><!-- --></div>';
	
								$out .= '<a href="' . $redirect . $demo_url . '" class="bbutton2" target="_blank"><div class="lh13 bb2a"><span class="f18 bb2b">' . $demo_button . '</span><span class="f11 block bb2c">' . $demo_button_tagline . '</span></div></a>';

								$out .= '<div class="h20"><!-- --></div>';

							$out .= '</div>';

						endif;


						// PURCHASE
						$out .= '<div class="fr br3 relative mb25 pr-bar">';
						
		
							// Price & Purchase button
							if ($price || $button_url || $button_current == 'custom' || $after_button) :
	
	
								$out .= '<div class="t2-price-holder">';
		
		
									// Price
									if ($price) :
			
										$out .= '<span class="a">'.$price.'</span>';
					
										if ($price_old) : $out .= '&nbsp; <span class="b"><strike>'.$price_old.'</strike></span>'; endif;
		
										$out .= '<div class="clear h17"><!-- --></div>';
		
									else :
		
										$out .= '<div class="clear h5"><!-- --></div>';
		
									endif;
									
	
									// Before button
									$out .= $before_button ? '<div class="clear mt-10"><!-- --></div><div id="before-button">'.do_shortcode($before_button).'</div><div class="clear h17"><!-- --></div>' : '';


									// Purchase button
									if ($button_current == 'custom') : 
					
										if ($custom_button) : $out .= $custom_button.'<div class="clear h10"><!-- --></div>'; endif;
					
									 else :
	
										if ($button_url) :
	
											$tagline = $button_tagline ? $button_tagline : get_the_title();
					
											$out .= '<a href="'.$redirect.$button_url.'" class="bbutton2"'.$target.'><div class="lh13 bb2a"><span class="f18 bb2b">'.$button.'</span><span class="f11 block bb2c">'.$tagline.'</span></div></a>';
		
										endif;
					
									endif;
	
	
									// After button
									$out .= $after_button ? '<div class="clear h17"><!-- --></div><div id="after-button">'.do_shortcode($after_button).'</div>' : '<div class="clear h5"><!-- --></div>';
	
		
								$out .= '</div>';
	
							else :

								$out .= '<div class="mb1e"><!-- --></div>';

							endif;


							// AFTER PURCHASE SECTION
							$out .= ($after_purchase == 'enable') ? '<div id="after-purchase">' . do_shortcode($after_purchase_data) . '</div><div class="clear"><!-- --></div><div class="mb1e"><!-- --></div>' : '<div class="mt-1e"><!-- --></div>';

	
						$out .= '</div>';


						echo $out;

					?>

	
				</div><!-- end_of_contentbox -->

	
				<?php
				// AFTER PRODUCT
				if ($panda_pr['after_product']=='enable' && $after_product) { echo '<div id="after_product">'.do_shortcode($after_product).'</div>'; }
	
				// RELATED PRODUCTS
				if ($related_products == 'yes') : include(TEMPLATEPATH.'/inc/products/related.php'); endif;
		
		
		endwhile; endif;


	get_footer();

?>