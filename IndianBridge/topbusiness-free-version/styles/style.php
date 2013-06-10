<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

// by PandaThemes.com

$template_url = get_bloginfo('template_url');

$out = '
/*
	THIS FILE HAS BEEN CREATED AUTOMATICALLY.
	DO NOT MODIFY THIS FILE. ALL CUSTOM CHANGES WILL BE LOST.
*/

';



// ***********************************
//
// 	S T Y L E
//
// ***********************************



	// LAYOUT

		// Background
		$out .= ( $panda_st['background_color'] ) ? 'body, body.login { background-color:#'.$panda_st['background_color'].'; } ' : 'body.login { background-color:#1569a2; }';

		// Pattern
		$pattern = ( $panda_st['pattern'] ) ? 'body, body.login { background-image:url('.$template_url.'/images/patterns/'.$panda_st['pattern'].'.png); } ' : 'body.login { background-image:url('.$template_url.'/images/grd-white-80.png); }';

		// Custom pattern
		$custom_pattern = ( $panda_st['background_image'] ) ? 'body, body.login { background-image:url('.$panda_st['background_image'].'); } ' : '';

		// Select pattern
		$out .= ( $custom_pattern ) ? $custom_pattern : $pattern;

		// Pattern options
		if ( $pattern || $custom_pattern ) :

			// Position
			$out .= ( $panda_st['background_image_position'] ) ? 'body, body.login { background-position:'.$panda_st['background_image_position'].'; } ' : 'body.login { background-position:top center; }';
	
			// Tiling
			$out .= ( $panda_st['background_repeat'] ) ? 'body, body.login { background-repeat:'.$panda_st['background_repeat'].'; } ' : 'body.login { background-repeat:repeat-x; }';

			// Attachment
			$out .= ( $panda_st['background_attachment'] ) ? 'body, body.login { background-attachment:fixed; } ' : 'body, body.login { background-attachment:scroll; } ';

		endif;



		// LAYOUT LOG IN

			// Logo
			$out .= ($theme_options['logo']) ? 'body.login h1 a { background:transparent url('.$theme_options['logo'].') center center no-repeat; }' : 'body.login h1 a { background:transparent url('.$template_url.'/images/logo.png) center center no-repeat; }';

			// Login form
			$out .= 'body.login form { box-shadow:0 10px 30px rgba(0, 0, 0, 0.3); }';

			// Links
			$out .= '
				.login #nav a,
				.login #nav a:hover,
				.login #backtoblog a,
				.login #backtoblog a:hover {
					text-shadow:0 0 6px rgba(0, 0, 0, 0.8);
					color:#FFF !important;
					}
				';



	// MENU

		// Color

			// By default
			$out .= ( $panda_st['menu_color'] ) ? 'ul.top-menu > li > a, #topmess, #topmess a, #topmess strong, #selectElement { color:#'.$panda_st['menu_color'].'; } ' : '';

			// By hover
			$out .= ( $panda_st['menu_hover_color'] ) ? 'ul.top-menu > li > a:hover, ul.top-menu > li.hover-has-ul > a, ul.top-menu > li.hover-has-ul > a:hover, #topmess a:hover { color:#'.$panda_st['menu_hover_color'].' !important; } ' : '';

			// Current
			$out .= ( $panda_st['menu_selected_color'] ) ? 'ul.top-menu > li.current-menu-item > a, ul.top-menu > li.current-menu-ancestor > a { color:#'.$panda_st['menu_selected_color'].'; } ' : '';


		// Background

			// By default
			$out .= ( $panda_st['menu_bg_color'] ) ? '
				#menuwrapper,
				#searchform input#searchsubmit,
				.feedemail-form .feedemail-button,
				.pr-bar h3,
				.kul,
				.prdcts h3,
				.wp-pagenavi span.current,
				div.item-list-tabs,
				#selectElement option {
					background-color:#'.$panda_st['menu_bg_color'].';
					}

				.wp-pagenavi span.current {
					border-color:#'.$panda_st['menu_bg_color'].';
					}

				' : '';

			// By hover
			$out .= ( $panda_st['menu_hover_bg_color'] ) ? '
				ul.top-menu > li > a:hover,
				ul.top-menu > li.hover-has-ul > a,
				ul.top-menu > li.hover-has-ul > a:hover,
				ul.top-menu li.basic ul li a:hover,
				ul.top-menu li.basic ul li.current-cat > a:hover,
				ul.top-menu li.basic ul li.current_page_item > a:hover {
					background-color:#'.$panda_st['menu_hover_bg_color'].' !important;
					}
				' : '';

			// Current
			$out .= ( $panda_st['menu_selected_bg_color'] ) ? '
				ul.top-menu > li.current-menu-item > a,
				ul.top-menu > li.current-menu-ancestor > a {
					background-color:#'.$panda_st['menu_selected_bg_color'].';
					}
				' : '';


		// Divider
		$out .= ( $panda_st['separator'] && $panda_st['separator'] != 'default' ) ? '
			ul.top-menu > li { background:url('.$template_url.'/images/'.$panda_st['separator'].'.png) top right repeat-y; }
			ul.top-menu { background-image:url('.$template_url.'/images/'.$panda_st['separator'].'.png); }
			' : '';



	// FOOTER

		// Background
		$out .= ( $panda_st['footer_bg'] ) ? '#footerarea { background-color:#'.$panda_st['footer_bg'].'; } #subfooter { background-image:url(../../images/shadow.png); height:39px; } ' : '';

		// Titles
		$out .= ( $panda_st['footer_titles_color'] ) ? '#footer h1, #footer h2, #footer h3, #footer h4, #footer h, #footer h5, #footer h6, #footer .dropcap { color:#'.$panda_st['footer_titles_color'].'; } ' : '';

		// Text
		$out .= ( $panda_st['footer_text_color'] ) ? '#footerarea, #footerarea li, #footerarea p, #footerarea td, #footerarea th, #footerarea span, #footerarea div, #footerarea #wp-calendar tbody a { color:#'.$panda_st['footer_text_color'].'; } ' : '';

		// Links
		$out .= ( $panda_st['footer_links_color'] ) ? '
			#footerarea a {
				color:#'.$panda_st['footer_links_color'].';
				}

			#wp-calendar a,
			#footerarea .tag-cloud a:hover {
				background:#'.$panda_st['footer_links_color'].';
				}

			' : '';



// ***********************************
//
// 	T Y P O R G A P H Y
//
// ***********************************



	// TITLES


		// Select the group 


			// C u f o n
			if ( $panda_ty['titles_font_group'] == 'cufon' ) :

				/* nothing here */


			// G o o g l e
			elseif ( $panda_ty['titles_font_group'] == 'google' ) :
	
				// classes
				$classes = '
					/* titles */
						h1, h2, h3, h4, h5, h6,
					/* pricing box header */
						.pbox .pa *,
					/* misc titles */
						.font-replace, .dropcap, .dropcap2 div div, .f36, .f30, .f26, .f24, .f22, .f20, .f18, .f16,
					/* buttons*/
						.bbutton .c
				';
				
				// custom classes
				$classes .= ( $panda_ty['google_classes'] ) ? ', '.$panda_ty['google_classes'] : '';
	
				// select google font
				if ( $panda_ty['google_custom'] ) :

					$out .= $classes.' { font-family: \''.$panda_ty['google_panda_family_custom'].'\'; } ';

				else :

					$out .= $classes.' { font-family: \''.$panda_ty['google_panda_family'].'\'; } ';

				endif;

	
			// S y s t e m
			else :
	
				$out .= ( $panda_ty['titles_font'] != 'select' ) ? '
					/* titles */
						h1, h2, h3, h4, h5, h6,
					/* pricing box header */
						.pbox .pa *,
					/* misc titles */
						.font-replace, .dropcap, .dropcap2 div div { font-family:'.$panda_ty['titles_font'].'; } ' : '';
	
	
			endif;



		// H1
		$out .= ( $panda_ty['h1_color'] ) ? 'h1, h1 a { color:#'.$panda_ty['h1_color'].'; } ' : '';
		$out .= ( $panda_ty['h1_size'] != 'select' ) ? 'h1 { font-size:'.$panda_ty['h1_size'].'px; } ' : '';

		// H2
		$out .= ( $panda_ty['h2_color'] ) ? 'h2, h2 a { color:#'.$panda_ty['h2_color'].'; } ' : '';
		$out .= ( $panda_ty['h2_size'] != 'select' ) ? 'h2 { font-size:'.$panda_ty['h2_size'].'px; } ' : '';

		// H3
		$out .= ( $panda_ty['h3_color'] ) ? 'h3, h3 a, .it1 h3 a, .it2 h3 a, .it3 h3 a { color:#'.$panda_ty['h3_color'].'; } ' : '';
		$out .= ( $panda_ty['h3_size'] != 'select' ) ? 'h3 { font-size:'.$panda_ty['h3_size'].'px; } ' : '';

		// H4
		$out .= ( $panda_ty['h4_color'] ) ? 'h4, h4 a { color:#'.$panda_ty['h4_color'].'; } ' : '';
		$out .= ( $panda_ty['h4_size'] != 'select' ) ? 'h4 { font-size:'.$panda_ty['h4_size'].'px; } ' : '';

		// H5
		$out .= ( $panda_ty['h5_color'] ) ? 'h5, h5 a { color:#'.$panda_ty['h5_color'].'; } ' : '';
		$out .= ( $panda_ty['h5_size'] != 'select' ) ? 'h5 { font-size:'.$panda_ty['h5_size'].'px; } ' : '';

		// H6
		$out .= ( $panda_ty['h6_color'] ) ? 'h6, h6 a { color:#'.$panda_ty['h6_color'].'; } ' : '';
		$out .= ( $panda_ty['h6_size'] != 'select' ) ? 'h6 { font-size:'.$panda_ty['h6_size'].'px; } ' : '';



	// CONTENT

		// Font family
		$out .= ( $panda_ty['font'] && $panda_ty['font'] != 'select' ) ? 'td, body, div, input { font-family:\''.$panda_ty['font'].'\'; } ' : '';
		$out .= ( $panda_ty['font_size'] && $panda_ty['font_size'] != 'select' ) ? 'td, body, div { font-size:'.$panda_ty['font_size'].'px; } ' : '';

		// Links
		$out .= ( $panda_ty['links_color'] ) ? '
			a {
				color:#'.$panda_ty['links_color'].';
				}


			/* T o g g l e */
			.tgl-1 .ta {
				color:#'.$panda_ty['links_color'].';
				}

			.tgl-1.tclicked .ta {
				background-color:#'.$panda_ty['links_color'].';
				}


			/* D e m o  W */
			.demo-W {
				background-color:#'.$panda_ty['links_color'].';
			}


			/* H 1   t i t l e */
			#header h1.logo a strong {
				color:#'.$panda_ty['links_color'].';
			}


			/* T i t l e s */
			h2 a, h3 a, h4 a, h5 a, h6 a {
				color:#'.$panda_ty['links_color'].';
			}


			/* P a n d a   s l i d e r   t a b s */
			.ps-1 .pnavbar span:hover {
				color:#'.$panda_ty['links_color'].';
				}
			
			.ps-1 .pnavbar span.pscurrent {
				border-top:3px solid #'.$panda_ty['links_color'].';
				color:#'.$panda_ty['links_color'].';
				}
			
			.ps-1 .pnavbar span.pscurrent strong {
				background-color:#'.$panda_ty['links_color'].';
				}


			.tag-cloud a:hover {
				background:#'.$panda_ty['links_color'].';
				}


			/* C u s t o m   p o s t s   c a t e g o r y  t a b s */
			.ctabs a.current {
				border-top:3px solid #'.$panda_ty['links_color'].';
				color:#'.$panda_ty['links_color'].';
				}
			
			.ctabs a.current span {
				background-color:#'.$panda_ty['links_color'].';
				}


			/* t 1   b u t t o n s */
			.t1-button-holder a:hover span {
				background-color:#'.$panda_ty['links_color'].';
				border-color:#'.$panda_ty['links_color'].';
				}


			/* B i g   b u t t o n   v . 2  */
			a.bbutton2 {
				background-color:#'.$panda_ty['links_color'].';
				}


			/* k u l   t a b   h o v e r s */
			li.kcurrent, .kul li:hover, li.current  {
				color:#'.$panda_ty['links_color'].';
				}


			/* B u d d y P r e s s   t a b s */
			#item-nav a:hover {
				color:#'.$panda_ty['links_color'].';
				}
			
			#item-nav .selected a, #item-nav .current a {
				color:#'.$panda_ty['links_color'].';
				}

			#subnav li.current a {
				background:#'.$panda_ty['links_color'].';
				}


			#wp-calendar a,
			#searchform input#searchsubmit:hover,
			.feedemail-form .feedemail-button:hover,
			/* C u s t o m   m e n u   w i d g e t */
			.widget ul.menu > li > a:hover {
				background-color:#'.$panda_ty['links_color'].';
				}

			' : '';

		// Links by hover
		$out .= ( $panda_ty['links_hover_color'] ) ? '
			a:hover {
				color:#'.$panda_ty['links_hover_color'].';
				}

			.tag-cloud a:hover {
				background:#'.$panda_ty['links_hover_color'].';
				}

			.widget ul.menu > li > a:hover {
				background-color:#'.$panda_ty['links_hover_color'].';
				}


			/* P a n d a   s l i d e r   t a b s */
			.ps-1 .pnavbar span:hover strong,
			.ps-1 .pnavbar span.pscurrent:hover strong {
				background-color:#'.$panda_ty['links_hover_color'].';
				}
			
			.ps-1 .pnavbar span:hover {
				color:#'.$panda_ty['links_hover_color'].';
				border-top:3px solid #'.$panda_ty['links_hover_color'].';
				}


			/* C u s t o m   p o s t s   c a t e g o r y  t a b s */
			.ctabs a:hover {
				border-top:3px solid #'.$panda_ty['links_hover_color'].';
				color:#'.$panda_ty['links_hover_color'].';
				}
			
			.ctabs a:hover span,
			.ctabs a.current:hover span {
				background-color:#'.$panda_ty['links_hover_color'].';
				}


			' : '';



// ***********************************
//
// 	C U S T O M   C S S
//
// ***********************************

	// CUSTOM CSS
	$out .= ( $panda_st['custom_css'] ) ? $panda_st['custom_css'] : '';



	// WRITE A FILE

	$cssFile = TEMPLATEPATH."/styles/custom_style.css";

	chmod($cssFile, 0777);

	$fh = fopen($cssFile, 'w+') or die('<div class="pa-updated-message">Can not open ../wp-content/themes/THEMENAME/styles/custom_style.css file. Set a CHMOD 777 for the custom_style.css file.</div>');

		fwrite($fh, $out);
		
		chmod($cssFile, 0777);

	fclose($fh);

?>