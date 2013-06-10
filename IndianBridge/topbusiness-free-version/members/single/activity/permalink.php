<?php get_header( 'buddypress' ); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">

				<div class="activity no-ajax" role="main">

					<?php if ( bp_has_activities( 'display_comments=threaded&show_hidden=true&include=' . bp_current_action() ) ) : ?>
				
						<ul id="activity-stream" class="no activity-list item-list">

							<?php

								while ( bp_activities() ) : bp_the_activity();
					
									locate_template( array( 'activity/entry.php' ), true );
					
								endwhile;

							?>

						</ul>
				
					<?php endif; ?>

				</div>

			<?php do_action( 'bp_after_directory_activity_page' ); ?>

			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

<?php get_footer( 'buddypress' ); ?>