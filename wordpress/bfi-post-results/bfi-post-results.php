<?php
/*
Plugin Name: BFI Post Results
Plugin URI: http://bfi.net.in
Description: A plugin to accept post requests to post results of indian bridge events
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: � 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'BFI_Post_Results' ) ) {
	

	class BFI_Post_Results {
		
		public function __construct() {
			add_filter( 'xmlrpc_methods', array( $this, 'add_xml_rpc_methods' ) );
		}
		
		public function add_xml_rpc_methods( $methods ) {
			$methods['indianbridge.postResults'] = array( $this, 'bfi_post_results' );
			$methods['bfi.addTourney'] = array($this,'bfi_addTourney');
			$methods['bfi.getTourneys'] = array($this,'bfi_getTourneys');
			return $methods;
		}
		
		private function validateCredentials($params) {
			$parameterNames = array("username","password");
			$parameters = $this->checkParameters($params, $parameterNames);	
			global $wp_xmlrpc_server;
			$username = $parameters['username'];
			$password = $parameters['password'];
			// verify credentials
			if (!$user = $wp_xmlrpc_server -> login($username, $password)) {
				throw new Exception($wp_xmlrpc_server -> error -> message);
			}
			// Check if user is allowed to manage masterpoints
			if (!current_user_can('edit_pages')) {
				throw new Exception("User $username is not authorized to edit or create tourney pages.");
			}
			return $this->createSuccessMessage("Validated $username Successfully.");
		}	
		
		private function checkParameters($params,$parameterNames) {
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server -> escape($params);
			$parameters = $params;
			foreach ($parameterNames as $parameterName) {
				if (!array_key_exists($parameterName, $parameters)) {
					throw new Exception("Missing parameter $parameterName");
				}
				if (!$parameters[$parameterName]) {
					throw new Exception("Invalid value $parameters[$parameterName] for parameter $parameterName");
				}
			}	
			return $parameters;					
		}
		
		private function bfi_getPageInfo($page) {
			return array('id'=>$page->ID,'title'=>$page->post_title,'url'=>get_permalink($page->ID),'directory'=>get_page_uri($page->ID));
		}
		
		public function bfi_getTourneys($params) {
			try {
				// Check credentials first
				$this->validateCredentials($params);	
				$ignorePageIDs = array(172);
				$expandPageIDs = array(1464,893);
				$pages = get_pages( array( 'child_of' => 171,  'parent'=>171 ) );	
				$content = array();	
				foreach($pages as $page) {
					if (!in_array($page->ID, $ignorePageIDs,TRUE) && !in_array($page->ID, $expandPageIDs,TRUE)) {
						$content[] = $this->bfi_getPageInfo($page);
					}
				}	
				foreach($expandPageIDs as $expandPage) {
					$pages = get_pages( array( 'child_of' => $expandPage,  'parent'=>$expandPage ) );		
					foreach($pages as $page) {
						$content[] = $this->bfi_getPageInfo($page);
					}				
				}
				return $this->createSuccessMessage('Success',$content);
			}
			catch (Exception $ex) {
				return $this->createExceptionMessage($ex);
			}	
		}

		public function createMessage($error,$message,$content) {
			$return_value = array("error"=>$error,"message"=>__($message),"content"=>$content);
			return json_encode($return_value);
		}

		public function createErrorMessage($message, $content=array()) {
			return $this->createMessage(true,$message,$content);
		}
		
		private function createExceptionMessage($ex) {
			return $this->createErrorMessage($ex->getMessage());
		}			

		public function createSuccessMessage($message,$content=array()) {
			return $this->createMessage(false,$message,$content);
		}
		
		private function bfi_checkParentPageValidity($page_id) {
			$tourneys_page_id = 171;
			if (empty($page_id)) {
				throw new Exception("'parentPageID' parameter cannot be empty");
			}
			if (!is_numeric($page_id)) {
				throw new Exception("parentPageID : $page_id has to be numeric id of tourney page.");
			}
			$page_id = intval($page_id);
			if ($page_id === 0) {
				throw new Exception("parentPageID : $page_id is not a valid integer id of page.");
			}
			$page = get_post($page_id);
			if ($page == null) {
				throw new Exception("Page with parentPageID : $page_id does not exist.");
			}
			if ($page->post_status !== 'publish') {
				throw new Exception("Page with parentPageID : $page_id does not have published status.");
			}
			// Check if it is child of tourneys
			$ancestors = get_post_ancestors($page_id);
			if (!$ancestors || !in_array($tourneys_page_id,$ancestors,TRUE)) {
				throw new Exception("Page with parentPageID : $page_id is not a child page of Tourneys page.");
			}
											
		}
		
		private function bfi_createYearPage($page_id, $tourneyYear) {
			if (empty($tourneyYear)) {
				throw new Exception("'tourneyYear' parameter cannot be empty");
			}
			if (!is_numeric($tourneyYear)) {
				throw new Exception("tourneyYear : $tourneyYear has to be numeric year.");
			}		
			$tourneyYear = intval($tourneyYear);
			$today = getdate();
			$currentYear = intval($today['year']);	
			if($tourneyYear !== $currentYear && $tourneyYear !== $currentYear+1) {
				throw new Exception("tourneyYear : $tourneyYear has to be current year ($currentYear) or next year only.");
			}	
			$pageName = 'y'.$tourneyYear;
			$uri = get_page_uri($page_id).'/'.$pageName;
			$pageExists = get_page_by_path($uri);
			if ($pageExists != null) {
				//throw new Exception("$uri already exists. Year Page and sub pages can be created only once using this interface.");
				return;
			}
			$page_template = 'page-tourney.php';
			$page_template_meta = '_wp_page_template';
			$post = array(
				'comment_status' => 'closed',
				'ping_status'    => 'closed',
				'post_content'   => '[subpages]',
				'post_parent'    => $page_id,
				'post_status'    => 'publish',
				'post_name'      => $pageName,
				'post_title'     => ''.$tourneyYear,
				'post_type'      => 'page'
			);  	
			$rootPage_id = wp_insert_post($post,true);	
			if (is_wp_error($rootPage_id)) {
				throw new Exception("Unable to create page at $uri because".$rootPage_id->get_error_message().PHP_EOL.'Subpages will not be created.');
			}
			update_post_meta( $rootPage_id, $page_template_meta, $page_template );
			$path = get_page_uri($page_id).DIRECTORY_SEPARATOR.$tourneyYear;
			$this->bfi_createDirectory($path);
			$this->bfi_createDirectory($path.DIRECTORY_SEPARATOR.'bulletins');
			$this->bfi_createDirectory($path.DIRECTORY_SEPARATOR.'results');
			return $rootPage_id;
		}

		private function bfi_createSubPage($parent_page_id,$post_title,$post_content) {
			$post = array(
				'comment_status' => 'closed',
				'ping_status'    => 'closed',
				'post_content'   => $post_content,
				'post_parent'    => $parent_page_id,
				'post_status'    => 'publish',
				'post_title'     => $post_title,
				'post_type'      => 'page'
			);  	
			$page_template = 'page-tourney.php';
			$page_template_meta = '_wp_page_template';	
			$newPage_id = wp_insert_post($post,true);	
			if (is_wp_error($newPage_id)) {
				throw new Exception("Unable to create page at $uri because".$newPage_id->get_error_message());
			}
			else {
				update_post_meta( $newPage_id, $page_template_meta, $page_template );		
			}	
			return "$uri created.";
		}
		
		private function bfi_createDirectory($path) {
			$replaced_path = str_replace('/', DIRECTORY_SEPARATOR, $path);
			if (file_exists($replaced_path)) return;
			$this->bfi_createDirectory(dirname($replaced_path));
			mkdir($replaced_path);
			return;
		}
		
		private function bfi_childPageExists($posts,$title) {
			foreach($posts as $post) {
				if ($post->post_title === $title) return true;
			}
			return false;
		}
		
		public function bfi_addTourney($params) {
			try {
				// Check credentials first
				$this->validateCredentials($params);	

				do_action( 'xmlrpc_call', 'bfi.addTourney' ); // patterned on the core XML-RPC actions			
				
				// Parse the parameters	
				$content = json_decode($params['content'],true);
				//return print_r($content,true);
				$page_id = $content['parentPageID'];
				//return print_r($content['parentPageID'],true);
				$this->bfi_checkParentPageValidity($page_id);

				// Check if root page already exists or create it
				$tourneyYear = $content['tourneyYear'];
				$parent_page_id = $this->bfi_createYearPage($page_id, $tourneyYear);
				$posts = get_posts(array('post_type' => 'page','post_parent' => $parent_page_id));
				
				$tourneyPages = $content['tourneyPages'];
				if (empty($tourneyPages)) {
					throw new Exception("'tourneyPages' parameter cannot be empty");
				}

				$returnContent = array();
				$error = false;
				$message = "Success";
				foreach($tourneyPages as $tourneyPage) {
					
					try {
						$post_content = (empty($tourneyPage['content'])?'<h1>'.$page->post_title.' : '.$tourneyYear.' : '.$tourneyPage['title'].'</h1>':$tourneyPage['content']);
						$post_title = $tourneyPage['title'];
						if ($this->bfi_childPageExists($posts, $post_title)) {
							$returnContent[] = array('error'=>true,'message'=>"$post_title exists already. Nothing done.");
						}
						else {
							$this->bfi_createSubPage($parent_page_id, $post_title, $post_content);
							$returnContent[] = array('error'=>false,'message'=>"$post_title created successfully");
						}
					}	
					catch (Exception $ex) {
						$error = true;
						$message = "Errors Found";
						$returnContent[] = array('error'=>true,'message'=>$ex->getMessage());
					}	
				}
				return $this->createMessage($error,$message,$returnContent);
			}
			catch (Exception $ex) {
				return $this->createExceptionMessage($ex);
			}									
						
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
			if ( ! current_user_can( 'edit_pages' ) )
				return new IXR_Error( 403, __( 'User '.$username.' is not allowed to edit pages and so cannot post results.' ) );
			
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