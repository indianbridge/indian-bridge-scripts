<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$panda_ty = get_option('panda_ty');
	$panda_st = get_option('panda_st');
	$theme_options = get_option('ikarina'); // for login logo

	require_once(TEMPLATEPATH.'/styles/style.php');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="panda_st_action" name="panda_st_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> / <small><?php _e('Style','pandathemes'); ?></small><input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $panda_st['tab']; ?>" id="input-tab" />

<?php echo pt_get_full_theme() ?>

<div class="adminarea">

    <!-- tabs -->
    <ul class="tab-navigation">
		<li><a href="#" title="general"><?php _e('General','pandathemes') ?></a></li>
		<li><a href="#" title="st_layout"><?php _e('Layout','pandathemes') ?></a></li>
		<li><a href="#" title="st_menu"><?php _e('Menu','pandathemes') ?></a></li>
		<li><a href="#" title="st_footer"><?php _e('Footer','pandathemes') ?></a></li>
		<li><a href="#" title="st_custom"><?php _e('Custom CSS','pandathemes') ?></a></li>
    </ul>
    <div class="clear" style="height:1%; border-top:3px solid #6d6d6d;"></div>

    <div id="tabbed-content">

   <!----------------------------------------------------------------------------
								G E N E R A L   P A G E
	----------------------------------------------------------------------------->

    <div id="general">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Basic style','pandathemes') ?></legend>
		<table><tr>
			<td class="size17">
				<span><?php _e('Select a style','pandathemes') ?></span>
			</td>
			<td>
			
				<?php $style = $panda_st['style']; ?>

				<div class="tmpl_radio">
					<label class="lable-img" for="style_1s"><img src="<?php echo $template_url ?>/admin/graphics/styles/1.png" width="40" height="40"></label>
					<input type="radio" value="1" name="style" id="style_1s" <?php if ($style==1):?> checked="checked" <?php endif; ?> />
					<label for="style_1s">1</label>
				</div>

				<?php
					$arr = array(2,3,4,5,6,7,8,9,10);
					foreach ($arr as $value) {
						$out = '<div class="tmpl_radio">';
							$out .= '<label class="lable-img" for="style_'.$value.'s"><img src="'.$template_url.'/admin/graphics/styles/'.$value.'.png" width="40" height="40"></label>';
	
							$ch = '';
							if ($style==$value) :
								$out .= '<input type="radio" value="'.$value.'" name="style" id="style_'.$value.'s" checked="checked" '.$ch.' />';
							else :
								$out .= '<input type="radio" value="'.$value.'" name="style" id="style_'.$value.'s" '.$ch.' />';
							endif;

							$out .= '<label for="style_'.$value.'s"> '.$value.' </label>';
						$out .= '</div>';
						echo $out;
					};
				?>

				<small><?php _e('Select a basic style. All changes (below) will be applied to the chosen style.','pandathemes') ?></small>
			</td>
		</tr></table>
		</fieldset>

   	</div><!-- end General -->


   <!----------------------------------------------------------------------------
								L A Y O U T
	----------------------------------------------------------------------------->

	<div id="st_layout">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Layout','pandathemes') ?></legend>
		<table><tr>
			<td class="size17">
				<span><?php _e('Background','pandathemes') ?></span>
			</td>
			<td>
				<div class="color-thumb" id="background_color_thumb" style="background-color:#<?php if ($panda_st['background_color']<>'') {echo $panda_st['background_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
				<input type="text" name="background_color" id="background_color" value="<?php echo $panda_st['background_color'] ?>" class="input" size="8" />
				<small><?php _e('Select the primary color of background.','pandathemes') ?></small>
			</td>
		</tr></table>

		<div class="clear h20"><!-- --></div>

		<table><tr>
			<td class="size17">
				<span><?php _e('Pattern','pandathemes') ?></span>
			</td>
			<td>

				<div><!-- DO NOT REMOVE THIS DIV -->
					<input type="text" name="pattern" value="<?php echo $panda_st['pattern'] ?>" class="input input-panda size20 fl" size="20" />
					<input type="text" name="pattern_current" value="<?php echo $panda_st['pattern_current'] ?>" class="input input-panda-current none size8 fl" size="8" />
					<img class="browse-button" src="<?php echo $template_url ?>/admin/graphics/folder_button.gif" alt="Browse"> 

					<div class="preview-panda">
						<div class="pa-window">
							<div class="pa-wrapper">
								<div class="pa-preview" id="pa-pattern">
									<div id="pa-slider" style="background-color:#<?php if ($panda_st['background_color']<>'') {echo $panda_st['background_color'];} else {echo 'EEE';} ?>;">
										<?php
											$arr = array('01','02','03','04','05','06','07','08','09','10','11','12','13','14','15','16','17','18','19','20','21','22','23','24','25','26');
											foreach ($arr as $value) {
												$out = '<div id="pa-'.$value.'s"><!-- slide --></div>';
												echo $out;
											};
										?>
									</div>
								</div>
								<div class="pa-list">
									<ol>

										<?php
										
											$arr = array(
												'default'				=> '- '.__("default",'pandathemes').' -',
												'vertical_lines'		=> __("Vertical lines",'pandathemes'),
												'vertical_lines_2'		=> __("Vertical lines",'pandathemes').' #2',
												'noise'					=> __("Noise",'pandathemes'),
												'cells'					=> __("Cells",'pandathemes'),
												'grid'					=> __("Grid",'pandathemes'),
												'grid_2'				=> __("Grid",'pandathemes').' #2',
												'jeans'					=> __("Jeans",'pandathemes'),
												'tissue'				=> __("Tissue",'pandathemes'),
												'gradient-white-60'		=> __("Gradient White",'pandathemes').' 60%',
												'gradient-black-60'		=> __("Gradient Black",'pandathemes').' 60%',
												'oblique_lines'			=> __("Oblique lines",'pandathemes'),
												'horizontal_lines'		=> __("Horizontal lines",'pandathemes'),
												'vertical_lines_3'		=> __("Vertical lines",'pandathemes').' #3',
												'vertical_lines_4'		=> __("Vertical lines",'pandathemes').' #4',
												'square'				=> __("Square",'pandathemes'),
												'dotted'				=> __("Dotted",'pandathemes'),
												'leaves'				=> __("Leaves",'pandathemes'),
												'square_2'				=> __("Square",'pandathemes').' #2',
												'rain'					=> __("Rain",'pandathemes'),
												'carbon'				=> __("Carbon",'pandathemes'),
												'oblique_lines_2'		=> __("Oblique lines",'pandathemes').' #2',
												'oblique_lines_3'		=> __("Oblique lines",'pandathemes').' #3',
												'flower-pattern'		=> __("Flowers",'pandathemes'),
												'bubbles_1'				=> __("Bubbles",'pandathemes'),
												'ornament_flowers'		=> __("Ornament",'pandathemes'),
												'empty'					=> __("No pattern",'pandathemes')
											);
					
											foreach ( $arr as $key => $value ) {
	
												echo '<li alt="'. $key .'">'. $value .'</li>';

											};
					
										?>

									</ol>
								</div>
							</div>
							<div class="pa-controls">
								<input type="button" class="button-primary add-new-h2 apply-button-panda" value="<?php _e('Apply pattern','pandathemes') ?>" />
								<span class="button add-new-h2 cancel-button-panda"><?php _e('Cancel','pandathemes') ?></span>
							</div>
						</div>
					</div>
				</div><!-- END DIV -->
				<small><?php _e('Select the pattern of background or set your own.','pandathemes') ?></small>
			</td>
		</tr></table>

		<div class="clear h20"><!-- --></div>

		<table><tr>
			<td class="size17">
				<span><?php _e('Custom pattern','pandathemes') ?></span>
			</td>
			<td>
				<input type="text" name="background_image" value="<?php echo $panda_st['background_image'] ?>" class="input size60" id="custom-pattern-input" size="60" />
				<a class="thickbox button select-image" id="custom-pattern" href="media-upload.php&type=image&TB_iframe=1&width=640&height=469"><?php _e('Browse','pandathemes') ?></a>
				<small>
					<?php _e('e.q.','pandathemes') ?> <code>http://yoursite.com/images/pattern.png</code> <br>
					<?php _e('NOTE! This input field takes precedence over the previous.','pandathemes') ?>
				</small>
			</td>
		</tr></table>

		<div class="clear h20"><!-- --></div>

		<table><tr>
			<td class="size17">
				<span><?php _e('Pattern options','pandathemes') ?></span>
			</td>
			<td>
				<span><?php _e('Position','pandathemes') ?>:</span>
				<select name="background_image_position">
					<?php
						$p = $panda_st['background_image_position'];

						$arr = array('top center','top left','top right','bottom center','bottom left','bottom right');
						foreach ($arr as $value) {
						
							$out = '<option value="'.$value.'"';
								if ( $p == $value ) : $out .= ' selected'; endif;
								$out .= '>';
							$out .= __($value,'pandathemes');
							$out .= ' &nbsp;</option>';

							echo $out;
						};
					?>
				</select>

				&nbsp; &nbsp; &nbsp;

				<span><?php _e('Tiling','pandathemes') ?>:</span>
				<select name="background_repeat">
					<?php $r = $panda_st['background_repeat']; ?>
					<option value="repeat-x" <?php if ( $r =='repeat-x' ) : echo 'selected'; endif; ?>><?php _e('repeat-x','pandathemes') ?> &nbsp;</option>
					<option value="repeat-y" <?php if ( $r =='repeat-y' ) : echo 'selected'; endif; ?>><?php _e('repeat-y','pandathemes') ?> &nbsp;</option>
					<option value="repeat" <?php if ( $r =='repeat' ) : echo 'selected'; endif; ?>><?php _e('repeat','pandathemes') ?> &nbsp;</option>
					<option value="no-repeat" <?php if ( $r =='no-repeat' ) : echo 'selected'; endif; ?>><?php _e('no-repeat','pandathemes') ?> &nbsp;</option>
				</select>

				&nbsp; &nbsp; &nbsp;

				<span><?php _e('Attachment','pandathemes') ?>: &nbsp; </span>
				<input type="checkbox" name="background_attachment" value="enable" <?php if ($panda_st['background_attachment']=='enable') { echo 'checked' ;} ?> /> 
				<span><?php _e('fixed','pandathemes') ?></span>
			</td>
		</tr></table>
		</fieldset>

	</div><!-- end posts -->


   <!----------------------------------------------------------------------------
								M E N U
	----------------------------------------------------------------------------->

	<div id="st_menu">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Menu','pandathemes') ?></legend>

		<table><tr>
			<td class="size17">
				<span><?php _e('Approx preview','pandathemes') ?></span>
			</td>
			<td>
				<table id="demo-menu"><tr>
					<td class="demo first" style="
						background-color:#<?php if ($panda_st['menu_bg_color']<>'') { echo $panda_st['menu_bg_color'];} else { echo '191919';} ?>;
						color:#<?php if ($panda_st['menu_color']<>'') { echo $panda_st['menu_color'];} else { echo 'CCC';} ?>;">Menu item
					</td>
					<td class="demo" style="
						background-color:#<?php if ($panda_st['menu_bg_color']<>'') { echo $panda_st['menu_bg_color'];} else { echo '191919';} ?>;
						color:#<?php if ($panda_st['menu_color']<>'') { echo $panda_st['menu_color'];} else { echo 'CCC';} ?>;">Menu item
					</td>
					<td class="demo-current" style="
						background-color:#<?php if ($panda_st['menu_selected_bg_color']<>'') { echo $panda_st['menu_selected_bg_color'];} else { echo '555';} ?>;
						color:#<?php if ($panda_st['menu_selected_color']<>'') { echo $panda_st['menu_selected_color'];} else { echo 'FFF';} ?>;">Current item
					</td>
					<td class="demo-hover" style="
						background-color:#<?php if ($panda_st['menu_hover_bg_color']<>'') { echo $panda_st['menu_hover_bg_color'];} else { echo '65a4c1';} ?>;
						color:#<?php if ($panda_st['menu_hover_color']<>'') { echo $panda_st['menu_hover_color'];} else { echo 'FFF';} ?>;">By hover
					</td>
					<td class="demo last" style="
						background-color:#<?php if ($panda_st['menu_bg_color']<>'') { echo $panda_st['menu_bg_color'];} else { echo '191919';} ?>;
						color:#<?php if ($panda_st['menu_color']<>'') { echo $panda_st['menu_color'];} else { echo 'CCC';} ?>;">Menu item
					</td>
				</tr></table>
			</td>
		</tr></table>

		<div class="clear h20"><!-- --></div>
		<div class="clear h10"><!-- --></div>

		<table><tr>
			<td class="size17">
				<span>&nbsp;</span>
			</td>
			<td>
				<div class="color-thumb" id="menu_color_thumb" style="background-color:#<?php if ($panda_st['menu_color']<>'') {echo $panda_st['menu_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
				<input type="text" name="menu_color" id="menu_color" value="<?php echo $panda_st['menu_color'] ?>" class="input" size="8" />
				<small><?php _e('Color','pandathemes') ?></small>
			</td>
			<td>
				<div class="color-thumb" id="menu_bg_color_thumb" style="background-color:#<?php if ($panda_st['menu_bg_color']<>'') {echo $panda_st['menu_bg_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
				<input type="text" name="menu_bg_color" id="menu_bg_color" value="<?php echo $panda_st['menu_bg_color'] ?>" class="input" size="8" />
				<small><?php _e('Background','pandathemes') ?></small>
			</td>
		</tr></table>

		<div class="clear h20"><!-- --></div>

		<table><tr>
			<td class="size17">
				<span><?php _e('By hover','pandathemes') ?></span>
			</td>
			<td>
				<div class="color-thumb" id="menu_hover_color_thumb" style="background-color:#<?php if ($panda_st['menu_hover_color']<>'') {echo $panda_st['menu_hover_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
				<input type="text" name="menu_hover_color" id="menu_hover_color" value="<?php echo $panda_st['menu_hover_color'] ?>" class="input" size="8" />
				<small><?php _e('Color','pandathemes') ?></small>
			</td>
			<td>
				<div class="color-thumb" id="menu_hover_bg_color_thumb" style="background-color:#<?php if ($panda_st['menu_hover_bg_color']<>'') {echo $panda_st['menu_hover_bg_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
				<input type="text" name="menu_hover_bg_color" id="menu_hover_bg_color" value="<?php echo $panda_st['menu_hover_bg_color'] ?>" class="input" size="8" />
				<small><?php _e('Background','pandathemes') ?></small>
			</td>
		</tr></table>

		<div class="clear h20"><!-- --></div>

		<table><tr>
			<td class="size17">
				<span><?php _e('Current','pandathemes') ?></span>
			</td>
			<td>
				<div class="color-thumb" id="menu_selected_color_thumb" style="background-color:#<?php if ($panda_st['menu_selected_color']<>'') {echo $panda_st['menu_selected_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
				<input type="text" name="menu_selected_color" id="menu_selected_color" value="<?php echo $panda_st['menu_selected_color'] ?>" class="input" size="8" />
				<small><?php _e('Color','pandathemes') ?></small>
			</td>
			<td>
				<div class="color-thumb" id="menu_selected_bg_color_thumb" style="background-color:#<?php if ($panda_st['menu_selected_bg_color']<>'') {echo $panda_st['menu_selected_bg_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
				<input type="text" name="menu_selected_bg_color" id="menu_selected_bg_color" value="<?php echo $panda_st['menu_selected_bg_color'] ?>" class="input" size="8" />
				<small><?php _e('Background','pandathemes') ?></small>

			</td>
		</tr></table>

		<div class="clear h20"><!-- --></div>

		<table><tr>
			<td class="size17">
				<span><?php _e('Divider','pandathemes') ?></span>
			</td>
			<td>
				<select name="separator">
					<?php $d = $panda_st['separator']; ?>
					<option value="default" <?php if ( $d =='default' ) : echo 'selected'; endif; ?>>- <?php _e('default','pandathemes') ?> -</option>
					<?php
						$arr = array(
							'line-white-10',
							'line-white-20',
							'line-white-40',
							'line-white-60',
							'line-black-10',
							'line-black-20',
							'line-black-40',
							'line-black-60'
						);

						foreach ($arr as $value) {
						
							$out = '<option value="'.$value.'"';
								if ( $d == $value ) : $out .= ' selected'; endif;
								$out .= '>';
							$out .= __($value.'%-opacity','pandathemes');
							$out .= ' &nbsp;</option>';

							echo $out;
						};
					?>
					<option value="empty" <?php if ( $d =='empty' ) : echo 'selected'; endif; ?>><?php _e('None','pandathemes') ?></option>
				</select>
				<small><?php _e('Select a vertical divider for the top level menu items.','pandathemes') ?></small>
			</td>
		</tr></table>

		</fieldset>

	</div><!-- end products -->


   <!----------------------------------------------------------------------------
								F O O T E R
	----------------------------------------------------------------------------->

	<div id="st_footer">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Footer','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Approx preview','pandathemes') ?></span>
				</td>
				<td>
					<table id="demo-footer"><tr>
						<td class="demo first" style="
							background-color:#<?php if ($panda_st['footer_bg']<>'') { echo $panda_st['footer_bg'];} else { echo '191919';} ?>;
							color:#<?php if ($panda_st['footer_text_color']<>'') { echo $panda_st['footer_text_color'];} else { echo 'CCC';} ?>;
							">
								<span class="demo-title" style="color:#<?php if ($panda_st['footer_titles_color']<>'') { echo $panda_st['footer_titles_color'];} else { echo 'FFF';} ?>;">Sample title</span>
								Sed sed neque metus. Nullam mauris metus, bibendum at facilisis nec, ullamcorper sit amet orci.
								<span class="demo-link" style="color:#<?php if ($panda_st['footer_links_color']<>'') { echo $panda_st['footer_links_color'];} else { echo '5accff';} ?>;">Link...</span>
						</td>
						<td class="demo last" style="
							background-color:#<?php if ($panda_st['footer_bg']<>'') { echo $panda_st['footer_bg'];} else { echo '191919';} ?>;
							color:#<?php if ($panda_st['footer_text_color']<>'') { echo $panda_st['footer_text_color'];} else { echo 'CCC';} ?>;
							">
								<span class="demo-title" style="color:#<?php if ($panda_st['footer_titles_color']<>'') { echo $panda_st['footer_titles_color'];} else { echo 'FFF';} ?>;">Sample title</span>
								Proin quis elit suscipit massa luctus imperdiet non a mauris. Duis sodales convallis purus.
								<span class="demo-link" style="color:#<?php if ($panda_st['footer_links_color']<>'') { echo $panda_st['footer_links_color'];} else { echo '5accff';} ?>;">Link...</span>
						</td>
					</tr></table>
				</td>
			</tr></table>
	
			<div class="clear h20"><!-- --></div>
			<div class="clear h10"><!-- --></div>
	
			<table><tr>
				<td class="size17">
					<span><?php _e('Background','pandathemes') ?></span>
				</td>
				<td>
					<div class="color-thumb" id="footer_bg_thumb" style="background-color:#<?php if ($panda_st['footer_bg']<>'') {echo $panda_st['footer_bg'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
					<input type="text" name="footer_bg" id="footer_bg" value="<?php echo $panda_st['footer_bg'] ?>" class="input" size="8" />
					<small><?php _e('Select a background color of the footer.','pandathemes') ?></small>
				</td>
			</tr></table>
	
			<div class="clear h20"><!-- --></div>
	
			<table><tr>
				<td class="size17">
					<span><?php _e('Titles','pandathemes') ?></span>
				</td>
				<td>
					<div class="color-thumb" id="footer_titles_color_thumb" style="background-color:#<?php if ($panda_st['footer_titles_color']<>'') {echo $panda_st['footer_titles_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
					<input type="text" name="footer_titles_color" id="footer_titles_color" value="<?php echo $panda_st['footer_titles_color'] ?>" class="input" size="8" />
					<small><?php _e('Select a color of the titles on footer.','pandathemes') ?></small>
				</td>
			</tr></table>
	
			<div class="clear h20"><!-- --></div>
	
			<table><tr>
				<td class="size17">
					<span><?php _e('Text','pandathemes') ?></span>
				</td>
				<td>
					<div class="color-thumb" id="footer_text_color_thumb" style="background-color:#<?php if ($panda_st['footer_text_color']<>'') {echo $panda_st['footer_text_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
					<input type="text" name="footer_text_color" id="footer_text_color" value="<?php echo $panda_st['footer_text_color'] ?>" class="input" size="8" />
					<small><?php _e('Select a color of the text on footer.','pandathemes') ?></small>
				</td>
			</tr></table>
	
			<div class="clear h20"><!-- --></div>
	
			<table><tr>
				<td class="size17">
					<span><?php _e('Links','pandathemes') ?></span>
				</td>
				<td>
					<div class="color-thumb" id="footer_links_color_thumb" style="background-color:#<?php if ($panda_st['footer_links_color']<>'') {echo $panda_st['footer_links_color'];} else {echo 'EEE';} ?>;"><!-- COLOR THUMB --></div>
					<input type="text" name="footer_links_color" id="footer_links_color" value="<?php echo $panda_st['footer_links_color'] ?>" class="input" size="8" />
					<small><?php _e('Select a color of the footer links.','pandathemes') ?></small>
				</td>
			</tr></table>

		</fieldset>

	</div><!-- end products -->


   <!----------------------------------------------------------------------------
								C U S T O M   C S S
	----------------------------------------------------------------------------->

	<div id="st_custom">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Custom CSS','pandathemes') ?></legend>
			<table><tr>
				<td class="size17">
					<span><?php _e('Insert here','pandathemes') ?></span>
				</td>
				<td>
					<textarea cols="70" rows="20" name="custom_css" id="custom_css" class="input" /><?php echo $panda_st['custom_css'] ?></textarea>
					<small>
					<?php _e('Please DO NOT modify a source files, because you may lose your changes after the future theme updates in this case. If you need to make a deep style changes please insert your custom styles on this textarea. This is a safety way.','pandathemes') ?>
					</small>
				</td>
			</tr></table>
		</fieldset>

	</div><!-- end products -->


</div>               

<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes') ?>" name="cp_save"/></p>

   <!----------------------------------------------------------------------------
								R E S E T   S T Y L E S
	----------------------------------------------------------------------------->

	<p style="text-align:center;"><span class="button add-new-h2 reset-style-panda">Reset style settings</span></p>
	<div id="confirmation-reset">	
		<p><?php _e('Are you sure?','pandathemes') ?></p><br>
		<span class="button add-new-h2 reset-style-panda-no"><?php _e('No, thanks','pandathemes') ?></span> &nbsp; 
		<span class="button add-new-h2 reset-style-panda-yes"><?php _e('Yes, please','pandathemes') ?></span>
	</div>
	<div id="sucess-reset-style">
		<?php _e('Style settings have been reset. Do not forget to update settings.','pandathemes') ?>
	</div>

	<div class="clear h10"><!-- --></div>

</div><!-- end adminarea -->
</form>