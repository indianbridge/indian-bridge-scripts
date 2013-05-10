<?php

/*
Template Name: Blank
*/

	get_header();

	if (have_posts()) : while (have_posts()) : the_post(); 

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true);

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

			<?php
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