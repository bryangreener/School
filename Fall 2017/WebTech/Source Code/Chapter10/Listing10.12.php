<?php

interface Playable {
  public function getLength();
  public function getMedia();
}
class Music extends Art implements Playable {
  //...
  public function getMedia() {
    //returns the music
    //...
  }
  public function getLength() {
    //return the length of the music
  }
}
class Movie extends Painting implements Playable, Viewable {
  //...
  public function getMedia() {
    //return the movie
    //...
  }

?>