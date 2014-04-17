<?php

/**
 * Display navigation to next/previous post when applicable.
 *
 * @return void
 */
function bfi_bootstrap_post_nav() {
	// Don't print empty markup if there's nowhere to navigate.
	$previous = ( is_attachment() ) ? get_post( get_post()->post_parent ) : get_adjacent_post( false, '', true );
	$next     = get_adjacent_post( false, '', false );

	if ( ! $next && ! $previous ) {
		echo 'Test';
		return;
	}
	?>
	<nav class="navigation paging-navigation" role="navigation">
		<div class="nav-links">
			<ul class="pager">
			<?php if ( $next ) : ?>
			<li class="previous"><?php next_post_link( '%link', '<i class="fa fa-hand-o-left"></i> %title' ); ?></li>
			<?php endif; ?>

			<?php if ( $previous ) : ?>
			<li class="next"><?php previous_post_link( '%link', '%title <i class="fa fa-hand-o-right"></i>' ); ?></li>
			<?php endif; ?>
			</ul>
		</div><!-- .nav-links -->
	</nav><!-- .navigation -->	
	<?php
}

function _bootstrap_comments_paging_nav() {
	$prev_text = '<i class="fa fa-hand-o-left"></i> Newer Comments';
	$next_text = 'Older Comments <i class="fa fa-hand-o-right"></i>';
	?>
	<nav id="comment-nav-above" class="comment-navigation" role="navigation">
		<div class="nav-links">
			<ul class="pager">
			<?php if ( get_next_comments_link() ) : ?>
			<li class="next"><?php next_comments_link( $next_text ); ?></li>
			<?php endif; ?>
			<?php if ( get_previous_comments_link() ) : ?>
			<li class="previous"><?php previous_comments_link( $prev_text ); ?></li>
			<?php endif; ?>			
			</ul>		
		</div>
	</nav><!-- #comment-nav-above -->	
	<?php
}

function _bootstrap_paginate() {	
	// Don't print empty markup if there's only one page.
	if ( $GLOBALS['wp_query']->max_num_pages < 2 ) {
		return;
	}
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
			    echo _bootstrap_paginate_links( apply_filters( '_bootstrap_pagination_args', $args_pagination ) );
			?>
		</div>
	</nav>	
	<?php
}

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

?>