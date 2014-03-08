<?php
/**
 * The template to show when no widget has been added to sidebar.
 *
 * @package _bootstrap
 */
$page_name = 'sidebar';
$section_name = 'sidebar';
$options = _bootstrap_get_container_options( $page_name, $section_name );
?>
<div class="widget-area" role="complementary">
<aside id="post-<?php the_ID(); ?>" <?php post_class( $options['container_class'] ); ?> >
	<header class="<?php echo $options['header_class']; ?>" >
		<<?php echo $options['title_tag']; ?> class="<?php echo $options['title_class']; ?>" >
			No Widgets Added to this Sidebar!
		</<?php echo $options['title_tag']; ?>>
	</header>			
	<!-- Show content/excerpt -->
	<section class="<?php echo $options['body_class']; ?>">
		<p>You can either <a href="<?php echo admin_url( 'widgets.php' ); ?>">add some widgets</a> or change the layout in <a href="<?php echo admin_url( 'themes.php' ); ?>">theme options</a> to show a different sidebar or not show a sidebar at all.</p>
	</section>

</aside><!-- #post-## -->
</div><!-- .widget-area  --> 
