<?php 

	global
		$theme_options,
		$panda_bp,
		$panda_mi;

	// GET TYPE OF POST
	$type = get_post_type();
	

// ***************************************
//
// 	P R O D U C T   R E V I E W S
//
// ***************************************

	if ($type == 'product') :

		// DISPLAY REVIEWS
		if (have_comments()) :

			echo '<ol class="no cmt f13">';
				wp_list_comments('type=comment&callback=comment_template');
			echo '</ol>';
			
		endif;

		// REVIEWS CLOSED FOR THIS ONE PRODUCT ONLY
		if (!comments_open()) :

			_e('Reviews are closed.','pandathemes');

		else :

			?>
		
			<div id="review-label-holder"><input type="submit" id="review-label" class="none cancel-reply" value="<?php _e('Show review form','pandathemes'); ?>" /></div>

			<!-- MAJOR FORM -->

			<form action="<?php echo get_option('siteurl'); ?>/wp-comments-post.php" method="post" id="commentform">

				<?php

					echo '<h5>'.__('Write a Review','pandathemes').'</h5>'; ?>

					<div class="rating">
						<?php _e('Your rate:','pandathemes'); ?>
						<div id="rstars">
							<div class="tooltip-t" title="<?php _e('Very bad','pandathemes') ?>">
								</div><div class="tooltip-t" title="<?php _e('No good','pandathemes') ?>">
								</div><div class="tooltip-t" title="<?php _e('So-so','pandathemes') ?>">
								</div><div class="tooltip-t" title="<?php _e('Good','pandathemes') ?>">
								</div><div class="tooltip-t" title="<?php _e('Excellent','pandathemes') ?>">
							</div>
						</div>
					</div>

					<input type="hidden" name="rate" id="rate" value="<?php echo esc_attr($comment_author_rate); ?>" />

					<p>
						<textarea
							name="comment"
							id="comment"
							cols="100%"
							rows="10"
							onblur="this.value=(this.value=='') ? '<?php _e('Enter here','pandathemes') ?>' : this.value;"
							onfocus="this.value=(this.value=='<?php _e('Enter here','pandathemes') ?>') ? '' : this.value;"
							tabindex="4"><?php _e('Enter here','pandathemes') ?></textarea>
					</p><?php

					if (is_user_logged_in()) :
						echo '<p class="f11">'.__('Logged in as','pandathemes').' <a href="'.get_option('siteurl').'/wp-admin/profile.php">'.$user_identity.'</a> - <a href="'.wp_logout_url(get_permalink()).'">'.__('Log out','pandathemes').'</a></p>';
					else :

						?>

						<p>
							<input
								type="text"
								name="author"
								id="author"
								value="<?php _e('Name','pandathemes') ?>"
								onblur="this.value=(this.value=='') ? '<?php _e('Name','pandathemes') ?>' : this.value;"
								onfocus="this.value=(this.value=='<?php _e('Name','pandathemes') ?>') ? '' : this.value;"
								size="22"
								tabindex="1"
								<?php if ($req) echo "aria-required='true'"; ?>
								/>
							<label for="author"><?php if ($req) _e('* required','pandathemes'); ?></label>
						</p>

						<p>
							<input
								type="text"
								name="email"
								id="email"
								value="<?php _e('Mail','pandathemes') ?>"
								onblur="this.value=(this.value=='') ? '<?php _e('Mail','pandathemes') ?>' : this.value;"
								onfocus="this.value=(this.value=='<?php _e('Mail','pandathemes') ?>') ? '' : this.value;"
								size="22"
								tabindex="2"
								<?php if ($req) echo "aria-required='true'"; ?>
								/>
							<label for="email"><?php if ($req) echo __('* required','pandathemes').', '.__('will not be published','pandathemes'); ?></label>
						</p>

						<p>
							<input
								type="text"
								name="url"
								id="url"
								value="<?php _e('Website','pandathemes') ?>"
								onblur="this.value=(this.value=='') ? '<?php _e('Website','pandathemes') ?>' : this.value;"
								onfocus="this.value=(this.value=='<?php _e('Website','pandathemes') ?>') ? '' : this.value;"
								size="22"
								tabindex="3"
								/>
						</p>

						<?php

					endif;
				
					?>

					<p>
						<span class="submit-wrap">
						<input
							name="submit"
							type="submit"
							id="submit"
							tabindex="5"
							value="<?php _e('Submit Review','pandathemes') ?>"
						/>
						</span>	
						<?php comment_id_fields(); ?>
					</p>

					<?php do_action('comment_form', $post->ID); ?>

			</form>

			<!-- QUICK REPLY FORM -->

			<form action="<?php echo get_option('siteurl'); ?>/wp-comments-post.php" method="post" id="commentform-a" class="hidden absolute">

				<?php

					echo '<h6>'.__('Reply to:','pandathemes').' <span id="to-author"></span></h6>'; ?>

					<p>
						<textarea
							name="comment"
							id="comment"
							cols="100%"
							rows="5"
							onblur="this.value=(this.value=='') ? '<?php _e('Enter here','pandathemes') ?>' : this.value;"
							onfocus="this.value=(this.value=='<?php _e('Enter here','pandathemes') ?>') ? '' : this.value;"
							tabindex="4"><?php _e('Enter here','pandathemes') ?></textarea>
					</p><?php

					if (is_user_logged_in()) :
						echo '<p class="f11">'.__('Logged in as','pandathemes').' <a href="'.get_option('siteurl').'/wp-admin/profile.php">'.$user_identity.'</a> - <a href="'.wp_logout_url(get_permalink()).'">'.__('Log out','pandathemes').'</a></p>';
					else :

						?>

						<p>
							<input
								type="text"
								name="author"
								id="author"
								value="<?php _e('Name','pandathemes') ?>"
								onblur="this.value=(this.value=='') ? '<?php _e('Name','pandathemes') ?>' : this.value;"
								onfocus="this.value=(this.value=='<?php _e('Name','pandathemes') ?>') ? '' : this.value;"
								size="22"
								tabindex="1"
								<?php if ($req) echo "aria-required='true'"; ?>
								/>
							<label for="author"><?php if ($req) _e('* required','pandathemes'); ?></label>
						</p>

						<p>
							<input
								type="text"
								name="email"
								id="email"
								value="<?php _e('Mail','pandathemes') ?>"
								onblur="this.value=(this.value=='') ? '<?php _e('Mail','pandathemes') ?>' : this.value;"
								onfocus="this.value=(this.value=='<?php _e('Mail','pandathemes') ?>') ? '' : this.value;"
								size="22"
								tabindex="2"
								<?php if ($req) echo "aria-required='true'"; ?>
								/>
							<label for="email"><?php if ($req) echo __('* required','pandathemes').', '.__('will not be published','pandathemes'); ?></label>
						</p>

						<?php

					endif;
				
					?>

					<p>
						<span class="submit-wrap">
						<input
							name="submit"
							type="submit"
							id="submit"
							tabindex="5"
							value="<?php _e('Reply','pandathemes') ?>"
						/>
						</span> 
						<?php comment_id_fields(); ?>
					</p>

					<?php do_action('comment_form', $post->ID); ?>

				</form>
				
			<?php

		endif;

	else :
	
// ***************************************
//
// 	S T A N D A R D   C O M M E N T S
//
// ***************************************

	// M E T A   T A B S

	$tabs = '
		<div class="clear h20"><!-- --></div>
		<ul class="kul" id="comments">
	';
	$counter = 0;

		// COMMENTS
		if ($theme_options['post_comments'] == 'enable') : $tabs .= '<li id="cmt">'.__('Comments','pandathemes').'</li>'; $counter++;

			// TRACKBACKS
			if ($theme_options['post_trackbacks'] == 'enable') : $tabs .= '<li id="trb" class="none">'.__('Trackbacks','pandathemes').'</li>'; endif;
	
		endif;

		// METAS
		if ($theme_options['post_metas'] == 'enable') : $tabs .= '<li>'.__('About post','pandathemes').'</li>'; $counter++; endif;

		$tabs .= '</ul>';

	if ($counter != 0) : echo $tabs; ?>
			
	<div class="ktabs">

		<?php
	
		// C O M M E N T S	

		if ($theme_options['post_comments'] == 'enable') :	?>

		<!-- start COMMENTS -->
		<div class="relative">

			<?php 
				// PASSWORD PROTECTED
				if (post_password_required()) : _e( 'This post is password protected. Enter the password to view any comments.','pandathemes');
					return;
				endif;

				// DISPLAY COMMENTS
				if (have_comments()) : 

					$trackbacks = array();

					foreach ($comments as $comment) {
						$comment_type = get_comment_type();
						if($comment_type != 'comment') {	$trackbacks[] = $comment; }
					}

					$total_tb = sizeof($trackbacks);

					$comments_number = get_comments_number() - $total_tb;
					echo '<span id="cmt-qty" class="none">'.$comments_number.'</span>';
					echo '<span id="trb-qty" class="none">'.$total_tb.'</span>';
					?>

					<ol class="no cmt f13">
						<?php
							wp_list_comments( array('callback' => 'pandathemes_comment' ));
						?>
					</ol>

					<?php
						// PAGINATION
						if (get_comment_pages_count() > 1 && get_option('page_comments')) : ?>
							<div class="nav-previous"><?php previous_comments_link(__( '&larr; Older Comments','pandathemes')); ?></div>
							<div class="nav-next"><?php next_comments_link(__( 'Newer Comments &rarr;','pandathemes')); ?></div><?php
						endif;

				endif;

				// COMMENTS CLOSED
				if (!comments_open()) :

						_e('Comments are closed.','pandathemes');

				else :

					// FORM
					if ( get_option('comment_registration') && !is_user_logged_in() ) : 

						echo '<p>'.__('You must be logged in to post a comment.','pandathemes').' - <a href="'.wp_login_url(get_permalink()).'">'.__('Log in','pandathemes').'</a></p>';

					else :
					
						echo '<div id="review-label-holder"><input type="submit" id="review-label" class="none cancel-reply" value="' . __('Show comment form','pandathemes') . '" /></div>';
					
						?>

						<form action="<?php echo get_option('siteurl'); ?>/wp-comments-post.php" method="post" id="commentform">

						<?php

							echo '<h5>'.__('Leave a Reply','pandathemes').'</h5>'; ?>

							<p>
								<textarea
									name="comment"
									id="comment"
									cols="100%"
									rows="10"
									onblur="this.value=(this.value=='') ? '<?php _e('Enter here','pandathemes') ?>' : this.value;"
									onfocus="this.value=(this.value=='<?php _e('Enter here','pandathemes') ?>') ? '' : this.value;"
									tabindex="4"><?php _e('Enter here','pandathemes') ?></textarea>
							</p><?php

							if (is_user_logged_in()) :

								echo '<p class="f11">'.__('Logged in as','pandathemes').' <a href="'.get_option('siteurl').'/wp-admin/profile.php">'.$user_identity.'</a> - <a href="'.wp_logout_url(get_permalink()).'">'.__('Log out','pandathemes').'</a></p>';

							else :

								?>

								<p>
									<input
										type="text"
										name="author"
										id="author"
										value="<?php _e('Name','pandathemes') ?>"
										onblur="this.value=(this.value=='') ? '<?php _e('Name','pandathemes') ?>' : this.value;"
										onfocus="this.value=(this.value=='<?php _e('Name','pandathemes') ?>') ? '' : this.value;"
										size="22"
										tabindex="1"
										<?php if ($req) echo "aria-required='true'"; ?>
										/>
									<label for="author"><?php if ($req) _e('* required','pandathemes'); ?></label>
								</p>

								<p>
									<input
										type="text"
										name="email"
										id="email"
										value="<?php _e('Mail','pandathemes') ?>"
										onblur="this.value=(this.value=='') ? '<?php _e('Mail','pandathemes') ?>' : this.value;"
										onfocus="this.value=(this.value=='<?php _e('Mail','pandathemes') ?>') ? '' : this.value;"
										size="22"
										tabindex="2"
										<?php if ($req) echo "aria-required='true'"; ?>
										/>
									<label for="email"><?php if ($req) echo __('* required','pandathemes').', '.__('will not be published','pandathemes'); ?></label>
								</p>

								<?php

									// Website an input field
									if ($theme_options['website_on_comments'] == 'yes' ) : ?>
										<p>
											<input
												type="text"
												name="url"
												id="url"
												value="<?php _e('Website','pandathemes') ?>"
												onblur="this.value=(this.value=='') ? '<?php _e('Website','pandathemes') ?>' : this.value;"
												onfocus="this.value=(this.value=='<?php _e('Website','pandathemes') ?>') ? '' : this.value;"
												size="22"
												tabindex="3"
												/>
										</p><?php
									endif;

							endif; ?>

							<p>
								<span class="submit-wrap">
								<input
									name="submit"
									type="submit"
									id="submit"
									tabindex="5"
									value="<?php _e('Submit Comment','pandathemes') ?>"
								/>
								</span>	
								<?php comment_id_fields(); ?>
							</p>

							<?php do_action('comment_form', $post->ID); ?>

						</form>

						<!-- QUICK REPLY FORM -->

						<form action="<?php echo get_option('siteurl'); ?>/wp-comments-post.php" method="post" id="commentform-a" class="hidden absolute">

							<?php

								echo '<h6>'.__('Reply to:','pandathemes').' <span id="to-author"></span></h6>'; ?>

								<p>
									<textarea
										name="comment"
										id="comment"
										cols="100%"
										rows="5"
										onblur="this.value=(this.value=='') ? '<?php _e('Enter here','pandathemes') ?>' : this.value;"
										onfocus="this.value=(this.value=='<?php _e('Enter here','pandathemes') ?>') ? '' : this.value;"
										tabindex="4"><?php _e('Enter here','pandathemes') ?></textarea>
								</p><?php

								if (is_user_logged_in()) :
									echo '<p class="f11">'.__('Logged in as','pandathemes').' <a href="'.get_option('siteurl').'/wp-admin/profile.php">'.$user_identity.'</a> - <a href="'.wp_logout_url(get_permalink()).'">'.__('Log out','pandathemes').'</a></p>';
								else :

									?>

									<p>
										<input
											type="text"
											name="author"
											id="author"
											value="<?php _e('Name','pandathemes') ?>"
											onblur="this.value=(this.value=='') ? '<?php _e('Name','pandathemes') ?>' : this.value;"
											onfocus="this.value=(this.value=='<?php _e('Name','pandathemes') ?>') ? '' : this.value;"
											size="22"
											tabindex="1"
											<?php if ($req) echo "aria-required='true'"; ?>
											/>
										<label for="author"><?php if ($req) _e('* required','pandathemes'); ?></label>
									</p>

									<p>
										<input
											type="text"
											name="email"
											id="email"
											value="<?php _e('Mail','pandathemes') ?>"
											onblur="this.value=(this.value=='') ? '<?php _e('Mail','pandathemes') ?>' : this.value;"
											onfocus="this.value=(this.value=='<?php _e('Mail','pandathemes') ?>') ? '' : this.value;"
											size="22"
											tabindex="2"
											<?php if ($req) echo "aria-required='true'"; ?>
											/>
										<label for="email"><?php if ($req) echo __('* required','pandathemes').', '.__('will not be published','pandathemes'); ?></label>
									</p>

									<?php

								endif;
				
								?>

								<p>
									<span class="submit-wrap">
									<input
										name="submit"
										type="submit"
										id="submit"
										tabindex="5"
										value="<?php _e('Reply','pandathemes') ?>"
									/>
									</span> 
									<?php comment_id_fields(); ?>
								</p>

								<?php do_action('comment_form', $post->ID); ?>

							</form>

							<?php

					endif;

				endif; ?>

		</div>
		<!-- end COMMENTS -->
	
			<?php
		
			// T R A C K B A C K S

			if ($theme_options['post_trackbacks'] == 'enable') : ?>
				<!-- start TRACKBACKS -->
				<div>
					<?php
						if($total_tb) {
							echo '<ol>';
								foreach ($trackbacks as $comment) {	?>
									<li>
										<?php comment_author_link() ?>
										<p><?php comment_text() ?></p>
									</li>
									<?php
								} 
							echo '</ol>';
						}
					?>
				</div>
				<!-- end TRACKBACKS -->
			<?php endif; ?>

		<?php
	
		endif;
	
		// M E T A S
	
		if ($theme_options['post_metas'] == 'enable') : ?>

		<!-- start METAS -->
		<div>

			<table class="meta">

				<tr>
					<td class="right strong w25p">
						<?php _e('posted on','pandathemes') ?>
					</td>
					<td>
						<?php the_time( get_option('date_format') ); ?>
					</td>
				</tr>

				<?php if (! is_page()) : ?>
					<tr>
						<td class="right strong w25p">
							<?php _e('in','pandathemes') ?>
						</td>
						<td>
							<?php the_category(', ') ?>
						</td>
					</tr>
				<?php endif; ?>

				<?php if (get_the_tags()) : ?>
					<tr>
						<td class="right strong w25p">
							<?php _e('tagged','pandathemes') ?>
						</td>
						<td>
							<?php the_tags('', ', ', ''); ?>
						</td>
					</tr>
				<?php endif; ?>

				<?php //AUTHOR INFO
					if ($theme_options['author_info'] == 'enable') { ?>
					<tr>
						<td class="right strong w25p">
							<?php _e('about author','pandathemes') ?>
						</td>
						<td>
							<?php

								$out = '';

								// Userpic
								if ( $panda_bp['compatibility']=='yes' ) :
	
									$member_id = get_the_author_meta('ID');
									$out .= '<span class="alignright">' . bp_core_fetch_avatar ( array( 'item_id' => $member_id, 'type' => 'full' ) ) . '</span>';
	
								else :
	
									$out .= '<span class="alignright">' . get_avatar(get_the_author_meta('user_email'),'110') . '</span>';												
	
								endif;

								// Nickname
								$out .= '<span style="background-image:url(' . get_bloginfo('template_url') . '/images/icons/16/led-icons/user.png) !important;" class="icon16">' . get_the_author_meta('nickname') . '</span>';


								$out .= '<div class="h10"><!-- --></div>';


								$user_name = get_the_author_meta('first_name');
								$user_url = get_the_author_meta('user_url');

								if ($user_name || $user_url) :

									$out .= '<p>';
	
										// Name
										$out .= $user_name ? '<span class="f120">' . $user_name . '&nbsp;' . get_the_author_meta('last_name') . '</span><br/>' : '';
	
										// Website
										$out .= $user_url ? '<a target="_blank" href="' . get_bloginfo('template_url') . '/go.php?' . $user_url . '">' . $user_url . '</a>' : '';
	
									$out .= '</p>';

								endif;


								// All publications
								$out .= '<noindex><a rel="nofollow" class="button" href="' . get_author_posts_url(get_the_author_meta('ID')) . '"><span>' . __('All posts by this author','pandathemes') . '</span></a></noindex>';


								$out .= '<div class="clear h10"><!-- --></div>';


								// Author description
								$out .= '<p>' . get_the_author_meta('description') . '</p>';


								echo $out;

							?>
						</td>
					</tr>
				<?php } ?>

			</table>

		</div>
		<!-- end METAS -->
	
		<?php endif; ?>

	</div>

	<?php
	
	endif;

endif;

?>