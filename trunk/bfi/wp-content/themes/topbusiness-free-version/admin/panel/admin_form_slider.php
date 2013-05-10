<?php if ( !defined( 'ABSPATH' ) ) : exit; endif;

	$panda_sl = get_option('panda_sl');
	$template_url = get_bloginfo('template_url');

?>
<form action="" enctype="multipart/form-data" method="post" class="themeform">
<input type="hidden" id="panda_sl_action" name="panda_sl_action" value="save">
<div class="icon32" id="icon-themes"><br></div>
<div class="wrap pb14"><h2><?php _e('Theme settings','pandathemes'); ?> / <small><?php _e('Slider','pandathemes'); ?></small> <input type="submit" class="button add-new-h2" value="Update" name="cp_save"/></h2></div>
<input type="hidden" name="tab" value="<?php echo $panda_sl['tab']; ?>" id="input-tab" />

<?php echo pt_get_full_theme() ?>

<div class="adminarea">

    <!-- tabs -->
<!--
    <ul class="tab-navigation">
		<li><a href="#" title="general"><?php _e('General','pandathemes') ?></a></li>
    </ul>
    <div class="clear" style="height:1%; border-top:3px solid #6d6d6d;"></div>
-->
    <div id="tabbed-content">

   <!----------------------------------------------------------------------------
								G E N E R A L   P A G E
	----------------------------------------------------------------------------->
    <div id="general">

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Slider','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Select','pandathemes') ?></span>
				</td>
				<td>
					<?php
	
						// Get a posts
						$loop = new WP_Query(array(
							'post_type'			=> 'slider',
							'posts_per_page'	=> -1,
							'orderby'			=> 'name',
							'order'				=> 'ASC',
							'post_status'		=> 'publish'
							)
						);
	
						if ($loop->have_posts()) :
	
							$out = '<select name="slider">';
	
								while ( $loop->have_posts() ) : $loop->the_post();
								
									$out .= '<option value="'.get_the_ID().'"';
									if ( get_the_ID() == $panda_sl['slider'] ) : $out .= ' selected'; endif;
									$out .= '>'.get_the_title().' &nbsp;</option>';
			
								endwhile;
		
								wp_reset_postdata();
	
							$out .= '</select>';
							$out .= '<small>'.__('Select a slider.','pandathemes').'</small>';
						
						else :
	
							$out = '<div class="clear h5"><!-- --></div><a class="button" href="'.get_bloginfo('siteurl').'/wp-admin/post-new.php?post_type=slider">'.__('Create a first slider','pandathemes').'</a><div class="clear h10"><!-- --></div>';
	
						endif;
	
						echo $out;
	
					?>
				</td>
			</tr></table>

		</fieldset>

		<fieldset class="panda-admin-fieldset"><legend><?php _e('Display on','pandathemes') ?></legend>

			<table><tr>
				<td class="size17">
					<span><?php _e('Pages','pandathemes') ?></span>
				</td>
				<td>

					<div class="h5"><!-- --></div>

					<?php
					
						$arr = array(
							'slider_on_homepage'			=> __("Homepage",'pandathemes'),
							'slider_on_blog'				=> __("Blog",'pandathemes'),
							'slider_on_page'				=> __("Pages",'pandathemes'),
							'slider_on_post'				=> __("Posts",'pandathemes'),
							'slider_on_archives'			=> __("Archives",'pandathemes'),
							'slider_on_product'				=> __("Product",'pandathemes'),
							'slider_on_poducts_archives'	=> __("Products archive",'pandathemes'),
							'slider_on_search'				=> __("Search",'pandathemes'),
							'slider_on_404'					=> 404
							);

						$free = 1;

						foreach ( $arr as $key => $value ) :

							if ($free == 1) :
								$en = '';
								$free++;
							else :
								$en = 'disabled="disabled" title="Available for FULL version only"';
							endif;

							$check = ($panda_sl[$key]=='enable') ? 'checked' : '';

							$out = '
								<table><tr>
									<td class="size3"><input ' . $en . ' type="checkbox" name="'.$key.'" value="enable" '.$check.' /></td>
									<td><span>'.$value.'</span></td>
								</tr></table>

								<div class="h10"><!-- --></div>
							';

							echo $out;
					
						endforeach;

					?>

					<small class="no-p"><?php _e("The slider will display on selected pages.",'pandathemes') ?></small>
					
				</td>
			</tr></table>

		</fieldset>

   	</div><!-- end General -->
  
</div>               
<p class="submit align-center update-settings" style="display:none;"><input type="submit" class="button-primary" value="<?php _e('Update settings','pandathemes') ?>" name="cp_save"/></p>
</div><!-- end adminarea -->
</form>