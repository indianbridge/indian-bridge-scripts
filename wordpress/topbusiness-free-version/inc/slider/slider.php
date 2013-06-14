<?php

// by PandaThemes.com



// *********************************************
//
// 	P A N D A   S L I D E R
//
// *********************************************



	// CHECK THE CURRENT PAGE

		// Homepage
		if ( is_front_page() && $panda_sl['slider_on_homepage'] == 'enable' ) : panda_slider(); endif;

		// Page
		if ( is_page() && ! is_page_template('archive-blog.php') && ! is_page_template('page-blank.php') && ! is_page_template('archive-products.php') && $panda_sl['slider_on_page'] == 'enable' ) : panda_slider(); endif;

		// Page Blank
		if ( is_page_template('page-blank.php') && ! is_front_page() && $panda_sl['slider_on_page'] == 'enable' ) : panda_slider(); endif;

		// Post
		if ( is_single() && ! is_singular(array( 'product', 'slider' )) && $panda_sl['slider_on_post'] == 'enable' ) : panda_slider(); endif;

		// Blog
		if ( is_page_template('archive-blog.php') && $panda_sl['slider_on_blog'] == 'enable' ) : panda_slider(); endif;

		// Archive
		if ( is_archive() && $panda_sl['slider_on_archives'] == 'enable' ) : panda_slider(); endif;

		// Product
		if ( is_singular('product') && $panda_sl['slider_on_product'] == 'enable' ) : panda_slider(); endif;

		// Product's archive
		if ( is_page_template('archive-products.php') && $panda_sl['slider_on_poducts_archives'] == 'enable' ) : panda_slider(); endif;

		// Search
		if ( is_search() && $panda_sl['slider_on_search'] == 'enable' ) : panda_slider(); endif;

		// 404
		if ( is_404() && $panda_sl['slider_on_404'] == 'enable' ) : panda_slider(); endif;




	// PANDA SLIDER
	function panda_slider(){

		global $panda_sl;

		$id = $panda_sl['slider'];
		
			// Get a slider
			$slider = new WP_Query(array(
				'post_type'			=> 'slider',
				'p'					=> $id
				)
			);
	
	
				if ($slider->have_posts()) :
			
					while ( $slider->have_posts() ) : $slider->the_post();
		
		
						// GET CUSTOM DATA
						$custom = get_post_custom($id);  
		
						// Kind of transitions
						$transition = ($custom['transition'][0]) ? $custom['transition'][0] : 'ptd';
		
						// Time delay
						$delay = ($custom['delay'][0]) ? $custom['delay'][0].'000' : '4000';
	
	
						$max_sum = $custom['specsum_ajax'][0] ? $custom['specsum_ajax'][0] + 1 : 1;
	
	
	
							$out = '
									<div class="wrapper960 br3top">
							
										<div id="homeslider" class="pslider ps-1 '.$transition.'" title="'.$delay.'">



											<div class="pnavbar">
												<div class="pnav">
													<div>
														<div>';


															// Display navbar
															if ($max_sum > 1) :

																for( $i=1; $i < $max_sum; $i++ ) {

																	${'s-title'.$i} = ( isset( $custom['s-title'.$i][0] ) ) ? $custom['s-title'.$i][0] : '';

																	$title = ( ${'s-title'.$i} ) ? ${'s-title'.$i} : $i;

																		if ( $i == 1 ) :

																			$out .= '<span class="pscurrent"><strong><!-- bullit --></strong>'.$title.'</span><em><!-- divider --></em>';

																		else :

																			$out .= '<span><strong><!-- bullit --></strong>'.$title.'</span><em><!-- divider --></em>';

																		endif;
				
																};

															endif;

															$out .= '
														</div>
													</div>
												</div>
											</div>



											<div class="ploading"><!-- --></div>
	
											<div class="pslides">';
	
	
	
												// Display slides
												if ($max_sum > 1) :
										
													for( $i=1; $i < $max_sum; $i++ ) {

														${'s-img'.$i} = ( isset( $custom['s-img'.$i][0] ) ) ? $custom['s-img'.$i][0] : '';

														${'s-height'.$i} = ( isset( $custom['s-height'.$i][0] ) ) ? $custom['s-height'.$i][0] : '';

														${'s-url'.$i} = ( isset( $custom['s-url'.$i][0] ) ) ? $custom['s-url'.$i][0] : '';

														${'s-target'.$i} = ( isset( $custom['s-target'.$i][0] ) ) ? $custom['s-target'.$i][0] : '';

	
														$target = ( ${'s-target'.$i} == 'blank' ) ? ' target="_blank"' : '';
	
														$out .= ( ${'s-url'.$i} )
	
															? '<div style="height:' . ${'s-height'.$i} . 'px;"><a href="' . ${'s-url'.$i} . '"' . $target . '><img src="' . ${'s-img'.$i} . '" height="' . ${'s-height'.$i} . '"></a><div class="clear"><!-- --></div></div>'
//															? '<div><a href="' . ${'s-url'.$i} . '"' . $target . '><img src="' . ${'s-img'.$i} . '"></a><div class="clear"><!-- --></div></div>'

															: '<div style="height:' . ${'s-height'.$i} . 'px;"><img src="' . ${'s-img'.$i} . '" height="' . ${'s-height'.$i} . '"><div class="clear"><!-- --></div></div>';
//															: '<div><img class="slide-img" src="' . ${'s-img'.$i} . '"><div class="clear"><!-- --></div></div>';

													};
										
												endif;
	
	
	
												$out .= '

											</div>';
	
	
	
											$out .= '

										</div>

									</div>

									<div id="sub-slider"><!-- --></div>';
	
	
	
					endwhile;

					wp_reset_postdata();

					echo $out;
			
				endif;

	};
	
?>