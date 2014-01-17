<?php 

	// M E T A   T A B S

	$tabs = '<ul class="kul">';
	
		// DESCRIPTION
		$tabs .= $panda_pr['tab_desc'] ? '<li class="kcurrent">'.$panda_pr['tab_desc'].'</li>' : '<li class="kcurrent">'.__('Description','pandathemes').'</li>';
		
		// SPECS
		$tabs .= $panda_pr['tab_spec'] ? '<li id="specs-data-tab" class="none">'.$panda_pr['tab_spec'].'</li>' : '<li id="specs-data-tab" class="none">'.__('Specs','pandathemes').'</li>';
		
		// IMAGES
		$tabs .= $panda_pr['tab_imgs'] ? '<li id="images-data-tab" class="none">'.$panda_pr['tab_imgs'].'</li>' : '<li id="images-data-tab" class="none">'.__('Images','pandathemes').'</li>';
	
		// CUSTOM TAB
		$tabs .= $custom_tab_title ? '<li id="custom-data-tab">'.$custom_tab_title.'</li>' : '' ;
	
		// REVIEWS
		if ($panda_pr['reviews'] == 'yes') : $tabs .= $panda_pr['tab_rews'] ? '<li id="tab-reviews">'.$panda_pr['tab_rews'].'</li>' : '<li id="tab-reviews">'.__('Reviews','pandathemes').'</li>'; endif;

	$tabs .= '</ul>';

	echo $tabs;

?>
			
<div class="ktabs">

	<!--------------------- D E S C R I P T I O N --------------------->

	<div class="block p10">
		<?php
			// DESCRIPTION
			the_content();
		?>
	</div>

	<!--------------------- S P E C S --------------------->

	<div id="specs-data" class="p10"><?php
		// SPECS
		$max_sum = $custom['specsum_ajax'][0] ? $custom['specsum_ajax'][0] + 1 : 1;
		if ($max_sum > 1) :
			echo '<table class="meta">';
			for( $i=1; $i<$max_sum; $i++ ) {

				${'s-text'.$i} = $custom['s-text'.$i][0];
				${'s-value'.$i} = $custom['s-value'.$i][0];

				$out = '<tr>';
					$out .= '<td class="right strong w25p">';
						$out .= ${'s-text'.$i};
					$out .= '</td>';
					$out .= '<td>';
						$out .= ${'s-value'.$i};
					$out .= '</td>';
				$out .= '</tr>';

				echo $out;
			};
			echo '</table>';
		endif;
	?></div>

	<!--------------------- I M A G E S --------------------->

	<div id="images-data"><?php
		// IMAGES
		$max_sum = $custom['postsum_ajax'][0] ? $custom['postsum_ajax'][0] : 1;
		if ($max_sum > 2) :
			$counter = 0;
			for( $i=2; $i<$max_sum; $i++ ) {

				$counter++;
				$class = ($counter == 3) ? 'pr-img-last' : 'pr-img';

				$src = $custom['i'.$i][0];

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
				// Display image
				echo '<a rel="prettyPhoto[gallery]" class="inline-block relative product-image '.$class.'" href="'.$src.'"><div><!-- hover --></div><img class="br3" src="'.$path.'" width="172" height="172" alt="'.get_the_title().'" /></a>';

			};
			echo '<div class="clear"><!-- --></div>';
		endif;
	?></div>


	<!--------------------- C U S T O M   T A B --------------------->

	<?php
		if ($custom_tab_title) : echo '<div id="prd-custom-tab" class="p10">'.do_shortcode($custom_tab_content).'<div class="clear h10"><!-- --></div></div>'; endif;
	?>

	<!--------------------- R E V I E W S --------------------->

	<div class="relative">
		<?php
			if ($panda_pr['reviews'] == 'yes') : comments_template(); endif;
		?>
	</div>

</div>