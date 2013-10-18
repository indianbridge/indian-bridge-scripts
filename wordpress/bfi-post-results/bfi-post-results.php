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
		private $jsonDelimiter;
		public function __construct() {
			$dataGridName = "datatables";

			$jsFilePath = 'js/jquery.' . $dataGridName . '.min.js';
			$cssFilePath = 'css/jquery.' . $dataGridName . '.css';
			$jsIdentifier = 'jquery_' . $dataGridName . '_js';
			$cssIdentifier = 'jquery_' . $dataGridName . '_css';

			// register the javascript
			//wp_register_script($jsIdentifier, plugins_url($jsFilePath, __FILE__), array('jquery'));
			//wp_enqueue_script($jsIdentifier);

			// Register bfi master point javascript
			wp_register_script('bfi_post_results', plugins_url('bfi_post_results.js', __FILE__), array('jquery'));
			wp_enqueue_script('bfi_post_results');

			// register the css
			//wp_register_style($cssIdentifier, plugins_url('css/jquery.datatables_themeroller.css', __FILE__));
			//wp_enqueue_style($cssIdentifier);
			//wp_register_style('jquery_dataTables_redmond_css', plugins_url('css/jquery_ui/jquery-ui-1.10.3.custom.min.css', __FILE__));
			//wp_enqueue_style('jquery_dataTables_redmond_css');
			
			add_shortcode('bfi_results_display', array($this, 'bfi_results_display'));			
			add_filter( 'xmlrpc_methods', array( $this, 'add_xml_rpc_methods' ) );
			
			$this->jsonDelimiter = "!@#";
		}

		/**
		 * Replace shortcode with posts
		 */
		function bfi_results_display($atts, $content = null) {
			ob_start();
			echo '<h1>You have to be a member of BFI and be logged in to see Masterpoint Summary and Details</h1>';
			$tabs = array('lookupmemberid' => 'Find Your BFI Member ID', 'leaderboard' => "Masterpoint Leaderboard");
			$selectedTab = 'lookupmemberid';
			$html = "";
			$html .= '<div class="tabber-widget-default">';
			$html .= '<ul class="tabber-widget-tabs">';
			$tabIDPrefix = 'masterpoint_page_tab_';
			foreach ($tabs as $tab => $tabName) {
				$html .= '<li><a id="' . $tabIDPrefix . $tab . '" onclick="switchResultsTab(\'' . $tab . '\',\'' . $tabIDPrefix . '\',\'' . plugins_url('jquery.datatables.results_display.php', __FILE__) . '\',\'' . $member_id . '\',\'' . DB_NAME . '\');" href="javascript:void(0)">' . $tabName . '</a></li>';
			}
			$html .= '</ul>';
			$html .= '<div class="tabber-widget-content">';
			$html .= '<div class="tabber-widget">';
			$html .= '<div id="bfi_masterpoints_table_container">';
			$html .= '</div></div></div></div>';
			$html .= '<script type="text/javascript">';
			$html .= 'switchResultsTab(\'' . $selectedTab . '\',\'' . $tabIDPrefix . '\',\'' . plugins_url('jquery.datatables.results_display.php', __FILE__) . '\',\'' . $member_id . '\',\'' . DB_NAME . '\');';
			$html .= '</script>';
			echo $html;			
			//echo $this -> getMasterpointTabs($tabs, $selectedTab, '');
			$out = ob_get_clean();
			return $out;					
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
			$methods['indianbridge.postResults'] = array( $this, 'bfi_post_results' );
			$methods['bfi.addTourney'] = array($this,'bfi_addTourney');
			$methods['bfi.getTourneys'] = array($this,'bfi_getTourneys');
			return $methods;
		}
		
		public function bfi_validateEditPageCredentials($params) {
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server->escape( $params );		
			$blog_id  = (int) $params[0]; // not used, but follow in the form of the wordpress built in XML-RPC actions
			$username = $params[1];
			$password = $params[2];

			// verify credentials
			if ( ! $user = $wp_xmlrpc_server->login( $username, $password ) ) {
				return $this->createErrorMessage($wp_xmlrpc_server->error);
			}
			
			// check for edit_posts capability (requires contributor role)
			if ( ! current_user_can( 'edit_pages' ) ) {
				return $this->createErrorMessage('User '.$username.' is not allowed to edit pages and so cannot create tourney pages.');
			}

			return $this->createSuccessMessage("Validated Credentials");			
		}
		
		public function bfi_getTourneys($params) {
			// Check credentials first
			$return_string = $this->bfi_validateEditPageCredentials($params);
			$result = json_decode($return_string);
			$error_flag = filter_var($result->error, FILTER_VALIDATE_BOOLEAN);
			if ($error_flag) return $return_string;			
			$ignorePageIDs = array(172);
			$expandPageIDs = array(1464,893);
			$pages = get_pages( array( 'child_of' => 171,  'parent'=>171 ) );		
			foreach($pages as $page) {
				if (!in_array($page->ID, $ignorePageIDs,TRUE) && !in_array($page->ID, $expandPageIDs,TRUE)) {
					$pageNames[] = $page->post_title;
					$pageIDs[] = $page->ID;
				}
			}	
			foreach($expandPageIDs as $expandPage) {
				$pages = get_pages( array( 'child_of' => $expandPage,  'parent'=>$expandPage ) );		
				foreach($pages as $page) {
					$pageNames[] = $page->post_title;
					$pageIDs[] = $page->ID;
				}				
			}
			$content = implode(",", $pageNames).'#'.implode(",",$pageIDs);
			return $this->createSuccessMessage('Success',$content);
		}

		public function hasError($message) {
			$lines = explode($this->jsonDelimiter, $message);
			return filter_var($lines[0], FILTER_VALIDATE_BOOLEAN);
		}
		
		public function getMessage($message) {
			$lines = explode($this->jsonDelimiter, $message);
			return $lines[1];
		}
		
		public function getContent($message) {
			$lines = explode($this->jsonDelimiter, $message);
			return $lines[2];			
		}

		public function createMessage($error, $message, $content) {
			$lines[] = $error;
			$lines[] = $message;
			$lines[] = $content;
			return implode($this->jsonDelimiter, $lines);
		}

		public function createErrorMessage($message, $content = '') {
			return $this -> createMessage("true", $message, $content);
		}

		public function createSuccessMessage($message, $content = '') {
			return $this -> createMessage("false", $message, $content);
		}
		
		public function bfi_addTourney($params) {
			// Check credentials first
			$return_string = $this->bfi_validateEditPageCredentials($params);
			$result = json_decode($return_string);
			$error_flag = filter_var($result->error, FILTER_VALIDATE_BOOLEAN);
			if ($error_flag) return $return_string;		
			
			do_action( 'xmlrpc_call', 'bfi.addTourney' ); // patterned on the core XML-RPC actions			
			
			// Parse the parameters
			$args     = $params[3];
			$page_id = $args['parentPageID'];
			$tourneys_page_id = 171;
			if (empty($page_id)) {
				return $this->createErrorMessage("'parentPageID' parameter cannot be empty");
			}
			if (!is_numeric($page_id)) {
				return $this->createErrorMessage("parentPageID : $page_id has to be numeric id of tourney page.");
			}
			$page_id = intval($page_id);
			if ($page_id === 0) {
				return $this->createErrorMessage("parentPageID : $page_id is not a valid integer id of page.");
			}
			$page = get_post($page_id);
			if ($page == null) {
				return $this->createErrorMessage("Page with parentPageID : $page_id does not exist.");
			}
			if ($page->post_status !== 'publish') {
				return $this->createErrorMessage("Page with parentPageID : $page_id does not have published status.");
			}
			
			// Check if it is child of tourneys
			$ancestors = get_post_ancestors($page_id);
			if (!$ancestors || !in_array($tourneys_page_id,$ancestors,TRUE)) {
				return $this->createErrorMessage("Page with parentPageID : $page_id is not a child page of Tourneys page.");
			}
			
			// Check if root page already exists
			$tourneyYear = $args['tourneyYear'];
			$tourneyPages = $args['tourneyPages'];
			if (empty($tourneyPages)) {
				return $this->createErrorMessage("'tourneyPages' parameter cannot be empty");
			}
			if (empty($tourneyYear)) {
				return $this->createErrorMessage("'tourneyYear' parameter cannot be empty");
			}
			if (!is_numeric($tourneyYear)) {
				return $this->createErrorMessage("tourneyYear : $tourneyYear has to be numeric year.");
			}		
			$tourneyYear = intval($tourneyYear);
			$today = getdate();
			$currentYear = intval($today['year']);	
			if($tourneyYear !== $currentYear && $tourneyYear !== $currentYear+1) {
				return $this->createErrorMessage("tourneyYear : $tourneyYear has to be current year ($currentYear) or next year only.");
			}
			$pageName = 'y'.$args['tourneyYear'];
			$uri = get_page_uri($page_id).'/'.$pageName;
			$pageExists = get_page_by_path($uri);
			if ($pageExists != null) {
				return $this->createErrorMessage("A page already exists at $uri");
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
				return $this->createErrorMessage("Unable to create page at $uri because".$rootPage_id->get_error_message().PHP_EOL.'Subpages will not be created.');
			}
			update_post_meta( $rootPage_id, $page_template_meta, $page_template );
			$tourneyNames = explode(',', $tourneyPages);
			$message = '';
			$error = 'false';
			foreach($tourneyNames as $tourneyName) {
				$post = array(
					'comment_status' => 'closed',
					'ping_status'    => 'closed',
					'post_content'   => '<h1>'.$page->post_title.' : '.$tourneyYear.' : '.$tourneyName.'</h1>',
					'post_parent'    => $rootPage_id,
					'post_status'    => 'draft',
					'post_title'     => $tourneyName,
					'post_type'      => 'page'
				);  	
				$newPage_id = wp_insert_post($post,true);	
				if (is_wp_error($newPage_id)) {
					$error = 'true';
					$message .= "Unable to create page at $uri because".$newPage_id->get_error_message().PHP_EOL;
				}
				else {
					update_post_meta( $newPage_id, $page_template_meta, $page_template );		
				}						
			}
			if ($error === 'false') $message = 'All Pages created successfully';
			return $this->createMessage($error,$message);
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