<?php
// PANDA SIDEBAR TABS v.1.6
// www.pandathemes.com
add_action('init', 'widget_tabs_multi_register');
function widget_tabs_multi_register() {
	
	$prefix = 'tabs-multi'; // $id prefix
	$name = 'Panda - Tabs v.1.6';
	$widget_ops = array('classname' => 'widget_tabs_multi', 'description' => __('Recent comments, Tags, Categories, Archives, Pages. Up to 5 Tabs.'));
	$control_ops = array('width' => 400, 'height' => 200, 'id_base' => $prefix);
	
	$options = get_option('widget_tabs_multi');
	if(isset($options[0])) unset($options[0]);
	
	if(!empty($options)){
		foreach(array_keys($options) as $widget_number){
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_tabs_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_tabs_multi_control', $control_ops, array( 'number' => $widget_number ));

		}
	} else{
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_tabs_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_tabs_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_tabs_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('tabs-multi-', '', @$widget_id);
    $options = get_option('widget_tabs_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }

	$categories_order = $vars['categories_order'];
	$pages_order = $vars['pages_order'];
	$tags_order = $vars['tags_order'];
	$archives_order = $vars['archives_order'];

	// START ARRAY
	$pandatabs = array(
		$categories_order => array(
			"title" => $vars['categories_title']
		),
		$pages_order => array(
			"title" => $vars['pages_title']
		),
		$tags_order => array(
			"title" => $vars['tags_title']
		),
		$archives_order => array(
			"title" => $vars['archives_title']
		)
	);

	// SORT ARRAY
	ksort($pandatabs);

	?>

	<div class="widget pt10">

		<ul class="kul kul-short kul-auto">
			<?php // TABS
			foreach ($pandatabs as $key => $value) :

		   	if ($key > 0) :
				
					echo '<li>'.$pandatabs[$key]['title'].'</li>';

				endif;

			endforeach;
			?>
		</ul>
		
		<div class="ktabs nomargin ktabs-auto" style="background:#FFF;">

			<?php // TABS
			foreach ($pandatabs as $key => $value) :

		   	if ($key > 0) :


					// CATEGORIES
					if ($pandatabs[$key]['title'] == $vars['categories_title']) : ?>

						<div>
							<ul class="tabs-widget-cats">
								<?php wp_list_categories("depth=3&title_li=&hide_empty=1&show_count=1&include=".$vars['categories_include']); ?>
							</ul>
						</div>

					<?php ; endif;


					// PAGES
					if ($pandatabs[$key]['title'] == $vars['pages_title']) : ?>

						<div>
							<ul class="tabs-widget-pages">
								<?php wp_list_pages("depth=3&sort_column=menu_order&title_li=&include=".$vars['pages_include']); ?>
							</ul>
						</div>

					<?php ; endif;


					// TAGS
					if ($pandatabs[$key]['title'] == $vars['tags_title']) : ?>

						<div>
							<div class="tag-cloud tabs-widget-tags">
								<?php wp_tag_cloud(array(
									'smallest'	=> '11',
									'largest'	=> '11',
									'unit'		=> 'px',
									'separator'	=> '',
									'orderby'	=> 'count',
									'order'		=> 'DESC'
									));
								?> 
							</div>
						</div>

					<?php ; endif;


					// ARCHIVES
					if ($pandatabs[$key]['title'] == $vars['archives_title']) : ?>

						<div>
							<ul class="tabs-widget-arch">
								<?php wp_get_archives('type=monthly&show_post_count=1'); ?>
							</ul>
						</div>

					<?php ; endif;

				endif;

			endforeach; ?>

		</div>

	</div>

	<?php

}

function widget_tabs_multi_control($args) {

	$prefix = 'tabs-multi'; // $id prefix
	
	$options = get_option('widget_tabs_multi');
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
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_tabs_multi');
	}
	
	// $number - is dynamic number for multi widget, gived by WP
	// by default $number = -1 (if no widgets activated). In this case we should use %i% for inputs
	//   to allow WP generate number automatically
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// now we can output control
	$opts = @$options[$number];
	$title = @$opts['title'];
	// CATEGORIES
	$categories_order = @$opts['categories_order'];
	$categories_title = @$opts['categories_title'];
	$categories_include = @$opts['categories_include'];
	// PAGES
	$pages_order = @$opts['pages_order'];
	$pages_title = @$opts['pages_title'];
	$pages_include = @$opts['pages_include'];
	// TAGS
	$tags_order = @$opts['tags_order'];
	$tags_title = @$opts['tags_title'];
	// ARCHIVES
	$archives_order = @$opts['archives_order'];
	$archives_title = @$opts['archives_title'];
	?>
	<input style="display:none;" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" />


	<!-- T A B   C A T E G O R I E S -->


	<fieldset class="panda-admin-fieldset"><legend><?php _e('Categories','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab name','pandathemes') ?>
				</span>
			</td>
			<td>
				<input id="<?php echo $prefix; ?>[<?php echo $number; ?>][categories_title]" name="<?php echo $prefix; ?>[<?php echo $number; ?>][categories_title]" type="text" class="widefat" value="<?php if (!$categories_title) { _e('Categories','pandathemes') ;} else { echo $categories_title;} ?>" />
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab order','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][categories_order]">
					<option value="0" <?php if ($categories_order=='0'):?> selected <?php endif; ?>><?php _e('hide','pandathemes') ?>&nbsp;</option>
					<option value="1" <?php if ($categories_order=='1'):?> selected <?php endif; ?>>1</option>
					<option value="2" <?php if ($categories_order=='2'):?> selected <?php endif; ?>>2</option>
					<option value="3" <?php if ($categories_order=='3'):?> selected <?php endif; ?>>3</option>
					<option value="4" <?php if ($categories_order=='4'):?> selected <?php endif; ?>>4</option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Selected','pandathemes') ?>
				</span>
			</td>
			<td>
				<input id="<?php echo $prefix; ?>[<?php echo $number; ?>][categories_include]" name="<?php echo $prefix; ?>[<?php echo $number; ?>][categories_include]" type="text" class="widefat" value="<?php echo $categories_include; ?>" />
				<small><?php _e('Enter an IDs of selected categories e.g.&nbsp;1,2,3,4','pandathemes') ?></small>
			</td>
		</tr></table>

	</fieldset>


	<!-- T A B   P A G E S -->


	<fieldset class="panda-admin-fieldset"><legend><?php _e('Pages','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab name','pandathemes') ?>
				</span>
			</td>
			<td>
				<input id="<?php echo $prefix; ?>[<?php echo $number; ?>][pages_title]" name="<?php echo $prefix; ?>[<?php echo $number; ?>][pages_title]" type="text" class="widefat" value="<?php if (!$pages_title) { _e('Pages','pandathemes') ;} else { echo $pages_title;} ?>" />
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab order','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][pages_order]">
					<option value="0" <?php if ($pages_order=='0'):?> selected <?php endif; ?>><?php _e('hide','pandathemes') ?>&nbsp;</option>
					<option value="1" <?php if ($pages_order=='1'):?> selected <?php endif; ?>>1</option>
					<option value="2" <?php if ($pages_order=='2'):?> selected <?php endif; ?>>2</option>
					<option value="3" <?php if ($pages_order=='3'):?> selected <?php endif; ?>>3</option>
					<option value="4" <?php if ($pages_order=='4'):?> selected <?php endif; ?>>4</option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Selected','pandathemes') ?>
				</span>
			</td>
			<td>
				<input id="<?php echo $prefix; ?>[<?php echo $number; ?>][pages_include]" name="<?php echo $prefix; ?>[<?php echo $number; ?>][pages_include]" type="text" class="widefat" value="<?php echo $pages_include; ?>" />
				<small><?php _e('Enter an IDs of selected pages e.g.&nbsp;1,2,3,4','pandathemes') ?></small>
			</td>
		</tr></table>

	</fieldset>


	<!-- T A B   T A G S -->


	<fieldset class="panda-admin-fieldset"><legend><?php _e('Tags','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab name','pandathemes') ?>
				</span>
			</td>
			<td>
				<input id="<?php echo $prefix; ?>[<?php echo $number; ?>][tags_title]" name="<?php echo $prefix; ?>[<?php echo $number; ?>][tags_title]" type="text" class="widefat" value="<?php if (!$tags_title) { _e('Tags','pandathemes') ;} else { echo $tags_title;} ?>" />
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab order','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][tags_order]">
					<option value="0" <?php if ($tags_order=='0'):?> selected <?php endif; ?>><?php _e('hide','pandathemes') ?>&nbsp;</option>
					<option value="1" <?php if ($tags_order=='1'):?> selected <?php endif; ?>>1</option>
					<option value="2" <?php if ($tags_order=='2'):?> selected <?php endif; ?>>2</option>
					<option value="3" <?php if ($tags_order=='3'):?> selected <?php endif; ?>>3</option>
					<option value="4" <?php if ($tags_order=='4'):?> selected <?php endif; ?>>4</option>
				</select>
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

	</fieldset>


	<!-- T A B   A R C H I V E S -->


	<fieldset class="panda-admin-fieldset"><legend><?php _e('Archives','pandathemes') ?></legend>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab name','pandathemes') ?>
				</span>
			</td>
			<td>
				<input id="<?php echo $prefix; ?>[<?php echo $number; ?>][archives_title]" name="<?php echo $prefix; ?>[<?php echo $number; ?>][archives_title]" type="text" class="widefat" value="<?php if (!$archives_title) { _e('Archives','pandathemes') ;} else { echo $archives_title;} ?>" />
			</td>
		</tr></table>

		<div class="clear h10"><!-- --></div>

		<table class="w100p"><tr>
			<td class="size17">
				<span>
					<?php _e('Tab order','pandathemes') ?>
				</span>
			</td>
			<td>
				<select name="<?php echo $prefix; ?>[<?php echo $number; ?>][archives_order]">
					<option value="0" <?php if ($archives_order=='0'):?> selected <?php endif; ?>><?php _e('hide','pandathemes') ?>&nbsp;</option>
					<option value="1" <?php if ($archives_order=='1'):?> selected <?php endif; ?>>1</option>
					<option value="2" <?php if ($archives_order=='2'):?> selected <?php endif; ?>>2</option>
					<option value="3" <?php if ($archives_order=='3'):?> selected <?php endif; ?>>3</option>
					<option value="4" <?php if ($archives_order=='4'):?> selected <?php endif; ?>>4</option>
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