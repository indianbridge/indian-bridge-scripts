<?php
// Demo Styles widget v.2.0
// www.pandathemes.com
add_action('init', 'widget_demo_multi_register');
function widget_demo_multi_register() {
	
	$prefix = 'demo-multi'; // $id prefix
	$name = 'Panda - Demo Styles v.2.0';
	$widget_ops = array('classname' => 'widget_demo_multi', 'description' => 'Demo styles.');
	$control_ops = array('width' => 200, 'height' => 200, 'id_base' => $prefix);
	
	$options = get_option('widget_demo_multi');
	if(isset($options[0])) unset($options[0]);
	
	if(!empty($options)){
		foreach(array_keys($options) as $widget_number){
			wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_demo_multi', $widget_ops, array( 'number' => $widget_number ));
			wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_demo_multi_control', $control_ops, array( 'number' => $widget_number ));

		}
	} else{
		$options = array();
		$widget_number = 1;
		wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_demo_multi', $widget_ops, array( 'number' => $widget_number ));
		wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_demo_multi_control', $control_ops, array( 'number' => $widget_number ));
	}
}

function widget_demo_multi($args, $vars = array()) {
    extract($args);
    $widget_number = (int)str_replace('demo-multi-', '', @$widget_id);
    $options = get_option('widget_demo_multi');
    if(!empty($options[$widget_number])){
    	$vars = $options[$widget_number];
    }

$template_url = get_bloginfo('template_url');

// WIDGET OUTPUT
?>

<!------------------------------------ 
		P A N E L
------------------------------------->

<div id="demo-source">

	<div id="demo-panel">

		<div class="related">	

			<span id="demo-open" class="demo-none">&nbsp;</span>
			<span id="demo-close">&nbsp;</span>
	
			<div alt="s1" class="tab-style ts1 tab-style-current"></div>
			<div alt="s2" class="tab-style ts2"></div>
			<div alt="s3" class="tab-style ts3"></div>
			<div alt="s4" class="tab-style ts4"></div>
			<div alt="s5" class="tab-style ts5"></div>
			<div alt="s6" class="tab-style ts6"></div>
			<div alt="s7" class="tab-style ts7"></div>
			<div alt="s8" class="tab-style ts8"></div>
			<div alt="s9" class="tab-style ts9"></div>
			<div alt="s10" class="tab-style ts10"></div>

		</div>

	</div>

</div>


<!------------------------------------ 
		S C R I P T
------------------------------------->

<script type="text/javascript">

var d = jQuery.noConflict();
jQuery(document).ready(function(d) {

	var demo = d('#demo-source').html();

		// remove the placeholder
		d('#demo-source').remove();

		// append panel
		d('body').append(demo);

		// get the height of panel and browser window
		var demoH = d(demo).outerHeight();
		var wH = d(window).height();
		var top = (wH - demoH) / 3;
		var panel = d('#demo-panel');
		var panelW = panel.outerWidth() + 4;

		// set the position
		panel.css({'top':top});

		// onload animation
		panel.delay(2000).animate({'left':0}, 500);

		// onlick by tab
		d('.tab-style').click(function(){

				// remove current tab-style	
				d('.tab-style-current',panel).removeClass('tab-style-current');

				// set new tab-style
				d(this).addClass('tab-style-current');

				// remove current demo-style
				d('#demo-css').remove();

				// define new demo-style
				var s = d(this).attr('alt');
				var css = "<link rel='stylesheet' id='demo-css' href='<?php echo $template_url ?>/styles/predetermined/" + s + ".css' type='text/css' media='all' />";

				// set new demo-style
				d('head').append(css);

		})

	d('#demo-close').click(function() {

		panel.stop()

			.animate({ 'left': -panelW }, 300,

				function() {

					d('#demo-open',panel).removeClass('demo-none').animate({ 'left': panelW }, 300);

				}

			);

	});

	d('#demo-open').click(function() {
		
		d('#demo-open',panel)

			.animate({ 'left': 0 }, 300,

				function() {

					panel.stop()

						.animate({ 'left': 0 }, 300);

				}

			);

	});


});

</script>



<!------------------------------------ 
		S T Y L E S
------------------------------------->

<style type="text/css" media="all">

#demo-source {
	display:none;
	}

#demo-panel {
	position:fixed;
	left:-150px;
	top:0;
	width:30px;
	height:335px;
	border-top-right-radius:3px;
	border-bottom-right-radius:3px;
	background:#FFF;
	padding:0 0 0 7px;
	z-index:30;
	box-shadow:0 1px 3px rgba(0, 0, 0, 0.3);
	}

#demo-panel p {
	padding:5px 0;
	font-size:13px !important;
	font-family:arial !important;
	}

#demo-close {
	position:relative;
	width:40px;
	height:30px;
	margin:0 0 10px -10px;
	display:block;
	background:#555 url(<?php echo $template_url ?>/images/icons/16/led-icons/arrow_left.png) center no-repeat;
	cursor:pointer;
	border-top-right-radius:3px;
	}

#demo-open {
	position:absolute;
	width:22px;
	height:22px;
	margin:0;
	left:0;
	display:block;
	background:#FFF url(<?php echo $template_url ?>/images/icons/16/led-icons/color_swatch_2.png) center no-repeat;
	cursor:pointer;
	border-top-right-radius:3px;
	border-bottom-right-radius:3px;
	box-shadow:0 1px 3px rgba(0, 0, 0, 0.3);
	}

.demo-none {
	display:none;
	}

.tab-style {
	width:20px;
	height:20px;
	border:2px solid #DDD;
	float:left;
	margin:0 5px 5px 0;
	cursor:pointer;
	border-radius:2px;
	}

.tab-style:hover {
	border:2px solid orange;
	}

.tab-style-current {
	border:2px solid #999 !important;
	}

.ts1 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) 0px top no-repeat; }
.ts2 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -20px top no-repeat; }
.ts3 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -40px top no-repeat; }
.ts4 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -60px top no-repeat; }
.ts5 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -80px top no-repeat; }
.ts6 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -100px top no-repeat; }
.ts7 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -120px top no-repeat; }
.ts8 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -140px top no-repeat; }
.ts9 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -160px top no-repeat; }
.ts10 { background:url(<?php echo $template_url ?>/admin/graphics/styles/for-demo-widget.png) -180px top no-repeat; }

</style>



<?php
}

function widget_demo_multi_control($args) {

	$prefix = 'demo-multi'; // $id prefix
	
	$options = get_option('widget_demo_multi');
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
		$options = bf_smart_multiwidget_update($prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_demo_multi');
	}
	
	// $number - is dynamic number for multi widget, gived by WP
	// by default $number = -1 (if no widgets activated). In this case we should use %i% for inputs
	//   to allow WP generate number automatically
	$number = ($args['number'] == -1)? '%i%' : $args['number'];

	// now we can output control
	$opts = @$options[$number];
	$title = @$opts['title'];

/*	?>
	<p>
		<label for="title">Title:
		<input type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" class="widefat" />
	</p>
	<?php
*/
	?>
	<input style="display:none;" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][title]" value="<?php echo $title; ?>" />
	<p>
		Demo styles.
	</p>
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