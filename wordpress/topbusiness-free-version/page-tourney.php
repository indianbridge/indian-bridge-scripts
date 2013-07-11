<?php
/*
Template Name: Tourney
*/

	get_header();

	if (have_posts()) : while (have_posts()) : the_post(); 

	// GET CUSTOM SIDEBAR
	//$sidebar = get_post_meta($post->ID, 'sidebar_value', true);
	$sidebar='No sidebar';

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
				// Get Ancestors and locate root
				$ancestors = $post->ancestors;
				$root = $post->ID;
				foreach($ancestors as $ancestor) {
					$page_template = get_post_meta( $ancestor, '_wp_page_template', true );
					if ($page_template != 'page-tourney.php') break;
					$root = $ancestor;
				}
							
				// Set images from featured image of root
				if (has_post_thumbnail( $root ) ):
					$image = wp_get_attachment_image_src( get_post_thumbnail_id( $root ), 'single-post-thumbnail' );
					//echo '<a href="'.get_permalink($root).'"><img style="display: block;margin-left: auto;margin-right: auto;" src="'.$image[0].'"/></a>';
					$path = $image[0];
					$w='240';
					$h='240';
					$a = $theme_options['thumbs_crop_top']=='enable' ? '&a=t' : '';
					$out = '<a rel="prettyPhoto" href="' . $path . '" title="Test"><img id="single-feat-img" class="fr ml10 br3" src="' . get_bloginfo('template_url') . '/timthumb.php?src=' . $path . '&amp;h=' . $h . '&amp;w=' . $w . '&amp;zc=3&amp;q=90'. $a . '" width="' . $w . '" alt="altTest" /></a>';
					echo $out;
					//echo '<img style="display: block;margin-left: auto;margin-right: auto;" src="'.$image[0].'"/>';
				endif;			

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
				

			
				echo '<div class="clear h20"><!-- --></div>';

				?>
				<h2><?php 
				if ($root == $post->ID) {
					echo 'Home';
				}
				else {
					the_title();
				}
				edit_post_link( __( 'edit','pandathemes' ), '<span class="f13"> - ', '</span>' ) ?></h2>
				<?php
				
				$mypages = get_pages( array( 'child_of' => $root, 'parent' => $root, 'sort_column' => 'menu_order', 'sort_order' => 'asc' ) );
				echo '<div class="tabber-widget-default">';
					// Set the top menu
					echo '<ul class="tabber-widget-tabs">';	
						if ($root == $post->ID) {
							echo '<li><a class="selected" href="'.get_permalink($root).'">HOME</a></li>';
						}
						else {
							echo '<li><a href="'.get_permalink($root).'">HOME</a></li>';
						}
						foreach( $mypages as $page ) {
							if (in_array($page->ID, $ancestors) or $page->ID == $post->ID) {
								echo '<li><a class="selected" href="'.get_page_link($page->ID).'">'.$page->post_title.'</a></li>';
							}
							else {
								echo '<li><a href="'.get_page_link($page->ID).'">'.$page->post_title.'</a></li>';
							}
						}
					echo '</ul>';
								

					// Set the content
					echo '<div class="tabber-widget-content">';
						echo '<div class="tabber-widget">';
							// CONTENT
							the_content();
						echo '</div>';
					echo '</div>';
				echo '</div>';	

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
