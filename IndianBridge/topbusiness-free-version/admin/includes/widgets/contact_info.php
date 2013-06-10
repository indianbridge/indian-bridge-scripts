<?php



// ********************************************
//
// 	PANDA - CONTACT INFO WIDGET v.1.0
//	www.pandathemes.com
//
// ********************************************



	function widget_contact_info_multi_register() {
		
		$prefix = 'contact_info-multi'; // $id prefix
	
		$name = 'Panda - Contact Info v.1.0';
	
		$widget_ops = array(
			'classname'		=> 'widget_contact_info_multi',
			'description'	=> __('Contact Info widget displays your address, phone number, email etc.','pandathemes')
			);
	
		$control_ops = array(
			'width'			=> 400,
			'height'		=> 200,
			'id_base'		=> $prefix
			);
		
		$options = get_option('widget_contact_info_multi');
	
		if ( isset($options[0]) ) unset($options[0]);
		
		if ( !empty($options) ) :
	
			foreach ( array_keys($options) as $widget_number ) {
	
				wp_register_sidebar_widget($prefix.'-'.$widget_number, $name, 'widget_contact_info_multi', $widget_ops, array( 'number' => $widget_number ));
				wp_register_widget_control($prefix.'-'.$widget_number, $name, 'widget_contact_info_multi_control', $control_ops, array( 'number' => $widget_number ));
	
			};
	
		else :
	
			$options = array();
			$widget_number = 1;
	
			wp_register_sidebar_widget( $prefix . '-' . $widget_number, $name, 'widget_contact_info_multi', $widget_ops, array( 'number' => $widget_number ) );
			wp_register_widget_control( $prefix . '-' . $widget_number, $name, 'widget_contact_info_multi_control', $control_ops, array( 'number' => $widget_number ) );
	
		endif;
	
	}
	
	add_action('init', 'widget_contact_info_multi_register');



	// --  B A C K - E N D  --------------------------------- //

	function widget_contact_info_multi_control($args) {
	
		$prefix = 'contact_info-multi'; // $id prefix
		
		$options = get_option('widget_contact_info_multi');

		if ( empty($options) ) $options = array();

		if ( isset($options[0]) ) unset($options[0]);
			
		if ( !empty($_POST[$prefix]) && is_array($_POST) ) :

			foreach ($_POST[$prefix] as $widget_number => $values) :

				if ( empty($values) && isset($options[$widget_number]) ) // user clicked cancel

					continue;
				
				if ( !isset($options[$widget_number]) && $args['number'] == -1 ) :

					$args['number'] = $widget_number;
					$options['last_number'] = $widget_number;

				endif;

				$options[$widget_number] = $values;

			endforeach;
			
			if ( $args['number'] == -1 && !empty($options['last_number']) ) :

				$args['number'] = $options['last_number'];

			endif;
	
			$options = bf_smart_multiwidget_update( $prefix, $options, $_POST[$prefix], $_POST['sidebar'], 'widget_contact_info_multi' );

		endif;
		
		$number = ($args['number'] == -1)? '%i%' : $args['number'];
	
		// DATA
		$opts = @$options[$number];

			$title = @$opts['title'];
			$widget_title = @$opts['widget_title'];
			$introduce = @$opts['introduce'];
			$phone = @$opts['phone'];
			$email = @$opts['email'];
			$address = @$opts['address'];
			$name = @$opts['name'];
	
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

			<fieldset class="panda-admin-fieldset"><legend><?php _e('Contacts','pandathemes') ?></legend>
				<table class="w100p">

					<!--  I n t r o d u c e   t e x t  -->

						<tr>
							<td class="size17">
								<span>
									<?php _e('Introduce text','pandathemes') ?>
								</span>
							</td>
							<td>
								<textarea class="w100p" rows="4" cols="20" name="<?php echo $prefix; ?>[<?php echo $number; ?>][introduce]" ><?php echo $introduce; ?></textarea>
								<div class="clear h15"><!-- --></div>
							</td>
						</tr>

					<!--  P h o n e  -->

						<tr>
							<td class="size17">
								<span>
									<?php _e('Phone','pandathemes') ?>
								</span>
							</td>
							<td>
								<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][phone]" value="<?php echo $phone; ?>" />
								<div class="clear h15"><!-- --></div>
							</td>
						</tr>

					<!--  E m a i l  -->

						<tr>
							<td class="size17">
								<span>
									<?php _e('Email','pandathemes') ?>
								</span>
							</td>
							<td>
								<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][email]" value="<?php echo $email; ?>" />
								<div class="clear h15"><!-- --></div>
							</td>
						</tr>

					<!--  A d d r e s s  -->

						<tr>
							<td class="size17">
								<span>
									<?php _e('Address','pandathemes') ?>
								</span>
							</td>
							<td>
								<textarea class="w100p" rows="4" cols="20" name="<?php echo $prefix; ?>[<?php echo $number; ?>][address]" ><?php echo $address; ?></textarea>
								<div class="clear h15"><!-- --></div>
							</td>
						</tr>

					<!--  N a m e  -->

						<tr>
							<td class="size17">
								<span>
									<?php _e('Name','pandathemes') ?>
								</span>
							</td>
							<td>
								<input class="widefat" type="text" name="<?php echo $prefix; ?>[<?php echo $number; ?>][name]" value="<?php echo $name; ?>" />
							</td>
						</tr>

				</table>
				<div class="clear h10"><!-- --></div>
			</fieldset>

		<?php
	}



	// --  F R O N T - E N D  --------------------------------- //

	function widget_contact_info_multi( $args, $vars = array() ) {
	
		extract($args);
	
		$widget_number = (int)str_replace('contact_info-multi-', '', @$widget_id);
	
		$options = get_option('widget_contact_info_multi');
	
		if ( !empty($options[$widget_number]) ) :
	
			$vars = $options[$widget_number];
	
		endif;

		// DATA	
		$widget_title = $vars['widget_title'];
		$introduce = $vars['introduce'];
		$phone = $vars['phone'];
		$email = $vars['email'];
		$address = $vars['address'];
		$name = $vars['name'];

		// DISPLAY WIDGET

			// Before
			$out = '<div class="widget widget-contact-info">';
	
			// Title
			if ( !empty($vars['widget_title']) ) { $out .= '<h5>' . $widget_title . '</h5>' ;}

			// Introduce
			if ( !empty($vars['introduce']) ) { $out .= '<p>' . $introduce . '</p>' ;}

			// Phone
			if ( !empty($vars['phone']) ) { $out .= '<p id="ci-phone" class="ci-p">' . $phone . '</p>' ;}

			// Email
			if ( !empty($vars['email']) ) { $out .= '<p id="ci-email" class="ci-p"><a href="mailto:' . $email . '">' . $email . '</a></p>' ;}

			// Address
			if ( !empty($vars['address']) ) { $out .= '<p id="ci-address" class="ci-p">' . $address . '</p>' ;}

			// Name
			if ( !empty($vars['name']) ) { $out .= '<p id="ci-name" class="ci-p">' . $name . '</p>' ;}

			// After
			$out .= '</div>';
	
			echo $out;
	
	}



	// --  H E L P E R   F U N C T I O N  --------------------------------- //

	if ( !function_exists('bf_smart_multiwidget_update') ) :

		function bf_smart_multiwidget_update( $id_prefix, $options, $post, $sidebar, $option_name = '' ) {

			global $wp_registered_widgets;

			static $updated = false;
	
			$sidebars_widgets = wp_get_sidebars_widgets();

			if ( isset( $sidebars_widgets[$sidebar] ) ) :

				$this_sidebar =& $sidebars_widgets[$sidebar];

			else :

				$this_sidebar = array();

			endif;
			
			foreach ( $this_sidebar as $_widget_id ) :

				if ( preg_match( '/' . $id_prefix . '-([0-9]+)/i', $_widget_id, $match ) ) :

					$widget_number = $match[1];
					
					if ( !in_array( $match[0], $_POST['widget-id'] ) ) :

						unset( $options[$widget_number] );

					endif;

				endif;

			endforeach;
			
			if ( !empty($option_name) ) :

				update_option($option_name, $options);

				$updated = true;

			endif;
			
			return $options;
		}

	endif;



?>