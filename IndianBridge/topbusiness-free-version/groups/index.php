<?php

/**
 * BuddyPress - Groups Directory
 */

	get_header( 'buddypress' );

	// GET CUSTOM SIDEBAR
	$sidebar = get_post_meta($post->ID, 'sidebar_value', true);

	do_action( 'bp_before_directory_groups_page' ); ?>

		<div id="content">

			<div class="padder contentbox <?php if ($sidebar == 'No sidebar') : echo'wfull'; else : echo'w720'; endif; ?>">
	
			<?php do_action( 'bp_before_directory_groups' ); ?>
	
			<form action="" method="post" id="groups-directory-form" class="dir-form">
	
				<h3><?php _e( 'Groups Directory', 'buddypress' ); ?><?php if ( is_user_logged_in() && bp_user_can_create_groups() ) : ?> &nbsp;<a class="button" href="<?php echo trailingslashit( bp_get_root_domain() . '/' . bp_get_groups_root_slug() . '/create' ); ?>"><?php _e( 'Create a Group', 'buddypress' ); ?></a><?php endif; ?></h3>
	
				<?php do_action( 'bp_before_directory_groups_content' ); ?>
	
				<div id="group-dir-search" class="dir-search" role="search">
	
					<?php bp_directory_groups_search_form(); ?>
	
				</div><!-- #group-dir-search -->
	
				<?php do_action( 'template_notices' ); ?>

				<div id="item-nav">

					<div class="item-list-tabs" role="navigation">
	
						<ul>
	
							<li class="selected" id="groups-all">
	
								<a href="<?php echo trailingslashit( bp_get_root_domain() . '/' . bp_get_groups_root_slug() ); ?>"><?php printf( __( 'All Groups <span>%s</span>', 'buddypress' ), bp_get_total_group_count() ); ?></a>
	
							</li>
	
							<?php
	
								if ( is_user_logged_in() && bp_get_total_group_count_for_user( bp_loggedin_user_id() ) ) : ?>
	
									<li id="groups-personal">
	
										<a href="<?php echo trailingslashit( bp_loggedin_user_domain() . bp_get_groups_slug() . '/my-groups' ); ?>"><?php printf( __( 'My Groups <span>%s</span>', 'buddypress' ), bp_get_total_group_count_for_user( bp_loggedin_user_id() ) ); ?></a>
	
									</li><?php
	
								endif;
	
								do_action( 'bp_groups_directory_group_filter' );
	
							?>
	
						</ul>
	
					</div><!-- .item-list-tabs -->

				</div>

				<div class="item-list-tabs" id="subnav" role="navigation">

					<ul>

						<?php do_action( 'bp_groups_directory_group_types' ); ?>

						<li id="groups-order-select" class="last filter">

							<label for="groups-order-by"><?php _e( 'Order By:', 'buddypress' ); ?></label>

							<select id="groups-order-by">

								<option value="active"><?php _e( 'Last Active', 'buddypress' ); ?></option>
								<option value="popular"><?php _e( 'Most Members', 'buddypress' ); ?></option>
								<option value="newest"><?php _e( 'Newly Created', 'buddypress' ); ?></option>
								<option value="alphabetical"><?php _e( 'Alphabetical', 'buddypress' ); ?></option>
	
								<?php do_action( 'bp_groups_directory_order_options' ); ?>
	
							</select>

						</li>

					</ul>

				</div>
	
				<div id="groups-dir-list" class="groups dir-list">
	
					<?php locate_template( array( 'groups/groups-loop.php' ), true ); ?>
	
				</div><!-- #groups-dir-list -->
	
				<?php

					do_action( 'bp_directory_groups_content' );
	
					wp_nonce_field( 'directory_groups', '_wpnonce-groups-filter' );
	
					do_action( 'bp_after_directory_groups_content' );

				?>
	
			</form><!-- #groups-directory-form -->
	
			<?php do_action( 'bp_after_directory_groups' ); ?>
	
			</div><!-- end padder contentbox w620 -->

				<?php
					// SIDEBAR
					include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
				?>

			<div class="clear"><!-- --></div>

		</div><!-- #content -->

	</div>

	<?php do_action( 'bp_after_directory_groups_page' );

get_footer( 'buddypress' ); ?>