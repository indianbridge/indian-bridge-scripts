			<?php

				 // SIDEBAR
				$a = '
					<div class="sidebar-wrapper">
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
							
							// Left thin sidebar
							echo $aLeft; if (function_exists('dynamic_sidebar') && dynamic_sidebar('Sidebar Left')); echo $zLeft;

							// Right thin sidebar
							echo $aRight; if (function_exists('dynamic_sidebar') && dynamic_sidebar('Sidebar Right')); echo $zRight;

						echo $z;

					else :
					
						echo $a;

							// Wide sidebar
							if (function_exists('dynamic_sidebar') && dynamic_sidebar('Default Sidebar'));

							// Left thin sidebar
							echo $aLeft; if (function_exists('dynamic_sidebar') && dynamic_sidebar('Sidebar Left')); echo $zLeft;

							// Right thin sidebar
							echo $aRight; if (function_exists('dynamic_sidebar') && dynamic_sidebar('Sidebar Right')); echo $zRight;

						echo $z;
						
					endif;

				endif;
			?>