

/**
 * Fix for Bootstrap fixed navbar overlapping with content.
 *
 */

( function( $ ) {
	$(window).resize(function () { 
	   $('body').css('padding-top', parseInt($('#primary-navbar').css("height"))+10);
	});

	$(window).load(function () { 
	   $('body').css('padding-top', parseInt($('#primary-navbar').css("height"))+10);         
	}); 
} )( jQuery );