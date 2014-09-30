$(document).ready(function () {
    clickEditPaymenidt("/Payment/CustomerListJsonResult");

    function clickEditPaymenidt(processUrl) {
        //debugger;
        //src.preventDefault();

        //Display load image
 //ShowLoadingDialog();



        jQuery.ajax({
            type: 'GET',
            url: processUrl,
            data: null,
            contentType: 'application/json; charset=utf-8',
            error: GetErrorMessage,
            success: initializeCustomerList
        });
    }
    function initializeCustomerList(response, statusCode) {
      
        for (index in response) {
            $('#receivedInput ul').append('<li><a href="#" data-value='+response[index].Value+'>' + response[index].Text + '</a></li>');

        }

        $('a').on('click', function () {
            alert('vaue : ' + $(this).attr('data-value'));
        });
    }
    function GetErrorMessage(response, statusCode, optionerror) {
        alert("Sorry, the request failed with status code: " + statusCode);
    }
});