<?php get_header(); ?>

	<div id="wrapper">

		<div id="content">

			<div class="contentbox w620">

				<?php

					// BREADCRUMBS
					if ( !is_front_page() && function_exists( 'breadcrumb_trail' ) ) {
						breadcrumb_trail(	array(
							'before'		=> __('Browse:','pandathemes'),
							'show_home'		=> __('Home','pandathemes'),
							'front_page'	=> true,
							'separator'		=> '>'
						));
					}

					// Get number of search results
					if ( ! $_GET["cat"] == '' ) :

						$filter_cat = '&cat='.$_GET["cat"];

					endif;

					$allsearch = &new WP_Query( 's=' . str_replace( ' ', '+', get_search_query() ) . '&showposts=-1' . $filter_cat );

					$count = $allsearch->post_count;

					wp_reset_query();

					// Display
					$out = '<blockquote>';

						$out .= '<p class="f120 fl">' . __('Search results for &laquo;','pandathemes') . get_search_query() . '&raquo;</p>';

						$out .= '<div class="clear"><!-- --></div>';

						$out .= '<span class="f11">' . __('Entries found','pandathemes') . ':&nbsp;' . $count . '</span>';

					$out .= '</blockquote>';

					echo $out;

				?>

				<div id="archive">
				
				<?php

					if ( have_posts() ) :
					
						while ( have_posts() ) : the_post();

							// POSTS TEMPLATES
							$t = $theme_options['blog_template'];
								if ( (!($t)) || ( $t == 'default' ) ) : include(TEMPLATEPATH.'/inc/posts/default.php');
									else : include(TEMPLATEPATH.'/inc/posts/'.$t.'.php');
								endif;

						endwhile;

						// PAGINATION
						if (function_exists('wp_pagenavi')) { ?><div id="wp-pagenavibox"><?php wp_pagenavi(); ?></div><?php } 
						else { ?><div id="but-prev-next"><?php next_posts_link( __('Older Entries','pandathemes') ); previous_posts_link( __('Newer Entries','pandathemes') ); ?></div><?php }
						
					else :
					
						echo '<h3>'.__('Nothing found','pandathemes').'</h3>';
			
						echo '<p class="pb20">'.__( 'Sorry, but nothing matched your search criteria. Please try again with some different keywords.','pandathemes' ).'</p>';
						
						get_search_form();

					endif; ?>

					<div class="clear h30"><!-- --></div>

				</div><!-- end archive -->

			</div><!-- end contentbox -->

			<?php

			 // SIDEBAR
			$a = '<div class="sidebar-wrapper"><div class="sidebar">';
			$z = '</div></div>';

			// DISPLAY CUSTOM SIDEBAR
			if ( $sidebar != 'No sidebar' ) :

				if ($sidebar) :

					echo $a; if ( function_exists('dynamic_sidebar') && dynamic_sidebar($sidebar)); echo $z;

				else :
					
					echo $a; if (function_exists('dynamic_sidebar') && dynamic_sidebar('Default Sidebar')); echo $z;
						
				endif;

			endif; ?>

<?php get_footer(); ?>