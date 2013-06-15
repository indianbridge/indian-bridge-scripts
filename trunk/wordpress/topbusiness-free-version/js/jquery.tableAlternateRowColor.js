var f = jQuery.noConflict();

jQuery(document).ready(function() {
	f('table.stripeme').each(function() {
		f('tbody tr:odd',  this).addClass('odd').removeClass('even').addClass('alt');
		f('tbody tr:even', this).addClass('even').removeClass('odd');
	});
});