<?php

/**
 * Display navigation to next/previous set of posts when applicable.
 *
 * @return void
 */
function _bootstrap_wp_paging_nav( $args = '' ) {
	// Don't print empty markup if there's only one page.
	if ( $GLOBALS['wp_query']->max_num_pages < 2 ) {
		return;
	}
	$defaults = array(
		'prev_text' => __( '<span class="meta-nav">&laquo;</span> Previous', '_bootstrap' ),
		'next_text' => __( 'Next <span class="meta-nav">&raquo;</span>', '_bootstrap' ),
	);	
	$args = wp_parse_args( $args, $defaults );
	extract($args, EXTR_SKIP);
	?>
	<nav class="navigation paging-navigation" role="navigation">
		<div class="nav-links">

			<?php if ( get_next_posts_link() ) : ?>
			<div class="pull-left nav-previous"><?php next_posts_link( $prev_text ); ?></div>
			<?php endif; ?>

			<?php if ( get_previous_posts_link() ) : ?>
			<div class="pull-right nav-next"><?php previous_posts_link( $next_text ); ?></div>
			<?php endif; ?>

		</div><!-- .nav-links -->
	</nav><!-- .navigation -->
	<?php
}

	/**
	 * Display navigation to next/previous set of posts when applicable using Bootstrap Pager.
	 *
	 * @return void
	 */
function _bootstrap_paging_nav( $args = '' ) {
	// Don't print empty markup if there's only one page.
	if ( $GLOBALS['wp_query']->max_num_pages < 2 ) {
		return;
	}
	$defaults = array(
		'prev_text' => __( '<span class="meta-nav">&laquo;</span> Previous', '_bootstrap' ),
		'next_text' => __( 'Next <span class="meta-nav">&raquo;</span>', '_bootstrap' ),
	);	
	$args = wp_parse_args( $args, $defaults );
	extract($args, EXTR_SKIP);
	?>
	<nav class="navigation paging-navigation" role="navigation">
		<div class="nav-links">
			<ul class="pager">
			<?php if ( get_next_posts_link() ) : ?>
			<li class="previous"><?php next_posts_link( $prev_text ); ?></li>
			<?php endif; ?>

			<?php if ( get_previous_posts_link() ) : ?>
			<li class="next"><?php previous_posts_link( $next_text ); ?></li>
			<?php endif; ?>
			</ul>
		</div><!-- .nav-links -->
	</nav><!-- .navigation -->
	<?php
}

	/**
	 * Calls the appropriate pager/pagination function depending on selected options
	 *
	 * @return void
	 */	
function _bootstrap_paginate() {
	// Check is paging has been enabled
	$show_paging = _bootstrap_get_option( '_bootstrap_post_lists_show_paging' );
	if ( ! $show_paging ) {
		return;
	}		
	// Don't print empty markup if there's only one page.
	if ( $GLOBALS['wp_query']->max_num_pages < 2 ) {
		return;
	}

	$style = _bootstrap_get_option( '_bootstrap_post_lists_paging_style' );
	?>
	<nav class="navigation paging-navigation" role="navigation">
		<div class="nav-links">
			<?php
				$left = _bootstrap_get_font_awesome_icon( 'chevron-left', '<' );
				$right = _bootstrap_get_font_awesome_icon( 'chevron-right', '>' );
				$args_pagination = array(
			      'base'      => str_replace( 999999999, '%#%', get_pagenum_link( 999999999 ) ),
			      'format'    => '',
			      'current'     => max( 1, get_query_var('paged') ),
			      'total'     => $GLOBALS['wp_query']->max_num_pages,
			      'prev_text'   => $left,
			      'next_text'   => $right,
			      'end_size'    => 3,
			      'mid_size'    => 3,
			    );				
			    $args_paging = array(
			      'prev_text'   => $left . ' Older Entries',
			      'next_text'   => 'Newer Entries ' . $right,
			    );
				switch ( $style ) {
					case 'wp_paging' :
						_bootstrap_wp_paging_nav( apply_filters( '_bootstrap_paging_args', $args_paging ) );
						break;
					case 'wp_pagination' :
						echo paginate_links( apply_filters( '_bootstrap_pagination_args', $args_pagination ) );
						break;
					case 'bootstrap_paging' :
						_bootstrap_paging_nav( apply_filters( '_bootstrap_paging_args', $args_paging ) );
						break;
					case 'bootstrap_pagination' :
					default :
						echo _bootstrap_paginate_links( apply_filters( '_bootstrap_pagination_args', $args_pagination ) );
						break;							
				}
			?>
		</div>
	</nav>	
	<?php
}

/**
 * Retrieve pager link for archive post pages.
 *
 * Technically, the function can be used to create paginated link list for any
 * area. The 'base' argument is used to reference the url, which will be used to
 * create the paginated links. The 'format' argument is then used for replacing
 * the page number. It is however, most likely and by default, to be used on the
 * archive post pages.
 *
 * The 'type' argument controls format of the returned value. The default is
 * 'plain', which is just a string with the links separated by a newline
 * character. The other possible values are either 'array' or 'list'. The
 * 'array' value will return an array of the paginated link list to offer full
 * control of display. The 'list' value will place all of the paginated links in
 * an unordered HTML list.
 *
 * The 'total' argument is the total amount of pages and is an integer. The
 * 'current' argument is the current page number and is also an integer.
 *
 * An example of the 'base' argument is "http://example.com/all_posts.php%_%"
 * and the '%_%' is required. The '%_%' will be replaced by the contents of in
 * the 'format' argument. An example for the 'format' argument is "?page=%#%"
 * and the '%#%' is also required. The '%#%' will be replaced with the page
 * number.
 *
 * You can include the previous and next links in the list by setting the
 * 'prev_next' argument to true, which it is by default. You can set the
 * previous text, by using the 'prev_text' argument. You can set the next text
 * by setting the 'next_text' argument.
 *
 * If the 'show_all' argument is set to true, then it will show all of the pages
 * instead of a short list of the pages near the current page. By default, the
 * 'show_all' is set to false and controlled by the 'end_size' and 'mid_size'
 * arguments. The 'end_size' argument is how many numbers on either the start
 * and the end list edges, by default is 1. The 'mid_size' argument is how many
 * numbers to either side of current page, but not including current page.
 *
 * It is possible to add query vars to the link by using the 'add_args' argument
 * and see {@link add_query_arg()} for more information.
 *
 * @since 2.1.0
 *
 * @param string|array $args Optional. Override defaults.
 * @return array|string String of page links or array of page links.
 */
function _bootstrap_paginate_links( $args = '' ) {
	$defaults = array(
		'base' => '%_%', // http://example.com/all_posts.php%_% : %_% is replaced by format (below)
		'format' => '?page=%#%', // ?page=%#% : %#% is replaced by the page number
		'total' => 1,
		'current' => 0,
		'show_all' => false,
		'prev_next' => true,
		'prev_text' => __('&laquo; Previous'),
		'next_text' => __('Next &raquo;'),
		'end_size' => 1,
		'mid_size' => 2,
		'add_args' => false, // array of query args to add
		'add_fragment' => ''
	);

	$args = wp_parse_args( $args, $defaults );
	extract($args, EXTR_SKIP);

	// Who knows what else people pass in $args
	$total = (int) $total;
	if ( $total < 2 )
		return;
	$current  = (int) $current;
	$end_size = 0  < (int) $end_size ? (int) $end_size : 1; // Out of bounds?  Make it the default.
	$mid_size = 0 <= (int) $mid_size ? (int) $mid_size : 2;
	$add_args = is_array($add_args) ? $add_args : false;
	$r = '<ul class="pagination">';
	$n = 0;
	$dots = false;

	if ( $prev_next && $current && 1 < $current ) {
		$link = str_replace('%_%', 2 == $current ? '' : $format, $base);
		$link = str_replace('%#%', $current - 1, $link);
		if ( $add_args )
			$link = add_query_arg( $add_args, $link );
		$link .= $add_fragment;
		$r .= '<li><a class="prev page-numbers" href="' . esc_url( apply_filters( 'paginate_links', $link ) ) . '">' . $prev_text . '</a></li>';
	}
	else if ( $prev_next && $current ) {
		$r .= '<li class="disabled"><span>' . $prev_text . '</span></li>';
	}
	for ( $n = 1; $n <= $total; $n++ ) {
		$n_display = number_format_i18n($n);
		if ( $n == $current ) {
			$r .= '<li class="active"><span>' . $n_display . ' <span class="sr-only">(current)</span></span></li>';
			$dots = true;
		}
		else {
			if ( $show_all || ( $n <= $end_size || ( $current && $n >= $current - $mid_size && $n <= $current + $mid_size ) || $n > $total - $end_size ) ) {
				$link = str_replace('%_%', 1 == $n ? '' : $format, $base);
				$link = str_replace('%#%', $n, $link);
				if ( $add_args )
					$link = add_query_arg( $add_args, $link );
				$link .= $add_fragment;
				$r .= '<li><a class="page-numbers" href="' . esc_url( apply_filters( 'paginate_links', $link ) ) . '">' . $n_display . '</a></li>';
				$dots = true;
			}
			elseif ( $dots && !$show_all ) {
				$r .= '<li><span class="page-numbers dots">' . __( '&hellip;' ) . '</span></li>';
				$dots = false;
			}
		}
	}
	if ( $prev_next && $current && ( $current < $total || -1 == $total ) ) {
		$link = str_replace('%_%', $format, $base);
		$link = str_replace('%#%', $current + 1, $link);
		if ( $add_args )
			$link = add_query_arg( $add_args, $link );
		$link .= $add_fragment;
		$r .= '<li><a class="next page-numbers" href="' . esc_url( apply_filters( 'paginate_links', $link ) ) . '">' . $next_text . '</a></li>';
	}
	else if ( $prev_next && $current ) {
		$r .= '<li class="disabled"><span>' . $next_text . '</span></li>';
	}
	$r .= '</ul>';
	return $r;
}


/**
 * Display navigation to next/previous set of comments when applicable.
 *
 * @return void
 */
function _bootstrap_comments_wp_paging_nav( $prev_text, $next_text ) {
	?>
	<nav id="comment-nav-above" class="comment-navigation" role="navigation">
		<div class="nav-links clearfix">
			<div class="pull-left nav-previous"><?php previous_comments_link( $prev_text ); ?></div>
			<div class="pull-right nav-next"><?php next_comments_link( $next_text ); ?></div>
		</div>
	</nav><!-- #comment-nav-above -->	
	<?php
}

	/**
	 * Display navigation to next/previous set of posts when applicable using Bootstrap Pager.
	 *
	 * @return void
	 */
function _bootstrap_comments_paging_nav( $prev_text, $next_text ) {
	?>
	<nav id="comment-nav-above" class="comment-navigation" role="navigation">
		<div class="nav-links">
			<ul class="pager">
			<?php if ( get_previous_comments_link() ) : ?>
			<li class="previous"><?php previous_comments_link( $prev_text ); ?></li>
			<?php endif; ?>

			<?php if ( get_next_comments_link() ) : ?>
			<li class="next"><?php next_comments_link( $next_text ); ?></li>
			<?php endif; ?>
			</ul>		
		</div>
	</nav><!-- #comment-nav-above -->	
	<?php
}

	/**
	 * Calls the appropriate paging function for comments depending on selected options
	 *
	 * @return void
	 */	
function _bootstrap_paginate_comments() {
	$prev_text = __( '<span class="meta-nav">&laquo;</span> Older Comments', '_bootstrap' );
	$next_text = __( 'Newer Comments <span class="meta-nav">&raquo;</span>', '_bootstrap' );
	paginate_comments_links();
	//_bootstrap_comments_wp_paging_nav( $prev_text, $next_text );		
}

?>