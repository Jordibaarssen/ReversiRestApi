"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var apiUrl = 'url/super/duper/game';

var Game = function (url) {
  console.log('hallo, vanuit een module'); //Configuratie en state waarden

  var configMap = {
    apiUrl: url
  }; // Private function init

  var privateInit = function privateInit() {
    //console.log(configMap.apiUrl);
    //Game.Data.generateBord();
    //afterInit();
    setInterval(Game.Data.generateBord, 2000);
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    Game.Data.stateMap.gamestate = Game.Model.getGameState(); //Game.Data.generateBord();
  }; // Waarde object geretourneerd aan de outer scope


  return {
    init: privateInit
  };
}(apiUrl); //const $ = require("browser-sync");


Game.Data = function () {
  console.log('hallo, vanuit module Data');
  var configMap = {
    //apiKey: '7bae2caf9621a6b9d4bdf44399982675',
    mock: [{
      url: 'api/Spel/Beurt',
      data: 0
    }]
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock.data;
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  var stateMap = {
    environment: 'production',
    gamestate: '',
    wit: 0,
    zwart: 0
  };

  var get = function get(url) {
    if (stateMap.environment === "production") {
      return $.get(url).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
      });
    } else if (stateMap.environment === "development") {
      return getMockData(url);
    }
  };

  var generateBord = function generateBord() {
    var bord = get('/Api/Reversi/Bord').then(function (data) {
      if (stateMap.gamestate != data) {
        stateMap.gamestate = data;
        stateMap.wit = 0;
        stateMap.zwart = 0;
        var coordinates = JSON.parse(data);
        var bord = document.getElementById("ReversiBord"); // table rows

        for (var i = 0; i < bord.rows.length; i++) {
          // row cells
          for (var j = 0; j < bord.rows[i].cells.length; j++) {
            if (coordinates[i][j] == 1) {
              Game.Reversi.showFiche(i, j, "wit");
              stateMap.wit++;
            } else if (coordinates[i][j] == 2) {
              Game.Reversi.showFiche(i, j, "zwart");
              stateMap.zwart++;
            }
          }
        }

        afGelopen();
      }
    });
  };

  var doeZet = function doeZet(rij, kolom) {
    get('/Api/Reversi/DoeZet/' + rij + '&' + kolom).then(function (data) {
      if (data == true) {
        generateBord();
      }
    });
  };

  var afGelopen = function afGelopen() {
    var x = document.getElementById("ReversiBord");
    var y = document.getElementById("winScherm");
    get('/Api/Reversi/isAfgelopen').then(function (data) {
      if (data) {
        x.style.display = "none";
        y.style.display = "block";
        console.log('Klaar!');

        if (stateMap.wit > stateMap.zwart) {
          document.getElementById("winnaar").innerHTML = "Wit is de winnaar!";
          document.getElementById("scoreWinner").innerHTML = stateMap.wit;
          document.getElementById("scoreLoser").innerHTML = stateMap.zwart;
        } else {
          document.getElementById("winnaar").innerHTML = "Zwart is de winnaar!";
          document.getElementById("scoreWinner").innerHTML = stateMap.zwart;
          document.getElementById("scoreLoser").innerHTML = stateMap.wit;
        }
      } else {
        x.style.display = "block";
        y.style.display = "none";
      }
    });
  };

  var privateInit = function privateInit() {// if(stateMap.environment === "development")
    // {
    //     return getMockData();
    // }else if(stateMap.environment === "production")
    // {
    //     //doe request aan server
    //     //xhttp.open("GET", "ajax_info.txt", true);
    //     //xhttp.send();
    //
    //
    // }else
    // {
    //     throw new Error("de environment is geen development of production")
    // }
  };

  return {
    init: privateInit,
    get: get,
    stateMap: stateMap,
    generateBord: generateBord,
    doeZet: doeZet
  };
}();

Game.Model = function () {
  console.log('hallo, vanuit module Model');
  var configMap;

  var privateInit = function privateInit(callback) {
    console.log("private data");
  };

  var getWeather = function getWeather(url) {
    Game.Data.get(url).then(function (data) {
      if (data.main.temp == null) {
        throw new Error("Geen temperatuur");
      }

      console.log(data);
    });
  };

  var _getGameState = function _getGameState() {
    //aanvraag via Game.Data
    var data = Game.Data.get('/api/Spel/Beurt/<token>'); //controle of ontvangen data valide is

    if (data === 0) {//geen waarde
    } else if (data === 1) {//wit aan zet
    } else if (data === 2) {//zwart aan zet
    } else {
      throw new Error("ongeldige data");
    }
  };

  return {
    init: privateInit,
    weather: getWeather,
    getGameState: _getGameState
  };
}();

Game.Reversi = function () {
  console.log('hallo, vanuit module Reversi');
  var configMap;

  var privateInit = function privateInit() {
    console.log("private spel");
  };

  var showFiche = function showFiche(row, colomn, kleur) {
    var x = document.getElementById("ReversiBord").rows[row].cells[colomn];

    if (kleur == "wit") {
      x.innerHTML = '<div class="ficheWit fade-in"></div>';
    } else if (kleur == "zwart") {
      x.innerHTML = '<div class="ficheZwart fade-in"></div>';
    } else {
      console.log("dit is geen kleur");
    }
  };

  return {
    init: privateInit,
    showFiche: showFiche
  };
}();

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
  }

  _createClass(FeedbackWidget, [{
    key: "show",
    value: function show(message, type) {
      var x = document.getElementById(this._elementId);
      x.style.display = "block";
      $(x).text(message);

      if (type == "danger") {
        $(x).addClass('alert alert-danger');
        $(x).removeClass('alert alert-success');
      } else if (type == "success") {
        $(x).addClass('alert alert-success');
        $(x).removeClass('alert alert-danger');
      }

      var msg = {
        message: message,
        type: type
      };
      this.log(msg);
      console.log(this.history());
    }
  }, {
    key: "hide",
    value: function hide() {
      var x = document.getElementById(this._elementId);
      x.style.display = "none";
    } //    count = 1;

  }, {
    key: "log",
    value: function log(message) {
      {
        var lowestInt = this.count - 10;

        if (localStorage.length >= 10) {
          localStorage.removeItem('feedback_widget' + lowestInt);
          console.log("boven de 10");
        }

        localStorage.setItem('feedback_widget' + this.count, JSON.stringify(message));
        this.count++;
      }
    }
  }, {
    key: "removelog",
    value: function removelog() {
      var lowestInt = this.count - 10;
      var i;

      for (i = lowestInt; i < this.count; i++) {
        localStorage.removeItem('feedback_widget' + i);
      }

      for (i = 0; i < 10; i++) {
        localStorage.removeItem('feedback_widget' + i);
      }
    }
  }, {
    key: "history",
    value: function history() {
      var storage = new Array();
      var lowestInt = this.count - localStorage.length;
      var i;

      for (i = lowestInt; i < this.count; i++) {
        storage.push(JSON.parse(localStorage.getItem('feedback_widget' + i)));
      }

      var stringbuild = "";

      for (i = 0; i < storage.length; i++) {
        stringbuild += storage[i].type + " - " + storage[i].message + "\n";
      }

      return stringbuild; //console.log(stringbuild);
    }
  }, {
    key: "elementId",
    get: function get() {
      //getter, set keyword voor setter methode
      return this._elementId();
    }
  }]);

  return FeedbackWidget;
}();