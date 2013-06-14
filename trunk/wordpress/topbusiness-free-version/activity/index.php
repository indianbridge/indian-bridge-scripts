<?php

/**
 * Template Name: BuddyPress - Activity Directory
 */

	get_header( 'buddypress' );

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true);

	do_action( 'bp_before_directory_activity_page' ); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">

				<?php

					do_action( 'bp_before_directory_activity' );
		
					if ( !is_user_logged_in() ) :
		
						echo '<h3>' . __( 'Site Activity', 'buddypress' ) . '</h3>';
		
					endif;
		
					do_action( 'bp_before_directory_activity_content' );

					if ( is_user_logged_in() ) :
		
						locate_template( array( 'activity/post-form.php'), true );
		
					endif;
		
					do_action( 'template_notices' ); ?>

					<div id="item-nav">

						<div class="item-list-tabs activity-type-tabs" role="navigation">
	
							<ul>
	
								<?php do_action( 'bp_before_activity_type_tab_all' ); ?>
	
								<li class="selected" id="activity-all">
	
									<a href="<?php echo bp_loggedin_user_domain() . bp_get_activity_slug() . '/'; ?>" title="<?php _e( 'The public activity for everyone on this site.', 'buddypress' ); ?>"><?php printf( __( 'All Members <span>%s</span>', 'buddypress' ), bp_get_total_site_member_count() ); ?></a>
	
								</li>
	
								<?php
		
									if ( is_user_logged_in() ) :
	
		
										do_action( 'bp_before_activity_type_tab_friends' );
	
	
										if ( bp_is_active( 'friends' ) ) :
				
											if ( bp_get_total_friend_count( bp_loggedin_user_id() ) ) : ?>
				
												<li id="activity-friends">
	
													<a href="<?php echo bp_loggedin_user_domain() . bp_get_activity_slug() . '/' . bp_get_friends_slug() . '/'; ?>" title="<?php _e( 'The activity of my friends only.', 'buddypress' ); ?>"><?php printf( __( 'My Friends <span>%s</span>', 'buddypress' ), bp_get_total_friend_count( bp_loggedin_user_id() ) ); ?></a>
	
												</li><?php
		
											endif;
				
										endif;
	
	
										do_action( 'bp_before_activity_type_tab_groups' );
	
	
										if ( bp_is_active( 'groups' ) ) :
				
											if ( bp_get_total_group_count_for_user( bp_loggedin_user_id() ) ) : ?>
				
												<li id="activity-groups">
	
													<a href="<?php echo bp_loggedin_user_domain() . bp_get_activity_slug() . '/' . bp_get_groups_slug() . '/'; ?>" title="<?php _e( 'The activity of groups I am a member of.', 'buddypress' ); ?>"><?php printf( __( 'My Groups <span>%s</span>', 'buddypress' ), bp_get_total_group_count_for_user( bp_loggedin_user_id() ) ); ?></a>
	
												</li><?php
		
											endif;
				
										endif;
	
	
										do_action( 'bp_before_activity_type_tab_favorites' );
	
	
										if ( bp_get_total_favorite_count_for_user( bp_loggedin_user_id() ) ) : ?>
				
											<li id="activity-favorites">
	
												<a href="<?php echo bp_loggedin_user_domain() . bp_get_activity_slug() . '/favorites/'; ?>" title="<?php _e( "The activity I've marked as a favorite.", 'buddypress' ); ?>"><?php printf( __( 'My Favorites <span>%s</span>', 'buddypress' ), bp_get_total_favorite_count_for_user( bp_loggedin_user_id() ) ); ?></a>
	
											</li><?php
		
										endif;
	
										do_action( 'bp_before_activity_type_tab_mentions' ); ?>
	
	
										<li id="activity-mentions">
	
											<a href="<?php echo bp_loggedin_user_domain() . bp_get_activity_slug() . '/mentions/'; ?>" title="<?php _e( 'Activity that I have been mentioned in.', 'buddypress' ); ?>"><?php _e( 'Mentions', 'buddypress' ); ?><?php if ( bp_get_total_mention_count_for_user( bp_loggedin_user_id() ) ) : ?> <strong><?php printf( __( '<span>%s new</span>', 'buddypress' ), bp_get_total_mention_count_for_user( bp_loggedin_user_id() ) ); ?></strong><?php endif; ?></a>
	
										</li><?php
	
	
									endif;
	
			
								do_action( 'bp_activity_type_tabs' ); ?>
	
							</ul>
	
						</div><!-- .item-list-tabs -->

					</div>
		
					<div class="item-list-tabs no-ajax" id="subnav" role="navigation">

						<ul>

							<li class="feed"><a href="<?php bp_sitewide_activity_feed_link(); ?>" title="<?php _e( 'RSS Feed', 'buddypress' ); ?>"><?php _e( 'RSS', 'buddypress' ); ?></a></li>
		
							<?php do_action( 'bp_activity_syndication_options' ); ?>
		
							<li id="activity-filter-select" class="last">

								<label for="activity-filter-by"><?php _e( 'Show:', 'buddypress' ); ?></label> 

								<select id="activity-filter-by">

									<option value="-1"><?php _e( 'Everything', 'buddypress' ); ?></option>
									<option value="activity_update"><?php _e( 'Updates', 'buddypress' ); ?></option>
		
									<?php

										if ( bp_is_active( 'blogs' ) ) : ?>
		
											<option value="new_blog_post"><?php _e( 'Posts', 'buddypress' ); ?></option>
											<option value="new_blog_comment"><?php _e( 'Comments', 'buddypress' ); ?></option><?php

										endif;
		
										if ( bp_is_active( 'forums' ) ) : ?>
			
											<option value="new_forum_topic"><?php _e( 'Forum Topics', 'buddypress' ); ?></option>
											<option value="new_forum_post"><?php _e( 'Forum Replies', 'buddypress' ); ?></option><?php
	
										endif;
		
										if ( bp_is_active( 'groups' ) ) : ?>
			
											<option value="created_group"><?php _e( 'New Groups', 'buddypress' ); ?></option>
											<option value="joined_group"><?php _e( 'Group Memberships', 'buddypress' ); ?></option><?php

										endif;
			
										if ( bp_is_active( 'friends' ) ) : ?>
			
											<option value="friendship_accepted,friendship_created"><?php _e( 'Friendships', 'buddypress' ); ?></option><?php

										endif;

									?>
		
									<option value="new_member"><?php _e( 'New Members', 'buddypress' ); ?></option>
		
									<?php do_action( 'bp_activity_filter_options' ); ?>
		
								</select>

							</li>

						</ul>

					</div><!-- .item-list-tabs -->
		
					<?php do_action( 'bp_before_directory_activity_list' ); ?>
		
					<div class="activity" role="main">

						<?php locate_template( array( 'activity/activity-loop.php' ), true ); ?>

					</div><!-- .activity -->
		
					<?php

						do_action( 'bp_after_directory_activity_list' );
		
						do_action( 'bp_directory_activity_content' );
			
						do_action( 'bp_after_directory_activity_content' );
			
						do_action( 'bp_after_directory_activity' );

						do_action( 'bp_after_directory_activity_page' );

					?>

			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

<?php get_footer( 'buddypress' ); ?>