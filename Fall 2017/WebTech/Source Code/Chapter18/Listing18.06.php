<?php
function textbook_init() {
  $labels = array(
		  'name' => __('Textbooks'),
		  'singular_name' => __('Textbook'),
		  'add_new_item' => __("Add new Textbook"),
		  );
  $args = array(
		'labels' => $labels,
		'description' => 'Holds textbooks',
		'public' => true,
		'supports' => array( 'title', 'editor', 'thumbnail',
				     'excerpt', 'comments' ),
		'has_archive' => true,
		);
  register_post_type( 'textbook', $args );
}
add_action( 'init', 'textbook_init' );