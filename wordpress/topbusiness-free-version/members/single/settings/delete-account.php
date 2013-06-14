<?php

/**
 * BuddyPress Delete Account
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
	
					<h3><?php _e( 'Delete Account', 'buddypress' ); ?></h3>
	
					<form action="<?php echo bp_displayed_user_domain() . bp_get_settings_slug() . '/delete-account'; ?>" name="account-delete-form" id="account-delete-form" class="standard-form" method="post">
	
						<div id="message" class="info">

							<p><?php _e( 'WARNING: Deleting your account will completely remove ALL content associated with it. There is no way back, please be careful with this option.', 'buddypress' ); ?></p>

						</div>
	
						<input type="checkbox" name="delete-account-understand" id="delete-account-understand" value="1" onclick="if(this.checked) { document.getElementById('delete-account-button').disabled = ''; } else { document.getElementById('delete-account-button').disabled = 'disabled'; }" /> <?php _e( 'I understand the consequences of deleting my account.', 'buddypress' ); ?>
	
						<?php do_action( 'bp_members_delete_account_before_submit' ); ?>
	
						<div class="submit">

							<input type="submit" disabled="disabled" value="<?php _e( 'Delete My Account', 'buddypress' ); ?>" id="delete-account-button" name="delete-account-button" />

						</div>
	
						<?php

							do_action( 'bp_members_delete_account_after_submit' );

							wp_nonce_field( 'delete-account' );

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