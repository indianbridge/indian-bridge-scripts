<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$panda_pr = get_option('panda_pr');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="panda_pr_action" name="panda_pr_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> / <small><?php _e('Portfolio of products','pandathemes'); ?></small><input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $panda_pr['tab']; ?>" id="input-tab" />

<?php echo pt_get_full_theme() ?>

<div class="adminarea">

    <!-- tabs -->
    <ul class="tab-navigation">
		<li><a href="#" title="general"><?php _e('General','pandathemes') ?></a></li>
		<li><a href="#" title="catalog"><?php _e('Catalog page','pandathemes') ?></a></li>
		<li><a href="#" title="product"><?php _e('Product page','pandathemes') ?></a></li>
    </ul>
    <div class="clear" style="height:1%; border-top:3px solid #6d6d6d;"></div>

    <div id="tabbed-content">

   <!----------------------------------------------------------------------------
								G E N E R A L   P A G E
	----------------------------------------------------------------------------->

    <div id="general">

		<fieldset class="panda-admin-fieldset">
	
			<table><tr>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" class="checkbox-toggle" id="products_acvitation" name="products" value="enable" <?php if ($panda_pr['products']=='enable') { echo 'checked' ;} ?> /> 
						</td>
						<td>
							<span><?php _e('Enable','pandathemes') ?></span>
							<small class="no-p"><?php _e('Checkbox that in case you need to add products on your website.','pandathemes') ?></small>
						</td>
					</tr></table>
				</td>
			</tr></table>
	
		</fieldset>

		<div id="products_acvitation_data">

			<fieldset class="panda-admin-fieldset"><legend><?php _e('Template','pandathemes') ?></legend>
	
				<div class="tmpl_radio">
					<label class="lable-img" for="p1_template"><img src="<?php echo $template_url ?>/admin/graphics/templates/products_1.gif" width="135" height="110"></label>
					<input type="radio" value="t1" name="product_template" id="p1_template" checked="checked" />
					<label for="p1_template"><?php _e('Template','pandathemes') ?> #1</label>
				</div>
	
				<div class="tmpl_radio">
					<label title="Available for FULL version only" class="lable-img" for="p2_template"><img src="<?php echo $template_url ?>/admin/graphics/templates/products_2.gif" width="135" height="110"></label>
					<input disabled="disabled" title="Available for FULL version only" type="radio" value="t2" name="product_template" id="p2_template" />
					<label for="p2_template" title="Available for FULL version only"><?php _e('Template','pandathemes') ?> #2</label>
				</div>
	
				<small><?php _e('Select a template for the catalog page.','pandathemes') ?></small>
			</fieldset>

		</div><!-- end products_activation_data -->	


   	</div><!-- end General -->


   <!----------------------------------------------------------------------------
								C A T A L O G   P A G E
	----------------------------------------------------------------------------->

	<div id="catalog">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Catalog page','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Products per page','pandathemes') ?></span>
				</td>
				<td>
					<select name="posts_per_page">
						<?php
							$q = $panda_pr['posts_per_page'];
							$arr = array('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21');
							foreach ($arr as $value) {
								$out = '<option value="'.$value.'"';
								if ( $q == $value ) : $out .= ' selected'; endif;
								$out .= '>'.$value.' &nbsp;</option>';
								echo $out;
							};
						?>
					</select>
					<small class="no-p"><?php _e('A quantity of products per catalog page.','pandathemes') ?></small>
				</td>
			</tr></table>

			<div class="clear h20"><!-- --></div>

			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="after_catalog" id="after_catalog" class="checkbox-toggle" value="enable" /> 
						</td>
						<td>
							<span><?php _e('After catalog','pandathemes') ?></span>
							<small class="no-p"><?php _e('Display custom data in the end of catalog page. Shortcodes allowed.','pandathemes') ?></small>
							<div id="after_catalog_data">
							<div class="clear h10"><!-- --></div>
								<textarea cols="70" rows="5" name="after_catalog_data" class="input w97p" /><?php echo $panda_pr['after_catalog_data'] ?></textarea>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

		</fieldset>

	</div><!-- end posts -->


   <!----------------------------------------------------------------------------
								P R O D U C T   P A G E
	----------------------------------------------------------------------------->

	<div id="product">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Product page','pandathemes') ?></legend>
			<table><tr>
				<td class="size17">&nbsp;
				</td>
				<td>
					<table><tr>
						<td class="size3">
							<input type="checkbox" name="reviews" value="yes" <?php if ($panda_pr['reviews']) { echo 'checked' ;} ?> />
						</td>
						<td>
							<?php _e('Reviews & Ratings','pandathemes') ?>
							<small class="no-p"><?php _e('Allow visitors to leave a reviews.','pandathemes') ?></small>
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
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="related_products" id="related_products" class="checkbox-toggle" value="yes" />
						</td>
						<td>
							<?php _e('Related products','pandathemes') ?>
							<small class="no-p"><?php _e('Display products from the same catalog.','pandathemes') ?></small>
							<div id="related_products_data">
								<div class="h7"><!-- --></div>
								<select name="related_products_qty">
									<?php
										$q = $panda_pr['related_products_qty'];
										$arr = array(3,4,6,8,9,12,15,16);
										foreach ($arr as $value) {
											$out = '<option value="'.$value.'"';
											if ( $q == $value ) : $out .= ' selected'; endif;
											$out .= '>'.$value.' &nbsp;</option>';
											echo $out;
										};
									?>
								</select>
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
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="after_product" id="after_product" class="checkbox-toggle" value="enable" /> 
						</td>
						<td>
							<span><?php _e('After product','pandathemes') ?></span>
							<small class="no-p"><?php _e('Display custom data below the product page. Shortcodes allowed.','pandathemes') ?></small>
							<div id="after_product_data">
							<div class="clear h10"><!-- --></div>
								<textarea cols="70" rows="5" name="after_product_data" class="input w97p" /><?php echo $panda_pr['after_product_data'] ?></textarea>
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
							<input disabled="disabled" title="Available for FULL version only" type="checkbox" name="after_purchase" id="after_purchase" class="checkbox-toggle" value="enable" /> 
						</td>
						<td>
							<span><?php _e('After purchase','pandathemes') ?></span>
							<small class="no-p"><?php _e('Display custom data below the purchase section. Shortcodes allowed.','pandathemes') ?></small>
							<div id="after_purchase_data">
							<div class="clear h10"><!-- --></div>
								<textarea cols="70" rows="5" name="after_purchase_data" class="input w97p" /><?php echo $panda_pr['after_purchase_data'] ?></textarea>
							</div>
						</td>
					</tr></table>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Names of tabs','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
				</td>
				<td class="size80">

					<input type="text" name="tab_desc" value="<?php $z = ( $panda_pr['tab_desc'] ) ? $panda_pr['tab_desc'] : __('Description','pandathemes'); echo $z; ?>" class="input size30" size="30" />
					<small class="no-p"><?php _e('A common information.','pandathemes') ?></small>
					<div class="clear h20"><!-- --></div>

					<input type="text" name="tab_spec" value="<?php $z = ( $panda_pr['tab_spec'] ) ? $panda_pr['tab_spec'] : __('Specs','pandathemes'); echo $z; ?>" class="input size30" size="30" />
					<small class="no-p"><?php _e('Specifications, roles, ingredients etc.','pandathemes') ?></small>
					<div class="clear h20"><!-- --></div>

					<input type="text" name="tab_imgs" value="<?php $z = ( $panda_pr['tab_imgs'] ) ? $panda_pr['tab_imgs'] : __('Images','pandathemes'); echo $z; ?>" class="input size30" size="30" />
					<small class="no-p"><?php _e('Photos, screenshots, schemes, graphs etc.','pandathemes') ?></small>
					<div class="clear h20"><!-- --></div>

					<input type="text" name="tab_rews" value="<?php $z = ( $panda_pr['tab_rews'] ) ? $panda_pr['tab_rews'] : __('Reviews','pandathemes'); echo $z; ?>" class="input size30" size="30" />
					<small class="no-p"><?php _e('Testimonials, feedbacks, customer opinions etc.','pandathemes') ?></small>

				</td>
			</tr></table>

		</fieldset>

	</div><!-- end products -->

  
</div>               
<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes') ?>" name="cp_save"/></p>
</div><!-- end adminarea -->
</form>