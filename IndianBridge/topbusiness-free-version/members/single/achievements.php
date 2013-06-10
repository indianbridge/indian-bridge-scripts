<?php

/**
 * BuddyPress - Achievements
 */

	get_header( 'buddypress' );

	// RESET SIDEBAR
	$sidebar = ''; ?>

	<div id="content">

		<div class="padder contentbox w720">

			<?php do_action( 'bp_before_member_home_content' ); ?>

			<div id="item-header">

				<?php dpa_load_template( array( 'members/single/member-header.php' ) ); ?>

			</div><!-- #item-header -->

			<div id="item-nav">

				<div class="item-list-tabs no-ajax" id="object-nav">

					<ul>

						<?php

							bp_get_displayed_user_nav();

							do_action( 'bp_member_options_nav' );

						?>

					</ul>

				</div>

			</div><!-- #item-nav -->

			<div id="item-body">

				<?php

					do_action( 'bp_before_member_body' );

					if ( dpa_is_member_my_achievements_page() ) :
	
						dpa_load_template( array( 'members/single/achievements/unlocked.php' ) );
	
					endif;

					do_action( 'bp_after_member_body' );

				?>

			</div><!-- #item-body -->

			<?php do_action( 'bp_after_member_home_content' ); ?>

		</div><!-- end padder contentbox w620 -->

			<?php
				// SIDEBAR
				include(TEMPLATEPATH.'/inc/sidebar_buddy.php');
			?>

		<div class="clear"><!-- --></div>

	</div><!-- #content -->

<?php get_footer( 'buddypress' ); ?>