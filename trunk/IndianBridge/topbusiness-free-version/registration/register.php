<?php

	get_header( 'buddypress' );

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">
	
			<?php do_action( 'bp_before_register_page' ); ?>
	
			<div class="page" id="register-page">
	
				<form action="" name="signup_form" id="signup_form" class="standard-form" method="post" enctype="multipart/form-data">
	
					<?php

						if ( 'registration-disabled' == bp_get_current_signup_step() ) :

							do_action( 'template_notices' );

							do_action( 'bp_before_registration_disabled' ); ?>
		
							<p><?php _e( 'User registration is currently not allowed.', 'buddypress' ); ?></p>
		
							<?php do_action( 'bp_after_registration_disabled' );

						endif; // registration-disabled signup setp
		
						if ( 'request-details' == bp_get_current_signup_step() ) : ?>
		
							<h2><?php _e( 'Create an Account', 'buddypress' ); ?></h2>
			
							<?php do_action( 'template_notices' ); ?>
			
							<p><?php _e( 'Registering for this site is easy, just fill in the fields below and we\'ll get a new account set up for you in no time.', 'buddypress' ); ?></p>
			
							<?php do_action( 'bp_before_account_details_fields' ); ?>

							<div class="h20"><!-- --></div>

							<div class="register-section" id="basic-details-section">
			
								<?php /***** Basic Account Details ******/ ?>
			
								<h4><?php _e( 'Account Details', 'buddypress' ); ?></h4>
			
								<label for="signup_username"><?php _e( 'Username', 'buddypress' ); echo ' '; _e( '(required)', 'buddypress' ); ?></label>
								<?php do_action( 'bp_signup_username_errors' ); ?>
								<input type="text" name="signup_username" id="signup_username" value="<?php bp_signup_username_value(); ?>" />
			
								<label for="signup_email"><?php _e( 'Email Address', 'buddypress' ); echo ' '; _e( '(required)', 'buddypress' ); ?></label>
								<?php do_action( 'bp_signup_email_errors' ); ?>
								<input type="text" name="signup_email" id="signup_email" value="<?php bp_signup_email_value(); ?>" />
			
								<label for="signup_password"><?php _e( 'Choose a Password', 'buddypress' ); echo ' '; _e( '(required)', 'buddypress' ); ?></label>
								<?php do_action( 'bp_signup_password_errors' ); ?>
								<input type="password" name="signup_password" id="signup_password" value="" />
			
								<label for="signup_password_confirm"><?php _e( 'Confirm Password', 'buddypress' ); echo ' '; _e( '(required)', 'buddypress' ); ?></label>
								<?php do_action( 'bp_signup_password_confirm_errors' ); ?>
								<input type="password" name="signup_password_confirm" id="signup_password_confirm" value="" />
			
							</div><!-- #basic-details-section -->
		
							<?php do_action( 'bp_after_account_details_fields' );
		
							/***** Extra Profile Details ******/
			
							if ( bp_is_active( 'xprofile' ) ) :
			
								do_action( 'bp_before_signup_profile_fields' ); ?>
			
								<div class="register-section" id="profile-details-section">
			
									<h4><?php _e( 'Profile Details', 'buddypress' ); ?></h4>
			
									<?php
	
										/* Use the profile field loop to render input fields for the 'base' profile field group */
										if ( bp_is_active( 'xprofile' ) ) :
	
											if ( bp_has_profile( 'profile_group_id=1' ) ) :
	
												while ( bp_profile_groups() ) : bp_the_profile_group();
				
													while ( bp_profile_fields() ) : bp_the_profile_field(); ?>
							
														<div class="editfield">
							
															<?php
	
																if ( 'textbox' == bp_get_the_profile_field_type() ) : ?>
							
																	<label for="<?php bp_the_profile_field_input_name(); ?>"><?php bp_the_profile_field_name(); if ( bp_get_the_profile_field_is_required() ) : echo ' ' . __( '(required)', 'buddypress' ); endif; ?></label>
	
																	<?php do_action( 'bp_' . bp_get_the_profile_field_input_name() . '_errors' ); ?>
	
																	<input type="text" name="<?php bp_the_profile_field_input_name(); ?>" id="<?php bp_the_profile_field_input_name(); ?>" value="<?php bp_the_profile_field_edit_value(); ?>" /><?php
	
																endif;
							
																if ( 'textarea' == bp_get_the_profile_field_type() ) : ?>
							
																	<label for="<?php bp_the_profile_field_input_name(); ?>"><?php bp_the_profile_field_name(); if ( bp_get_the_profile_field_is_required() ) : echo ' ' . __( '(required)', 'buddypress' ); endif; ?></label>
	
																	<?php do_action( 'bp_' . bp_get_the_profile_field_input_name() . '_errors' ); ?>
	
																	<textarea rows="5" cols="40" name="<?php bp_the_profile_field_input_name(); ?>" id="<?php bp_the_profile_field_input_name(); ?>"><?php bp_the_profile_field_edit_value(); ?></textarea><?php
	
																endif;
							
																if ( 'selectbox' == bp_get_the_profile_field_type() ) : ?>
							
																	<label for="<?php bp_the_profile_field_input_name(); ?>"><?php bp_the_profile_field_name(); if ( bp_get_the_profile_field_is_required() ) : echo ' ' . __( '(required)', 'buddypress' ); endif; ?></label>
	
																	<?php do_action( 'bp_' . bp_get_the_profile_field_input_name() . '_errors' ); ?>
	
																	<select name="<?php bp_the_profile_field_input_name(); ?>" id="<?php bp_the_profile_field_input_name(); ?>">
	
																		<?php bp_the_profile_field_options(); ?>
	
																	</select><?php
	
																endif;
							
																if ( 'multiselectbox' == bp_get_the_profile_field_type() ) : ?>
							
																	<label for="<?php bp_the_profile_field_input_name(); ?>"><?php bp_the_profile_field_name(); if ( bp_get_the_profile_field_is_required() ) : echo ' ' . __( '(required)', 'buddypress' ); endif; ?></label>
	
																	<?php do_action( 'bp_' . bp_get_the_profile_field_input_name() . '_errors' ); ?>
	
																	<select name="<?php bp_the_profile_field_input_name(); ?>" id="<?php bp_the_profile_field_input_name(); ?>" multiple="multiple">
	
																		<?php bp_the_profile_field_options(); ?>
	
																	</select><?php
	
																endif;
							
																if ( 'radio' == bp_get_the_profile_field_type() ) : ?>
							
																	<div class="radio">
	
																		<span class="label"><?php bp_the_profile_field_name(); if ( bp_get_the_profile_field_is_required() ) : echo ' ' . __( '(required)', 'buddypress' ); endif; ?></span>
								
																		<?php
	
																			do_action( 'bp_' . bp_get_the_profile_field_input_name() . '_errors' );
	
																			bp_the_profile_field_options();
	
																			if ( !bp_get_the_profile_field_is_required() ) : ?>
	
																				<a class="clear-value" href="javascript:clear( '<?php bp_the_profile_field_input_name(); ?>' );"><?php _e( 'Clear', 'buddypress' ); ?></a><?php
	
																			endif;
	
																		?>
	
																	</div><?php
	
																endif;
							
																if ( 'checkbox' == bp_get_the_profile_field_type() ) : ?>
							
																	<div class="checkbox">
	
																		<span class="label"><?php bp_the_profile_field_name(); if ( bp_get_the_profile_field_is_required() ) : echo ' ' . __( '(required)', 'buddypress' ); endif; ?></span>
	
																		<?php
	
																			do_action( 'bp_' . bp_get_the_profile_field_input_name() . '_errors' );
	
																			bp_the_profile_field_options();
	
																		?>
	
																	</div><?php
	
																endif;
							
																if ( 'datebox' == bp_get_the_profile_field_type() ) : ?>
								
																	<div class="datebox">
	
																		<label for="<?php bp_the_profile_field_input_name(); ?>_day"><?php bp_the_profile_field_name(); if ( bp_get_the_profile_field_is_required() ) : echo ' ' . __( '(required)', 'buddypress' ); endif; ?></label>
	
																		<?php do_action( 'bp_' . bp_get_the_profile_field_input_name() . '_errors' ); ?>
								
																		<select name="<?php bp_the_profile_field_input_name(); ?>_day" id="<?php bp_the_profile_field_input_name(); ?>_day">
																			<?php bp_the_profile_field_options( 'type=day' ); ?>
																		</select>
								
																		<select name="<?php bp_the_profile_field_input_name(); ?>_month" id="<?php bp_the_profile_field_input_name(); ?>_month">
																			<?php bp_the_profile_field_options( 'type=month' ); ?>
																		</select>
								
																		<select name="<?php bp_the_profile_field_input_name(); ?>_year" id="<?php bp_the_profile_field_input_name(); ?>_year">
																			<?php bp_the_profile_field_options( 'type=year' ); ?>
																		</select>
	
																	</div><?php
	
																endif;
							
																do_action( 'bp_custom_profile_edit_fields' );
	
																//echo '<p class="description">';
																//bp_the_profile_field_description();
																//echo '</p>';

															?>
							
														</div><?php
	
													endwhile; ?>
							
													<input type="hidden" name="signup_profile_field_ids" id="signup_profile_field_ids" value="<?php bp_the_profile_group_field_ids(); ?>" /><?php
	
												endwhile;
	
											endif;
	
										endif;
	
									?>
							
								</div><!-- #profile-details-section --><?php
	
								do_action( 'bp_after_signup_profile_fields' );
						
							endif;
	
	
	
							if ( bp_get_blog_signup_allowed() ) :
			
								do_action( 'bp_before_blog_details_fields' );
			
								/***** Blog Creation Details ******/ ?>

								<div class="clear h20"><!-- --></div>

								<div class="register-section" id="blog-details-section">
			
									<h4><?php _e( 'Blog Details', 'buddypress' ); ?></h4>
			
									<p><input type="checkbox" name="signup_with_blog" id="signup_with_blog" value="1"<?php if ( (int) bp_get_signup_with_blog_value() ) : ?> checked="checked"<?php endif; ?> /> <?php _e( 'Yes, I would like to create a new site', 'buddypress' ); ?></p>
			
									<div id="blog-details"<?php if ( (int) bp_get_signup_with_blog_value() ) : ?>class="show"<?php endif; ?>>
			
										<label for="signup_blog_url"><?php _e( 'Blog URL', 'buddypress' );  echo ' ' . __( '(required)', 'buddypress' ); ?></label>
	
										<?php
	
											do_action( 'bp_signup_blog_url_errors' );
			
											if ( is_subdomain_install() ) : ?>
	
												http:// <input type="text" name="signup_blog_url" id="signup_blog_url" value="<?php bp_signup_blog_url_value(); ?>" /> .<?php bp_blogs_subdomain_base();
	
											else :
	
												echo site_url(); ?>/ <input type="text" name="signup_blog_url" id="signup_blog_url" value="<?php bp_signup_blog_url_value(); ?>" /><?php
	
											endif;
	
										?>
	
										<label for="signup_blog_title"><?php _e( 'Site Title', 'buddypress' ); echo ' ' . __( '(required)', 'buddypress' ); ?></label>
	
										<?php do_action( 'bp_signup_blog_title_errors' ); ?>
	
										<input type="text" name="signup_blog_title" id="signup_blog_title" value="<?php bp_signup_blog_title_value(); ?>" />
			
										<span class="label"><?php _e( 'I would like my site to appear in search engines, and in public listings around this network.', 'buddypress' ); ?>:</span>
	
										<?php do_action( 'bp_signup_blog_privacy_errors' ); ?>
			
										<label style="display:inline-block; margin:0 10px 0 0;"><input type="radio" name="signup_blog_privacy" id="signup_blog_privacy_public" value="public"<?php if ( 'public' == bp_get_signup_blog_privacy_value() || !bp_get_signup_blog_privacy_value() ) : ?> checked="checked"<?php endif; ?> /> <?php _e( 'Yes', 'buddypress' ); ?></label>
	
										<label style="display:inline-block; margin:0 10px 0 0;"><input type="radio" name="signup_blog_privacy" id="signup_blog_privacy_private" value="private"<?php if ( 'private' == bp_get_signup_blog_privacy_value() ) : ?> checked="checked"<?php endif; ?> /> <?php _e( 'No', 'buddypress' ); ?></label>
			
									</div>
			
								</div><!-- #blog-details-section -->
			
								<?php do_action( 'bp_after_blog_details_fields' );
			
							endif;
	
							do_action( 'bp_before_registration_submit_buttons' ); ?>

							<div class="clear h20"><!-- --></div>

							<hr />

							<div class="signup_submit">

								<input type="radio" id="signup_confirm" />&nbsp;<?php _e( 'I have read and agree with <a href="' . get_bloginfo('home') . '/terms-of-use">Terms of Use</a>', 'buddypress' ); ?>

								<div class="clear h20"><!-- --></div>

								<input type="submit" class="none right-submit" name="signup_submit" id="signup_submit" value="<?php _e( 'Complete Sign Up', 'buddypress' ); ?>" />

								<div class="clear h40"><!-- --></div>

							</div>
			
							<?php
	
								do_action( 'bp_after_registration_submit_buttons' );
	
								wp_nonce_field( 'bp_new_signup' );
	
						endif; // request-details signup step



						if ( 'completed-confirmation' == bp_get_current_signup_step() ) : ?>
		
							<h2><?php _e( 'Sign Up Complete!', 'buddypress' ); ?></h2>
			
							<?php

								do_action( 'template_notices' );

								do_action( 'bp_before_registration_confirmed' );
			
								if ( bp_registration_needs_activation() ) : ?>

									<p><?php _e( 'You have successfully created your account! To begin using this site you will need to activate your account via the email we have just sent to your address.', 'buddypress' ); ?></p><?php

								else : ?>

									<p><?php _e( 'You have successfully created your account! Please log in using the username and password you have just created.', 'buddypress' ); ?></p><?php

								endif;
			
								do_action( 'bp_after_registration_confirmed' );
			
						endif; // completed-confirmation signup step
		
					do_action( 'bp_custom_signup_steps' ); ?>
	
				</form>
	
			</div>
	
			<?php do_action( 'bp_after_register_page' ); ?>
	
			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

	<script type="text/javascript">

		jQuery(document).ready( function() {

			if ( jQuery('div#blog-details').length && !jQuery('div#blog-details').hasClass('show') )

				jQuery('div#blog-details').toggle();

			jQuery( 'input#signup_with_blog' ).click( function() {

				jQuery('div#blog-details').fadeOut().toggle();

			});

		});

	</script>

<?php get_footer( 'buddypress' ); ?>