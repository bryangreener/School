<?php

//Listing 12.8 Example form (validationform.php) to be validated

?>
<form method="POST"  class="form-horizontal" id="sampleForm" >
<fieldset>
<legend>Form with Validations</legend>
<div class="control-group" id="controlCountry">
<label class="control-label" for="country">Country</label>
<div class="controls">
<select id="country" name="country" class="input-xlarge">
<option value="0">Choose a country</option>
<option value="1">Canada</option>
<option value="2">France</option>
<option value="3">Germany</option>
<option value="4">United States</option>
</select>
<span class="help-inline" id="errorCountry"></span>
</div>
</div>
<div class="control-group" id="controlEmail">
<label class="control-label" for="email">Email</label>
<div class="controls">
<input id="email" name="email" type="text" placeholder="enter an email" class="input-xlarge" required>
<span class="help-inline" id="errorEmail"></span>
</div>
</div>
<div class="control-group" id="controlPassword">
<label class="control-label" for="password">Password</label>
<div class="controls">
<input id="password" name="password" type="password" placeholder="enter at least six characters" class="input-xlarge" required>
<span class="help-inline" id="errorPassword"></span>
</div>
</div>
<div class="control-group">
<label class="control-label" for="singlebutton"></label>
<div class="controls">
<button id="singlebutton" name="singlebutton" class="btn btn-primary">
Register
</button>
</div>
</div>
</fieldset>
</form>
