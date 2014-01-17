<?php
// PANDA - RECENT POSTS WIDGET v.2.5
// www.pandathemes.com
add_action('init', 'widget_recent_posts_multi_register');
function widget_recent_posts_multi_register() {
	
	$prefix = 'recent-posts-multi'; // $id prefix
	$name = 'Panda - Recent Posts v.2.5';
	$widget_ops = array(
		'classname'		=> 'widget_recent_posts_multi',
		'description'	=> __('Recent posts with thumbnails, dates, excerpts.','pandathemes')
		 );
	$control_ops = array(
		'width'			=> 400,
		'height'		=> 200,
		'id_base'		=> $prefix
		);
	
	$options = get_option('widget_recent_posts_multi');
	if(isset($options[0])) unset($options[0]);
	
	if ( !empty($options) ) {
		foreach(array_keys($options) as $widget_number) {
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_recent_posts_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_recent_posts_multi_control', $control_ops, array( 'number' => $widget_number ));
		}
	} else {
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_recent_posts_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_recent_posts_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_recent_posts_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('recent-posts-multi-', '', @$widget_id);
    $options = get_option('widget_recent_posts_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }
	$widget_title = $vars['widget_title'];
	$cats = $vars['cats'];
	$cats = preg_split("/[\s,]+/", $cats); // array
	$post_type = $vars['post_type'];
	$quantity = $vars['quantity'];
	$offset = $vars['offset'];
	$order = $vars['order'];
	$thumbs = $vars['thumbs'];
	$thumbs_float = $vars['thumbs_float'];
	$thumbs_crop = $vars['thumbs_crop'];
	$width = $vars['width'];
	$height = $vars['height'];
	$titles = $vars['titles'];
	$size = $vars['size'];
	$date = $vars['date'];
	$excerpt = $vars['excerpt'];

    // DISPALY WIDGET
	$out = '<div class="widget widget-recent">';

		if( ! empty($vars['widget_title'] ) ) : $out .= '<h5>'.$widget_title.'</h5>'; endif;

		// Get posts
		if ($post_type == 'post') :

			$loop = new WP_Query(array(
				'post_type'			=> 'post',
				'posts_per_page'	=> $quantity,
				'orderby'			=> $order,
				'category__in'		=> $cats,
				'offset'			=> $offset
				)
			);
			
		// Get products
		elseif ($post_type == 'product') :

			$loop = new WP_Query(array(
				'post_type'			=> 'product',
				'posts_per_page'	=> $quantity,
				'orderby'			=> $order,
				'offset'			=> $offset
				)
			);

		endif;

		global $post;

		$counter = 1;

		if ($loop->have_posts()) :
		
			while ( $loop->have_posts() ) : $loop->the_post();
				
				$out .= $counter == 1 ? '<div class="a a-first-child">' : '<div class="a">'; $counter++;

				if ($thumbs == 'display') :
				
					// DATA
					$crop = ($thumbs_crop == 'top') ? '&a=t' : '';

					// IMAGE

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
						if ($src) : $out .= '<a href="'.get_permalink().'" class="block '.$thumbs_float.'" style="width:'.$width.'px; height:'.$height.'px;"><img src="'.$path.'" width="'.$width.'" height="'.$height.'" alt="'.get_the_title().'" /></a>'; endif;

				endif;

				if ($titles == 'display') :

					$font_replace = ( $size > 14 ) ? 'font-replace ' : '';

					$out .= '<a class="'.$font_replace.'ntd f'.$size.'" href="'.get_permalink($post->ID).'">'.get_the_title($post->ID).'</a>';

				endif;

				if ($date == 'display') : $out .= '<em class="block f11">'.get_the_time( get_option('date_format'), $post->ID ).'</em>'; endif;

				if ($excerpt == 'display') : $out .= '<p>'.get_the_excerpt().' - <a href="'.get_permalink().'">'.__('More','pandathemes').'</a></p>'; endif;
				
				$out .= '<div class="clear"><!-- --></div></div>';

			endwhile;

			wp_reset_postdata();

		endif;

		$out .= '</div>';

	echo $out;
	
}

function widget_recent_posts_multi_control($args) {

	$prefix = 'recent-posts-multi'; // $id prefix
	
	$options = get_option('widget_recent_posts_multi');
	if(empty($options)) $options = array();
	if(isset($options[0])) unset($options[0]);
		
	// update options array
	if(!empty($_POST[$prefix]) && is_array($_POST)){
		foreach($_POST[$prefix] as $widget_number => $values){
			if(empty($values) && isset($options[$widget_number])) // user clicked cancel
				continue;
			
			if(!isset($options[$widget_number]) && $args['number'] == -1){
				$args['number'] = $widget_number;
				$options['last_number'] = $widget_number;
			}
			$options[$widget_number] = $values;
		}
		
		// update number
		if($args['number'] == -1 && !empty($options['last_number'])){
			$args['number'] = $options['last_number'];
		}

		// clear unused options and update options in DB. return actual options array
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_recent_posts_multi');
	}
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// WIDGET VARS
	$opts = @$options[$number];
	$title = @$opts['title'];
	$widget_title = @$opts['widget_title'];
	$cats = @$opts['cats'];
	$post_type = @$opts['post_type'];
	$quantity = @$opts['quantity'];
	$offset = @$opts['offset'];
	$order = @$opts['order'];
	$thumbs = @$opts['thumbs'];
	$thumbs_float = @$opts['thumbs_float'];
	$thumbs_crop = @$opts['thumbs_crop'];
	$width = @$opts['width'];
	$height = @$opts['height'];
	$titles = @$opts['titles'];
	$size = @$opts['size'];
	$date = @$opts['date'];
	$excerpt = @$opts['excerpt'];
	// WIDGET CONTROLS
	?>
	<input style="display:none;" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" />

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Widget title','pandathemes') ?></legend>
		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Enter here','pandathemes') ?>
				</span>
			</td>
			<td>
				<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][widget_title]" value="<?php echo $widget_title; ?>" />
			</td>
		</tr></table>
		<div class="clear h10"><!-- --></div>
	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Posts','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Type','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][post_type]">
					<option value="post" <?php if ($post_type == 'post') : ?> selected <?php endif; ?>><?php _e('post','pandathemes') ?></option>
					<option value="product" <?php if ($post_type == 'product') : ?> selected <?php endif; ?>><?php _e('product','pandathemes') ?> &nbsp;</option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Categories','pandathemes') ?> *
				</span>
			</td>
			<td>
				<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][cats]" value="<?php echo $cats; ?>" />
				<small><?php _e("Enter an IDs of categories e.g. 1,2,3,4",'pandathemes'); echo '<br>* '; _e("For the 'post' type only.",'pandathemes'); ?></small>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Quantity','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][quantity]">
					<?php
						$arr = array('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20');
						foreach ($arr as $value) {
							$out = '<option value="'.$value.'"';
							if ( $quantity == $value ) : $out .= ' selected'; endif;
							$out .= '>'.$value.' &nbsp;</option>';
							echo $out;
						};
					?>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Offset','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][offset]">
					<?php
						$arr = array('0','1','2','3','4','5','6','7','8','9','10');
						foreach ($arr as $value) {
							$out = '<option value="'.$value.'"';
							if ( $offset == $value ) : $out .= ' selected'; endif;
							$out .= '>'.$value.' &nbsp;</option>';
							echo $out;
						};
					?>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Order by','pandathemes') ?>
				</span>
			</td>
			<td>
			<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][order]">
				<option value="date" <?php if ($order == 'date') : ?> selected <?php endif; ?>><?php _e('date','pandathemes') ?></option>
				<option value="rand" <?php if ($order == 'rand') : ?> selected <?php endif; ?>><?php _e('random','pandathemes') ?> &nbsp;</option>
			</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Titles','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Display','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][titles]" value="display" <?php if ($titles == 'display') : echo 'checked'; endif; ?> /> 
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Font size','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][size]">
					<?php
						$arr = array('default','12','13','14','15','16','18','20','22','24','26','30');
						foreach ($arr as $value) {
							$out = '<option value="'.$value.'"';
							if ( $size == $value ) : $out .= ' selected'; endif;
							$out .= '>'.$value.' &nbsp;</option>';
							echo $out;
						};
					?>
				</select> &nbsp;px
			</td>
		</tr></table>
		
		<div class="clear h10"><!-- --></div>

	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Thumbnails','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Display','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][thumbs]" value="display" <?php if ($thumbs == 'display') : echo 'checked'; endif; ?> /> 
			</td>
		</tr></table>
		
		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Align','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][thumbs_float]">
					<option value="fl mr10" <?php if ($thumbs_float == 'fl mr10') : echo 'selected'; endif; echo '>'. __('by left','pandathemes') ?></option>
					<option value="fr ml10" <?php if ($thumbs_float == 'fr ml10') : echo 'selected'; endif; echo '>'. __('by right','pandathemes') ?> &nbsp;</option>
					<option value="mcb10" <?php if ($thumbs_float == 'mcb10') : echo 'selected'; endif; echo '>'.__('by center','pandathemes') ?> &nbsp; </option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Width','pandathemes') ?>
				</span>
			</td>
			<td>
				<input type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][width]" size="3" value="<?php if ($width) : echo $width; else : echo '70'; endif; ?>" /> &nbsp;px
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Height','pandathemes') ?>
				</span>
			</td>
			<td>
				<input type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][height]" size="3" value="<?php if ($height) : echo $height; else : echo '50'; endif; ?>" /> &nbsp;px
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Crop','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][thumbs_crop]">
					<option value="top" <?php if ($thumbs_crop == 'top') : ?> selected <?php endif; ?>><?php _e('by top','pandathemes') ?></option>
					<option value="center" <?php if ($thumbs_crop == 'center') : ?> selected <?php endif; ?>><?php _e('by center','pandathemes') ?> &nbsp;</option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Misc','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Date','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][date]" value="display" <?php if ($date == 'display') : echo 'checked'; endif; ?> /> 
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Excerpt','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][excerpt]" value="display" <?php if ($excerpt == 'display') : echo 'checked'; endif; ?> /> 
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<?php
}

// helper function can be defined in another plugin
if(!function_exists('bf_smart_multiwidget_update')){
	function bf_smart_multiwidget_update($id_prefix, $options, $post, $sidebar, $option_name = ''){
		global $wp_registered_widgets;
		static $updated = false;

		// get active sidebar
		$sidebars_widgets = wp_get_sidebars_widgets();
		if ( isset($sidebars_widgets[$sidebar]) )
			$this_sidebar =& $sidebars_widgets[$sidebar];
		else
			$this_sidebar = array();
		
		// search unused options
		foreach ( $this_sidebar as $_widget_id ) {
			if(preg_match('/'.$id_prefix.'-([0-9]+)/i', $_widget_id, $match)){
				$widget_number = $match[1];
				
				// $_POST['widget-id'] contain current widgets set for current sidebar
				// $this_sidebar is not updated yet, so we can determine which was deleted
				if(!in_array($match[0], $_POST['widget-id'])){
					unset($options[$widget_number]);
				}
			}
		}
		
		// update database
		if(!empty($option_name)){
			update_option($option_name, $options);
			$updated = true;
		}
		
		// return updated array
		return $options;
	}
}
?>