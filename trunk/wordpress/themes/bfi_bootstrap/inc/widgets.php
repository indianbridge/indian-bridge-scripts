<?php

function bfi_bootstrap_widgets_init() {
	$page_name = 'sidebar';
	$container_class = bfi_bootstrap_get_container_options( $page_name );	
	$before_widget = '<aside id="%1$s" class="' . $container_class . ' widget %2$s">';
	$after_widget = '</section></aside>';
	$before_title = '<header class="panel-heading"><h3 class="panel-title widget-title">';
	$after_title = '</h3></header><section class="panel-body">';

	register_sidebar( array(
		'name'          => __( 'Sidebar', 'bfi_bootstrap' ),
		'id'            => 'sidebar-1',
		'description'   => '',
		'before_widget' => $before_widget,
		'after_widget'  => $after_widget,
		'before_title'  => $before_title,
		'after_title'   => $after_title,	
	) );			
	
	bfi_bootstrap_register_footer_widgets();
}
add_action( 'init', 'bfi_bootstrap_widgets_init' );

function bfi_bootstrap_register_footer_widgets() {
	$page_name = 'footer';
	$section_name = 'widgets';	
	$number_of_sidebars = 3;
	$area = 'Footer';
	$container_class = bfi_bootstrap_get_container_options( $page_name . '-' .$section_name );
	$before_widget = '<aside id="%1$s" class="' . $container_class . ' widget %2$s">';
	$after_widget = '</section></aside>';
	$before_title = '<header class="panel-heading"><h3 class="panel-title widget-title">';
	$after_title = '</h3></header><section class="panel-body">';
	for ( $i = 1; $i <= $number_of_sidebars; $i++ ) {
		$id = sprintf( '%s-sidebar-%d', strtolower( $area ), $i );
		register_sidebar( array(
			'name'          => sprintf( __( '%s Sidebar %d', 'bfi_bootstrap' ), $area, $i ),
			'id'            => $id,
			'before_widget' => $before_widget,
			'after_widget'  => $after_widget,
			'before_title'  => $before_title,
			'after_title'   => $after_title,
		) );	
	}	
}

/**
 * Google calendar upcoming events widget
 */
class bfi_google_calendar_upcoming_events extends WP_Widget {

    /** constructor */
    function bfi_google_calendar_upcoming_events() {
        parent::WP_Widget(false, $name = 'Google Calendar Upcoming Events');
    }

    /** @see WP_Widget::widget */
    function widget($args, $instance) {
        extract( $args );
		global $wpdb;

        $title = apply_filters('widget_title', $instance['title']);
        if ( ! $title ) $title = 'Upcoming Events';
		$gravatar = $instance['gravatar'];
		$count = $instance['count'];

		if(!$size)
			$size = 40;

        ?>
              <?php echo $before_widget; ?>
                  <?php if ( $title )
                        echo $before_title . $title . $after_title; ?>
						<?php
							$url = 'https://www.google.com/calendar/feeds/p5ovbitb077i6hg666s41qn0j4%40group.calendar.google.com/public/full?alt=json&orderby=starttime&sortorder=ascending&max-results=5&futureevents=true&singleevents=true';      						
							$response = wp_remote_get( $url );
							//var_dump($response['body']);
							$data = json_decode($response['body']);
							echo $data->feed->entry[0]->title->{'$t'};
						?>                        
              <?php echo $after_widget; ?>
        <?php
    }

    /** @see WP_Widget::update */
    function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['title'] = strip_tags($new_instance['title']);
		$instance['gravatar'] = strip_tags($new_instance['gravatar']);
		$instance['count'] = strip_tags($new_instance['count']);
        return $instance;
    }

    /** @see WP_Widget::form */
    function form($instance) {	

        $title = esc_attr($instance['title']);
		$gravatar = esc_attr($instance['gravatar']);
		$count = esc_attr($instance['count']);

        ?>
         <p>
          <label for="<?php echo $this->get_field_id('title'); ?>"><?php _e('Title:'); ?></label>
          <input class="widefat" id="<?php echo $this->get_field_id('title'); ?>" name="<?php echo $this->get_field_name('title'); ?>" type="text" value="<?php echo $title; ?>" />
        </p>

		<p>
          <input id="<?php echo $this->get_field_id('count'); ?>" name="<?php echo $this->get_field_name('count'); ?>" type="checkbox" value="1" <?php checked( '1', $count ); ?>/>
          <label for="<?php echo $this->get_field_id('count'); ?>"><?php _e('Display Post Count?'); ?></label>
        </p>

		<p>
          <input id="<?php echo $this->get_field_id('gravatar'); ?>" name="<?php echo $this->get_field_name('gravatar'); ?>" type="checkbox" value="1" <?php checked( '1', $gravatar ); ?>/>
          <label for="<?php echo $this->get_field_id('gravatar'); ?>"><?php _e('Display Author Gravatar?'); ?></label>
        </p>

        <?php
    }

} 
add_action('widgets_init', create_function('', 'return register_widget("bfi_google_calendar_upcoming_events");'));

?>