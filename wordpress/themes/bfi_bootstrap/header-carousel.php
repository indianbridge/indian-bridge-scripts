<?php
	$page_name = 'header';
	$section_name = 'carousel';
	$show_carousel = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'show' );
	if ( $show_carousel === 'all' || ( $show_carousel === 'front' && is_front_page() ) ) {
		$carousel_categories = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'categories' );
		$slide_count = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'slide-count' );
		if ( ! $carousel_categories ) {
			$carousel_categories[] = get_option('default_category');
		}
		$category_list = implode( ',', $carousel_categories );
		$args = array(
			'posts_per_page'   => $slide_count,
			'offset'           => 0,
			'category'         => $category_list,
			'orderby'          => 'post_date',
			'order'            => 'DESC',
			'include'          => '',
			'exclude'          => '',
			'meta_key'         => '',
			'meta_value'       => '',
			'post_type'        => 'post',
			'post_mime_type'   => '',
			'post_parent'      => '',
			'post_status'      => 'publish',
			'suppress_filters' => true 
		);
		$slide_posts = get_posts( $args );
		?>
			<div id="carousel-example-generic" class="carousel slide" data-ride="carousel">
	    		<ol class="carousel-indicators">
		<?php
		
		for ( $i = 0;$i < $slide_count; $i++) {
			if ( $i === 0 ) {
				?>
				<li data-target="#carousel-example-generic" data-slide-to="0" class="active"></li>
				<?php
			}
			else {
				?>
				<li data-target="#carousel-example-generic" data-slide-to="<?php echo $i; ?>"></li>
				<?php
				
			}
		}
		?>
	    		</ol>
	    	<div class="carousel-inner">
	    <?php		
	    $i = 0;
		foreach( $slide_posts as $post ) {
			setup_postdata( $post );
			if ( $i === 0 ) {
				?>
			        <div class="item active">			
				<?php
			}
			else {
				?>
			        <div class="item">			
				<?php		
			}
			bfi_bootstrap_show_slide();
			?> </div> <?php
			$i++;
		}
		?>
		</div>
	    <a class="left carousel-control" href="#carousel-example-generic" data-slide="prev">
	        <span class="glyphicon glyphicon-chevron-left"></span></a><a class="right carousel-control"
	            href="#carousel-example-generic" data-slide="next"><span class="glyphicon glyphicon-chevron-right">
	            </span></a>
	</div>
<?php } ?>