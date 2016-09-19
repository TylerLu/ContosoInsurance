"use strict";
var router_1 = require('@angular/router');
var nav_component_1 = require('../app/nav/nav.component');
var claim_search_component_1 = require('../app/search/claim-search.component');
var claim_approve_component_1 = require('../app/approve/claim-approve.component');
var appRoutes = [
    { path: '', redirectTo: 'search', pathMatch: 'full' },
    {
        path: 'nav',
        component: nav_component_1.NavComponent
    },
    {
        path: 'search',
        component: claim_search_component_1.ClaimSearchComponent
    },
    {
        path: 'approve/:id',
        component: claim_approve_component_1.ClaimApproveComponent
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map