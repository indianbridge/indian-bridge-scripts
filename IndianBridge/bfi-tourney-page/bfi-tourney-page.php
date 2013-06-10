<?php
/*
Plugin Name: BFI Tourney Page Display
Plugin URI: http://bfi.net.in
Description: A plugin to show tourney pages using ajax
Author: Sriram Narasimhan
Author URI: ...
Version: 1.0

	Copyright: © 2013 Sriram Narasimhan (email : indianbridge@gmail.com)
	License: GNU General Public License v3.0
	License URI: http://www.gnu.org/licenses/gpl-3.0.html
*/


if ( ! class_exists( 'BFI_Tourney_Page_Display' ) ) {
	

	class BFI_Tourney_Page_Display {
		
		public function __construct() {
			// register the javascript for the ajax function
			wp_register_script( 'bfi_tourney_page', plugins_url( 'bfi-tourney-page.js', __FILE__ ), array( 'jquery' ) );	
			wp_enqueue_script('bfi_tourney_page');
			
			// Add the shortcode
			add_shortcode('bfi_tourney_page', array($this, 'bfi_tourney_page'));
		}
		
		public function getFullPath($path) {
			return 'http://localhost/bfi/'.$path;
		}
		
		public function joinPaths($path1,$path2) {
			return rtrim($path1).'/'.ltrim($path2);
		}
		
		public function replaceRelativePath($path) {
			 return str_replace('./','http://localhost/bfi/',$path);
		}
		
		/**
		 * Replace shortcode with posts
		 */
		function bfi_tourney_page ($atts, $content = null ) {
			// Extract shortcode attributes
			extract(shortcode_atts(array(
				'image'		=> '',
				'root'      => '',
				'tabs'      => '',
				'pages'     => ''
			), $atts));
			$tabList = explode(',',$tabs);
			$pageList = explode(',',$pages);
			$error = false;
			ob_start();
			if ($root) {
				
				if (!$image) {
					$image = $this->joinPaths($this->getFullPath($root),'header.jpg');
				}
				if (@getimagesize($image)) {
					echo '<img style="display:block;margin-left:auto;margin-right:auto;" src="'.$image.'" alt="No Image Available"/>';
				}
				else {
					echo '<h1>No Image Available</h1>';
				}
				//$pattern = $this->joinPaths($this->joinPaths('.',$root),'*.{htm,html}');
				$pattern = $this->joinPaths($this->joinPaths('.',$root),'[!_]*');
				$files = glob($pattern,GLOB_ONLYDIR);
				$tabList = array();
				$fullNameTabList = array();
				$pageList = array();
				foreach ($files as $dirname) {
					$filename = $this->joinPaths($dirname,'index.htm');
					if (!file_exists($filename)) {
						$filename = $this->joinPaths($dirname,'index.html');
					}
					if (file_exists($filename)) {
						$path_parts = pathinfo($filename);
						$dir_parts = pathinfo($dirname);
						$tabName = ucwords($dir_parts['filename']);
						$tabList[] = $tabName;
						$fullNameTabList[] = $this->replaceRelativePath($dirname);
						$pageList[] = $this->replaceRelativePath($filename);
						//echo 'File : '.ucwords($path_parts['filename']).'  <br/>';
					}
				}
				//echo json_encode($tabList);
				$tabIDPrefix = 'bfi_tourney_page-tab';
				$contentIDPrefix = 'bfi_tourney_page-content';
				?>
				<script type="text/javascript">
					function callSwitchTabs(tabID, pageURL) {
						bfi_tourney_page.switchTabs(tabID,pageURL,'<?php echo $tabIDPrefix; ?>','<?php echo $contentIDPrefix; ?>','<?php echo $loadingImageURL ?>');
						return false;
					}
					function callLoadPage(pageID,pageURL) {
						bfi_tourney_page.loadPage(pageID,pageURL,'<?php echo $tabIDPrefix; ?>','<?php echo $contentIDPrefix; ?>','<?php echo $loadingImageURL ?>');
					}				
				</script>
				<?php				
				echo '<div class="tabber-widget-default">';
					echo '<ul class="tabber-widget-tabs">';	
						$counter = 1;
						foreach ( $tabList as $tabID ) {
							echo '<li><a id="'.$tabIDPrefix.$counter.'" onclick="callSwitchTabs(\''.$counter.'\',\''.$pageList[$counter-1].'\');" href="javascript:void(0)">'.$tabID.'</a></li>';
							$counter++;
						}
					echo '</ul>';
					echo '<div class="tabber-widget-content">';
						echo '<div class="tabber-widget">';
							$counter = 1;
							foreach ( $pageList as $pageID ) { 
								echo '<div id="'.$contentIDPrefix.$counter.'" style="display:none;"></div>';
								$counter++;
							} 
						echo '</div>';
					echo '</div>';
				echo '</div>';	
				?>
				<script type="text/javascript">
					callSwitchTabs('1','<?php echo $pageList[0]; ?>');
					bfi_tourney_page.saveTabList('<?php echo json_encode($fullNameTabList); ?>');
				</script> 	
				<?php								
			}
			else {		
				echo '<h1>No Information Available<h1>';
			}
			/*if (count($tabList) == 0) {
				echo '<h1>No Pages specified</h1>';
				$error = true;
			}
			if (count($pageList) == 0) {
				echo '<h1>No Page Contents specified</h1>';
				$error = true;
			}
			if (count($tabList) != count($pageList)) {
				echo '<h1>Pages and Page Contents list do not have same size</h1>';
				$error = true;
			}
			if (!$error) {
				$tabIDPrefix = 'bfi_tourney_page-tab';
				$contentIDPrefix = 'bfi_tourney_page-content';
				?>
				<script type="text/javascript">
					function callSwitchTabs(tabID, pageURL) {
						bfi_tourney_page.switchTabs(tabID,pageURL,'<?php echo $tabIDPrefix; ?>','<?php echo $contentIDPrefix; ?>','<?php echo $loadingImageURL ?>');
						return false;
					}
					function callLoadPage(pageID,pageURL) {
						bfi_tourney_page.loadPage(pageID,pageURL,'<?php echo $contentIDPrefix; ?>','<?php echo $loadingImageURL ?>');
					}
				</script>
				<?php				
				echo '<div class="tabber-widget-default">';
					echo '<ul class="tabber-widget-tabs">';	
						$counter = 1;
						foreach ( $tabList as $tabID ) {
							echo '<li><a id="'.$tabIDPrefix.$counter.'" onclick="callSwitchTabs(\''.$counter.'\',\''.$pageList[$counter-1].'\');" href="javascript:void(0)">'.$tabID.'</a></li>';
							$counter++;
						}
					echo '</ul>';
					echo '<div class="tabber-widget-content">';
						echo '<div class="tabber-widget">';
							$counter = 1;
							foreach ( $pageList as $pageID ) { 
								echo '<div id="'.$contentIDPrefix.$counter.'" style="display:none;"></div>';
								$counter++;
							} 
						echo '</div>';
					echo '</div>';
				echo '</div>';	
				?>
				<script type="text/javascript">
					callSwitchTabs('1','<?php echo $pageList[0]; ?>');
				</script> 	
				<?php				
			}*/
			$out = ob_get_clean();
			return $out;
		}			
		
	}

	// finally instantiate our plugin class and add it to the set of globals
	$GLOBALS['bfi_tourney_page_display'] = new BFI_Tourney_Page_Display();
}