import { Routes, RouterModule } from '@angular/router';

import { NavComponent }      from '../app/nav/nav.component';
import { ClaimSearchComponent }      from '../app/search/claim-search.component';
import { ClaimApproveComponent }      from '../app/approve/claim-approve.component';

const appRoutes: Routes = [
    { path: '', redirectTo: 'search', pathMatch: 'full' },
    {
        path: 'nav',
        component: NavComponent
    },
    {
        path: 'search',
        component: ClaimSearchComponent
    },
    {
        path: 'approve/:id',
        component: ClaimApproveComponent
    }
];
export const routing = RouterModule.forRoot(appRoutes);
