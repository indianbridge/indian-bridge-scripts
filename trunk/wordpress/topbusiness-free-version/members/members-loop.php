<?php

/**
 * BuddyPress - Members Loop
 */

	do_action( 'bp_before_members_loop' );

	if ( bp_has_members( bp_ajax_querystring( 'members' ) ) ) : ?>

		<div id="pag-top" class="pagination">
	
			<div class="pag-count" id="member-dir-count-top">
	
				<?php bp_members_pagination_count(); ?>
	
			</div>
	
			<div class="pagination-links" id="member-dir-pag-top">
	
				<?php bp_members_pagination_links(); ?>
	
			</div>
	
		</div>
	
		<?php do_action( 'bp_before_directory_members_list' ); ?>
	
		<ul id="members-list" class="item-list" role="main">
	
			<?php

				while ( bp_members() ) : bp_the_member(); ?>
		
					<li>

						<div class="item-avatar"><a href="<?php bp_member_permalink(); ?>"><?php bp_member_avatar(); ?></a></div>

						<div class="action">
			
							<?php do_action( 'bp_directory_members_actions' ); ?>
			
						</div>

						<div class="item table">

							<div class="item-title">

								<a href="<?php bp_member_permalink(); ?>"><?php bp_member_name(); ?></a>

								<?php
								
									if ( function_exists( 'dpa_member_achievements_score' ) ) :
									
										$user_id = bp_get_member_user_id();
										
										echo '<span class="ach-score f11"><a href="'; bp_member_permalink(); echo '/achievements/">';
								
										dpa_member_achievements_score( $user_id );
								
										echo '</a></span>';
								
									endif;
								
								?>

							</div>
			
							<div class="item-meta"><span class="activity"><?php bp_member_last_active(); ?></span> <?php bp_member_latest_update(); ?></div>

							<?php

								do_action( 'bp_directory_members_item' );

								/*
								 * If you want to show specific profile fields here you can,
								 * but it'll add an extra query for each member in the loop
								 * (only one regardless of the number of fields you show):
								 *
								 * bp_member_profile_data( 'field=the field name' );
								 */
							?>

						</div>
			
						<div class="clear"><!-- --></div>

					</li><?php

				endwhile;

			?>
	
		</ul>
	
		<?php

			do_action( 'bp_after_directory_members_list' );
	
			bp_member_hidden_fields();

		?>
	
		<div id="pag-bottom" class="pagination">
	
			<div class="pag-count" id="member-dir-count-bottom">
	
				<?php bp_members_pagination_count(); ?>
	
			</div>
	
			<div class="pagination-links" id="member-dir-pag-bottom">
	
				<?php bp_members_pagination_links(); ?>
	
			</div>
	
		</div><?php

	else: ?>
	
		<div id="message" class="info"><p><?php _e( "Sorry, no members were found.", 'buddypress' ); ?></p></div><?php

	endif;

	do_action( 'bp_after_members_loop' );

?>