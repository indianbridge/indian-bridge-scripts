<?php
	$page_name = 'header';
	$section_name = '';
	$header_style = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'style' );
	$header_alignment = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'alignment' );
	$header_image = bfi_bootstrap_get_redux_option( $page_name, $section_name, 'image' );
	$header_image = $header_image[ 'url' ];
?>

	<nav id="header-navbar" class="navbar navbar-<?php echo $header_style; ?> navbar-<?php echo $header_alignment; ?>-top" role="navigation">
		
	   <div class="navbar-header">
	      <button type="button" class="navbar-toggle" data-toggle="collapse" 
	         data-target="#example-navbar-collapse">
	         <span class="sr-only">Toggle navigation</span>
	         <span class="icon-bar"></span>
	         <span class="icon-bar"></span>
	         <span class="icon-bar"></span>
	      </button>
	      <?php if ( $header_image ) { ?>
	      <a href="<?php echo esc_url( home_url( '/' ) ); ?>"><img align="middle" class="pull-left" height="50" src=" <?php echo $header_image; ?> "></img></a>
	      <?php } ?>
	      <a class="navbar-brand" href="<?php echo esc_url( home_url( '/' ) ); ?>">Bridge Federation of India</a>
	   </div>
	   <div class="collapse navbar-collapse" id="example-navbar-collapse">
			<?php
			wp_nav_menu( array(
				'menu'           => 'primary',
				'theme_location' => 'primary',
				'depth'          => 0,
				'container'      => 'div',
				'container_class'=> 'collapse navbar-collapse navbar-ex1-collapse',
				'menu_class'     => 'nav navbar-nav pull-right',
				'fallback_cb'=> 'wp_list_pages_bootstrap_navwalker::fallback',
				'walker'         => new wp_bootstrap_navwalker('NO_CARET'),)
			);
			?>
			<ul class="nav navbar-nav">
	         <li class="dropdown">
	            <a href="#" class="dropdown-toggle" data-toggle="dropdown">
	               Logged In as Sriram
	            </a>
	            <ul class="dropdown-menu">
	            <h1>Testing</h1>
	            <a href="www.google.com">Google</a>
	            </ul>
	         </li>
	      </ul>
	   </div>
	</nav>	