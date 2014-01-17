<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$theme_options = get_option('ikarina');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="ikarina_action" name="ikarina_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> <input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $theme_options['tab']; ?>" id="input-tab" />

<?php echo pt_get_full_theme() ?>

<div class="adminarea">

    <!-- tabs -->
    <ul class="tab-navigation">
		<li><a href="#" title="general"><?php _e('General','pandathemes'); ?></a></li>
		<li><a href="#" title="posts"><?php _e('Posts','pandathemes'); ?></a></li>
		<li><a href="#" title="pages"><?php _e('Pages','pandathemes'); ?></a></li>
		<?php
			if ( is_super_admin() ) :
				echo '<li><a href="#" title="help">' . __('Help','pandathemes') . '</a></li>';
			endif;
		?>
    </ul>
    <div class="clear" style="height:1%; border-top:3px solid #6d6d6d;"></div>

    <div id="tabbed-content">

   <!----------------------------------------------------------------------------
								G E N E R A L   P A G E
	----------------------------------------------------------------------------->
    <div id="general">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Logo','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">
					<?php _e('Display as','pandathemes'); ?>
				</td>
				<td>
	
					<input type="radio" value="Text" name="logo_type" id="logo_text" <?php if ($theme_options["logo_type"]=="Text"):?> checked="checked" <?php endif; ?> /> <label for="logo_text"><?php _e('Text','pandathemes'); ?></label>
	
					<input title="Available for FULL version only" type="radio" value="Image" name="logo_type" id="logo_image" <?php if ($theme_options["logo_type"]=="Image"):?> checked="checked" <?php endif; ?>/> <label title="Available for FULL version only" for="logo_image"><?php _e('Image','pandathemes'); ?></label>
	
				</td>
			</tr></table>
	
			<div class="clear h20"><!-- --></div>
	
			<table style="display:none;" id="logo_t"><tr>
				<td class="size17">
					<span><?php _e('Site name','pandathemes'); ?></span>
				</td>
				<td>
					<input type="text" name="sitename" value="<?php $z = ( $theme_options['sitename'] ) ? $theme_options['sitename'] : get_bloginfo('name'); echo $z; ?>" class="input size20" size="20" />
					<input type="text" name="sitedesc" value="<?php $z = ( $theme_options['sitedesc'] ) ? $theme_options['sitedesc'] : get_bloginfo('description'); echo $z; ?>" class="input size40" size="40" />
					<small><?php _e('Enter the site name and tagline.','pandathemes'); ?></small>
				</td>
			</tr></table>

			<table style="display:none;" id="logo_img"><tr>
				<td>
					<div class="logo"><img class="image-logo-preview" src="<?php $z = ( $theme_options['logo'] ) ? $theme_options['logo'] : get_bloginfo('template_url')."/images/logo.png"; echo $z; ?>" /></div>
				</td>
			</tr></table>
	
			<table style="display:none;" id="logo_i"><tr>
				<td class="size17">
					<span><?php _e('Image URL','pandathemes'); ?></span>
				</td>
				<td class="size80">
					<input type="text" name="logo" value="<?php $z = ( $theme_options['logo'] ) ? $theme_options['logo'] : get_bloginfo('template_url')."/images/logo.png"; echo $z; ?>" class="input size60" id="image-logo-input" size="60" />
					<a class="thickbox button select-image" id="image-logo" href="media-upload.php&type=image&TB_iframe=1&width=640&height=469"><?php _e('Browse','pandathemes'); ?></a>
					<small><?php _e('Enter an URL of your image logo e.q. <code>http://yoursite.com/image.png</code> <br>JPG, GIF or PNG format allowed.','pandathemes'); ?></small>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Favicon','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Image URL','pandathemes'); ?></span>
				</td>
				<td class="size80">
					<input type="text" name="favicon" value="<?php $z = ( $theme_options['favicon'] ) ? $theme_options['favicon'] : get_bloginfo('template_url')."/favicon.ico"; echo $z; ?>" class="input size60" id="image-favicon-input" size="60" style="background-image:url(<?php $z = ( $theme_options['favicon'] ) ? $theme_options['favicon'] : get_bloginfo('template_url')."/favicon.ico"; echo $z; ?>); background-position:3px 50%; background-repeat:no-repeat; padding:3px 3px 3px 23px;" />
					<a class="thickbox button select-image" id="image-favicon" href="media-upload.php&type=image&TB_iframe=1&width=640&height=469"><?php _e('Browse','pandathemes'); ?></a>
					<small><?php _e('e.q.','pandathemes'); ?> <code>http://yoursite.com/wp-content/themes/theme/favicon.ico</code></small>				
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Google Analytics','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Code','pandathemes'); ?></span>
				</td>
				<td>
					<textarea cols="70" rows="4" name="google_analytics" id="google_analytics" class="input" /><?php echo $theme_options['google_analytics'] ?></textarea>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Copyrights','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Enter here','pandathemes'); ?></span>
				</td>
				<td>
					<textarea cols="70" rows="2" name="copyrights" id="copyrights" class="input f11" /><?php $z = ( $theme_options['copyrights'] ) ? $theme_options['copyrights'] : date('Y') . ' &copy; ' . get_bloginfo('sitename'); echo $z; ?></textarea>
					<div class="clear h20"><!-- --></div>
				</td>
			</tr></table>
	
			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input title="Available for FULL version only" type="checkbox" name="dev_copyright" value="remove" <?php if ($theme_options['dev_copyright']=='remove') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<?php _e("Remove developer's link from the footer",'pandathemes'); ?>
							<div class="clear h10"><!-- --></div>
							<textarea style="background:#CCC; border-color:#CCC;" title="Available for FULL version only" cols="60" rows="2" name="dev_copyright_data" class="input w97p f11" /><?php $z = ( $theme_options['dev_copyright_data'] ) ? $theme_options['dev_copyright_data'] : '<a href="http://pandathemes.com/" target="_blank">TopBusiness theme</a> by PandaThemes'; echo $z; ?></textarea>
						</td>
					</tr></table>
				</td>
			</tr></table>

		</fieldset>

   	</div><!-- end General -->


   <!----------------------------------------------------------------------------
								B L O G   P A G E
	----------------------------------------------------------------------------->

	<div id="posts">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Template','pandathemes'); ?></legend>

			<div class="tmpl_radio">
				<label class="lable-img" for="default_template"><img src="<?php echo $template_url ?>/admin/graphics/templates/posts_default.gif" width="135" height="110"></label>
				<input type="radio" value="default" name="blog_template" id="default_template" <?php if ( ! $theme_options['blog_template'] || $theme_options['blog_template'] == 'default' ) : echo 'checked="checked"'; endif; ?> />
				<label for="default_template"><?php _e('Default','pandathemes'); ?></label>
			</div>

			<div class="tmpl_radio">
				<label class="lable-img" for="t1_template"><img src="<?php echo $template_url ?>/admin/graphics/templates/posts_1.gif" width="135" height="110"></label>
				<input type="radio" value="t1" name="blog_template" id="t1_template" <?php if ( $theme_options['blog_template'] == 't1' ) : echo 'checked="checked"'; endif; ?> />
				<label for="t1_template"><?php _e('Template','pandathemes'); ?> #1</label>
			</div>

			<div class="tmpl_radio">
				<label class="lable-img" for="t2_template"><img title="Available for FULL version only" src="<?php echo $template_url ?>/admin/graphics/templates/posts_2.gif" width="135" height="110"></label>
				<input disabled="disabled" title="Available for FULL version only" type="radio" value="t2" name="blog_template" id="t2_template" <?php if ( $theme_options['blog_template'] == 't2' ) : echo 'checked="checked"'; endif; ?> />
				<label title="Available for FULL version only" for="t2_template"><?php _e('Template','pandathemes'); ?> #2</label>
			</div>

			<div class="tmpl_radio tmpl_radio_last">
				<label class="lable-img" for="t3_template"><img title="Available for FULL version only" src="<?php echo $template_url ?>/admin/graphics/templates/posts_3.gif" width="135" height="110"></label>
				<input disabled="disabled" title="Available for FULL version only" type="radio" value="t3" name="blog_template" id="t3_template" <?php if ( $theme_options['blog_template'] == 't3' ) : echo 'checked="checked"'; endif; ?> />
				<label title="Available for FULL version only" for="t3_template"><?php _e('Template','pandathemes'); ?> #3</label>
			</div>

			<small><?php _e('Select the blog template.','pandathemes'); ?></small>
		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Common','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Posts per page','pandathemes'); ?></span>
				</td>
				<td>
					<select name="posts_qty">
						<?php $q = $theme_options['posts_qty'];
							$arr = array('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21');
							foreach ($arr as $value) {
								$out = '<option value="'.$value.'"';
								if ( $q == $value ) : $out .= ' selected'; endif;
								$out .= '>'.$value.' &nbsp;</option>';
								echo $out;
							};
						?>
					</select>
					<small><?php _e('A quantity of posts per blog page.','pandathemes'); ?></span></small>
				</td>
			</tr></table>
	
			<div class="clear h20"><!-- --></div>
	
			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="square_thumbs" id="square_thumbs" class="checkbox-toggle" value="enable" <?php if ($theme_options['square_thumbs']=='enable') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Square thumbnails','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Crop all thumbs by square.','pandathemes'); ?></small>
							<div id="square_thumbs_data">
								<div class="clear h10"><!-- --></div>
								<table><tr>
									<td class="size3">
										<input type="checkbox" name="thumbs_crop_top" value="enable" <?php if ($theme_options['thumbs_crop_top']=='enable') { echo 'checked' ;} ?> /> 
									</td>
									<td>
										<span><?php _e('Crop by top','pandathemes'); ?></span>
										<small class="no-p"><?php _e('Crop all thumbs by top.','pandathemes'); ?></small>
									</td>
								</tr></table>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

			<div class="clear h20"><!-- --></div>

			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="post_metas" id="post_metas" class="checkbox-toggle" value="enable" <?php if ($theme_options['post_metas']=='enable') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Meta','pandathemes'); ?></span>
							<small class="no-p">Date, tags, categories etc.</small>
							<div id="post_metas_data">
								<div class="clear h10"><!-- --></div>
								<table><tr>
									<td class="size3">
										<input type="checkbox" name="author_info" value="enable" <?php if ($theme_options['author_info']=='enable') { echo 'checked' ;} ?> /> 
									</td>
									<td>
										<span><?php _e("Author's info",'pandathemes'); ?></span>
										<small class="no-p"><?php _e('Display','pandathemes'); ?> <a href="http://en.gravatar.com/">Gravatar</a> <?php _e('and','pandathemes'); ?> <a href="<?php get_option('home'); ?>/wp-admin/profile.php"><?php _e("author's bio",'pandathemes'); ?></a> <?php _e('at the post page','pandathemes'); ?>.</small>
									</td>
								</tr></table>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Post page','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="before_post" id="before_post" class="checkbox-toggle" value="enable" /> 
						</td>
						<td>
							<span><?php _e('Before post','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Display custom data before the post. Shortcodes allowed.','pandathemes'); ?></small>
							<div id="before_post_data">
							<div class="clear h10"><!-- --></div>
								<textarea cols="70" rows="5" name="before_post_data" class="input w97p" /><?php echo $theme_options['before_post_data'] ?></textarea>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

			<div class="clear h20"><!-- --></div>

			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="after_title" id="after_title" class="checkbox-toggle" value="enable" /> 
						</td>
						<td>
							<span><?php _e('After title','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Display custom data after the title. Shortcodes allowed.','pandathemes'); ?></small>
							<div id="after_title_data">
							<div class="clear h10"><!-- --></div>
								<textarea cols="70" rows="5" name="after_title_data" class="input w97p" /><?php echo $theme_options['after_title_data'] ?></textarea>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

			<div class="clear h20"><!-- --></div>

			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="post_feat_image" id="post_feat_image" class="checkbox-toggle" value="enable" <?php if ($theme_options['post_feat_image']=='enable') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Featured image','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Display featured image on post page.','pandathemes'); ?></small>
							<div id="post_feat_image_data">
								<div class="clear h10"><!-- --></div>
								<table><tr>
									<td class="size3">
										<input type="checkbox" name="post_feat_image_link" value="enable" <?php if ($theme_options['post_feat_image_link']=='enable') { echo 'checked' ;} ?> /> 
									</td>
									<td>
										<span><?php _e('Link to an original','pandathemes'); ?></span>
										<small class="no-p"><?php _e('Open the full size image in lightbox by click.','pandathemes'); ?></small>
									</td>
								</tr></table>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

			<div class="clear h20"><!-- --></div>
	
			<table><tr>
				<td class="size17">&nbsp;
					
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="excerpt" value="enable" <?php if ($theme_options['excerpt']=='enable') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Excerpt','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Display excerpt on post page.','pandathemes'); ?></small>
						</td>
					</tr></table>
				</td>
			</tr></table>

			<div class="clear h20"><!-- --></div>

			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="after_post" id="after_post" class="checkbox-toggle" value="enable" /> 
						</td>
						<td>
							<span><?php _e('After post','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Display custom data after the post. Shortcodes allowed.','pandathemes'); ?></small>
							<div id="after_post_data">
							<div class="clear h10"><!-- --></div>
								<textarea cols="70" rows="5" name="after_post_data" class="input w97p" /><?php echo $theme_options['after_post_data'] ?></textarea>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Comments','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="post_comments" id="post_comments" class="checkbox-toggle" value="enable" <?php if ($theme_options['post_comments']=='enable') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Comments','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Display comments.','pandathemes'); ?></small>
							<div id="post_comments_data">
								<div class="clear h10"><!-- --></div>
								<table>
									<tr>
										<td class="size3">
											<input type="checkbox" name="gravatars_on_comments" value="yes" <?php if ($theme_options['gravatars_on_comments']=='yes') { echo 'checked' ;} ?> /> 
										</td>
										<td>
											<span><?php _e('Gravatars','pandathemes'); ?></span>
											<small class="no-p"><?php _e('Display a gravatars of comments.','pandathemes'); ?></small>
											<div class="clear h10"><!-- --></div>
										</td>
									</tr>
									<tr>
										<td class="size3">
											<input type="checkbox" name="dates_on_comments" value="yes" <?php if ($theme_options['dates_on_comments']=='yes') { echo 'checked' ;} ?> /> 
										</td>
										<td>
											<span><?php _e('Dates','pandathemes'); ?></span>
											<small class="no-p"><?php _e('Display a dates of comments.','pandathemes'); ?></small>
											<div class="clear h10"><!-- --></div>
										</td>
									</tr>
									<tr>
										<td class="size3">
											<input type="checkbox" name="website_on_comments" value="yes" <?php if ($theme_options['website_on_comments']=='yes') { echo 'checked' ;} ?> /> 
										</td>
										<td>
											<span><?php _e('Website an input field','pandathemes'); ?></span>
											<small class="no-p"><?php _e('Display a "Website" input field on the comment form.','pandathemes'); ?></small>
											<div class="clear h10"><!-- --></div>
										</td>
									</tr>
									<tr>
										<td class="size3">
											<input type="checkbox" name="post_trackbacks" value="enable" <?php if ($theme_options['post_trackbacks']=='enable') { echo 'checked' ;} ?> /> 
										</td>
										<td>
											<span><?php _e('Trackbacks','pandathemes'); ?></span>
											<small class="no-p"><?php _e('Display trackbacks.','pandathemes'); ?></small>
											<div class="clear h10"><!-- --></div>
										</td>
									</tr>
								</table>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Image placeholder','pandathemes'); ?></legend>
			<table id="logo_i"><tr>
				<td class="size17">
					<?php
						$src = $theme_options['img_placeholder'];

						// For multisite WordPress
						global $blog_id;
						if (isset($blog_id) && $blog_id > 1) {
							$imageParts = explode('/files/', $src);
							if (isset($imageParts[1])) {
								$path = '/blogs.dir/'.$blog_id.'/files/'.$imageParts[1];
							}
						}

						// For default WordPress
						else {
							$path = $src;
						}

						$thumb = ($src)
							? .$path
							: $template_url.'/admin/graphics/blog-thumb-100x100.jpg';
					?>
					<img src="<?php echo $thumb ?>" width="100" height="100">
				</td>
				<td class="size80">
					<input type="text" name="img_placeholder" value="<?php echo $theme_options['img_placeholder'] ?>" class="input size60" id="img-placeholder-input" size="60" />
					<a class="thickbox button select-image" id="img-placeholder" href="media-upload.php&type=image&TB_iframe=1&width=640&height=469"><?php _e('Browse','pandathemes'); ?></a>
					<small><?php _e('Enter an URL of image you would like to use as placeholder <br>e.q. <code>http://yoursite.com/image.png</code> <br><br>JPG, GIF or PNG format allowed.','pandathemes'); ?></small>
				</td>
			</tr></table>
		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Custom default gravatar','pandathemes'); ?></legend>
			<table id="logo_i"><tr>
				<td class="size17">
					<?php
						$src = $theme_options['gravatar'];

						// For multisite WordPress
						global $blog_id;
						if (isset($blog_id) && $blog_id > 1) {
							$imageParts = explode('/files/', $src);
							if (isset($imageParts[1])) {
								$path = '/blogs.dir/'.$blog_id.'/files/'.$imageParts[1];
							}
						}

						// For default WordPress
						else {
							$path = $src;
						}

						$thumb = ($src)
							? $path
							: $template_url.'/images/pandaGravatar.png';
					?>
					<img src="<?php echo $thumb ?>" width="100" height="100" />
				</td>
				<td class="size80">
					<input type="text" name="gravatar" value="<?php echo $theme_options['gravatar'] ?>" class="input size60" id="gravatar-input" size="60" />
					<a class="thickbox button select-image" id="gravatar" href="media-upload.php&type=image&TB_iframe=1&width=640&height=469"><?php _e('Browse','pandathemes'); ?></a>
					<small><?php _e('Enter an URL of image you would like to use as default gravatar <br>e.q. <code>http://yoursite.com/image.jpg</code> <br><br>Do not forget <a href="' . get_bloginfo('home') . '/wp-admin/options-discussion.php#submit">to apply</a> custom gravatar.','pandathemes'); ?></small>
				</td>
			</tr></table>
		</fieldset>

	</div><!-- end posts -->


   <!----------------------------------------------------------------------------
								M I S C
	----------------------------------------------------------------------------->
	<div id="pages">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Pages','pandathemes'); ?></legend>

			<table><tr>
				<td class="size17">&nbsp;
					
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="pages_metas" value="enable" <?php if ($theme_options['pages_metas']=='enable') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Comments & Meta','pandathemes'); ?></span>
							<small class="no-p"><?php _e('Display comments and meta on pages.','pandathemes'); ?></small>
						</td>
					</tr></table>
				</td>
			</tr></table>

		</fieldset>

   	</div><!-- end misc -->


   <!----------------------------------------------------------------------------
								H E L P
	----------------------------------------------------------------------------->
	<div id="help">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Quick self-help','pandathemes'); ?></legend>

			<?php _e('In case you have a question about theme settings and installation please take a look these resources. Most likely the answer for your question already exists. You can save the time by reading:','pandathemes'); ?>

			<div class="clear h20"><!-- --></div>

			<ol>
				<li>
					<strong><?php _e('Documentation','pandathemes'); ?></strong><br />
					<?php _e("You can find the theme documentation at the archive you have downloaded. Please, take a look a Documentation folder at an archive, you've downloaded.",'pandathemes'); ?>
					<div class="clear h10"><!-- --></div>
				</li>
			<li>
				<strong><?php _e('Manuals on demo-site','pandathemes'); ?></strong><br />
				<?php _e('Please, browse the manuals at','pandathemes'); ?> - <a href="http://topbusiness.pandathemes.com/" target="_blank">topbusiness.pandathemes.com</a>
				<div class="clear h10"><!-- --></div>
			</li>
			<li>
				<strong><?php _e('Support forum','pandathemes'); ?></strong><br />
				<?php _e('Please, browse the topics at','pandathemes'); ?> - <a href="http://support.pandathemes.com/" target="_blank">support.pandathemes.com</a>
				<div class="clear h10"><!-- --></div>
			</li>
			</ol>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Support forum','pandathemes'); ?></legend>
			<?php _e('In case you have not found an answer for your question by that way, please, start a new topic on this forum','pandathemes'); ?> - <a href="http://support.pandathemes.com/forum.php?id=10" target="_blank">support.pandathemes.com/forum.php?id=10</a>
			<div class="clear h10"><!-- --></div>
			
			<small>
			<a href="http://pandathemes.com/support/faq/" target="_blank"><?php _e('FAQ','pandathemes'); ?></a> &nbsp;&bull;&nbsp; 
			<a href="http://pandathemes.com/support-policy/" target="_blank"><?php _e('Support Policy','pandathemes'); ?></a> &nbsp;&bull;&nbsp; 
			<a href="http://pandathemes.com/terms-and-conditions/" target="_blank"><?php _e('Terms and Conditions','pandathemes'); ?></a> &nbsp;&bull;&nbsp; 
			<a href="http://pandathemes.com/licensing/" target="_blank"><?php _e('Licensing','pandathemes'); ?></a>
			</small>
		</fieldset>

	</div><!-- end help -->

   
</div>               
<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes'); ?>" name="cp_save"/></p>
</div><!-- end adminarea -->
</form>