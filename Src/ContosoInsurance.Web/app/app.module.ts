import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule }     from '@angular/http';

import { XHRBackend } from '@angular/http';
import { InMemoryBackendService, SEED_DATA } from 'angular2-in-memory-web-api';
import { InMemoryDataService } from './search/in-memory-data.service';

import { AppComponent }  from './app.component';
import { ClaimSearchComponent }  from './search/claim-search.component';
import { ClaimGridComponent }  from './search/claim-grid.component';
import { NavComponent }  from './nav/nav.component';
import { ClaimStatusComponent }  from './approve/claim-status.component';

import {Tab} from './tabs/tab';
import {Tabs} from './tabs/tabs';

import {routing} from './app.routing';

import {ClaimSearchService} from './search/claim-search.service';
import {ClaimDetailService} from './approve/claim-detail.service';



@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        routing,
        HttpModule,
    ],
    declarations: [
        AppComponent,
        ClaimSearchComponent,
        ClaimGridComponent,
        NavComponent,
        Tab,
        Tabs,
        ClaimStatusComponent,
    ],
    providers: [
        ClaimSearchService,
        ClaimDetailService,
        { provide: XHRBackend, useClass: InMemoryBackendService }, // in-mem server
        { provide: SEED_DATA, useClass: InMemoryDataService },     // in-mem server data
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
