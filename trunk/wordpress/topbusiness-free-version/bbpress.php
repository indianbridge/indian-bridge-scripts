<?php

/*
Template Name: BBPress
*/
	get_header();

	if (have_posts()) : while (have_posts()) : the_post(); 

	// GET CUSTOM SIDEBAR
	//$sidebar = get_post_meta($post->ID, 'sidebar_value', true);
	$sidebar = 'No sidebar';

	?>

	<div id="content">

		<div class="contentbox <?php
			if ($sidebar == 'No sidebar') :

				echo'wfull';

			elseif ($sidebar == 'Buddy Sidebar') :

				echo'w720';

			else :

				echo'w620';

			endif; ?>">

			<h2><?php the_title(); edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' ) ?></h2>

			<?php

				// BREADCRUMBS
				echo '<div class="mt-10 pb15">';
				if ( !is_front_page() && function_exists( 'breadcrumb_trail' ) ) {
					breadcrumb_trail(	array(
						'before'		=> __('Browse:','pandathemes'),
						'show_home'		=> __('Home','pandathemes'),
						'front_page'	=> true,
						'separator'		=> '>'
					));
				}
				echo '</div>';

				// CONTENT
				the_content();

				echo '<div class="clear h20"><!-- --></div>';

				// PAGINATION
				wp_link_pages(array('before' => '<p><strong>Pages:</strong> ', 'after' => '</p>', 'next_or_number' => 'number'));
				
				// COMMENTS ETC.
				if ( $theme_options['pages_metas'] == 'enable' ) : comments_template(); endif;
				
	endwhile;

		else : echo '<div class="contentbox w620"><h2>404</h2><p>'.__('Sorry, no posts matched your criteria.','pandathemes').'</p>';

	endif; ?>

	</div> <!-- end_of_contentbox -->

		<?php
			// SIDEBAR
			if ($sidebar == 'Buddy Sidebar') :
				include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
			else :
				include(TEMPLATEPATH.'/inc/sidebar.php');
			endif;

	get_footer();

?>