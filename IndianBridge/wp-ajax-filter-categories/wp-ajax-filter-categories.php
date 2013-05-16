<?php
/*
Plugin Name: Wordpress Ajax Filter Categories
Plugin URI: http://bfi.net.in
Description: A plugin to show posts filtered by categories using ajax
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'Wordpress_Ajax_Filter_Categories' ) ) {
	

	class Wordpress_Ajax_Filter_Categories {
		
		public function __construct() {
			// register the javascript for the ajax function
			wp_register_script( 'wp_ajax_filter_categories', plugins_url( 'wp-ajax-filter-categories.js', __FILE__ ), array( 'jquery' ) );	
			wp_enqueue_script('wp_ajax_filter_categories');
			
			// Add the action for the ajax call
			add_action( 'wp_ajax_nopriv_load-filter', array($this, 'prefix_load_cat_posts') );
			add_action( 'wp_ajax_load-filter', array($this, 'prefix_load_cat_posts') );
			
			// Add the shortcode
			add_shortcode('wp_ajax_filter_categories', array($this, 'wp_ajax_filter_categories'));
		}
		
		/**
		 * Replace shortcode with posts
		 */
		function wp_ajax_filter_categories ($atts, $content = null ) {
			// Extract shortcode attributes
			extract(shortcode_atts(array(
				'cat'		=> ''
			), $atts));
			$categories = explode(',',$cat);
			$ajaxURL = admin_url ('admin-ajax.php');
			$loadingImageURL = admin_url ( 'images/loading.gif' );
			$shortcode_atts = json_encode($atts);
			$tabIDPrefix = basename(get_permalink()).'-tab';
			$contentIDPrefix = basename(get_permalink()).'-content';
			?>
			<script type="text/javascript">
				function callSwitchTabs(catID) {
					wp_ajax_filter_categories.switchTabs(catID,'<?php echo $tabIDPrefix; ?>','<?php echo $contentIDPrefix; ?>');
				}
				function callLoadPosts(catID) {
					wp_ajax_filter_categories.loadPostsByCategory(catID,'<?php echo $contentIDPrefix; ?>','<?php echo $loadingImageURL ?>','<?php echo $ajaxURL ?>',<?php echo $shortcode_atts; ?>);
				}
				function call_ajax_filter(catID) {
					wp_ajax_filter_categories.loadPostsByCategory(catID,'<?php echo $ajaxurl; ?>',<?php echo $shortcode_atts; ?>);
				}
			</script>
			<div class="tabber-widget-default">
				<ul class="tabber-widget-tabs">
					<li><a id="<?php echo $tabIDPrefix; ?>" onclick="callSwitchTabs('');" href="#">All Categories</a></li>			
					<?php foreach ( $categories as $catID ) { ?>
						<li><a id="<?php echo $tabIDPrefix.$catID; ?>" onclick="callSwitchTabs('<?php echo $catID; ?>');" href="#"><?php echo get_cat_name($catID) ; ?></a></li>
					<?php } ?>
				</ul>
				<div class="tabber-widget-content">
					<div class="tabber-widget">
						<div id="<?php echo $contentIDPrefix; ?>" style="display:none;"></div>
						<?php foreach ( $categories as $catID ) { ?>
							<div id="<?php echo $contentIDPrefix.$catID; ?>" style="display:none;"></div>
						<?php } ?>						
					</div>
				</div>
			</div>
			<script type="text/javascript">
				callSwitchTabs('');
				callLoadPosts('');
				<?php foreach ( $categories as $catID ) { ?>
					callLoadPosts('<?php echo $catID; ?>');
				<?php } ?>
			</script> 			
			<?php			
		}	

				
		/**
		 * Load the posts for this category via ajax
		 */
		function prefix_load_cat_posts () {
			$atts = $_POST['atts'];
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
				'date'		=> 'no',
				'author'    => 'no',
				'postedin'	=> 'no',
				'tags'		=> 'no',
				'comments'	=> 'no',
				'excerpt'	=> 'yes',
				't'			=> 'none',
				'tw'		=> '100',
				'th'		=> '100',
				'crop'		=> '',
				'offset'	=> '0',
				'button'	=> 'Read More',
				'resp'		=> ''
			), $atts));		
			$cat		= $_POST['cat'];
			// Theme settings
			global $theme_options;

			// Data
			$template_url = get_bloginfo('template_url');
			$margin = ($m) ? ' style="margin:' . $m . '"' : '';
			$wWrap = ($w) ? ' style="width:' . $w . ';"' : '';
			if ( $align == 'left' ) : $alignWrap = ' fl';
			elseif ( $align == 'right' ) : $alignWrap = ' fr';
			elseif ( $align == 'center' ) : $alignWrap = ' fc';
			else : $alignWrap = '';
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
			$response = '<div class="block relative' . $gridAuto . $alignWrap . '"' . $wWrap . '>';

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
					$response .= '<div'.$width.' class="posts-post' . $class . $resp . '"><div'.$margin.$last.'>';
					// Image
					if ($t != 'none') :
						$c = ($crop == 'top') ? '&a=t' : '';
						if ($t == 'left') : $a = ' fl mfl';
						elseif ($t == 'right') : $a = ' fr mfr';
						elseif ($t == 'center') : $a = ' fc1e block';
						else : $a = '';
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
						if ($src) : $response .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.$path.'&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
						else : $response .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
						endif;
					endif;

					// Title
					if ($title == 'yes') :
						$edit = '';
						if (current_user_can('manage_options')) : 
							$edit = ' - <a href="'.get_edit_post_link($post->ID).'"><small>'.__('Edit','pandathemes').'</small></a>'; 
						endif;
						$response .= '<'.$titletag.' class="lh13"><a href="'.get_permalink($post->ID).'">'.get_the_title($post->ID).'</a>'.$edit.'</'.$titletag.'>';
					endif;
								// Date
								if ($date == 'yes') :
									$response .= '<div class="f11 ico tim" title="Post Date"><em>'.get_the_time( get_option('date_format'), $post->ID ).'</em></div>';
								endif;

								// author
								if ($author == 'yes') :
									$response .= '<div><!-- divider --></div>';
									$author_id = $post->post_author;
									$response .= '<div class="f11" title="Author"><em>'.get_avatar( $author_id,'16' ).' <a href="'.get_author_posts_url( $author_id ).'">'.get_the_author_meta( 'display_name',$author_id).'</a>'.'</em></div>';
								endif;

								// Posted in
								if ($postedin == 'yes') :
									$response .= '<div><!-- divider --></div>';
									$response .= '<div class="f11 ico cat" title="Categories">'.__('','pandathemes').' '.get_the_category_list(', ','', $post_id = $post->ID).'</div>';

								endif;

								// Comments
								if ($comments == 'yes' && get_comments_number($post->ID) != 0) :
									$response .= '<div><!-- divider --></div>';
									$response .= '<div class="f11 ico com" title="Comments">'.__('','pandathemes').' <a href="'.get_comments_link($post->ID).'">'.get_comments_number($post->ID).'</a></div>';

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
									$response .= '<div><!-- divider --></div>';
									$response .= '<div class="f11 ico tag" title="Tags">'.__('','pandathemes').' '.$tag.'</div>';
								endif;

								// Excerpt
								if ($excerpt == 'yes') :
									$response .= ($post->post_excerpt) ? '<p>'.$post->post_excerpt.'</p>' : '<p>'.get_the_excerpt($post->ID).'</p>';
									if ($button != 'no') :
										$response .= '<a href="'.get_permalink($post->ID).'" class="button"><span>'.$button.'</span></a>';
									endif;
								endif;

								$response .= '<div class="clear"><!-- --></div></div></div>'.$clear;
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
					$response .= '<div'.$width.' class="posts-post'.$class.'"><div'.$margin.$last.'>';
					
					// Image
					if ($t != 'none') : 
						$c = ($crop == 'top') ? '&a=t' : '';
						if ($t == 'left') :	$a = ' fl mfl';
						elseif ($t == 'right') : $a = ' fr mfr';
						elseif ($t == 'center') : $a = ' fc1e block';
						else : $a = '';
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
						if ($src) : 
							$response .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.$path.'&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
						else : 
							$response .= '<a class="'.$a.'" href="'.get_permalink($post->ID).'" style="width:'.$tw.'px; height:'.$th.'px;"><img src="'.$template_url.'/timthumb.php?src='.get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg&w='.$tw.'&h='.$th.'&zc=3&q=90'.$c.'" alt="'.get_the_title($post->ID).'" /></a>'; 
						endif;
					endif;

					// Title
					if ($title == 'yes') :
						if (current_user_can('manage_options')) : 
							$edit = ' - <a href="'.get_edit_post_link($post->ID).'"><small>'.__('Edit','pandathemes').'</small></a>'; 
						endif;
						$response .= '<'.$titletag.' class="lh13"><a href="'.get_permalink($post->ID).'">'.get_the_title($post->ID).'</a>'.$edit.'</'.$titletag.'>';
					endif;

					// Date
					if ($date == 'yes') :
						$response .= '<div class="f11 ico tim" title="Post Date"><em>'.get_the_time( get_option('date_format'), $post->ID ).'</em></div>';
					endif;

					// author
					if ($author == 'yes') :
						$response .= '<div><!-- divider --></div>';
						$author_id = $post->post_author;
						$response .= '<div class="f11" title="Author"><em>'.get_avatar( $author_id,'16' ).' <a href="'.get_author_posts_url( $author_id ).'">'.get_the_author_meta( 'display_name',$author_id).'</a>'.'</em></div>';
					endif;

					// Posted in
					if ($postedin == 'yes') :
						$response .= '<div><!-- divider --></div>';
						$response .= '<div class="f11 ico cat" title="Categories">'.__('','pandathemes').' '.get_the_category_list(', ','', $post_id = $post->ID).'</div>';
					endif;

					// Comments
					if ($comments == 'yes' && get_comments_number($post->ID) != 0) :
						$response .= '<div><!-- divider --></div>';
						$response .= '<div class="f11 ico com" title="Comments>'.__('','pandathemes').' <a href="'.get_comments_link($post->ID).'">'.get_comments_number($post->ID).'</a></div>';
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
						$response .= '<div><!-- divider --></div>';
						$response .= '<div class="f11 ico tag" title="Tags">'.__('','pandathemes').' '.$tag.'</div>';
					endif;

					// Excerpt
					if ($excerpt == 'yes') :
						$response .= ($post->post_excerpt) ? '<p>'.$post->post_excerpt.'</p>' : '<p>'.get_the_excerpt($post->ID).'</p>';
						if ($button != 'no') :
							$response .= '<a href="'.get_permalink($post->ID).'" class="button"><span>'.$button.'</span></a>';
						endif;
					endif;

					$response .= '</div></div>'.$clear;
				endwhile; 
				wp_reset_query();
			endif;
			$response .= '<div class="clear"><!-- --></div></div>';
			echo $response; 		
			die(1);
		}		
		
		
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['wp_ajax_filter_categories'] = new Wordpress_Ajax_Filter_Categories();
}