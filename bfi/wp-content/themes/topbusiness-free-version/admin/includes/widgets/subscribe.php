<?php
// PANDA - SUBSCRIBE WIDGET v.2.3
// www.pandathemes.com
add_action('init', 'widget_subscribe_multi_register');
function widget_subscribe_multi_register() {
	
	$prefix = 'subscribe-multi'; // $id prefix
	$name = 'Panda - Subscribe v.2.3';
	$widget_ops = array('classname' => 'widget_subscribe_multi', 'description' => __('Subscribe via FeedBurner'));
	$control_ops = array('width' => 400, 'height' => 200, 'id_base' => $prefix);
	
	$options = get_option('widget_subscribe_multi');
	if(isset($options[0])) unset($options[0]);
	
	if(!empty($options)){
		foreach(array_keys($options) as $widget_number){
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_subscribe_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_subscribe_multi_control', $control_ops, array( 'number' => $widget_number ));

		}
	} else{
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_subscribe_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_subscribe_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_subscribe_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('subscribe-multi-', '', @$widget_id);
    $options = get_option('widget_subscribe_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }
	$feedId = $vars['feedId'];
	$rss = $vars['rss'];
	$email = $vars['email'];
	$counter = $vars['counter'];
	if ($vars['descr']<>"") {$descr = stripslashes($vars['descr']);}
		else {$descr = __('Sign up for our newsletter to receive the latest news and event postings.','pandathemes');};

// GET A NUMBER OF FEEDBURNER SUBSCRIBERS
require_once(ABSPATH . 'wp-includes/class-snoopy.php');
$fb = get_option("feedburnersubscribecount");
if ($fb['lastcheck'] < ( mktime() - 600 ) ) {
	$snoopy = new Snoopy;
	$result = $snoopy->fetch("http://feedburner.google.com/api/awareness/1.0/GetFeedData?uri=".$feedId);
	if ($result) {
		preg_match('/circulation=\"([0-9]+)\"/',$snoopy->results, $matches);
		if ($matches[1] != 0)
			$fb['count'] = $matches[0];
		$fb['lastcheck'] = mktime();
		update_option("feedburnersubscribecount",$fb);
	}
}
$q = $fb['count'];

preg_match("/^(circulation=\")?([^\/]+)/i", $q, $matches);
	$q = $matches[2];
preg_match("/^()?([^\"]+)/i", $q, $matches);
	$q = $matches[2];

	echo '<div class="widget">';

		if ($rss == 'display') { ?>

			<div id="feederss-label">
				<a class="feederss-label" target="_blank" href="<?php bloginfo('template_url'); ?>/go.php?http://feeds.feedburner.com/<?php echo $feedId ?>">
					<h5 class="rss-title"><?php if ($counter == 'display') : echo $q.' '.__('readers','pandathemes'); else : echo 'RSS '.__('subscribe','pandathemes'); endif; ?></h5>
				</a>
			</div>

		<?php ;}

		if ($email == 'display') { ?>

			<div id="feedemail-form">
				<h5><?php _e('Email subscribe','pandathemes') ?></h5>
				<form class="feedemail-form" action="http://feedburner.google.com/fb/a/mailverify" method="post" target="popupwindow" onsubmit="window.open('http://feedburner.google.com/fb/a/mailverify?uri=<?php echo $feedId ?>', 'popupwindow', 'scrollbars=yes,width=600,height=550');return true">
					<div>
						<input type="text" class="feedemail-input" name="email" onblur="this.value=(this.value=='') ? 'your.email@address' : this.value;" onfocus="this.value=(this.value=='your.email@address') ? '' : this.value;" maxlength="150" value="your.email@address" />
						<input type="hidden" value="<?php echo $feedId ?>" name="uri"/>
						<input type="hidden" name="loc" value="en_US"/>
						<input type="submit" value="<?php _e('Subscribe','pandathemes') ?>" class="feedemail-button"/>
					</div>
				</form>
				<div class="clear h10"><!-- --></div>
				<?php echo $descr; ?>
			</div>

		<?php ;}
		
	echo '</div>';

}

function widget_subscribe_multi_control($args) {

	$prefix = 'subscribe-multi'; // $id prefix
	
	$options = get_option('widget_subscribe_multi');
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
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_subscribe_multi');
	}
	
	// $number - is dynamic number for multi widget, gived by WP
	// by default $number = -1 (if no widgets activated). In this case we should use %i% for inputs
	//   to allow WP generate number automatically
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// now we can output control
	$opts = @$options[$number];
	$title = @$opts['title'];
	$feedId = @$opts['feedId'];
	$rss = @$opts['rss'];
	$email = @$opts['email'];
	$counter = @$opts['counter'];
	if (@$opts['descr']<>"") {$descr = stripslashes(@$opts['descr']);}
		else {$descr = __('Sign up for our newsletter to receive the latest news and event postings.','pandathemes');};

/*	?>
	<p>
		<label for="title">Title:
		<input type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" class="widefat" />
	</p>
	<?php
*/
	?>
	<input style="display:none;" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" />

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Feedburner','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Feed ID','pandathemes') ?>
				</span>
			</td>
			<td>
				<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][feedId]" value="<?php echo $feedId; ?>" />
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('RSS-subscribe','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Display','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][rss]" value="display" <?php if ($rss == 'display') : echo 'checked'; endif; ?> /> 
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Counter','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][counter]" value="display" <?php if ($counter == 'display') : echo 'checked'; endif; ?> /> 
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>

	<fieldset class="panda-admin-fieldset"><legend><?php _e('Email-subscribe','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Display','pandathemes') ?>
				</span>
			</td>
			<td class="pt6">
				<input type="checkbox" name="<?php echo $prefix; ?>[<?php echo $number; ?>][email]" value="display" <?php if ($email == 'display') : echo 'checked'; endif; ?> /> 
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Description','pandathemes') ?>
				</span>
			</td>
			<td>
				<textarea class="w100p" rows="4" cols="20" name="<?php echo $prefix; ?>[<?php echo $number; ?>][descr]" ><?php echo $descr; ?></textarea>
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