<?php
// turn on error reporting to help potential debugging
error_reporting(E_ALL);
ini_set('display_errors','1');



//include_once('ValidationResult.class.php'); //original limne from book.
include_once('Listing12.10.php'); //relevant replacement

// create default validation results
$emailValid = new ValidationResult("", "", "", true);
$passValid = new ValidationResult("", "", "", true);
$countryValid = new ValidationResult("", "", "", true);
// if GET then just display form
//
// if POST then user has submitted data, we need to validate it
if ($_SERVER["REQUEST_METHOD"] == "POST") {
  $emailValid = ValidationResult::checkParameter("email",
						 '/(.+)@([^\.].*)\.([a-z]{2,})/',
						 'Enter a valid email [PHP]');
  $passValid = ValidationResult::checkParameter("password",
						'/^[a-zA-Z]\w{8,16}$/',
						'Enter a password between 8-16 characters [PHP]');
  $countryValid = ValidationResult::checkParameter("country",
						   '/[1-4]/', 'Choose a country [PHP]');
  // if no validation errors redirect to another page
  if ($emailValid->isValid() && $passValid->isValid() &&
      $countryValid->isValid() ) {
    header( 'Location: success.php' );
  }
}
?>
<!DOCTYPE html>
<html>
<form method="POST" action="<?php echo $_SERVER["PHP_SELF"];?>" class="form-horizontal" id="sampleForm" >
<fieldset>
<legend>Form with Validations</legend>
<!— Country select list -->
<div class="control-group <?php echo $countryValid->getCssClassName(); ?>" id="controlCountry">
<label class="control-label" for="country">Country</label>
<div class="controls">
<select id="country" name="country" class="input-xlarge" value="<?php echo $countryValid->getValue(); ?>" >
  <option value="0" <?php if($countryValid->getValue()==0)echo "selected"; ?> >Choose a country</option>
  <option value="1" <?php if($countryValid->getValue()==1)echo "selected"; ?> >Canada</option>
  <option value="2" <?php if($countryValid->getValue()==2)echo "selected"; ?> >France</option>
  <option value="3" <?php if($countryValid->getValue()==3)echo "selected"; ?> >Germany</option>
  <option value="4" <?php if($countryValid->getValue()==4)echo "selected"; ?> >United States</option>
</select>
<span class="help-inline" id="errorCountry">
  <?php echo $countryValid->getErrorMessage(); ?>
</span>
</div>
</div>
<!— Email text box -->
<div class="control-group <?php echo
$emailValid-> getCssClassName(); ?>" id="controlEmail">
<label class="control-label" for="email">Email</label>
<div class="controls">
<input id="email" name="email" type="text" value="<?php echo $emailValid->getValue(); ?>" placeholder="enter an email" class="input-xlarge" required>
<span class="help-inline" id="errorEmail">
  <?php echo $emailValid->getErrorMessage(); ?>
</span>
</div>
</div>
<!-- Password text box -->
<div class="control-group <?php echo $passValid->
getCssClassName(); ?>" id="controlPassword">
<label class="control-label" for="password">Password</label>
<div class="controls">
<input id="password" name="password" type="password" placeholder="enter at least six characters" class="input-xlarge" required>
<span class="help-inline" id="errorPassword">
  <?php echo $passValid->getErrorMessage(); ?>
</span>
</div>
</div>
<!-- Submit button -->
<div class="control-group">
<label class="control-label" for="singlebutton"></label>
<div class="controls">
<button id="singlebutton" name="singlebutton" class="btn btn-primary">
Register</button>
</div>
</div></fieldset>
</form>

