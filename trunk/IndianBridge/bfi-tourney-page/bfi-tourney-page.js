var bfi_tourney_page = function(){
	var previousCategory = null;
	var tabList = null;
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
	function loadPage_aux(pageID,pageURL, tabIDPrefix,containerIDPrefix, loadingImageURL) {
		var containerID = "#"+containerIDPrefix+pageID;
		// Showing the loading image
		jQuery(containerID).html('<img src="'+loadingImageURL+'"/>');
		// Send the AJAX request	
		jQuery.ajax( {
			url: pageURL,
			type: "GET",
			cache: false,
			error: function(xhr,status,error) {
				jQuery(containerID).html('<h1>Unable to load page</h1><p>'+xhr.status + " " + xhr.statusText+'</p>');
			},
			success: function(html) {
				jQuery(containerID).html(html);
				jQuery('a').on("click", function(event) { 
					var url = jQuery(this).attr('href');
					if (ignoreLink(url)) return true;
					var newURL = qualifyURL(url);
					var index = containsTab(newURL);
					if (index == -1) {
						if (!isRelativeLink(url)) return true;
						newURL = qualifyURL(dirname(pageURL)+url);
						index = containsTab(newURL);
					}				
					if (index != -1) {
						event.preventDefault();
						switchTabs_aux(index+1,newURL, tabIDPrefix,containerIDPrefix, loadingImageURL);
						return false;
					}
					else {
						return true;
					}	

				});				
		}});			
	}
	
	function qualifyURL(url) {
		var el= document.createElement('div');
		el.innerHTML= '<a href="'+url+'">x</a>';
		return el.firstChild.href;
	}	
	
	function ignoreLink(path) {
		if(path.toLowerCase().indexOf("javascript") >= 0) return true;
		return false;	
	}
	
	function isRelativeLink(path) {
		if (path.toLowerCase().indexOf("http") >= 0) return false;
		if(path.toLowerCase().indexOf("https") >= 0) return false;
		if(path.toLowerCase().indexOf("javascript") >= 0) return false;
		return true;
	}
	
	function dirname(path) {
		return path.match( /.*\// );
	}
	
	function containsTab(linkName) {
		link = linkName.toLowerCase();
		for (var i = 0; i < tabList.length; i++) {
			if (link.indexOf(tabList[i].toLowerCase()) != -1) return i;
		}
		return -1;	
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
		loadPage_aux(newCategory,newPageURL,tabIDPrefix,containerIDPrefix,loadingImageURL);
		var selectedClass = 'selected';
		if (previousCategory != null)
			jQuery("#"+tabIDPrefix+previousCategory).removeClass(selectedClass);
		jQuery("#"+tabIDPrefix+newCategory).addClass(selectedClass);
		if (previousCategory != null)
			jQuery("#"+containerIDPrefix+previousCategory).hide();
		jQuery("#"+containerIDPrefix+newCategory).show();
		previousCategory = newCategory;
	}
	
	function saveTabList_aux(tabList_aux) {
		tabList = jQuery.parseJSON(tabList_aux);
	}


	return {
		loadPage: loadPage_aux,
		switchTabs: switchTabs_aux,
		saveTabList: saveTabList_aux
	}
}();
