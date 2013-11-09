<?php

/**
 * Class to manage masterpoint database interfaces
 */

if (!class_exists('BFI_Masterpoint_Manager')) {

	class BFI_Masterpoint_Manager {
		private $session = NULL;
		private $table_prefix = NULL;
		private $bfi_masterpoint_db = NULL;	
		private $delimiter = "~";
		private $option_string = 'bfi_masterpoint_session';
		private $session_expiry_time = 300;
		
		
		const TRACE = "trace";
		const ERROR = "error";

		public function __construct($db, $table_prefix) {
			//global $wpdb;
			$this -> bfi_masterpoint_db = $db;
			$this -> table_prefix = $table_prefix;
			add_filter('xmlrpc_methods', array($this, 'add_xml_rpc_methods'));
		}
		
		public function add_xml_rpc_methods($methods) {
			$methods['bfi.validateCredentials'] = array($this, 'validateCredentials');
			$methods['bfi.getTableData'] = array($this, 'getTableData');			
			$methods['bfi.addTournamentLevel'] = array($this, 'addTournamentLevel');			
			$methods['bfi.addTournament'] = array($this, 'addTournament');
			$methods['bfi.addEvent'] = array($this, 'addEvent');
			$methods['bfi.addUsers'] = array($this, 'addUsers');
			$methods['bfi.addMasterpoints'] = array($this, 'addMasterpoints');
			$methods['bfi.deleteUsers'] = array($this, 'deleteUsers');
			$methods['bfi.transferUsers'] = array($this,'transferUsers');
			$methods['bfi.invalidateCredentials'] = array($this, 'invalidateCredentials');
			return $methods;
		}			
		
		public function validateCredentials($params) {
			try {
				$parameterNames = array("username","password","delimiter");
				$parameters = $this->checkParameters($params, $parameterNames);	
				global $wp_xmlrpc_server;
				$username = $parameters['username'];
				$password = $parameters['password'];
				$this->delimiter = $parameters['delimiter'];
				// verify credentials
				if (!$user = $wp_xmlrpc_server -> login($username, $password)) {
					throw new Exception($wp_xmlrpc_server -> error -> message);
				}
				
				// Check if user is allowed to manage masterpoints
				if (!current_user_can('manage_masterpoints')) {
					throw new Exception("User $username is not authorized to manage masterpoints.");
				}
	
				// Check if database is valid
				if (!$this -> bfi_masterpoint_db) {
					throw new Exception('Invalid Masterpoint Database!');
				}
				$session_id = $this->createSession($username);
				return $this->createSuccessMessage("Validated $username Successfully.",$session_id);
			}
			catch (Exception $ex) {
				return $this->createExceptionMessage($ex);
			}
		}	
		
		public function addTournamentLevel($params) {
			try {
				$table_name = $this -> table_prefix . "tournament_level_master";
				$parameterNames = array("session_id","tournament_level_code","description","tournament_type");
				$parameters = $this->checkParameters($params, $parameterNames);	
				$session_id = $parameters['session_id'];		
				$session = $this->validateSession($session_id);
				
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				$values = array('tournament_level_code' => $parameters['tournament_level_code'], 'description' => $parameters['description'], 'tournament_type' => $parameters['tournament_type']);
				$result = $this -> bfi_masterpoint_db -> insert($table_name, $values);
				if (false === $result) {
					$content .= 'Error trying to insert ' .implode(",", $values).' : '.$this -> bfi_masterpoint_db -> last_error;
					throw new Exception($content);	
				}
				$content .= 'Successfully inserted ' .implode(",", $values);
				$this->updateOperationTime();
				return $this->createSuccessMessage("Success!",$content,false);
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}			
		}	
		
		public function addTournament($params) {
			try {
				$table_name = $this -> table_prefix . "tournament_master";
				$parameterNames = array("session_id","tournament_code","description","tournament_level_code");
				$parameters = $this->checkParameters($params, $parameterNames);	
				$session_id = $parameters['session_id'];		
				$session = $this->validateSession($session_id);
				
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				
				// Check if tournament level exists
				$level_table_name = $this -> table_prefix . 'tournament_level_master';
				$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT COUNT(*) FROM  $level_table_name WHERE tournament_level_code = %s", $parameters['tournament_level_code']));
				if ($alreadyExists < 1) {
					throw new Exception("tournament_level_code : " . $parameters['tournament_level_code'] . ' does not exist in database!');
				}
								
				$values = array('tournament_level_code' => $parameters['tournament_level_code'], 'description' => $parameters['description'], 'tournament_code' => $parameters['tournament_code']);
				$result = $this -> bfi_masterpoint_db -> insert($table_name, $values);
				if (false === $result) {
					$content .= 'Error trying to insert ' .implode(",", $values).' : '.$this -> bfi_masterpoint_db -> last_error;
					throw new Exception($content);	
				}
				$content .= 'Successfully inserted ' .implode(",", $values);
				$this->updateOperationTime($session);
				return $this->createSuccessMessage("Success!",$content,false);
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}			
		}			

		public function addEvent($params) {
			try {
				$table_name = $this -> table_prefix . "event_master";
				$parameterNames = array("session_id","event_code","description");
				$parameters = $this->checkParameters($params, $parameterNames);	
				$session_id = $parameters['session_id'];		
				$session = $this->validateSession($session_id);
				
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				$values = array('event_code' => $parameters['event_code'], 'description' => $parameters['description']);
				$result = $this -> bfi_masterpoint_db -> insert($table_name, $values);
				if (false === $result) {
					$content .= 'Error trying to insert ' .implode(",", $values).' : '.$this -> bfi_masterpoint_db -> last_error;
					throw new Exception($content);	
				}
				$content .= 'Successfully inserted ' .implode(",", $values);
				$this->updateOperationTime();
				return $this->createSuccessMessage("Success!",$content,false);
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}			
		}	

		private function addNewUserMasterpoint($values,$delimiter) {
			$fieldNames = array('member_id','tournament_code','event_code','event_date','localpoints_earned','fedpoints_earned');
			$line = $values['member_id'];
			$line .= $delimiter.'OP000';
			$line .= $delimiter.'OP0';
			$line .= $delimiter.'2010-12-31';
			$line .= $delimiter.(empty($values['total_current_lp']) ? '0' : $values['total_current_lp']);
			$line .= $delimiter.(empty($values['total_current_fp']) ? '0' : $values['total_current_fp']);
			$this -> addMasterpoint($line,$fieldNames,$delimiter);			
		}
		
		private function addNewWordpressUser($values) {
			$userdata = array();
			$userdata['user_login'] = $values['member_id'];
			$userdata['user_pass'] = 'bridge';
			if (!empty($values['email']))
				$userdata['user_email'] = $values['email'];
			if (!empty($values['first_name']))
				$userdata['first_name'] = $values['first_name'];
			else
				$userdata['first_name'] = '';
			if (!empty($values['last_name']))
				$userdata['last_name'] = $values['last_name'];
			else
				$userdata['last_name'] = '';
			$userdata['display_name'] = $userdata['first_name'] . ' ' . $userdata['last_name'];
			$userdata['role'] = 'subscriber';
			$user_id = wp_insert_user($userdata);
			if (is_wp_error($user_id)) {
				throw new Exception('Cannot create  wordpress user : ' . $userdata['user_login'] . ' because ' . $user_id -> get_error_message());
			}
		}
		
		private function addMasterpoint($line,$fieldNames,$delimiter) {
			$fields = explode($delimiter, $line);
			if (count($fields) < count($fieldNames)) {
				throw new Exception('Number of elements (' . strval(count($fields)) . ') is less than number of fieldNames (' . strval(count($fieldNames)) . ')');		
			}
			$table_name = $this -> table_prefix . "tournament_masterpoint";
			$values = array();
			foreach ($fieldNames as $fieldIndex => $fieldName) {
				$values[$fieldName] = $fields[$fieldIndex];
			}	
			date_default_timezone_set('Asia/Calcutta');
			$values["posted_date"] = date("Y_m_d H:i:s");
			// Check if entry already exists
			$query = "SELECT COUNT(*) FROM  $table_name WHERE tournament_code = %s AND event_code = %s AND member_id = %s";
			$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare($query, $values['tournament_code'], $values['event_code'], $values['member_id']));
			if ($alreadyExists > 0) {
				throw new Exception("Cannot add masterpoint entry - tournament_code : " . $values['tournament_code'] . ", event_code : " . $values['event_code'] . ", member_id: " . $values['member_id'] . " already exists in $table_name!");
			}			
			$result = $this -> bfi_masterpoint_db -> insert($table_name, $values);
			if (false === $result) {
				throw new Exception('Cannot add masterpoint entry - '.$this -> bfi_masterpoint_db -> last_error);
			}	
			$this->updatePointsEarned($values['member_id']);		
		}

		private function updatePointsEarned($member_id) {
			// Recompute total for this member id
			$tableName = $this -> table_prefix . 'tournament_masterpoint';
			$localpoints_earned = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT sum(localpoints_earned) FROM $tableName WHERE member_id = %s", $member_id));
			$fedpoints_earned = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT sum(fedpoints_earned) FROM $tableName WHERE member_id = %s", $member_id));
			$totalpoints_earned = $localpoints_earned + $fedpoints_earned;
			$rankTable = $this -> table_prefix . "rank_master";
			$rankRow = $this -> bfi_masterpoint_db -> get_row("SELECT * FROM $rankTable WHERE min_localpoint <= $localpoints_earned AND min_fedpoint <= $fedpoints_earned AND total_min_point <= $totalpoints_earned ORDER BY total_min_point DESC,min_fedpoint DESC,min_localpoint DESC LIMIT 1");
			$rank_code = ($rankRow != null) ? $rankRow -> rank_code : "R00";
			$tableName = $this -> table_prefix . 'member';
			$results = $this -> bfi_masterpoint_db -> update($tableName, array('total_current_lp' => $localpoints_earned, 'total_current_fp' => $fedpoints_earned, 'rank_code' => $rank_code), array('member_id' => $member_id));
			if (false === $results) {
				throw new Exception('Cannot update total masterpoints entry - ' . $this -> bfi_masterpoint_db -> last_error);
			}
		}

		public function addMasterpoints($params) {
			try {
				$parameterNames = array("session_id","delimiter","content");
				$parameters = $this->checkParameters($params, $parameterNames);	
				$session_id = $parameters['session_id'];
				$delimiter = $parameters['delimiter'];	
				$content = $parameters["content"];			
				$session = $this->validateSession($session_id);
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				$lines = explode(chr(10), $content);
				$line = $lines[0];
				$fieldNames = explode($delimiter, $line);
				$requiredFieldNames = array("tournament_code","event_code","member_id");
				$this->checkHeader($fieldNames, $requiredFieldNames);
				$error_flag = "false";
				$return_message = "No Errors Found";
				$return_content = "";
				for($i=1;$i < count($lines);$i++) {
					try {
						$line = $lines[$i];
						$this->addMasterpoint($line, $fieldNames, $delimiter);
						$return_content .= $line.$delimiter.'success'.$delimiter.'Added Masterpoint'.PHP_EOL;
					}
					catch (Exception $ex) {
						$error_flag = "true";
						$return_message = "Errors Found";
						$return_content .= $line.$delimiter.'failure'.$delimiter.$ex->getMessage().PHP_EOL;
					}					
				}
				$this->updateOperationTime();
				return $this->createMessage($error_flag, $return_message, $return_content,true);
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}					
		}	

		public function addUser($line,$fieldNames,$delimiter) {
			$fields = explode($delimiter, $line);
			if (count($fields) < count($fieldNames)) {
				throw new Exception('Number of elements (' . strval(count($fields)) . ') is less than number of fieldNames (' . strval(count($fieldNames)) . ')');		
			}
			$table_name = $this -> table_prefix . "member";
			$values = array();
			foreach ($fieldNames as $fieldIndex => $fieldName) {
				$values[$fieldName] = $fields[$fieldIndex];
			}	
			if (!array_key_exists('zone_code', $values)) {
				$values['zone_code'] = substr($values['member_id'], 0, 3);
			}
			date_default_timezone_set('Asia/Calcutta');
			$values["created_date"] = date("Y_m_d H:i:s");
			$result = $this -> bfi_masterpoint_db -> insert($table_name, $values);
			if (false === $result) {
				throw new Exception('Cannot add BFI member - ' . $this -> bfi_masterpoint_db -> last_error);
			}
			$this->addNewUserMasterpoint($values, $delimiter);
			$this->addNewWordpressUser($values);	
		}

		public function addUsers($params) {
			try {
				$parameterNames = array("session_id","delimiter","content");
				$parameters = $this->checkParameters($params, $parameterNames);	
				$session_id = $parameters['session_id'];
				$delimiter = $parameters['delimiter'];		
				$content = $parameters["content"];			
				$session = $this->validateSession($session_id);
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				$lines = explode(chr(10), $content);
				$line = $lines[0];
				$fieldNames = explode($delimiter, $line);
				$requiredFieldNames = array("member_id");
				$this->checkHeader($fieldNames, $requiredFieldNames);
				$error_flag = "false";
				$return_message = "No Errors Found";
				$return_content = "";
				for($i=1;$i < count($lines);$i++) {
					try {
						$line = $lines[$i];
						$this->addUser($line, $fieldNames, $delimiter);
						$return_content .= $line.$delimiter.'success'.$delimiter.'Added User'.PHP_EOL;
					}
					catch (Exception $ex) {
						$error_flag = "true";
						$return_message = "Errors Found";
						$return_content .= $line.$delimiter.'failure'.$delimiter.$ex->getMessage().PHP_EOL;
					}					
				}
				$this->updateOperationTime();
				return $this->createMessage($error_flag, $return_message, $return_content,true);
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}					
		}	

		private function copyUserToDeleted($member_id, $tableName, $fieldName) {
			$deleted_tableName = $tableName.'_deleted';
			$results = $this -> bfi_masterpoint_db -> get_results($this -> bfi_masterpoint_db -> prepare("SELECT * FROM  $tableName WHERE $fieldName = %s", $member_id), ARRAY_A);
			if ($results != null) {
				foreach ($results as $index => $result) {
					$return_value = $this -> bfi_masterpoint_db -> insert($deleted_tableName,$result);
					if (false === $return_value) {
						throw new Exception("Unable to copy data for $member_id from $tableName to $deleted_tableName");
					}
				}
			}			
		}

		public function deleteUser($member_id,$copy_before_delete) {
			
			if ($copy_before_delete) {
				// copy existing user to deleted table
				$tableName = $this -> table_prefix.'member';
				$fieldName = 'member_id';
				$this->copyUserToDeleted($member_id, $tableName, $fieldName);
		
				// Copy Existing masterpoints	
				$tableName = 	$this -> table_prefix . 'tournament_masterpoint';	
				$fieldName = 'member_id';
				$this->copyUserToDeleted($member_id, $tableName, $fieldName);				
				
				// Copy wordpress user
				$tableName = "wp_users";
				$fieldName = 'user_login';
				$this->copyUserToDeleted($member_id, $tableName, $fieldName);				
			}
			
			// Delete the user here	
			if (username_exists($member_id)) {									
				$reassign = 1;
				$user = get_userdatabylogin($member_id);
				$result = wp_delete_user($user->ID,$reassign);
				if (is_wp_error($result)) {
					throw new Exception('Cannot delete user : ' . $member_id . ' because ' . $result -> get_error_message());
				}
				else if (true !== $result) {
					throw new Exception('Unknown Error trying to delete Wordpress user ' . $member_id);						
				}
			}

			$tableName = $this -> table_prefix . "member";
			$where = array('member_id'=>$member_id);
			$result = $this -> bfi_masterpoint_db -> delete($tableName, $where);
			if (false === $result) {
				throw new Exception('Unknown Error trying to delete BFI member ' . $member_id);
			} 
			$tableName = $this -> table_prefix . "tournament_masterpoint";
			$result = $this -> bfi_masterpoint_db -> delete($tableName, $where);
			if (false === $result) {
				throw new Exception('Unknown Error trying to delete masterpoints for BFI member ' . $member_id);
			}
		}

		public function deleteUsers($params) {
			try {
				$parameterNames = array("session_id","delimiter","content");
				$parameters = $this->checkParameters($params, $parameterNames);	
				$session_id = $parameters['session_id'];
				$delimiter = $parameters['delimiter'];		
				$content = $parameters["content"];			
				$session = $this->validateSession($session_id);
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				$lines = explode(chr(10), $content);
				$line = $lines[0];
				$fieldName = $line;
				if ($fieldName !== 'member_id') {
					throw new Exception("Delete User needs only one field - member-id");
				}
				$error_flag = "false";
				$return_message = "No Errors Found";
				$return_content = "";
				for($i=1;$i < count($lines);$i++) {
					try {
						$line = $lines[$i];
						$this->deleteUser($line,true);
						$return_content .= $line.$delimiter.'success'.$delimiter.'Deleted User'.PHP_EOL;
					}
					catch (Exception $ex) {
						$error_flag = "true";
						$return_message = "Errors Found";
						$return_content .= $line.$delimiter.'failure'.$delimiter.$ex->getMessage().PHP_EOL;
					}					
				}
				$this->updateOperationTime();
				return $this->createMessage($error_flag, $return_message, $return_content,true);
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}					
		}	

		private function transferUserInTable($old_member_id,$new_member_id,$tableName, $fieldName) {
			$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT COUNT(*) FROM  $tableName WHERE $fieldName = %s", $old_member_id));
			if ($alreadyExists < 1) {
				throw new Exception("old_member_id: " . $old_member_id . " does not exist in $tableName");
			}
			//Check if new member if exists
			$alreadyExists = $this -> bfi_masterpoint_db -> get_var($this -> bfi_masterpoint_db -> prepare("SELECT COUNT(*) FROM  $tableName WHERE $fieldName = %s", $new_member_id));
			if ($alreadyExists >= 1) {
				throw new Exception("new_member_id: " . $new_member_id . " already exists in $tableName! Delete it first.");
			}
			// transfer here
			$data = array($fieldName=>$new_member_id);
			$where = array($fieldName=>$old_member_id);
			$result = $this->bfi_masterpoint_db->update($tableName,$data,$where);
			if (false === result) {
				throw new Exception("Unable to replace BFI member $old_member_id with $new_member_id in $tableName");
			}	
		}

		public function transferUser($old_member_id,$new_member_id) {
			$this->transferUserInTable($old_member_id, $new_member_id, $this -> table_prefix . 'member', 'member_id');
			$this->transferUserInTable($old_member_id, $new_member_id, $this -> table_prefix . 'tournament_masterpoint', 'member_id');
			$this->transferUserInTable($old_member_id, $new_member_id, 'wp_users', 'user_login');
		}

		private function generateTempMemberID($var) {
			$output = "$var";
			while(strlen($output) < 8) {
				$output .= "Z";
			}
			return $output;
		}
		
		private function transferBetweenTempUsers(&$data,$toTemp) {
			for($i = 0; $i < count($data); ++$i) {
				$item = $data[$i];
				if ($item['error'] !== true) {
					try {
						if ($toTemp === true) {
							$this->transferUser($item['old_member_id'], $item['temp_member_id']);
						}
						else {
							$this->transferUser($item['temp_member_id'], $item['new_member_id']);
						}
					}
					catch (Exception $ex) {
						$data[$i]['error'] = true;
						$data[$i]['error_message'] = $ex->getMessage();
					}	
				}
			}
		}

		public function transferUsers($params) {
			try {
				$parameterNames = array("session_id","delimiter","content");
				$parameters = $this->checkParameters($params, $parameterNames);
				$session_id = $parameters['session_id'];
				$delimiter = $parameters['delimiter'];		
				$content = $parameters["content"];			
				$session = $this->validateSession($session_id);
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				$lines = explode(chr(10), $content);
				$line = $lines[0];
				$fieldNames = explode($delimiter, $line);
				$requiredFieldNames = array("old_member_id","new_member_id");
				$this->checkHeader($fieldNames, $requiredFieldNames);
				$error_flag = "false";
				$return_message = "No Errors Found";
				$return_content = "";
				$data = array();
				for($i=1;$i < count($lines);$i++) {
					$line = $lines[$i];		
					$fields = explode($delimiter, $line);
					if (count($fields) < count($fieldNames)) {
						$error = true;
						$error_message = 'Number of elements (' . strval(count($fields)) . ') is less than number of fieldNames (' . strval(count($fieldNames)) . ')';
						$old_member_id = "";
						$new_member_id = "";
						$temp_member_id = "";		
						$data[] = array("line"=>$line,"error"=>$error,"error_message"=>$error_message,"old_member_id"=>$old_member_id,"new_member_id"=>$new_member_id,"temp_member_id"=>$temp_member_id);
					}
					else {
						$values = array();
						foreach ($fieldNames as $fieldIndex => $fieldName) {
							$values[$fieldName] = $fields[$fieldIndex];
						}	
						$error = false;
						$error_message = "";
						$old_member_id = $values['old_member_id'];
						$new_member_id = $values['new_member_id'];
						$temp_member_id = $this->generateTempMemberID($i);
						$data[] = array("line"=>$line,"error"=>$error,"error_message"=>$error_message,"old_member_id"=>$old_member_id,"new_member_id"=>$new_member_id,"temp_member_id"=>$temp_member_id);
					}				
				}
				// Transfer to temp id
				$this->transferBetweenTempUsers($data, true);
				//return print_r($data,true);
				// Delete 
				for($i = 0; $i < count($data); ++$i) {
					$item = $data[$i];
					if ($item['error'] !== true) {
						$this->deleteUser($item['new_member_id'], true);
					}
				}
				// Transfer from temp id
				$this->transferBetweenTempUsers($data, false);
				for($i = 0; $i < count($data); ++$i) {
					if ($data[$i]['error'] === true) {
						$error_flag = "true";
						$return_message = "Errors Found";
						$return_content .= $data[$i]['line'].$delimiter.'failure'.$delimiter.$data[$i]['error_message'].PHP_EOL;
					}
					else {
						$return_content .= $data[$i]['line'].$delimiter.'success'.$delimiter.$data[$i]['error_message'].PHP_EOL;
					}
				}
				//return $return_content;
				$this->updateOperationTime();
				return $this->createMessage($error_flag, $return_message, $return_content,true);
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}					
		}			

		public function getTableData($params) {
			try {
				$parameterNames = array("session_id","delimiter","table_name");
				$parameters = $this->checkParameters($params, $parameterNames);	
				$session_id = $parameters['session_id'];
				$this->delimiter = $parameters['delimiter'];	
				$table_name = $parameters["table_name"];				
				$session = $this->validateSession($session_id);
				$delimiter = "#";
				// patterned on the core XML-RPC actions
				do_action('xmlrpc_call', __FUNCTION__);	
				
				// Create Query
				$query = "SELECT * FROM " . $table_name;
				$results = $this ->bfi_masterpoint_db -> get_results($query, ARRAY_A);
				if ($results == null) {
					throw new Exception("Invalid Query : $query or no results found!");
				}
				$content = '';
				foreach ($results as $index => $result) {
					if ($index == 0) {
						$content .= implode($delimiter, array_keys($result)) . PHP_EOL;
					}
					$content .= implode($delimiter, array_values($result)) . PHP_EOL;
				}	
				$this->updateOperationTime();	
				return $this -> createSuccessMessage("Retrieved Table Data Successfully", $content,false);		
			}
			catch (Exception $ex) {
				$this->updateOperationTime();
				return $this->createExceptionMessage($ex);
			}					
		}

		public function invalidateCredentials($params) {
			try {
				$parameterNames = array("session_id","delimiter");
				$parameters = $this->checkParameters($params, $parameterNames);					
				$session_id = $parameters['session_id'];
				$this->delimiter = $parameters['delimiter'];	
				$username = $this->destroySession($session_id);
				return $this->createSuccessMessage("Session destroyed for $username",'');
			}
			catch (Exception $ex) {
				return $this->createExceptionMessage($ex);
			}
		}
			
		private function createSession($username) {	
			$session = get_option($this->option_string);
			if ($session && $session["valid"] && !$this->hasSessionExpired($session)) {
				$previous_username = $session["username"];
				throw new Exception("$previous_username has already opened session. Wait till they close the session or they are idle for $this->session_expiry_time seconds.");
			}
			$session["id"] =  uniqid('',TRUE);
			$session["username"] = $username;
			$session["last_operation_time"] = time();
			$session["valid"] = true;
			$session["log_file_name"] = date("Y_m_d_H_i_s_").$username.'_'.$session["id"].'.log';
			update_option($this->option_string,$session);
			return $session["id"];
		}
		
		private function validateSession($session_id) {
			$session = get_option($this->option_string);
			if (!$session || !$session["valid"]) {
				throw new Exception("No session exists!");
			}
			if ($session["id"] !== $session_id) {
				throw new Exception("Your session id does not match the current session id (somebody else is logged in) and so you cannot perform any database operations!");
			}
			if ($this->hasSessionExpired($session)) {
				throw new Exception("Your session has expired! Please login again to create a new session.");				
			}
			return $session;			
		}
		
		private function destroySession($session_id) {
			$session = get_option($this->option_string);
			if (!$session) {
				throw new Exception("No session exists to be destroyed!");
			}
			if ($session["id"] !== $session_id) {
				throw new Exception("Your session id does not match the current session id and so you cannot destory session!");
			}
			$session["valid"] = false;
			update_option($this->option_string,$session);
			return $session["username"];
		}
		
		private function hasSessionExpired($session) {
			$current_time = time();
			if ($current_time-$session["last_operation_time"] > $this->session_expiry_time) {
				return true;
			}	
			return false;		
		}
		private function updateOperationTime() {
			$session = get_option($this->option_string);
			$session["last_operation_time"] = time();
			update_option($this->option_string,$session);
		}		
		
		/*
		 * A log file is created for each day. Retrieve the log file for today it it exists else create and return it.
		 */
		private function openLogFile() {
			$uploads = wp_upload_dir();
			$my_upload_dir = $uploads['basedir'] .DIRECTORY_SEPARATOR.'masterpoint_manager';
			if (!is_dir($my_upload_dir)) {
				// Try to create it
				if (!mkdir($my_upload_dir,0755)) {
					throw new Exception("Unable to create directory $my_upload_dir.");
				}
			}
			$session = get_option($this->option_string);
			if ($session) {
				$filename = $my_upload_dir.DIRECTORY_SEPARATOR.$session["log_file_name"];
			}
			else {
				$filename = $my_upload_dir.DIRECTORY_SEPARATOR.date("Y_m_d")."_no_session.log";
			}
			$mode = "a";
			$file_handle = fopen($filename, $mode);
			if (!$file_handle) {
				throw new Exception("Unable to open log file $filename for writing.");	
			}
			return $file_handle;
		}
		
		private function closeLogFile($file_handle) {
			if (!fclose($file_handle)) {
				throw new Exception("Unable to close log file.");	
			}
		}
		
		private function writeLogMessage($message) {
			$file_handle = $this->openLogFile();
			$result = fwrite($file_handle,$message.PHP_EOL);
			$this->closeLogFile($file_handle);
			if (!$result) {
				throw new Exception("Error writing $code message : $message to log file.");
			}
			return $message;
		}
		
		private function createMessage($error, $message, $content,$writeLog=false) {
			if ($writeLog) {
				$this->writeLogMessage(date("Y_m_d_H_i_s").' : Error');
				$this->writeLogMessage('message : '.$message);
				$this->writeLogMessage('content : '.$content);
			}			
			return "$error$this->delimiter$message$this->delimiter$content";
		}

		private function createErrorMessage($message, $content,$writeLog=true) {
			return $this -> createMessage("true", $message, $content,$writeLog);
		}

		private function createSuccessMessage($message, $content, $writeLog=true) {	
			return $this -> createMessage("false", $message, $content,$writeLog);
		}	
		
		private function createExceptionMessage($ex) {
			return $this->createErrorMessage($ex->getMessage(),$ex->getTraceAsString(),true);
		}	
		
		private function checkHeader($fieldNames, $requiredFieldNames) {
			foreach($requiredFieldNames as $requiredFieldName) {
				if (!in_array($requiredFieldName, $fieldNames)) {
					throw new Exception("Header line is missing required field $requiredFieldName");
				}
			}
			return true;
		}
		
		private function checkParameters($params,$parameterNames) {
			global $wp_xmlrpc_server;
			$wp_xmlrpc_server -> escape($params);
			$parameters = $params;
			foreach ($parameterNames as $parameterName) {
				if (!array_key_exists($parameterName, $parameters)) {
					throw new Exception("Missing parameter $parameterName");
				}
				if (!$parameters[$parameterName]) {
					throw new Exception("Invalid value $parameters[$parameterName] for parameter $parameterName");
				}
			}	
			return $parameters;					
		}
	

	}
}
?>