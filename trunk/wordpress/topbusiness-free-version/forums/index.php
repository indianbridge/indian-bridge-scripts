<?php

/**
 * BuddyPress - Forums Directory
 */

	get_header( 'buddypress' );

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true);

	do_action( 'bp_before_directory_forums_page' ); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">
	
				<?php do_action( 'bp_before_directory_forums' ); ?>
	
				<form action="" method="post" id="forums-search-form" class="dir-form">
	
					<h3><?php _e( 'Forums Directory', 'buddypress' ); ?><?php if ( is_user_logged_in() ) : ?> &nbsp;<a class="button show-hide-new" href="#new-topic" id="new-topic-button"><?php _e( 'New Topic', 'buddypress' ); ?></a><?php endif; ?></h3>
	
					<?php do_action( 'bp_before_directory_forums_content' ); ?>
	
					<div id="forums-dir-search" class="dir-search" role="search">
	
						<?php bp_directory_forums_search_form(); ?>
	
					</div>

				</form>
	
				<?php do_action( 'bp_before_new_topic_form' ); ?>
	
				<div id="new-topic-post">
	
					<?php

						if ( is_user_logged_in() ) :
		
							if ( bp_is_active( 'groups' ) && bp_has_groups( 'user_id=' . bp_loggedin_user_id() . '&type=alphabetical&max=100&per_page=100' ) ) : ?>
		
								<form action="" method="post" id="forum-topic-form" class="standard-form">
		
									<?php do_action( 'groups_forum_new_topic_before' ) ?>
		
									<strong><?php _e( 'Create New Topic:', 'buddypress' ); ?></strong>

									<div class="clear"><!-- --></div>
		
										<?php do_action( 'template_notices' ); ?>
			
										<label><?php _e( 'Title:', 'buddypress' ); ?></label>

										<input type="text" name="topic_title" id="topic_title" value="" />
			
										<label><?php _e( ' ', 'buddypress' ); ?></label>

										<textarea name="topic_text" id="topic_text"></textarea>
			
										<label><?php _e( 'Post In Group Forum:', 'buddypress' ); ?></label>

										<select id="topic_group_id" name="topic_group_id">
			
											<option value=""><?php _e( 'select', 'buddypress' ); ?></option>
			
											<?php

												while ( bp_groups() ) : bp_the_group();
			
													if ( bp_group_is_forum_enabled() && ( is_super_admin() || 'public' == bp_get_group_status() || bp_group_is_member() ) ) : ?>
				
														<option value="<?php bp_group_id(); ?>"><?php bp_group_name(); ?></option><?php
				
													endif;
			
												endwhile;

											?>
			
										</select><!-- #topic_group_id -->
			
										<?php do_action( 'groups_forum_new_topic_after' ); ?>

										<label><?php _e( 'Tags (comma separated):', 'buddypress' ); ?></label>

										<input type="text" name="topic_tags" id="topic_tags" value="" />

										<div class="submit">

											<input type="submit" name="submit_topic" id="submit" class="none" style="opacity:0;" value="<?php _e( 'Post Topic', 'buddypress' ); ?>" />

											<input type="button" name="submit_topic_cancel" id="submit_topic_cancel" value="<?php _e( 'Cancel', 'buddypress' ); ?>" />

										</div>
		
										<div class="clear h30"><!-- --></div>
									
									<?php wp_nonce_field( 'bp_forums_new_topic' ); ?>
		
								</form><!-- #forum-topic-form -->
		
								<div class="clear h10" id="checkGroup"><!-- --></div><?php

							elseif ( bp_is_active( 'groups' ) ) : ?>
		
								<div id="message" class="info"><p><?php printf( __( "You are not a member of any groups so you don't have any group forums you can post in. To start posting, first find a group that matches the topic subject you'd like to start. If this group does not exist, why not <a href='%s'>create a new group</a>? Once you have joined or created the group you can post your topic in that group's forum.", 'buddypress' ), site_url( bp_get_groups_root_slug() . '/create/' ) ) ?></p></div><?php

							endif;
	
						endif;

					?>

				</div><!-- #new-topic-post -->
	
				<?php

					do_action( 'bp_after_new_topic_form' );

					do_action( 'bp_before_topics' );

				?>
	
				<form action="" method="post" id="forums-directory-form" class="dir-form">

					<div id="item-nav">
	
						<div class="item-list-tabs" role="navigation">
	
							<ul>
	
								<li class="selected" id="forums-all">
	
									<a href="<?php echo trailingslashit( bp_get_root_domain() . '/' . bp_get_forums_root_slug() ); ?>"><?php printf( __( 'All Topics <span>%s</span>', 'buddypress' ), bp_get_forum_topic_count() ); ?></a>
	
								</li>
		
								<?php
	
									if ( is_user_logged_in() && bp_get_forum_topic_count_for_user( bp_loggedin_user_id() ) ) : ?>
		
										<li id="forums-personal">
	
											<a href="<?php echo trailingslashit( bp_loggedin_user_domain() . bp_get_forums_slug() . '/topics' ); ?>"><?php printf( __( 'My Topics <span>%s</span>', 'buddypress' ), bp_get_forum_topic_count_for_user( bp_loggedin_user_id() ) ); ?></a>
	
										</li><?php
	
									endif;
	
									do_action( 'bp_forums_directory_group_types' );
	
								?>
		
							</ul>
	
						</div>
	
					</div>

					<div class="item-list-tabs" id="subnav" role="navigation">

						<ul>
	
							<?php do_action( 'bp_forums_directory_group_sub_types' ); ?>
	
							<li id="forums-order-select" class="last filter">
	
								<label for="forums-order-by"><?php _e( 'Order By:', 'buddypress' ); ?></label>

								<select id="forums-order-by">

									<option value="active"><?php _e( 'Last Active', 'buddypress' ); ?></option>
									<option value="popular"><?php _e( 'Most Posts', 'buddypress' ); ?></option>
									<option value="unreplied"><?php _e( 'Unreplied', 'buddypress' ); ?></option>
	
									<?php do_action( 'bp_forums_directory_order_options' ); ?>
	
								</select>

							</li>

						</ul>

					</div>
	
					<div id="forums-dir-list" class="forums dir-list" role="main">
	
						<?php locate_template( array( 'forums/forums-loop.php' ), true ); ?>
	
					</div>
	
					<?php

						do_action( 'bp_directory_forums_content' );
	
						wp_nonce_field( 'directory_forums', '_wpnonce-forums-filter' );

					?>
	
				</form>
	
				<?php

					do_action( 'bp_after_directory_forums' );
	
					do_action( 'bp_after_directory_forums_content' );

				?>
	
			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

	<?php do_action( 'bp_after_directory_forums_page' );

get_footer( 'buddypress' ); ?>