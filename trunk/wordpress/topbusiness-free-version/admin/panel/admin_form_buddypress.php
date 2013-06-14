<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$panda_bp = get_option('panda_bp');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="panda_bp_action" name="panda_bp_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> / <small>BuddyPress</small> <input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $panda_bp['tab']; ?>" id="input-tab" />

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

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Compatibility','pandathemes') ?></legend>

			<table><tr>
				<td class="size3">
					<input type="checkbox" name="compatibility" value="yes" <?php if ($panda_bp['compatibility']=='yes') { echo 'checked' ;} ?> /> 
				</td>
				<td>
					<span><?php _e('Enabled','pandathemes') ?></span>
					<small class="no-p"><?php _e('An activation of BuddyPress compatibility.','pandathemes') ?></span></small>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Admin bar','pandathemes') ?></legend>

			<table><tr>
				<td class="size3">
					<input type="checkbox" name="admin_bar" value="yes" <?php if ($panda_bp['admin_bar']=='yes') { echo 'checked' ;} ?> /> 
				</td>
				<td>
					<span><?php _e('Enabled','pandathemes') ?></span>
					<small class="no-p"><?php _e('Display BuddyPress admin bar on front-end.','pandathemes') ?></span></small>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Achievements','pandathemes') ?></legend>

			Full version of TopBusiness theme required for getting a compatibility with <a target="_blank" href="http://wordpress.org/extend/plugins/achievements/">Achievements</a> plugin.
			<div class="clear h5"><!-- --></div>

		</fieldset>

   	</div><!-- end General -->
  
</div>               
<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes') ?>" name="cp_save"/></p>
</div><!-- end adminarea -->
</form>