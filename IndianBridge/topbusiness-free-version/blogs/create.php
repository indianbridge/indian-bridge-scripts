<?php

/**
 * BuddyPress - Create Blog
 */

	get_header( 'buddypress' );

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true);

	do_action( 'bp_before_directory_blogs_content' ); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">
	
			<?php do_action( 'template_notices' ); ?>
	
				<h3><?php _e( 'Create a Site', 'buddypress' ); ?> &nbsp;<a class="button" href="<?php echo trailingslashit( bp_get_root_domain() . '/' . bp_get_blogs_root_slug() ); ?>"><?php _e( 'Site Directory', 'buddypress' ); ?></a></h3>
	
			<?php

				do_action( 'bp_before_create_blog_content' );

				if ( bp_blog_signup_enabled() ) :

					bp_show_blog_signup_form();

				else: ?>
	
					<div id="message" class="info">
	
						<p><?php _e( 'Site registration is currently disabled', 'buddypress' ); ?></p>
	
					</div><?php

				endif;
	
				do_action( 'bp_after_create_blog_content' );

			?>
	
			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

	<?php do_action( 'bp_after_directory_blogs_content' );

get_footer( 'buddypress' ); ?>