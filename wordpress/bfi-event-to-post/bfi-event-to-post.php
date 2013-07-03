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
		private $bfi_event_to_post_meta_key;
		public function __construct() {
			$customPostType = 'ai1ec_event';
			$this->bfi_event_to_post_meta_key = 'bfi_event_to_post_meta_key';
			add_action( 'save_post', array( $this, 'postFromEventHook' ));
			//add_action( 'publish_post', array( $this, 'postFromEventHook' ));
			add_action( 'transition_post_status', array( $this, 'postFromEventTransitionHook'), 10, 3 );
		}
		
		function postFromEventTransitionHook($new_status,$old_status,$event) {

			if ($event->post_type == 'ai1ec_event' && !wp_is_post_autosave($event) && $new_status == 'publish') {
				$this->writeDebug('Post transition publish : '.$event->ID);
				$this->createEventFromPost($event,$old_status);
			}	
		}
		
		function postFromEventHook($event_id) {
			$this->writeDebug('Saving Event : '.$event_id);
			$event = get_post($event_id);
			if (!wp_is_post_autosave($event) && $event->post_type == 'ai1ec_event') {
				$this->updateEvent($event);
			}
		}

		function updateEvent($event) {

			$post_id = intval(get_post_meta($event->ID,$this->bfi_event_to_post_meta_key,true));
			if ($post_id == 0) return;
			$my_post = array();
			$my_post['post_title']    = $event->post_title;
			$my_post['post_content']  = $this->getContentPrefix($event->ID).$event->post_content;
			$my_post['post_author']   = $event->post_author;	
			$my_post['ID'] = $post_id;
			wp_update_post($my_post);
		}
		
		function createEventFromPost($event,$old_status) {
			$post_id = intval(get_post_meta($event->ID,$this->bfi_event_to_post_meta_key,true));
			if ($post_id == 0 && $old_status=='publish') {
				$this->writeDebug('For event '.$event->ID.' post_id is '.$post_id.' and old status is '.$old_status.' , so returning');
				return;
			}
			$my_post = array();
			$my_post['post_title']    = $event->post_title;
			$my_post['post_content']  = $this->getContentPrefix($event->ID).$event->post_content;
			$my_post['post_author']   = $event->post_author;
			if ($post_id == 0) {
				// Related post does not exist create
				$post_id = $this->createPost($my_post,$event->ID);
				$this->writeDebug('For event '.$event->ID.' created post_id '.$post_id);
			}
			else {
				if(get_post($post_id)) {
					// Related Post exists, so just update
					$my_post['ID'] = $post_id;
					wp_update_post($my_post);
					$this->writeDebug('For event '.$event->ID.' updated post_id '.$post_id);
				}
				else { 
					// Post probably  deleted
					//$post_id = $this->createPost($my_post,$event->ID);
					$this->writeDebug('This should not happen but For event '.$event->ID.' post_id '.$post_id. ' returning');
					return;
				}
			}
			if (has_post_thumbnail($event->ID) && !has_post_thumbnail($post_id)) {
				$thumbnail_id = get_post_thumbnail_id($event->ID);
				set_post_thumbnail($post_id, $thumbnail_id );
			}			
		}

		function writeDebug($text) {
			/*global $wpdb;
			$tableName = "wp_debug";
			$wpdb->insert( $tableName,array( 'text' => $text),array( '%s') );*/
		}
		
		
		function createPost($my_post,$eventID) {
			$my_post['post_status']   = 'draft';
			$my_post['post_category'] = array(get_cat_ID('Featured'),get_cat_ID('Tourneys'),get_cat_ID('Information'));
			$post_id = wp_insert_post($my_post);
			if ($post_id != 0) {
				add_post_meta($eventID,$this->bfi_event_to_post_meta_key,$post_id,true);
			}
			return $post_id;
		}

		function getContentPrefix($eventID) {
			global $wpdb;
			$table_name = $wpdb->prefix . 'ai1ec_events';
			$sql = "SELECT start,end,venue,country,address,city,province,postal_code,contact_name,contact_phone,contact_email,contact_url FROM $table_name WHERE post_id = %d";
			$query = $wpdb->prepare($sql,$eventID);
			$event = $wpdb->get_row( $query );	
			$contentPrefix = '';
			if ($event != null)	{	
				$contentPrefix .= 'Dates : '.substr($event->start,0,10).' to '.substr($event->end,0,10).' <br/>';
				$contentPrefix .= 'Venue : '.$event->venue.' : '.$event->address.' <br/>';
				$contentPrefix .= 'Website : '.$event->contact_url.' <br/>';
				$contentPrefix .= 'Contact : '.$event->contact_name.', Ph: '.$event->contact_phone.', Email: '.$event->contact_email.' <br/>';
			}	
			else $this->writeDebug('Getting event is null');
			$contentPrefix .= '<a href="'.get_permalink($eventID).'">See more details in the event page here.</a><br/>';
			return $contentPrefix;
		}
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_event_to_post'] = new BFI_Event_To_Post();
}