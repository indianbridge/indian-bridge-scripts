<?php

/**
 * BuddyPress Notification Settings
 */

	get_header( 'buddypress' );

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">
	
				<?php do_action( 'bp_before_member_settings_template' ); ?>
	
				<div id="item-header">
	
					<?php locate_template( array( 'members/single/member-header.php' ), true ); ?>
	
				</div><!-- #item-header -->
	
				<div id="item-nav">

					<div class="item-list-tabs no-ajax" id="object-nav" role="navigation">

						<ul>

							<?php

								bp_get_displayed_user_nav();
	
								do_action( 'bp_member_options_nav' );

							?>
	
						</ul>

					</div>

				</div><!-- #item-nav -->
	
				<div id="item-body" role="main">
	
					<?php do_action( 'bp_before_member_body' ); ?>
	
					<div class="item-list-tabs no-ajax" id="subnav">

						<ul>
	
							<?php

								bp_get_options_nav();
	
								do_action( 'bp_member_plugin_options_nav' );

							?>
	
						</ul>

					</div><!-- .item-list-tabs -->
	
					<h3><?php _e( 'Email Notification', 'buddypress' ); ?></h3>
	
					<?php do_action( 'bp_template_content' ); ?>
	
					<form action="<?php echo bp_displayed_user_domain() . bp_get_settings_slug() . '/notifications'; ?>" method="post" class="standard-form" id="settings-form">

						<p><?php _e( 'Send a notification by email when:', 'buddypress' ); ?></p>
	
						<?php

							do_action( 'bp_notification_settings' );
	
							do_action( 'bp_members_notification_settings_before_submit' );

						?>
	
						<div class="submit"><input type="submit" name="submit" value="<?php _e( 'Save Changes', 'buddypress' ); ?>" id="submit" class="auto" /></div>
	
						<?php

							do_action( 'bp_members_notification_settings_after_submit' );
	
							wp_nonce_field('bp_settings_notifications');

						?>
	
					</form>
	
					<?php do_action( 'bp_after_member_body' ); ?>
	
				</div><!-- #item-body -->
	
				<?php do_action( 'bp_after_member_settings_template' ); ?>
	
			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

<?php get_footer( 'buddypress' ); ?>