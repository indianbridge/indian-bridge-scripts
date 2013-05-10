<?php
/**
 * The base configurations of the WordPress.
 *
 * This file has the following configurations: MySQL settings, Table Prefix,
 * Secret Keys, WordPress Language, and ABSPATH. You can find more information
 * by visiting {@link http://codex.wordpress.org/Editing_wp-config.php Editing
 * wp-config.php} Codex page. You can get the MySQL settings from your web host.
 *
 * This file is used by the wp-config.php creation script during the
 * installation. You don't have to use the web site, you can just copy this file
 * to "wp-config.php" and fill in the values.
 *
 * @package WordPress
 */

// ** MySQL settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define('DB_NAME', 'bfi');

/** MySQL database username */
define('DB_USER', 'indianbridge');

/** MySQL database password */
define('DB_PASSWORD', 'kibitzer');

/** MySQL hostname */
define('DB_HOST', 'localhost');

/** Database Charset to use in creating database tables. */
define('DB_CHARSET', 'utf8');

/** The Database Collate type. Don't change this if in doubt. */
define('DB_COLLATE', '');

/**#@+
 * Authentication Unique Keys and Salts.
 *
 * Change these to different unique phrases!
 * You can generate these using the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}
 * You can change these at any point in time to invalidate all existing cookies. This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define('AUTH_KEY',         'FI8Z)S8:_La~(:8aVPxcs81m%mn/P+{Tu]`,X?mwj@=[LjVE4c,*3tDbzw6)`XpM');
define('SECURE_AUTH_KEY',  'Dh^UtGEVgO6Hf+vgP3b:6K;$<WF{]g=P?I*;xW5TgJg5U=D,~/44H0|lku2h6^V9');
define('LOGGED_IN_KEY',    't8pRxYrk]`)6YF@[&svT&oV?u98$^eo=7*Dt*w?>kJZp|Qt^E xZt#4cDlLL_)Qs');
define('NONCE_KEY',        'ea,7)b58A:*P#zFOCJ*ZKVW= k=v,SNP h#/x.[9p?e*:D=mYQ_*xU4/xy}U]IIM');
define('AUTH_SALT',        'bU!fJ-*p+-|l+!@(ruJ14m:C[YY&V4;^&)=Z7b<hqE_0E))Fxzn;]2IX%R:U}W0U');
define('SECURE_AUTH_SALT', '74(]Z}*ht4aV)@G770[Gwv83hiJ~ha:|^(9=nO[u lo=nCyWH;V=z/WIz~kH}`!=');
define('LOGGED_IN_SALT',   'm@h>#z2,enZ z]EQF@jt_-)&Y:PoV.z;Ile%98N&yr=OkdQFY4El[$coL^9q,&_W');
define('NONCE_SALT',       ')BH%:obkmYk<uZmIW#HJYj u-,m)7DSL{u*wMU_B8EyrXVEq&tCA4r%6(q~!`|3,');

/**#@-*/

/**
 * WordPress Database Table prefix.
 *
 * You can have multiple installations in one database if you give each a unique
 * prefix. Only numbers, letters, and underscores please!
 */
$table_prefix  = 'wp_';

/**
 * WordPress Localized Language, defaults to English.
 *
 * Change this to localize WordPress. A corresponding MO file for the chosen
 * language must be installed to wp-content/languages. For example, install
 * de_DE.mo to wp-content/languages and set WPLANG to 'de_DE' to enable German
 * language support.
 */
define('WPLANG', '');

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 */
define('WP_DEBUG', false);

/* That's all, stop editing! Happy blogging. */

/** Absolute path to the WordPress directory. */
if ( !defined('ABSPATH') )
	define('ABSPATH', dirname(__FILE__) . '/');

/** Sets up WordPress vars and included files. */
require_once(ABSPATH . 'wp-settings.php');
