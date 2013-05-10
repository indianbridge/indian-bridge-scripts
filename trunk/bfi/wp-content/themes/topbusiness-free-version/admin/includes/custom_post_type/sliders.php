<?php

// by PandaThemes.com



// *********************************************
//
// 	S L I D E R
//
// *********************************************



function slider_register() {


	// 	P O S T   T Y P E

	$args = array(

		$labels = array(
				'name'					=> __('Sliders','pandathemes'),
				'singular_name'			=> __('Slider','pandathemes'),
				'add_new'				=> __('Add New','pandathemes'),
				'add_new_item'			=> __('Add New Slider','pandathemes'),
				'edit_item'				=> __('Edit Slider','pandathemes'),
				'new_item'				=> __('New Slider','pandathemes'),
				'view_item'				=> __('View Slider','pandathemes'),
				'search_items'			=> __('Search Slider','pandathemes'),
				'not_found'				=> __('No sliders found','pandathemes'),
				'not_found_in_trash'	=> __('No sliders found in Trash','pandathemes'), 
				'parent_item_colon'		=> ''
			),
			'labels'			=> $labels,
			'description'		=>	__('A group of posts which can be used as sliders.','pandathemes'),
			'public'			=> true,
			'show_ui'			=> true,
			'capability_type'	=> 'page',
			'menu_position'		=> 8,
			'hierarchical'		=> false,
			'rewrite'			=> array('slug' => 'sliders'),
			'query_var'			=> true,
			'has_archive'		=> true,
			'supports'			=> array('title')
		);

	register_post_type( 'slider' , $args );

}

add_action('init', 'slider_register');



// *********************************************
//
// 	R E G I S T E R   M E T A B O X E S
//
// *********************************************



function slider_admin_init(){

	add_meta_box('sliderInfo-meta', 'Slider Options', 'slider_meta_options', 'slider', 'normal', 'low');

}

add_action('admin_init', 'slider_admin_init');



// *********************************************
//
// 	S A V E   M E T A B O X E S
//
// *********************************************



function save_slider(){

	global $post;

	$custom_meta_fields = 
		array(
			'transition',
			'delay'
		);

	foreach( $custom_meta_fields as $custom_meta_field ):

		update_post_meta($post->ID, $custom_meta_field, $_POST[$custom_meta_field]);

	endforeach;

}

add_action('save_post', 'save_slider');



// *********************************************
//
// 	S T A R T   M E T A   O P T I O N S
//
// *********************************************



function slider_meta_options(){

	global $post;

	$custom = get_post_custom($post->ID);

		$transition = $custom['transition'][0];
		$delay = $custom['delay'][0];

	
	// for slider
	$max_sum_spec = $custom['specsum_ajax'][0];

		for( $i=1; $i<=$max_sum_spec; $i++ ) :
	
			${'s-title'.$i} = $custom['s-title'.$i][0];
			${'s-img'.$i} = $custom['s-img'.$i][0];
			${'s-height'.$i} = $custom['s-height'.$i][0];
			${'s-url'.$i} = $custom['s-url'.$i][0];
			${'s-target'.$i} = $custom['s-target'.$i][0];
	
		endfor;

	?>


	
		<input type="hidden" name="postid_ajax" value="<?php echo $post->ID; ?>" />
		
		<!-- For slider -->
		<input type="hidden" id="specsum_ajax" name="specsum_ajax" value="<?php echo $max_sum_spec ?>" />
		<input type="hidden" id="specsum_ajax_before" name="specsum_ajax_before" value="<?php echo $max_sum_spec; ?>" />



		
		<div class="categorydiv" id="slider-metas">



			<ul class="category-tabs" id="options-tabs" style="display:none;">

				<li id="tab-1" class="tabs"><a title="1" href="#"><?php _e('Slides','pandathemes') ?></a></li>

				<li id="tab-2"><a title="2" href="#"><?php _e('Options','pandathemes') ?></a></li>

			</ul>
		


			<!-- start SLIDES TAB -->
				<div style="display:none;" class="tabs-panel" id="option-tab-1">


					<ul id="slide-sortable" class="categorychecklist">

						<li>
			
							<div class="element-sortable element-sortable-clone not-sortable" style="display:none">
								<table class="w100p"><tr>

									<td class="options-toggle">
									
										<div>
											<span class="slide-title"><?php _e('no image','pandathemes') ?></span>
											<span class="moving-span">&nbsp;</span>
										</div>


										<div style="display:none;">
										
											<table class="options-table">

												<th colspan="2">
													<?php _e('no image','pandathemes') ?>
												</th>

												<tr>
													<td>
														<label><?php _e('Title','pandathemes') ?></label>
													</td>
													<td>
														<input type="text" class="s-title">
													</td>
												</tr>

												<tr>
													<td>&nbsp;
														
													</td>
													<td>
														<img class="thumb-img" src="<?php echo get_bloginfo('template_url').'/admin/graphics/thumb-photo.png' ?>" alt="" width="150" height="150">
													</td>
												</tr>

												<tr>
													<td>
														<label><?php _e('Image','pandathemes') ?></label>
													</td>
													<td>
														<input type="hidden" class="s-img">
														<a class="thickbox button select-image clone-select" href="media-upload.php?post_id=<?php echo the_ID() ?>&amp;type=image&amp;TB_iframe=1&amp;width=640&amp;height=469"><?php _e('Browse image','pandathemes') ?></a>
														<input type="hidden" class="s-height">
													</td>
												</tr>

												<tr>
													<td>
														<label><?php _e('Target URL','pandathemes') ?></label>
													</td>
													<td>
														<input type="text" class="s-url w97p">
													</td>
												</tr>

												<tr>
													<td>&nbsp;
														
													</td>
													<td>
														<input type="checkbox" class="s-target" value="blank" /> <small><?php _e('Open at new window','pandathemes') ?></small>
													</td>
												</tr>

											</table>

										</div>

									</td>

									<td class="table-control">

										<span class="ico-setting opt-toggle">&nbsp;</span>

									</td>

									<td class="table-control" style="height:40px">

										<span class="delete-element">&nbsp;</span>

									</td>

								</tr></table>
							</div>
			
							<?php 	
								for ( $i=1; $i<=$max_sum_spec; $i++ ) : 
									?>
										<div class="element-sortable" title="<?php echo $i; ?>">
											<table class="w100p"><tr>

												<td class="options-toggle">

													<?php
														$path_parts = pathinfo(${'s-img'.$i});
														$path = $path_parts['basename'];
														$imgname = ($path) ? $path : __('no image','pandathemes');
													?>
												
													<div>
														<span class="slide-title"><?php echo $imgname; ?></span>
														<span class="moving-span">&nbsp;</span>
													</div>


													<div style="display:none;">
													
														<table class="options-table">

															<th colspan="2">
																<?php echo $imgname; ?>
															</th>

															<tr>
																<td>
																	<label><?php _e('Title','pandathemes') ?></label>
																</td>
																<td>
																	<input type="text" class="s-title" name="s-title<?php echo $i ?>" value="<?php echo ${'s-title'.$i} ?>">
																</td>
															</tr>

															<tr>
																<td>&nbsp;
																	
																</td>
																<td>
																	<img class="thumb-img" id="thumb-img<?php echo $i; ?>" src="<?php echo get_bloginfo('template_url').'/admin/graphics/thumb-photo.png' ?>" alt="<?php echo ${'s-img'.$i} ?>" width="150" height="150">
																</td>
															</tr>

															<tr>
																<td>
																	<label><?php _e('Image','pandathemes') ?></label>
																</td>
																<td>
																	<input type="hidden" id="s-img<?php echo $i ?>" class="s-img" name="s-img<?php echo $i ?>" value="<?php echo ${'s-img'.$i} ?>">
																	<a class="thickbox button select-image clone-select" id="<?php echo $i ?>" title="<?php echo $i; ?>" href="media-upload.php?post_id=<?php echo the_ID() ?>&amp;type=image&amp;TB_iframe=1&amp;width=640&amp;height=469"><?php _e('Browse image','pandathemes') ?></a>
																	<input type="hidden" id="s-height<?php echo $i ?>" class="s-height" name="s-height<?php echo $i ?>" value="<?php echo ${'s-height'.$i} ?>">
																</td>
															</tr>

															<tr>
																<td>
																	<label><?php _e('Target URL','pandathemes') ?></label>
																</td>
																<td>
																	<input type="text" class="s-url w97p" name="s-url<?php echo $i ?>" value="<?php echo ${'s-url'.$i} ?>">
																</td>
															</tr>

															<tr>
																<td>&nbsp;
																	
																</td>
																<td>
																	<input type="checkbox" name="s-target<?php echo $i ?>" value="blank" <?php if (${'s-target'.$i}=='blank') { echo 'checked' ;} ?> /> <small><?php _e('Open at new window','pandathemes') ?></small>
																</td>
															</tr>

														</table>

													</div>

												</td>

												<td class="table-control">

													<span class="ico-setting opt-toggle">&nbsp;</span>

												</td>

												<td class="table-control" style="height:40px">

													<span class="delete-element">&nbsp;</span>

												</td>

											</tr></table>
										</div>
									<?php
								endfor;
							?>
						</li>

						<li>
							<br/>
							<div id="spec-alert" style="width:80%; text-align:center; background:#FFFFE0; border:1px solid #E6DB55; border-radius:3px; padding:0 0.6em; margin:0 auto;"><p><?php _e('Please, save this slider as draft before adding the slides. Do not forget to set the title before saving.','pandathemes') ?></p></div>
							<a class="add-element button none" id="add-specs-button" href="#"><?php _e('Add new','pandathemes') ?></a> &nbsp; 
							<a class="save-element button-primary none" id="save-specs-button" href="#"><?php _e('Update slides','pandathemes') ?></a>
						</li>

					</ul>


					<div class="h30"><!-- --></div>
				</div>
			<!-- end SLIDES TAB -->



			<!-- start OPTIONS TAB -->
				<div style="display:none;" class="tabs-panel" id="option-tab-2">
	
			
					<fieldset class="panda-admin-fieldset"><legend><?php _e('Options','pandathemes') ?></legend>
	
						<div class="pa-col-l-fl"><?php _e('Transition','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<select name="transition">
								<?php

									$arr = array(
										'ptd'				=> __("Default",'pandathemes'),
										'ptf'				=> __("Fade",'pandathemes'),
										'ptfv'				=> __("Fade vertical",'pandathemes'),
										'ptv'				=> __("Slide vertical",'pandathemes'),
										'pth'				=> __("Slide horizontal",'pandathemes'),
										'pto'				=> __("Overlay vertical",'pandathemes')
									);

									foreach ( $arr as $key => $value ) :

										$selected = ( $transition == $key ) ? ' selected' : '';

										echo '<option value="'.$key.'"'.$selected.'>'.$value.' &nbsp;</option>';
						
									endforeach;

								?>
							</select>
							<small><?php _e('Select a kind of transition.','pandathemes') ?></small>
						</div>

						<div class="clear h20"><!-- --></div>

						<div class="pa-col-l-fl"><?php _e('Delay','pandathemes') ?></div>
						<div class="pa-col-r-fl">
							<select name="delay">
								<?php
									$arr = array('3','4','5','6','7','10','15','20','60','9999');
									foreach ($arr as $value) {
										$out = '<option value="'.$value.'"';
										if ( $delay == $value ) : $out .= ' selected'; endif;
										$out .= '>'.$value.' &nbsp;</option>';
										echo $out;
									};
								?>
							</select> <?php _e('seconds','pandathemes') ?>
							<small><?php _e('Select a time delay between transitions.','pandathemes') ?></small>
						</div>

					</fieldset>

					<div class="h15"><!-- --></div>

					<a class="button-primary" id="save-options-button" href="#"><?php _e('Update options','pandathemes') ?></a>


				</div>
			<!-- end OPTIONS TAB -->




		</div><!-- end #slider-metas -->



		<div id="slider-alarm">

			<p><?php _e('Slider Options do not available on sidebar. Please, move the Slider Options bar on the central section.','pandathemes') ?></p>

		</div>



		<script type='text/javascript'>
		
			var k = jQuery.noConflict();


			
			// PRODUCT META TABS
			k('#options-tabs a').click(function() {

				k('#options-tabs li').removeClass('tabs');
				k(this).parent().addClass('tabs');
			
				pcurrent = k(this).attr('title');
				k('#slider-metas .tabs-panel').hide();
				k('#option-tab-'+pcurrent).show();
			
				return false;

			});


			// Options toggle
			k('.opt-toggle').toggle(function(){
				
				var toggle = k(this).parent().parent().find('.options-toggle');
				toggle.children().eq(0).hide();
				toggle.children().eq(1).show();
				
			}, function() {

				var toggle = k(this).parent().parent().find('.options-toggle');
				toggle.children().eq(0).show();
				toggle.children().eq(1).hide();
			
			});


			// UPLOAD IMAGE BUTTON + CURRENT TAB
			k(document).ready(function() {

				// Hide Publish button for draft post
				var draft = k('input#auto_draft').val();

					if ( draft == 1 ) { k('#publishing-action').remove(); }

				// variable check of last image selected
				var last_image = false;
				
				// by press on .select-image
				k('.select-image').click(function() {
					id = k(this).attr('id');
					tb_show('', 'media-upload.php?type=image&amp;TB_iframe=true');
					k('#TB_title').next('#TB_title').hide();
					return false;
			
				});
				
				// ajax loading progress
				function add_indicator() {
					k('#slider-metas').parent().parent().find('h3 span').addClass('ajax-loader').animate({left: '+0px'}, 4000, function() { k(this).removeClass('ajax-loader'); });
				}
			
				function remove_indicator() {
					k('.ajax-loader').removeClass('ajax-loader');
				}
			
				// Activation
					k('#options-tabs, #option-tab-1').show();
			
				// Display `Save slides` button
					k('#sample-permalink').each(function(){
						k('#save-specs-button, #add-specs-button').removeClass('none');
						k('#spec-alert').addClass('none');
					})


				// APPLY AN IMAGE
				window.send_to_editor = function(html) {

					var imgurl = jQuery('img',html).attr('src');
					var ext = imgurl.substr(imgurl.length - 3);
					
					var imgH = jQuery('img',html).attr('height');
				
						// Insert value to input field
						k('#s-img'+id).val(imgurl);
						k('#s-height'+id).val(imgH);
				
						// Close thickbox
						tb_remove();


					// Replace the title (filename as title)
					var imgname = imgurl.substring(imgurl.lastIndexOf('/')+1);
					k('#s-img'+id).parents(':eq(2)').find('th').html(imgname);
					k('#s-img'+id).parents(':eq(5)').find('.slide-title').html(imgname);

					// Replace thumb
					k('#thumb-img'+id).attr({
						src: function() {
							if (ext == 'jpg') { var path = imgurl.replace(/.jpg/g,'-150x150.jpg'); }
							if (ext == 'peg') { var path = imgurl.replace(/.jpeg/g,'-150x150.jpeg'); }
							if (ext == 'png') { var path = imgurl.replace(/.png/g,'-150x150.png'); }
							if (ext == 'gif') { var path = imgurl.replace(/.gif/g,'-150x150.gif'); }
							return path;
						}
					});

					save_cs();
				}


				// DISPLAY THUMBS
				k('.thumb-img').each(function(){
					var imgurl = k(this).attr('alt');
					if (imgurl) {
						var ext = imgurl.substr(imgurl.length - 3);
							if (ext == 'jpg') { var path = imgurl.replace(/.jpg/g,'-150x150.jpg'); }
							if (ext == 'peg') { var path = imgurl.replace(/.jpeg/g,'-150x150.jpeg'); }
							if (ext == 'png') { var path = imgurl.replace(/.png/g,'-150x150.png'); }
							if (ext == 'gif') { var path = imgurl.replace(/.gif/g,'-150x150.gif'); }
						k(this).attr('src',path);
					}
				})

				// SORTABLE			
				k.field_sortable = function(config){
					var defaults = {
						sortElement:'element-sortable'
					}
					var data = k.extend(defaults,config);
					
					var ele = k('#'+data.targetID);
					var rawClass = data.fieldsClass;
					var prefixClass = rawClass.split(',');
					
					var update_cs = function(save){
						for( i=1; i<=prefixClass.length; i++ ){
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

					// SAVE SLIDES
					var save_cs = function(){
						
						add_indicator();
						
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

							for ( i=1; i<=prefixClass.length; i++ ){
								var num = k('.'+prefixClass[i-1],ele).length;
								twin.find('.'+prefixClass[i-1]).attr('name',prefixClass[i-1]+num).attr('id',prefixClass[i-1]+num);

								if (prefixClass[i-1]=='select-image') { twin.find('.'+prefixClass[i-1]).attr('id',num); }

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
					targetID:		'slider-metas', 
					fieldsClass:	's-title,s-img,s-height,s-url,s-target,select-image,thumb-img', 
					sumID:			'specsum_ajax', 
					sumBeforeID:	'specsum_ajax_before'
				});

				// SAVE OPTIONS
				k('#save-options-button').click(function(){

						add_indicator();
						
						k.post("post.php", k("form#post").serialize());

					return false;

				});


			});
			
		</script>



		<style type="text/css">

			#edit-slug-box,
			#preview-action {
				display:none !important;
			}

		</style>



<?php
} // END META OPTIONS FUNCTION



require_once (TEMPLATEPATH.'/admin/includes/custom_post_type/ajax_handler.php');



?>