<?php
/*
Template Name: Blog
*/

// GET DATA
$sidebar = get_post_meta($post->ID, 'sidebar_value', true);

get_header(); ?>

	<div id="wrapper">

		<div id="content">

			<div class="contentbox <?php
				if ($sidebar == 'No sidebar') :

					echo'wfull';

				elseif ($sidebar == 'Buddy Sidebar') :

					echo'w720';

				else :

					echo'w620';

				endif; ?>">

				<div class="clear h10"><!-- --></div>

				<div id="archive">
			
					<?php
				
						// P O S T S
				
						$qty = $theme_options['posts_qty'];
						$temp = $wp_query;
						$wp_query = null;
						$wp_query = new WP_Query();
						$wp_query->query( 'showposts=' . $qty . '&paged=' . $paged );
				
						while ( $wp_query->have_posts() ) : $wp_query->the_post();
			
							// POSTS TEMPLATES
							$t = $theme_options['blog_template'];

								if ( !$t || $t == 'default' ) :

									include(TEMPLATEPATH.'/inc/posts/default.php');

								else :

									include(TEMPLATEPATH.'/inc/posts/' . $t . '.php');

								endif;
			
						endwhile;
				
						// PAGINATION
						if (function_exists('wp_pagenavi')) { ?><div id="wp-pagenavibox"><?php wp_pagenavi(); ?></div><?php } 
						else { ?><div id="but-prev-next"><?php next_posts_link( __('Older Entries','pandathemes') ); previous_posts_link( __('Newer Entries','pandathemes') ); ?></div><?php }
				
						$wp_query = null;
						$wp_query = $temp;
				
					?>
				
				<div class="clear h30"><!-- --></div>
			
				</div><!-- end archive -->
			
			</div><!-- end contentbox -->

		<?php

	// SIDEBAR
	if ($sidebar == 'Buddy Sidebar') :
		include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
	else :
		include(TEMPLATEPATH.'/inc/sidebar.php');
	endif;

get_footer(); ?>