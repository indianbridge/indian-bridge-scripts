var wp_ajax_filter_categories = function(){
	var previousCategory = null;
	/**
	 * Loads posts filtered by specified category to specified container.
	 *
	 * @method loadPostsByCategory
	 * @param category {String} the category to use as filter
	 * @param containerIDPrefix {String} the prefix to use to determine the container ID (category will be appended to this)
	 * @param loadingImageURL {String} the image to display while content is loading via AJAX
	 * @param ajaxURL {String} the wordpress AJAX url
	 * @param shortcodeAttributes {Array} the attributes used to format content
	 */
	function loadPostsByCategory_aux(category, containerIDPrefix, loadingImageURL, ajaxURL, shortcodeAttributes) {
		var containerID = "#"+containerIDPrefix+category;
		// Showing the loading image
		jQuery(containerID).html('<img src="'+loadingImageURL+'"/>');
		// Send the AJAX request
		jQuery.ajax({
			type: 'POST',
			url: ajaxURL,
			data: {"action": "load-filter", cat: category, atts: shortcodeAttributes},
			success: function(response) {
				jQuery(containerID).html(response);
				return false;
			}
		});		
	}
	
	/**
	 * Switch between tabs
	 *
	 * @method switchTabs
	 * @param newCategory {String} the category to switch to
	 * @param tabIDPrefix {String} the prefix to use to determine the previous and current tabs
	 * @param containerIDPrefix {String} the prefix to use to determine the previous and current content containers
	 */	
	function switchTabs_aux(newCategory, tabIDPrefix, containerIDPrefix) {
		var selectedClass = 'selected';
		if (previousCategory != null)
			jQuery("#"+tabIDPrefix+previousCategory).removeClass(selectedClass);
		jQuery("#"+tabIDPrefix+newCategory).addClass(selectedClass);
		if (previousCategory != null)
			jQuery("#"+containerIDPrefix+previousCategory).hide();
		jQuery("#"+containerIDPrefix+newCategory).show();
		previousCategory = newCategory;
	}

	return {
		loadPostsByCategory: loadPostsByCategory_aux,
		switchTabs: switchTabs_aux
	}
}();
