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
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
require("rxjs/add/operator/toPromise");
var ZenithService = (function () {
    function ZenithService(http) {
        this.http = http;
        this.BASE_URL = "http://localhost:5000";
        this.headers = new http_1.Headers({ 'Content-Type': 'application/json' });
    }
    ZenithService.prototype.getEvents = function () {
        return this.http.get(this.BASE_URL + '/api/eventsAPI')
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    ZenithService.prototype.getAPIToken = function (username, password) {
        var body = 'grant_type=password&username=' + username + '&password=' + password;
        var headers3 = new http_1.Headers();
        headers3.append('Content-Type', 'application/x-www-form-urlencoded');
        return this.http
            .post(this.BASE_URL + '/connect/token', body, { headers: headers3 })
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    ZenithService.prototype.register = function (newUser) {
        var body = "Username=" + newUser.Username + "&FirstName=" + newUser.FirstName + "&LastName=" + newUser.LastName + "&Email=" + newUser.Email + "&Password=" + newUser.Password + "&ConfirmPassword=" + newUser.ConfirmPassword;
        var headers3 = new http_1.Headers();
        headers3.append('Content-Type', 'application/x-www-form-urlencoded');
        return this.http
            .post(this.BASE_URL + '/connect/register', body, { headers: headers3 })
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    ZenithService.prototype.getNewWeek = function (token, num) {
        var body = '';
        var headers2 = new http_1.Headers({ 'Accept': 'application/json' });
        headers2.append('Authorization', 'Bearer ' + token);
        var options = new http_1.RequestOptions({ headers: headers2 });
        return this.http.get(this.BASE_URL + '/api/eventsAPI/' + num.toString(), options)
            .toPromise()
            .then(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    ZenithService.prototype.handleError = function (error) {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    return ZenithService;
}());
ZenithService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], ZenithService);
exports.ZenithService = ZenithService;
