<?php
// PANDA - TWITTER WIDGET v.2.2
// www.pandathemes.com
add_action('init', 'widget_twitter_multi_register');
function widget_twitter_multi_register() {
	
	$prefix = 'twitter-multi'; // $id prefix
	$name = 'Panda - Twitter v.2.2';
	$widget_ops = array('classname' => 'widget_twitter_multi', 'description' => __('Twitter widget displays your recent entries.'));
	$control_ops = array('width' => 400, 'height' => 200, 'id_base' => $prefix);
	
	$options = get_option('widget_twitter_multi');
	if(isset($options[0])) unset($options[0]);
	
	if(!empty($options)){
		foreach(array_keys($options) as $widget_number){
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_twitter_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_twitter_multi_control', $control_ops, array( 'number' => $widget_number ));

		}
	} else{
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_twitter_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_twitter_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_twitter_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('twitter-multi-', '', @$widget_id);
    $options = get_option('widget_twitter_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }
	$widget_title = $vars['widget_title'];
	$id = $vars['id'];
	$qty = $vars['qty'];

	// Scripts on footer
	if ( !wp_script_is('blogger', $list = 'registered') ) {
		wp_register_script('blogger', 'http://twitter.com/javascripts/blogger.js', false, null, true, true);
		wp_enqueue_script(array('blogger'), false, null, true, true);
	}
	if ( !wp_script_is('user_timeline', $list = 'registered') ) {
		wp_register_script('user_timeline', 'https://api.twitter.com/1/statuses/user_timeline.json?screen_name=' . $id . '&include_rts=1&callback=twitterCallback2&count=' . $qty, false, null, true, true);
		wp_enqueue_script(array('blogger','user_timeline'), false, null, true, true);
	}

    // WIDGET OUTPUT
		echo $before_widget;
		// TITLE
		// if(!empty($vars['title'])){
		//	echo $before_title . $vars['title'] . $after_title; } 
		// CONTENT
		?>
		<?php if(!empty($vars['widget_title'])){ echo '<h5 id="twitter">'.$widget_title.'</h5>' ;} ?>
		<ul id="twitter_update_list"><li></li></ul>
			<div class="followme-box"><a class="button" target="_blank" href="<?php bloginfo('template_url'); ?>/go.php?http://www.twitter.com/<?php echo $id ?>/"><span><?php _e('Follow Me','pandathemes') ?></span></a></div><div class="fix"><!-- --></div>
		<?php
    	echo $after_widget;
}

function widget_twitter_multi_control($args) {

	$prefix = 'twitter-multi'; // $id prefix
	
	$options = get_option('widget_twitter_multi');
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
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_twitter_multi');
	}
	
	// $number - is dynamic number for multi widget, gived by WP
	// by default $number = -1 (if no widgets activated). In this case we should use %i% for inputs
	//   to allow WP generate number automatically
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// now we can output control
	$opts = @$options[$number];
	$title = @$opts['title'];
	$widget_title = @$opts['widget_title'];
	$id = @$opts['id'];
	$qty = @$opts['qty'];

/*	?>
	<p>
		<label for="title">Title:
		<input type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" class="widefat" />
	</p>
	<?php
*/
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

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Twitter','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Account name','pandathemes') ?>
				</span>
			</td>
			<td>
				<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][id]" value="<?php echo $id; ?>" />
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Twittes qty','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][qty]">
					<?php
						$arr = array('1','2','3','4','5','6','7','8','9','10');
						foreach ($arr as $value) {
							$out = '<option value="'.$value.'"';
							if ( $qty == $value ) : $out .= ' selected'; endif;
							$out .= '>'.$value.' &nbsp;</option>';
							echo $out;
						};
					?>
				</select>
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