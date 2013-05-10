<?php // by PandaThemes.com



// ***********************************
//
// 	I C O N   6 0
//
// ***********************************



	function icon60( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'w'				=> '',
			'm'				=> '',
			'size'			=> '16',
			'ico'			=> '1',
			'src'			=> '',
			'url'			=> '',
			'target'		=> 'parent',
			'bg'			=> 'FFF',
			'bg_hover'		=> '030303',
			'color'			=> '333',
			'color_hover'	=> 'FFF',
			'bg_ico'		=> '333',
			'bg_ico_hover'	=> '1569ff',
			'class'			=> ''
		), $atts));

		// Common
		$tag = ($url) ? 'a href="' . $url . '" target="_' . $target . '"' : 'div';

		$tag_end = ($url) ? 'a' : 'div';

		$w = ($w) ? ' width:' . $w . ';' : '';

		$m = ($m) ? ' margin:' . $m . ';' : '';

		$size = ($size) ? 'font-size:' . $size . 'px;' : '';

		// Background
		$bg_color = ($bg) ? ' background-color:#' . $bg . ';' : '';

		$bg = ($bg) ? ' bg="' . $bg . '"' : '';

		$bg_hover = ($bg_hover) ? ' bg_hover="' . $bg_hover . '"' : '';

		// Color
		$color_color = ($color) ? ' color:#' . $color . ';' : '';

		$color = ($color) ? ' color="' . $color . '"' : '';

		$color_hover = ($color_hover) ? ' color_hover="' . $color_hover . '"' : '';

		// Icon
		$src = ($src) ? $src : get_bloginfo('template_url') . '/images/icons/32/white/' . $ico . '.png';

		$bg_ico_color = ($bg_ico) ? ' background-color:#' . $bg_ico . ';' : '';

		$bg_ico = ($bg_ico) ? ' bg_ico="' . $bg_ico . '"' : '';

		$bg_ico_hover = ($bg_ico_hover) ? ' bg_ico_hover="' . $bg_ico_hover . '"' : '';

		// Out
		$out = '
			<' . $tag . ' class="br3 icon60-link block ' . $class . '" style="' . $w . $m . $bg_color . $color_color . '"' . $bg . $bg_hover . $color . $color_hover . $bg_ico . $bg_ico_hover . '>
				<table>
					<tr>
						<td style="width:70px;"><span class="block i60 br3" style="' . $bg_ico_color . '"><em class="block" style="background:url(' . $src . ') center center no-repeat;"><!-- icon --></em></span></td>
						<td style="' . $size . '">' . $content . '</td>
					</tr>
				</table>
			</' . $tag_end . '>';

	return $out; }

	add_shortcode('icon60', 'icon60');



// ***********************************
//
// 	I C O N   1 6
//
// ***********************************



	function icon( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'name'			=> 'arrow_right.png',
			'src'			=> '',
			'url'			=> '',
			'target'		=> 'parent',
			'tooltip'		=> ''
		), $atts));
		
		$tag = ($url) ? 'a href="'.$url.'" target="_'.$target.'"' : 'span';
		
		$src = ($src) ? $src : get_bloginfo('template_url').'/images/icons/16/led-icons/'.$name;

		$tooltip = ($tooltip) ? ' title="'.$tooltip.'"' : '';

		$tipclass = ($tooltip) ? ' tooltip-t' : '';

		$out = '<'.$tag.' class="icon16'.$tipclass.'"'.$tooltip.' style="background-image:url('.$src.') !important;">'.$content.'</'.$tag.'>';

	return $out; }

	add_shortcode('icon', 'icon');



// ***********************************
//
// 	I C O N   M A I L
//
// ***********************************



	function email( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'name'			=> 'email.png',
			'src'			=> '',
			'to'			=> 'mailbox@website.tld',
			'tooltip'		=> ''
		), $atts));

		$src = ($src) ? $src : get_bloginfo('template_url').'/images/icons/16/led-icons/'.$name;

		$tooltip = ($tooltip) ? ' title="'.$tooltip.'"' : '';

		$tipclass = ($tooltip) ? ' tooltip-t' : '';

		$out = '<a href="mailto:'.$to.'" class="icon16'.$tipclass.'"'.$tooltip.' style="background-image:url('.$src.') !important;">'.$content.'</a>';

		return $out; }
	add_shortcode('email', 'email');



?>