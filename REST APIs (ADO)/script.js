/**
 * CMPE 2550 - Web Applications
 * Author: Jonathan Le
 * Date: Nov. 25, 2025
 * Purpose: demonstration of pulling student data from an SSMS database with a ASP backend
 * 
 */
console.log("script.js connected");

const baseURL = "https://localhost:7174";

$(document).ready(function(){
    // don't want to re-render new student form, so will separate from retrieve table
    CallAjax(baseURL + "/Classes", "get", {}, "json", NewStudentForm, AjaxError);
    CallAjax(baseURL, "get", {}, "json", RetrieveTable, AjaxError); 
});
/**
 * NewStudentForm() renders the form for adding a new student, with inputs for names, school id, and classes
 * @param {*} response json data from the backend 
 */
function NewStudentForm(response){
    CallAjax(baseURL, "get", {}, "json", RetrieveTable, AjaxError); 
    let ids = [];
    let classes = [];
    // separate id and desc
    response["data"].forEach((data, index) => {
        if(index == 0)
            return;
        ids.push(data[0]);
        classes.push(data[1]);
    });

    // reset the new student form
    $("#insert").html("");
    const header = $(`<h3>Add New Student</h3>`);
    $("#insert").append(header);

    // adding the form elements and inputs
    const titles = ["First Name: ", "Last Name: ", "School ID: ", "Class: "];
    titles.forEach((title, index) => { 
        let input; // the input type to append to the insert div
        switch(title){ // each title will have different input types
            case "First Name: ":
                // validate id is <= 6 chars (2 letters, 4 nums), num only, not negative 
                input = addInput(title, "input_first_name", "text", "First Name Here");
                break;
            case "Last Name: ":
                input = addInput(title, "input_last_name", "text", "Last Name Here");
                break;
            case "School ID: ":
                input = addInput(title, "input_school_id", "text", "School ID Here");
                break;
            case "Class: ":
                input = addClassDropdown(classes, ids);
                break;
        }
        $("#insert").append(input);
    });
    // submit button for the form
    const button = $("<button type='submit' id='add_student'>Add Student</button>");
    $("#insert").append(button);

    // status message for any errors that may occur
    StatusMessage("#insert_status", (response["status"] == undefined)?"":response["status"]);

    // add student event handler
    AddStudentEvents();
}
/**
 * AddStudentEvents() holds all of the event handlers for the new student form
 */
function AddStudentEvents(){
    $("#add_student").click(()=>{
        const firstName = $(`#input_first_name`).val();
        const lastName = $(`#input_last_name`).val();
        const schoolId = $(`#input_school_id`).val();
        const classes = $(`#select_class`).val();

        CallAjax(baseURL + "/AddStudent", "post", {"firstName": firstName, "lastName": lastName, "schoolId": schoolId, "classes": classes} , "json", NewStudentForm, AjaxError);
    });
}
/**
 * RetrieveTable() renders a table from the information received from the database (students with first names starting with E or F)
 * @param {object} response the response data from the ASP.NET backend
 */
function RetrieveTable(response){
    // reset output div
    $("#display").html("");

    // creating the new table to be rendered
    const table =  $(`<table>`);

    if(response["rows"] > 0){
        const header = RenderHeader("Get Students", "Student Id", "Last Name", "First Name", "School Id", "Action");
        $(table).append(header);

        // creating rows from titles table
        $(response["data"]).each((index, row) => {
            if(index == 0)
                return;
            const newRow = RenderRow(row, true);
            // append the populated row into the table
            $(table).append(newRow);
        });
    }
    // append table to form, then form to dom
    $("#display").append(table);
    // output of the number of rows returned in the query
    StatusMessage("#status", response["status"]);
    
    // hook event handlers
    RetrieveStudentsEvents(); // buttons events
}
/**
 * OutputTable() renders a table with the class information for a selected student from the database
 * @param {object} response the response data from the ASP.NET backend
 */
function OutputTable(response){
    console.log(response);
     // reset output div
    $("#output").html("");

    // creating the new table to be rendered
    const table =  $(`<table>`);


    if(response["rows"] > 0){
        const header = RenderHeader("Class Id", "Class Desc", "Days", "StartDate", "Instructor Id", "Instructor First Name", "Instructor Last Name");
        $(table).append(header);

        // creating rows from titles table
        $(response["data"]).each((index, row) => {
            if(index == 0)
                return;
            const newRow = RenderOutputRows(row);
            // append the populated row into the table
            $(table).append(newRow);
        });
    }
    // append table to form, then form to dom
    $("#output").append(table);
    // output of the number of rows returned in the query
    StatusMessage("#status", response["status"]);
}

/**
 * RetrieveStudentsEvents() holds all of the event hanlders for the onload table
 */
function RetrieveStudentsEvents(){
    $("tr").click((e)=>{
        const button = e.target.className;
        const target = e.currentTarget.id;
        // only register clicks for the retrieve button
        switch (button){
            case "retrieve":
                CallAjax(baseURL + "/GetClassInfo/" + target, "get", {}, "json", OutputTable, AjaxError);
                break;
            case "delete":
                CallAjax(baseURL + "/DeleteStudent/" + target, "delete", {}, "json", RetrieveTable, AjaxError);
                break;
            case "edit":
                ToggleEdit(target, true);
                break;
            case "update":
                const firstName = $(`tr#${target}>td.first_name>input`).val();
                const lastName = $(`tr#${target}>td.last_name>input`).val();
                const schoolId = $(`tr#${target}>td.school_id>input`).val();
                CallAjax(baseURL + "/ChangeStudentInfo", "put", {"id": target, "firstName": firstName, "lastName": lastName, "schoolId": schoolId}, "json", RetrieveTable, AjaxError);
                break;
            case "cancel":
                ToggleEdit(target);
                break;
        }
    });
}