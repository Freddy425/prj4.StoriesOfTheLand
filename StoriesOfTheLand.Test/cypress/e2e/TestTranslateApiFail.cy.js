/**
 * This test are meant to be run alone with the deployment httpApiFail
 * 
 */



describe('template spec', () => {
    let globalURL = 'https://localhost:7202'

    beforeEach(() => {

        cy.visit(globalURL + '/Specimen');
    })


    context('Specimen tests', () => {

        //This test will create a specimen,and because the deepl api fail it wont create a fr specimen but will display an error message
        it('TestCreateSpecimenDisplayErrorMessageAndShowsSameValues  ', () => {
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

            cy.get('body').contains("Failed to translate specimen to French. Proceeding with English only.");

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

            //check french values
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

             //check for the error message
            cy.get('body').contains("Failed to translate specimen to French. Proceeding with English only.");


            cy.contains('a', 'French Plant').click();
            //go to the details page

            //check french values            cy.get('#toggleLanguage').click();

            // Check for "English Name" to be updated to "Fleur française"

                        cy.get('#toggleLanguage').click();

            cy.get('h3').should('contain', 'English Name');
            cy.get('body').should('contain', "French Plant");
            

            // Update "Cree Name" check to "Cree plant"
            cy.get('h3').should('contain', 'Cree Name');
            cy.get('body').should('contain', 'Cree Plant');

            // Update "Latin Name" check to "Latin plant"
            cy.get('h3').should('contain', 'Latin Name');
            cy.get('body').should('contain', 'Latin Plant');

            // Update the "Cultural Significance" text
            cy.get('h3').should('contain', 'Cultural Significance');
            cy.get('body').should('contain', "not so common plant");
           
            // Update "Description" to "Specimen Description" and its content
            cy.get('h3').should('contain', 'Description');
            cy.get('body').should('contain', "french plant");



        });

        //This test checks that clicking Delete on the confirmation popup performs changes and the specimen is not available anymore
        //French specimen is also deleted
        it('TestDeleteSpecimenAlsoDeletesFrenchVersion', () => {

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

            // Check that "French Flower" is no longer in the page, indicating it was deleted.
            cy.contains('tr', 'PlaceholderName').should('not.exist');

                        cy.get('#toggleLanguage').click();

            // Check that french version is no longer in the page, indicating it was deleted.
            cy.contains('tr', 'Fleur française').should('not.exist');

        });

    });

    context('Resource tests', () => {

        //This test will create a Resource but wont create a FR_Resource because of the API fail. Will display message
        it('TestCreateResourceDisplayErrorMessageAndShowsSameValues', () => {

            cy.visit(globalURL + '/Resources');

            cy.get('button').contains('Create Resource').click();
            cy.get('#CreateResourceModal').should('be.visible');
            cy.get('#CreateResourceModal').within(() => {
                typeWithRetry('#ResourceTitle', 'Create Title');
                typeWithRetry('#ResourceURL', 'https://description.com');
                cy.get('button[type="submit"]').contains('Create').click();
            });

            //check message

            cy.get('body').contains('Failed to translate resource to French. Proceeding with English only.');

            // Ensure the resource has been added successfully
            cy.contains('tr', 'Description').should('be.visible');

            // Click the Learner View button to switch views
            cy.get('#toggleLearnerView').click();

            // Ensure the Learner View is visible
            cy.get('.resource-container').should('be.visible');

            // Now, find the container with the "Create Title" resource in the Learner View
            cy.get('.resource-container').contains('Create Title').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Create Title');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://description.com');
            });


                        cy.get('#toggleLanguage').click();

            // Now, find the container with the "Create Title" resource in the Learner View
            cy.get('.resource-container').contains('Create Title').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Create Title');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://description.com');
            });

        });

        //This test will Edit a resource, but wont create a French Resource. Will display error message
        it('TestEditResourceAlsoEditsTheFrenchVersion ', () => {

            cy.visit(globalURL + '/Resources');

            cy.contains('Create Title').scrollIntoView();

            // User navigates to the Edit form of the resource
            cy.contains('tr', 'Create Title').within(() => {
                cy.get('button').contains('Edit').click();
            });

            //User edits the resource details with information from EditResource2
            cy.get('#EditResourceModal').within(() => {
                cy.wait(500);
                cy.get('input[name="ResourceTitle"]').clear().type('Edit Title');
                cy.get('textarea[name="ResourceDescription"]').clear().type('Edit Description');
                cy.get('input[name="ResourceURL"]').clear().type('https://description.com');

                // User saves the changes
                cy.get('button').contains('Save Changes').click();
            });
            //check message

            cy.get('body').contains('Failed to translate resource to French. Proceeding with English only.');

            //check that in the table the values are correct.

            // Ensure the resource has been added successfully
            cy.contains('tr', 'Edit Title').should('be.visible');


            // User navigates to the Learner View to verify the changes
            cy.get('button').contains('Learner View').click();

            // User finds the edited resource in the container and verifies its presence
            cy.contains('.resource-container', 'Edit Title').should('be.visible');

            // Now, find the container with the "Edit Title" resource in the Learner View
            cy.get('.resource-container').contains('Edit Title').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Edit Title');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', 'Edit Description');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://description.com');
            });

            //now click french button

                        cy.get('#toggleLanguage').click();

            // User finds the edited resource in the container and verifies its presence
            cy.contains('.resource-container', 'Edit Title').should('be.visible');

            // Now, find the container with the "Edit Title" resource in the Learner View
            cy.get('.resource-container').contains('Edit Title').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Edit Title');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', 'Edit Description');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://description.com');
            });

        });

        //This test will dekete a resource created when the api failed
        it('TestClickingDeleteResourceInDeleteConfirmationClosesThePopUpAndBothLanguagesResourcesAreDeleted', () => {

            cy.visit(globalURL + '/Resources');

            // Find the "Edit Title" resource and click the associated "Delete" button.
            cy.contains('tr', 'Edit Title').within(() => {
                cy.get('button').contains('Delete').click();
            });

            // Confirmation pop-up appears
            cy.get('#DeleteResourceModal').should('be.visible');

            // User clicks on "Delete Resource"
            cy.get('#DeleteResourceModal').within(() => {
                cy.get('button').contains('Delete Resource').click();
            });

            // Wait for the deletion process to complete and the alert to be potentially visible
            cy.wait(500); // Adjust the wait time based on how long your deletion process takes.

            // Verify the success message appears within the alert placeholder
            cy.get('#alertPlaceholder').should('be.visible');
            cy.get('#alertMessage').contains('Resource was deleted successfully').should('be.visible');

            // Verify "Edit Title" resource is no longer in the list
            cy.contains('tr', 'Edit Title').should('not.exist');

            //verify the other resource is still available

            cy.contains('tr', 'NCTR').should('exist');

            //clicks on French button
            cy.get('button').contains('Learner View').click();
                        cy.get('#toggleLanguage').click();

            cy.contains('tr', 'Polytechnique de la Saskatchewan').should('not.exist');
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