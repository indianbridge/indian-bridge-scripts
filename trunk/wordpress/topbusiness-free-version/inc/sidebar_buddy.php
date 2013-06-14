			<?php

				 // SIDEBAR
				$a = '
					<div class="sidebar-wrapper buddy-sidebar">
						<div class="sidebar">
				';

					$aLeft = '
						<div class="clear"><!-- --></div>
						<div class="fl w50p">
							<div class="side-l">
					';

					$zLeft = '
							</div>
						</div>
					';

					$aRight = '
						<div class="fr w50p">
							<div class="side-r">
					';

					$zRight = '
							</div>
						</div>
						<div class="clear"><!-- --></div>
					';


				$z = '
						</div>
					</div>
				';


				// DISPLAY CUSTOM SIDEBAR
				if ( $sidebar != 'No sidebar' ) :

					if ($sidebar) :

						echo $a;

							// Wide sidebar
							if ( function_exists('dynamic_sidebar') && dynamic_sidebar($sidebar));

						echo $z;

					else :
					
						echo $a;

							// Buddy sidebar
							if ( is_user_logged_in() ) {

								if ( function_exists('dynamic_sidebar') && dynamic_sidebar('Buddy Sidebar - logged in') );

							} else {

								if ( function_exists('dynamic_sidebar') && dynamic_sidebar('Buddy Sidebar - logged out') );

							};

						echo $z;
						
					endif;

				endif;

			?>