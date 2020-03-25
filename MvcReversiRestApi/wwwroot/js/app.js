const apiUrl = 'url/super/duper/game';

const Game = (function(url){
    console.log('hallo, vanuit een module');

    //Configuratie en state waarden
    let configMap = {
        apiUrl: url
    };

    // Private function init
    const privateInit = function(afterInit){
        console.log(configMap.apiUrl);
        afterInit();
    };

    const _getCurrentGameState = function () {
        Game.Data.s
    };


    // Waarde/object geretourneerd aan de outer scope
    return {
        init: privateInit
    }

})(apiUrl);



Game.Data = (function(){
    console.log('hallo, vanuit module Data');

    let configMap = {
        //apiKey: '7bae2caf9621a6b9d4bdf44399982675',
        mock: [
            {
                url: 'api/Spel/Beurt',
                data: 0
            }
        ]
    };

    const getMockData = function(url){

        const mockData = configMap.mock;

        return new Promise((resolve, reject) => {
            resolve(mockData);
        });

    };

    let stateMap = {
        environment : 'development',
        gamestate : ''

    };

    const get = function(url) {

        if(stateMap.environment === "production") {
            return $.get(url)
                .then(r => {
                    return r
                })
                .catch(e => {
                    console.log(e.message);

                });
        }else if(stateMap.environment === "development") {
            return getMockData(url);
        }
     };


    const privateInit = function(){
        if(stateMap.environment === "development")
        {
            return getMockData();
        }else if(stateMap.environment === "production")
        {
            //doe request aan server
            //xhttp.open("GET", "ajax_info.txt", true);
            //xhttp.send();


        }else
        {
            throw new Error("de environment is geen development of production")
        }
    };

    return{
        init: privateInit,
        get: get
    }

})();
Game.Model = (function(){
    console.log('hallo, vanuit module Model');

    let configMap;

    const privateInit = function(callback){
        console.log("private data");
    };

    const getWeather = function (url) {
        Game.Data.get(url).then(data =>
        {

            if(data.main.temp == null){
                throw new Error("Geen temperatuur")
            }

            console.log(data)
        });

    };

    const _getGameState = function(){

        //aanvraag via Game.Data
        let data = Game.Data.get('/api/Spel/Beurt/<token>');
        //controle of ontvangen data valide is
        if(data === 0){
            //geen waarde

        }else if(data === 1){
            //wit aan zet

        }else if(data === 2){
            //zwart aan zet

        }else{
            throw new Error("ongeldige data")
        }

    };

    return{
        init: privateInit,
        weather: getWeather,
        getGameState: _getGameState
    }

})();
Game.Reversi = (function(){
    console.log('hallo, vanuit module Reversi')

    let configMap;

    const privateInit = function(){
        console.log("private spel");
    };

    return{
        init: privateInit
    }

})();

class FeedbackWidget {
    constructor(elementId) {
        this._elementId = elementId;
    }



    get elementId() { //getter, set keyword voor setter methode
        return this._elementId();
    }

    show(message, type) {
        var x = document.getElementById(this._elementId);
        x.style.display = "block";
        $(x).text(message);
        if(type == "danger"){
            $(x).addClass('alert alert-danger')
            $(x).removeClass('alert alert-success')
        }else if(type == "success"){
            $(x).addClass('alert alert-success')
            $(x).removeClass('alert alert-danger')
        }

        var msg = {message : message, type : type};
        this.log(msg);

console.log(this.history());
    }

    hide() {
        var x = document.getElementById(this._elementId);
        x.style.display = "none";
    }

    count = 1;
    log(message){
        {
            let lowestInt = this.count - 10;
            if(localStorage.length >= 10){
                localStorage.removeItem('feedback_widget'+ (lowestInt));
                console.log("boven de 10");
            }
            localStorage.setItem('feedback_widget'+ (this.count) , JSON.stringify(message));
            this.count++;
        }
    }

    removelog(){
        let lowestInt = this.count - 10;
        let i;
        for(i=lowestInt; i<this.count; i++){
            localStorage.removeItem('feedback_widget' +(i));
        }
        for(i=0; i<10; i++){
            localStorage.removeItem('feedback_widget' +(i));
        }
    }

    history() {
        
        let storage = new Array();
        let lowestInt = this.count - localStorage.length;
        let i;
        for (i = lowestInt; i < this.count; i++) {
            storage.push(JSON.parse(localStorage.getItem('feedback_widget' +(i))));
        }

        let stringbuild = "";
        for(i=0; i<storage.length;i++){
            stringbuild += storage[i].type + " - " + storage[i].message + "\n"
        }
        return stringbuild;
        //console.log(stringbuild);
    }

}




