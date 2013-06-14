<html xmlns="http://www.w3.org/1999/xhtml">

<?php

	define('DOING_AJAX', true);

	require_once('../../../../../../../../wp-load.php');

	$sc = array (

		// NAVIGATION
		'Navigation' => array(

			'button' => array(
				'alt'			=> __('B U T T O N  :  A buttons with any color and any size. Borders can be rounded by your choice.','pandathemes'),
				'attributes'	=> array(
					'url'			=> __('Target URL e.g. http://google.com','pandathemes'),
					'bg'			=> __('Background color e.g. 336699','pandathemes'),
					'bg_hover'		=> __('Background color by hover e.g. 000000','pandathemes'),
					'color'			=> __('Text color e.g. FFFFFF','pandathemes'),
					'color_hover'	=> __('Text color by hover e.g. CCCCCC','pandathemes'),
					'size'			=> __('Font size in pixels e.g. 16','pandathemes'),
					'radius'		=> __('Border radius in pixels e.g. 5','pandathemes'),
					'tooltip'		=> __('Tooltip e.g. Hello World!','pandathemes'),
					'target'		=> __('Target window e.g. blank','pandathemes')
				)
			),

			'bbutton' => array(
				'alt'			=> __('B I G   B U T T O N  :  A buttons with the specified colors and taglines.','pandathemes'),
				'attributes'	=> array(
					'url'			=> __('Target URL e.g. http://google.com','pandathemes'),
					'bg'			=> __('Background color e.g. 336699','pandathemes'),
					'tooltip'		=> __('Tooltip e.g. Hello World!','pandathemes'),
					'target'		=> __('Target window e.g. blank','pandathemes')
				)
			),

			'toggle' => array(
				'alt'			=> __('T O G G L E  :  Allows you to hide the part of content with toggle.','pandathemes'),
				'attributes'	=> array(
					'title'			=> __('Toggle title e.g. Click me!','pandathemes'),
					'status'		=> __('Status by loading e.g. open','pandathemes'),
					'style'			=> __('Toggle style e.g. 2 (by default: 1)','pandathemes')
				)
			)

		),

		// FEATURED
		'Featured' => array(

			'fieldset' => array(
				'alt'			=> __('F I E L D S E T  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'pbox' => array(
				'alt'			=> __('P R I C I N G   B O X  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(

				)
			),
	
			'note' => array(
				'alt'			=> __('N O T E  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'marker' => array(
				'alt'			=> __('M A R K E R  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			)

		),

		// TYPOGRAPHY
		'Typography' => array(

			'quote' => array(
				'alt'			=> __('Q U O T E  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'dropcap' => array(
				'alt'			=> __('D R O P C A P  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			)

		),

		// DESIGN
		'Design' => array(

			'sidebar' => array(
				'alt'			=> __('S I D E B A R  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'divider' => array(
				'alt'			=> __('D I V I D E R  :  Available for FULL version only','pandathemes'),
				'attributes'	=> ''
			),
	
			'line' => array(
				'alt'			=> __('L I N E  :  Available for FULL version only','pandathemes'),
				'attributes'	=> ''
			),
	
			'icon' => array(
				'alt'			=> __('I C O N  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),

			'email' => array(
				'alt'			=> __('E M A I L  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),

			'icon60' => array(
				'alt'			=> __('I C O N  6 0  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			)

		),

		// CONTENT
		'Content' => array(

			'posts' => array(
				'alt'			=> __('P O S T S  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'posts2' => array(
				'alt'			=> __('P O S T S #2  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'products' => array(
				'alt'			=> __('P R O D U C T S  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'img' => array(
				'alt'			=> __('I M G  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'hidden' => array(
				'alt'			=> __('H I D D E N  :  Available for FULL version only','pandathemes'),
				'attributes'	=> ''
			),
	
			'visible' => array(
				'alt'			=> __('V I S I B L E  :  Available for FULL version only','pandathemes'),
				'attributes'	=> ''
			)

		),

		// MISC
		'Misc' => array(

			'pages' => array(
				'alt'			=> __('P A G E S  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'categories' => array(
				'alt'			=> __('C A T E G O R I E S  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			),
	
			'archives' => array(
				'alt'			=> __('A R C H I V E S  :  Available for FULL version only','pandathemes'),
				'attributes'	=> ''
			),
	
			'tweet' => array(
				'alt'			=> __('T W E E T  :  Available for FULL version only','pandathemes'),
				'attributes'	=> ''
			),
	
			'like' => array(
				'alt'			=> __('L I K E  :  Available for FULL version only','pandathemes'),
				'attributes'	=> ''
			),

			'clear' => array(
				'alt'			=> __('C L E A R  :  Available for FULL version only','pandathemes'),
				'attributes'	=> array(
				)
			)

		)

	);

?>

	<head>

		<title><?php _e('Insert or Edit Shortcode','pandathemes') ?></title>
	
		<meta http-equiv="Content-Type" content="<?php bloginfo('html_type'); ?>; charset=<?php echo get_option('blog_charset'); ?>" />
		
		<script language="javascript" type="text/javascript" src="<?php echo get_option('siteurl') ?>/wp-includes/js/jquery/jquery.js"></script>
		<script language="javascript" type="text/javascript" src="<?php echo get_option('siteurl') ?>/wp-includes/js/tinymce/tiny_mce_popup.js"></script>

		<script language="javascript" type="text/javascript">
			tinyMCE.addI18n("en.shortcode_editor", {
				insert : 'Insert',
				update: 'Update'
			});
		</script>

		<script language="javascript" type="text/javascript">

			var jq = jQuery.noConflict();

				jq.selectFirstOption = function(){

					jq('#ok_button').click(function(){
						jq('#help').css({top:0}).animate({top:-310},500);
						return false;
					});

					// TABS
					jq('.kul li').each(function(){
						
						jq(this).click(function(){
	
							var li = jq(this);
				
								li.parent().children('.kcurrent').removeClass('kcurrent');
								li.addClass('kcurrent');
				
							var liIndex = li.prevAll().length;
							var ktabs = li.parent().next('.ktabs');
				
								ktabs.children('.block').removeClass('block');
								ktabs.children().eq(liIndex).addClass('block');
				
						})
				
					});

					// add and select the empty list
					jq('<option disabled="disabled">select</option>').prependTo('#gse_new_prop_name');
					jq('#gse_new_prop_name option:first').attr('selected','selected');
				}

				jq.addOptionToDom = function(value){
					var cclass, option = jq('#gse_new_prop_name option').remove();
						
					if(value){
						if(value == 'auto') 
							value = document.getElementById('gse_tag').value;
						else 
							removeAllSelectedOptions('gse_properties');
						
						document.getElementById('gse_remove_prop').disabled = true;
						
						// added to dom
						
						jq.each(option, function(i,v){
							if(jq(v).is('.option-'+value)){
								jq(v).appendTo('#gse_new_prop_name');
							}
						});
						jq.selectFirstOption();
						
					} else {
						jq.selectFirstOption();
						
						jq('#gse_tag').live('change',function(){ 
							cclass = jq(this).val();
							
							// plugin specific
							removeAllSelectedOptions('gse_properties');
							document.getElementById('gse_remove_prop').disabled = true;
							
							// remove all from dom
							jq('#gse_new_prop_name option').remove();
							
							// added to dom
							jq.each(option, function(i,v){
								if(jq(v).is('.option-'+cclass)){
									jq(v).appendTo('#gse_new_prop_name');
								}
							});
							jq.selectFirstOption();
							
						});	
					}
				
				}


			window.onload = function() {

				jq('form:eq(0)').live('submit',function() {

					//tinyMCEPopup.execCommand('mceInsertContent', true, makeShortcode());
					
					var shortcodeToAdd =  makeShortcode();
					tinyMCE.execInstanceCommand('content', 'mceInsertContent', false, shortcodeToAdd);
					
					tinyMCEPopup.close();

					return false;

				});

				document.getElementById('gse_new_prop_name').onkeydown = document.getElementById('gse_new_prop_value').onkeydown = function(e) {

						if ( (e.keyCode ? e.keyCode : e.charCode) == 13 ) { 

							if ( document.getElementById('gse_npn_label').style.display == 'none' ) updateProperty(); 

							else addProperty(); 

							return false; 

						}

				};

				jq('#gse_new_prop_add').live('click',function(e) {

					addProperty();

				});

				jq('#shortcode_close').live('click',function() {

					tinyMCEPopup.close();

				});

				jq('#gse_properties').live('change',function() {

					checkProperty();

				});

				jq('#gse_uncheck_prop').live('click',function(e) {

					uncheckProperties();

				});

				jq('#gse_remove_prop').live('click',function(e) {

					removeProperties();

					uncheckProperties();

				});
				
				var
					scont = tinyMCEPopup.editor.selection.getContent(),
					tager = parent.gse_shortcode_er;
					
					// if IE use selection from parameter url
					if(jq.browser.msie) {
						var IEselection = unescape(location.search.substr(1).split(";")[0].split('=')[1].split('&')[0]);
							scont = IEselection;
					}
					// console.log(IEselection);
						
					if ( scont ) {

						var test = tager.exec(scont); // the test works fine only one in each two times, why?

						if ( !test ) test = tager.exec(scont); // this fixes the problem, but not explains ...

						if ( test ) {

							var
								tag = test[1],
								props = test[2] || '',
								cont = test[4] || '',
								b = document.getElementById('shortcode_submit');
							
							jq.addOptionToDom(tag);
							selectOption('gse_tag', tag);

							setProperties(props);
							
							jq('#gse_content').html(cont);

							b.value = tinyMCEPopup.getLang('shortcode_editor.update');

							b.disabled = false;

							document.getElementById('gse_tag').disabled = true; // shortcode tag is not editable
							
						} 
					} else {	
						
						jq.addOptionToDom();
							
					}

				document.getElementById('gse_tag').focus();

			};


			
			function setProperties(props) {

				var parts = props.split(/\"/), p = [];

				for ( var i = 0; i < parts.length; i += 2 ) {

					var n = parts[i].replace(/^\s+|\s+$/g, '').replace('=', ''), v = parts[i+1];

					if ( n == 'sc_id' ) document.getElementById(n).value = v;

					else if ( n && v ) addOption('gse_properties', n+': '+v, n+'='+v);

				}

			}



			function uncheckProperties() {
				
				jq.addOptionToDom('auto');
				
				selectOption('gse_properties', '', true);

				document.getElementById('gse_new_prop_name').value = document.getElementById('gse_new_prop_value').value = '';

				document.getElementById('gse_remove_prop').disabled = document.getElementById('gse_uncheck_prop').disabled = true;

				document.getElementById('gse_npa_label').style.display = 'none'; 

				document.getElementById('gse_npn_label').style.display = 'inline'; 
				
				jq('#gse_new_prop_add').die('click');
				
				jq('#gse_new_prop_add').live('click' ,function() { addProperty(); }); 

				document.getElementById('gse_new_prop_name').focus();

			}



			function addProperty() {

				var
					n = document.getElementById('gse_new_prop_name'),
					v = document.getElementById('gse_new_prop_value');

				if ( !n.value ) return n.focus();

				if ( !v.value ) return v.focus();

				addOption('gse_properties', n.value+': '+v.value, n.value+'='+v.value);

				n.value = v.value = '';

					n.focus();

			}



			function updateProperty() {

				var
					n = document.getElementById('gse_new_prop_name'),
					v = document.getElementById('gse_new_prop_value'),
					sel = document.getElementById('gse_properties');

				if ( !n.value ) return n.focus();

				if ( !v.value ) return v.focus();

				sel.options[sel.selectedIndex].innerHTML = n.value+': '+v.value;

				sel.options[sel.selectedIndex].value = n.value+'='+v.value;

				uncheckProperties();

			}



			function removeProperties() {

				removeSelectedOptions('gse_properties');

				document.getElementById('gse_remove_prop').disabled = true;

			}



			// Select a tag
			function setTag() {

				document.getElementById('shortcode_submit').disabled = document.getElementById('gse_tag').selectedIndex == 0;

			}



			function checkProperty() {

				var
					sel = document.getElementById('gse_properties'),
					selected = sel.selectedIndex;

				document.getElementById('gse_remove_prop').disabled = document.getElementById('gse_uncheck_prop').disabled = (selected == -1);

					if ( selected > -1 ) {

						var pair = sel.options[selected].value.split('=');

						document.getElementById('gse_new_prop_name').value = pair[0]; 

						document.getElementById('gse_new_prop_value').value = pair[1];

						document.getElementById('gse_npa_label').style.display = 'inline'; 

						document.getElementById('gse_npn_label').style.display = 'none'; 
						
						jq('#gse_new_prop_add').die('click');
						
						jq('#gse_new_prop_add').live('click' ,function() { updateProperty(); }); 

					}

			}



			function addOption(sid, label, val) {

				var
					sel = document.getElementById(sid),
					opt = document.createElement('option');

				opt.text = label;

				opt.value = val;

				try { sel.add(opt, null); }

				catch(e) { sel.add(opt); } // IE only

			}



			function removeSelectedOptions(sid) {

				var sel = document.getElementById(sid);

				for ( var i = sel.length - 1; i >= 0; i-- ) {

					if ( sel.options[i].selected ) sel.remove(i);

				}
			}
			
			
			function removeAllSelectedOptions(sid) {

				var sel = document.getElementById(sid);

				for ( var i = sel.length - 1; i >= 0; i-- ) {

					sel.remove(i);

				}
			}



			// Detect created shortcode
			function selectOption(sid, val, uncheck) {
				
				var sel = document.getElementById(sid);

				if ( uncheck ) sel.selectedIndex = -1;

				for ( var i = 0; i < sel.options.length; i++ ) {

					if ( sel.options[i].value == val ) sel.options[i].selected = true;

				}
				
				// fix for opera
				jq('#'+sid+' option[value="'+val+'"]').attr('selected',true);

			}



			function makeShortcode(cod) {

				var
					tag = document.getElementById('gse_tag').value, 
					props = document.getElementById('gse_properties').options, 
					cont = document.getElementById('gse_content').value, 
					scid = 0, 
					code = '['+tag;

				if ( props.length ) {

					for ( var i = 0; i < props.length; i++ ) code += ' '+props[i].value.replace('=', '="')+'"';

				}

				var scid = document.getElementById('sc_id').value;

				if ( !scid ) scid = tinyMCEPopup.editor.plugins.shortcode_editor.getId();

					code += ' sc_id="'+scid+'"]';

				if ( cont ) code += cont+'[/'+tag+']';

					tinyMCEPopup.editor.plugins.shortcode_editor.cache(scid, code);
				
				return cod ? code : tinyMCEPopup.editor.plugins.shortcode_editor.toHTML(code);

/*				var is_chrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;

				if (is_chrome) {

					return cod ? code : tinyMCEPopup.close();

				} else {

					return cod ? code : tinyMCEPopup.editor.plugins.shortcode_editor.toHTML(code);

				};
*/
			}

		</script>

		<style type="text/css">

			* {
				font-family:Verdana,Arial;
				font-size:11px !important;
				}

			form {
				padding:0;
				margin:0;
				}

			select,
			input,
			textarea {
				padding:2px;
				border-radius:3px;
				}

			#gse_properties {
				width:100%;
				height:8em;
				}
	
			.small {
				width:100px;
				}
	
			.autow {
				width:auto;
				}
	
			#gse_remove_prop,
			#gse_uncheck_prop {
				float:right;
				margin:0 0 0 5px;
				}
	
			#gse_tag {
				margin:0;
				padding:3px;
				width:130px;
				float:left;
				}

			#gse_content {
				width:100%;
				height:155px;
				}

			.sc-button {
				border:1px solid #bbb; 
				margin:0; 
				padding:2px 5px;
				font-size:11px;
				color:#000;
				cursor:pointer;
				-webkit-border-radius:3px;
				border-radius:3px;
				background-color: #eee; /* Fallback */
				background-image: -ms-linear-gradient(bottom, #ddd, #fff); /* IE10 */
				background-image: -moz-linear-gradient(bottom, #ddd, #fff); /* Firefox */
				background-image: -o-linear-gradient(bottom, #ddd, #fff); /* Opera */
				background-image: -webkit-gradient(linear, left bottom, left top, from(#ddd), to(#fff)); /* old Webkit */
				background-image: -webkit-linear-gradient(bottom, #ddd, #fff); /* new Webkit */
				background-image: linear-gradient(bottom, #ddd, #fff); /* proposed W3C Markup */		
			}

			.sc-button[disabled="disabled"],
			.sc-button[disabled="disabled"]:hover {
				color:#888;
				border:1px solid #bbb;
				cursor:default;
				}

			#shortcode_close,
			#shortcode_submit,
			#ok_button {
				border: 1px solid #bbb; 
				margin:0; 
				padding:0 0 1px;
				font-weight:bold;
				font-size: 11px;
				width:100px; 
				height:24px;
				color:#000;
				cursor:pointer;
				-webkit-border-radius: 3px;
				border-radius: 3px;
				background-color: #eee; /* Fallback */
				background-image: -ms-linear-gradient(bottom, #ddd, #fff); /* IE10 */
				background-image: -moz-linear-gradient(bottom, #ddd, #fff); /* Firefox */
				background-image: -o-linear-gradient(bottom, #ddd, #fff); /* Opera */
				background-image: -webkit-gradient(linear, left bottom, left top, from(#ddd), to(#fff)); /* old Webkit */
				background-image: -webkit-linear-gradient(bottom, #ddd, #fff); /* new Webkit */
				background-image: linear-gradient(bottom, #ddd, #fff); /* proposed W3C Markup */
			}

			.sc-button:hover,
			.sc-button:focus,
			#shortcode_close:hover,
			#shortcode_close:focus,
			#shortcode_submit:hover,
			#shortcode_submit:focus,
			#ok_button:hover {
				border: 1px solid #555;
			}

			fieldset {
				margin:0 0 10px;
				padding:5px 10px 10px;
				border:1px solid #CCC;
				border-radius:2px;
				position:relative;
				}

			.clear {
				clear:both;
				}

			.h10 {
				height:10px;
				}

			/*-- TABS -----------------------------------------------*/

			.kul {
				list-style:none;
				padding:0;
				margin:0;
				}
			
			.kul li {
				float:left;
				margin:0 2px -1px 0;
				padding:3px 10px;
				font-size:10px;
				display:block;
				cursor:pointer;
				border:1px solid #919B9C;
				border-top-left-radius:2px;
				border-top-right-radius:2px;
				background-color: #eee; /* Fallback */
				background-image: -ms-linear-gradient(bottom, #ddd, #fff); /* IE10 */
				background-image: -moz-linear-gradient(bottom, #ddd, #fff); /* Firefox */
				background-image: -o-linear-gradient(bottom, #ddd, #fff); /* Opera */
				background-image: -webkit-gradient(linear, left bottom, left top, from(#ddd), to(#fff)); /* old Webkit */
				background-image: -webkit-linear-gradient(bottom, #ddd, #fff); /* new Webkit */
				background-image: linear-gradient(bottom, #ddd, #fff); /* proposed W3C Markup */
				}
			
			li.kcurrent,
			.kul li:hover {
				background:#FFF;
				border-bottom:1px solid #FFF;
				cursor:pointer;
				}
			
			.ktabs {
				clear:both;
				border:1px solid #919B9C;
				margin:0 0 10px;
				padding:10px;
				background:#FFF;
				border-radius:2px;
				border-top-left-radius:0;
				min-height:157px;
				}
			
			.ktabs .kt {
				display:none;
				}

			.ktabs .block {
				display:block;
				}

			#helplink {
				float:right;
				margin:3px 0 0 0;
				}

			#help {
				width:375px;
				height:310px;
				background:#F1F1F1;
				top:-310px;
				left:0;
				position:absolute;
				z-index:1000;
			}

			#h {
				position:relative;
				padding:30px 25px 25px 25px;
			}

			.ul {
				margin:0 25px 3em 0;
			}

			.ul li {
				margin:0 0 1em 0;
			}

			.block {
				display:block;
			}

			.none {
				display:none;
				}

		</style>

	</head>

	<body>

		<form action="" method="post" id="shortcode_dialog">

			<fieldset><legend><?php _e('Shortcode','pandathemes') ?></legend>

				<select id="gse_tag" attr="gse_tag" name="gse_tag" onChange="setTag()">
	
					<option value="0"><?php _e('Select one','pandathemes') ?></option>
	
					<?php
	
						$out = '';
	
						foreach ($sc as $group => $shortcode) {
						
							$out .= '<option disabled> </option><option disabled> ' . $group . ' </option>';
						
							foreach ($shortcode as $name => $alt) {

								//if ( $name == 'button' || $name == 'bbutton' || $name == 'toggle' ) :
									$status = '';
								//else :
									//$status = 'disabled="disabled"';
								//endif;

								$out .= '<option ' . $status . ' value="' . $name . '" title="' . $alt['alt'] . '">&nbsp; ' . $name . '</option>';
	
							}
						
						}
	
						echo $out;
	
					?>
	
				</select>

				<a id="helplink" target="_blank" href="http://pandathemes.com/wordpress-themes/topbusiness/">Get FULL version</a>

				<div class="clear"><!-- --></div>

			</fieldset>

			<ul class="kul kul-auto">
				<li class="kcurrent">Attibutes</li>
				<li>Content</li>
			</ul>
			
			<div class="ktabs ktabs-auto">
		
				<div class="kt block">

					<select id="gse_new_prop_name">
	
						<?php
	
							$out = '';
	
							foreach ($sc as $group => $shortcode) {
							
								foreach ($shortcode as $name => $attributes) {
	
									foreach ($attributes as $attr => $desc) {
	
										foreach ($desc as $value => $v) {
	
											$out .= '<option class="option-' . $name . '" value="' . $value . '" title="' . $v . '">' . $value . '</option>';
	
										}
	
									}
	
								}
							
							}
	
							echo $out;
	
						?>
	
					</select>
	
					<input type="text" id="gse_new_prop_value" />
	
					<input class="sc-button" type="button" id="gse_new_prop_add" value="<?php _e('Apply','pandathemes') ?>" class="autow" />

					<div class="clear h10"><!-- --></div>

					<select id="gse_properties" size="8"></select>

					<div class="clear h10"><!-- --></div>

					<input class="sc-button" type="button" id="gse_uncheck_prop" value="<?php _e('Uncheck','pandathemes') ?>" disabled="disabled" />
					<input class="sc-button" type="button" id="gse_remove_prop" value="<?php _e('Remove','pandathemes') ?>" disabled="disabled" />

					<div class="clear"><!-- --></div>

				</div>
		
				<div class="kt">
					<textarea id="gse_content" name="gse_content"></textarea>
				</div>
		
			</div>

			<div style="float:left;"><input type="button" id="shortcode_close" value="<?php _e('Cancel','pandathemes') ?>" /></div>
			<div style="float:right;"><input type="submit" id="shortcode_submit" value="{#shortcode_editor.insert}" disabled="disabled" /></div>
	
			<input type="hidden" id="sc_id" />

		</form>

	</body>

</html>