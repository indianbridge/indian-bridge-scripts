<?php
/**
 * Retrieve information about posts
 *
 * @package _bootstrap
 */
 
if ( ! function_exists( '_bootstrap_get_published_date' ) ) {
	/**
	 * Prints HTML with meta information for the current post published date.
	 * 
	 * @param boolean $get_last_update_date should the last update date be included.
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
	 * @param boolean $get_last_update_date should the last update date be included.
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
	 * This is an internal function used by __bootstrap_get_post_date.
	 * 
	 * @param boolean $get_last_update_date should the last updated date be printed.
	 * @return void
	  */	
	function _bootstrap_get_post_date_internal( $get_last_update_date=FALSE ) {
		
		// Get the post dates for this post
		$date_time = $get_last_update_date?get_the_date( 'c' ):get_the_modified_date( 'c' );
		$display_date = $get_last_update_date?get_the_date():get_the_modified_date();
	 	$information = array(
	 		'title' => $get_last_update_date ? 
	 				   __( 'Last Updated On', '_bootstrap' ) :
	 				   __( 'Published On', '_bootstrap' ),
	 		'icon'	=> $get_last_update_date ? 'refresh' : 'calendar', // Icon names so dont internationalized
	 		'url'	=> get_permalink(),
	 		'text'	=> $display_date,
	 	);
	 	echo '<time datetime="' . esc_attr( $date_time ) . '">'; // HTML tag should not be internationalized
	 	_bootstrap_post_information( $information  );	
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
	 	$information = array(
	 		'title' => __( 'Authored By', '_bootstrap'),
	 		'icon'	=> 'user', // Icon names so dont internationalized
	 		'url'	=> get_author_posts_url( get_the_author_meta( 'ID' ) ),
	 		'text'	=> get_the_author(),
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
	 	/* translators: used between list items, there is a space after the comma */
	 	$categories_list = get_the_category_list( __( ', ', '_bootstrap' ) );
		if ( $categories_list ) {
		 	$information = array(
		 		'title' => __( 'Post Categories', '_bootstrap'),
		 		'icon'	=> 'folder-open', // Icon names so dont internationalized
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
	 	/* translators: used between list items, there is a space after the comma */
	 	$tags_list = get_the_tag_list( '', __( ', ', '_bootstrap' ) );
		if ( $tags_list ) {
		 	$information = array(
		 		'title' => __( 'Post Tags', '_bootstrap'),
		 		'icon'	=> 'tags', // Icon names so dont internationalized
		 		'links'	=> $tags_list,
		 	);
		 	_bootstrap_post_information( $information  );
	 	}
	}
}

if ( ! function_exists( '_bootstrap_post_comments' ) ) {
	
	/**
	 * Prints HTML with meta information for the current post tags.
	 * 
	 * @return void
	 */
	 function _bootstrap_post_comments() {
	 	$information = array(
	 		'title' => __( 'Post Comments', '_bootstrap'),
	 		'icon'	=> 'comment', // Icon names so dont internationalized
	 	);
	 	// Cannot call __bootstrap_post_information because comments_popup_link() prints instead of returning.
		$print_string = '<span data-toggle="tooltip" title="' . $information[ 'title' ] . '" ';
		$print_string .= 'class="label label-primary">';
		$print_string .= '<span class="glyphicon glyphicon-' . $information[ 'icon' ] . '"></span>';
		printf( __( $print_string, '_bootstrap' ) );
		comments_popup_link( 
 			__( 'Leave a comment', '_bootstrap' ), 
 			__( '1 Comment', '_bootstrap' ), 
 			__( '% Comments', '_bootstrap' ) 
	 	);
	 	printf( __( '</span>', '_bootstrap' ) );
	}
}

if ( ! function_exists( '_bootstrap_post_information' ) ) {
	
	/**
	 * Prints HTML with meta information for the current post author.
	 * 
	 * @return void
	 */
	 function _bootstrap_post_information( $information ) {
	 	// All information is already internationalized so just print
	 	echo '<span data-toggle="tooltip" title="' . $information[ 'title' ] . '" class="label label-primary">';
		echo '<span class="glyphicon glyphicon-' . $information[ 'icon' ] . '"></span>';
		if ( array_key_exists( 'links', $information ) ) {
			printf( '   %1$s</span>', $information[ 'links' ] );			
		}
		else {
			printf( 
				' <a href="%1$s">%2$s</a></span>', 
				esc_url( $information[ 'url' ] ), 
				esc_html( $information[ 'text' ] ) 
			);	
		}
	}
}

