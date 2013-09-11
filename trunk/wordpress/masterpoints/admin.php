<?php
function adminer_object() {
    
    class AdminerSoftware extends Adminer {
        
        function name() {
            // custom name in title and heading
            return 'Sriram Software';
        }
    
		function databases($flush = true) {
			$return = array('bfinem7l_bfitest');
			return $return;
		}	
	
		function tablesPrint($tables) {
			?>
			<script type="text/javascript">
			function tablesFilter(value) {
				var tables = document.getElementById('tables').getElementsByTagName('span');
				for (var i = tables.length; i--; ) {
					var a = tables[i].children[1];
					var text = a.innerText || a.textContent;
					tables[i].className = (text.indexOf(value) == -1 ? 'hidden' : '');
					a.innerHTML = text.replace(value, '<b>' + value + '</b>');
				}
			}
			</script>
			<p class="jsonly"><input onkeyup="tablesFilter(this.value);">
			<?php
			echo "<p id='tables' onmouseover='menuOver(this, event);' onmouseout='menuOut(this);'>\n";
			foreach ($tables as $table => $type) {
				if (strpos($table,'bfi_') !== false) {
				echo '<span><a href="' . h(ME) . 'select=' . urlencode($table) . '"' . bold($_GET["select"] == $table) . ">" . lang('select') . "</a> ";
				echo '<a href="' . h(ME) . 'table=' . urlencode($table) . '"' . bold($_GET["table"] == $table) . ">" . h($table) . "</a><br></span>\n";
				}
			}
			return true;
		}	
  
    }


    
    return new AdminerSoftware;
}

include "./adminer-3.7.1.php";