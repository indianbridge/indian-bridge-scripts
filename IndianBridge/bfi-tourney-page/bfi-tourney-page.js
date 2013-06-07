var bfi_tourney_page = function(){
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
	function loadPage_aux(pageID,pageURL, containerIDPrefix, loadingImageURL) {
		var containerID = "#"+containerIDPrefix+pageID;
		// Showing the loading image
		jQuery(containerID).html('<img src="'+loadingImageURL+'"/>');
		// Send the AJAX request
		jQuery(containerID).load(pageURL, function(response, status, xhr) {
			if (status == "error") {
				jQuery(containerID).html('<h1>Unable to load page</h1><p>'+xhr.status + " " + xhr.statusText+'</p>');
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
	function switchTabs_aux(newCategory, newPageURL, tabIDPrefix, containerIDPrefix, loadingImageURL) {
		loadPage_aux(newCategory,newPageURL,containerIDPrefix,loadingImageURL);
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
		loadPage: loadPage_aux,
		switchTabs: switchTabs_aux
	}
}();
