<?php // by PandaThemes.com


// ***********************************
//
// 	B U T T O N
//
// ***********************************


	function button( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'url'			=> '#',
			'bg'			=> '',
			'bg_hover'		=> '',
			'color'			=> '',
			'color_hover'	=> '',
			'size'			=> '',
			'radius'		=> '',
			'tooltip'		=> '',
			'target'		=> 'parent'
		), $atts));
	
		$id = 'button'.rand(99,99999);

		$a = '';
		$s = '';
		$sHover = '';


			// A styles
			if ( $size || $radius ) :

				$a .= ($size) ? ' font-size:'.$size.'px !important;' : '';
				$a .= ($radius) ? ' border-radius:'.$radius.'px !important;' : '';

			endif;


			// Span styles
			if ( $bg || $color || $size || $radius ) :

				$s .= ($bg) ? ' background:#' . $bg . ' url(' . get_bloginfo('template_url') . '/images/bg_tab_span4.png) top left repeat-x !important; background-size:auto auto !important; border:1px solid #' . $bg . ' !important;' : '';
				$s .= ($color) ? ' color:#'.$color.' !important;' : '';
				$s .= ($color == 'FFF' || $color == 'FFFFFF') ? ' text-shadow:-1px -1px 0 rgba(0,0,0,0.15) !important;' : '';
				$s .= ($radius) ? ' border-radius:'.$radius.'px !important;' : '';

			endif;


			// Span styles by hover
			if ( $bg_hover || $color_hover ) :

				$sHover .= ($bg_hover) ? ' background-color:#'.$bg_hover.' !important; border:1px solid #'.$bg_hover.' !important;' : '';
				$sHover .= ($color_hover) ? ' color:#'.$color_hover.' !important;' : '';

			endif;


		$style = "<style type='text/css'>";

			$style .= ($a) ? '#'.$id.' {'.$a.' } ' : '';

			$style .= ($s) ? '#'.$id.' {'.$s.' } ' : '';

			$style .= ($sHover) ? '#'.$id.':hover {'.$sHover.' } ' : '';

		if ($style != "<style type='text/css'>") : $style .= '</style>'; else : $style = ''; endif;


		$radius = ($radius) ? 'style="border-radius:'.$radius.'px"' : '';

		$tooltip = ($tooltip) ? ' title="'.$tooltip.'"' : '';

		$tipclass = ($tooltip) ? ' tooltip-t' : '';


		$out = '<a class="button'.$tipclass.'" id="'.$id.'" href="'.$url.'"'.$tooltip.' target="_'.$target.'">'.do_shortcode($content).'</a> '.$style;


	return $out; }

	add_shortcode('button', 'button');



// ***********************************
//
// 	B I G   B U T T O N
//
// ***********************************



	function bbutton( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'w'				=> 'auto',
			'url'			=> '#',
			'bg'			=> '',
			'tooltip'		=> '',
			'tagline'		=> '',
			'style'			=> '2',
			'target'		=> 'parent'
		), $atts));

		$id = rand(99,9999);


		$tooltip = ($tooltip) ? ' title="'.$tooltip.'"' : '';

		$tipclass = ($tooltip) ? ' tooltip-t' : '';


		// STYLE 2
		if ($style == '2') :

			// Styles
			$s = '';
	
				// Bg color
				if ( $bg ) :

					$s .= ($bg) ? 'background-color:#'.$bg.' !important;' : '';

				endif;
	
				// Width
				$s .= 'width:' . $w . ';';


			// Tagline
			if ( $tagline ) :
	
				$tagline = ($tagline) ? '<span class="f11 block bb2c lh13">' . $tagline . '</span>' : '';
	
			endif;

			// Out
			$out = '<a style="' . $s . '" class="bbutton2' . $tipclass . '" href="' . $url . '"'.$tooltip . ' target="_' . $target . '"' . $bg . '><div class="lh13 bb2a"><span class="f18 bb2b">' . $content . '</span>' . $tagline . '</div></a>';


		// STYLE 1
		else :

			// Bull color
			if ( $bg ) :

				$bg = ($bg) ? ' style="background-color:#'.$bg.' !important;"' : '';

			endif;

			// Tagline
			if ( $tagline ) :
	
				$tagline = ($tagline) ? '<span class="block f11 gray shdw-w-txt-100 lh10">' . $tagline . '</span>' : '';
	
			endif;

			// Out
			$out = '<div><a class="bbutton left' . $tipclass . '" href="' . $url . '"' . $tooltip . ' target="_' . $target . '"><span class="b"><span' . $bg . '><!-- bull --></span></span><span class="a relative"><div class="div-as-table"><div><div><span class="c f22 shdw-w-txt-100 lh10">' . $content . '</span>' . $tagline . '</div></div></div></span></a></div>';


		endif;


	return $out; }

	add_shortcode('bbutton', 'bbutton');

?>