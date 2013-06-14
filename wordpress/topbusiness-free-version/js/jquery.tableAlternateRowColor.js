var f = jQuery.noConflict();

jQuery(document).ready(function() {
	f('table.bfi').each(function() {
		f('tr:odd',  this).addClass('odd').removeClass('even');
		f('tr:even', this).addClass('even').removeClass('odd');
	});
});