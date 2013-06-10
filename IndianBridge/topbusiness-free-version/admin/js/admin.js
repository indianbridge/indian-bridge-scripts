var k = jQuery.noConflict();

jQuery(document).ready(function(k) {

	k('.update-settings').css({'display':'block'});
	
	k('body').addClass('panda-admin-page');

/*------------------------------------------------------
				A D M I N  T A B S
------------------------------------------------------*/
	k('ul.tab-navigation a').click(function(){
		var title = k(this).attr('title');
		// 
		k('ul.tab-navigation a').removeClass('selected');
		k(this).addClass('selected');
		// Send value to input field
		k('#input-tab').val(title);
		// Display tab
		k('#tabbed-content > div').hide();
		k('#tabbed-content').find('#'+title).show();
		// End
		return false;
	});

	var current = k('#input-tab').val();
		// Display tab
		k('#'+current).show();
		// Select current menu item
		k("a[title='"+current+"']").addClass('selected');

	// Default activation
	if ( ! k('#input-tab').val() ) { k('#general').show(); k("a[title='general']").addClass('selected'); }

/*------------------------------------------------------
				C O M M O N
------------------------------------------------------*/

	var id = [
		"#before_catalog",
		"#after_catalog",
		"#before_product",
		"#after_product",
		"#after_purchase",
		"#notice",
		"#hcustom",
		"#lifestream",
		"#related_products",
		"#before_post",
		"#after_post",
		"#after_title",
		"#post_feat_image",
		"#square_thumbs",
		"#post_comments",
		"#post_metas",
		"#products_acvitation"
	];
	for (var i in id) {
		check = k(id[i]).attr('checked'); if (check) { k(id[i]+'_data').addClass('block'); };
	};

/*------------------------------------------------------
				L O G O
------------------------------------------------------*/
	k('#logo_image').click(function() { k('#logo_img, #logo_i').show(); k('#logo_t').hide(); });
	k('#logo_text').click(function() { k('#logo_img, #logo_i').hide(); k('#logo_t').show(); });
	check = k('#logo_image').attr('checked'); if (check) { k('#logo_img, #logo_i').css({'display':'block'}); }
	check = k('#logo_text').attr('checked'); if (check) { k('#logo_t').css({'display':'block'}); }

// IMAGE SELECTOR MULTIPLYABLE
	// by press on .select-image
	k('.select-image').click(function() {
		id = k(this).attr('id');
		tb_show('', 'media-upload.php?type=image&amp;TB_iframe=true');
		return false;
	});
	// apply an image
	window.send_to_editor = function(html) {
		imgurl = jQuery('img',html).attr('src');
		k('#'+id+'-input').val(imgurl);
		k('img.'+id+'-preview').attr('src', imgurl);
		tb_remove();
	}

/*------------------------------------------------------
				T E M P L A T E S
------------------------------------------------------*/
	k('.tmpl_radio input').click(function() {
										k(this).parent().parent().find('.tmpl_selected').removeClass('tmpl_selected');
										k(this).parent().addClass('tmpl_selected');
									});
	var id = [
		"#default_template",
		"#t1_template",
		"#t2_template",
		"#t3_template",
		"#style_1s",
		"#style_2s",
		"#style_3s",
		"#style_4s",
		"#style_5s",
		"#style_6s",
		"#style_7s",
		"#style_8s",
		"#style_9s",
		"#style_10s",
		"#footer_sidebars_1s",
		"#footer_sidebars_2s",
		"#footer_sidebars_3s",
		"#footer_sidebars_4s",
		"#footer_sidebars_5s",
		"#footer_sidebars_6s",
		"#footer_sidebars_7s",
		"#p1_template",
		"#p2_template"
	];
	for (var i in id) {
		check = k(id[i]).attr('checked'); if (check) { k(id[i]).parent().addClass('tmpl_selected'); };
	};


/*------------------------------------------------------
				P O S T S
------------------------------------------------------*/


/*------------------------------------------------------
				S T Y L E S
------------------------------------------------------*/

/*	RESET STYLE SETTINGS
*/
	k('.reset-style-panda-yes').click(function() {
		// reset controls
		k('#st_footer, #st_menu, #st_layout').find('input[type="text"]').val('');
		k('#general').find('input[type="radio"]').attr('checked', false).parent().removeClass('tmpl_selected');
		k('#general').find('input:radio[value="1"]').attr('checked', true).parent().addClass('tmpl_selected');
		k('.color-thumb').css({'background-color':'#EEEEEE'});
		k('#st_layout').find('select').find('option').removeAttr('selected');
		// reset approx menu
		k('#demo-menu .demo').css({'background-color':'#191919','color':'#CCC'});
		k('#demo-menu .demo-current').css({'background-color':'#555','color':'#FFF'});
		k('#demo-menu .demo-hover').css({'background-color':'#65a4c1','color':'#FFF'});
		// reset approx footer
		k('#demo-footer .demo').css({'background-color':'#191919','color':'#CCC'});
		k('#demo-footer .demo-title').css({'color':'#FFF'});
		k('#demo-footer .demo-link').css({'color':'#5accff'});
		k('#confirmation-reset').hide();
		k('#sucess-reset-style').show();
	});
	k('.reset-style-panda').click(function() { k('#confirmation-reset').show(); k('#sucess-reset-style').hide(); });
	k('.reset-style-panda-no').click(function() { k('#confirmation-reset').hide(); });


/*------------------------------------------------------
				T Y P O G R A P H Y
------------------------------------------------------*/
	k('#default_font_group').click(function() { k('.default_font_group').show(); k('.cufon_font_group, .google_font_group, .pa-window').hide();  });
	k('#cufon_font_group').click(function() { k('.cufon_font_group').show(); k('.default_font_group, .google_font_group, .pa-window').hide();  });
	k('#google_font_group').click(function() { k('.google_font_group').show(); k('.cufon_font_group, .default_font_group, .pa-window').hide();  });
	var id = ["default_font_group", "cufon_font_group", "google_font_group"];
	for (var i in id) {
		check = k('#'+id[i]).attr('checked');
		if (check) { k('.'+id[i]).css({'display':'block'});};
	};

// OPEN & CLOSE WINDOW
	k('.input-panda, .browse-button').click(function() {
		var parent = k(this).parent();
		k('.pa-window').hide();
		parent.find('.pa-window').show();
	});
	k('.cancel-button-panda').click(function() { k('.pa-window').hide(); });

// CURRENT ITEM AT LIST
	k('.pa-list ol li').click(function() {
		k(this).parent().find('.pa-list-current').removeClass('pa-list-current');
		k(this).addClass('pa-list-current');
	;});

// APPLY
	k('.apply-button-panda').click(function() {
		var parent = k(this).parent().parent().parent().parent();
			parent.find('.input-panda').val(preselect);
			parent.find('.input-panda-data').val(predata);
			parent.find('.input-panda-current').val(current);
			parent.find('.pa-window').hide();
	});

// DEFAULT ITEM
	k('.preview-panda').each(function(){
		current = k(this).parent().find('.input-panda-current').attr('value');
		current = current++;
		k('.pa-list ol li',this).eq(current).addClass('pa-list-current');
		k('.pa-preview:not(#pa-pattern)',this).css('background-position','left -'+current*280+'px');
		k('#pa-slider',this).css('top','-'+current*280+'px');
	});

// ON CLICK
	// as background
	k('.pa-list ol').each(function(){
		var top = k('li',this).click(function() {
			current = top.index(this);
			k(this).parent().parent().parent().find('.pa-preview').css('background-position','left -'+current*280+'px');
			preselect = k(this).attr('alt');
			predata = k(this).html();
		});
	});
	// as slides
	k('.pa-list ol').each(function(){
		var top = k('li',this).click(function() {
			current = top.index(this);
			k(this).parent().parent().parent().find('#pa-slider').css('top','-'+current*280+'px');
			preselect = k(this).attr('alt');
		});
	});


/*------------------------------------------------------
				M I S C
------------------------------------------------------*/


/*------------------------------------------------------
				C O L O R  P I C K E R S
------------------------------------------------------*/

/*	BACKGROUND
*/
	k("input#background_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#background_color').attr('value', hex);
			k('#background_color_thumb, #pa-slider').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	H1 TITLE
*/
	k("input#h1_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#h1_color').attr('value', hex);
			k('#h1_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	H2 TITLE
*/
	k("input#h2_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#h2_color').attr('value', hex);
			k('#h2_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	H3 TITLE
*/
	k("input#h3_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#h3_color').attr('value', hex);
			k('#h3_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	H4 TITLE
*/
	k("input#h4_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#h4_color').attr('value', hex);
			k('#h4_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	H5 TITLE
*/
	k("input#h5_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#h5_color').attr('value', hex);
			k('#h5_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	H6 TITLE
*/
	k("input#h6_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#h6_color').attr('value', hex);
			k('#h6_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	LINKS COLOR
*/
	k("input#links_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#links_color').attr('value', hex);
			k('#links_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	LINKS COLOR:HOVER
*/
	k("input#links_hover_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#links_hover_color').attr('value', hex);
			k('#links_hover_color_thumb').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });




/*	MENU LINKS COLOR
*/
	k("input#menu_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#menu_color').attr('value', hex);
			k('#menu_color_thumb').css('background-color', '#'+hex);
			k('#demo-menu .demo').css('color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	MENU BACKGROUND COLOR
*/
	k("input#menu_bg_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#menu_bg_color').attr('value', hex);
			k('#menu_bg_color_thumb, #demo-menu .demo').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	MENU LINKS COLOR:HOVER
*/
	k("input#menu_hover_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#menu_hover_color').attr('value', hex);
			k('#menu_hover_color_thumb').css('background-color', '#'+hex);
			k('#demo-menu .demo-hover').css('color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	MENU BACKGROUND COLOR:HOVER
*/
	k("input#menu_hover_bg_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#menu_hover_bg_color').attr('value', hex);
			k('#menu_hover_bg_color_thumb, #demo-menu .demo-hover').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	MENU LINKS COLOR:CURRENT
*/
	k("input#menu_selected_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#menu_selected_color').attr('value', hex);
			k('#menu_selected_color_thumb').css('background-color', '#'+hex);
			k('#demo-menu .demo-current').css('color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	MENU BACKGROUND COLOR:CURRENT
*/
	k("input#menu_selected_bg_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#menu_selected_bg_color').attr('value', hex);
			k('#menu_selected_bg_color_thumb, #demo-menu .demo-current').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });




/*	FOOTER BACKGROUND
*/
	k("input#footer_bg").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#footer_bg').attr('value', hex);
			k('#footer_bg_thumb, #demo-footer .demo').css('background-color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	FOOTER TITLES
*/
	k("input#footer_titles_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#footer_titles_color').attr('value', hex);
			k('#footer_titles_color_thumb').css('background-color', '#'+hex);
			k('#demo-footer .demo-title').css('color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	FOOTER TEXT COLOR
*/
	k("input#footer_text_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#footer_text_color').attr('value', hex);
			k('#footer_text_color_thumb').css('background-color', '#'+hex);
			k('#demo-footer .demo').css('color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

/*	FOOTER LINKS COLOR
*/
	k("input#footer_links_color").ColorPicker({ onSubmit: function(hsb, hex, rgb, el) { k(el).val(hex); k(el).ColorPickerHide(); },
		onBeforeShow: function() {	k(this).ColorPickerSetColor(this.value); },
		onChange: function(hsb, hex, rgb) {
			k('input#footer_links_color').attr('value', hex);
			k('#footer_links_color_thumb').css('background-color', '#'+hex);
			k('#demo-footer .demo-link').css('color', '#'+hex); }
	}).bind('keyup', function(){ k(this).ColorPickerSetColor(this.value); });

});

/*------------------------------------------------------
				F U N C T I O N S
------------------------------------------------------*/

jQuery.noConflict();
(function(m) { 
	m(function() {

		// CHECKBOX TOGGLE
		m('.checkbox-toggle').click(function(){
			var id = '#' + m(this).attr('id');
			if (m(id+'_data').hasClass('block')) {m(id+'_data').removeClass('block')}
			else {m(id+'_data').addClass('block')};
		});

		// TABS
		m('.kul li').each(function(){
		
			m(this).click(function(){
				var li = m(this);
					li.parent().children('.kcurrent').removeClass('kcurrent');
					li.addClass('kcurrent');
				var liIndex = li.prevAll().length;
				var ktabs = li.parent().next('.ktabs');
					ktabs.children('.block').removeClass('block');
					ktabs.children().eq(liIndex).addClass('block');
			})

		});

	});
})(jQuery);

