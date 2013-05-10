<?php



	function comment_ratings($comment_id) {
		add_comment_meta($comment_id, 'rate', $_POST['rate']);
	}
	add_action('comment_post','comment_ratings');
	
	
	
	function comment_template( $comment, $args, $depth ) {
	
		$GLOBALS['comment'] = $comment;
	
		?>
		<li id="comment-<?php comment_ID(); ?>">
			<?php
	
				$avatar_size = 50;
					if ('0' != $comment->comment_parent)
						$avatar_size = 25;
	
				// COMMENT HEADER
				$out = '<div class="cmt-header">';
					
					// Gravatar
					$out .= get_avatar( $comment, $avatar_size );
	
					// METAS
					$out .= '<div class="cmt-metas div-as-table"><div><div>';
	
						// Rating
						$rate = get_comment_meta($comment->comment_ID, 'rate');
							if ($rate[0] <> '') : $out .= rating($rate[0]); endif;
	
						// Date and Edit link
						$out .= '<span class="cmt-time hide">';
							$out .= get_comment_date().' '.__('at','pandathemes').' '.get_comment_time();
							if (current_user_can('manage_options')) : $out .= ' - <a class="ntd" href="'.get_edit_comment_link().'">Edit</a>'; endif;
						$out .= '</span>';
	
						// Author name
						if ($rate[0] <> '') : $out .= __('by:','pandathemes'); endif;
						$out .= ' <span class="strong" id="author-'.get_comment_ID().'">'.get_comment_author_link().'</span>';
	
						// Reply link
						if (comments_open()) :
							$out .= '<span class="reply hide">';
								$out .= ' &nbsp;<a title="'.get_comment_ID().'" class="button quick-reply" href="/?p='.get_the_ID().'&replytocom='.get_comment_ID().'#respond">'.__('Reply','pandathemes').'</a>';
							$out .= '</span>';
						endif;
	
					$out .= '</div></div></div>';
					
				$out .= '</div>';
	
				// COMMENT TEXT
				$out .= wpautop(get_comment_text());
	
				// Pre-moderation
				if ( $comment->comment_approved == '0' ) : $out .= '<div class="blue_note"><div class="nt">'.__('Your message is awaiting moderation.','pandathemes').'</div></div>'; endif;
				
				$out .= '<div class="clear"><!-- --></div><a href="#" class="cancel-reply cancel-reply-button absolute ntd lh10 f10 none"><!-- --></a><div class="quick-holder"></div>';
	
				echo $out;
	
	}
	
	function rating($grade) {
	
			$out = '';
	
				for ($i = 0; $i < 5; $i++) {
	
					if ($grade > $i)
	
						$out .= '<img src="'.get_bloginfo('template_url').'/images/icons/16/ico_star.png"> ';
	
					else
	
						$out .= '<img src="'.get_bloginfo('template_url').'/images/icons/16/ico_star_off.png"> ';
	
				}
	
			return '<div class="rated-review h17 f11">'.$out.'</div>';
	
	}
	
	function get_average_ratings($id) {
		$comment_array = get_approved_comments($id);
		$count = 1;
	 
		if ($comment_array) :
	
			$i = 0;
			$total = 0;
	
				foreach ($comment_array as $comment) {
					$rate = get_comment_meta($comment->comment_ID, 'rate');
					if (isset($rate[0]) && $rate[0] !== '') {
						$i++;
						$total += $rate[0];
					}
				}
	 
				if ($i == 0)
	
					return false;
	
				else
	
					$final = $total/$i;

					if ($final > 4.5) : $out = '200';
					elseif ($final > 4 && $final <= 4.5) : $out = '180';
					elseif ($final > 3.5 && $final <= 4) : $out = '160';
					elseif ($final > 3 && $final <= 3.5) : $out = '140';
					elseif ($final > 2.5 && $final <= 3) : $out = '120';
					elseif ($final > 2 && $final <= 2.5) : $out = '100';
					elseif ($final > 1.5 && $final <= 2) : $out = '80';
					elseif ($final > 1 && $final <= 1.5) : $out = '60';
					elseif ($final <= 1) : $out = '40';
					else : $out = '0';
					endif;
					
					return $out;
	
		else :
	
			return '0';
	
		endif;
	
	}

?>