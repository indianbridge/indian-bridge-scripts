// by PandaThemes.com

var m = jQuery.noConflict();

m(function(){



// ******************************
//
//		M E N U
//
// ******************************



	// FIX SUB-LEVELS OF DEFAULT MENU
	var def = m('#menu-by-default');

		m('.children',def).addClass('sub-menu');

		m('li',def).addClass('basic');

		m('a',def).removeAttr('title');



	// MENU ROUNDED CORNERS
	m('.basic .sub-menu li:last-child').addClass('li-last');

	m('.basic .sub-menu ul li:first-child').addClass('li-first');

	m('ul.top-menu > li > ul.sub-menu, .megamenu').prev().addClass('a-level');



	/* -- B A S I C - M E G A -------------------------------------------- */

	m('.menubox ul.top-menu li.basic-mega').each(function(){

		m(this).hover(function(){

			var position = m(this).position();
			var left = position.left + 10;

				m(this).addClass('hover-has-ul');
				m('ul:first',this).css({top: '100%', left: -left, opacity: 0}).animate({opacity: 1}, 250);

		}, function(){

			m(this).removeClass('hover-has-ul');
			m('ul:first',this).css({top: -9999});

		})

	});

	m('.menubox ul.top-menu li.basic-mega:has(ul)').find('a:first').append('<span>&nbsp;</span>');

	var bMega = m('.basic-mega');

		bMega.find('.basic').removeClass('basic');
	
		bMega.find('.li-first').removeClass('li-first');
	
		bMega.find('.li-last').removeClass('li-last');
	
		m('.sub-menu',bMega).find('.sub-menu').removeClass('sub-menu');



	/* -- B A S I C -------------------------------------------- */

	m('.menubox li.basic:has(ul)')

		.hover(function(){

			m(this).addClass('hover-has-ul');
	
			var ulFirst = m('ul:first',this);
	
			ulFirst.css({top: '100%', opacity: 0}).animate({opacity: 1}, 250);

		}, function(){

			m(this).removeClass('hover-has-ul');
			m('ul',this).css({top: -9999});

		});

	m('.menubox ul ul li.basic:has(ul)')

		.hover(function(){

			var t = m(this).height();

			m('ul',this).css({margin: '-' + t + 'px 0 0 0'});

		});

	m('.menubox li.basic:has(ul)').find('a:first').append('<span class="arrright">&nbsp;</span>');



	/* -- M E G A M E N U -------------------------------------------- */

	m('.menubox li:has(ul.megamenu)').each(function(){

		m(this).hover(function(){

			var position = m(this).position();
			var left = position.left + 10;

				m(this).addClass('hover-has-ul');
				m('ul:first',this).css({top: '100%', left: -left, opacity: 0}).animate({opacity: 1}, 250);

		}, function(){

			m(this).removeClass('hover-has-ul');
			m('ul:first',this).css({top: -9999});

		})

		m(this).find('a:first').append('<span>&nbsp;</span>');

	});



});