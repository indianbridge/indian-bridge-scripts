<?php

// by pandathemes.com



// *********************************************
//
// 	P R O D U C T S
//
// *********************************************



function product_register() {


	// 	P O S T   T Y P E

	$args = array(

		$labels = array(
				'name'					=> __('Products','pandathemes'),
				'singular_name'			=> __('Product','pandathemes'),
				'add_new'				=> __('Add New','pandathemes'),
				'add_new_item'			=> __('Add New Product','pandathemes'),
				'edit_item'				=> __('Edit Product','pandathemes'),
				'new_item'				=> __('New Product','pandathemes'),
				'view_item'				=> __('View Product','pandathemes'),
				'search_items'			=> __('Search Product','pandathemes'),
				'not_found'				=> __('No products found','pandathemes'),
				'not_found_in_trash'	=> __('No products found in Trash','pandathemes'), 
				'parent_item_colon'		=> ''
			),
			'labels'			=> $labels,
			'description'		=>	__('A group of posts which can be used as portfolio of products.','pandathemes'),
			'public'			=> true,
			'publicly_queryable'=> true,
			'show_ui'			=> true,
			'capability_type'	=> 'page',
			'menu_position'		=> 5,
			'hierarchical'		=> false,
			'rewrite'			=> array('slug' => 'portfolio', 'with_front' => true),
			'query_var'			=> true,
			'has_archive'		=> true,
			'supports'			=> array('title', 'editor', 'thumbnail', 'comments', 'page-attributes')
		);

	register_post_type( 'product' , $args );


	// 	C U S T O M   C A T E G O R I E S
	
	register_taxonomy(
		'catalog',
		array('product'),
		array(
			'hierarchical'		=> true,
			'label'				=> __('Catalogs','pandathemes'),
			'singular_label'	=> __('Catalog','pandathemes'),
			'query_var'			=> true,
			'show_ui'			=> true,
			'rewrite' =>		array( 'slug' => 'catalog' )
		)
	);


}

add_action('init', 'product_register');



// *********************************************
//
// 	R E G I S T E R   M E T A B O X E S
//
// *********************************************



function admin_init(){

	add_meta_box('prodInfo-meta', 'Product Options', 'meta_options', 'product', 'normal', 'high');

}

add_action('admin_init', 'admin_init');



// *********************************************
//
// 	S A V E   M E T A B O X E S
//
// *********************************************



function save_product(){

	global $post;

	$custom_meta_fields = 
		array(
			'tab',
			'purchase',
			'price',
			'price_old',
			'demo_url',
			'label',
			'button',
			'button_tagline',
			'button_current',
			'button_url',
			'direct_url',
			'before_button',
			'after_button',
			'custom_button',
			'demo_button',
			'demo_button_tagline',
			'desc',
			'custom_tab_title',
			'custom_tab_content'
		);

	foreach( $custom_meta_fields as $custom_meta_field ):

		update_post_meta($post->ID, $custom_meta_field, $_POST[$custom_meta_field]);

	endforeach;

}

add_action('save_post', 'save_product');



// *********************************************
//
// 	S T A R T   M E T A   O P T I O N S
//
// *********************************************



function meta_options(){

	global $post;

	$custom = get_post_custom($post->ID);

		$tab = $custom['tab'][0];
		$purchase = $custom['purchase'][0];
		$price = $custom['price'][0];
		$price_old = $custom['price_old'][0];
		$demo_url = $custom['demo_url'][0];
		$label = $custom['label'][0];
		$button = $custom['button'][0];
		$button_tagline = $custom['button_tagline'][0];
		$button_current = $custom['button_current'][0];
		$button_url = $custom['button_url'][0];
		$direct_url = $custom['direct_url'][0];
		$before_button = $custom['before_button'][0];
		$after_button = $custom['after_button'][0];
		$custom_button = $custom['custom_button'][0];
		$demo_button = $custom['demo_button'][0];
		$demo_button_tagline = $custom['demo_button_tagline'][0];
		$desc = $custom['desc'][0];
		$custom_tab_title = $custom['custom_tab_title'][0];
		$custom_tab_content = $custom['custom_tab_content'][0];



	// for product thumbnail
	$max_sum = $custom['postsum_ajax'][0] ? $custom['postsum_ajax'][0] : 1;

		for ( $i=1; $i<=$max_sum; $i++ ) :

			${'i'.$i} = $custom['i'.$i][0];

		endfor;

	
	// for product spec
	$max_sum_spec = $custom['specsum_ajax'][0];

		for( $i=1; $i<=$max_sum_spec; $i++ ) :
	
			${'s-text'.$i} = $custom['s-text'.$i][0];
			${'s-value'.$i} = $custom['s-value'.$i][0];
	
		endfor;

	?>


	
		<input type="hidden" name="tab" id="tab" value="<?php echo $tab; ?>" />
		<input type="hidden" name="postid_ajax" value="<?php echo $post->ID; ?>" />
		
		<!-- For product thumbnail -->
		<input type="hidden" id="postsum_ajax" name="postsum_ajax" value="<?php echo $max_sum; ?>" />
		<input type="hidden" id="postsum_ajax_before" name="postsum_ajax_before" value="<?php echo $max_sum; ?>" />
		
		<!-- For product spec -->
		<input type="hidden" id="specsum_ajax" name="specsum_ajax" value="<?php echo $max_sum_spec ?>" />
		<input type="hidden" id="specsum_ajax_before" name="specsum_ajax_before" value="<?php echo $max_sum_spec; ?>" />



		
		<div class="categorydiv" id="product-metas">



			<ul class="category-tabs" id="product-tabs" style="display:none;">

				<li id="tab-1"><a title="1" href="#"><?php _e('Common','pandathemes') ?></a></li>

				<li id="tab-2"><a title="2" href="#"><?php _e('Images','pandathemes') ?></a></li>

				<li id="tab-3"><a title="3" href="#"><?php _e('Specs','pandathemes') ?></a></li>

				<li id="tab-4"><a title="4" href="#"><?php _e('Purchase','pandathemes') ?></a></li>

				<li id="tab-5"><a title="5" href="#"><?php _e('Custom tab','pandathemes') ?></a></li>

			</ul>
		


			<!-- start COMMON TAB -->
				<div style="display:none;" class="tabs-panel" id="product-tab-1">
	
			
					<fieldset class="panda-admin-fieldset"><legend><?php _e('Short description','pandathemes') ?></legend>
	
						<div class="pa-col-l-fl"><?php _e('Enter here','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<textarea cols="70" rows="2" name="desc" id="desc" class="input autosize" /><?php echo $desc ?></textarea>
							<small><?php _e('e.g. tagline or major feature.','pandathemes') ?></small>
						</div>
	
					</fieldset>
	
			
					<fieldset class="panda-admin-fieldset"><legend><?php _e('Live demo','pandathemes') ?></legend>

						<div class="pa-col-l-fl"><?php _e('Button','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<input type="text" name="demo_button" size="10" value="<?php if ($demo_button) { echo $demo_button; } else { _e('Live demo','pandathemes') ;} ?>">
							<input type="text" name="demo_button_tagline" size="25" value="<?php if ($demo_button_tagline) { echo $demo_button_tagline; } else { the_title() ;} ?>">
							<small><?php _e('e.g. Live demo, Live preview etc.','pandathemes') ?></small>
						</div>

						<div class="clear h20"><!-- --></div>

						<div class="pa-col-l-fl"><?php _e('URL','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<input type="text" name="demo_url" size="80" class="size100" value="<?php echo $demo_url ?>">
							<small><?php _e('Enter an URL to live-preview.','pandathemes') ?></small>
						</div>
	
					</fieldset>
	
			
					<fieldset class="panda-admin-fieldset"><legend><?php _e('Label','pandathemes') ?></legend>
	
						<div class="pa-col-l-fl"><?php _e('Enter here','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<input type="text" name="label" size="80" class="size100" value="<?php echo $label ?>">
							<small><?php _e("In case you'd like to add the label please enter that here e.g. Best choice, Sale, Ultimate edition etc. or leave this field an empty.",'pandathemes') ?></small>
						</div>
	
					</fieldset>
	
			
				</div>
			<!-- end COMMON TAB -->



			<!-- start IMAGES TAB -->
				<div style="display:none;" class="tabs-panel" id="product-tab-2">


					<ul class="categorychecklist">
						<li>


							<div id="" class="thumb-product clone-thumb-product not-sortable" style="display:none">
								<div>
									<div>
										<div class="thumb-panel">
											<a class="thickbox button-primary select-image clone-select" style="display:block" id="" href="media-upload.php?post_id=<?php echo the_ID() ?>&amp;type=image&amp;TB_iframe=1&amp;width=640&amp;height=469"><?php _e('Select image','pandathemes') ?></a>
											<a class="button remove-thumb clone-select" style="display:block;" href="#"><img src="<?php echo get_bloginfo('template_url')?>/images/icons/16/ico_cross.png"></a>
										</div>
										<img class="thumb-img" id="no-img" src="<?php echo get_bloginfo('template_url').'/admin/graphics/thumb-photo.png';?>">
										<input id="" class="none" type="text" name="" value="" />
									</div>
								</div>
							</div>


							<?php 	
								for ( $i=1; $i<=$max_sum; $i++ ) :
									$not_sortable = ($i==$max_sum) ? 'not-sortable' : ''; ?>

										<div id="i<?php echo $i; ?>" class="thumb-product <?php echo $not_sortable; ?>">
											<div>
												<div>
													<div class="thumb-panel">
														<?php
															if ( ${'i'.$i} && ${'i'.$i}!=='none') { $select = 'none'; $remove = 'block';} else { $select = 'block'; $remove = 'none'; }
															
															// remove "remove box" on last box
															$remove_thumb = ($i==$max_sum) ? 'style="display:none"' : 'style="display:block"';
															
															// display thumbnail
															$display_thumb = (${'i'.$i} && ${'i'.$i}!=='none') ? ${'i'.$i} : get_bloginfo('template_url').'/admin/graphics/thumb-photo.png';
														?>
														<a class="thickbox button-primary select-image" style="display:<?php echo $select ?>;" id="image-<?php echo $i; ?>" href="media-upload.php?post_id=<?php echo the_ID() ?>&amp;type=image&amp;TB_iframe=1&amp;width=640&amp;height=469"><?php _e('Select image','pandathemes') ?></a>
														<a class="button remove-thumb clone-select" style="display:<?php echo $remove ?>;;" href="#"><img src="<?php echo get_bloginfo('template_url')?>/images/icons/16/ico_cross.png"></a>
													</div>
													<img class="thumb-img" id="image-<?php echo $i; ?>-thumb" alt="<?php echo $display_thumb ?>">
													<input id="image-<?php echo $i; ?>-input" class="none" type="text" name="i<?php echo $i; ?>" value="<?php echo ${'i'.$i}; ?>" />
												</div>
											</div>
										</div>

									<?php
								endfor;
							?>

			
						</li>
					</ul>


				</div>
			<!-- end IMAGES TAB -->



			<!-- start SPECS TAB -->
				<div style="display:none;" class="tabs-panel" id="product-tab-3">


					<ul id="spec-sortable" class="categorychecklist">

						<li>
			
							<div class="spec-title none">
								<div style="width:5%; margin:0 5px 0 0;">&nbsp;</div>
								<div style="width:20%"><?php _e('Parameter','pandathemes') ?></div>
								<div><?php _e('Value','pandathemes') ?></div>
							</div>
			
							<div class="element-sortable element-sortable-clone not-sortable" style="display:none">
								<span class="moving-span">&nbsp;</span>
								<label><?php _e('Parameter','pandathemes') ?></label><input type="text" class="s-text" value="">
								<label><?php _e('Value','pandathemes') ?></label><input type="text" class="s-value" value="">
								<span class="delete-element">&nbsp;</span>
							</div>
			
							<?php 	
								for ( $i=1; $i<=$max_sum_spec; $i++ ) : 
									?>
										<div class="element-sortable">
											<span class="moving-span">&nbsp;</span>
											<label><?php _e('Parameter','pandathemes') ?></label><input type="text" class="s-text" name="s-text<?php echo $i ?>" value="<?php echo ${'s-text'.$i} ?>">
											<label><?php _e('Value','pandathemes') ?></label><input type="text" class="s-value" name="s-value<?php echo $i ?>" value="<?php echo ${'s-value'.$i} ?>">
											<span class="delete-element">&nbsp;</span>
										</div>
									<?php
								endfor;
							?>
						</li>

						<li>
							<br/>
							<div id="spec-alert" style="width:80%; text-align:center; background:#FFFFE0; border:1px solid #E6DB55; border-radius:3px; padding:0 0.6em; margin:0 auto;"><p><?php _e('Please, save this product as draft before adding the specifications. Do not forget to set the title before saving.','pandathemes') ?></p></div>
							<a class="add-element button none" id="add-specs-button" href="#"><?php _e('Add new','pandathemes') ?></a> &nbsp; 
							<a class="save-element button-primary none" id="save-specs-button" href="#"><?php _e('Update specs','pandathemes') ?></a>
						</li>

					</ul>


					<div class="h30"><!-- --></div>
				</div>
			<!-- end SPECS TAB -->



			<!-- start PURCHASE TAB -->
				<div style="display:none;" class="tabs-panel" id="product-tab-4">
			
					<fieldset class="panda-admin-fieldset"><legend><?php _e('Cost','pandathemes') ?></legend>
				
						<div class="pa-col-l-fl"><?php _e('Price','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<input type="text" name="price" size="3" value="<?php echo $price ?>">
							<small><?php _e('e.g. $19.95, 19 USD, FREE etc.','pandathemes') ?></small>
						</div>
			
						<div class="clear h20"><!-- --></div>
					
						<div class="pa-col-l-fl"><?php _e('Old price','pandathemes') ?></div>
						<div class="pa-col-r-fl"><input type="text" name="price_old" size="3" value="<?php echo $price_old ?>"></div>
					
					</fieldset>
			
					<fieldset class="panda-admin-fieldset"><legend><?php _e('Button','pandathemes') ?></legend>
			
						<div class="pa-col-l-fl"><?php _e('Select the type','pandathemes') ?></div>
						<div class="pa-col-r-fl">

							<div class="h5"><!-- --></div>
				
							<input type="radio" value="default" name="purchase" id="purchase_default"
							<?php if ($purchase=="default" || $purchase==""):?> checked="checked" <?php endif; ?> /> <label for="purchase_default"><?php _e('Default','pandathemes') ?></label>
			
							<input type="radio" value="custom" name="purchase" id="purchase_custom"
							<?php if ($purchase=="custom"):?> checked="checked" <?php endif; ?> /> <label for="purchase_custom"><?php _e('Custom','pandathemes') ?></label>
			
							<input type="hidden" name="button_current" id="button_current" size="8" value="<?php echo $button_current; ?>">
			
						</div>
			
						<div class="clear h30"><!-- --></div>
			
						<!-- start default_button -->
						<div id="default_button" style="display:none;">
			
							<div class="pa-col-l-fl"><?php _e('Button','pandathemes') ?></div>
							<div class="pa-col-r-fl">
								<input type="text" name="button" size="10" value="<?php if ($button) { echo $button ;} else { _e('Purchase','pandathemes') ;} ?>">
								<input type="text" name="button_tagline" size="25" value="<?php if ($button_tagline) { echo $button_tagline ;} else { the_title() ;} ?>">
								<small><?php _e('e.g. Purchase, Buy now, Checkout, Download etc.','pandathemes') ?></small>
							</div>
			
							<div class="clear h20"><!-- --></div>
			
							<div class="pa-col-l-fl"><?php _e('URL','pandathemes') ?></div>
							<div class="pa-col-r-fl">
								<input type="text" name="button_url" size="80" class="size100" value="<?php echo $button_url; ?>">
								<small><?php _e('e.g. http://gateway.tld/checkout/ -OR- path to archive file in case that for free.','pandathemes') ?></small>
							</div>
			
							<div class="clear h10"><!-- --></div>
			
							<div class="pa-col-l-fl"><!-- --></div>
							<div class="pa-col-r-fl">
								<input type="checkbox" name="direct_url" value="yes" <?php if ($direct_url == 'yes') { echo 'checked'; } ?> /> 
								<?php _e('Direct link','pandathemes') ?>
								<small><?php _e('By default, a target URL will be opened at new window using a redirect, however, you can use a direct link instead of redirect.','pandathemes') ?></small>
							</div>
			
							<div class="clear h20"><!-- --></div>

							<div class="pa-col-l-fl"><?php _e('Before button','pandathemes') ?></div>
							<div class="pa-col-r-fl">
								<textarea cols="70" rows="2" name="before_button" id="before_button" class="input autosize" /><?php echo $before_button ?></textarea>
								<small><?php _e('If you need to display an additional information above the button please put it on here.','pandathemes') ?></small>
							</div>
			
							<div class="clear h20"><!-- --></div>
			
							<div class="pa-col-l-fl"><?php _e('After button','pandathemes') ?></div>
							<div class="pa-col-r-fl">
								<textarea cols="70" rows="2" name="after_button" id="after_button" class="input autosize" /><?php echo $after_button ?></textarea>
								<small><?php _e('An additional information below the button please put on here.','pandathemes') ?></small>
							</div>
			
						</div>
						<!-- end default_button -->
						
						<!-- start custom_button -->
						<div id="custom_button" style="display:none;">
							<div class="pa-col-l-fl"><?php _e('Code','pandathemes') ?></div>
							<div class="pa-col-r-fl">
								<textarea cols="70" rows="2" name="custom_button" id="custom_button" class="input autosize" /><?php echo $custom_button ?></textarea>
								<small><?php _e('In case you would like to display a custom button (e.g. PayPal button) please put it on here.','pandathemes') ?></small>
							</div>
						</div>
						<!-- end custom_button -->
						
					</fieldset>


				</div>
			<!-- end PURCHASE TAB -->



			<!-- start CUSTOM TAB -->
				<div style="display:none;" class="tabs-panel" id="product-tab-5">
			
					<fieldset class="panda-admin-fieldset"><legend><?php _e('Custom tab','pandathemes') ?></legend>

						<div class="pa-col-l-fl"><?php _e('Tab title','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<input type="text" name="custom_tab_title" size="10" value="<?php echo $custom_tab_title; ?>">
							<small><?php _e('e.g. Video, Misc etc.','pandathemes') ?></small>
						</div>

						<div class="clear h20"><!-- --></div>

						<div class="pa-col-l-fl"><?php _e('Content','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<textarea cols="70" rows="2" name="custom_tab_content" id="custom_tab_content" class="input autosize" /><?php echo $custom_tab_content; ?></textarea>
							<small><?php _e('Paste content here.','pandathemes') ?></small>
						</div>
	
					</fieldset>

			
				</div>
			<!-- end CUSTOM TAB -->



		</div><!-- end #product-metas -->




		<script type='text/javascript'>
		
			var k = jQuery.noConflict();
			
			k('#postsum_ajax').val(k('.thumb-product').length);

			// PRODUCT META TABS
			k('#product-tabs a').click(function() {
				k('#product-tabs li').removeClass('tabs');
				k(this).parent().addClass('tabs');
			
				pcurrent = k(this).attr('title');
				k('#product-metas .tabs-panel').hide();
				k('#product-tab-'+pcurrent).show();
				k('#tab').val(pcurrent);
			
				return false;
			});

			// BUTTON TABS
			k('#purchase_default').click(function() {
				k('#default_button').show();
				k('#custom_button').hide();
				k('#button_current').val('default');
			});

			k('#purchase_custom').click(function() {
				k('#default_button').hide();
				k('#custom_button').show();
				k('#button_current').val('custom');
			});

			// UPLOAD IMAGE BUTTON + CURRENT TAB
			k(document).ready(function() {
				var id_c;
				// variable check of last image selected
				var last_image = false;
				
				// by press on .select-image
				k('.select-image').click(function() {
					id_c = k(this).attr('id');
					tb_show('', 'media-upload.php?type=image&amp;TB_iframe=true');
					k('#TB_title').next('#TB_title').hide();
					return false;
			
				});
				
				// by press on .remove-image
				k('.remove-image,.remove-thumb').click(function() {
					if(k(this).hasClass('remove-thumb')){
						k(this).parents('.thumb-product').hide('slow', function() {
								k(this).remove();
								update_attribute();
							});
					} else {
						k(this).prev().show();
						k(this)
							.parent().parent().find('.thumb-img').attr('src','<?php echo get_bloginfo('template_url').'/admin/graphics/thumb-photo.png' ?>')
							.parent().find('input').val('none');
						k(this).hide();
					}
					save_all('remove');
					return false;
				});
				
				// ajax loading progress
				function add_indicator() {
					k('#product-metas').parent().parent().find('h3 span').addClass('ajax-loader').animate({left: '+0px'}, 4000, function() { k(this).removeClass('ajax-loader'); });
				}
			
				function remove_indicator() {
					k('.ajax-loader').removeClass('ajax-loader');
				}
				
				// apply an image
				window.original_send_to_editor = window.send_to_editor;
				window.send_to_editor = function(html) {
					if(id_c){
						var id = id_c;
							id_c = undefined;
						var imgurl = jQuery('img',html).attr('src');
						var ext = imgurl.substr(imgurl.length - 3);
							k('#'+id+'-input').val(imgurl);
							tb_remove();
				
						k('#'+id+'-thumb').attr({
							src: function() {
								if (ext == 'jpg') { var path = imgurl.replace(/.jpg/g,'-150x150.jpg'); }
								if (ext == 'peg') { var path = imgurl.replace(/.jpeg/g,'-150x150.jpeg'); }
								if (ext == 'png') { var path = imgurl.replace(/.png/g,'-150x150.png'); }
								if (ext == 'gif') { var path = imgurl.replace(/.gif/g,'-150x150.gif'); }
								return path;
							}
						});

						k('.select-image:not(.clone-select)').hide();
						k('.remove-image:not(.clone-select)').show();
						
						if(k('.thumb-product').last().find('input').attr('id') == id+'-input'){
							last_image = true;
							k('.thumb-product').last().removeClass('not-sortable');
						}	
						save_all();
					} else{
						window.original_send_to_editor(html);
					}
				}

				// Display thumbs
				k('.thumb-img').each(function(){
					var imgurl = k(this).attr('alt');
					if (imgurl) {
						var ext = imgurl.substr(imgurl.length - 3);
							if (ext == 'jpg') { var path = imgurl.replace(/.jpg/g,'-150x150.jpg'); }
							if (ext == 'peg') { var path = imgurl.replace(/.jpeg/g,'-150x150.jpeg'); }
							if (ext == 'png') { var path = imgurl.replace(/.png/g,'-150x150.png'); }
							if (ext == 'gif') { var path = imgurl.replace(/.gif/g,'-150x150.gif'); }
						k(this).attr('src',path).removeAttr('alt');
					}
				})
				
				// Save all
				function save_all(type) {
					
					add_indicator();
				
					if(type != 'remove' && last_image) 
						k('#postsum_ajax').val(k('.thumb-product').length);
					else 
						k('#postsum_ajax').val(k('.thumb-product').length-1);
			
					var formValues = k("form#post").serialize();
					
					var data = {
						type: 'save',
						action: 'iajax_save',
						data: formValues
					}
					k.post(
						ajaxurl,
						data,
						function(message){
							if(message == 'save_success') { 
								
								if(type != 'remove' && last_image) {
								
									// update before sum value
									k('#postsum_ajax_before').val(k('.thumb-product').length);
									
									k('.remove-thumb').css('display','block');
									
									// clone, clean, setup attribute and append new thumb box
									var sum = k('.thumb-product').length;
									var twin = k('.clone-thumb-product').clone(true);
									twin.attr('id','i'+sum).removeClass('clone-thumb-product').css('display','block').find('.clone-select').removeClass('clone-select')
									.end().find('.select-image').attr('id','image-'+sum)
									.end().find('.remove-thumb').hide()
									.end().find('.thumb-img').attr('id','image-'+sum+'-thumb')
									.end().find('input.none').attr('id','image-'+sum+'-input').attr('name','i'+sum);
									twin.appendTo('#product-tab-2 .categorychecklist li:first');
									
									last_image = false;
									
								} else if(type =='remove') {
								
									// update before sum value, since box remove -> -1
									k('#postsum_ajax_before').val(k('.thumb-product').length-1);
									
								}
								k('#product-tab-2 .categorychecklist li:first').sortable('refresh');
								
								if(type != 'sortable')
								remove_indicator();
								
							}
						});
				}
				function update_attribute(){
					k('.thumb-product:not(.clone-thumb-product)').each(function(i){
						k(this).attr('id','i'+(i+1))
						.find('.select-image').attr('id','image-'+(i+1))
						.end().find('.thumb-img').attr('id','image-'+(i+1)+'-thumb')
						.end().find('input.none').attr('id','image-'+(i+1)+'-input').attr('name','i'+(i+1));
					});
					save_all('sortable');
				}
			
				// Set the current tab for product
				var tab = k('#tab').val();
					if (!(tab)) { tab = '1';}
					k('#tab-'+tab).addClass('tabs');
					k('#product-tabs, #product-tab-'+tab).show();
			
				// Set the current tab for button
				var but_tab = k('#button_current').val();
					if (but_tab=="default" || but_tab=="") { k('#default_button').show() ;}
					else { k('#custom_button').show() ;}
			
				// Display `Save specs` button
					k('#sample-permalink').each(function(){
						k('#save-specs-button, #add-specs-button, .spec-title').removeClass('none');
						k('#spec-alert').addClass('none');
					})
			
				k('#product-tab-2 .categorychecklist li:first').sortable({
					items: '.thumb-product:not(.not-sortable)',
					placeholder: "thumb-ui-state-highlight",
					update: update_attribute
				});
				k('#product-tab-2 .categorychecklist li:first').disableSelection();
				
				k.field_sortable = function(config){
					var defaults = {
						sortElement:'element-sortable'
					}
					var data = k.extend(defaults,config);
					
					var ele = k('#'+data.targetID);
					var rawClass = data.fieldsClass;
					var prefixClass = rawClass.split(',');
					
					var update_cs = function(save){
						for(i=1;i<=prefixClass.length;i++){
							k('.'+prefixClass[i-1],ele).each(function(ite){
								if(ite!=0){
									k(this).attr('name',prefixClass[i-1]+ite);
								}
							});
						}
						if(save) save_cs();
					}
					
					var update_max_cs = function(before){
						if(before) 
							k('#'+data.sumBeforeID).val(k('.'+data.sortElement+':not(.not-sortable)',ele).length);
						else
							k('#'+data.sumID).val(k('.'+data.sortElement+':not(.not-sortable)',ele).length);
					}
					
					var save_cs = function(){
						
						add_indicator()
						
						var formValues = k("form#post").serialize();
						var dataSubmit = {
							type: 'save',
							action: 'iajax_save_cs',
							data: formValues,
							prefix: rawClass,
							sumID: data.sumID, 
							sumBeforeID: data.sumBeforeID
						}
						
						k.post(
							ajaxurl,
							dataSubmit,
							function(message){
								if(message == 'save_success') {
									update_max_cs(true);
									remove_indicator()	
								}
							});
					
					}
					// add and remove function
					k('.add-element,.delete-element,.save-element',ele).click(function(e){
					
						e.preventDefault();
						var thisButton = k(this);
						if(thisButton.hasClass('add-element')){
						
							var twin = k('.element-sortable-clone',ele).clone(true);
							twin.removeClass('element-sortable-clone not-sortable').css('display','block');
							for(i=1;i<=prefixClass.length;i++){
								var num = k('.'+prefixClass[i-1],ele).length;
								twin.find('.'+prefixClass[i-1]).attr('name',prefixClass[i-1]+num);
							}
							k('.element-sortable',ele).parent().append(twin);
							update_max_cs();
							
						} else if(thisButton.hasClass('delete-element')) {
						
							k(this).parents('.element-sortable').hide('slow', function() {
								k(this).remove();
								update_max_cs();
								update_cs(true);
							});
							
						} else {
						
							save_cs();
			
						}
						
					});
					
					// sortable function
					k('.'+data.sortElement, ele).parent().sortable({
						items:			'.'+data.sortElement+':not(.not-sortable)',
						placeholder:	"ui-state-highlight",
						handle:			'.moving-span',
						update:			update_cs
					})
			
				}
				k.field_sortable({
					targetID:		'spec-sortable', 
					fieldsClass:	's-text,s-value', 
					sumID:			'specsum_ajax', 
					sumBeforeID:	'specsum_ajax_before'
				});
				// ANIMATE TEXT AREA BY FOCUS
				k('textarea.autosize').focus(function() {
					if ( ! k(this).hasClass('height-ready') ) {
						k(this).css({height: 70}).animate({height: 150}, 300, function(){ k(this).addClass('height-ready'); });
					}
				});
			});
			
		</script>



<?php
} // END META OPTIONS FUNCTION



// *********************************************
//
// 	C U S T O M   S E T   O F   C O L U M N S
//
// *********************************************



function prod_edit_columns($columns){
	$columns = array(
		'cb'			=> '<input type="checkbox" />',
		'title'			=> __('Product Title','pandathemes'),
		'thumb'			=> __('Thumbnail','pandathemes'),
		'catalog'		=> __('Catalog','pandathemes'),
		'price'			=> __('Price','pandathemes')
	);
	return $columns;
}

add_filter("manage_edit-product_columns", "prod_edit_columns");



function prod_custom_columns($column){

	global $post;

	switch ($column) {

		case "catalog" :
			echo get_the_term_list($post->ID, 'catalog', '', ', ','');
			break;

		case "price" :
			$custom = get_post_custom();
			echo $custom['price'][0];
			break;

		case "thumb" :
			echo get_the_post_thumbnail($thumbnail->ID, 'thumb-150');
			break;

	}

}

add_action("manage_posts_custom_column", "prod_custom_columns");



require_once (TEMPLATEPATH.'/admin/includes/custom_post_type/ajax_handler.php');



?>