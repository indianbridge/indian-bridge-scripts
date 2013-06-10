<?php
// PANDA - ADS 300px WIDGET v.3.0
// www.pandathemes.com
add_action('init', 'widget_ads300_multi_register');
function widget_ads300_multi_register() {
	
	$prefix = 'ads300-multi'; // $id prefix
	$name = 'Panda - Ads 300px v.3.0';
	$widget_ops = array('classname' => 'widget_ads300_multi', 'description' => __('Advertising 300px banner\'s place. Google AdSense compatible.'));
	$control_ops = array('width' => 400, 'height' => 200, 'id_base' => $prefix);
	
	$options = get_option('widget_ads300_multi');
	if(isset($options[0])) unset($options[0]);
	
	if(!empty($options)){
		foreach(array_keys($options) as $widget_number){
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_ads300_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_ads300_multi_control', $control_ops, array( 'number' => $widget_number ));

		}
	} else{
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_ads300_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_ads300_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_ads300_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('ads300-multi-', '', @$widget_id);
    $options = get_option('widget_ads300_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }
	$code = stripslashes($vars['code']);

	echo '<div class="widget ads300">'.$code.'</div>';

}

function widget_ads300_multi_control($args) {

	$prefix = 'ads300-multi'; // $id prefix
	
	$options = get_option('widget_ads300_multi');
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
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_ads300_multi');
	}
	
	// $number - is dynamic number for multi widget, gived by WP
	// by default $number = -1 (if no widgets activated). In this case we should use %i% for inputs
	// to allow WP generate number automatically
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// now we can output control
	$opts = @$options[$number];
	$title = @$opts['title'];
	$code = stripslashes(@$opts['code']);

	?>
	<input style="display:none;" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" />
	<p>
		Code:
		<textarea class="widefat" rows="6" cols="20" name="<?php echo $prefix; ?>[<?php echo $number; ?>][code]" ><?php echo $code; ?></textarea>
	</p>

		<?php _e('Draft','pandathemes') ?>: <code>&lt;a href="#" target="_blank" rel="nofollow">&lt;img src="image.jpg" />&lt;/a></code>
	
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