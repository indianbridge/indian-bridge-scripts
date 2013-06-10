<?php // by PandaThemes.com


// ***********************************
//
// 	S L I D E R
//
// ***********************************



	function slider( $atts, $content = null ) {

		global $height;

		extract(shortcode_atts(array(
		'width'			=> '',
		'height'		=> '300',
		'autoplay'		=> '',
		'tabs'			=> 'auto',
		'float'			=> '',
		'transition'	=> 'ptd'
		), $atts));


		// Float
		if ($float == 'left') : $float = ' fl';

		elseif ($float == 'right') : $float = ' fr';

		endif;

		// Style
		$style = ' style="';

			// Width
			$style .= $width ? 'width:' . $width . 'px;' : 'width:100%;';

			// Margin
			if ( $float == 'right' ) :

				$style .= 'margin-left:20px';

			elseif ( $float == 'left' ) :

				$style .= 'margin-right:20px';

			endif;

		$style .= '"';

		// Autoplay
		$autoplay = $autoplay ? ' title="'.$autoplay.'000"' : '';

		// Tabs
		if ( $tabs != 'auto' ) :

			// explode names of tabs // make an array
			$arr = explode(",", $tabs);

			$tabs = '';

			$count = 0;

			foreach ($arr as $value) :

				$count++;

				$tabs .= ( $count == 1 ) ? '<span class="pscurrent">' . $value . '</span>' : '<em><!-- divider --></em><span>' . $value . '</span>';

			endforeach;

		else : $tabs = '';

		endif;


		$out = '
			<div class="pslider ps-2 ' . $transition . $float . '"' . $style . $autoplay . '>
			
				<div class="ploading"><!-- --></div>
			
				<div class="pslides">

					'.do_shortcode($content).'

				</div>
			
				<div class="pnavbar">
					<div class="pnav">
						<div>
							<div>' . $tabs . '</div>
						</div>
					</div>
				</div>
			
			</div>
		';


		return $out;

	}
	add_shortcode('slider', 'slider');



// ***********************************
//
// 	S L I D E S
//
// ***********************************



	function s( $atts, $content = null ) {

		global $height;

		extract(shortcode_atts(array(
		'h'	=> ''
		), $atts));

		$h = $h ? $h : $height;


		$out = '
			<div style="height:' . $h . 'px;">

				'.do_shortcode($content).'

				<div class="clear"><!-- --></div>
			</div>
		';


		return $out;

	}
	add_shortcode('s', 's');



?>