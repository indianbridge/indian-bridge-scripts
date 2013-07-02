<?php
/*
Plugin Name: BFI Event to Post
Plugin URI: http://bfi.net.in
Description: A plugin to accept post requests to post results of indian bridge events
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'BFI_Event_To_Post' ) ) {
	

	class BFI_Event_To_Post {
		
		public function __construct() {
			$customPostType = 'ai1ec_event';
			//add_action('publish_'.$customPostType, array( $this, 'createPostFromEvent' ));
			//add_action('new_to_publish', array( $this, 'createPostFromEvent' ));		
			//add_action('draft_to_publish', array( $this, 'createPostFromEvent' ));		
			//add_action('pending_to_publish', array( $this, 'createPostFromEvent' ));	
			add_action('transition_post_status', array( $this, 'createPostFromEvent' ), 10, 3);			
		}
		function createPostFromEvent($new_status, $old_status, $post) {
			if($new_status=='publish' && $old_status == 'auto-draft' && $post->post_type == 'ai1ec_event') {
				$contentPrefix = '<a href="'.get_permalink($post->ID).'">See more details in the event page here.</a><br/>';
			$my_post = array();
			$my_post['post_title']    = $post->post_title;
			$my_post['post_content']  = $contentPrefix.$post->post_content;
			$my_post['post_status']   = 'draft';
			$my_post['post_author']   = $post->post_author;
			$my_post['post_category'] = array(get_cat_ID('Featured'),get_cat_ID('Tourneys'),get_cat_ID('Information'));		
			wp_insert_post($my_post);
			}
		}
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_event_to_post'] = new BFI_Event_To_Post();
}