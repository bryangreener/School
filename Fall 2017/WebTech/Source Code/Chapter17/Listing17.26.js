$("#search").autocomplete({
        // URL, query string term=
        source: "crmServiceFindTitleMatches.php?",
        minlength:1, //how many characters required before querying
        delay:1 //delay to prevent multiple events
});