<?php


// *****************
//
//		H A L F
//
// *****************


	// Column 1/2
	function one_half( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="one_half '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('one_half', 'one_half');


// *****************
//
//		T H I R D
//
// *****************


	// Column 1/3
	function one_third( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="one_third '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('one_third', 'one_third');


	// Column 2/3
	function two_third( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="two_third '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('two_third', 'two_third');


// *****************
//
//		Q U A R T E
//
// *****************


	// Column 1/4
	function one_fourth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="one_fourth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('one_fourth', 'one_fourth');


	// Column 2/4
	function two_fourth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="two_fourth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('two_fourth', 'two_fourth');


	// Column 3/4
	function three_fourth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="three_fourth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('three_fourth', 'three_fourth');


// *****************
//
//		F I F T H
//
// *****************


	// Column 1/5
	function one_fifth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="one_fifth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('one_fifth', 'one_fifth');


	// Column 2/5
	function two_fifth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="two_fifth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('two_fifth', 'two_fifth');


	// Column 3/5
	function three_fifth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="three_fifth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('three_fifth', 'three_fifth');


	// Column 4/5
	function four_fifth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="four_fifth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('four_fifth', 'four_fifth');



// *****************
//
//		S I X T H
//
// *****************


	// Column 1/6
	function one_sixth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="one_sixth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('one_sixth', 'one_sixth');


	// Column 2/6
	function two_sixth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="two_sixth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('two_sixth', 'two_sixth');


	// Column 3/6
	function three_sixth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="three_sixth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('three_sixth', 'three_sixth');


	// Column 4/6
	function four_sixth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="four_sixth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('four_sixth', 'four_sixth');


	// Column 5/6
	function five_sixth( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="five_sixth '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('five_sixth', 'five_sixth');



// *****************
//
//		E I G H T
//
// *****************



	// Column 1/8
	function one_eight( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="one_eight '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('one_eight', 'one_eight');


	// Column 3/8
	function three_eight( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="three_eight '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('three_eight', 'three_eight');


	// Column 5/8
	function five_eight( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="five_eight '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('five_eight', 'five_eight');


	// Column 7/8
	function seven_eight( $atts, $content = null ) {

		extract(shortcode_atts(array(
			'm' 		=> '25',
			'align'		=> ''
		), $atts));

		return '<div class="seven_eight '.$align.'"><div style="margin:0 '.$m.'px 0 0;">'.do_shortcode($content).'</div></div>'; }
	add_shortcode('seven_eight', 'seven_eight');


?>