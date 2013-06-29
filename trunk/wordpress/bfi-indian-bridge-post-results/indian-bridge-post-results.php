<?php
/*
Plugin Name: BFI Post Results
Plugin URI: http://bfi.net.in
Description: A plugin to accept post requests to post results of indian bridge events
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'BFI_Post_Results' ) ) {
	

	class BFI_Post_Results {
		
		public function __construct() {
			add_filter( 'xmlrpc_methods', array( &$this, 'add_xml_rpc_methods' ) );
		}
		
		
		/**
		 * Filter to add our custom XML-RPC methods
		 * 
		 * @param array $methods associative array of XMl-RPC method name to 
		 *        function callback
		 * @return array associative array of XMl-RPC method name to 
		 *         function callback
		 */
		public function add_xml_rpc_methods( $methods ) {
			$methods['indianbridge.postResults'] = array( &$this, 'bfi_post_results' );
			return $methods;
		}
		
		
		/**
		 * Post Results XML-RPC method.  This is invoked as 'indianbridge.postresults'
		 * and expects blog_id, username, password, and an associative array
		 * of containing "url", "title", "content" as XML-RPC parameters
		 * 
		 * @param array $params array of passed XML-RPC arguments
		 * 
		 * @return mixed IXR_Error, or string
		 */
		public function bfi_post_results( $params ) {
			
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $args );
			
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];
			$args     = $params[3];
			
			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $wp_xmlrpc_server->error;
			}
			
			// check for edit_posts capability (requires contributor role)
			// (obviously not required for this simple example, but just for demonstration purposes)
			if ( ! current_user_can( 'edit_pages' ) )
				return new IXR_Error( 403, __( 'User '.$username.' is not allowed to edit pages and so cannot run this software.' ) );
			
			do_action( 'xmlrpc_call', 'indianbridge.postResults' ); // patterned on the core XML-RPC actions
			
			// required parameters
			if ( empty( $args['pagePath'] ) ) return new IXR_Error( 500, __( "Missing parameter 'pagePath'" ) );
			//if ( empty( $args['pageName'] ) ) return new IXR_Error( 500, __( "Missing parameter 'pageName'" ) );
			if ( empty( $args['pageTitle'] ) ) return new IXR_Error( 500, __( "Missing parameter 'pageTitle'" ) );
			if ( empty( $args['pageContent'] ) ) return new IXR_Error( 500, __( "Missing parameter 'pageContent'" ) );
			
			$pagePath = $args['pagePath'];
			//$pageName = $args['pageName'];
			$pageTitle = $args['pageTitle'];
			$pageContent = $args['pageContent'];
			$pageTemplate = $args['pageTemplate'];
			
			// Check if page already exists
			$pageID = url_to_postid($pagePath);
			if (empty($pageID)) {
				// Does not exist. Try to create it.
				$parentPagePath = dirname($pagePath);
				$pageName = basename($pagePath);
				
				// Check if parent exists
				$parentPageID = url_to_postid($parentPagePath);
				if (empty($parentPageID)) {
					return new IXR_Error( 500, __( "Cannot find page at ".$parentPagePath) );
				}
				else {
					// Create new page
					$my_post = array(
						'post_title' => $pageTitle,
						'post_name' => $pageName,
						'post_content' => $pageContent,
						'post_status' => 'publish',
						'post_author' => $user->ID,
						'post_type' => 'page',
						'post_parent' => $parentPageID
					);			
					$pageID = wp_insert_post($my_post);	
					if($pageID && !empty($pageTemplate)) {
						update_post_meta($pageID, '_wp_page_template',  $pageTemplate);
					}
				}
			}
			else {
				// Already exists. Update it
				$my_post = array(
					'ID' => $pageID,
					'post_title' => $pageTitle,
					'post_content' => $pageContent
				);	
				// Update the post into the database
				wp_update_post( $my_post );					
			}
			return strval($pageID);
		}
		
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_post_results'] = new BFI_Post_Results();
}