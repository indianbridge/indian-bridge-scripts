// by PandaThemes.com



// ******************************
//
//		B Y   R E A D Y
//
// ******************************



var f = jQuery.noConflict();

jQuery(document).ready(function() {




	/* -- P A N D A   S L I D E R -------------------------------------------- */

			/*
				TRANSITIONS
				ptd		- Default
				ptf		- Fade
				ptfv	- Fade vertical
				ptv		- Slide vertical
				pth		- Slide horizontal
				pto		- Overlay vertical
			*/




	f('.pslider').each(function(){

		var
			parent = f(this),
			pslides = parent.find('.pslides'),
			timeDelay = parent.attr('title');

		parent.removeAttr('title');




		// ACTIVATION

			// Common
			var h = f('.pslides > div:first',this).innerHeight(true);

				f('.ploading',this).hide();
				pslides.css({height: 50}).delay(300).animate({height: h}, 750);

			// First slide for the group of transitions
			var pttransition = [
				"ptd",
				"ptv",
				"pth",
				"pto"
			];

				for (var i in pttransition) {
					if (parent.hasClass(pttransition[i])) {
						f('.pslides > div:first',parent).addClass('block');
					};
				};

			// First slide for the Fade group of transitions
			var pttransition = [
				"ptf",
				"ptfv"
			];

				for (var i in pttransition) {
					if (parent.hasClass(pttransition[i])) {
						f('.pslides > div',this).css({opacity:0});
						f('.pslides > div:first',this).addClass('block').animate({opacity:1}, 750);
					};
				};




		// AUTOPLAY

		f(function() {
		
			if (timeDelay) {
	
				setInterval(function() {
		
					checkHover();
		
				}, timeDelay);
	
			}

	  })




		// CHECK MOUSEHOVER

		var checkCount = 0;

		function checkHover() {
			
			checkCount++;

			if ( parent.hasClass('autopause') || !checkCount ) {

				f(this).delay(timeDelay,function() { checkHover() })

			} else {

				next()

			};

		}




		// AUTOPAUSE

		parent.hover(

			function() {
				f(this).addClass('autopause');
			},

			function() {
				f(this).removeClass('autopause');
			}

		)




		// NEXT AUTO

		function next() {

			// Get a slides
			var currentSlide = parent.find('.pslides > div.block');
			nextSlide = currentSlide.next().length ? currentSlide.next() : parent.find('.pslides').children().eq(0);

			// Get the tabs
			var currentTab = parent.find('.pscurrent');
			var nextTab = currentTab.next().next().length ? currentTab.next().next() : parent.find('.pnav span:first');

				// Select the tabs
				currentTab.removeClass('pscurrent');
				nextTab.addClass('pscurrent');


			// T r a n s i t i o n s

				// Transition Default
				if ( parent.hasClass('ptd') ) {

					// Hide active slide
					currentSlide.removeClass('block');

					// Show new slide
					nextSlide.addClass('block');

					placeholderHeight();

				}

				// Transition Fade
				if ( parent.hasClass('ptf') ) {

					// Hide active slide
					currentSlide.animate({opacity: 0}, 750, function(){ f(this).css({left: '-9999px'}).removeClass('block'); placeholderHeight(); });

					// Show new slide
					nextSlide.addClass('block').css({opacity: 0, left: 0}).animate({opacity: 1}, 750);
	
				}

				// Transition Fade vertical
				if ( parent.hasClass('ptfv') ) {

					// Hide active slide
					currentSlide.css({zIndex: 2}).animate({opacity: 0, top: 50}, 750, function(){ f(this).css({zIndex: 1, left: '-9999px'}).removeClass('block'); placeholderHeight(); });

					// Show new slide
					nextSlide.addClass('block').css({opacity: 0, top: 25, left: 0}).animate({opacity: 1, top: 0}, 750);

				}

				// Transition Vertical
				if ( parent.hasClass('ptv') ) {

					h = currentSlide.height();

					// Hide active slide
					currentSlide.css({zIndex: 1}).animate({top: -h/20, opacity: 0}, 750, function(){ f(this).css({top: 0, opacity: 1, left: '-9999px'}).removeClass('block'); });

					// Show new slide
					nextSlide.addClass('block').css({zIndex: 2, top: h, left: 0}).animate({top: 0}, 750);

					// Change the height of placeholder
					placeholderHeight();

				}

				// Transition Horizontal
				if ( parent.hasClass('pth') ) {

					w = f('.pslides',parent).width();

					// Hide active slide
					currentSlide.css({zIndex: 1}).animate({left: -w/2, opacity: 0}, 750, function(){ f(this).css({left: 0, opacity: 1, left: '-9999px'}).removeClass('block') });

					// Show new slide
					nextSlide.addClass('block').css({zIndex: 2, opacity: 0, left: w/6}).animate({left: 0, opacity: 1}, 750, function(){ placeholderHeight(); });

				}

				// Transition Overlay
				if ( parent.hasClass('pto') ) {

					var h = currentSlide.height();
					var newH = nextSlide.height();

					// Hide active slide
					currentSlide.css({zIndex: 1}).animate({height: 0}, 750, function(){ currentSlide.css({height: h, left: '-9999px'}).removeClass('block') });

					// Show new slide
					nextSlide.addClass('block').css({zIndex: 2, height: 0, left: 0}).animate({height: newH}, 750, function(){ placeholderHeight(); });

				}


			// Change placeholder height
			function placeholderHeight() {

				newH = nextSlide.height();
				f('.pslides',parent).animate({height: newH}, 750);

			}


		}




		// AUTO TABS - for [slider] shortcode

		f('.pnav').each(function(){

			var q = f('div div',this).html();
			var pnav = f(this);

			if ( !q ) {

				function tabqty(){
				
					var tabqty = f(pnav).parent().parent().find('.pslides > div').size();

					var tabs = '';

					var i = 1;

					for ( i = 1; i <= tabqty; i++ )	{

						if ( i == 1 )

							tabs += '<span class="pscurrent">' + i + '</span>';

						else

							tabs += '<em><!-- divider --></em><span>' + i + '</span>';

					};

					f('div div',pnav).html(tabs);

				};
				
				tabqty();

			}

		});




		// BY CLICK

		f('.pnav span',this).click(function(){

			span = f(this);

				
				if ( ! span.hasClass('pscurrent')) {

					// Select the tab
					span.parent().children('.pscurrent').removeClass('pscurrent');
					span.addClass('pscurrent');

					spanIndex = span.prevAll('span').length;
					newSlide = f('.pslides',parent).children().eq(spanIndex);
					newH = newSlide.height();

					// Hide active slide
					f('.pslides',parent).children('.block').animate({opacity: 0, left: 0}, 300, function(){ f(this).css({opacity: 1, left: '-9999px'}).removeClass('block'); });

					// Show new slide
					newSlide.addClass('block').css({opacity: 0, top: 0, left: 0}).animate({opacity: 1}, 200);

					placeholderH();

				}


				// Change placeholder height
				function placeholderH() {

					var newH = newSlide.height();
					f('.pslides',parent).animate({height: newH}, 750);

				}


		});



	})

	/* -- e n d   P A N D A   S L I D E R -------------------------------------------- */



	// AUTOMATIC PRETTYPHOTO FOR IMAGES
	f('a').each(function(){

		var rel = f(this).attr('rel');

		if (rel != 'prettyPhoto[gallery]') {

			var url = f(this).attr('href');

			if (typeof url != "undefined" && url.match(/\b(jpg|jpeg|png|gif)\b/gi)) {

				f(this).attr('rel','prettyPhoto');

			};

		}

	});



	// PRETTYPHOTO FOR FLICKR
	f('.flickr_badge_image a').attr({

		rel: 'prettyPhoto[flickr]',
		href: function() {

			var src = f(this).find('img').attr('src');

			var href = src.replace(/_s.jpg/g,'.jpg');

			return href;

		}

	});



	// PRETTYPHOTO ACTIVATION
	f("a[rel^='prettyPhoto']").prettyPhoto();



	// FIX COMMENT FORM ON IE7
	if (f.browser.msie && parseInt(f.browser.version, 10) == 7) {
		f('.ktabs > div, #commentform').css({display: 'inline-block'});
	};



});




// ******************************
//
// 		C O M M O N
//
// ******************************



var p = jQuery.noConflict();

p(function(){



	// RESPONSIVE DROP-DOWN MENU
	p("#selectElement").change(function() {

		if ( p(this).val() ) {
			window.open( p(this).val(), '_parent' );
		}

	});



	// DEFAULT RESPONSIVE DROP-DOWN MENU
	p("#menuwrapper #page_id").change(function() {

		var val = p(this).val();

			if (val) {
				p(this).parent().submit();
			}

	});



	// FRONTPAGE SLIDER HEIGHT, LOOP
	var sid = setInterval(homeSliderHeight, 2750);
	
	function homeSliderHeight() {

		var	contentHeight = p('.pslides .block').innerHeight(true);

			p('#homeslider .pslides').animate({'height':contentHeight}, 750);
	
	}



	// FIX IFRAME HEIGHT BY AUTO, LOOP
	var sid = setInterval(iframeHeight, 4900);
	
	function iframeHeight() {
		
		f('.activity-inner p > iframe,#prd-custom-tab > iframe').each(function(){
			f(this).wrap('<div class="video-container" />');
		});

	}



	// HIDE A LINK TITLES
	p('.hidetitle')

		.mouseenter(function(){
							 
			var t = p(this).attr("title");

			p(this).removeAttr('title');

			p(this)

				.mouseleave(function(){

					p(this).attr('title', t);

				})
				
				.mousedown(function(){

					p(this).attr('title', t);

				})

		});



	// TOOLTIPS
	p('.tooltip-t').mouseenter(function(){

		var tt_title = p(this).attr("title");

		if (tt_title) {

			var tt_id = 'tt_id' + Math.floor(Math.random()*999);

			p(this).attr('title','');
			p("body").append('<div id="' + tt_id + '" class="tt_wrap"><div class="ttbox"><span>' + tt_title + '</span></div></div>');

			offset = p(this).offset();
			width = p(this).outerWidth() / 2;
			tt_left = offset.left - 90 + width;
			tt_top = offset.top - 250;
			tt_wrap_id = '#' + tt_id;

			var tt_box_id = tt_wrap_id + '> div';

			p(tt_wrap_id).css({left: tt_left, top: tt_top});
			p(tt_box_id).css({bottom:15, opacity:0}).animate({bottom:5, opacity:1}, 300);

		}

		p(this).mouseleave(function(){

			p(tt_box_id).stop().animate({bottom:30, opacity:0}, 300, rem);

				function rem() {
					p(this).parent().remove();
				}

			p(this).attr('title',tt_title);

		})

	})



	// TABS
	p('.kul li').each(function(){
		
		p(this).click(function(){

			var li = p(this);

				li.parent().children('.kcurrent').removeClass('kcurrent');
				li.addClass('kcurrent');

			var
				liIndex = li.prevAll().length,
				ktabs = li.parent().next('.ktabs');

				ktabs.children('.block').removeClass('block');
				ktabs.children().eq(liIndex).css({opacity: 0}).addClass('block').animate({opacity: 1}, 300);

		})

	});



		// Tabs auto-activation
		p('.kul-auto li:first-child').addClass('kcurrent');
		p('.ktabs-auto div:first-child').addClass('block');


		// Current tab on comments
		var cmt = p('#comments');
			cmt.children().eq(0).addClass('kcurrent');
			cmt.next('.ktabs').children().eq(0).addClass('block');


		// Comments qty on tabs
		var cm = p('#cmt-qty').html();
			if (cm > 0) { cmt.children('#cmt').append('<span>' + cm + '</span>').css({'position':'relative'}); } // Relative for Chrome


		// Tracbacks qty on tabs
		var tr = p('#trb-qty').html();
			if (tr > 0) { cmt.children('#trb').append('<span>' + tr + '</span>').removeClass('none').css({'position':'relative'}); } // Relative for Chrome


		// Calculate the qty of images
		var q = p('.product-image').size();
			if (q) { p('#images-data-tab').append('<span>' + q + '</span>').css({'position':'relative'}) }


		// Calculate the qty of reviews
		var q = p('.rated-review').size();
			if (q) { p('#tab-reviews, #reviews-total').append('<span>' + q + '</span>').css({'position':'relative'}) }


		// Open the Reviews tab
		p('#reviews-total').click(function(){
			p('.kul li').removeClass('kcurrent').parent().find('#tab-reviews').addClass('kcurrent');
			p('.ktabs > div').removeClass('block');
			p('.ktabs > div:last-child').addClass('block');
		})


		// Display filled Specs & Images tabs
		var id = [
			"#specs-data",
			"#images-data"
		];

		for (var i in id) {

			var spec = p(id[i]).html();

			if ( spec ) { p(id[i]+'-tab').removeClass('none'); };

		};



	// RATING
	p('#rstars div').mouseenter(function(){

		var div = p(this);

		starIndex = div.prevAll().length + 1;

		var pos = 40 * starIndex;

		div.parent().css('background-position','0 -'+pos+'px');

	}).mouseleave(function(){

		p('#rstars').removeAttr('style');

	}).click(function(){

		p(this).parent().removeAttr('class').addClass('star-'+starIndex);
		p('#rate').val(starIndex);

	});



	// QUICK REPLY FORM
	var	hForm_a = p('#commentform-a').outerHeight(true) + 15 + 25 + 25; // paddings & margin

	p('a.quick-reply').click(function(){

		var
			id = p(this).attr('title'),
			form = p('#commentform-a'),
			author = p('#author-'+id).html();

			form.find('#to-author').html(author);

			form.removeClass('hidden absolute');

		//	form.remove();

			p('.cancel-reply').addClass('none');

			// Hide major form
			p('#commentform').stop(true, false).animate({height: 0, opacity: 0}, 500, function(){
															p(this).addClass('hidden absolute').css({height: 'auto'});
															p('#review-label').removeClass('none');
														});

			// Close all .quick-holder's
			p('.quick-holder').stop(true, false).animate({height: 0}, 500);

			// Put the form to the holder
			p('#comment-'+id).find('.quick-holder:eq(0)').append(form).animate({height: hForm_a}, 500, function(){
																											p(this).css({height: 'auto'});
																											p('#comment-'+id).find('.cancel-reply:eq(0)').removeClass('none');
																										});

			// Set an ID for hidden field
			p('#comment_parent').val(id);

		return false;

	})



		// Cancel reply
		p('.cancel-reply').click(function(){

			var	hForm = p('#commentform').outerHeight(true);

			// Display major form
			p('#commentform').removeClass('hidden absolute').stop(true, false).animate({height: hForm, opacity: 1}, 1000, function(){ p(this).css({height: 'auto'}) });

			p('.cancel-reply').addClass('none');
			p('#commentform-a').addClass('hidden absolute');

			// Close all .quick-holder's
			p('.quick-holder').css({height: 0});

			return false;

		})


		// Check the Website input field
		p('input[type="submit"]').mousedown(function(){

			var url = p(this).parent().parent().parent().find('input[name="url"]');
			var lnk = url.val();

			if ( lnk == 'Website' ) { url.val('') ;}

			return true;

		})



	// FIX THE HEIGHT OF THE POSTS ON PRODUCT ARCHIVE
	p('.prdcts').each(function(){

		// Titles
		var maxH = 0;

			p('h3',this).each(function(){

				var h = p(this).height();

					if (h > maxH) { maxH = h }

			})

			p('h3',this).css('height',maxH);

		// Excerpts
		var maxH = 0;

			p('.prdcts-excerpt .t1-desc',this).each(function(){

				var h = p(this).height();

					if (h > maxH) { maxH = h }

			})

		p('.prdcts-excerpt .t1-desc',this).css('height',maxH);

		// Excerpt placeholder
		var maxH = 0;

			p('.prdcts-excerpt',this).each(function(){

				var h = p(this).height();

					if (h > maxH) { maxH = h }

			})

		p('.prdcts-excerpt',this).css('height',maxH);

	})



	// HOVER ON PRODUCTS ARCHIVE & PRODUCT PAGE

		// Template 1
		p('.t1-thumb, .product-image')
	
			.mouseenter(function(){
	
				var
					zoom = p('div:eq(0)',this),
					width = p(this).width(),
					height = p(this).height(),
					top = (height - 100)/2,
					left = (width - 100)/2,
					t = (height - 50)/2,
					l = (width - 5)/2;
	
				zoom.stop(true, false).css({top: t, left: l}).animate({width: '100px', height: '100px', top: top, left: left}, 300);
	
				p(this).mouseleave(function(){
	
					zoom.stop(true, false).animate({width: 0, height: 0, top: t, left: l}, 200);
	
				});
	
			});



	// LEARN MORE HOVER
	p('.learn-more-hover')

		.mouseenter(function(){

			var
				more = p('span:eq(0)',this),
				w = more.outerWidth(true),
				h = more.outerHeight(true),
				width = p(this).outerWidth(true),
				height = p(this).outerHeight(true),
				top = (height - h)/2,
				left = (width - w)/2;

			more.css({top: top, left: left}).stop(true, false).animate({opacity: 1}, 300);

			p(this).mouseleave(function(){

				more.stop(true, false).animate({opacity: 0}, 200);

			});

		});



	// FIX THE HEIGHT OF THE POSTS SHORTCODE
/*	p('.grid-auto').each(function(){

		var maxH = 0;

			p('.posts-post',this).each(function(){

				var h = p(this).height();

					if (h > maxH) { maxH = h }

			})

		p('.posts-post > div',this).css('height',maxH);

	})
*/


	// TOGGLE + ACCORDION
	p('.toggle .ta, .toggle .t').click(function() {

		var parent = p(this).parent();

		if ( parent.parent().hasClass('accordion') ) {

			var accordion = parent.parent();

		};

		arrow = parent.find('.t div');
		title = p(this);
		box = p(this).parent().find('.tb');
		height = box.find('.tc').outerHeight(true);

		if ( ! parent.hasClass('tclicked')) {

			// For accordion
			if (accordion) {

				accordion

					.find('.tclicked').removeClass('tclicked')

					.find('.tb').stop(true, false).animate({height:0}, 300)

					.parent().find('.t div').stop(true, false).animate({top: '0'});

			};

			// For toggle
			parent.addClass('tclicked');

			box.stop(true, false).animate({height:height}, 300, function(){

				arrow.stop(true, false).animate({top: '-26px'});

			});

		}

		else {

			// For accordion
			if (accordion) {

				accordion

					.find('.tclicked').removeClass('tclicked')

					.find('.tb').stop(true, false).animate({height:0}, 300)

					.parent().find('.t div').stop(true, false).animate({top: '0'});

			};

			// For toggle
			parent.removeClass('tclicked');

			box.stop(true, false).animate({height:0}, 300, function(){

				arrow.stop(true, false).animate({top: '0'});

			});

		}

	});



		// Toggle auto-activation
		p('.toggle-auto').each(function(){

			var
				toggle = p(this),
				height = toggle.find('.tc').outerHeight(true);

				toggle

					.addClass('tclicked')

					.find('.tb').stop(true, false).animate({height: height}, 300)

					.parent().find('.t div').css({top: '-26px'});

		});



	// SIDEBAR HEIGHT LOOP
	var tid = setInterval(sbarHeight, 3799);
	
	function sbarHeight() {

		p('#content .sidebar-wrapper').css('height','auto');

		var layoutWidth = p('#layout').outerWidth(true);

		if (layoutWidth > 767) {

			var
				contentHeight = p('.contentbox').outerHeight(true),
				sidebarHeight = p('#content .sidebar-wrapper').outerHeight(true);

				if ( contentHeight != sidebarHeight && contentHeight > sidebarHeight ) { p('#content .sidebar-wrapper').css('height',contentHeight - 30); }

		} else {

			p('#content .sidebar-wrapper').css('height','auto');

		}

	}



	// ENABLE SUBMIT BUTTON for buddy forums & registration page
	p('#topic_group_id, #signup_confirm').click(function(){

		p('input[name="submit_topic"]').removeClass('none').animate({opacity: 1}, 500);
		p('input[name="signup_submit"]').removeClass('none').css({opacity: 0}).animate({opacity: 1}, 500);

	});



	// ANIMATED ICONS 60
	p('.icon60-link')

		.mouseenter(function(){

			var
				bg_hover = p(this).attr('bg_hover'),
				color_hover = p(this).attr('color_hover'),
				bg_ico_hover = p(this).attr('bg_ico_hover');

			// Move an icon
			p('tr > td em',this).stop(true, false).animate({top: 60}, 150, function(){
																				p(this).css({top: -60}).animate({top: 0}, 150);
																			});
			// Background of icon
			p('.i60',this).stop(true, false).animate({
						backgroundColor: '#' + bg_ico_hover
						}, 350);

			// Background of placeholder
			p(this).stop(true, false).animate({
						backgroundColor: '#' + bg_hover,
						color: '#' + color_hover
						}, 350);

		})
	
		.mouseleave(function(){

			var
				bg = p(this).attr('bg'),
				color = p(this).attr('color'),
				bg_ico = p(this).attr('bg_ico');

			// Background of icon
			p('.i60',this).stop(true, false).animate({
						backgroundColor: '#' + bg_ico
						}, 250);

			// Background of placeholder
			p(this).stop(true, false).animate({
						backgroundColor: '#' + bg,
						color: '#' + color
						}, 250);

		});



	// ANIMATE TEXT AREA BY FOCUS
	p('textarea').not('#whats-new, .ac-input').focus(function() {
		if ( p(this).height() < 151 && ! p(this).hasClass('height-ready') ) {
			p(this).css({height: 70}).animate({height: 150}, 300, function(){ p(this).addClass('height-ready'); });
		}
	});



});