/**
 * appendAll appends all jQuery childnodes to a parent node
 * @param {*} parent the parent jQuery node to be appended to
 * @param  {...any} childNodes the child jQuery nodes to append
 */
function appendAll(parent, ...childNodes){
    childNodes.forEach(child => {
        $(parent).append(child);
    });
}
/**
 * OutputMessage is a general helper function that will append messages to a parent container
 * @param {*} parent 
 * @param  {...any} messages 
 */
function StatusMessage(parent, ...messages){
    $(parent).html("");
    messages.forEach(message => {
        const output = $(`<p>${message}</p>`);
        $(parent).append(output);
    });
}
/**
 * generateButton() returns a button dom element and attaches a label, name, value, id, and class to it
 * @param {string} label the label that users will see directly on the webpage (*requred)
 * @param {string} name the name to use with the $POST request (optional)
 * @param {string} value the value of the specific button (optional)
 * @param {string} id the id of the button (optional)
 * @param {string} className the class of the button (optional)
 * @returns a button dom element with associated labels, name, value, and classes
 */
function generateButton(label, name = "", value = "", id = "", className = ""){
    const button = document.createElement("button");
    button.textContent = label;
    const attributes = {
        type: "button",
        name: name,
        class: className,
        id: id,
        value: value
    };
    addAttributes(button, attributes);

    return button;
}
/**
 * GenerateTable() creates a table with the supplied titles in the table headers
 * @param  {...any} titles uses rest parameters to take in an infinite number of arguments for title names
 * @returns a DOM table with the user specified titles
 */
function generateTable(...titles){
    const table = document.createElement("table");
    const header = document.createElement("tr");

    // adding titles
    titles.forEach(title => {
        const th = document.createElement("th");
        th.textContent = title;
        titles.appendChild(th);
    });
    table.appendChild(header);

    return table;
}
/**
 * addAttributes() affixes user specified attributes to a specific DOM element
 * @param {*} element the parent element to have the attributes affixed to
 * @param {*} attr any attributes that can below to a DOM element, must be sent in as an object with key=>value pairs
 */
function addAttributes(element, attr){
    Object.keys(attr).forEach((name) => {
        element.setAttribute(name, attr[name]);
    });
}
/**
 * addDataCell() creates a td element to be inserted into a row within a table
 * @param {*} data can be anything that will be put within the data cell of a table
 * @param {*} selector class identifier to the td
 * @returns a td DOM element with a user specified data affixed to it
 */
function addDataCell(data, selector = ""){
    let returnElement = $(`<td>${data}</td>`);
    if(selector != "")
        returnElement = $(`<td class='${selector}'>${data}</td>`)
    return returnElement;
}
// rest parameter (...) to take in unlimited child arguments 
/**
 * appendAllChildren() appends all user specified child elements into a user specified parent element 
 * @param {*} parent the primary DOM element the function will append child elements to
 * @param  {...any} children using a rest parameter to take in unlimited child elements to append to the parent
 */
function appendAllChildren(parent, ...children){
    children.forEach(child => {
        parent.appendChild(child);
    });
}
/**
 * RenderRow() is a general dom manipulation function that will generate and populate information into the a row in a table
 * @param {object} row a single row from the database based on the query that was submitted
 * @param {bool} button true will render a button at the first position, false will skip the button
 * @returns 
 */
function RenderOutputRows(row, button = false){
    const newRow = $(`<tr id='${row[0]}'></tr>`);

    if(button){
        // retrieve button
        const retrieveButton = $(`<td><button class='retrieve'>Retrieve Class Info</button></td>`);
        $(newRow).append(retrieveButton);
    }
    // iterate through returned row and populate the row
    $.each(row, (key, value) => {
        switch (key){
            case 0:
                $(newRow).append(addDataCell(value, "class_id"));
                break;
            case 1:
                $(newRow).append(addDataCell(value, "class_desc"));
                break;
            case 2:
                $(newRow).append(addDataCell(value, "days"));
                break;
            case 3:
                $(newRow).append(addDataCell(value, "start_date"));
                break;
            case 4:
                $(newRow).append(addDataCell(value, "instructor_id"));
                break;
            case 5:
                $(newRow).append(addDataCell(value, "instructor_first"));
                break;
            case 6:
                $(newRow).append(addDataCell(value, "instructor_last"));
                break;    
        }
        
    });
    if(button){
        const actionButtons = $(`<td class='action'><button class='delete'>Delete</button><button class='edit'>Edit</button></td>`);
        $(newRow).append(actionButtons);
    }

    return newRow;
}
/**
 * RenderRow() is a general dom manipulation function that will generate and populate information into the a row in a table
 * @param {object} row a single row from the database based on the query that was submitted
 * @param {bool} button true will render a button at the first position, false will skip the button
 * @returns 
 */
function RenderRow(row, button = false){
    const newRow = $(`<tr id='${row[0]}'></tr>`);

    if(button){
        // retrieve button
        const retrieveButton = $(`<td><button class='retrieve'>Retrieve Class Info</button></td>`);
        $(newRow).append(retrieveButton);
    }
    // iterate through returned row and populate the row
    $.each(row, (key, value) => {
        switch (key){
            case 0:
                $(newRow).append(addDataCell(value, "student_id"));
                break;
            case 1:
                $(newRow).append(addDataCell(value, "last_name"));
                break;
            case 2:
                $(newRow).append(addDataCell(value, "first_name"));
                break;
            case 3:
                $(newRow).append(addDataCell(value, "school_id"));
                break;   
        }
        
    });
    if(button){
        const actionButtons = $(`<td class='action'><button class='delete'>Delete</button><button class='edit'>Edit</button></td>`);
        $(newRow).append(actionButtons);
    }

    return newRow;
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
function ToggleEdit(row, edit = false){
    const action = $(`tr#${row}>td.action`);

    action.html("");
    if(edit){
        const prevFirstName = $(`tr#${row}>td.first_name`);
        const prevLastName = $(`tr#${row}>td.last_name`);
        const prevSchoolId = $(`tr#${row}>td.school_id`);
        const firstName = $(`<input type='text' value='${prevFirstName.text()}' class='first_name'>`);
        const lastName = $(`<input type='text' value='${prevLastName.text()}' class='last_name'>`);
        const schoolId = $(`<input type='text' value='${prevSchoolId.text()}' class='school_id'>`);
        // resets
        prevFirstName.html("");
        prevLastName.html("");
        prevSchoolId.html("");

        // change first name, last name, school id to inputs
        prevFirstName.append(firstName);
        prevLastName.append(lastName);
        prevSchoolId.append(schoolId);

        // change action buttons
        const updateButton = $(`<button class='update'>Update</button><button class='cancel'>Cancel</button>`);
        action.append(updateButton);
    }
    else{
        // selectors for the parent td elements
        const tdFirstName = $(`tr#${row}>td.first_name`);
        const tdLastName = $(`tr#${row}>td.last_name`);
        const tdSchoolId = $(`tr#${row}>td.school_id`);

        // input values
        const prevFirstName = $(`tr#${row}>td.first_name>input`);
        const prevLastName = $(`tr#${row}>td.last_name>input`);
        const prevSchoolId = $(`tr#${row}>td.school_id>input`);

        // getting previous values
        const firstName = prevFirstName.attr("value");
        const lastName = prevLastName.attr("value");
        const schoolId = prevSchoolId.attr("value");
        
        // resets
        tdFirstName.html("");
        tdLastName.html("");
        tdSchoolId.html("");

        // put original text back in
        tdFirstName.text(firstName);
        tdLastName.text(lastName);
        tdSchoolId.text(schoolId);

        // change first name, last name, school id to inputs
        prevFirstName.append(firstName);
        prevLastName.append(lastName);
        prevSchoolId.append(schoolId);

        const actionButtons = $(`<button class='delete'>Delete</button><button class='edit'>Edit</button>`);
        action.append(actionButtons);
    }
}
function addInput(title, name = "", type = "text", placeholder = ""){
    const parent = $("<div>");
    // add label
    const labelContainer = $(`<div>`);
    const label = $(`<label for='${name}'>${title}</label>`);
    labelContainer.append(label);
    // add input
    const inputContainer = $("<div>");
    const input = $(`<input type='${type}' name='${name}' id='${name}' placeholder='${placeholder}'>`);
    inputContainer.append(input);

    // package everything into a container to make styling easier
    appendAll(parent, labelContainer, inputContainer);

    return parent;
}
function addClassDropdown(classes, ids){
    // div to hold label/input combo
    const parent = $("<div>");
    // add label
    const labelContainer = $("<div>");
    const label = $("<label for='select_class'>Class ID: </label>");
    labelContainer.append(label);
    // add select
    const selectContainer = $("<div>");
    const select = $(`<select id='select_class' name='select_class' multiple></select>`);
    // placeholder option - no value
    const placeholder = $(`<option disabled>Choose a Class</option>`);
    $(select).append(placeholder);
    classes.forEach((choice, index) => {
        const option = $(`<option value='${ids[index]}'>${choice}</option>`);
        select.append(option);
    });
    
    selectContainer.append(select);

    parent.append(labelContainer);
    parent.append(selectContainer);
    return parent;
}