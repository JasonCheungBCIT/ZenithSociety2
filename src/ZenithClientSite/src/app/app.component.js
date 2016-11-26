"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var zenith_service_1 = require("./zenith.service");
var Users_1 = require("./Users");
var new_user_1 = require("./new-user");
require("rxjs/add/operator/map");
require("rxjs/add/operator/toPromise");
var AppComponent = (function () {
    function AppComponent(ZenithService) {
        this.ZenithService = ZenithService;
        this.title = 'app works!';
        this.user = new Users_1.Users();
        this.newUser = new new_user_1.NewUser();
        this.count = 0;
        this.showLogin = false;
        this.showRegister = false;
        this.fail = false;
        this.error = "";
        this.errorBool = false;
        this.what = [];
        this.eventsKeys = [];
        this.eventsDictionary = {};
    }
    AppComponent.prototype.getEvents = function () {
        var _this = this;
        this.ZenithService.getEvents().then(function (events) { return _this.reformatData(events); });
    };
    AppComponent.prototype.getNextWeek = function (num) {
        var _this = this;
        console.log("Get next week");
        this.eventsKeys = [];
        this.eventsDictionary = {};
        this.count += num;
        this.ZenithService.getNewWeek(this.token.access_token, this.count)
            .then(function (events) { return _this.reformatData(events); });
    };
    AppComponent.prototype.reformatData = function (data) {
        var _loop_1 = function (e) {
            var fromDate = new Date(e.fromDate);
            var toDate = new Date(e.toDate);
            var dayKey = fromDate.toDateString();
            if (!this_1.eventsKeys.find(function (s) { return s == dayKey; }))
                this_1.eventsKeys.push(dayKey);
            (this_1.eventsDictionary[dayKey] = this_1.eventsDictionary[dayKey] ? this_1.eventsDictionary[dayKey] : []).push(e);
        };
        var this_1 = this;
        for (var _i = 0, data_1 = data; _i < data_1.length; _i++) {
            var e = data_1[_i];
            _loop_1(e);
        }
        console.log(this.eventsDictionary);
    };
    AppComponent.prototype.ngOnInit = function () {
        this.getEvents();
    };
    AppComponent.prototype.verify = function (username, password) {
        var _this = this;
        this.ZenithService
            .getAPIToken(username, password)
            .then(function (token) { return _this.onVerifyResult(token); })
            .catch(function (error) { return _this.handleRegisterError(error); });
    };
    AppComponent.prototype.register = function () {
        var _this = this;
        this.ZenithService
            .register(this.newUser)
            .then(function (response) { return _this.onRegisterResult(response); })
            .catch(function (error) { return _this.handleRegisterError(error); });
    };
    AppComponent.prototype.onRegisterResult = function (newUser) {
        console.log(this.newUser.FirstName);
        this.verify(this.newUser.Username, this.newUser.Password);
    };
    AppComponent.prototype.handleRegisterError = function (error) {
        console.log(error);
        this.errorBool = true;
        this.error = "Attempt Failed, Please Try Again!";
    };
    AppComponent.prototype.handleVerifyResult = function (error) {
        console.log("Handle verify error");
        return Promise.reject(error.messge || error);
    };
    AppComponent.prototype.onVerifyResult = function (token) {
        console.log(this.fail);
        if (this.token) {
            this.fail = true;
            console.log(this.fail);
        }
        this.token = token;
    };
    return AppComponent;
}());
AppComponent = __decorate([
    core_1.Component({
        selector: 'app-root',
        templateUrl: './app.component.html',
        styleUrls: ['./app.component.css'],
        providers: [zenith_service_1.ZenithService]
    }),
    __metadata("design:paramtypes", [zenith_service_1.ZenithService])
], AppComponent);
exports.AppComponent = AppComponent;
