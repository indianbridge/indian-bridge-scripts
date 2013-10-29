<?php

	if ( ! function_exists( 'pandathemes_comment' ) ) :
	
		function pandathemes_comment( $comment, $args, $depth ) {
	
			$GLOBALS['comment'] = $comment;
	
			switch ( $comment->comment_type ) :
	
				case 'comment' :
		
					global $theme_options; ?>
			
					<li id="comment-<?php comment_ID(); ?>">
						<?php
				
							$avatar_size = ('0' != $comment->comment_parent) ? 25 : 50;

							$top_user = ('0' == $comment->comment_parent) ? ' class="top-level-comment"' : ' class="low-level-comment"';

							// COMMENT HEADER
							$out = '<div class="cmt-header">';
								
								// Gravatar
								$out .= $theme_options['gravatars_on_comments'] == 'yes' ? get_avatar( $comment, $avatar_size ) : '';
								
				
								// METAS
								$out .= $theme_options['gravatars_on_comments'] == 'yes' ? '<div class="cmt-metas div-as-table"><div><div>' : '<div class="cmt-metas cmt-metas-no-gravatar div-as-table"><div><div>';
				
									// Date and Edit link
									$out .= '<span class="cmt-time">';
										$out .= $theme_options['dates_on_comments'] == 'yes' ? get_comment_date().' '.__('at','pandathemes').' '.get_comment_time() : '';
										if (current_user_can('manage_options')) : $out .= ' - <a class="ntd" href="'.get_edit_comment_link().'">'.__('Edit','pandathemes').'</a>'; endif;
									$out .= '</span>';
				
									// Author name
									$out .= '<span'.$top_user.' id="author-'.get_comment_ID().'">'.get_comment_author_link().'</span>';
				
									// Reply link
									if (comments_open() && is_user_logged_in()) :
										$out .= '<br/><span class="reply">';
											$out .= ' &nbsp;<a title="'.get_comment_ID().'" class="button quick-reply" href="/?p='.get_the_ID().'&replytocom='.get_comment_ID().'#respond">'.__('Reply','pandathemes').'</a>';
										$out .= '</span>';
									endif;
									
								$out .= '</div></div></div>';
								
							$out .= '</div>';
				
							// COMMENT TEXT
							$raw_comment_text = apply_filters('the_content', do_shortcode(make_clickable(convert_smilies(wpautop(get_comment_text())))));
							$out .= wpautop($raw_comment_text);
				
							// Pre-moderation
							if ( $comment->comment_approved == '0' ) : $out .= '<p><em class="comment-awaiting-moderation">'.__('Your comment is awaiting moderation.','pandathemes').'</em></p>'; endif;
							
							$out .= '<div class="clear"><!-- --></div><a href="#" class="cancel-reply cancel-reply-button absolute none"><!-- --></a><div class="quick-holder"></div>';
				
							echo $out;
			
				break;
		
			endswitch;
		
		}
	
	endif; // ends check for pandathemes_comment()

?>