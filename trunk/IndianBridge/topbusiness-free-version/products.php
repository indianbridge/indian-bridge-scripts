 <?php
/*
Template Name: Products
*/

	// GET COMMON DATA
	$template_url = get_bloginfo('template_url');
	$after_catalog = $panda_pr['after_catalog_data'];
	$posts_per_page = $panda_pr['posts_per_page'];
	$t = $panda_pr['product_template'];

	// HEADER
	get_header(); ?>

	<div id="content">

		<?php

			// A LIST OF CUSTOM CATEGORIES
			$terms = get_terms('catalog');

			$count = count($terms);

			if ($count > 1) :

				$out = '<ul class="no relative ctabs">';

					// Catalog page (this page)
					$out .= '<li><a href="' . get_permalink() . '" class="current"><span><!-- --></span>' . __( 'All','pandathemes' ) . '</a></li>';

					// Custom categories
					foreach ($terms as $term) :
						$out .= '<li><a href="' . get_term_link($term->slug, 'catalog') . '"><span><!-- --></span>' . $term->name . '</a></li>';
					endforeach;

				$out .= '</ul>';

				echo $out;

			endif;

		?>

		<div class="contentbox wfull">

			<?php
				// DEFAULT CONTENT
				if (have_posts()) : while (have_posts()) : the_post();?>

					<h2><?php the_title(); edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span> ' ); ?></h2><?php

					// BREADCRUMBS
					echo '<div class="mt-10 pb25">';
					if ( !is_front_page() && function_exists( 'breadcrumb_trail' ) ) :
						breadcrumb_trail(	array(
							'before'		=> __('Browse:','pandathemes'),
							'show_home'		=> __('Home','pandathemes'),
							'front_page'	=> true,
							'separator'		=> '>'
						));
					endif;
					echo '</div>';

					// CONTENT
					if (get_the_content()) :
					
						echo '<div class="gray-contentbox"><div class="h25"><!-- --></div>';
					
							the_content();
					
						echo '<div class="h17"><!-- --></div></div><div class="clear h25"><!-- --></div>';
					
					endif;


				endwhile;
				endif;

			// GET DATA
			$reviews = $panda_pr['reviews'];

			// GET POSTS
			$paged = (get_query_var('paged')) ? get_query_var('paged') : 1;

			$args = array(
				'post_type'			=> 'product',
				'posts_per_page'	=> $posts_per_page,
				'orderby'			=> 'menu_order',
				'order'				=> 'DESC',
				'paged'				=> $paged,
				'post_status'		=> 'publish'
			);

			$temp = $wp_query;
			$wp_query = null;
			$wp_query = new WP_Query($args);

				// PRODUCT TEMPLATES
					if ( (!($t)) || ( $t == 't1' ) ) : include(TEMPLATEPATH.'/inc/products/t1.php');
						else : include(TEMPLATEPATH.'/inc/products/'.$t.'.php');
					endif;

				// PAGINATION
				if(function_exists('wp_pagenavi')) { ?><div id="wp-pagenavibox"><?php wp_pagenavi(); ?></div><?php } 
				else { ?><div id="but-prev-next"><?php next_posts_link( __('Older Entries','pandathemes') ); previous_posts_link( __('Newer Entries','pandathemes') ); ?></div><?php }

			$wp_query = null;
			$wp_query = $temp;
			wp_reset_query();

			?>

		</div>
				
	<?php

		// AFTER CATALOG
		if ($panda_pr['after_catalog']=='enable' && $after_catalog) { echo '<div id="after_catalog">'.do_shortcode($after_catalog).'</div>'; }

	// Display footer
	get_footer();

?>
