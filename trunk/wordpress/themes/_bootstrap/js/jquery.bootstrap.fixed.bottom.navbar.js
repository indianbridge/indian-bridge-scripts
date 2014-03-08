/**
 * Fix for Bootstrap fixed navbar overlapping with content.
 * This assumes that header has id footer-menu
 */

( function( $ ) {
	$(window).resize(function () { 
	   $('body').css('padding-bottom', parseInt($('#footer-menu').css("height"))+10);
	});

	$(window).load(function () { 
	   $('body').css('padding-bottom', parseInt($('#footer-menu').css("height"))+10);         
	}); 
} )( jQuery );