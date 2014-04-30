<?php

// Show/Hide FeaturedImage/Excerpt/Content	
function _bootstrap_show_post_content( $show_featured_image, $show_excerpt ) {
	if ( ! $show_featured_image ) {
		$show_excerpt ? the_excerpt() : the_content();
	}
	else {
		_bootstrap_show_feature_image_and_content( $show_excerpt );
	}		
}

/**
* If featured image is enabled then show the content and featured image based on chosen 
* options for featured image
* 
* @param boolean $show_excerpt should the exceprt be shown instead of content.
* 
* @return nothing
*/
function _bootstrap_show_feature_image_and_content( $show_excerpt ) {
	$width = 150;
	$height = 150;
	$fi_size = array(
		array( $width, $height ),
		$width,
		$height,
	);	
	$image_attributes = wp_get_attachment_image_src( get_post_thumbnail_id(), $fi_size[0] );
	$image_url = $image_attributes[0];
	if ( ! $image_url ) {
		$image_url = 'http://placehold.it/' . $fi_size[1] . 'x' . $fi_size[2] . '&text=No+Featured+Image';
	}
		$img_string = '<img width="' . $fi_size[1] . 'px" height = "' . $fi_size[2] . 'px" src="' . $image_url . '" alt="Feature Image" class="img-responsive media-object img-rounded pull-left"></img>';
	?>
		<div class="media">
			<?php echo $img_string; ?>
			<div class="media-body">					
				<?php $show_excerpt ? the_excerpt() : the_content(); ?>
			</div>
		</div>
	<?php
}

function bfi_bootstrap_show_slide() {
	$width = 300;
	$height = 300;
	$fi_size = array(
		array( $width, $height ),
		$width,
		$height,
	);	
	$image_attributes = wp_get_attachment_image_src( get_post_thumbnail_id(), $fi_size[0] );
	$image_url = $image_attributes[0];
	if ( ! $image_url ) {
		$image_url = 'http://placehold.it/' . $fi_size[1] . 'x' . $fi_size[2] . '&text=No+Featured+Image';
	}
	?>
        <img class="center-block" src="<?php echo $image_url; ?>" alt="<?php echo the_title(); ?>">
        <div class="carousel-caption">
            <h3><?php the_title(); ?></h3>
        </div>	
	<?php
}
 
/**
 * Displays all meta information about current post.
 * Which information to display and what order to display is determined by options.
 * 
 * @return void
  */		
function _bootstrap_display_meta_information() {
		$page_name = 'content';
		$section_name = 'meta';
		$enabled_metas = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'items', 'enabled' );
		$style = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'container_style' );
		$class = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'container_class' );	
		
		foreach ( $enabled_metas as $meta => $value ) {
			switch ( $meta ) {	
				case 'publish_date' : 
					_bootstrap_get_published_date( $style, $class );
					break;
				case 'last_updated_date' : 
					_bootstrap_get_updated_date( $style, $class );
					break;
				case 'author' : 
					_bootstrap_posted_by( $style, $class );
					break;
				case 'categories' : 
					_bootstrap_post_categories( $style, $class );
					break;
				case 'tags' : 
					_bootstrap_post_tags( $style, $class );
					break;
				case 'comments' : 
					_bootstrap_post_comments( $style, $class );
					break;
				default :
					break;
			}
		}	
}
	
function _bootstrap_get_published_date( $style, $class ) {
	_bootstrap_get_post_date_internal(  $style, $class, FALSE );
}

function _bootstrap_get_updated_date( $style, $class ) {
	_bootstrap_get_post_date_internal(  $style, $class, TRUE );
}

function _bootstrap_get_post_date_internal(  $style, $class, $get_last_update_date=FALSE ) {
	
	// Get the post dates for this post
	$date_time = $get_last_update_date ? get_the_modified_date( 'c' ) : get_the_date( 'c' );
	$display_date = $get_last_update_date ? get_the_modified_date() : get_the_date();
	$year = $get_last_update_date ? get_the_modified_date( 'Y' ) : get_the_date( 'Y' );
	$month = $get_last_update_date ? get_the_modified_date( 'm' ) : get_the_date( 'm' );
 	$information = array(
 		'icon-title' => $get_last_update_date ? 
 				   __( 'Last Updated On', 'bfi_bootstrap' ) :
 				   __( 'Published On', 'bfi_bootstrap' ),
 		'icon'	=> $get_last_update_date ? 'refresh' : 'calendar', // Icon names so dont internationalized
 		'title'	=> __( 'View the Archived for this Month', 'bfi_bootstrap' ),
 		'url'	=> get_month_link( $year, $month),
 		'text'	=> $display_date,
 	);
 	echo '<time datetime="' . esc_attr( $date_time ) . '">'; // HTML tag should not be internationalized
 	_bootstrap_post_information(  $style, $class, $information );	
 	echo '</time>'; // HTML tag should not be internationalized
}



//Prints HTML with meta information for the current post author.
 function _bootstrap_posted_by( $style, $class ) {
 	$author_name = get_the_author();
 	$information = array(
 		'icon-title' => __( 'Authored By', 'bfi_bootstrap'),
 		'icon'	=> 'user', // Icon names so dont internationalized
 		'url'	=> get_author_posts_url( get_the_author_meta( 'ID' ) ),
 		'title'	=> sprintf( __( 'View Post by %s', 'bfi_bootstrap' ), esc_attr( $author_name )),
 		'text'	=> $author_name,
 	);
 	_bootstrap_post_information(  $style, $class, $information  );
}


//Prints HTML with meta information for the current post categories.
 function _bootstrap_post_categories( $style, $class ) {
 	$categories_list = _bootstrap_get_categories( ' ', $style, $class );
	if ( $categories_list ) {
	 	$information = array(
	 		'icon-title' => __( 'Post Categories', 'bfi_bootstrap'),
	 		'icon'	=> 'folder', // Icon names so dont internationalized
	 		'links'	=> $categories_list,
	 	);
	 	_bootstrap_post_information(  $style, $class, $information  );
 	}
}

//Prints HTML with meta information for the current post tags.
 function _bootstrap_post_tags( $style, $class ) {
 	$tags_list = _bootstrap_get_tags( ' ', $style, $class );
	if ( $tags_list ) {
	 	$information = array(
	 		'icon-title' => __( 'Post Tags', 'bfi_bootstrap'),
	 		'icon'	=> 'tags', // Icon names so dont internationalized
	 		'links'	=> $tags_list,
	 	);
	 	_bootstrap_post_information(  $style, $class, $information  );
 	}
}

//Prints HTML with meta information for the current post comments.
 function _bootstrap_post_comments( $style, $class ) {
 	if ( comments_open() ) {
	 	$num_comments = get_comments_number(); // get_comments_number returns only a numeric value
	 	if ( $num_comments == 0 ) {
	 		$comments = __( 'Leave a Comment', 'bfi_bootstrap' );
	 	} 
	 	elseif ( $num_comments > 1 ) {
	 		$comments = $num_comments . __( ' Comments', 'bfi_bootstrap' );
	 	}
	 	else {
	 		$comments = __( '1 Comment', 'bfi_bootstrap' );
	 	}	 	
	 	$information = array(
	 		'icon-title' 	=> __( 'Comments', 'bfi_bootstrap'),
	 		'icon'			=> 'comments', // Icon names so dont internationalized
	 		'url'			=> get_comments_link(),
	 		'title'			=> __( 'View Comments on this Post', 'bfi_bootstrap' ),
	 		'text'			=> $comments,
	 	);
	 	_bootstrap_post_information(  $style, $class, $information  );
 	}
}

/**
 * Prints HTML with meta information for the current post date, author, categories or tags.
 * This is an internal function used by each individual meta information display function above.
 */
 function _bootstrap_post_information(  $style, $class, $information ) {
 	// All information is already internationalized so just print
	echo _bootstrap_get_font_awesome_icon( $information['icon'], $information['icon-title'] ) . ' ';
	if ( array_key_exists( 'links', $information ) ) {			
		echo $information['links'];						
	}
	else {
		echo '<a href="' . esc_url( $information['url'] ) . '" title="' . $information['title'] . '" ';
		if ( $style  === 'button' ) {
			echo 'class="btn btn-xs btn-' . $class . '" ';
		}
		if ( $style === 'plain' ) {
			echo 'class="text-' . $class . '" ';
		} 					
		echo '>';
		if ( $style === 'label' ) {
			echo '<span class="label label-' . $class . '">';
		}
		echo esc_html( $information['text'] );
		if ( $style === 'label' ) {
			echo '</span>';
		}	
		echo '</a>';			
	}
	echo ' </span>';
}

/**
 * Gets a list of categories styled as plain text, bootstrap label or bootstrap button
 * This is used in lieu of get_the_category_list because the span for label and class for button
 * has to inside the a href for the text to be displayed correctly
 * 
 * @param $separator the separator between the list items
 * @param $style how to style each element of list (plain, lable or button)
 * @param $class an optional string to assign a class to label or button
 * @return a string of delimited categories with appropriate styling
 */	
function _bootstrap_get_categories( $separator, $style, $class='none' ) {
	$categories = get_the_category();
	$output = '';
	if( $categories ) {
		foreach($categories as $category) {
			$output .= '<a href="' . get_category_link( $category->term_id ) . '" ';
			$output .= 'title="' . esc_attr( sprintf( __( 'View all posts in %s', 'bfi_bootstrap' ), $category->name ) ) . '" ';
			if ( $style === 'button' ) {
				$output .= 'class="btn btn-xs btn-' . $class . '" ';
			}
			if ( $style === 'plain' ) {
				$output .= 'class="text-' . $class . '" ';
			} 					
			$output .= '>';
			// Add the label class if requested
			if ( $style === 'label' ) {
				$output .= '<span class="label label-' . $class . '">';
				$output .= esc_attr( $category->cat_name );
				$output .= '</span>';
			}
			else {
				$output .= esc_attr( $category->cat_name );
			}
			$output .= '</a>' . $separator;
		}
		return trim( $output, $separator );
	}
	return '';		
}

/**
 * Gets a list of tags styled as plain text, bootstrap label or bootstrap button
 * This is used in lieu of get_the_category_list because the span for label and class for button
 * has to inside the a href for the text to be displayed correctly
 * 
 * @param $separator the separator between the list items
 * @param $style how to style each element of list (plain, lable or button)
 * @param $class an optional string to assign a class to label or button
 * @return a string of delimited tag list with appropriate styling
 */	
function _bootstrap_get_tags( $separator, $style, $class='none' ) {
	$tags = get_the_tags();
	$output = '';
	if( $tags ) {
		foreach($tags as $tag) {
			$output .= '<a href="' . get_tag_link( $tag->term_id ) . '" ';
			$output .= 'title="' . esc_attr( sprintf( __( 'View all posts tagged %s', 'bfi_bootstrap' ), $tag->name ) ) . '" ';
			if ( $style === 'button' ) {
				$output .= 'class="btn btn-xs btn-' . $class . '" ';
			}
			if ( $style === 'plain' ) {
				$output .= 'class="text-' . $class . '" ';
			}				
			$output .= '>';
			// Add the label class if requested
			if ( $style === 'label' ) {
				$output .= '<span class="label label-' . $class . '">';
				$output .= esc_attr( $tag->name );
				$output .= '</span>';
			}
			else {
				$output .= esc_attr( $tag->name );
			}
			$output .= '</a>' . $separator;
		}
		return trim( $output, $separator );
	}
	return '';		
}

/**
 * Displays a title and term description for an archive page (like category, tag, author etc.)
 * 
 * @param nothing
 * @return nothing
 */	
function _bootstrap_echo_archives_page_header() {		
	?>
	<div class="page-header">
		<h1>
		<?php
			remove_filter('term_description','wpautop');
			$term_description = term_description();
			// Change diaplay title depending on what kind of page.
			if ( is_category() ) :
				_e( 'Posts in category ', 'bfi_bootstrap' );
				echo single_cat_title();
				if ( ! empty($term_description) ) : 
					echo ' <small>(' . $term_description . ')</small>';
				endif;
				
			elseif ( is_tag() ) :
				_e( 'Posts tagged ', 'bfi_bootstrap' );
				echo single_tag_title();
				if ( ! empty($term_description) ) : 
					echo ' <small>(' . $term_description . ')</small>';
				endif;

			elseif ( is_author() ) :
				printf( __( 'Posts by %s %s', 'bfi_bootstrap' ), '<span class="vcard">' . get_the_author() . '</span>', get_avatar(get_the_author_meta('ID'),'60') );

			elseif ( is_day() ) :
				printf( __( 'Day: %s', 'bfi_bootstrap' ), '<span>' . get_the_date() . '</span>' );

			elseif ( is_month() ) :
				printf( __( 'Month: %s', 'bfi_bootstrap' ), '<span>' . get_the_date( _x( 'F Y', 'monthly archives date format', 'bfi_bootstrap' ) ) . '</span>' );

			elseif ( is_year() ) :
				printf( __( 'Year: %s', 'bfi_bootstrap' ), '<span>' . get_the_date( _x( 'Y', 'yearly archives date format', 'bfi_bootstrap' ) ) . '</span>' );

			else :
				_e( 'Archives ', 'bfi_bootstrap' );
				if ( ! empty($term_description) ) : 
					echo ' <small>(' . $term_description . ')</small>';
				endif;				
				printf ( '%s <small>%s</small>', _e( 'Archives', 'bfi_bootstrap' ), $term_description );

			endif;			
		?>
		 </h1>
	</div>
	<?php
	
}

function _bootstrap_get_font_awesome_icon( $icon_name, $icon_title, $class = '' ) {
	 return '<i title="' . $icon_title . '" class="fa fa-' . $icon_name . ' ' . $class . '"></i>';
}

/**
* Change the Read More text at end of excerpt to custom text and html.
*
* @param string $more the current read more text
* 
* @return the new read more html
*/
function _bootstrap_new_read_more( $more ) {
	// Get the user specified options
	$text  = 'Read More';
	$style = 'label';
	$class = 'primary';
	switch ( $style ) {
		case 'label' :
			return ' <a href="'. get_permalink( get_the_ID() ) . '"><span class="label label-' .
			$class . '">' . $text . '</span></a>';
			break;
		case 'button' :
			return ' <a class="btn btn-xs btn-' . $class . '" href="'. get_permalink( get_the_ID() ) . '">' . $text . '</a>';
			break;
		case 'plain' :
		default :
			return ' <a class="text-' . $class . '" href="'. get_permalink( get_the_ID() ) . '">' . $text . '</a>';
			break;
	}
}
add_filter( 'excerpt_more', '_bootstrap_new_read_more' );


