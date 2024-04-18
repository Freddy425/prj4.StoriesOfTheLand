describe('template spec', () => {
    let globalURL = 'https://localhost:7202'
    beforeEach(() => {
        // We made cypress visit the specimen index list
        cy.visit(globalURL + '/Specimen')
    })

    context('TestProgressUpdateFunctionality', () => {

        //This test will check if opening a edit page it is poblated with the previous values of the specimen
        it('TestThatDiscoveryProgressUpdatesOnAddAndDelete', () => {


            createSpecimenPlaceholderName()
            cy.visit(globalURL + '/Specimen')


            cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (1/14)')


            cy.getCookie('MySessionCookie').should('exist')

            cy.visit(globalURL + '/Specimen/')

            // Find the <tr> that contains the text "PlaceholderName". This looks for the row with the specimen we want to delete.
            cy.contains('tr', 'PlaceholderName').within(() => {
                // Click on the "Delete" button within this row.
                cy.get('button').contains('Delete').click();
            });

          

            cy.wait(100);

            // Find the Cancel button in the popup and click it.
            cy.get('#deleteSpecimenModal').within(() => {
                cy.get('button').contains('Delete').click();
            });

            cy.wait(100);
            // On alert, check the content is correct
            cy.on('window:alert', (str) => {
                expect(str).to.equal('You deleted the specimen successfully.')
            });
            cy.wait(1000);

            cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (0/13)')


            createSpecimenPlaceholderName()


            



        });

        it('TestDeleteSpecimenDoesntMessWithProgress', () => {

            cy.visit(globalURL + '/Specimen/Details/1')

            cy.visit(globalURL + '/Specimen/Details/2')

            cy.visit(globalURL + '/Specimen/')

            cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (2/14)')

           


            // Find the <tr> that contains the text "PlaceholderName". This looks for the row with the specimen we want to delete.
            cy.contains('tr', 'PlaceholderName').within(() => {
                // Click on the "Delete" button within this row.
                cy.get('button').contains('Delete').click();
            });



            cy.wait(100);

            // Find the Cancel button in the popup and click it.
            cy.get('#deleteSpecimenModal').within(() => {
                cy.get('button').contains('Delete').click();
            });

            cy.wait(100);
            // On alert, check the content is correct
            cy.on('window:alert', (str) => {
                expect(str).to.equal('You deleted the specimen successfully.')
            });
            cy.wait(1000);


            cy.get('#dropdownMenuButton').should('have.text', 'Discovery Progress (2/13)')
        });

    })
})

//This function will create a specimen that will be deleted in the delete testing functionality
function createSpecimenPlaceholderName() {
    //fill with a valid specimen
    const uniqueLatinName = 'Tired';
    const uniqueEnglishName = 'PlaceholderName';

    // Click on the "Create Specimen" button
    cy.get('button').contains('Create Specimen').click();

    // Wait for the modal to be fully open
    cy.get('#specimenModal').should('be.visible');

    cy.get('#LatinName').clear().type(uniqueLatinName);
    cy.get('#LatinName').invoke('val', '');
    // Cypress bug: first time we type works randomly, fix is to type, clean and type again
    cy.get('#LatinName').type(uniqueLatinName);
    cy.get('#EnglishName').type(uniqueEnglishName);
    cy.get('#SpecimenDescription').type('This specimen wanted to disappear; he is really tired. Lucky for him in order to pass this test he needs to cease existence.');
    cy.get('#CulturalSignificance').type('This specimen needs to be deleted.');
    cy.get('button[type="submit"]').contains(' Create ').click();

    cy.wait(1000); // Adjust wait time based on your application's behavior
    cy.visit('https://localhost:7202' + '/Specimen')
}

//These functions will generate random letters, used in spicemens names
function randomLetter() {
    const positionInAlphabet = Math.round(Math.random() * 25); // 0 to 25 (a to z)
    return String.fromCharCode(97 + positionInAlphabet); // position + offset
}
function randomLetters() {
    return randomLetter() + randomLetter() + randomLetter() + randomLetter() + randomLetter() + randomLetter() + randomLetter();
}