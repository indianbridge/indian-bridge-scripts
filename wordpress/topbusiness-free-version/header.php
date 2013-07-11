<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	global
		$theme_options,
		$panda_sl,
		$panda_he;

?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html <?php language_attributes(); ?>>

	<head>
		<meta http-equiv="Content-Type" content="<?php bloginfo('html_type'); ?>; charset=<?php bloginfo('charset'); ?>" />
		<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
		<meta name="distribution" content="global" />
		<title><?php wp_title('-', true, 'right'); ?><?php bloginfo('name'); ?></title>
		<link rel="Shortcut Icon" href="<?php echo $theme_options['favicon']; ?>" type="image/x-icon" />
		<link rel="stylesheet" href="<?php bloginfo('stylesheet_url'); ?>" type="text/css" media="screen" />
		<?php
			do_action('styles');
			do_action('ie');
			wp_head();
			do_action('google_fonts');
			echo $theme_options['google_analytics'];
		?>
	</head>

	<body <?php body_class(); ?>>
	
		<div id="site-wrapper">
	
			<?php
				// Notice for visitors
				if ($panda_he['notice'] == 'enable') : echo '<div id="quick-notice">' . $panda_he['notice_data'] . '</div>'; endif;
			?>
		
			<div id="topbg"><!-- --></div>
		
			<div id="headwrapper">
		
				<div id="header">
			
					<?php
					// LOGO
					$logo_type = $theme_options['logo_type'];
			
						// Text logo
						if ($logo_type == 'Text') : ?>
							<div id="logo" class="fl div-as-table">
								<div>
									<div>
										<?php
											$logo = ($theme_options['sitename']) ? $theme_options['sitename'] : get_bloginfo('name');
											$desc = ($theme_options['sitedesc']) ? $theme_options['sitedesc'] : get_bloginfo('description');
											echo '<h1 class="logo"><a href="'.get_option('home').'">'.$logo.'</a></h1> &nbsp;'.$desc;
										?>
									</div>
								</div>
							</div>
							<?php ;
			
						// Default			
						else : ?>						
							<div id="logo" class="fl div-as-table">
								<div>
									<div>
									<h1 id="imglogo" class="logo"><a href="<?php echo get_option('home'); ?>"><img src="<?php echo $theme_options['logo'] ?>" alt="<?php bloginfo('title'); ?>"/></a></h1>
									</div>
								</div>
							</div>
						<?php ;
			
						endif;
			
						// Search form
						if ($panda_he['searchform'] == 'enable') : ?>
							<div id="hsearch" class="hmisc fr div-as-table">
								<div>
									<div id="topsearch">
									<form id="searchform" method="get" action="<?php echo get_option('home'); ?>/">
										<div>
										<input 
											type="text" 
											value="<?php _e('Search...','pandathemes') ?>" 
											onblur="this.value=(this.value=='') ? '<?php _e('Search...','pandathemes') ?>' : this.value;"
											onfocus="this.value=(this.value=='<?php _e('Search...','pandathemes') ?>') ? '' : this.value;"
											maxlength="150" 
											name="s"
											id="s"
											class="searchfield"
										/>
										<input type="submit" value="&nbsp;" id="searchsubmit">
										</div>
									</form>
									</div>
								</div>
							</div>
							<?php ;
						endif;
			
						// Lifestream
						if ($panda_he['lifestream'] == 'enable') : ?>
							<div id="hlifestream" class="hmisc fr div-as-table">
								<div>
									<?php
										// SOCIAL NETWORKS
										if ($panda_he['lifestream'] == 'enable') : $icount = 0;
			
											echo '<div id="icons">';
											
												$arr = array(
													'rss',
													'twitter',
													'deviantart',
													'digg',
													'facebook',
													'flickr',
													'lastfm',
													'linkedin',
													'myspace',
													'picasa',
													'reddit',
													'reddit',
													'skype',
													'stumbleupon',
													'technorati',
													'youtube',
													'vimeo',
													'blogger',
													'delicious',
													'designfloat',
													'designmoo'
												);
			
												foreach ($arr as $value) :
			
													if ($panda_he['life_'.$value]) :
			
														echo '<a href="'.$panda_he['life_'.$value].'" target="_blank" rel="nofollow"><img class="tooltip-t" title="'.$value.'" src="'.get_bloginfo('template_url').'/images/icons/'.$value.'_16.png" alt=""/></a>';
			
													endif;
			
												endforeach;
			
											// Lifestream custom
											if ($panda_he['lifestream_custom']) : echo $panda_he['lifestream_custom']; endif;
			
											echo '</div><!-- end icons -->';
			
										endif;
									?>
								</div>
							</div>
							<?php ;
						endif;
			
						// Custom data on header
						if ($panda_he['hcustom'] == 'enable') : ?>
							<div id="hcustom" class="hmisc fr div-as-table">
								<div>
									<div>
										<?php echo do_shortcode($panda_he['hcustom_data']) ?>
									</div>
								</div>
							</div>
							<?php ;
						endif;
						?>
			
					<div class="clear"><!-- --></div>
			
				</div><!-- end header -->
		
				<div id="menuwrapper">
		
					<?php

					// MAIN MENU
						wp_nav_menu(array(
							'theme_location'	=> 'primary-menu',
							'sort_column'		=> 'menu_order',
							'container_class'	=> 'menubox',
							'menu_class'		=> 'top-menu',
							'echo'				=> true,
							'depth'				=> 0,
							'fallback_cb'		=> 'karina_fallback_menu',
							'context'			=> 'frontend',
							'walker'			=> new kclass_megamenu_walker()
							)
						);

						// Message on the menu bar
						$out = $panda_he['topmessage'] ? '<div id="topmess">' . $panda_he['topmessage'] . '</div>' : '';
						echo $out;

						// Drop-down menu
						wp_nav_menu(array(
							'theme_location' 	=> 'primary-menu',
							'echo'				=> true,
							'container_class'	=> 'selectElement',
							'menu_class'		=> 'dropdown-menu',
							'depth'				=> 0,
							'fallback_cb'		=> 'dropdown_fallback_menu',
							'items_wrap'     	=> '<select id="selectElement"><option value="#">' . __('Select a page &crarr;','pandathemes') . '</option>%3$s</select>',
							'walker'        	=> new Walker_Nav_Menu_Dropdown()
						));

					?>
					<script>header_menu_showMenu();</script>

					<div class="clear"><!-- --></div>
			
				</div>
		
			</div>

			<div id="sub1"><!-- shadow --></div>

			<div id="layout">
		
			<?php
				// SLIDER - CUSTOM POST TYPE
				include(TEMPLATEPATH."/inc/slider/slider.php");
			?>