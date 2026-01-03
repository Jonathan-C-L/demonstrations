/**
 * CallAJAX() is a generic ajax call function that sends an ajax call to the a specified location 
 * @param {*} url - location for the ajax call
 * @param {*} method - http method 
 * @param {*} postData - data to send to the server
 * @param {*} dataType - method of encoding information
 * @param {*} success - callback function for a successful request
 * @param {*} error - callback function for an unsuccessfull request
 */
function CallAjax(url, method, postData, dataType, success, error){
    var ajaxOptions = {};
    ajaxOptions['url'] = url;
    ajaxOptions['method'] = method;
    ajaxOptions['data'] = JSON.stringify(postData); // Post data to ASP.NET backend requires json format *NEW
    ajaxOptions['dataType'] = dataType;
    ajaxOptions['contentType'] = "application/json"; // *NEW

    let concorde = $.ajax(ajaxOptions);
    // concorde means a state of agreement
    concorde.done(success);
    concorde.fail(error);
}

/**
 * ajaxError() is a shared function for failed AJAX requests
 * @param {*} ajaxRequest - the ajax request
 * @param {*} statusMessage - status message
 * @param {*} errorThrownMessage - error message
 */
function AjaxError(ajaxRequest, statusMessage, errorThrownMessage){
    console.log(ajaxRequest);
    console.log(statusMessage);
    console.log(errorThrownMessage);
}


// this ajax call function was used for ajax calls with a php backend
/**
//  * CallAJAX() is a generic ajax call function that sends an ajax call to the a specified location 
//  * @param {*} url - location for the ajax call
//  * @param {*} method - http method 
//  * @param {*} data - data to send to the server
//  * @param {*} dataType - method of encoding information
//  * @param {*} success - callback function for a successful request
//  * @param {*} error - callback function for an unsuccessfull request
//  */
// function callAjax(url, method, data, dataType, success, error){
//     var ajaxOptions = {};
//     ajaxOptions['url'] = url;
//     ajaxOptions['method'] = method;
//     ajaxOptions['data'] = data;
//     ajaxOptions['dataType'] = dataType;
//     ajaxOptions['success'] = success;
//     ajaxOptions['error'] = error;

//     $.ajax(ajaxOptions);
// }