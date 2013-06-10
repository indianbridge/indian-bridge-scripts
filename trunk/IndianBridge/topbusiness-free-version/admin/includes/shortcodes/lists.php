<?php // by PandaThemes.com


// ***********************************
//
// 	U L
//
// ***********************************


	function ul( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'bullit'	=> '',
			'm'			=> ''
		), $atts));
		
		$id = 'list'.rand(99,9999);

		$out = str_replace('<ul>', '<ul class="inline-block list" id="'.$id.'">', do_shortcode($content));

		$style = "<style type='text/css'>";

			$style .= ($bullit) ? '

				ul#'.$id.' {
					list-style-type:none;
					margin:0;
					}

				ul#'.$id.' > li {
					background:url('.get_bloginfo('template_url').'/images/icons/16/led-icons/'.$bullit.') left 2px no-repeat !important;
					padding:0 0 0 23px;
					}

				' : '';

			$style .= ($m) ? '

				ul#'.$id.' > li {
					margin:0 0 '.$m.'px 0 !important;
					}
				
				' : '';

		if ($style != "<style type='text/css'>") : $style .= '</style>'; else : $style = ''; endif;

		$out .= $style;

	return $out; }

	add_shortcode('ul', 'ul');



// ***********************************
//
// 	L I S T   O F   P A G E S
//
// ***********************************



	function pages( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'exclude'	=> '',
			'include'	=> ''
		), $atts));

		$id = 'list'.rand(99,9999);

		$style = "<style type='text/css'>";

			$style .= '

				ul#'.$id.' {
					list-style-type:none;
					margin:0 0 1em;
					}

				ul#'.$id.' li {
					background:url('.get_bloginfo('template_url').'/images/icons/16/led-icons/page_white_text_width.png) left 2px no-repeat !important;
					padding:0 0 0 22px;
					}

				ul#'.$id.' li ul.children {
					margin:0 0 0 -1em;
					}

				';

		$style .= '</style>';


		if ($include) :

			$pages = wp_list_pages('title_li=&child_of=&echo=0&include='.$include);

		else :

			$pages = wp_list_pages('title_li=&child_of=&echo=0&exclude='.$exclude);

		endif;


	return '<ul id="'.$id.'" class="inline-block list">'.$pages.'</ul>'.$style;}

	add_shortcode('pages', 'pages');



// ***********************************
//
// 	L I S T   O F   C A T E G O R I E S
//
// ***********************************



	function categories( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'exclude'		=> '',
			'include'		=> ''
		), $atts));

		$id = 'list'.rand(99,9999);

		$style = "<style type='text/css'>";

			$style .= '

				ul#'.$id.' {
					list-style-type:none;
					margin:0 0 1em;
					}

				ul#'.$id.' li {
					background:url('.get_bloginfo('template_url').'/images/icons/16/led-icons/folder.png) 1px 1px no-repeat !important;
					padding:0 0 0 22px;
					}

				ul#'.$id.' li ul.children {
					margin:0 0 0 -1em;
					}

				';

		$style .= '</style>';


		if ($include) :

			$categories = wp_list_categories('title_li=&child_of=&echo=0&show_count=1&hide_empty=0&include='.$include);

		else :

			$categories = wp_list_categories('title_li=&child_of=&echo=0&show_count=1&hide_empty=0&exclude='.$exclude);

		endif;


	return '<ul id="'.$id.'" class="inline-block list">'.$categories.'</ul>'.$style;}

	add_shortcode('categories', 'categories');



// ***********************************
//
// 	L I S T   O F   A R C H I V E S
//
// ***********************************



	function archives( $atts, $content = null ) {

		$id = 'list'.rand(99,9999);

		$style = "<style type='text/css'>";

			$style .= '

				ul#'.$id.' {
					list-style-type:none;
					margin:0 0 1em;
					}

				ul#'.$id.' li {
					background:url('.get_bloginfo('template_url').'/images/icons/16/led-icons/page_white_stack.png) 1px 2px no-repeat !important;
					padding:0 0 0 22px;
					}

				ul#'.$id.' li ul.children {
					margin:0 0 0 -1em;
					}

				';

		$style .= '</style>';


		$archives = wp_get_archives('echo=0&type=monthly&show_post_count=true');


	return '<ul id="'.$id.'" class="inline-block list">'.$archives.'</ul>'.$style;}

	add_shortcode('archives', 'archives');



?>