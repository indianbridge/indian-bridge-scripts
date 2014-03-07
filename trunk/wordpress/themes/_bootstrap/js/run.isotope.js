		// Add isotope js
		//wp_enqueue_script( '_isotope_js', THEME_DIR_URI . '/js/isotope.pkgd.min.js', array('jquery'), '20140219', true );
		wp_enqueue_script( '_packery_js', THEME_DIR_URI . '/js/packery.pkgd.min.js', array('jquery'), '20140219', true );
		wp_enqueue_script( '_masonry_js', THEME_DIR_URI . '/js/masonry.pkgd.min.js', array('jquery'), '20140219', true );
		wp_enqueue_script( '_imagesLoaded_js', THEME_DIR_URI . '/js/imagesloaded.pkgd.min.js', array('jquery'), '20140219', true );
		wp_enqueue_script( '_draggability_js', THEME_DIR_URI . '/js/draggabilly.pkgd.min.js', array('jquery'), '20140219', true );		
		wp_enqueue_script( '_run_masonry_js', THEME_DIR_URI . '/js/run.isotope.js', array('_masonry_js','_imagesLoaded_js'), '20140219', true );

/* For isotope, masonry and packery */
.item { width: 20%; }


( function( $ ) {
	var type = 'packery';
	var draggable = true;
	// or with jQuery
	var $container = $('#articles');
	// initialize Masonry after all images have loaded  
	$container.imagesLoaded( function() {
		if (type === 'masonry') {
		  $container.masonry({
		  	columnWidth: 10,
		  	itemSelector: '.item'
		  });
	  	}
	  	else if (type === 'packery') {
			var container = document.querySelector('#articles');
			var pckry = new Packery( container, {
				itemSelector: '.item',
				gutter: 10
			});
			if (draggable) {
				var itemElems = pckry.getItemElements();
				// for each item...
				for ( var i=0, len = itemElems.length; i < len; i++ ) {
				  var elem = itemElems[i];
				  // make element draggable with Draggabilly
				  var draggie = new Draggabilly( elem );
				  // bind Draggabilly events to Packery
				  pckry.bindDraggabillyEvents( draggie );
				}  
			}
		}	
		else {
			$('#container').isotope({
			  // options
			  itemSelector : '.item',
			  layoutMode : 'masonry'
			});			
			
		}
	});	
} )( jQuery );	