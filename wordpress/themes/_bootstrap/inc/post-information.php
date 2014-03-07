<?php
/**
 * Retrieve and display information and meta information about posts
 *
 * @package _bootstrap
 */
 
if ( ! function_exists( '_bootstrap_show_post_content' ) ) {
	/**
	 * Displays the post content for the current post.
	 * 
	 * @return void
	  */		
	function _bootstrap_show_post_content( ) {
		$page_name = 'archives';
		$section_name = 'excerpt';
		$show_excerpt = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'use_excerpt') );	
		$section_name = 'featured_image';	
		$show_featured_image = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'show' ) );
		if ( $show_featured_image ) {
		}
		if ( ! $show_featured_image ) {
			$show_excerpt ? the_excerpt() : the_content();
		}
		else {
			_bootstrap_show_feature_image_and_content( $show_excerpt );
		}		
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
	$page_name = 'archives';
	$section_name = 'featured_image';		
	$fi_location = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'location' ) );
	$fi_class = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'class' ) );
	$fi_thumbnail_size = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'size' ) );
	if ( $fi_thumbnail_size === 'custom' ) {
		$fi_container_dimensions = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'custom_dimensions' ) );
		$width = $fi_container_dimensions['width'];
		$height = $fi_container_dimensions['height'];
		$fi_size = array(
			array( $width, $height ),
			$width,
			$height,
		);
	}
	else {
		$fi_size = explode( '~', $fi_thumbnail_size );
	}
	$image_attributes = wp_get_attachment_image_src( get_post_thumbnail_id(), $fi_size[0] );
	$image_url = $image_attributes[0];
	if ( ! $image_url ) {
		$place_holder = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'place_holder' ) );
		if ( $place_holder ) {
			$image_url = str_replace( '$width', $fi_size[1], $place_holder );
			$image_url = str_replace( '$height', $fi_size[2], $image_url );
			//$image_url = 'http://placehold.it/' . $fi_size[1] . 'x' . $fi_size[2] . '&text=No+Featured+Image';
		}
	}
	if ( $image_url ) {
		$img_string = '<img width="' . $fi_size[1] . 'px" height = "' . $fi_size[2] . 'px" src="' . $image_url . '" alt="Feature Image" class="img-responsive media-object ' .  $fi_class . ' ';
		if ( $fi_location === 'above-center' ) {
			$img_string .= 'center-block ';
		}
		else if ( $fi_location === 'left' || $fi_location === 'above-left' ) {
			$img_string .= 'pull-left ';
		}
		else if ( $fi_location === 'right' || $fi_location === 'above-right' ) {
			$img_string .= 'pull-right ';
		}
		$img_string .= '">';	
	?>
		<div class="media">
			<?php 
				echo $img_string;
				if ( $fi_location === 'above-left' || $fi_location === 'above-center' || $fi_location === 'above-right' ) {
			?>
		</div>
		<div class="media">
			<?php } ?>
			<div class="media-body">					
				<?php $show_excerpt ? the_excerpt() : the_content(); ?>
			</div>
		</div>
	<?php
	} // ( $image_url ) 
	else {
		?>
		<div class="media">
			<div class="media-body">					
				<?php $show_excerpt ? the_excerpt() : the_content(); ?>
			</div>
		</div>		
		<?php
	}
}
 
if ( ! function_exists( '_bootstrap_display_meta_information' ) ) {
	/**
	 * Displays all meta information about current post.
	 * Which information to display and what order to display is determined by options.
	 * 
	 * @return void
	  */		
	function _bootstrap_display_meta_information() {
		$page_name = 'archives';
		$section_name = 'meta';
		$enabled_metas = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'items' ), 'enabled' );
		foreach ( $enabled_metas as $meta => $value ) {
			switch ( $meta ) {	
				case 'publish_date' : 
					_bootstrap_get_published_date( );
					break;
				case 'last_updated_date' : 
					_bootstrap_get_updated_date( );
					break;
				case 'author' : 
					_bootstrap_posted_by();
					break;
				case 'categories' : 
					_bootstrap_post_categories();
					break;
				case 'tags' : 
					_bootstrap_post_tags();
					break;
				case 'comments' : 
					_bootstrap_post_comments();
					break;
				default :
					break;
			}
		}
	}
}
 
if ( ! function_exists( '_bootstrap_get_published_date' ) ) {
	/**
	 * Prints HTML with meta information for the current post published date.
	 * 
	 * @return void
	  */	
	function _bootstrap_get_published_date( ) {
		_bootstrap_get_post_date_internal( FALSE );
	}
}

if ( ! function_exists( '_bootstrap_get_updated_date' ) ) {
	/**
	 * Prints HTML with meta information for the current post last updated date.
	 * 
	 * @return void
	  */	
	function _bootstrap_get_updated_date( ) {
		_bootstrap_get_post_date_internal( TRUE );
	}
}

if ( ! function_exists( '_bootstrap_get_post_date_internal' ) ) {
	/**
	 * Prints HTML with meta information for the current post published or updated time.
	 * The passed parameter determined which value is printed.
	 * This is an internal function used by _bootstrap_get_published_date and _bootstrap_get_updated_date.
	 * 
	 * @param boolean $get_last_update_date should the last updated date be printed.
	 * @return void
	  */	
	function _bootstrap_get_post_date_internal( $get_last_update_date=FALSE ) {
		
		// Get the post dates for this post
		$date_time = $get_last_update_date?get_the_date( 'c' ):get_the_modified_date( 'c' );
		$display_date = $get_last_update_date?get_the_date():get_the_modified_date();
	 	$information = array(
	 		'icon-title' => $get_last_update_date ? 
	 				   __( 'Last Updated On', '_bootstrap' ) :
	 				   __( 'Published On', '_bootstrap' ),
	 		'icon'	=> $get_last_update_date ? 'refresh' : 'calendar', // Icon names so dont internationalized
	 		'title'	=> __( 'View the Post', '_bootstrap' ),
	 		'url'	=> get_permalink(),
	 		'text'	=> $display_date,
	 	);
	 	echo '<time datetime="' . esc_attr( $date_time ) . '">'; // HTML tag should not be internationalized
	 	_bootstrap_post_information( $information );	
	 	echo '</time>'; // HTML tag should not be internationalized
	}
}

if ( ! function_exists( '_bootstrap_posted_by' ) ) {
	
	/**
	 * Prints HTML with meta information for the current post author.
	 * 
	 * @return void
	 */
	 function _bootstrap_posted_by() {
	 	$author_name = get_the_author();
	 	$information = array(
	 		'icon-title' => __( 'Authored By', '_bootstrap'),
	 		'icon'	=> 'user', // Icon names so dont internationalized
	 		'url'	=> get_author_posts_url( get_the_author_meta( 'ID' ) ),
	 		'title'	=> sprintf( __( 'View Post by %s', '_bootstrap' ), esc_attr( $author_name )),
	 		'text'	=> $author_name,
	 	);
	 	_bootstrap_post_information( $information  );
	}
}

if ( ! function_exists( '_bootstrap_post_categories' ) ) {
	
	/**
	 * Prints HTML with meta information for the current post categories.
	 * 
	 * @return void
	 */
	 function _bootstrap_post_categories() {
	 	$page_name = 'archives';
		$section_name = 'meta';
		$style = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ) );
		$class = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'container_class' ) );	
	 	$categories_list = _bootstrap_get_categories( ' ', $style, $class );
		if ( $categories_list ) {
		 	$information = array(
		 		'icon-title' => __( 'Post Categories', '_bootstrap'),
		 		'icon'	=> 'folder', // Icon names so dont internationalized
		 		'links'	=> $categories_list,
		 	);
		 	_bootstrap_post_information( $information  );
	 	}
	}
}

if ( ! function_exists( '_bootstrap_post_tags' ) ) {
	
	/**
	 * Prints HTML with meta information for the current post tags.
	 * 
	 * @return void
	 */
	 function _bootstrap_post_tags() {
	 	$page_name = 'archives';
		$section_name = 'meta';
		$style = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ) );
		$class = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'container_class' ) );	
	 	$tags_list = _bootstrap_get_tags( ' ', $style, $class );
		if ( $tags_list ) {
		 	$information = array(
		 		'icon-title' => __( 'Post Tags', '_bootstrap'),
		 		'icon'	=> 'tags', // Icon names so dont internationalized
		 		'links'	=> $tags_list,
		 	);
		 	_bootstrap_post_information( $information  );
	 	}
	}
}

if ( ! function_exists( '_bootstrap_post_comments' ) ) {
	
	/**
	 * Prints HTML with meta information for the current post comments.
	 * 
	 * @return void
	 */
	 function _bootstrap_post_comments() {
	 	if ( comments_open() ) {
		 	$num_comments = get_comments_number(); // get_comments_number returns only a numeric value
		 	if ( $num_comments == 0 ) {
		 		$comments = __( 'Leave a Comment', '_bootstrap' );
		 	} 
		 	elseif ( $num_comments > 1 ) {
		 		$comments = $num_comments . __( ' Comments', '_bootstrap' );
		 	}
		 	else {
		 		$comments = __( '1 Comment', '_bootstrap' );
		 	}	 	
		 	$information = array(
		 		'icon-title' 	=> __( 'Comments', '_bootstrap'),
		 		'icon'			=> 'comments', // Icon names so dont internationalized
		 		'url'			=> get_comments_link(),
		 		'title'			=> __( 'View Comments on this Post', '_bootstrap' ),
		 		'text'			=> $comments,
		 	);
		 	_bootstrap_post_information( $information  );
	 	}
	}
}

if ( ! function_exists( '_bootstrap_post_information' ) ) {
	
	/**
	 * Prints HTML with meta information for the current post date, author, categories or tags.
	 * This is an internal function used by each individual meta information display function above.
	 * 
	 * @return void
	 */
	 function _bootstrap_post_information( $information ) {
	 	// All information is already internationalized so just print
	 	$page_name = 'archives';
		$section_name = 'meta';
		$style = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'container_style' ) );
		$class = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'container_class' ) );	
	 	echo '<i title="' . $information[ 'icon-title' ] . '" class="fa fa-' . $information[ 'icon' ] . '"></i> ';
		if ( array_key_exists( 'links', $information ) ) {			
			echo $information[ 'links' ];						
		}
		else {
			echo '<a href="' . esc_url( $information[ 'url' ] ) . '" title="' . $information[ 'title' ] . '" ';
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
			echo esc_html( $information[ 'text' ] );
			if ( $style === 'label' ) {
				echo '</span>';
			}	
			echo '</a>';			
		}
		echo ' </span>';
	}
}

if ( ! function_exists( '_bootstrap_get_categories' ) ) {
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
				$output .= 'title="' . esc_attr( sprintf( __( 'View all posts in %s', '_bootstrap' ), $category->name ) ) . '" ';
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
}

if ( ! function_exists( '_bootstrap_get_tags' ) ) {
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
				$output .= 'title="' . esc_attr( sprintf( __( 'View all posts tagged %s', '_bootstrap' ), $tag->name ) ) . '" ';
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
}


if ( ! function_exists( '_bootstrap_echo_archives_page_header' ) ) {
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
					_e( 'Posts in category ', '_bootstrap' );
					echo single_cat_title();
					if ( ! empty($term_description) ) : 
						echo ' <small>(' . $term_description . ')</small>';
					endif;
					
				elseif ( is_tag() ) :
					_e( 'Posts tagged ', '_bootstrap' );
					echo single_tag_title();
					if ( ! empty($term_description) ) : 
						echo ' <small>(' . $term_description . ')</small>';
					endif;

				elseif ( is_author() ) :
					printf( __( 'Posts by %s %s', '_bootstrap' ), '<span class="vcard">' . get_the_author() . '</span>', get_avatar(get_the_author_meta('ID'),'60') );

				elseif ( is_day() ) :
					printf( __( 'Day: %s', '_bootstrap' ), '<span>' . get_the_date() . '</span>' );

				elseif ( is_month() ) :
					printf( __( 'Month: %s', '_bootstrap' ), '<span>' . get_the_date( _x( 'F Y', 'monthly archives date format', '_bootstrap' ) ) . '</span>' );

				elseif ( is_year() ) :
					printf( __( 'Year: %s', '_bootstrap' ), '<span>' . get_the_date( _x( 'Y', 'yearly archives date format', '_bootstrap' ) ) . '</span>' );

				elseif ( is_tax( 'post_format', 'post-format-aside' ) ) :
					_e( 'Asides', '_bootstrap' );

				elseif ( is_tax( 'post_format', 'post-format-gallery' ) ) :
					_e( 'Galleries', '_bootstrap');

				elseif ( is_tax( 'post_format', 'post-format-image' ) ) :
					_e( 'Images', '_bootstrap');

				elseif ( is_tax( 'post_format', 'post-format-video' ) ) :
					_e( 'Videos', '_bootstrap' );

				elseif ( is_tax( 'post_format', 'post-format-quote' ) ) :
					_e( 'Quotes', '_bootstrap' );

				elseif ( is_tax( 'post_format', 'post-format-link' ) ) :
					_e( 'Links', '_bootstrap' );

				elseif ( is_tax( 'post_format', 'post-format-status' ) ) :
					_e( 'Statuses', '_bootstrap' );

				elseif ( is_tax( 'post_format', 'post-format-audio' ) ) :
					_e( 'Audios', '_bootstrap' );

				elseif ( is_tax( 'post_format', 'post-format-chat' ) ) :
					_e( 'Chats', '_bootstrap' );

				else :
					_e( 'Archives ', '_bootstrap' );
					if ( ! empty($term_description) ) : 
						echo ' <small>(' . $term_description . ')</small>';
					endif;				
					printf ( '%s <small>%s</small>', _e( 'Archives', '_bootstrap' ), $term_description );

				endif;			
			?>
			 </h1>
		</div>
		<?php
		
	}
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
	$page_name = 'archives';
	$section_name = 'excerpt';
	$text  = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'read_more_text' ) );
	$style = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'read_more_text_container_style' ) );
	$class = _bootstrap_get_option( _bootstrap_get_option_name( $page_name, $section_name, 'read_more_text_container_class' ) );
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

/**
* Changed the excerpt length based on user supplied value.
* 
* @param number $length the current length
* 
* @return the new excerpt length
*/
function custom_excerpt_length( $length ) {
	$option_name = _bootstrap_get_option_name( 'archives', 'excerpt', 'length');
	return _bootstrap_get_option( $option_name );
}
add_filter( 'excerpt_length', 'custom_excerpt_length', 999 );

