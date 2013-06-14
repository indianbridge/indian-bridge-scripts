<?php


	function update_notifier_menu() {  
		$xml = get_latest_theme_version(86400); // This tells the function to cache the remote call for 21600 seconds (6 hours)
		$theme_data = get_theme_data(TEMPLATEPATH . '/style.css'); // Get theme data from style.css (current version is what we want)
		
		if (version_compare($theme_data['Version'], $xml->latest) == -1)
			add_object_page( $theme_data['Name'] . 'Theme Updates', 'Update<span class="update-plugins count-1"><span class="update-count">v.' . $xml->latest . '</span></span>', 'administrator', strtolower($theme_data['Name']) . '-updates', update_notifier);
	}  
	add_action('admin_menu', 'update_notifier_menu');


	
	function update_notifier() { 
		$xml = get_latest_theme_version(86400); // This tells the function to cache the remote call for 21600 seconds (6 hours)
		$theme_data = get_theme_data(TEMPLATEPATH . '/style.css'); // Get theme data from style.css (current version is what we want) ?>
		
		<style>
			.wrap { max-width:800px; }
			.f32 { font-size:32px; font-weight:normal; }
			.f20 { font-size:20px; color:#777; line-height:1.6em; }
			.pt_changelog ul { list-style-type:disc; padding:0 0 1em 1.5em; }
		</style>
	
		<div class="wrap">
		
			<div id="icon-tools" class="icon32"></div>
			<h2><?php echo $theme_data['Name']; ?> Theme Updates</h2>
			<div id="message" class="updated below-h2"><p><strong>There is a new version of the <?php echo $theme_data['Name']; ?> theme available.</strong> You have version <?php echo $theme_data['Version']; ?> installed. Update to version <?php echo $xml->latest; ?></p></div>

			<div class="h10"><!-- --></div>
			
			<img style="float: left; margin: 0 20px 20px 0;" src="<?php echo get_bloginfo( 'template_url' ) . '/screenshot.png'; ?>" />
			
			<div>
				<h3>Update instructions</h3>
				<div style="display:table;">
					<ol style="margin-top:0;">
						<li>To update the theme, please, download an updated version from <a target="_blank" href="http://pandathemes.com/topbusiness-theme-free-version/">PandaThemes.com</a></li>
						<li>Deactivate & delete the current version of theme and install new one.</li>
					</ol>
				</div>
				<p>
					<strong>Please note:</strong> make a backup of the theme inside your WordPress installation folder <code>/wp-content/themes/<?php echo strtolower($theme_data['Name']); ?>/</code> -OR- make sure you've an archive of the current version of theme.
					If you didn't make any changes to the theme files, you are free to overwrite them with the new ones without the risk of losing theme settings, pages, posts, etc, and backwards compatibility is guaranteed.
				</p>
			</div>
			<div class="clear"></div>
	
			<h2 class="nav-tab-wrapper">
				<a href="#" class="nav-tab nav-tab-active">What's New?</a>
				<a href="http://support.pandathemes.com/" target="_blank" class="nav-tab">Support Forum</a>
				<a href="http://pandathemes.com/wordpress-themes/topbusiness/" target="_blank" class="nav-tab">Purchase Theme</a>
			</h2>

			<div class="h10"><!-- --></div>

			<h3 class="f32"><?php echo $theme_data['Name'] . ' ' . $xml->latest; ?></h3>

			<?php echo $xml->changelog; ?>

		</div>
		
	<?php } 



	function get_latest_theme_version($interval) {

		$notifier_file_url = 'http://pandathemes.com/xml/topbusiness-free-notifier.xml';
		
		$db_cache_field = 'contempo-notifier-cache';
		$db_cache_field_last_updated = 'contempo-notifier-last-updated';
		$last = get_option( $db_cache_field_last_updated );
		$now = time();
		// check the cache
		if ( !$last || (( $now - $last ) > $interval) ) {
			// cache doesn't exist, or is old, so refresh it
			if( function_exists('curl_init') ) { // if cURL is available, use it...
				$ch = curl_init($notifier_file_url);
				curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
				curl_setopt($ch, CURLOPT_HEADER, 0);
				curl_setopt($ch, CURLOPT_TIMEOUT, 10);
				$cache = curl_exec($ch);
				curl_close($ch);
			} else {
				$cache = file_get_contents($notifier_file_url); // ...if not, use the common file_get_contents()
			}
			
			if ($cache) {			
				// we got good results
				update_option( $db_cache_field, $cache );
				update_option( $db_cache_field_last_updated, time() );			
			}
			// read from the cache file
			$notifier_data = get_option( $db_cache_field );
		}
		else {
			// cache file is fresh enough, so read from it
			$notifier_data = get_option( $db_cache_field );
		}
		
		$xml = simplexml_load_string($notifier_data); 
		
		return $xml;
	}



?>