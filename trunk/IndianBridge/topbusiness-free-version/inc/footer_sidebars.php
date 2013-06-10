<div id="footerbox">

	<?php

		// DATA
		$var = $panda_sb['footer_sidebars'];


		// None
		if (!$var || $var == 'no') :

			echo '';



//		1/3 + 1/3 + 1/3

		elseif ($var == 1) :

			echo '<div class="fl one_third fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 1'));
			echo '</div></div>';

			echo '<div class="fl one_third fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 2'));
			echo '</div></div>';

			echo '<div class="fl one_third"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 3'));
			echo '</div></div>';
			
			echo '<div class="clear"><!-- --></div>';



//		1/4 + 1/4 + 2/4

		elseif ($var == 2) :

			echo '<div class="fl one_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 1'));
			echo '</div></div>';

			echo '<div class="fl one_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 2'));
			echo '</div></div>';

			echo '<div class="fl two_fourth"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 3'));
			echo '</div></div>';
			
			echo '<div class="clear"><!-- --></div>';



//		1/4 + 1/4 + 2/4

		elseif ($var == 3) :

			echo '<div class="fl one_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 1'));
			echo '</div></div>';

			echo '<div class="fl two_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 2'));
			echo '</div></div>';

			echo '<div class="fl one_fourth"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 3'));
			echo '</div></div>';
			
			echo '<div class="clear"><!-- --></div>';



//		2/4 + 1/4 + 1/4

		elseif ($var == 4) :

			echo '<div class="fl two_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 1'));
			echo '</div></div>';

			echo '<div class="fl one_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 2'));
			echo '</div></div>';

			echo '<div class="fl one_fourth"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 3'));
			echo '</div></div>';
			
			echo '<div class="clear"><!-- --></div>';



//		1/4 + 1/4 + 1/4 + 1/4

		elseif ($var == 5) :

			echo '<div class="fl one_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 1'));
			echo '</div></div>';

			echo '<div class="fl one_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 2'));
			echo '</div></div>';

			echo '<div class="fl one_fourth fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 3'));
			echo '</div></div>';

			echo '<div class="fl one_fourth"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 4'));
			echo '</div></div>';
			
			echo '<div class="clear"><!-- --></div>';



//		2/3 + 1/3

		elseif ($var == 6) :

			echo '<div class="fl two_third fdivider"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 1'));
			echo '</div></div>';

			echo '<div class="fl one_third"><div class="fbox">';
				if (function_exists('dynamic_sidebar') && dynamic_sidebar('Footer Sidebar 2'));
			echo '</div></div>';
			
			echo '<div class="clear"><!-- --></div>';



		endif;

	?>

</div><!-- end footerbox -->