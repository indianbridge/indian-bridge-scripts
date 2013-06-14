<?php // by PandaThemes.com


// ***********************************
//
// 	I M A G E
//
// ***********************************


	function img( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'src'			=> '',
			'w'			=> '',
			'h'			=> '',
			'crop'		=> '',
			'f'			=> '',
			'title'		=> '',
			'alt'		=> '',
			'align'		=> ''
		), $atts));

		if ($src) :

			$out = '';
			$class = '';


			// Width
			if ( ! $w ) :

				$img_w = '100';
				$w = '&w=100';

			elseif ($w == 'auto') :

				$w = '';
				$img_w = '';

			else :

				$img_w = $w;
				$w = '&w='.$w;

			endif;


			// Height
			if ( ! $h ) :

				$h = '&h=100';

			elseif ($h == 'auto') :

				$h = '';

			else :

				$h = '&h='.$h;

			endif;


			// Crop
			$c = '';

			if ($crop == 'top') :

				$c = '&a=t';

			endif;


			// Filter
			if ($f) :

				$f = '&'.$f;

			endif;


			// Align
			if ($align == 'left') :

				$class .= 'img-fl';

			elseif ($align == 'right') :

				$class .= 'img-fr';

			elseif ($align == 'center') :

				$class .= 'img-ma';

			else :

				$class .= 'img-none';

			endif;


			// For multisite WordPress
			global $blog_id;
			if (isset($blog_id) && $blog_id > 1) {
				$imageParts = explode('/files/', $src);
				if (isset($imageParts[1])) {
					$path = '/blogs.dir/'.$blog_id.'/files/'.$imageParts[1];
				}
			}
			// For default WordPress
			else {
				$path = $src;
			}


			// Without title
			if ( ! $title ) :

				$out .= '<a rel="prettyPhoto" class="quick-img" title="'.$alt.'" href="'.$src.'"><img class="br3 '.$class.'" src="'.get_bloginfo('template_url').'/timthumb.php?src='.$path.$w.$h.$c.$f.'&zc=1&q=90" alt="" /></a>';

			// With title
			else :

				$out .= '<span class="quick-img '.$class.'" style="width:'.$img_w.'px;"><a rel="prettyPhoto" title="'.$alt.'" href="'.$src.'"><img class="br3" src="'.get_bloginfo('template_url').'/timthumb.php?src='.$path.$w.$h.$c.$f.'&zc=1&q=90" alt="'.$title.'" /></a>'.$title.'</span>';

			endif;


		endif;

	return $out; }

	add_shortcode('img', 'img');



// ***********************************
//
// 	H I D D E N   C O N T E N T 
//
// ***********************************



	function hidden( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'else' => 'default'
		), $atts));

		if ( is_user_logged_in() && ! is_null($content) && ! is_feed() ) :

			return $content;

		else :

			if ($else == 'default') :

				return '<div class="note-alert br3 mb1e block" style="background-image:url('.get_bloginfo('template_url').'/images/icons/16/led-icons/lock.png)"><div class="a">'.__('This part of the content is only available to members. Please login to view hidden text.','pandathemes').'</div></div>';

			elseif ($else == 'none') :

			else :

				return $else;

			endif;

		endif;

	}
	add_shortcode('hidden', 'hidden');




// ***********************************
//
// 	V I S I B L E   C O N T E N T 
//
// ***********************************



	function visible( $atts, $content = null ) {

		if ( ! is_user_logged_in() ) :

			return $content;

		endif;

	}
	add_shortcode('visible', 'visible');



// ***********************************
//
// 	F I E L D S E T
//
// ***********************************



	function fieldset( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'w'			=> 'auto',
			'h'			=> '',
			'm'			=> '',
			'align'		=> '',
			'text'		=> 'left',
			'legend'	=> '',
			'icon'		=> ''
		), $atts));


		$width = ($w) ? ' style="width:'.$w.';"' : '';

		$height = ($h) ? ' height:'.$h.';' : '';

		$margin = ($m) ? ' margin:0 '.$m.';' : '';


		// Align
		if ($align == 'left') :

			$a = ' inline-block mb1e fl';

		elseif ($align == 'right') :

			$a = ' inline-block mb1e fr';

		elseif ($align == 'center') :

			$a = ' block fc1e';

		else :

			$a = '';

		endif;


		// Text align
		$a .= ' ' . $text;


		// Fieldset

		$out = '<div class="relative '.$a.'"'.$width.'>';

		$out .= '<fieldset class="br3 block" style="'.$margin.$height.'">';


		// Legend
		
		$i = ($icon) ? ' style="padding:3px 5px 3px 25px; background:url('.get_bloginfo('template_url').'/images/icons/16/led-icons/'.$icon.') 5px 50% no-repeat;"' : '';
		
		$out .= ($legend) ? '<legend'.$i.'>'.$legend.'</legend><div class="clear"><!-- --></div>' : '';


		// Content

		$out .= do_shortcode($content);


		// End of fieldset

		$out .= '</fieldset>';

		$out .= '</div>';


	return $out; }

	add_shortcode('fieldset', 'fieldset');



// ***********************************
//
// 	P R I C I N G   B O X
//
// ***********************************



	function pbox( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'w'			=> 'auto',
			'h'			=> '',
			'm'			=> '',
			'bg'		=> '',
			'title'		=> '',
			'price'		=> '',
			'button'	=> '',
			'tagline'	=> '',
			'align'		=> '',
			'url'		=> '#'
		), $atts));

		$width = ($w) ? ' style="width:'.$w.';"' : '';

		$height = ($h) ? ' style="height:'.$h.'px;"' : '';

		$margin = ($m) ? ' style="margin:0 '.$m.'px;"' : '';

		if ($align == 'left') :

			$a = ' fl';

		elseif ($align == 'right') :

			$a = ' fr';

		elseif ($align == 'center') :

			$a = ' fc2e';

		else :

			$a = '';

		endif;

		$background = ($bg) ? ' style="background-color:#'.$bg.' !important;"' : '';

		$btn = ($button) ? $button : __('Purchase','pandathemes');

		$tline = ($tagline) ? $tagline : $title;


		// Out
		$out = '
			<div class="relative mb2e'.$a.'"'.$width.'>
				<div class="pbox"'.$margin.'>

				<!-- Price placeholder -->
				<div class="pa br3"'.$background.'>
					<div class="pb lh11">
						'.$title.'
					</div>
					<div class="pc lh10">
						'.$price.'
					</div>
				</div>

				<!-- Content placeholder -->
				<div class="pd br3"'.$height.'>
					<div class="pe">
						'.do_shortcode($content).'
					</div>
				</div>

				<!-- Button -->
				<div class="pf">
					<a target="_blank" rel="nofollow" href="'.$url.'" class="bbutton left">
						<span class="b"><span'.$background.'><!-- bull --></span></span>
						<span class="a relative">
							<div class="div-as-table">
								<div>
									<div>
										<span class="c f22 shdw-w-txt-100 lh10">'.$btn.'</span>
										<span class="block f11 gray shdw-w-txt-100 lh10">'.$tline.'</span>
									</div>
								</div>
							</div>
						</span>
					</a>
				</div>

			</div>
		</div>
		';


	return $out; }

	add_shortcode('pbox', 'pbox');



// ***********************************
//
// 	N O T E S
//
// ***********************************



	function note( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'type'		=> 'pin'
		), $atts));

		$out = '<div class="note-'.$type.' br3 note-padd mb1e"><div>'.do_shortcode($content).'</div></div>';

	return $out; }

	add_shortcode('note', 'note');



// ***********************************
//
// 	Q U O T E S
//
// ***********************************



	function quote( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'w'			=> '',
			'm'			=> '',
			'bg'		=> '',
			'color'		=> '',
			'align'		=> '',
			'cite'		=> 'John Doe',
			'subcite'	=> 'CEO, Company Name',
			'pic'		=> '1',
			'src'		=> ''
		), $atts));

		$width = ($w) ? ' style="width:'.$w.';"' : '';

		$margin = ($m) ? ' style="margin:0 '.$m.';"' : '';

		$background = '';

		if ($bg || $color) :

			$background = ' style="background:#'.$bg.'; color:#'.$color.';"';

		endif;

		$triangle = ($bg) ? ' style="border-color:transparent #'.$bg.';"' : '';

		if ($align == 'left') :

			$a = ' fl';

		elseif ($align == 'right') :

			$a = ' fr';

		elseif ($align == 'center') :

			$a = ' fc1e';

		else :

			$a = '';

		endif;


	if ($src) :

		// For multisite WordPress
		global $blog_id;
		if (isset($blog_id) && $blog_id > 1) {
			$imageParts = explode('/files/', $src);
			if (isset($imageParts[1])) {
				$path = '/blogs.dir/'.$blog_id.'/files/'.$imageParts[1];
			}
		}
		// For default WordPress
		else {
			$path = $src;
		}

		$image = get_bloginfo('template_url').'/timthumb.php?src='.$path.'&w=35&h=40&zc=1&q=90';

	elseif ($pic) :

		$image = get_bloginfo('template_url').'/images/userpic/'.$pic.'.png';

	else :

		$image = get_bloginfo('template_url').'/images/userpic/1.png';

	endif;


	$out = '
		<div class="block mb1e quote relative'.$a.'"'.$width.'>

			<div'.$margin.'>

				<!-- Quote -->	
				<div class="qa"'.$background.'>'.do_shortcode($content).'</div>

				<div class="qe relative">

					<table class="qt">
						<tr>
							<td class="qd f11 lh13">
	
								<img class="qc" src="'.$image.'" alt=""/>
	
								<strong>'.$cite.'</strong><br/>
								'.$subcite.'
	
							</td>
						</tr>
					</table>
	
					<!-- Triangle -->
					<div class="qb fr inline-block"'.$triangle.'></div>

					<div class="clear"><!-- --></div>

				</div>

				<div class="clear h30"><!-- --></div>

			</div>

		</div>
	';

	return $out; }

	add_shortcode('quote', 'quote');



// ***********************************
//
// 	D R O P   C A P S
//
// ***********************************



	function dropcap( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'bg'		=> '',
			'color' 	=> '333',
			'size' 		=> '36'
		), $atts));

		if ($bg) :

			$out = '<div class="dropcap2" style="background:#'.$bg.'; color:#'.$color.';"><div><div style="font-size:'.$size.'px; width:'.$size.'px; height:'.$size.'px; padding:0.15em 0.2em 0.2em; line-height:'.$size.'px;">'.do_shortcode($content).'</div></div></div>';

		else :

			$out = '<span class="dropcap" style="font-size:'.$size.'px; height:'.$size.'px; line-height:'.$size.'px; margin:0 5px 0 -0.08em;">'.do_shortcode($content).'</span>';

		endif;

	return $out; }

	add_shortcode('dropcap', 'dropcap');



// ***********************************
//
// 	M A R K E R
//
// ***********************************



	function marker( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'color'		=> ''
		), $atts));

		$background = ($color) ? ' style="background-color:'.$color.';"' : '';

		$out = '<span class="marker"'.$background.'>'.do_shortcode($content).'</span>';

	return $out; }

	add_shortcode('marker', 'marker');



// ***********************************
//
// 	C L E A R
//
// ***********************************



	function clear( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'h'		=> ''
		), $atts));

		if ($h) : $out = '<div class="clear" style="height:'.$h.'px !important;"><!-- --></div>';

		else : $out = '<div class="clear"><!-- --></div>';

		endif;

	return $out; }

	add_shortcode('clear', 'clear');



// ***********************************
//
// 	L I N E
//
// ***********************************



	function line( $atts, $content = null ) {

	return '<div class="line mb1e"><!-- --></div>'; }

	add_shortcode('line', 'line');



// ***********************************
//
// 	D I V I D E R
//
// ***********************************



	function divider( $atts, $content = null ) {

	return '<div class="divider"><!-- --></div>'; }

	add_shortcode('divider', 'divider');



// ***********************************
//
// 	T W E E T
//
// ***********************************



	function tweet( $atts, $content = null ) {

	return '<a href="http://twitter.com/share" class="twitter-share-button" data-count="horizontal">Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>'; }

	add_shortcode('tweet', 'tweet');



// ***********************************
//
// 	L I K E
//
// ***********************************



	function like( $atts, $content = null ) {

	return '<script src="http://connect.facebook.net/en_US/all.js#xfbml=1"></script><fb:like layout="button_count" show_faces="false" width="100"></fb:like>'; }

	add_shortcode('like', 'like');



// ***********************************
//
// 	S I D E B A R
//
// ***********************************



	function sidebar( $atts ) {

		extract (shortcode_atts(array(
			'w'			=> '',
			'm'			=> '',
			'align'		=> '',
			'name'		=> ''
		), $atts));

		$width = ($w) ? ' style="width:'.$w.';"' : '';

		$margin = ($m) ? ' style="margin:0 '.$m.'px;"' : '';

			if ($align == 'left') :

				$a = ' fl';

			elseif ($align == 'right') :

				$a = ' fr';

			elseif ($align == 'center') :

				$a = ' fc1e';

			else :

				$a = '';

			endif;


		ob_start();

			dynamic_sidebar ($name);

			$sidebar = ob_get_contents();

		ob_end_clean();


	return '<div class="sidebar'.$a.'"'.$width.'><div'.$margin.'>'.$sidebar.'</div></div>';}

	add_shortcode ('sidebar', 'sidebar');



// ***********************************
//
// 	T O G G L E
//
// ***********************************



	function toggle( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'title'		=> 'Toggle',
			'status'	=> '',
			'style'		=> '1'
		), $atts));

		$open = ($status == 'open') ? ' toggle-auto' : '';


		$out = '

			<div class="toggle tgl-' . $style . $open . '">

				<div class="t"><div><!-- --></div></div>

				<div class="ta">' . $title . '</div>

				<div class="tb">

					<div class="tc">

						'.do_shortcode($content).'

					</div>

				</div>

			</div>

		';


	return $out; }

	add_shortcode('toggle', 'toggle');



// ***********************************
//
// 	A C C O R D I O N
//
// ***********************************



	function accordion( $atts, $content = null ) {

		$out = '<div class="accordion">'.do_shortcode($content).'</div>';

	return $out; }

	add_shortcode('accordion', 'accordion');



// ***********************************
//
// 	T A B S
//
// ***********************************



	// KUL
	function tabs( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'names'		=> ''
		), $atts));

		if ($names) :


			// Tabs
			$out = '<ul class="kul kul-auto">';

			$arr = explode(",", $names);

				foreach ($arr as $value) :

					if ($value && $value != ' ') :

						$out .= '<li>'.$value.'</li>';

					endif;

				endforeach;

			$out .= '</ul>';


			// Content
			$out .= '<div class="ktabs ktabs-auto">'.do_shortcode($content).'</div>';


		else :

			$out = '<em>'.__("The tabs do not selected. Use the 'names' attribute.",'pandathemes').'</em>';

		endif;


	return $out; }

	add_shortcode('tabs', 'tabs');



	// KTABS
	function t( $atts, $content = null ) {

   	return '<div>'.do_shortcode($content).'</div>'; }

	add_shortcode('t', 't');



?>