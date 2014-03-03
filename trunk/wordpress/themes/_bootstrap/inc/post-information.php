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
		$show_excerpt = _bootstrap_get_option( '_bootstrap_post_lists_content_or_excerpt' ) === 'excerpt';		
		$show_featured_image = _bootstrap_get_option( '_bootstrap_post_lists_show_featured_image' );
		?>
		<div class="media">
			<?php if ( $show_featured_image ) { ?>
				<a class="pull-left">
					<?php
						$image_attributes = wp_get_attachment_image_src( get_post_thumbnail_id(), 'thumbnail');
						$image_url = $image_attributes[0];
					?>
					<img class="media-object" src="<?php echo $image_url; ?>" alt="Featured Image">		
				</a>
			<?php } ?>
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
		$metas = _bootstrap_get_option( '_bootstrap_post_meta_information_control' );
		if ( is_array( $metas) ) {
			$enabled_metas = $metas[ 'enabled' ];
			if ( is_array( $enabled_metas) ) {
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
		$style = _bootstrap_get_option( '_bootstrap_post_lists_post_meta_information_container_style' );
		$class = _bootstrap_get_option( '_bootstrap_post_lists_post_meta_information_container_class' );	
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
		$style = _bootstrap_get_option( '_bootstrap_post_lists_post_meta_information_container_style' );
		$class = _bootstrap_get_option( '_bootstrap_post_lists_post_meta_information_container_class' );	
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
		$style = _bootstrap_get_option( '_bootstrap_post_lists_post_meta_information_container_style' );
		$class = _bootstrap_get_option( '_bootstrap_post_lists_post_meta_information_container_class' );	
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

