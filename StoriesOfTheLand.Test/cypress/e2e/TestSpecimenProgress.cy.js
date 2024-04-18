describe('template spec', () => {
    let globalURL = 'https://localhost:7202'
    beforeEach(() => {
       

        
        cy.visit(globalURL + '/Specimen/')
    })

    context('TestSpecimenProgress', () => {
        it('Progress Cookie on Specimen List', () => {

            //drop down works
            cy.get('#dropdownMenuButton').click();
            cy.get('#discoveryProgress').should('be.visible');
            cy.get('.dropdown-item').should('have.length', 13);

            cy.get('#dropdownMenuButton').click();
            cy.get('#discoveryProgress').should('not.be.visible');

           
            //ProgressSession yet exist
            Cypress.session.clearAllSavedSessions()
            Cypress.session.clearCurrentSessionData()

            cy.visit(globalURL + '/Specimen/')

            cy.getCookie('MySessionCookie').should('exist')

            //ProgressIsEmptyList

            cy.visit(globalURL + '/Specimen/')

            cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (0/13)')

            //ProgressHas1
            cy.visit(globalURL + '/Specimen/Details/1')
            cy.wait(100)
            cy.visit(globalURL + '/Specimen/')
            cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (1/13)')
            cy.get('.dropdown-item').contains('Velvet Leaf Blueberry');
            cy.get('.dropdown-item[data-specimen-id="1"]');
            cy.get('[data-specimen-id="1"]').should('have.class', 'discovered');


            //ProgressCookieHasAllSpecimen 
            for (let i = 1; i <= 13; i++) {
                cy.visit(`${globalURL}/Specimen/Details/${i}`);
            }

            cy.visit(globalURL + '/Specimen/')

            cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (13/13)')


        });

    it('Progress Cookie on Specimen Details', () => {

        //drop down works
        cy.get('#dropdownMenuButton').click();
        cy.get('#discoveryProgress').should('be.visible');
        cy.get('.dropdown-item').should('have.length', 13);

        cy.get('#dropdownMenuButton').click();
        cy.get('#discoveryProgress').should('not.be.visible');

        //ProgressSessionNotYetMadeSpecimen
        Cypress.session.clearAllSavedSessions()

        cy.visit(globalURL + '/Specimen/Details/1')
        cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (1/13)')
        //cy.getCookie('MySessionCookie').should('exist')
        cy.get('[data-specimen-id="1"]').should('have.class', 'discovered');
        

        //ProgressSessionIsEmptySpecimen 
        Cypress.session.clearAllSavedSessions()
        Cypress.session.clearCurrentSessionData()

        cy.visit(globalURL + '/Specimen/')

        cy.visit(globalURL + '/Specimen/Details/1')

        cy.getCookie('MySessionCookie').should('exist')

        cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (1/13)')
        cy.get('[data-specimen-id="1"]').should('have.class', 'discovered');

        // ProgressCookieHas1Specimen 
        cy.visit(globalURL + '/Specimen/Details/2')

        cy.getCookie('MySessionCookie').should('exist')

        cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (2/13)')
        cy.get('[data-specimen-id="1"]').should('have.class', 'discovered');
        cy.get('[data-specimen-id="2"]').should('have.class', 'discovered');

        //ProgressCookieHasSameSpecimen 
        cy.visit(globalURL + '/Specimen/Details/2')

        cy.getCookie('MySessionCookie').should('exist')

        cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (2/13)')
        cy.get('[data-specimen-id="1"]').should('have.class', 'discovered');
        cy.get('[data-specimen-id="2"]').should('have.class', 'discovered');

        //ProgressCookieHasAllSpecimen 
        for (let i = 1; i <= 13; i++) {
            cy.visit(`${globalURL}/Specimen/Details/${i}`);
        }
        cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (13/13)')


        Cypress.session.clearAllSavedSessions()
        Cypress.session.clearCurrentSessionData()

        for (let i = 2; i <= 13; i += 2) {
            cy.visit(`${globalURL}/Specimen/Details/${i}`);
        }

        // Visit all odd-numbered specimens
        for (let i = 1; i <= 13; i += 2) {
            cy.visit(`${globalURL}/Specimen/Details/${i}`);
        }
        cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (13/13)')
         
    });

    })

    
    
})

//This function will create a specimen that will be deleted in the delete testing functionality
function createSpecimenPlaceholderName() {
    //fill with a valid specimen
    cy.wait(10);
    const uniqueLatinName = 'Tired';
    cy.wait(10);
    const uniqueEnglishName = 'PlaceholderName';

    // Click on the "Create Specimen" button
    cy.get('button').contains('Create Specimen').click();

    // Wait for the modal to be fully open
    cy.get('#specimenModal').should('be.visible');

    cy.get('#LatinName').type(uniqueLatinName);
    cy.get('#LatinName').invoke('val', '');
    // Cypress bug: first time we type works randomly, fix is to type, clean and type again
    cy.get('#LatinName').clear().type(uniqueLatinName);
    cy.get('#EnglishName').type(uniqueEnglishName);
    cy.get('#SpecimenDescription').type('This specimen wanted to disappear; he is really tired. Lucky for him in order to pass this test he needs to cease existence.');
    cy.get('#CulturalSignificance').type('This specimen needs to be deleted.');
    cy.get('button[type="submit"]').contains(' Create ').click();

    cy.wait(1000); // Adjust wait time based on your application's behavior
    cy.visit('https://localhost:7202' + '/Specimen')
}