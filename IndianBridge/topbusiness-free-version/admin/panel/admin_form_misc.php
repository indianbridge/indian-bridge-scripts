<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$panda_mi = get_option('panda_mi');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="panda_mi_action" name="panda_mi_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> / <small><?php _e('Miscellaneous','pandathemes'); ?></small><input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $panda_mi['tab']; ?>" id="input-tab" />

<?php echo pt_get_full_theme() ?>

<div class="adminarea">

    <!-- tabs -->
    <ul class="tab-navigation">
		<li><a href="#" title="general"><?php _e('Basic','pandathemes') ?></a></li>
		<li><a href="#" title="advanced"><?php _e('Advanced','pandathemes') ?></a></li>
    </ul>
    <div class="clear" style="height:1%; border-top:3px solid #6d6d6d;"></div>

    <div id="tabbed-content">

   <!----------------------------------------------------------------------------
								G E N E R A L   P A G E
	----------------------------------------------------------------------------->

    <div id="general">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Basic','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">&nbsp;
					
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="admin_bar" value="no" <?php if ($panda_mi['admin_bar']=='no') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Admin Bar','pandathemes') ?></span>
							<small class="no-p"><?php _e('Remove an admin bar from the front-end.','pandathemes') ?></small>
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
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="buy_themes" value="no" <?php if ($panda_mi['buy_themes']=='no') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Buy themes','pandathemes') ?></span>
							<small class="no-p"><?php _e('Remove "Buy themes" menu item from admin panel.','pandathemes') ?></small>
						</td>
					</tr></table>
				</td>
			</tr></table>
	
		</fieldset>

   	</div><!-- end General -->

   <!----------------------------------------------------------------------------
								A D V A N C E D   P A G E
	----------------------------------------------------------------------------->

    <div id="advanced">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Autoformatting','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Select','pandathemes') ?></span>
				</td>
				<td>
					<select name="formatting">
						<?php $formatting = $panda_mi['formatting']; ?>
						<option value="enabled" <?php if ( $formatting == 'enabled' ) : echo 'selected'; endif; ?>><?php _e('enabled','pandathemes') ?> &nbsp;</option>
						<option value="raw" <?php if ( $formatting == 'raw' ) : echo 'selected'; endif; ?>><?php _e('disabled by [raw] shortcode','pandathemes') ?> &nbsp;</option>
						<option value="disabled" <?php if ( $formatting == 'disabled' ) : echo 'selected'; endif; ?>><?php _e('disabled at all','pandathemes') ?> &nbsp;</option>
					</select>
					<small><?php _e('WordPress formatting behavior.','pandathemes') ?></span></small>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('CHMOD','pandathemes') ?></legend>

			<?php

				// Set a write permissions
				$chmod = $panda_mi['chmod'];
				$dir = TEMPLATEPATH."/cache/";

				if ( $chmod == '755' ) :
					
					if( ! chmod( $dir, 0755 ) ) :

						chmodError();

					else :

						chmod($dir, 0755);

					endif;

				else :

					if( ! chmod( $dir, 0777 ) ) :

						chmodError();

					else :

						chmod($dir, 0777);

					endif;

				endif;

				function chmodError() {
					echo ( '<div class="pa-updated-message">' . __("In accordance with your server's security settings there is no possible to set a write permissions an automatically. Please, do that by yourself or ask your hosting provider.",'pandathemes') . '</div>' );
				}

			?>

			<table><tr>
				<td class="size17">
					<span><?php _e('Select','pandathemes') ?></span>
				</td>
				<td>
					<select name="chmod">
						<option value="777" <?php if ( $chmod == '777' ) : echo 'selected'; endif; ?>>777 &nbsp;</option>
						<option value="755" <?php if ( $chmod == '755' ) : echo 'selected'; endif; ?>>755 &nbsp;</option>
					</select>
					<small><?php _e('Write permissions for ../topbusiness/cache/ directory.','pandathemes') ?></span></small>
				</td>
			</tr></table>

		</fieldset>

   	</div><!-- end General -->

</div>
<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes') ?>" name="cp_save"/></p>
</div><!-- end adminarea -->
</form>