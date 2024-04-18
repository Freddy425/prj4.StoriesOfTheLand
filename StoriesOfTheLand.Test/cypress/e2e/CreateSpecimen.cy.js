describe('template spec', () => {
    let globalURL = 'https://localhost:7202'

    beforeEach(() => {
        // We made cypress visit the specimen index list
        cy.visit(globalURL + '/Specimen')
    })

    /**
     * This tests will in the future replace the create specimen test in the Specimen.Test.cs to migrate from Selenium to Cypress
     * This change will only be made after these tests run successfully
     */
    context('Create Specimen Tests', () => {

        beforeEach(() => {
            cy.get('button').contains('Create Specimen').click();
        });

        //This test will check that after creating a specimen the user will be redirected to the Specimen list page
        it('TestAddValidSpecimenRedirectsToDetailsPage', () => {
            //fill with a valid specimen
            const uniqueLatinName = 'Unique Latin Name' + randomLetters();
            const uniqueEnglishName = 'Unique English Name' + randomLetters();
            const uniqueCreeName = 'Unique Cree Name' + randomLetters();

            cy.wait(2000);
            cy.get('#LatinName').invoke('val', '');
            // Cypress bug: first time we type works randomly, fix is to type, clean and type again
            cy.wait(2000);

            cy.get('#LatinName').type(uniqueLatinName);
            cy.wait(2000);

            cy.get('#EnglishName').type(uniqueEnglishName);
            cy.wait(2000);

            cy.get('#CreeName').type(uniqueCreeName);
            cy.wait(2000);

            cy.get('#SpecimenDescription').type('Valid Description');
            cy.get('#CulturalSignificance').type('Valid Significance');
            cy.get('button[type="submit"]').contains(' Create ').click();

            //after submitting the specimen check the url
            cy.url().should('include', '/Specimen/Details/');
            cy.get('h3').should('contain', 'English Name');
            cy.get('h3').contains('English Name').parent().should('contain', uniqueEnglishName);
            cy.get('h3').should('contain', 'Cree Name');
            cy.get('h3').contains('Cree Name').parent().should('contain', uniqueCreeName);
            cy.get('h3').should('contain', 'Latin Name');
            cy.get('h3').contains('Latin Name').parent().should('contain', uniqueLatinName);
            cy.get('h3').should('contain', 'Cultural Significance');
            cy.get('h3').contains('Cultural Significance').parent().should('contain', 'Valid Significance');
            cy.get('h3').should('contain', 'Description');
            cy.get('h3').contains('Description').parent().should('contain', 'Valid Description');
        });
        
        it('TestIntegritySpecimenAddedToTable', () => {
            //fill with a valid specimen
            const uniqueLatinName = 'Unique Latin Name' + randomLetters();
            const uniqueEnglishName = 'Unique English Name' + randomLetters();
            const uniqueCreeName = 'Unique Cree Name' + randomLetters();

            cy.get('#LatinName').type(uniqueLatinName);
            cy.get('#LatinName').invoke('val', '');

            cy.get('#LatinName').type(uniqueLatinName);
            cy.get('#EnglishName').type(uniqueEnglishName);
            cy.get('#CreeName').type(uniqueCreeName);

            cy.get('#SpecimenDescription').type('Unique Description');
            cy.get('#CulturalSignificance').type('Unique Significance');
            cy.get('button[type="submit"]').contains(' Create ').click();


            // Check if the specimen is on the list by looking for a row that contains the unique English Name
            cy.visit(globalURL + '/Specimen');
            cy.get('table.table').should('contain', uniqueEnglishName);
            cy.get('table.table').contains(uniqueEnglishName).parent().parent().should('contain', uniqueCreeName);
            cy.get('table.table').contains(uniqueEnglishName).parent().parent().should('contain', uniqueLatinName);
        });

        //This test will make sure the user is provided validation messages if there is an invalid specimen
        it('TestAddEmptySpicemen', () => {
            cy.get('#specimenModal').within(() => {
                cy.get('input[name="LatinName"]').invoke('val', '')
                cy.get('input[name="EnglishName"]').invoke('val', '')
                cy.get('input[name="CreeName"]').invoke('val', '')
                cy.get('textarea[name="SpecimenDescription"]').invoke('val', '')
                cy.get('textarea[name="CulturalSignificance"]').invoke('val', '')
                cy.get('button[type="submit"]').contains(' Create ').click();
            });

            //after submitting the specimen check the url
            cy.url().should('not.include', '/Specimen/Details');
            cy.url().should('include', '/Specimen');

            //check the messages
            cy.get('#specimenModal').within(() => {
                cy.get('input[name="EnglishName"]').parent().should('contain', 'English Name is required');
                cy.get('input[name="LatinName"]').parent().should('contain', 'Latin Name is required');
                cy.get('input[name="CreeName"]').parent().should('contain', '');
                cy.get('textarea[name="SpecimenDescription"]').parent().should('contain', 'SpecimenDescription cannot be blank'); 
                cy.get('textarea[name="CulturalSignificance"]').parent().should('contain', 'Cultural Significance is required'); 
            });
        });
    });
})

//These functions will generate random letters, used in spicemens names
function randomLetter() {
    const positionInAlphabet = Math.round(Math.random() * 25); // 0 to 25 (a to z)
    return String.fromCharCode(97 + positionInAlphabet); // position + offset
}
function randomLetters() {
    return randomLetter() + randomLetter() + randomLetter() + randomLetter() + randomLetter() + randomLetter() + randomLetter();
}