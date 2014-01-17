<?php

	global $panda_sb;
	$qty = $panda_sb['additional_sidebars'];

	// DEFAULT SIDEBAR
	if ( function_exists('register_sidebar') )
	register_sidebars(1,array('name' => 'Default Sidebar',
		'before_widget'		=> '<div class="widget">',
		'after_widget'		=> '</div>',
		'before_title'		=> '<h5>',
		'after_title'		=> '</h5>'
	));

	// LEFT THIN SIDEBAR
	if ( function_exists('register_sidebar') )
	register_sidebars(1,array('name' => 'Sidebar Left',
		'before_widget'		=> '<div class="widget">',
		'after_widget'		=> '</div>',
		'before_title'		=> '<h5>',
		'after_title'		=> '</h5>'
	));

	// RIGHT THIN SIDEBAR
	if ( function_exists('register_sidebar') )
	register_sidebars(1,array('name' => 'Sidebar Right',
		'before_widget'		=> '<div class="widget">',
		'after_widget'		=> '</div>',
		'before_title'		=> '<h5>',
		'after_title'		=> '</h5>'
	));

	// FOOTER SIDEBARS
	if ( function_exists('register_sidebar') )
	register_sidebars(4,array('name' => 'Footer Sidebar %d',
		'before_widget'		=> '<div class="f-widget">',
		'after_widget'		=> '</div>',
		'before_title'		=> '<h5>',
		'after_title'		=> '</h5>'
	));

	// BUDDY SIDEBAR // LOGGED IN
	if ( function_exists('register_sidebar') )
	register_sidebars(1,array('name' => 'Buddy Sidebar - logged in',
		'before_widget'		=> '<div class="bp-widget">',
		'after_widget'		=> '</div>',
		'before_title'		=> '<h5>',
		'after_title'		=> '</h5>'
	));

	// BUDDY SIDEBAR // LOGGED OUT
	if ( function_exists('register_sidebar') )
	register_sidebars(1,array('name' => 'Buddy Sidebar - logged out',
		'before_widget'		=> '<div class="bp-widget">',
		'after_widget'		=> '</div>',
		'before_title'		=> '<h5>',
		'after_title'		=> '</h5>'
	));

	// ADDTIONAL SIDEBARS
	if ( function_exists('register_sidebar') )
	register_sidebars($qty,array('name' => 'Side %d',
		'before_widget'	=> '<div class="widget">',
		'after_widget'		=> '</div>',
		'before_title'		=> '<h5>',
		'after_title'		=> '</h5>'
	));



	// A R R A Y S

		$new_meta_boxes = 
	
			array(
	
				"sidebar" => array(
	
					"name"			=> "sidebar",
					"std"			=> "",
					"title"			=> __("Please, select",'pandathemes'),
					"description"	=> __("You can set an unique sidebar to each page or post. <a href=\"admin.php?page=sidebars\">Create new sidebar</a>",'pandathemes')
				)
	
			);
	
		$ext_feat_image = 
	
			array(
	
				"feat_image" => array(
	
					"name"			=> "feat_img",
					"std"			=> "",
					"title"			=> __("Enter an URL",'pandathemes'),
					"description"	=>
						'<a href="#" class="pa-toggle">' .
						__('How to use?','pandathemes') .
						'</a></p><div id="pa-toggle-body" class="none">' . 
						__('If you need to set a featured image from URL, please, paste an image path. Take a look a list of allowed websites:') .
						'<ol>
							<li><a target="_blank" href="http://flickr.com">flickr.com</a></li>
							<li><a target="_blank" href="http://staticflickr.com">staticflickr.com</a></li>
							<li><a target="_blank" href="http://picasa.com">picasa.com</a></li>
							<li><a target="_blank" href="http://tinypic.com">tinypic.com</a></li>
							<li><a target="_blank" href="http://photobucket.com">photobucket.com</a></li>
							<li><a target="_blank" href="http://imageshack.us">imageshack.us</a></li>
							<li><a target="_blank" href="http://img.youtube.com">img.youtube.com</a></li>
						</ol></div>'
				)
	
			);



	// F E A T   I M A G E   P A N E L

		function ext_feat_image() {

			// ADD CSS STYLE
			echo '
				<style type="text/css">
					#del_feat_img {
						display:inline-block;
						margin:4px 0 0 3px;
					}
				</style>
			';

			// FOR EACH LOOP
			global $post, $ext_feat_image, $qty;
	
			foreach ( $ext_feat_image as $ext_feat ) {
	
				echo' <input type="hidden" name="' . $ext_feat['name'] . '_noncename" id="' . $ext_feat['name'] . '_noncename" value="' . wp_create_nonce( plugin_basename(__FILE__) ) . '" />';
	
					$ext_feat_value = get_post_meta($post->ID, $ext_feat['name'].'_value', true);
	
					if ( $ext_feat_value == '' )
	
						$ext_feat_value = $ext_feat['std'];
	
						// POST IMAGE
			?>

						<table class="pandameta">
							<tr>
								<td><p><?php echo '<input id="del_feat_img_input" style="width:90%; vertical-align:top;" type="text" name="' . $ext_feat['name'] . '_value" value="' . $ext_feat_value . '" size="" /><a href="#" id="del_feat_img"><img src="' . get_bloginfo('template_url') . '/images/icons/16/led-icons/cross.png"></a><span style="font-size:11px;">' . $ext_feat['description'] . '</span>';?></p></td>
							</tr>
						</table>
					<?php
			}

			// ADD SHORT JS + CSS ?>

				<script type='text/javascript'>

					var fi = jQuery.noConflict();

						// Remove an image
						fi('#del_feat_img').click(function() {

							fi('#del_feat_img_input').val('');
							fi('#feat_img').animate({height: 0, opacity: 0}, 750, function(){ fi(this).remove(); });

							return false;

						});

						// Toggle
						fi('.pa-toggle').click(
							function(){
								fi(this).remove();
								fi('#pa-toggle-body').removeClass('none');
								return false;
							}
						);

				</script>

				<style type="text/css">
					.none {
						display:none !important;
						}
				</style>

			<?php
	
		}



	// S I D E B A R   P A N E L

		function new_meta_boxes() {
	
			// ADD CSS STYLE
			echo '
				<style type="text/css">
					.pandameta {			width:92%; margin:0 auto; }
					.pandameta p span {		display:block; padding:5px 0 0 6px !important; color:#666; }
				</style>
			';
	
			// FOR EACH LOOP
			global $post, $new_meta_boxes, $qty;

			foreach ( $new_meta_boxes as $meta_box ) {
	
				echo' <input type="hidden" name="' . $meta_box['name'] . '_noncename" id="' . $meta_box['name'] . '_noncename" value="' . wp_create_nonce( plugin_basename(__FILE__) ) . '" />';
	
					$meta_box_value = get_post_meta($post->ID, $meta_box['name'].'_value', true);
	
					if ( $meta_box_value == '' )
	
						$meta_box_value = $meta_box['std'];
	
						// SELECT THE SIDEBAR ?>
						<table class="pandameta">
							<tr>
								<td>
									<p>
										<select name="<?php echo $meta_box['name']; ?>_value" id="<?php echo $meta_box['name']; ?>_value">
											<option value="Default Sidebar"<?php if ( $meta_box_value == "Default Sidebar" ) { echo 'selected'; } ?>><?php _e('Default Sidebar','pandathemes') ?>&nbsp;</option>
											<option value="Buddy Sidebar - logged in"<?php if ( $meta_box_value == "Buddy Sidebar - logged in" ) { echo 'selected'; } ?>><?php _e('Buddy Sidebar - logged in','pandathemes') ?>&nbsp;</option>
											<option value="Buddy Sidebar - logged out"<?php if ( $meta_box_value == "Buddy Sidebar - logged out" ) { echo 'selected'; } ?>><?php _e('Buddy Sidebar - logged out','pandathemes') ?>&nbsp;</option>
											<option value="No sidebar"<?php if ( $meta_box_value == "No sidebar" ) { echo 'selected'; } ?>><?php _e('No sidebar','pandathemes') ?></option>
											<?php
												for ( $s = 1; $s < ($qty + 1); $s++ ) {
	
													$selected = ($meta_box_value == 'Side '.$s) ? 'selected' : '';
	
													echo '<option value="Side ' . $s . '"' . $selected.'>' . __('Side','pandathemes') . ' ' . $s . '</option>';
	
												};
											?>
										</select>
										<span style="font-size:11px;"><?php echo $meta_box['description']; ?></span>
									</p>
								</td>
							</tr>
						</table>
					<?php ;
	
			}
	
		}



	// R E G I S T E R   M E T A B O X E S

		function create_meta_box() {

			global $new_meta_boxes;

			if ( function_exists('add_meta_box') ) :

				//if ( current_user_can('administrator') || current_user_can('editor') || !current_user_can('author') && !current_user_can('contributor') ) :

				global $user;

				if ( $user->has_cap('edit_pages') ) :

					// External Featured image
					add_meta_box( 'ext-feat-image', 'Featured Image from URL', 'ext_feat_image', 'post', 'side', 'low' );

					// Custom sidebar
					add_meta_box( 'new-meta-boxes', 'Custom sidebar', 'new_meta_boxes', 'post', 'side', 'low' );
					add_meta_box( 'new-meta-boxes', 'Custom sidebar', 'new_meta_boxes', 'page', 'side', 'low' );

				endif;

			endif;

		}



	// S A V E   D A T A

		function save_postdata( $post_id ) {
		
			global $post, $new_meta_boxes, $ext_feat_image;



				// for $new_meta_boxes
				foreach ( $new_meta_boxes as $meta_box ) :


					// VERIFY 
					if ( !wp_verify_nonce( $_POST[$meta_box['name'].'_noncename'], plugin_basename(__FILE__) ) ) :

						return $post_id;

					endif;


					if ( $_POST['post_type'] == 'page' ) :

						if ( !current_user_can( 'edit_page', $post_id ) ) :

							return $post_id;

						endif;

					else :

						if ( !current_user_can( 'edit_post', $post_id ) ) :

							return $post_id;

						endif;

					endif;


					$data = $_POST[$meta_box['name'].'_value'];


					if ( get_post_meta( $post_id, $meta_box['name'] . '_value') == '' ) :

						add_post_meta( $post_id, $meta_box['name'] . '_value', $data, true );

					elseif ( $data != get_post_meta($post_id, $meta_box['name'] . '_value', true ) ) :

						update_post_meta( $post_id, $meta_box['name'] . '_value', $data );

					elseif( $data == '' ) :

						delete_post_meta( $post_id, $meta_box['name'] . '_value', get_post_meta( $post_id, $meta_box['name'] . '_value', true ) );

					endif;


				endforeach;



				// for $ext_feat_image
				foreach($ext_feat_image as $ext_feat) {
				
					// VERIFY 
					if ( !wp_verify_nonce( $_POST[$ext_feat['name'].'_noncename'], plugin_basename(__FILE__) ) ) {
				
						return $post_id;
				
					}
				
					if ( 'page' == $_POST['post_type'] ) {
				
						if ( !current_user_can( 'edit_page', $post_id ) )
				
							return $post_id;
				
					} else {
				
						if ( !current_user_can( 'edit_post', $post_id ) )
				
							return $post_id;
					}
				
					$data = $_POST[$ext_feat['name'].'_value'];
				
			
					if ( get_post_meta( $post_id, $ext_feat['name'] . '_value') == '' )
			
						add_post_meta( $post_id, $ext_feat['name'] . '_value', $data, true );
			
					elseif ( $data != get_post_meta($post_id, $ext_feat['name'] . '_value', true ) )
			
						update_post_meta( $post_id, $ext_feat['name'] . '_value', $data );
			
					elseif( $data == '' )
			
						delete_post_meta( $post_id, $ext_feat['name'] . '_value', get_post_meta( $post_id, $ext_feat['name'] . '_value', true ) );
			
				}



		}
		
		add_action('admin_menu', 'create_meta_box');
		add_action('save_post', 'save_postdata');


?>