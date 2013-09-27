<?php

/**
 * Class to customize user profile
 */

if (!class_exists('BFI_Custom_User_Profile')) {

	class BFI_Custom_User_Profile {
		private $bfi_table_prefix = '';
		private $bfi_db;
		private $bfi_member_table_fieldNames;
		public function __construct($db, $table_prefix) {
			$this -> bfi_db = $db;
			$this -> bfi_table_prefix = $table_prefix;
			$this -> bfi_member_table_fieldNames = array('address_1', 'address_2', 'address_3', 'city', 'country', 'residence_phone', 'mobile_no', 'sex', 'dob');
			add_action('show_user_profile', array($this, 'bfi_add_custom_user_profile_fields'));
			add_action('edit_user_profile', array($this, 'bfi_add_custom_user_profile_fields'));
			add_action('personal_options_update', array($this, 'bfi_save_custom_user_profile_fields'));
			add_action('edit_user_profile_update', array($this, 'bfi_save_custom_user_profile_fields'));
			add_filter('user_contactmethods', array($this, 'remove_contactmethods'), 10, 1);
		}

		function remove_contactmethods($contactmethods) {
			unset($contactmethods['aim']);
			unset($contactmethods['jabber']);
			unset($contactmethods['yim']);
			return $contactmethods;
		}

		function bfi_save_custom_user_profile_fields($user_id) {
			if (!current_user_can('edit_user', $user_id))
				return FALSE;

			$user_info = get_userdata($user_id);
			$member_id = $user_info -> user_login;
			$query = "UPDATE " . $this -> bfi_table_prefix . "member SET ";
			$updateFields = array();
			$updateFields[] = "first_name='$user_info->user_firstname'";
			$updateFields[] = "last_name='$user_info->user_lastname'";
			foreach ($this->bfi_member_table_fieldNames as $fieldName) {
				$updateFields[] = "$fieldName='$_POST[$fieldName]'";
			}
			$query .= implode(',', $updateFields);
			$query .= " WHERE member_id=%s";
			$rows = $this -> bfi_db -> query($this -> bfi_db -> prepare($query, $member_id));
			// Dont update password since masterpoint valid table uses a blob for pass.
		}

		function bfi_add_custom_user_profile_fields($user) {
			$query = "SELECT * FROM " . $this -> bfi_table_prefix . "member WHERE member_id=%s";
			$member_id = $user -> user_login;
			$row = $this -> bfi_db -> get_row($this -> bfi_db -> prepare($query, $member_id));
			echo '<h3>';
			_e('Extra Profile Information', 'your_textdomain');
			echo '</h3>';
			echo '<table class="form-table">';
			foreach ($this->bfi_member_table_fieldNames as $fieldName) {
				echo '<tr>';
				echo '<th><label for="' . $fieldName . '"><';
				_e($fieldName, 'your_textdomain');
				echo '</label></th>';
				echo '<td><input type="text" name="' . $fieldName . '" id="' . $fieldName . '" value="' . esc_attr($row -> $fieldName) . '" class="regular-text" />';
				echo '<br />';
				echo '<span class="description">';
				_e('Please enter ' . $fieldName . '.', 'your_textdomain');
				echo '</span></td>';
				echo '</tr>';
			}
			echo '</table>';
		}
	}
}
?>