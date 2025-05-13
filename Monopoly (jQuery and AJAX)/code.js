/***************************************variable declaration*******************************************/ 
const takeAChanceText = ["Second Place in Beauty Contest: $10", "Bank Pays You Dividend of $50","Repair your Properties. You owe $250", "Speeding Fine: $15", "Holiday Fund Matures: Receive $100", "Pay Hospital Fees: $100"];
const takeAChanceMoney = [10, 50, -250, -15, 100, -100];
let altArray = ["Green among us character", "Purple among us character"];
let colors = ["brown", "lightblue", "purple", "orange", "red", "yellow", "green", "blue"];
let preloadDice = [];
let players = [];
//player number and alt information
let numPlayers = 2;
//dice value
let diceSum = 0;
let numSteps = 0;
let repeat = false;
//player turn
let player = 0;
//timer for animations
let timerID = 0;

/******************************************event handler***********************************************/
$(document).ready(()=>{    
    //variables
    let rollDice = $("#RollDice");
    window.alert("Welcome to Monopoly! Make sure to load property purchase prices first by clicking the 'Load Property Prices' button when the game loads");
    rollDice.prop("disabled", true);

    //function calls
    fPreloadDice();
    fPreLoadPlayers();
    fBoardInit();
    fIconLoad();
    fIndicator();
    $("#loadPrice").click(fLoadPrices);
    rollDice.click(fRollDice);
});

/******************************************pseudo class***********************************************/
function Player(Number, Alt){
    this.player = Number;
    this.total = 3000;
    this.position = 0;
    this.playerIcon = new Image();
    this.playerIcon.src = `./images/Player${Number}.png`;
    this.playerIcon.alt = Alt;
}

/********************************************functions************************************************/

/**************************************************AJAX***************************************************/
/*
this assignment requires:
    type = “POST”
    dataType = “JSON”
    url = https://thor.cnt.sast.ca/~aulakhha/filesAssLab/lab3.php
*/
//re-usable ajax request function
function AJAXCall(url, type, datatype, data, success, error){
    let ajaxOptions = {};
    //the orange colored text is IMPORTANT - NEED TO HAVE IT TYPED CORRECTLY
    ajaxOptions['url'] = url;
    ajaxOptions['type'] = type;
    ajaxOptions['dataType'] = datatype;
    ajaxOptions['data'] = data;

    $.ajax(ajaxOptions).done(success).fail(error).always(jsonAlways());
}
//general always message to demonstrate that the function is correctly being calledback
function jsonAlways(textStatus){
    console.log("Always look both ways when crossing");
}
function PriceSuccess(responseData){
    console.log(responseData);
    let board = $("section");
    //load prices from ajax call
    for(let i = 0; i < board.length; i++){
        if(responseData[i] > 0){
            board.eq(i).append(` $${responseData[i]}`).attr("val", responseData[i]);
        }
    }
    $("#loadPrice").prop("disabled", true);
}
function DiceSuccess(responseData){
    console.log(responseData);
    let dice = $("img.die");

    if(responseData.dice1 == responseData.dice2){
        repeat = true;
    }
    //change dice image based on random dice value
    dice[0].src = `./images/dice${responseData.dice1}.png`;
    dice[1].src = `./images/dice${responseData.dice2}.png`;
    //sum the dice values
    diceSum = responseData.dice1 + responseData.dice2;
    //value needed for because setInterval can't take parameters
    numSteps = diceSum;
    fMove();
}
//general error message if the json data/request is incorrect
function jsonError(ajaxRequest, textStatus, errorThrown){
    console.log('Error!! ' + textStatus + ': ' + errorThrown);
    window.alert('Error!! ' + textStatus + ': ' + errorThrown);
}
/*******************************************************************************************************/

//load prices
function fLoadPrices(){
    AJAXCall("https://thor.cnt.sast.ca/~aulakhha/filesAssLab/lab3.php", "POST", "JSON", {"action":"propertyPrices"}, PriceSuccess, jsonError);
    $("#RollDice").prop("disabled", false);
}
//reorganizing the grid
function fBoardInit(){

    let suite = $("section");
    //separating rr and cc information from the "suite" attribute then assigning to the corresponding grid area
    for(let i = 0; i < suite.length; i++){
        suite[i].attributes[0].value = suite[i].attributes[0].value.substring(0, 2)+"/"+suite[i].attributes[0].value.substring(2, 4);
        suite[i].style.gridArea = suite[i].attributes[0].value;
        suite.eq(i).attr("rent", 0);

        //splitting the string in class attribute to get color information
        let color = suite[i].attributes[2].value.substring(4);
        let row = suite[i].attributes[0].value.substring(0, 2);
        let col = suite[i].attributes[0].value.substring(3);
        //specifying border direction based on row or col
        if(row == '01')
            suite[i].style.borderBottom = `15px solid ${color}`;   
        if(row == '11')
            suite[i].style.borderTop = `15px solid ${color}`;
        if(col == '01')
            suite[i].style.borderRight = `15px solid ${color}`;
        if(col == '11')
            suite[i].style.borderLeft = `15px solid ${color}`;
    }
    console.log("Board arranged");
}

//preload dice images
function fPreloadDice(){
    //number of dice images to be preloaded
    let numDice = 6;
    for(let d = 1; d <= numDice; d++){
        let temp = new Image();
        temp.src = `./images/dice${d}.png`;
        preloadDice.push(temp);
    }
    console.log("Dice preloaded");
}

//preload player images
function fPreLoadPlayers(){
    //iterating through number of players to preload icons
    for(let p = 1; p <= numPlayers; p++){
        let temp = new Player(p, altArray[p-1]);
        players.push(temp);
    }
    console.log("Player icons preloaded");
}

//displaying player icons on the board
function fIconLoad(){
    for(let i = 0; i< numPlayers; i++){
        let playerIcon = $(`#player${i+1}`);
        let playerStart = $(`#go`);
        //creating new image nodes
        let playerIcons = fImgLoadHelper(i);
        //for center display
        playerIcon.append(playerIcons);
        //for board start
        playerStart.append(players[0].playerIcon);
        playerStart.append(players[1].playerIcon);
    }
    console.log("Player icons loaded");
}

//creates the new image objects and returns it
function fImgLoadHelper(index){
    //create new image object
    let newNode = new Image();
    //take information from Players pseudo class
    newNode.src = players[index].playerIcon.src;
    newNode.alt = players[index].Alt;
    //return new image with copied information
    return newNode;
}

//dice roll function
function fRollDice(){

    AJAXCall("https://thor.cnt.sast.ca/~aulakhha/filesAssLab/lab3.php", "POST", "JSON", {"action": "diceroll"}, DiceSuccess, jsonError);

}

//helper function returning a random number
function fRandomizer(min, max){
    //generates random number between min(inclusive) and max(exclusive)
    return Math.floor(Math.random()*(max-min))+min;
}

//indicates which player is playing
function fIndicator(){
    //for indicating which player is playing
    let player1Img = $("#player1>img");
    let player2Img = $("#player2>img");
    
    if(player == 0){
        player1Img.attr("class", "active");
        player2Img.removeAttr("class");
        console.log("P2's just moved");
    }
    else{
        player1Img.removeAttr("class");
        player2Img.attr("class", "active");
        console.log("P1's just moved");
    }
}
//animates the icons
function fMove(){
    //set animation speed if there are steps to be taken
    let interval = (numSteps > 0)?600:-1;
    //button disabled during play
    $("#RollDice").prop("disabled", true);
    //turn on the animation
    timerID = setInterval(fUpdatePosition, interval);
}
//increments and moves the icon
function fUpdatePosition(){
    //increment player position and cap at 40 
    players[player].position++;
    players[player].position = (players[player].position % $("section").length);
    //move player icon
    $("section")[players[player].position].append(players[player].playerIcon);
    numSteps--;

    //check if Go is passed or landed on
    fCheckGo();

    //end animation
    if(numSteps <= 0){
        fCheckEndPosition();
        clearInterval(timerID);
        //button enabled again
        $("#RollDice").prop("disabled", false);
        if(repeat){
            player++;
            repeat = false;
        }
        player++;
        player = player%numPlayers;
        fIndicator();
    }
}
//checks if Go is passed or landed on
function fCheckGo(){
    if(players[player].position == 0){
        players[player].total += 200;
        fUpdateCash();
    }
}
//updates the cash display
function fUpdateCash(){
    let opPlayer = (player+1)%numPlayers;

    //if a player goes backrupt, end game
    if(players[player].total <= 0){
        window.alert(`Player ${player+1} loses!`);
        location.reload();
    }
    if(players[opPlayer].total <= 0){
        window.alert(`Player ${opPlayer+1} loses!`);
        location.reload();
    }

    $(`#player${player+1}amt`).text(`$${players[player].total}`);
    $(`#player${opPlayer+1}amt`).text(`$${players[opPlayer].total}`)
}
//checks the landing position and transfers money accordingly
function fCheckEndPosition(){
    //must be .eq() to return a jQuery object ($("section")[players[playerNum].position] is a DOM object)
    let playerPos = $("section").eq(players[player].position);
    let opPlayer = (player+1)%numPlayers;
    
    /*********************************************Wild Cards************************************************/
    //Taxes
    if(playerPos.hasClass("tax")){
        players[player].total -= playerPos.attr("val");
        window.alert(`Player ${player+1} has been taxed: $${playerPos.attr("val")}`);
    }
    //Go to Jail
    if(playerPos.hasClass("goToJail")){
        //player sent to jail position
        players[player].position = 10;
        $("section")[players[player].position].append(players[player].playerIcon);
        window.alert(`Player ${player+1} is going to jail`);
    }
    //Jail
    if(playerPos.hasClass("jail")){
        players[player].total -= playerPos.attr("val");
        window.alert(`Player ${player+1} is in jail and has been charged: $${playerPos.attr("val")}`);
    }
    //Community Chest and Chance
    if(playerPos.attr("val") == -1){
        let randIndex = fRandomizer(0, 6);
        players[player].total += takeAChanceMoney[randIndex];
        window.alert(takeAChanceText[randIndex]);
    }
    /************************************************Rental Properties*****************************************************/
    if(playerPos.hasClass(`p${opPlayer+1}`)){
        //utilities
        if(playerPos.hasClass("utility")){
            players[player].total -= Number(5*diceSum);
            players[opPlayer].total += Number(5*diceSum);
            window.alert(`Player ${player+1} paid $${5*diceSum} and Player ${opPlayer+1} gained $${5*diceSum}`);
        }

        //railroads
        if(playerPos.hasClass("rr")){
            //check number of railroads own
            let railsOwned = 0;
            for(let i = 0; i < $("section").length; i++){
                if($("section").eq(i).hasClass("rr") && $("section").eq(i).hasClass(`p${opPlayer+1}`)){
                    railsOwned++;
                }
            };
            //rental charges
            players[player].total -= Number(25*railsOwned);
            players[opPlayer].total += Number(25*railsOwned);
            window.alert(`Player ${player+1} paid $${25*railsOwned} and Player ${opPlayer+1} gained $${25*railsOwned}`);
        }

        //colored
        let rentPrice = 0;
        colors.forEach(color =>{
            if(playerPos.hasClass(color)){
                //first rental charge on a colored property
                if(playerPos.attr("rent") <= 0){
                    rentPrice = Math.round(playerPos.attr("val")*0.10);
                    playerPos.attr("rent", rentPrice);
                }
                //subsequent rental charges on a colored proptery
                else if(playerPos.attr("rent") > 0){
                    
                    rentPrice = Math.round(playerPos.attr("rent")*1.2);
                    playerPos.attr("rent", rentPrice);
                }
                players[player].total -= Number(playerPos.attr("rent"));
                players[opPlayer].total += Number(playerPos.attr("rent"));
                window.alert(`Player ${player+1} paid $${playerPos.attr("rent")} and Player ${opPlayer+1} gained $${playerPos.attr("rent")}`);
            }
        });
    }
    /************************************************Buy Properties*****************************************************/
    if(!playerPos.hasClass("owned") && !playerPos.hasClass("tax") && playerPos.attr("val")>50){
        playerPos.addClass(`owned p${player+1}`);
        players[player].total -= Number(playerPos.attr("val"));
    }
    fUpdateCash();
}



/******************************************MANUAL MOVE**********************************************/
let diceTotal = 0; 
function fManMove(playerNum, steps){
    let manualID = 0;
    diceTotal = steps;
    let interval = (steps > 0)?600:-1;
    manualID = setInterval(fManualUpdate, interval);
    fManIndicator();

    function fManualUpdate(){
        //increment player position and cap at 40 
        players[playerNum].position++;
        players[playerNum].position = (players[playerNum].position % $("section").length);
        //move player icon
        $("section")[players[playerNum].position].append(players[playerNum].playerIcon);
        steps--;

        //check if Go is passed or landed on
        if(players[playerNum].position == 0){
            players[playerNum].total += 200;
            fManUpdateCash();
        }

        //button disabled during play
        $("#RollDice").prop("disabled", true);
        $("section")[players[playerNum].position].append(players[playerNum].playerIcon);

        //fMove() call back to end the animation
        if(steps <= 0){
            fManCheckEndPosition();

            clearInterval(manualID);
            //button enabled again
            $("#RollDice").prop("disabled", false);
        }
    }
    function fManIndicator(){
        //for indicating which player is playing
        let player1Img = $("#player1>img");
        let player2Img = $("#player2>img");
        
        if(playerNum == 0){
            player1Img.attr("class", "active");
            player2Img.removeAttr("class");
            console.log("P2's just moved");
        }
        else{
            player1Img.removeAttr("class");
            player2Img.attr("class", "active");
            console.log("P1's just moved");
        }
    }
    function fManUpdateCash(){
        let opPlayer = (playerNum+1)%numPlayers;
        $(`#player${playerNum+1}amt`).text(`$${players[playerNum].total}`);
        $(`#player${opPlayer+1}amt`).text(`$${players[opPlayer].total}`)
    }
    function fManCheckEndPosition(){
        //must be .eq() to return a jQuery object ($("section")[players[playerNum].position] is a DOM object)
        let playerPos = $("section").eq(players[playerNum].position);
        let opPlayer = (playerNum+1)%numPlayers;

        /*********************************************Wild Cards************************************************/
        //Taxes
        if(playerPos.hasClass("tax")){
            players[playerNum].total -= Number(playerPos.attr("val"));
            window.alert(`Player ${playerNum+1} has been taxed: $${playerPos.attr("val")}`);
        }
        //Go to Jail
        if(playerPos.hasClass("goToJail")){
            //player sent to jail position
            players[playerNum].position = 10;
            $("section")[players[playerNum].position].append(players[playerNum].playerIcon);
            window.alert(`Player ${playerNum+1} is going to jail`);
        }
        //Jail
        if(playerPos.hasClass("jail")){
            players[playerNum].total -= Number(playerPos.attr("val"));
            window.alert(`Player ${playerNum+1} is in jail and has been charged: $${playerPos.attr("val")}`);
        }
        //Community Chest and Chance
        if(playerPos.attr("val") == -1){
            let randIndex = fRandomizer(0, 6);
            players[playerNum].total += takeAChanceMoney[randIndex];
            window.alert(takeAChanceText[randIndex]);
        }

        /************************************************Rental Properties*****************************************************/
        if(playerPos.hasClass(`p${opPlayer+1}`)){
            //utilities
            if(playerPos.hasClass("utility")){
                players[playerNum].total -= Number(5*diceTotal);
                players[opPlayer].total += Number(5*diceTotal);
                window.alert(`Player ${playerNum+1} paid $${(5*diceTotal)} and Player ${opPlayer+1} gained $${(5*diceTotal)}`);
            }

            //railroads
            //check number of railroads own
            if(playerPos.hasClass("rr")){
                let railsOwned = 0;
                for(let i = 0; i < $("section").length; i++){
                    if($("section").eq(i).hasClass("rr") && $("section").eq(i).hasClass(`p${opPlayer+1}`)){
                        railsOwned++;
                    }
                };
                //rental charges
                players[playerNum].total -= Number(25*railsOwned);
                players[opPlayer].total += Number(25*railsOwned);
                window.alert(`Player ${playerNum+1} paid $${(25*railsOwned)} and Player ${opPlayer+1} gained $${(25*railsOwned)}`);
            }

            //colored
            colors.forEach(color =>{
                let rentPrice = 0;
                if(playerPos.hasClass(color)){
                    //first rental charge on a colored property
                    if(playerPos.attr("rent")<=0){
                        rentPrice = Math.round(playerPos.attr("val")*0.10);
                        playerPos.attr("rent", rentPrice);
                    }
                    //subsequent rental charges on a colored proptery
                    else if(playerPos.attr("rent")>0){
                        
                        rentPrice = Math.round(playerPos.attr("rent")*1.2);
                        playerPos.attr("rent", rentPrice);
                    }
                    players[playerNum].total -= Number(playerPos.attr("rent"));
                    players[opPlayer].total += Number(playerPos.attr("rent"));
                    window.alert(`Player ${playerNum+1} paid $${playerPos.attr("rent")} and Player ${opPlayer+1} gained $${playerPos.attr("rent")}`);
                }
            });
        }
        /************************************************Buy Properties*****************************************************/
        if(!playerPos.hasClass("owned") && !playerPos.hasClass("tax") && playerPos.attr("val")>50){
            playerPos.addClass(`owned p${playerNum+1}`);
            players[playerNum].total -= Number(playerPos.attr("val"));
        }
        fManUpdateCash();
    }
}

