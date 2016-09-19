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
var core_1 = require('@angular/core');
var platform_browser_1 = require('@angular/platform-browser');
var forms_1 = require('@angular/forms');
var http_1 = require('@angular/http');
var http_2 = require('@angular/http');
var angular2_in_memory_web_api_1 = require('angular2-in-memory-web-api');
var in_memory_data_service_1 = require('./search/in-memory-data.service');
var app_component_1 = require('./app.component');
var claim_search_component_1 = require('./search/claim-search.component');
var claim_grid_component_1 = require('./search/claim-grid.component');
var nav_component_1 = require('./nav/nav.component');
var claim_status_component_1 = require('./approve/claim-status.component');
var tab_1 = require('./tabs/tab');
var tabs_1 = require('./tabs/tabs');
var app_routing_1 = require('./app.routing');
var claim_search_service_1 = require('./search/claim-search.service');
var claim_detail_service_1 = require('./approve/claim-detail.service');
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [
                platform_browser_1.BrowserModule,
                forms_1.FormsModule,
                app_routing_1.routing,
                http_1.HttpModule,
            ],
            declarations: [
                app_component_1.AppComponent,
                claim_search_component_1.ClaimSearchComponent,
                claim_grid_component_1.ClaimGridComponent,
                nav_component_1.NavComponent,
                tab_1.Tab,
                tabs_1.Tabs,
                claim_status_component_1.ClaimStatusComponent,
            ],
            providers: [
                claim_search_service_1.ClaimSearchService,
                claim_detail_service_1.ClaimDetailService,
                { provide: http_2.XHRBackend, useClass: angular2_in_memory_web_api_1.InMemoryBackendService },
                { provide: angular2_in_memory_web_api_1.SEED_DATA, useClass: in_memory_data_service_1.InMemoryDataService },
            ],
            bootstrap: [app_component_1.AppComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map