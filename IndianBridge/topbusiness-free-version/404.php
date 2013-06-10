<?php get_header(); ?>

	<div id="layout">

		<div id="wrapper">

			<div id="content">

				<div class="contentbox w620">

					<div id="archive">
					
						<?php
						
							echo '<h3>404</h3>';

							echo '<p class="pb20">'.__('Sorry, no posts matched your criteria.','pandathemes').'</p>';
							
							get_search_form();

							echo '<div class="h20"><!-- --></div>';

							echo '<h5>'.__('Categories','pandathemes').'</h5>';

								echo '<ul>';

									wp_list_categories('title_li=&depth=1&show_count=1');

								echo '</ul>';

							echo '<div class="h20"><!-- --></div>';

							echo '<h5>'.__('Archives','pandathemes').'</h5>';

								echo '<ul>';

									wp_get_archives('type=monthly&show_post_count=1');

								echo '</ul>';
						
							?>

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

			</div>

<?php get_footer(); ?>