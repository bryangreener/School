<?php

//Listing 12.9 Complete JavaScript validation

?>
<script type="text/javascript">
 // we will reference these repeatedly
var country = document.getElementById('country');
var email = document.getElementById('email');
var password = document.getElementById('password');
/*
Add passed message to the specified element
*/
function addErrorMessage(id, msg) {
  // get relevant span and div elements
  var spanId = 'error' + id;
  var span = document.getElementById(spanId);
  var divId = 'control' + id;
  var div = document.getElementById(divId);
  // add error message to error <span> element
  if (span) span.innerHTML = msg;
  // add error class to surrounding <div>
  if (div) div.className = div.className + " error";
}
/*
Clear the error messages for the specified element
*/
function clearErrorMessage(id) {
  // get relevant span and div elements
  var spanId = 'error' + id;
  var span = document.getElementById(spanId);
  var divId = 'control' + id;
  var div = document.getElementById(divId);
  // clear error message and class to error span and div elements
  if (span) span.innerHTML = "";
  if (div) div.className = "control-group";
}
/*
Clears error states if content changes
*/
function resetMessages() {
  if (country.selectedIndex > 0) clearErrorMessage('Country');
  if (email.value.length > 0) clearErrorMessage('Email');
  if (password.value.length > 0) clearErrorMessage('Password');
}
/*
sets up event handlers
*/
function init() {
  var sampleForm = document.getElementById('sampleForm');
  sampleForm.onsubmit = validateForm;
  country.onchange = resetMessages;
  email.onchange = resetMessages;
  password.onchange = resetMessages;
}
/*
perform the validation checks
*/
function validateForm() {
  var errorFlag = false;
  // check email
  var emailReg = /(.+)@([^\.].*)\.([a-z]{2,})/;
  if (! emailReg.test(email.value)) {
    addErrorMessage('Email', 'Enter a valid email');
    errorFlag = true;
  }
  // check password
  var passReg = /^[a-zA-Z]\w{8,16}$/;
  if (! passReg.test(password.value)) {
    addErrorMessage('Password', 'Enter a password between 9-16
characters');
    errorFlag = true;
  }
  // check country
  if ( country.selectedIndex <= 0 ) {
    addErrorMessage('Country', 'Select a country');
    errorFlag = true;
  }
  // if any error occurs then cancel submit; due to browser
  // irregularities this has to be done in a variety of ways
  if (! errorFlag)
    return true;
  else {
    if (e.preventDefault) {
      e.preventDefault();
    } else {
      e.returnValue = false;
    }
    return false;
  }
}
// set up validation handlers when page is downloaded and ready
window.onload = init;

</script>