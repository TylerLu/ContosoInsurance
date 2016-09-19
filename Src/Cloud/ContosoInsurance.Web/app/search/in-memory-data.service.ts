export class InMemoryDataService {
    createDb() {
        let searchClaims = [
            { claimId: 21, firstName: 'Albert', lastName: 'Xie', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('08-09-2016'), claimStatus: 'Approved', damangeAssessment:'Severe'},
            { claimId: 22, firstName: 'John', lastName: 'Green', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('08-11-2016'), claimStatus: 'pending', damangeAssessment: 'Moderate' },
            { claimId: 23, firstName: 'Hanna', lastName: 'Bat', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('08-12-2016'), claimStatus: 'closed', damangeAssessment: 'Minimal' },
            { claimId: 24, firstName: 'Lucy', lastName: 'Bauman', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('08-15-2016'), claimStatus: 'closed', damangeAssessment: 'Minimal' },
            { claimId: 25, firstName: 'Kate', lastName: 'Da', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('07-12-2016'), claimStatus: 'pending', damangeAssessment: 'Severe' },
            { claimId: 26, firstName: 'Jim', lastName: 'King', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('09-09-2016'), claimStatus: 'closed', damangeAssessment: 'Moderate' },
            { claimId: 27, firstName: 'Todd', lastName: 'Balag', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('09-01-2016'), claimStatus: 'Approved', damangeAssessment: 'Minimal' },
            { claimId: 28, firstName: 'Damian', lastName: 'LLLL', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('08-07-2016'), claimStatus: 'Approved', damangeAssessment: 'Moderate' },
            { claimId: 29, firstName: 'Tyler', lastName: 'Lu', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('08-07-2016'), claimStatus: 'closed', damangeAssessment: 'Minimal'},
            { claimId: 30, firstName: 'Arthur', lastName: 'Zheng', policyHolderId: 100, claimType: 'Automobile', dueDate: new Date('08-09-2016'), claimStatus: 'pending', damangeAssessment: 'Moderate' },
        ];
        return { searchClaims };
    }
}
