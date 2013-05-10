<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$panda_he = get_option('panda_he');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="panda_he_action" name="panda_he_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> / <small><?php _e('Header','pandathemes'); ?></small><input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $panda_he['tab']; ?>" id="input-tab" />

<?php echo pt_get_full_theme() ?>

<div class="adminarea">

    <!-- tabs -->
<!--
    <ul class="tab-navigation">
		<li><a href="#" title="general"><?php _e('General','pandathemes') ?></a></li>
    </ul>
    <div class="clear" style="height:1%; border-top:3px solid #6d6d6d;"></div>
-->
    <div id="tabbed-content">

   <!----------------------------------------------------------------------------
								G E N E R A L   P A G E
	----------------------------------------------------------------------------->

    <div id="general">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Header','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">&nbsp;
					
				</td>
				<td>

				<table><tr>
					<td class="size3">
						<input type="checkbox" name="notice" id="notice" class="checkbox-toggle" value="enable" <?php if ($panda_he['notice']=='enable') { echo 'checked' ;} ?> /> 
					</td>
					<td>
						<span><?php _e('Notice for visitors','pandathemes') ?></span>
						<small class="no-p"><?php _e('Display notice for visitors.','pandathemes') ?></small>
						<div id="notice_data">
						<div class="clear h10"><!-- --></div>
							<textarea cols="70" rows="3" name="notice_data" class="input w97p" /><?php echo $panda_he['notice_data'] ?></textarea>
							<small class="no-p"><?php _e('The best way to catch visitor attention. Shortcodes allowed.','pandathemes') ?></small>
						</div>
					</td>
				</tr></table>

				<div class="clear h20"><!-- --></div>

				<table><tr>
					<td class="size3">
						<input type="checkbox" name="hcustom" id="hcustom" class="checkbox-toggle" value="enable" <?php if ($panda_he['hcustom']=='enable') { echo 'checked' ;} ?> /> 
					</td>
					<td>
						<span><?php _e('Custom data','pandathemes') ?></span>
						<small class="no-p"><?php _e('Display custom data on header.','pandathemes') ?></small>
						<div id="hcustom_data">
						<div class="clear h10"><!-- --></div>
							<textarea cols="70" rows="3" name="hcustom_data" class="input w97p" /><?php echo $panda_he['hcustom_data'] ?></textarea>
							<small class="no-p"><?php _e('In case you need to display some data on the header please put it on here. Shortcodes allowed.','pandathemes') ?></small>
						</div>
					</td>
				</tr></table>

				<div class="clear h20"><!-- --></div>

				<table><tr>
					<td class="size3">
						<input type="checkbox" name="lifestream" id="lifestream" class="checkbox-toggle" value="enable" <?php if ($panda_he['lifestream']=='enable') { echo 'checked' ;} ?> /> 
					</td>
					<td>
						<span><?php _e('Lifestream','pandathemes') ?></span>
						<small class="no-p"><?php _e('Display icons of the social networks accounts you have.','pandathemes') ?></small>
						<div id="lifestream_data">
						<div class="clear h10"><!-- --></div>
						<?php
							$arr = array(
								'life_rss',
								'life_twitter',
								'life_deviantart',
								'life_digg',
								'life_facebook',
								'life_flickr',
								'life_lastfm',
								'life_linkedin',
								'life_myspace',
								'life_picasa',
								'life_reddit',
								'life_skype',
								'life_stumbleupon',
								'life_technorati',
								'life_youtube',
								'life_vimeo',
								'life_blogger',
								'life_delicious',
								'life_designfloat',
								'life_designmoo'
								);
							foreach ($arr as $value) {
								echo '<input type="text" name="'.$value.'" value="'.$panda_he[$value].'" class="input '.$value.'" size="" /> ';
							};
						?>
						<small class="no-p"><?php _e('Enter an URLs of your social networks acconts. Enter your FeedBurner URL on the RSS field.','pandathemes') ?></small>

						<div class="clear h20"><!-- --></div>
						<textarea cols="70" rows="3" name="lifestream_custom" class="input w97p" /><?php echo $panda_he['lifestream_custom'] ?></textarea>
						<small class="no-p">
							<?php _e('You can apply custom icons, counters etc. Shortcodes allowed.','pandathemes') ?><br/>
							<?php _e('Draft','pandathemes') ?>: <code>&lt;a rel="nofollow" target="_blank" href="#">&lt;img src="ICON-PATH.PNG" title="TOOLTIP-GOES-HERE" class="tooltip-t" alt="">&lt;/a></code>
						</small>

						</div>
					</td>
				</tr></table>

				<div class="clear h20"><!-- --></div>

				<table><tr>
					<td class="size3">
						<input type="checkbox" name="searchform" id="searchform" value="enable" <?php if ($panda_he['searchform']=='enable') { echo 'checked' ;} ?> /> 
					</td>
					<td>
						<span><?php _e('Search','pandathemes') ?></span>
						<small class="no-p"><?php _e('Display search form.','pandathemes') ?></small>
					</td>
				</tr></table>

				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Short message at menu bar','pandathemes') ?></legend>
		<table><tr>
			<td class="size17">
				<span><?php _e('Enter here','pandathemes') ?></span>
			</td>
			<td>
				<textarea cols="30" rows="1" name="topmessage" id="topmessage" class="input" /><?php echo $panda_he['topmessage'] ?></textarea>
				<small><?php _e('e.q. Call us <code>&lt;strong></code>1.800.000.12.34<code>&lt;/strong></code>','pandathemes') ?></small>
			</td>
		</tr></table>
		</fieldset>

   	</div><!-- end General -->

</div>
<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes') ?>" name="cp_save"/></p>
</div><!-- end adminarea -->
</form>