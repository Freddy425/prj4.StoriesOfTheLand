describe('template spec', () => {
    let globalURL = 'https://localhost:7202'

    beforeEach(() => {

        cy.visit(globalURL + '/Specimen');
    })


    context('French tests', () => {

        //This test will create a specimen, and after redirection will check on the integrity in both english and french
        it('TestCreateSpecimenAlsoCreatesTheFrenchSpecimen ', () => {
            cy.get('button').contains('Create Specimen').click();
            cy.wait(200);
            cy.get('#specimenModal').within(() => {
                typeWithRetry('#LatinName', 'Latin Flower');
                typeWithRetry('#EnglishName', 'English Flower');
                typeWithRetry('#CreeName', 'Cree Flower');
                typeWithRetry('#SpecimenDescription', 'common flower');
                typeWithRetry('#CulturalSignificance', 'bilingual flower');
                cy.get('button[type="submit"]').contains('Create').click();
            });
 
            //after submitting the specimen check the url 
            cy.url().should('include', '/Specimen/Details/');

            //check english values
            cy.get('h3').should('contain', 'English Name');
            cy.get('h3').contains('English Name').parent().should('contain', 'English Flower');
            cy.get('h3').should('contain', 'Cree Name');
            cy.get('h3').contains('Cree Name').parent().should('contain', 'Cree Flower');
            cy.get('h3').should('contain', 'Latin Name');
            cy.get('h3').contains('Latin Name').parent().should('contain', 'Latin Flower');
            cy.get('h3').should('contain', 'Cultural Significance');
            cy.get('h3').contains('Cultural Significance').parent().should('contain', 'bilingual flower');
            cy.get('h3').should('contain', 'Description');
            cy.get('h3').contains('Description').parent().should('contain', 'common flower');

            //check french values
            cy.get('#toggleLanguage').click();


            cy.get('h3').should('contain', 'English Name');
            cy.get('h3').contains('French Name').parent().should('contain', 'Fleur anglaise');

            cy.get('h3').should('contain', 'Cree Name');
            cy.get('h3').contains('Cree Name').parent().should('contain', 'Cree Flower');

            cy.get('h3').should('contain', 'Latin Name');
            cy.get('h3').contains('Latin Name').parent().should('contain', 'Latin Flower');

            cy.get('h3').should('contain', 'Description');

            cy.get('body').should('contain', "fleur commune");


            cy.get('h3').should('contain', 'Cultural Significance');
            cy.get('body').should('contain', "fleur bilingue");
        });

        it('TestEditSpecimenAlsoEditsTheFrenchVersion', () => {
            // Find the "Plantain" specimen and click the associated "Edit" button.
            cy.contains('tr', 'English Flower').within(() => {
                cy.get('button').contains('Edit').click();
            });
            // Edit specimen

            cy.get('#editSpecimenModal').within(() => {
                // Check that the fields are populated with the 
                cy.get('input[name="EnglishName"]').invoke('val').should('eq', 'English Flower');
                cy.get('input[name="LatinName"]').invoke('val').should('eq', 'Latin Flower');
                cy.get('input[name="CreeName"]').invoke('val').should('eq', 'Cree Flower');
               
                cy.get('textarea[name="SpecimenDescription"]').invoke('eq', 'bilingual flower ');
                cy.get('textarea[name="CulturalSignificance"]').invoke('eq', 'common flower');

             
                cy.get('button').contains('Save').click();
            });
            cy.contains('English Flower').scrollIntoView();

            cy.wait(500);

            cy.contains('tr', 'English Flower').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                typeWithRetry('input[name="LatinName"]', 'Latin Plant');
                typeWithRetry('input[name="EnglishName"]', 'French Plant');
                typeWithRetry('input[name="CreeName"]', 'Cree Plant');
                typeWithRetry('textarea[name="SpecimenDescription"]', 'french plant');
                typeWithRetry('textarea[name="CulturalSignificance"]', 'not so common plant');
                cy.get('button[type="submit"]').contains('Save Changes').click();
            });

            // Open again and check it is still there
            cy.visit(globalURL + '/Specimen')

            cy.get('#toggleLanguage').click();

            cy.contains('Plante française').scrollIntoView();

            cy.contains('Plante française').click();
            //go to the details page

            //check french valuescy.get('button').contains('French').click();

            // Check for "English Name" to be updated to "Fleur française"

            cy.get('#toggleLanguage').click();

            cy.get('h3').should('contain', 'English Name');
            cy.get('body').should('contain', "Plante française");
            

            // Update "Cree Name" check to "Cree plant"
            cy.get('h3').should('contain', 'Cree Name');
            cy.get('body').should('contain', 'Cree Plant');

            // Update "Latin Name" check to "Latin plant"
            cy.get('h3').should('contain', 'Latin Name');
            cy.get('body').should('contain', 'Latin Plant');

            // Update the "Cultural Significance" text
            cy.get('h3').should('contain', 'Cultural Significance');
            cy.get('body').should('contain', "une plante peu commune");
           
            // Update "Description" to "Specimen Description" and its content
            cy.get('h3').should('contain', 'Description');
            cy.get('body').should('contain', "plante française");



        });

        //This test checks that clicking Delete on the confirmation popup performs changes and the specimen is not available anymore
        //French specimen is also deleted
        it('TestDeleteSpecimenAlsoDeletesFrenchVersion ', () => {

            // Find the <tr> that contains the text "French Flower". This looks for the row with the specimen we want to delete.
            cy.contains('tr', 'French Plant').within(() => {
                // Click on the "Delete" button within this row.
                cy.get('button').contains('Delete').click();
            });

            // Wait for the modal to be fully open
            cy.get('#deleteSpecimenModal').should('be.visible')
                .and('contain', 'Confirm Deletion of Specimen')
                .and('contain', 'Are you sure you want to delete "French Plant" specimen? This action cannot be undone');


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

            // Check that the popup is not visible anymore.
            cy.get('#deleteSpecimenModal').should('not.be.visible');

            // Check that "French Plant" is no longer in the page, indicating it was deleted.
            cy.contains('tr', 'French Plant').should('not.exist');

            cy.get('#toggleLanguage').click();


        });

    });
})
/**
 * This function is used to try multiple times to click a button in a modal.
 * @param {any} modalId =  ID of the modal with the button
 * @param {any} buttonText = Text displayed on the button
 * @param {any} times = number of times to be attempted
 */
function attemptClickButtonInModal(modalId, buttonText, times) {
    if (times > 0) {
        cy.get('body').then(body => {
            // Check if the button exists within the modal
            if (body.find(`#${modalId} button:contains("${buttonText}")`).length > 0) {
                cy.get(`#${modalId}`).within(() => {
                    cy.wait(500); // Wait for potential animations or transitions
                    cy.get('button').contains(buttonText).click({ force: true });
                }).then(() => {
                    // Recursive call to attempt the next click, if needed
                    attemptClickButtonInModal(modalId, buttonText, times - 1);
                });
            }
        });
    } else {
        // After all attempts, check if the modal has indeed been closed as expected
        cy.get(`#${modalId}`).should('not.be.visible');
    }
}

/**
 * This function will try to type a specific text multiple times
 * @param {any} selector = Where Im going to type
 * @param {any} value =  the text to enter
 * @param {any} retries =  numbers to retry
 */
function typeWithRetry(selector, value, retries = 3) {
    const typeAndCheck = (remainingRetries) => {
        if (remainingRetries === 0) {
            throw new Error(`Failed to type '${value}' into ${selector} after several retries.`);
        }

        cy.get(selector).clear().type(value).then($el => {
            if ($el.val() !== value) { // Check if the value is correctly set
                cy.wait(500); // Wait for half a second before retrying (adjust based on need)
                typeAndCheck(remainingRetries - 1); // Retry
            }
        });
    };

    typeAndCheck(retries);
}