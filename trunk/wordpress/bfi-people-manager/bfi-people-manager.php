<?php

/*
 Plugin Name: BFI People Manager
Plugin URI: http://bfi.net.in
Description: A plugin to manage people and state associations associated with BFI
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
License: GNU General Public License v3.0
License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if (!class_exists('BFI_People_Manager')) {


	class BFI_People_Manager {

		private $typeNames;
		private $stateTypeName;
		private $committeeTypeName;
		private $directorTypeName;
		private $potentialTypeName;
		private $stateFields;
		private $committeeFields;
		private $directorFields;
		private $potentialFields;
		private $stateCustomFields;
		private $committeeCustomFields;
		private $directorCustomFields;
		private $potentialCustomFields;
		private $views;
		public function __construct() {
			$this->stateTypeName = 'bfi_associations';
			$this->committeeTypeName = 'bfi_committee';
			$this->directorTypeName = 'bfi_director';
			$this->potentialTypeName = 'bfi_potential';
			$this->typeNames = array($this->stateTypeName => 'BFI State Association',
					$this->committeeTypeName => 'BFI Office Bearer',
					$this->directorTypeName => 'BFI Director',
					$this->potentialTypeName => 'BFI Potential Director');
			// Field Array
			$prefix = $this->stateTypeName.'_';
			$this->stateFields = array(
					'title' => 1,
					'image' => 6
			);
			$this->stateCustomFields = array(
					array(
							'label'=> 'President',
							'desc'	=> 'President of the State Association.',
							'id'	=> $prefix.'president',
							'type'	=> 'text',
							'index' => 5
					),
					array(
							'label'=> 'Secretary',
							'desc'	=> 'Secretary of the State Association.',
							'id'	=> $prefix.'secretary',
							'type'	=> 'text',
							'index' => 2
					),
					array(
							'label'=> 'Contact Email',
							'desc'	=> 'Email Contact for State Association',
							'id'	=> $prefix.'email',
							'type'	=> 'text',
							'index' => 4
					),
					array(
							'label'=> 'Contact Phone Number',
							'desc'	=> 'Contact phone number for State Association',
							'id'	=> $prefix.'phone',
							'type'	=> 'text',
							'index' => 3
					)

			);

			$prefix = $this->committeeTypeName.'_';
			$this->committeeFields = array (
					'title' => 0,
					'content' => 7,
					'image' => 6
			);
			$this->committeeCustomFields = array(
					array(
							'label'=> 'Designation',
							'desc'	=> 'Designation of Office Bearer.',
							'id'	=> $prefix.'designation',
							'type'	=> 'text',
							'index' => 2
					),
					array(
							'label'=> 'Address',
							'desc'	=> 'Address of Office Bearer.',
							'id'	=> $prefix.'address',
							'type'	=> 'textarea',
							'index' => 4
					),
					array(
							'label'=> 'Contact Email',
							'desc'	=> 'Email Contact for Office Bearer.',
							'id'	=> $prefix.'email',
							'type'	=> 'text',
							'index' => 5
					),
					array(
							'label'=> 'Contact Phone Number',
							'desc'	=> 'Contact phone number for Office Bearer.',
							'id'	=> $prefix.'phone',
							'type'	=> 'text',
							'index' => 3
					)

			);

			$prefix = $this->directorTypeName.'_';
			$this->directorFields = array (
					'title' => 1,
					'image' => 5
			);
			$this->directorCustomFields = array(
					array(
							'label'=> 'Region',
							'desc'	=> 'Region of Director.',
							'id'	=> $prefix.'region',
							'type'	=> 'text',
							'index' => 2
					),
					array(
							'label'=> 'Tournament Level',
							'desc'	=> 'Tournament Level of Director.',
							'id'	=> $prefix.'level',
							'type'	=> 'text',
							'index' => 3
					),
					array(
							'label'=> 'Contact Email',
							'desc'	=> 'Email Contact for Director.',
							'id'	=> $prefix.'email',
							'type'	=> 'text',
							'index' => 4
					),
					array(
							'label'=> 'Contact Phone Number',
							'desc'	=> 'Contact phone number for Director.',
							'id'	=> $prefix.'phone',
							'type'	=> 'text'
					),
					array(
							'label'=> 'Dues Paid?',
							'desc'  => 'Has this person paid his director renewal fees?.',
							'id'    => $prefix.'dues_paid',
							'type'  => 'checkbox',
							'index' => 6
					)

			);

			$prefix = $this->potentialTypeName.'_';
			$this->potentialFields = array (
					'title' => 1,
					'image' => 5
			);
			$this->potentialCustomFields = array(
					array(
							'label'=> 'Region',
							'desc'	=> 'Region of Director.',
							'id'	=> $prefix.'region',
							'type'	=> 'text',
							'index' => 2
					),
					array(
							'label'=> 'Tournament Level',
							'desc'	=> 'Tournament Level of Director.',
							'id'	=> $prefix.'level',
							'type'	=> 'text',
							'index' => 3
					),
					array(
							'label'=> 'Contact Email',
							'desc'	=> 'Email Contact for Director.',
							'id'	=> $prefix.'email',
							'type'	=> 'text',
							'index' => 4
					),
					array(
							'label'=> 'Contact Phone Number',
							'desc'	=> 'Contact phone number for Director.',
							'id'	=> $prefix.'phone',
							'type'	=> 'text'
					)
			);

			if (is_admin()) {
				$jsFilePath = 'jquery.imageLoader.js';
				$jsIdentifier = 'jquery_imageLoader_js';
				wp_register_script( $jsIdentifier, plugins_url($jsFilePath, __FILE__ ), array( 'jquery' ) );
				wp_enqueue_script($jsIdentifier);
			}

			register_activation_hook( __FILE__, array( $this, 'activate' ) );
			register_deactivation_hook(__FILE__, array( $this, 'deactivate' ));

			add_action( 'init', array($this, 'register_custom_post_types') );
			add_filter( 'post_updated_messages', array($this, 'my_updated_messages') );
			add_action('admin_menu', array($this, 'add_import_export'));
			add_action( 'add_meta_boxes', array($this,'add_custom_meta_box') );
			add_action('save_post', array($this,'save_custom_meta'));
			foreach($this->typeNames as $registerTypeName=>$typeName) {
				$shortcodeName = $registerTypeName.'_display';
				add_shortcode($shortcodeName, array($this, $shortcodeName));
			}
		}


		function deactivate() {
			$this->deleteFile('archive-bfi-default.php');
			$this->deleteFile('single-bfi-default.php');
			foreach($this->typeNames as $registerTypeName=>$typeName) {
				$this->deleteFile('archive-'.$registerTypeName.'.php');
				$this->deleteFile('single-'.$registerTypeName.'.php');
			}
		}

		function activate() {
			$this->copyFile('archive-bfi-default.php');
			$this->copyFile('single-bfi-default.php');
			foreach($this->typeNames as $registerTypeName=>$typeName) {
				$this->copyFile('archive-'.$registerTypeName.'.php');
				$this->copyFile('single-'.$registerTypeName.'.php');
			}
		}

		function deleteFile($fileName) {
			$destination = get_template_directory().DIRECTORY_SEPARATOR.$fileName;
			unlink($destination);
		}

		function copyFile($fileName) {
			$source = rtrim(plugin_dir_path(__FILE__),'/').DIRECTORY_SEPARATOR.'templates'.DIRECTORY_SEPARATOR.$fileName;
			$destination = get_template_directory().DIRECTORY_SEPARATOR.$fileName;
			copy($source,$destination);
		}

		function generic_shortcode_template($registerTypeName,$customFields) {
			$args=array(
					'post_type' => $registerTypeName,
					'post_status' => 'publish',
					'posts_per_page' => -1,
					'caller_get_posts'=> 1,
					'order' => 'ASC',
					'orderby' => 'title'
			);
			$query = new WP_Query( $args );
			$rootDir = get_template_directory_uri();
			$imagePath = '/images/icons/16/led-icons/';
			// The Loop
			if ( $query->have_posts() ) {
				echo '<div id="archive">';
				while ( $query->have_posts() ) {
					$query->the_post();
					$post_id = get_the_ID();
					echo '<div class="item">';
					$w='120';
					$h='120';
					$a = $theme_options['thumbs_crop_top']=='enable' ? '&a=t' : '';

					// Check featured image from URL
					$feat_img = get_post_meta($post_id, 'feat_img_value', true);

					if ($feat_img) :

					$src = $feat_img;
					$path = $src;

					else :

					// Get image source
					$src = wp_get_attachment_image_src(get_post_thumbnail_id($post_id), 'Full Size');
					$src = ($src) ? $src[0] : get_bloginfo('template_url').'/images/blog-thumb-240x240.jpg';
					$path = $src;

					endif;
					// Display image
					if ($src) :
					$out = '<a href="'.get_permalink().'"><img id="single-feat-img" class="t1 fl br3" src="' . get_bloginfo('template_url') . '/timthumb.php?src=' . $path . '&amp;h=' . $h . '&amp;w=' . $w . '&amp;zc=3&amp;q=90'. $a . '" width="' . $w . '" alt="' . get_the_title() . '" /></a>';
					endif;
					echo $out;
					echo '<div class="t1-right fr">';
					echo '<h3><a href="'.get_permalink().'">';
					the_title();
					edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' );
					echo '</a></h3>';
					foreach($customFields as $field=>$image) {
						$value = get_post_meta($post_id,$registerTypeName.'_'.$field,true);
						$imageURL = $rootDir.$imagePath.$image;
						if ($field == 'email') {
							$value = '<a href=mailto:'.$value.'>'.$value.'</a>';
						}
						if ($value == 'true') {
							$value = '<img src="'.$rootDir.$imagePath.'accept.png'.'"/>';
						}
						else if ($value == 'false') {
							$value = '<img src="'.$rootDir.$imagePath.'cross.png'.'"/>';
						}
						$out = '<div class="icon16" style="background-image:url('.$imageURL.') !important;">'.ucfirst($field).' : '.$value.' </div><br/>';
						echo $out;
					}
					echo '<div class="clear"></div><div class="h30"></div></div><div class="clear"></div>';
					echo '<p>'.get_the_excerpt().'<span class="btwrap"><a class="button" href="'.get_permalink().'"><span>'.__('Read More','pandathemes').'</span></a></span></p>';
					echo '<div class="clear"></div></div>';
				}
				echo '</div>';
			} else {
				echo '<h1>No State Association information found</h1>';
			}
				
			/* Restore original Post Data */
			wp_reset_postdata();
		}

		function bfi_associations_display($atts, $content = null) {
			$registerTypeName = 'bfi_associations';
			$customFields = array (
					'president' => 'user_business.png',
					'secretary' => 'user.png',
					'email' => 'email.png',
					'phone' => 'mobile_phone.png'
			);

			$this->generic_shortcode_template($registerTypeName,$customFields);
		}

		function bfi_committee_display($atts, $content = null) {
			$registerTypeName = 'bfi_committee';
			$customFields = array (
					'designation' => 'user.png',
					'address' => 'house.png',
					'email' => 'email.png',
					'phone' => 'mobile_phone.png'
			);

			$this->generic_shortcode_template($registerTypeName,$customFields);
		}

		function bfi_director_display($atts, $content = null) {
			$registerTypeName = 'bfi_director';
			$customFields = array (
					'region' => 'map.png',
					'level' => 'star_2.png',
					'email' => 'email.png',
					'phone' => 'mobile_phone.png',
					'dues_paid' => 'money.png'
			);

			$this->generic_shortcode_template($registerTypeName,$customFields);
		}

		function bfi_potential_display($atts, $content = null) {
			$registerTypeName = 'bfi_potential';
			$customFields = array (
					'region' => 'map.png',
					'level' => 'star_2.png',
					'email' => 'email.png',
					'phone' => 'mobile_phone.png'
			);

			$this->generic_shortcode_template($registerTypeName,$customFields);
		}

		// Add the Meta Box
		function add_custom_meta_box() {
			foreach($this->typeNames as $registerTypeName=>$typeName) {
				add_meta_box(
						$registerTypeName.'_custom_meta_box', // $id
						$typeName.' Information', // $title
						array($this,'show_'.$registerTypeName.'_custom_meta_box'), // $callback
						$registerTypeName, // $page
						'normal', // $context
						'high'); // $priority
			}

		}

		function save_custom_meta_specific($post_id,$custom_meta_fields) {
			// loop through fields and save the data
			foreach ($custom_meta_fields as $field) {
				$old = get_post_meta($post_id, $field['id'], true);
				$new = $_POST[$field['id']];
				if ($field['type'] == 'checkbox' && !$new) $new = 'false';
				if ($new && $new != $old) {
					update_post_meta($post_id, $field['id'], $new);
				} elseif ('' == $new && $old) {
					delete_post_meta($post_id, $field['id'], $old);
				}
			} // end foreach
		}

		function save_custom_meta($post_id) {

			// verify nonce
			if (!wp_verify_nonce($_POST['custom_meta_box_nonce'], basename(__FILE__)))
				return $post_id;
			// check autosave
			if (defined('DOING_AUTOSAVE') && DOING_AUTOSAVE)
				return $post_id;
			// check permissions
			if (!current_user_can('edit_posts', $post_id)) {
				return $post_id;
			}
			if ($this->stateTypeName == $_POST['post_type']) {
				$this->save_custom_meta_specific($post_id,$this->stateCustomFields);
			}
			else if ($this->committeeTypeName == $_POST['post_type']) {
				$this->save_custom_meta_specific($post_id,$this->committeeCustomFields);
			}
			else if ($this->directorTypeName == $_POST['post_type']) {
				$this->save_custom_meta_specific($post_id,$this->directorCustomFields);
			}
			else if ($this->potentialTypeName == $_POST['post_type']) {
				$this->save_custom_meta_specific($post_id,$this->potentialCustomFields);
			}

		}

		function show_custom_meta_box($custom_meta_fields) {
			global $post;
			// Use nonce for verification
			echo '<input type="hidden" name="custom_meta_box_nonce" value="'.wp_create_nonce(basename(__FILE__)).'" />';

			// Begin the field table and loop
			echo '<table class="form-table">';
			foreach ($custom_meta_fields as $field) {
				// get value of this field if it exists for this post
				$meta = get_post_meta($post->ID, $field['id'], true);
				// begin a table row with
				echo '<tr>
				<th><label for="'.$field['id'].'">'.$field['label'].'</label></th>
				<td>';
				switch($field['type']) {
					// case items will go here
					// text
					case 'text':
						echo '<input type="text" name="'.$field['id'].'" id="'.$field['id'].'" value="'.$meta.'" size="30" />
						<br /><span class="description">'.$field['desc'].'</span>';
						break;
					case 'textarea':
						echo '<textarea name="'.$field['id'].'" id="'.$field['id'].'" cols="60" rows="4">'.$meta.'</textarea>
						<br /><span class="description">'.$field['desc'].'</span>';
						break;
						// checkbox
					case 'checkbox':
						if ($meta == "false") $meta = null;
						echo '<input type="checkbox" value="true" name="'.$field['id'].'" id="'.$field['id'].'" ',$meta ? ' checked="checked"' : '','/>
						<label for="'.$field['id'].'">'.$field['desc'].'</label>';
						break;
					case 'select':
						echo '<select name="'.$field['id'].'" id="'.$field['id'].'">';
						foreach ($field['options'] as $option) {
							echo '<option', $meta == $option['value'] ? ' selected="selected"' : '', ' value="'.$option['value'].'">'.$option['label'].'</option>';
						}
						echo '</select><br /><span class="description">'.$field['desc'].'</span>';
						break;
						// radio
					case 'radio':
						foreach ( $field['options'] as $option ) {
							echo '<input type="radio" name="'.$field['id'].'" id="'.$option['value'].'" value="'.$option['value'].'" ',$meta == $option['value'] ? ' checked="checked"' : '',' />
							<label for="'.$option['value'].'">'.$option['label'].'</label><br />';
						}
						break;
						// checkbox_group
					case 'checkbox_group':
						foreach ($field['options'] as $option) {
							echo '<input type="checkbox" value="'.$option['value'].'" name="'.$field['id'].'[]" id="'.$option['value'].'"',$meta && in_array($option['value'], $meta) ? ' checked="checked"' : '',' />
							<label for="'.$option['value'].'">'.$option['label'].'</label><br />';
						}
						echo '<span class="description">'.$field['desc'].'</span>';
						break;
						// image
					case 'image':
						$image = get_template_directory_uri().'/images/image.png';
						echo '<span class="custom_default_image" style="display:none">'.$image.'</span>';
						if ($meta) {
							$image = wp_get_attachment_image_src($meta, 'medium');	$image = $image[0];
						}
						echo	'<input name="'.$field['id'].'" type="hidden" class="custom_upload_image" value="'.$meta.'" />
						<img src="'.$image.'" class="custom_preview_image" alt="" /><br />
						<input class="custom_upload_image_button button" type="button" value="Choose Image" />
						<small>&nbsp;<a href="#" class="custom_clear_image_button">Remove Image</a></small>
						<br clear="all" /><span class="description">'.$field['desc'].'</span>';
						break;
						// repeatable
					case 'repeatable':
						echo '<a class="repeatable-add button" href="#">+</a>
						<ul id="'.$field['id'].'-repeatable" class="custom_repeatable">';
						$i = 0;
						if ($meta) {
							foreach($meta as $row) {
								echo '<li><span class="sort hndle">|||</span>
								<input type="text" name="'.$field['id'].'['.$i.']" id="'.$field['id'].'" value="'.$row.'" size="30" />
								<a class="repeatable-remove button" href="#">-</a></li>';
								$i++;
							}
						} else {
							echo '<li><span class="sort hndle">|||</span>
							<input type="text" name="'.$field['id'].'['.$i.']" id="'.$field['id'].'" value="" size="30" />
							<a class="repeatable-remove button" href="#">-</a></li>';
						}
						echo '</ul>
						<span class="description">'.$field['desc'].'</span>';
						break;
				} //end switch
				echo '</td></tr>';
			} // end foreach
			echo '</table>'; // end table
		}

		function show_bfi_associations_custom_meta_box() {
			$this->show_custom_meta_box($this->stateCustomFields);
		}

		function show_bfi_committee_custom_meta_box() {
			$this->show_custom_meta_box($this->committeeCustomFields);
		}

		function show_bfi_director_custom_meta_box() {
			$this->show_custom_meta_box($this->directorCustomFields);
		}

		function show_bfi_potential_custom_meta_box() {
			$this->show_custom_meta_box($this->potentialCustomFields);
		}


		function add_import_export() {
			foreach($this->typeNames as $registerTypeName=>$typeName) {
				$view_hook_name = add_submenu_page( 'edit.php?post_type='.$registerTypeName,'Import and Export '.$typeName, 'Import & Export '.$typeName, 'edit_posts', 'import-export-'.$registerTypeName,array($this, 'import_export_callback'));
				$this->views[$view_hook_name.'_register'] = $registerTypeName;
				$this->views[$view_hook_name.'_type'] = $typeName;
			}
		}

		function import_export_callback() {
			$registerTypeName = $this->views[current_filter().'_register'];
			$typeName = $this->views[current_filter().'_type'];
			$this->import_export_forms($registerTypeName,$typeName);
		}


		function import_export_forms($registerTypeName,$typeName) {
			//must check that the user has the required capability
			if (!current_user_can('edit_posts'))   {
				wp_die( __('You do not have sufficient permissions to access this page.') );
			}
				
			// variables for the field and option names
			echo '<div id="update-status" class="updated"></div>';
			$dataField = 'state_associations_csv';
			$hidden_field_name = 'mt_submit_hidden';
			$callbackName = 'import_'.$registerTypeName;
			// See if the user has posted us some information
			// If they did, this hidden field will be set to 'I' or 'D'
			if( isset($_POST[ $hidden_field_name ]) && ($_POST[ $hidden_field_name ] == 'I' || $_POST[ $hidden_field_name ] == 'D')) {
					
				$text = '';
				if (isset($_POST[$dataField])) {
					$text = $_POST[$dataField];
				}
				if ($_POST[ $hidden_field_name ] == 'I') {
					$updatedHTML = $this->$callbackName($registerTypeName,$text);
				}
				else if ($_POST[ $hidden_field_name ] == 'D') {
				}
					
				echo '<div class="updated">';
				echo 'Finished : '.$updatedHTML;
				echo '</div>';
			}

			echo '<div class="wrap">';
			echo "<h2>" . __( 'Import '.$typeName, 'menu-test' ) . "</h2>";
			echo '<form name="form1" method="post" action="">';
			echo '<input type="hidden" name="'.$hidden_field_name.'" value="I">';
			echo '<span>CSV Text : </span><textarea name="'.$dataField.'" cols="60" rows="10"></textarea>';
			echo '<p class="submit"><input type="submit" name="Submit" class="button-primary" value="Import '.$typeName.'"/></p>';
			echo '</form></div>';
		}

		function import_bfi_associations($registerTypeName,$text) {
			return $this->import_data($registerTypeName,$text,$this->stateFields,$this->stateCustomFields);
		}
		function import_bfi_committee($registerTypeName,$text) {
			return $this->import_data($registerTypeName,$text,$this->committeeFields,$this->committeeCustomFields);
		}
		function import_bfi_director($registerTypeName,$text) {
			return $this->import_data($registerTypeName,$text,$this->directorFields,$this->directorCustomFields);
		}
		function import_bfi_potential($registerTypeName,$text) {
			return $this->import_data($registerTypeName,$text,$this->potentialFields,$this->potentialCustomFields);
		}
		
		function checkIfPostExists($registerTypeName,$title) {
			global $wpdb;
			
			$query = $wpdb->prepare(
					'SELECT ID FROM ' . $wpdb->posts . '
					WHERE post_title = %s
					AND post_type = %s',
					$title,$registerTypeName
			);
			$wpdb->query( $query );
			
			if ( $wpdb->num_rows ) {
				$post_id = $wpdb->get_var( $query );
				return $post_id;
			} else {
				return null;
			}			
		}

		function import_data($registerTypeName,$text,$fieldList,$custom_meta_fields) {
			$lines = explode(PHP_EOL,$text);
			$html = '<br/>';
			foreach($lines as $line) {
				$fields = explode("\t",$line);
				$fieldName = 'title';
				$title = array_key_exists($fieldName,$fieldList)?$fields[$fieldList[$fieldName]]:'';
				$fieldName = 'content';
				$content = array_key_exists($fieldName,$fieldList)?$fields[$fieldList[$fieldName]]:'';
				if (!empty($title)) {
					$post_id = $this->checkIfPostExists($registerTypeName,$title);
					$my_post = array(
							'post_title'    => $title,
							'post_content'  => $content,
							'post_status'   => 'publish',
							'post_type'      => $registerTypeName
					);
					
					$prefixText = '';
					
					if ($post_id == null) {
						$prefixText = "Added";
						// Insert the post into the database
						$post_id = wp_insert_post( $my_post );
					}
					else {
						$prefixText = "Updated";
						$my_post['ID'] = $post_id;
						wp_update_post($my_post);
					}
						
					if (!has_post_thumbnail($post_id)) {
						// check if featured image is present
						$fieldName = 'image';
						if (array_key_exists($fieldName,$fieldList)) {
							$filename = $fields[$fieldList[$fieldName]];
							$wp_filetype = wp_check_filetype($filename, null );
							$mime_type = $wp_filetype[type];
							$attachment = array(
									'post_mime_type' => $wp_filetype['type'],
									'post_title' => preg_replace('/\.[^.]+$/', '', basename($filename)),
									'post_name' => preg_replace('/\.[^.]+$/', '', basename($filename)),
									'post_content' => '',
									'post_parent' => $post_id,
									'post_excerpt' => $thumb_credit,
									'post_status' => 'inherit'
							);
							$attachment_id = wp_insert_attachment($attachment, $filename, $post_id);
							if($attachment_id != 0) {
								$attachment_data = wp_generate_attachment_metadata($attachment_id, $filename);
								wp_update_attachment_metadata($attachment_id, $attach_data);
								set_post_thumbnail($post_id,$attachment_id);
							}
						}
					}
						
					// All meta fields
					foreach ($custom_meta_fields as $field) {
						if (array_key_exists('index',$field)) {
							update_post_meta($post_id,$field['id'],$fields[$field['index']]);
						}
					}

					$html .= $prefixText.' '.$title.'<br/>';
				}
				else {
					$html .= 'Empty title at index '.$fieldList['title'].' in '.$line.'<br/>';
				}
			}
			return $html;
		}


		function register_custom_post_types() {
			foreach($this->typeNames as $registerTypeName=>$typeName) {
				$pluralTypeName = $typeName.'s';
				$labels = array(
						'name'               => _x( $pluralTypeName, 'post type general name' ),
						'singular_name'      => _x( $typeName, 'post type singular name' ),
						'add_new'            => _x( 'Add New', $typeName ),
						'add_new_item'       => __( 'Add New '.$typeName ),
						'edit_item'          => __( 'Edit '.$typeName ),
						'new_item'           => __( 'New '.$typeName ),
						'all_items'          => __( 'All '.$pluralTypeName ),
						'view_item'          => __( 'View '.$typeName ),
						'search_items'       => __( 'Search '.$pluralTypeName ),
						'not_found'          => __( 'No '.$pluralTypeName.' found' ),
						'not_found_in_trash' => __( 'No '.$pluralTypeName.' found in the Trash' ),
						'parent_item_colon'  => '',
						'menu_name'          => $pluralTypeName
				);
				$args = array(
						'labels' => $labels,
						'description'   => 'Holds our '.$pluralTypeName.' and '.$typeName.' specific data',
						'public' => true,
						'menu_position' => 15,
						'supports' => array( 'title', 'editor', 'thumbnail' ),
						'taxonomies' => array( '' ),
						'has_archive' => true
				);
				register_post_type($registerTypeName,$args);
			}
		}


		function my_updated_messages( $messages) {
			foreach($this->typeNames as $registerTypeName=>$typeName) {
				$pluralTypeName = $typeName.'s';
				global $post, $post_ID;
				$messages[$registerTypeName] = array(
						0 => '',
						1 => sprintf( __($typeName.' updated. <a href="%s">View '.$typeName.'</a>'), esc_url( get_permalink($post_ID) ) ),
						2 => __('Custom field updated.'),
						3 => __('Custom field deleted.'),
						4 => __($typeName.' updated.'),
						5 => isset($_GET['revision']) ? sprintf( __($typeName.' restored to revision from %s'), wp_post_revision_title( (int) $_GET['revision'], false ) ) : false,
						6 => sprintf( __($typeName.' published. <a href="%s">View '.$typeName.'</a>'), esc_url( get_permalink($post_ID) ) ),
						7 => __($typeName.' saved.'),
						8 => sprintf( __($typeName.' submitted. <a target="_blank" href="%s">Preview '.$typeName.'</a>'), esc_url( add_query_arg( 'preview', 'true', get_permalink($post_ID) ) ) ),
						9 => sprintf( __($typeName.' scheduled for: <strong>%1$s</strong>. <a target="_blank" href="%2$s">Preview '.$typeName.'</a>'), date_i18n( __( 'M j, Y @ G:i' ), strtotime( $post->post_date ) ), esc_url( get_permalink($post_ID) ) ),
						10 => sprintf( __($typeName.' draft updated. <a target="_blank" href="%s">Preview '.$typeName.'</a>'), esc_url( add_query_arg( 'preview', 'true', get_permalink($post_ID) ) ) ),
				);
			}
			return $messages;
		}

	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_people_manager'] = new BFI_People_Manager();
}