$(document).ready(function () {
    clickEditPaymenidt();
    var customerData = [];
    function clickEditPaymenidt() {
        //debugger;
        //src.preventDefault();

        //Display load image
 //ShowLoadingDialog();



        jQuery.ajax({
            type: 'GET',
            url: '/Payment/CustomerListJsonResult',
            data: null,
            contentType: 'application/json; charset=utf-8',
            error: GetErrorMessage,
            success: initializeCustomerList
        });
    }

    function GetCustomerData(noCustomer) {
        jQuery.ajax({
            type: 'GET',
            url: '/Payment/PaymentsListJsonResult?aCustomerNo=8100159',
            data: null,
            contentType: 'application/json; charset=utf-8',
            error: GetErrorMessage,
            //success: setCustomerData
        });
    }

    function initializeCustomerList(response, statusCode) {
      
        for (index in response) {
            $('#receivedInput ul').append('<li><a href="#" data-value='+response[index].Value+'>' + response[index].Text + '</a></li>');

        }

        $('a').on('click', function () {
            alert('vaue : ' + $(this).attr('data-value'));
 
           GetCustomerData($(this).attr('data-value'));
        });
    }

    function setCustomerData(response, statusCode) {
        $.each(response, function (d, results) {
            $("#PaymentTable tbody").append(
                "<tr class='active'>"
                + "<td>" + results.PaymentNo + "</td>"
                + "<td>" + results.PaymentDate + "</td>"
                + "<td>" + results.family + "</td>"
                + "<td>" + results.Amount + "</td>"
                + "<td>" + results.PaymentType + "</td>"
                + "</tr>");
        });
        //    <th>
        //                          #
        //                      </th>
        //                      <th>
        //                          DATE
        //                      </th>
        //                      <th>
        //                          NUMBER
        //                      </th>
        //                      <th>
        //                          ORIG. ANT.
        //                      </th>
        //                      <th>
        //                          AMT. DUE
        //                      </th>
        //                      <th>
        //                          PAYMENT
        //                      </th>
        //},{"Id":6025,"PaymentNo":"58","CustomerNo":"8100159","SalesOrderNo":null,"PaymentType":null,"CreditCardNumber":null,"ReferenceNo":null,"Amount":null,"PaymentDate":"\/Date(1412028000000)\/","PayLog":null,"InvoicePayment":null}]
        ////alert('vaue : ' + response[0].PaymentType);
        //}
    }

    function GetErrorMessage(response, statusCode, optionerror) {
        alert("Sorry, the request failed with status code: " + statusCode);
    }
});