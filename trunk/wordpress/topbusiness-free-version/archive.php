<?php get_header(); ?>

	<div id="wrapper">

		<div id="content">

			<?php

				if ( have_posts() ) :



					// ********************************************
					//
					// 	D E F A U L T   P O S T S
					//
					// ********************************************

					if ( get_post_type() == 'post' ) : ?>

						<div class="contentbox w620">
		
							<?php
								// BREADCRUMBS
								if ( !is_front_page() && function_exists( 'breadcrumb_trail' ) ) {
									breadcrumb_trail(	array(
										'before'		=> __('Browse:','pandathemes'),
										'show_home'		=> __('Home','pandathemes'),
										'front_page'	=> true,
										'separator'		=> '>'
									));
								}
							?>
		
							<div class="clear h5"><!-- --></div>

							<?php

								// DATE

								if ( is_day() || is_month() || is_year() ) :

									// Get the date
									if ( is_day() ) :
			
										$date = __( 'Daily archives', 'pandathemes' ) . ': ' . get_the_date();
										$y = get_the_date('Y');
										$n = get_the_date('n');
										$j = get_the_date('j');

										$qty = get_posts( 'year=' . $y . '&monthnum=' . $n . '&day=' . $j );
										$qty = count($qty);

									elseif ( is_month() ) :
			
										$date = __( 'Monthly archives', 'pandathemes' ) . ': ' . get_the_date( _x( 'F Y', 'monthly archives date format', 'pandathemes' ) );
										$y = get_the_date('Y');
										$n = get_the_date('n');

										$qty = get_posts( 'year=' . $y . '&monthnum=' . $n );
										$qty = count($qty);

									elseif ( is_year() ) :
			
										$date = __( 'Yearly archives', 'pandathemes' ) . ': ' . get_the_date( _x( 'Y', 'yearly archives date format', 'pandathemes' ) );
										$y = get_the_date('Y');
										$qty = 0;

											$arr = array(1,2,3,4,5,6,7,8,9,10,11,12);

											foreach ($arr as $value) :

												$a = count(get_posts( 'year=' . $y . '&monthnum=' . $value ));
												$qty = $qty + $a;

											endforeach;

									endif;

									$out = '<blockquote>';

										$out .= '<p class="f120 fl">' . $date . '</p>';

										$out .= '<div class="clear"><!-- --></div>';	

										$out .= '<span class="f11">' . __('Entries found','pandathemes') . ':&nbsp;' . $qty . '</span>';

									$out .= '</blockquote>';



								// TAG

								elseif ( is_tag() ) :

									// Get number of posts by tag
									$the_term = get_term_by( 'name', single_tag_title('', false), 'post_tag' );

									$out = '<blockquote>';

										$out .= '<p class="f120 fl">' . __('Search by tag','pandathemes') . ' &laquo;' . single_tag_title('', false) . '&raquo;</p>';

										$out .= '<div class="clear"><!-- --></div>';

										$out .= '<span class="f11">' . __('Entries found','pandathemes') . ':&nbsp;' . $the_term->count . '</span>';

									$out .= '</blockquote>';



								// CATEGORY

								elseif ( is_category() ) :

									$out = '<blockquote>';

										$category = get_category( get_query_var( 'cat' ) );

										$out .= '<p class="f120 fl">' . $category->cat_name . '</p>';

										$out .= '<span class="icon16 fr" style="margin:3px 0 0 0; background-image:url(' . get_bloginfo('template_url') . '/images/icons/16/led-icons/feed_document.png) !important;"><a class="tooltip-t" style="color:#ff7700;" href="' . get_bloginfo('home') . '/?cat=' . $category->cat_ID . '&feed=rss2" title="' . __('Subscribe to this category','pandathemes') . '">RSS</a></span>';

										$out .= '<div class="clear"><!-- --></div>';

										$out .= '<span class="f11">' . __('Posts','pandathemes') . ':&nbsp;' . $category->category_count . '</span>';

										$out .= '<div class="h5"><!-- --></div><em>' . category_description() . '</em>';

									$out .= '</blockquote>';



								// ABOUT AUTHOR

								elseif ( is_author() ) :

									the_post();

										$out = '<blockquote>';

											// Userpic
											if ( $panda_bp['compatibility']=='yes' ) :

												$member_id = get_the_author_meta('ID');
												$out .= '<span class="alignright">' . bp_core_fetch_avatar ( array( 'item_id' => $member_id, 'type' => 'full' ) ) . '</span>';

											else :

												$out .= '<span class="alignright">' . get_avatar(get_the_author_meta('user_email'),'110') . '</span>';												

											endif;


											// Nickname
											$out .= '<span style="background-image:url(' . get_bloginfo('template_url') . '/images/icons/16/led-icons/user.png) !important;" class="icon16">' . get_the_author_meta('nickname') . '</span>';


											$out .= '<div class="h10"><!-- --></div>';


											$out .= '<p>';

												// Name
												$out .= '<span class="f120">' . get_the_author_meta('first_name') . '&nbsp;' . get_the_author_meta('last_name') . ' &nbsp; </span><br/>';

												// Website
												$out .= get_the_author_meta('user_url') ? '<a target="_blank" href="' . get_bloginfo('template_url') . '/go.php?' . get_the_author_meta('user_url') . '">' . get_the_author_meta('user_url') . '</a>' : '';

											$out .= '</p>';


											$out .= '<div class="h10"><!-- --></div>';


											$register_date = date("F j, Y", strtotime( get_the_author_meta('user_registered' )));


											$out .= '<span class="f11">' . __('Created on','pandathemes') . '&nbsp;' . $register_date . '<br/>' . __('Posts written','pandathemes') . ':&nbsp;' . get_the_author_posts() . '</span>';


											$out .= '<div class="clear h5"><!-- --></div>';


											// Author description
											$out .= '<p><em>' . get_the_author_meta('description') . '</em></p>';


										$out .= '</blockquote>';

										$out .= '<h5>' . __('All posts written by','pandathemes') . ' ' . get_the_author_meta('nickname') . '</h5>';

										$out .= '<div class="line"><!-- --></div><div class="h25"><!-- --></div>';

									rewind_posts();



								endif;

								echo $out;	

							?>

							<div id="archive">
		
								<?php 

									while ( have_posts() ) : the_post();
		
										// POSTS TEMPLATES
										$t = $theme_options['blog_template'];

										if ( (!($t)) || ( $t == 'default' ) ) :

											include(TEMPLATEPATH.'/inc/posts/default.php');

										else :

											include(TEMPLATEPATH.'/inc/posts/'.$t.'.php');

										endif;
		
									endwhile;
		
									// PAGINATION
									if (function_exists('wp_pagenavi')) { ?><div id="wp-pagenavibox"><?php wp_pagenavi(); ?></div><?php } 
									else { ?><div id="but-prev-next"><?php next_posts_link( __('Older Entries','pandathemes') ); previous_posts_link( __('Newer Entries','pandathemes') ); ?></div><?php } 

								?>
		
								<div class="clear h30"><!-- --></div>
		
							</div><!-- end archive -->
		
						</div><!-- end contentbox -->
		
						<?php
		
							// SIDEBAR
							include(TEMPLATEPATH.'/inc/sidebar.php');



					// ********************************************
					//
					// 	P R O D U C T S
					//
					// ********************************************
	
					elseif ( get_post_type() == 'product' ) :


						// A LIST OF CUSTOM CATEGORIES
						$terms = get_terms('catalog');
			
						$count = count($terms);
			
						if ($count > 1) :
			
							$out = '<ul class="no relative ctabs">';
			
								// Catalog page (this page)
								$pages = get_pages(array(
									'meta_key'		=> '_wp_page_template',
									'meta_value'	=> 'products.php'
								));
			
								foreach($pages as $page) :
									$out .= '<li><a href="' . get_permalink( $page->ID ) . '"><span><!-- --></span>' . __( 'All','pandathemes' ) . '</a></li>';
								endforeach;

								// Custom categories
								$current = get_queried_object()->slug;
								foreach ($terms as $term) :
									$out .= ($current == $term->slug) ?
										'<li><a href="' . get_term_link($term->slug, 'catalog') . '" class="current"><span><!-- --></span>' . $term->name . '</a></li>' :
										'<li><a href="' . get_term_link($term->slug, 'catalog') . '"><span><!-- --></span>' . $term->name . '</a></li>';
								endforeach;
			
							$out .= '</ul>';
			
							echo $out;
			
						endif; ?>


						<div class="contentbox wfull">

							<h2><?php echo get_queried_object()->name; ?></h2>

							<?php

								// BREADCRUMBS
								echo '<div class="mt-10 pb25">';
								if ( !is_front_page() && function_exists( 'breadcrumb_trail' ) ) {
									breadcrumb_trail(	array(
										'before'		=> __('Browse:','pandathemes'),
										'show_home'		=> __('Home','pandathemes'),
										'front_page'	=> true,
										'separator'		=> '>'
									));
								}
								echo '</div>';

								// Description
								$out = get_queried_object()->description;
								$out = ($out) ? '<div class="gray-contentbox"><div class="h25"><!-- --></div>' . do_shortcode($out) . '<div class="h25"><!-- --></div></div><div class="clear h25"><!-- --></div>' : '';
			
								echo $out;

							?>

							<div id="archive">

								<?php 

									// P R O D U C T S   A R C H I V E


										// DEFINE THE CATALOG
										$terms = get_the_terms( $post->ID, 'catalog' );
											$draught_links = array();
												foreach ($terms as $term) {
													$draught_links[] = $term->slug;
												}
											$on_draught = join( ", ", $draught_links );

										// GET DATA
										$reviews = $panda_pr['reviews'];
										$t = $panda_pr['product_template'];

										// GET POSTS
										$paged = (get_query_var('paged')) ? get_query_var('paged') : 1;
										$posts_per_page = $panda_pr['posts_per_page'];

										$args = array(
											'post_type'			=> 'product',
											'posts_per_page'	=> $posts_per_page,
											'order'				=> 'DESC',
											'orderby'			=> 'menu_order',
											'paged'				=> $paged,
											'post_status'		=> 'publish',
											'catalog'			=> $on_draught
										);
					
										$temp = $wp_query;
										$wp_query = null;
										$wp_query = new WP_Query($args);

											// PRODUCT TEMPLATES
											if ( (!($t)) || ( $t == 't1' ) ) :

												include(TEMPLATEPATH.'/inc/products/t1.php');

											else :

												include(TEMPLATEPATH.'/inc/products/'.$t.'.php');

											endif;
											
											// PAGINATION
											if(function_exists('wp_pagenavi')) { ?><div id="wp-pagenavibox"><?php wp_pagenavi(); ?></div><?php } 
											else { ?><div id="but-prev-next"><?php next_posts_link( __('Older Entries','pandathemes') ); previous_posts_link( __('Newer Entries','pandathemes') ); ?></div><?php }

										$wp_query = null;
										$wp_query = $temp;
										wp_reset_query();

								?>

							</div><!-- end archive -->

						</div><!-- end contentbox -->
		
						<?php
				
					endif;



				else : ?>

					<div class="contentbox w620">
						<div class="clear h10"><!-- --></div>
						<div id="archive">
							<?php _e('Sorry, no posts matched your criteria.','pandathemes') ?>
						</div><!-- end archive -->
					</div><!-- end contentbox -->
	
					<?php
	
						// SIDEBAR
						include(TEMPLATEPATH.'/inc/sidebar.php');

				endif;


get_footer(); ?>