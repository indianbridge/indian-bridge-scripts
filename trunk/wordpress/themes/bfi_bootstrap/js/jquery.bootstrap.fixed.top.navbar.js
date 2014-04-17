/**
 * Fix for Bootstrap fixed navbar overlapping with content.
 * This assumes that header has id primary-menu
 */

( function( $ ) {
	$(window).resize(function () { 
	   $('body').css( 'padding-top', parseInt( $('#header-navbar').height() ) + 10 );
	});

	$(window).load(function () { 
	   $('body').css( 'padding-top', parseInt( $('#header-navbar').height() ) + 10 );         
	}); 
} )( jQuery );