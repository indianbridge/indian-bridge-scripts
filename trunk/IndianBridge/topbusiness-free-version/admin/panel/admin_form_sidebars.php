<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$panda_sb = get_option('panda_sb');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="panda_sb_action" name="panda_sb_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> / <small><?php _e('Sidebars','pandathemes'); ?></small><input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $panda_sb['tab']; ?>" id="input-tab" />

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

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Additional sidebars','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Quantity','pandathemes') ?></span>
				</td>
				<td>
					<input type="text" name="additional_sidebars" value="<?php echo $panda_sb['additional_sidebars'] ?>" class="input" size="3"  />
					<small><?php _e('Enter the number of additional sidebars. Min qty of additional sidebars is 2.','pandathemes') ?></small>
					<small><?php _e('NOTE! Do not set a value less than an amount of the used sidebars you have. This may lead to the loss of your data.','pandathemes') ?></small>
				</td>
			</tr></table>
	
		</fieldset>	

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Footer sidebars','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Select one','pandathemes') ?></span>
				</td>
				<td>
					<div class="tmpl_radio">
						<label class="lable-img" for="footer_sidebars_1s"><img src="<?php echo $template_url ?>/admin/graphics/footers/1.png" width="40" height="40"></label>
						<input type="radio" value="1" name="footer_sidebars" id="footer_sidebars_1s" <?php if ($panda_sb['footer_sidebars']=='1' || $panda_sb['footer_sidebars']=='' ):?> checked="checked" <?php endif; ?> />
					</div>
					<?php
						$arr = array('2','3','4','5','6','no');
						foreach ($arr as $value) {
							$out = '<div class="tmpl_radio">';
							if ( $value != 'no' ) :
								$out .= '<label title="Available for FULL version only" class="lable-img" for="footer_sidebars_'.$value.'s"><img src="'.$template_url.'/admin/graphics/footers/'.$value.'.png" width="40" height="40"></label>';
							else :
								$out .= '<label class="lable-img" for="footer_sidebars_'.$value.'s"><img src="'.$template_url.'/admin/graphics/footers/'.$value.'.png" width="40" height="40"></label>';
							endif;
							if ( $panda_sb['footer_sidebars'] == $value ) : $ch = 'checked="checked"'; else : $ch = ''; endif;
							if ( $value != 'no' ) : $free = 'disabled="disabled" title="Available for FULL version only" '; else : $free = ''; endif;
							$out .= '<input type="radio" value="'.$value.'" name="footer_sidebars" id="footer_sidebars_'.$value.'s" '.$free.$ch.' />';
							$out .= '</div>';
							echo $out;
						};
					?>
					<small><?php _e('Select a variant of the footer sidebars. Please note, some of custom widgets may have a fixed or limited width.','pandathemes') ?></small>
				</td>
			</tr></table>

		</fieldset>	

   	</div><!-- end General -->

</div>
<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes') ?>" name="cp_save"/></p>
</div><!-- end adminarea -->
</form>