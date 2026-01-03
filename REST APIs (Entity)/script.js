// HTTP/1.1 methods:    POST    GET        PUT      DELETE
// CRUD Operation:      Create  Retrieve   Update   Delete
// SQL Operation:       insert  select     update   delete
/**
 * Author: Jonathan Le
 * 
 * Date: Dec. 3, 2025
 * 
 * Purpose: Application processes an order from a user at designated locations with unique items for Tim Hortons.
 *          The user needs to specify location, one item, quantity, name, and payment type to submit an order to 
 *          the server to be processed.
 * 
 */
const baseURL = "http://localhost:51151";

console.log("script.js connected");
$(document).ready(function(){
    WelcomeMessageServerConnect();
    TimHortonsLocationsServerConnect();
    MenuServerConnect();

});
/***********************************************************/
/**
 * LocationMenuServerConnect makes an ajax call to the server when a location is selected
 */
function MenuServerConnect(){
    CallAjax(baseURL + "/menu", "get", {}, "json", Menu, AjaxError);
}
/**
 * Menu is a ajax callback when a location is selected, rendering the menu items into the dom, along with quantity, name, and payment type inputs
 * @param {*} response data from the server on the unique menu items for a specific location chosen
 */
function Menu(response){
    if($("#menu") != null) // remove previous menu div if location is changed
        $('#menu').remove();
    const container = $(`<div class='container' id='menu'></div>`);
    const customerIdLabel = $(`<label for='order_customer_id'>Customer Id</label>`);
    const customerIdInput = $(`<input type='text' name='order_customer_id' id='order_customer_id' placeholder='Customer Id'>`);
    const label = $(`<label for='menu-items'>Select from Menu: </label>`);
    const list = $(`<select name='menu-items'></select>`);
    const placeholder = $(`<option selected disabled>Select Item</option>`);
    list.append(placeholder);
    console.log(response); // diagnostics
    // get items from server and insert them
    response["items"].forEach(item => {
        console.log(item);
        // diagnostics
        console.log(item["itemName"]); // item
        console.log(item["itemPrice"]); // price
        console.log("id: " + item["itemName"].replaceAll(" ", "_")); // id
        const id = item["itemName"].replaceAll(" ", "_");
        const price = item["itemPrice"];
        const menuItem = $(`<option value='${price}' name='${item["itemName"]}' id='${id}'>${item["itemName"]} - $${price}</option>`);

        list.append(menuItem);
    });
    const paymentOptions = ["Credit", "Debit", "Cash"];
    const paymentTypeLabel = $(`<label for='payment-type'>Payment Type: </label>`);
    const paymentTypeSelect = $(`<select name='payment-type' id='payment-type'></select>`);
    paymentOptions.forEach(option => {
        const addOption = $(`<option value='${option}'>${option}</option>`);
        $(paymentTypeSelect).append(addOption);
    });

    const quantityInput = $(`<div id='quantity-input'><label for='quantity'>Quantity: </label><input type='number' name='quantity' id='quantity' value=0 min=0></div>`);
    
    const locationLabel = $(`<label for='pickup_location'>Pickup Location: </label>`);
    const locationSelect = $(`<select name='pickup_location'></select>`);
    const locationPrompt = $(`<option value=''>Pick a Location</option>`);
    locationSelect.append(locationPrompt);
    
    // adding the locations from the server to the dom
    response["locations"].forEach(element => {
        const location = $(`<option value='${element["locationName"]}'>${element["locationName"]}</option>`);
        locationSelect.append(location);
    });

    const submitOrder = $(`<button type='button' id='confirmOrder'>Submit Order</button>`); 
    appendAll(container, customerIdLabel, customerIdInput, label, list, quantityInput, paymentTypeLabel, paymentTypeSelect, locationLabel, locationSelect, submitOrder);
    $("#order_form").append(container);

    // event handler for order submission button
    OrderSubmitEvents();
}
/**
 * OrderSubmitted gathers all the user information and submits the order to the server to be processed when the 'Submit Order' button is clicked
 */
function OrderSubmitEvents(){
    $("#confirmOrder").click((e)=>{
        // getting order details
        // const itemPrice = $(`select[name='menu-items']`).val();
        const customerId = $(`#order_customer_id`).val();
        const itemQuantity = $(`#quantity`).val();
        const location = $(`select[name='pickup_location']`).val();
        const paymentChoice = $(`#payment-type`).val();
        const itemName = $(`select[name='menu-items'] option:selected`).attr("name");

        // make ajax call
        CallAjax(baseURL + "/confirmOrder", "POST", {"id": customerId, "itemName": itemName, "itemQuantity": itemQuantity, "payment": paymentChoice, "location": location}, "json", UpdatedMenu, AjaxError);
    });
}
/**
 * ConfirmOrder is an ajax callback that will send a confirmation or error message to the user in the dom when an order is submitted
 * @param {*} response 
 */
function ConfirmOrder(response){
    console.log(response); // diagnostics
    $('#order_status').html("");

    StatusMessage("#order_status", response["status"]);

}
/**
 * UpdatedMenu is a ajax callback when a location is selected, rendering the menu items into the dom, along with quantity, name, and payment type inputs
 * @param {*} response data from the server on the unique menu items for a specific location chosen
 */
function UpdatedMenu(response){
    ConfirmOrder(response);
    $("#order_form").html("");

    const container = $(`<div class='container' id='menu'></div>`);
    const orderIdLabel = $(`<label for='order_id'>Order Id</label>`);
    const orderIdInput = $(`<input type='text' name='order_id' id='order_id' value='${response["orderId"]}' disabled>`);
    const customerIdLabel = $(`<label for='order_customer_id'>Customer Id</label>`);
    const customerIdInput = $(`<input type='text' name='order_customer_id' id='order_customer_id' value='${response["customerId"]}' placeholder='Customer Id' disabled>`);
    const label = $(`<label for='menu-items'>Select from Menu: </label>`);
    const list = $(`<select name='menu-items'></select>`);
    const placeholder = $(`<option selected disabled>Select Item</option>`);
    list.append(placeholder);
    console.log(response); // diagnostics
    // get items from server and insert them
    response["items"].forEach(item => {
        console.log(item);
        // diagnostics
        console.log(item["itemName"]); // item
        console.log(item["itemPrice"]); // price
        console.log("id: " + item["itemName"].replaceAll(" ", "_")); // id
        const id = item["itemName"].replaceAll(" ", "_");
        const price = item["itemPrice"];
        let menuItem = $(`<option value='${price}' name='${item["itemName"]}' id='${id}'>${item["itemName"]} - $${price}</option>`);

        if(item["itemName"] == response["selectedItem"])
            menuItem = $(`<option value='${price}' name='${item["itemName"]}' id='${id}' selected>${item["itemName"]} - $${price}</option>`);

        list.append(menuItem);
    });
    // payment options
    const paymentOptions = ["Credit", "Debit", "Cash"];
    const paymentTypeLabel = $(`<label for='payment-type'>Payment Type: </label>`);
    const paymentTypeSelect = $(`<select name='payment-type' id='payment-type'></select>`);
    paymentOptions.forEach(option => {
        let addOption = $(`<option value='${option}'>${option}</option>`);
        if(option == response["payment"])
            addOption = $(`<option value='${option}' selected>${option}</option>`);
        $(paymentTypeSelect).append(addOption);
    });

    const quantityInput = $(`<div id='quantity-input'><label for='quantity'>Quantity: </label><input type='number' name='quantity' id='quantity' value=${response["selectedQuantity"]} min=0></div>`);
    
    const locationLabel = $(`<label for='pickup_location'>Pickup Location: </label>`);
    const locationSelect = $(`<select name='pickup_location' disabled></select>`);
    const locationPrompt = $(`<option value=''>Pick a Location</option>`);
    locationSelect.append(locationPrompt);
    
    // adding the locations from the server to the dom
    response["locations"].forEach(element => {
        let location = $(`<option value='${element["locationName"]}'>${element["locationName"]}</option>`);
        if(element["locationName"] == response["selectedLocation"])
            location = $(`<option value='${element["locationName"]}' selected>${element["locationName"]}</option>`);

        locationSelect.append(location);
    });

    const updateOrder = $(`<button type='button' id='updateOrder'>Update Order</button>`); 
    appendAll(container, orderIdLabel, orderIdInput, customerIdLabel, customerIdInput, label, list, quantityInput, paymentTypeLabel, paymentTypeSelect, locationLabel, locationSelect, updateOrder);
    $("#order_form").append(container);

    console.log(response["time"]);
    // event handler for order submission button
    UpdateOrderEvents(response["time"]);
}
function UpdateOrderEvents(time){
    $(`#updateOrder`).click((e)=>{
        // getting order details
        const orderId = $(`#order_id`).val();
        const customerId = $(`#order_customer_id`).val();
        const itemQuantity = $(`#quantity`).val();
        const location = $(`select[name='pickup_location']`).val();
        const paymentChoice = $(`#payment-type`).val();
        const itemName = $(`select[name='menu-items'] option:selected`).attr("name");

        // make ajax call
        CallAjax(baseURL + "/updateOrder", "PUT", {"time": toString(time), "orderId": orderId, "id": customerId, "itemName": itemName, "itemQuantity": itemQuantity, "payment": paymentChoice, "location": location}, "json", UpdatedMenu, AjaxError);
    });
}
/***********************************************************/
/**
 * WelcomeMessageServerConnect will connect to the server onload and put up a welcome message
 */
function WelcomeMessageServerConnect(){
    CallAjax(baseURL, "get", {}, "json", WelcomeMessage, AjaxError);
}
/**
 * WelcomeMessage renders the welcome message from the server in the dom 
 * @param {*} response data received from the ajax request onload
 */
function WelcomeMessage(response){
    console.log(response); // diagnostics
    const container = $(`<div class='container' id='welcome'></div>`);
    const icon = $(`<img id='logo' src='./assets/timh_logo.png' alt='Tim Horton's welcome'>`)
    const welcomeMessage = $(`<h3>${response}</h3>`);
    appendAll(container, icon, welcomeMessage);
    $("header").append(container);
}
/**
 * TimHortonsLocationsServerConnect makes an ajax call to the server when a location is selected
 */
function TimHortonsLocationsServerConnect(){
    CallAjax(baseURL + "/location", "get", {}, "json", TimHortonsLocations, AjaxError);
}
function TimHortonsLocations(response){
    console.log(response); // diagnostics
    const container = $(`<div class='container' id='order_history'></div>`);
    const customerIdLabel = $(`<label for='customer_id'>Customer Id</label>`);
    const customerIdInput = $(`<input type='text' name='customer_id' id='customer_id' placeholder='Insert Customer Id'>`);
    const locationLabel = $(`<label for='prev_order_location'>Select Location: </label>`);
    const locationSelect = $(`<select name='prev_order_location' id='prev_order_location'></select>`);
    const locationPrompt = $(`<option value=''>Pick a Location</option>`);
    locationSelect.append(locationPrompt);
    
    // adding the locations from the server to the dom
    response.forEach(element => {
        const location = $(`<option value='${element["locationName"]}'>${element["locationName"]}</option>`);
        locationSelect.append(location);
    });
    // get items from server and insert them 
    appendAll(container, customerIdLabel, customerIdInput, locationLabel, locationSelect);
    $("#retrieve_input").append(container);

    // check what location is selected
    InputEvents();
}
/**
 * LocationSelected is the event handler for the location selection, which makes an ajax call to get the location's unique menu
 */
function InputEvents(){
   $(`#order_history>select`).on("change", (e)=>{
        const location = e.target.value;
        const customerId = $(`#customer_id`).val();

        if(customerId == "") // guard statement
            return;

        CallAjax(baseURL + `/customerOrders/${customerId}/${location}`, "get", {}, "json", RetrieveTable, AjaxError);
    });
    $(`#customer_id`).blur((e)=>{
        const location = $(`#order_history>select`).val();
        const customerId = $(`#customer_id`).val();

        if(location == "") // guard statement
            return;

        CallAjax(baseURL + `/customerOrders/${customerId}/${location}`, "get", {}, "json", RetrieveTable, AjaxError);
    });
}
/**
 * RetrieveTable() renders a table from the information received from the database (students with first names starting with E or F)
 * @param {object} response the response data from the ASP.NET backend
 */
function RetrieveTable(response){
    console.log(response);
    // reset output div
    $("#retrieve_output").html("");

    // output of the number of rows returned in the query
    StatusMessage("#retrieve_status", response["status"]);
    if(!response["data"] || response["data"].length <= 0)
        return;
    // creating the new table to be rendered
    const table =  $(`<table>`);
    const header = RenderHeader("Order Id", "Order Date", "Payment Method", "Item Name", "Item Price", "Item Count", "Delete");
    $(table).append(header);

    // creating rows from titles table
    $(response["data"]).each((index, row) => {
        if(index == 0) // headers returned form the server
            return;
        const newRow = RenderRow(row, true);
        // append the populated row into the table
        $(table).append(newRow);
        
    });
    // append table to form, then form to dom
    $("#retrieve_output").append(table);

    // Delete event handler
    DeleteEvents();
}
function DeleteEvents(){
    $(`#retrieve_output>table>tr`).click((e)=>{
        const location = $(`#order_history>select`).val();
        const customerId = $(`#customer_id`).val();
        const orderId = e.currentTarget.id;
        const selector = e.target.className;
        if(selector == "delete"){
            CallAjax(baseURL + `/deleteOrder/${orderId}/${customerId}/${location}`, "delete", {}, "json", RetrieveTable, AjaxError);
        }
    });
}
/**
 * RenderHeader() generates a header based on the provided argument names
 * @param  {...any} titles titles to be added into the first row of a table
 * @returns a header dom element containing the titles specified
 */
function RenderHeader(...titles){
    const header = $(`<tr>`);

    // adding titles
    titles.forEach(title => {
        const th = $(`<th>`);
        th.text(title);
        $(header).append(th);
    });
    return header;
}
/**
 * RenderRow() is a general dom manipulation function that will generate and populate information into the a row in a table
 * @param {object} row a single row from the database based on the query that was submitted
 * @param {bool} button true will render a button at the first position, false will skip the button
 * @returns 
 */
function RenderRow(row, button = false){
    const newRow = $(`<tr id='${row["orderId"]}'></tr>`);

    // iterate through returned row and populate the row
    $.each(row, (key, value) => {
        $(newRow).append(addDataCell(value));
    });
    if(button){
        // retrieve button
        const button = $(`<td><button class='delete'>Delete</button></td>`);
        $(newRow).append(button);
    }
    return newRow;
}
/**
 * addDataCell() creates a td element to be inserted into a row within a table
 * @param {*} data can be anything that will be put within the data cell of a table
 * @returns a td DOM element with a user specified data affixed to it
 */
function addDataCell(data){
    return $(`<td>${data}</td>`);
}
/**
 * StatusMessage is a general helper function that will append messages to a parent container
 * @param {*} parent 
 * @param  {...any} messages 
 */
function StatusMessage(parent, ...messages){
    $(parent).html("");
    messages.forEach(message => {
        const input = $(`<p>${message}</p>`);
        $(parent).append(input);
    });
}
// dom manipulation functions to add to library file later
function appendAll(parent, ...childNodes){
    childNodes.forEach(child => {
        parent.append(child);
    });
}

