<?php

	get_header( 'buddypress' );

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">
	
			<?php do_action( 'bp_before_activation_page' ); ?>
	
			<div class="page" id="activate-page">
	
				<?php

					if ( bp_account_was_activated() ) : ?>
		
						<h2 class="widgettitle"><?php _e( 'Account Activated', 'buddypress' ); ?></h2>
		
						<?php
	
							do_action( 'bp_before_activate_content' );
		
							if ( isset( $_GET['e'] ) ) : ?>
	
								<p><?php _e( 'Your account was activated successfully! Your account details have been sent to you in a separate email.', 'buddypress' ); ?></p><?php
	
							else : ?>
	
								<p><?php _e( 'Your account was activated successfully! You can now log in with the username and password you provided when you signed up.', 'buddypress' ); ?></p><?php
	
							endif;
		
					else : ?>
	
						<h3><?php _e( 'Activate your Account', 'buddypress' ); ?></h3>
		
						<?php do_action( 'bp_before_activate_content' ); ?>
		
						<p><?php _e( 'Please provide a valid activation key.', 'buddypress' ); ?></p>
		
						<form action="" method="get" class="standard-form" id="activation-form">
		
							<label for="key"><?php _e( 'Activation Key:', 'buddypress' ); ?></label>

							<input type="text" name="key" id="key" value="" />
		
							<p class="submit"><input type="submit" class="right-submit" name="submit" value="<?php _e( 'Activate', 'buddypress' ); ?>" /></p>
		
						</form><?php

					endif;
	
					do_action( 'bp_after_activate_content' ); ?>
	
			</div><!-- .page -->
	
			<?php do_action( 'bp_after_activation_page' ); ?>
	
			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

<?php get_footer( 'buddypress' ); ?>