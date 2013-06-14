<?php



// *********************************************
//
// 	A J A X   H A N D L E R
//
// *********************************************



function iajax_save_function() {

	if ( !current_user_can('edit_theme_options') ) die('-1');

	global $post;


	if($_POST['type'] == 'save') :

		$data = $_POST['data'];
		parse_str($data, $saved);


		// delete it first to clean meta
		for ( $i=1; $i<=$saved['postsum_ajax_before']; $i++ ) {

			$custom_meta_fields_delete[] = 'i'.$i;

		}


		foreach ( $custom_meta_fields_delete as $custom_meta_field ) :

			delete_post_meta($saved['postid_ajax'], $custom_meta_field);

		endforeach;

		
		// save post meta
		for ( $i=1; $i<=$saved['postsum_ajax']; $i++ ) {

			$custom_meta_fields[] = 'i'.$i;

		}


		foreach ( $custom_meta_fields as $custom_meta_field ) :

			if(isset($saved[$custom_meta_field]) && $saved[$custom_meta_field] != ""){

				update_post_meta($saved['postid_ajax'], $custom_meta_field, $saved[$custom_meta_field]);

			} else if($saved[$custom_meta_field] == "") {

				delete_post_meta($saved['postid_ajax'], $custom_meta_field, $saved[$custom_meta_field]);

			}

		endforeach;

		
		update_post_meta($saved['postid_ajax'], 'postsum_ajax', $saved['postsum_ajax']);
		
		echo 'save_success';

		die;


	endif;

}

add_action('wp_ajax_iajax_save', 'iajax_save_function');



// *********************************************
//
// 	A J A X   C U S T O M   S O R T A B L E
//
// *********************************************



function iajax_save_function_cs() {
	
	if ( !current_user_can('edit_theme_options') ) die('-1');

	global $post;
	

	if($_POST['type'] == 'save') :

		$data = $_POST['data'];
		parse_str($data, $saved);
		
		$fields = explode(',',$_POST['prefix']);
		

		foreach ($fields as $field) :

			
			if($saved[$_POST['sumBeforeID']]) :

				// delete it first to clean meta
				for ( $i=1; $i<=$saved[$_POST['sumBeforeID']]; $i++ ){
					$custom_meta_fields_delete[] = $field.$i;
				}

				foreach ( $custom_meta_fields_delete as $custom_meta_field ) :
					delete_post_meta($saved['postid_ajax'], $custom_meta_field);
				endforeach;

			endif;


			// save post meta
			$maxi = $saved[$_POST['sumID']] ? $saved[$_POST['sumID']] : 1;


			for ( $i=1; $i<=$maxi; $i++ ){
				$custom_meta_fields[] = $field.$i;
			}


			foreach ( $custom_meta_fields as $custom_meta_field ) :

				if(isset($saved[$custom_meta_field]) && $saved[$custom_meta_field] != ""){

					update_post_meta($saved['postid_ajax'], $custom_meta_field, $saved[$custom_meta_field]);

				} else if($saved[$custom_meta_field] == "") {

					delete_post_meta($saved['postid_ajax'], $custom_meta_field, $saved[$custom_meta_field]);

				}

			endforeach;


		endforeach;
		
		update_post_meta($saved['postid_ajax'], $_POST['sumID'], $saved[$_POST['sumID']]);

		
		echo 'save_success';

		die;


	endif;

}

add_action('wp_ajax_iajax_save_cs', 'iajax_save_function_cs');

?>