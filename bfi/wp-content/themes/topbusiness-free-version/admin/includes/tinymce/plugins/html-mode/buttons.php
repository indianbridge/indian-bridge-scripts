	<script type="text/javascript">



		// Line
		QTags.addButton( 'p_line', '&mdash;', '[line]');



		// Raw
		QTags.addButton( 'p_raw', 'raw', '[raw] ', ' [/raw]' );



		// Clear
		QTags.addButton( 'p_clear', 'clear', prompt_user );
		
			function prompt_user(e, c, ed) {
				prmt = prompt('Optional: besides preventing the float of objects you can set a vertical indent. Enter a number of pixels e.g. 50');
				if ( prmt === null ) return;
	
				//rtrn = '[clear h="' + prmt + '"]';
				if (prmt)
					{ rtrn = '[clear h="' + prmt + '"]' }
				else
					{ rtrn = '[clear]' };
	
				this.tagStart = rtrn;
				QTags.TagButton.prototype.callback.call(this, e, c, ed);
			};



		// Toggle
		QTags.addButton( 'p_toggle', 'toggle', toggle_prompt );
		function toggle_prompt(e, c, ed) {

			prmt = prompt('Enter the toggle title');
			if ( prmt === null ) return;

			this.tagStart = '[toggle title="' + prmt + '"] ';
			this.tagEnd = ' [/toggle]';
			QTags.TagButton.prototype.callback.call(this, e, c, ed);
		};



		// Accordion
		QTags.addButton( 'p_accordion', 'accordion', '[accordion] ', ' [/accordion]' );



		// Fieldset
		QTags.addButton( 'p_fieldset', 'fieldset', fieldset_prompt );
		function fieldset_prompt(e, c, ed) {

			prmt = prompt('Enter the fieldset legend');
			if ( prmt === null ) return;

			this.tagStart = '[fieldset legend="' + prmt + '"] ';
			this.tagEnd = ' [/fieldset]';
			QTags.TagButton.prototype.callback.call(this, e, c, ed);
		};



		// Tooltip
		QTags.addButton( 'p_tooltip', 'tooltip', tooltip_prompt );
		function tooltip_prompt(e, c, ed) {

			prmt = prompt('Enter the tooltip text');
			if ( prmt === null ) return;

			this.tagStart = '<span class="tooltip-t" title="' + prmt + '">';
			this.tagEnd = '</span>';
			QTags.TagButton.prototype.callback.call(this, e, c, ed);
		};



		// 1/2
		QTags.addButton( 'p_1-2', '1/2', '[one_half] ', ' [/one_half]' );

		// 1/3
		QTags.addButton( 'p_1-3', '1/3', '[one_third] ', ' [/one_third]' );

		// 2/3
		QTags.addButton( 'p_2-3', '2/3', '[two_third] ', ' [/two_third]' );

		// 1/4
		QTags.addButton( 'p_1-4', '1/4', '[one_fourth] ', ' [/one_fourth]' );

		// 3/4
		QTags.addButton( 'p_3-4', '3/4', '[three_fourth] ', ' [/three_fourth]' );

		// 1/5
		QTags.addButton( 'p_1-5', '1/5', '[one_fifth] ', ' [/one_fifth]' );

		// 2/5
		QTags.addButton( 'p_2-5', '2/5', '[two_fifth] ', ' [/two_fifth]' );

		// 3/5
		QTags.addButton( 'p_3-5', '3/5', '[three_fifth] ', ' [/three_fifth]' );

		// 4/5
		QTags.addButton( 'p_4-5', '4/5', '[four_fifth] ', ' [/four_fifth]' );

		// 1/6
		QTags.addButton( 'p_1-6', '1/6', '[one_sixth] ', ' [/one_sixth]' );

		// 5/6
		QTags.addButton( 'p_5-6', '5/6', '[five_sixth] ', ' [/five_sixth]' );



	</script>