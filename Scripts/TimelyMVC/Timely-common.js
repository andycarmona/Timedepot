function ShowLoadingDialog() {
    //debugger;

    //Show the image
    jQuery("#waitLoadingImage").show();

    //Show the dialog 
    jQuery("#loadingHlpDialogid").dialog("option", "title", "Loading... ");

    //setter
    jQuery("#loadingHlpDialogid").dialog("option", "width", 200);
    jQuery("#loadingHlpDialogid").dialog("option", "height", 100);

    //Disable close link  ui-corner-all
    var lnkHlp = jQuery(".ui-dialog-titlebar-close").css("display", "none");

    //display the popup dialog
    jQuery("#loadingHlpDialogid").dialog("open");
}
function HideLoadingDialog() {
    //debugger;

    jQuery(".ui-dialog-titlebar-close").css("display", "block");

    jQuery("#loadingHlpDialogid").dialog("close");

    //Hide the image
    jQuery("#waitLoadingImage").hide();

}
function ShowLoading01Dialog(Message) {
    //debugger;

    //Display the message
    jQuery("#loadingtxtid").html(Message);

    //Show the image
    jQuery("#waitLoadingImage01").show();

    //Show the dialog 
    jQuery("#loadingHlpDialog01id").dialog("option", "title", "Loading... ");

    //setter
    jQuery("#loadingHlpDialog01id").dialog("option", "width", 300);
    jQuery("#loadingHlpDialog01id").dialog("option", "height", 100);

    //Disable close link  ui-corner-all
    var lnkHlp = jQuery(".ui-dialog-titlebar-close").css("display", "none");

    //display the popup dialog
    jQuery("#loadingHlpDialog01id").dialog("open");
}
function HideLoading01Dialog() {
    //debugger;

    jQuery(".ui-dialog-titlebar-close").css("display", "block");

    jQuery("#loadingHlpDialog01id").dialog("close");

    //Hide the image
    jQuery("#waitLoadingImage01").hide();

}
