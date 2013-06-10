<?php
// PANDA - FLICKR WIDGET v.2.4
// www.pandathemes.com
add_action('init', 'widget_flickr_multi_register');
function widget_flickr_multi_register() {
	
	$prefix = 'flickr-multi'; // $id prefix
	$name = 'Panda - Flickr v.2.4';
	$widget_ops = array('classname' => 'widget_flickr_multi', 'description' => __('Flickr Widget.','pandathemes'));
	$control_ops = array('width' => 400, 'height' => 200, 'id_base' => $prefix);
	
	$options = get_option('widget_flickr_multi');
	if(isset($options[0])) unset($options[0]);
	
	if(!empty($options)){
		foreach(array_keys($options) as $widget_number){
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_flickr_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_flickr_multi_control', $control_ops, array( 'number' => $widget_number ));

		}
	} else{
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_flickr_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_flickr_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_flickr_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('flickr-multi-', '', @$widget_id);
    $options = get_option('widget_flickr_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }
	if ($vars['id']) {$id = $vars['id'];} else {$id = '10729228@N07';};
	if ($vars['type']) {$type = $vars['type'];} else {$type = 'user';};
	$qty = $vars['qty'];
	$title = $vars['title'];
	$order = $vars['order'];
	$button = $vars['button'];

    // WIDGET OUTPUT

	echo '<div class="widget">';

		if ($title) : ?><h5 id="flickrtitle"><?php echo $title; ?></h5><?php ; endif; ?>

		<div id="flickr">
	
			<script type="text/javascript" src="http://www.flickr.com/badge_code_v2.gne?count=<?php echo $qty; ?>&amp;display=<?php echo $order ?>&amp;size=s&amp;layout=x&amp;source=<?php echo $type ?>&amp;<?php echo $type ?>=<?php echo $id ?>"></script>

			<div class="clear"><!-- --></div>

			<?php
				if ($button) :

					if ($type == 'user') :

						echo '<div style="text-align:center; padding:1em 0 0;"><a target="_blank" class="button" href="'.get_bloginfo('template_url').'/go.php?http://www.flickr.com/photos/'.$id.'/"><span>'.$button.'</span></a></div>';

					else :

						echo '<div style="text-align:center; padding:1em 0 0;"><a target="_blank" class="button" href="'.get_bloginfo('template_url').'/go.php?http://www.flickr.com/groups/'.$id.'/"><span>'.$button.'</span></a></div>';

					endif;

				endif;
			?>

		</div>

		<?php

	echo '</div>';

}

function widget_flickr_multi_control($args) {

	$prefix = 'flickr-multi'; // $id prefix
	
	$options = get_option('widget_flickr_multi');
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
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_flickr_multi');
	}
	
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// now we can output control
	$opts = @$options[$number];
	$title = @$opts['title'];
	if (@$opts['id']) {$id = @$opts['id'];} else {$id = '10729228@N07';};
	if (@$opts['type']) {$type = @$opts['type'];} else {$type = 'user';};
	if (@$opts['qty']) {$qty = @$opts['qty'];} else {$qty = 6;};
	$order = @$opts['order'];
	$button = @$opts['button'];

	?>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Widget title','pandathemes') ?></legend>
		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Enter here','pandathemes') ?>
				</span>
			</td>
			<td>
				<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" />
			</td>
		</tr></table>
		<div class="clear h10"><!-- --></div>
	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Flickr','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Account ID','pandathemes') ?>
				</span>
			</td>
			<td>
				<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][id]" value="<?php echo $id; ?>" />
				<small><?php _e('Enter your Flickr ID','pandathemes') ?> (<a href="http://www.idgettr.com">idGettr</a>).</small>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Account type','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][type]">
					<option value="user" <?php if ($type=="user"):?> selected <?php endif; ?>><?php _e('user','pandathemes') ?></option>
					<option value="group" <?php if ($type=="group"):?> selected <?php endif; ?>><?php _e('group','pandathemes') ?>&nbsp;</option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Photos','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Quantity','pandathemes') ?>
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

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Order','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][order]">
					<option value="random" <?php if ($order == 'random'):?> selected <?php endif; ?>><?php _e('random','pandathemes') ?>&nbsp;</option>
					<option value="latest" <?php if ($order == 'latest'):?> selected <?php endif; ?>><?php _e('latest','pandathemes') ?></option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Button','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Text','pandathemes') ?>
				</span>
			</td>
			<td>
				<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][button]" value="<?php echo $button; ?>" />
				<small><?php _e("In case you'd like to display the 'Follow Us' button please enter here the text of button.",'pandathemes') ?></small>
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