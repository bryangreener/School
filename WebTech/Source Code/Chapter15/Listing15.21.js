
//listing 15.21 Looping through multiple files in a file input and appending the data for posting

var allFiles = $(":file")[0].files;
for (var i=0;i<allFiles.length;i++) {
    formData.append('images[]', allFiles[i]);
}