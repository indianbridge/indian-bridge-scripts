<?php
// PANDA - ADS 125px WIDGET v.3.0
// www.pandathemes.com
add_action('init', 'widget_ads125_multi_register');
function widget_ads125_multi_register() {
	
	$prefix = 'ads125-multi'; // $id prefix
	$name = 'Panda - Ads 125px v.3.0';
	$widget_ops = array('classname' => 'widget_ads125_multi', 'description' => __('Advertising 125px banner\'s place. Up&nbsp;to&nbsp;6 places.'));
	$control_ops = array('width' => 400, 'height' => 200, 'id_base' => $prefix);
	
	$options = get_option('widget_ads125_multi');
	if(isset($options[0])) unset($options[0]);
	
	if(!empty($options)){
		foreach(array_keys($options) as $widget_number){
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_ads125_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_ads125_multi_control', $control_ops, array( 'number' => $widget_number ));

		}
	} else{
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_ads125_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_ads125_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_ads125_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('ads125-multi-', '', @$widget_id);
    $options = get_option('widget_ads125_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }
	$shuffle = stripslashes($vars['shuffle']);
	$dummy = __('Insert Ad code','pandathemes');
	$code_1 = stripslashes($vars['code_1']);
	$code_2 = stripslashes($vars['code_2']);
	$code_3 = stripslashes($vars['code_3']);
	$code_4 = stripslashes($vars['code_4']);
	$code_5 = stripslashes($vars['code_5']);
	$code_6 = stripslashes($vars['code_6']);
	$code_7 = stripslashes($vars['code_7']);
	$code_8 = stripslashes($vars['code_8']);
	$code_9 = stripslashes($vars['code_9']);
	$code_10 = stripslashes($vars['code_10']);

		?>

		<div class="widget">
		<div class="ads125">
			<?php
				$arr = array($code_1, $code_2, $code_3, $code_4, $code_5, $code_6, $code_7, $code_8, $code_9, $code_10);
				if ($shuffle=='yes') {
					shuffle($arr);
				};
				foreach ($arr as $value) {
					if ($value<>'' && $value!=$dummy) {
						echo '<div>'.$value.'</div>';
					}
				};
			?>
		</div>
		<div class="clear"><!-- --></div>
		</div>
		<?php

}

function widget_ads125_multi_control($args) {

	$prefix = 'ads125-multi'; // $id prefix
	
	$options = get_option('widget_ads125_multi');
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
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_ads125_multi');
	}
	
	// $number - is dynamic number for multi widget, gived by WP
	// by default $number = -1 (if no widgets activated). In this case we should use %i% for inputs
	//   to allow WP generate number automatically
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// now we can output control
	$opts = @$options[$number];
	$title = @$opts['title'];
	$shuffle = @$opts['shuffle'];
	$dummy = __('Insert Ad code','pandathemes');
	$code_1 = stripslashes(@$opts['code_1']);
	$code_2 = stripslashes(@$opts['code_2']);
	$code_3 = stripslashes(@$opts['code_3']);
	$code_4 = stripslashes(@$opts['code_4']);
	$code_5 = stripslashes(@$opts['code_5']);
	$code_6 = stripslashes(@$opts['code_6']);
	$code_7 = stripslashes(@$opts['code_7']);
	$code_8 = stripslashes(@$opts['code_8']);
	$code_9 = stripslashes(@$opts['code_9']);
	$code_10 = stripslashes(@$opts['code_10']);

/*	?>
	<p>
		<label for="title">Title:
		<input type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" class="widefat" />
	</p>
	<?php
*/
	?>
	<input style="display:none;" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" />

		<ul class="kul">
			<li class="kcurrent">1</li>
			<li>2</li>
			<li>3</li>
			<li>4</li>
			<li>5</li>
			<li>6</li>
			<li>7</li>
			<li>8</li>
			<li>9</li>
			<li>10</li>
		</ul>

		<div class="ktabs">
			<?php
				$arr = array($code_1, $code_2, $code_3, $code_4, $code_5, $code_6, $code_7, $code_8, $code_9, $code_10);
				$count = 0;
				foreach ($arr as $value) {

					$count++;
				
					$out = ($count == 1) ? '<div class="block">' : '<div>';

					$out .= '<textarea class="widefat" rows="6" cols="20" name="';

					$out .= $prefix.'['.$number.'][code_'.$count.']" >';

					if ($value) : $out .= $value; else : $out .= $dummy; endif;

					$out .= '</textarea>';
					
					$out .= '</div>';

					echo $out;

				};
			?>
		</div>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Settings','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Shuffle','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][shuffle]" value="yes" <?php if ($shuffle == 'yes') { echo 'checked' ;} ?> />
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<div class="clear h10"><!-- --></div>

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