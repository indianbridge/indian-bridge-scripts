<?php
/**
 * The template for displaying Comments.
 *
 * The area of the page that contains both current comments
 * and the comment form.
 *
 * @package _bootstrap
 */

/*
 * If the current post is protected by a password and
 * the visitor has not yet entered the password we will
 * return early without loading the comments.
 */

if ( post_password_required() ) {
	return;
}
$page_name = 'comments';
$options = _bootstrap_get_area_container_options( $page_name );
if ( $options['show'] ) {
?>
	<section id="comments" class="comments-area <?php echo $options['container_class']; ?>">
		<?php if ( have_comments() ) { ?>
			<<?php echo $options['title_tag']; ?> class="comments-title <?php echo $options['title_class']; ?>" >
				<?php
					printf( _nx( 'One comment on &ldquo;%2$s&rdquo;', '%1$s comments on &ldquo;%2$s&rdquo;', get_comments_number(), 'comments title', '_bootstrap' ),
						number_format_i18n( get_comments_number() ), '<span>' . get_the_title() . '</span>' );
				?>
			</<?php echo $options['title_tag']; ?>>

			<?php 
			if ( get_comment_pages_count() > 1 && get_option( 'page_comments' ) ) : // are there comments to navigate through
				_bootstrap_paginate_comments();
			endif; // check for comment navigation 
			?>
			<div>
			<ul class="media-list">
				<?php
					wp_list_comments( array(
						'style'      => 'ul',
						'short_ping' => true,
						'callback' => 'bootstrap_comment',
					) );
				?>
			</ul><!-- .comment-list -->
			</div>
			<?php 
			if ( get_comment_pages_count() > 1 && get_option( 'page_comments' ) ) : // are there comments to navigate through
				_bootstrap_paginate_comments();
			endif; // check for comment navigation 
			?>

		<?php } // have_comments() ?>

		<?php
			// If comments are closed and there are comments, let's leave a little note, shall we?
			if ( ! comments_open() && '0' != get_comments_number() && post_type_supports( get_post_type(), 'comments' ) ) {
		?>
			<p class="no-comments"><?php _e( 'Comments are closed.', '_bootstrap' ); ?></p>
		<?php } ?>

		<?php comment_form(); ?>

	</section>
<?php } ?>
