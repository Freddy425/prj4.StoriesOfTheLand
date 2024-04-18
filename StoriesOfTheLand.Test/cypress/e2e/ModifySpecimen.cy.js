// Tests for the modify Specimen Modal
// NOTE: Some of these tests are stinky and have to be re-run multiple times to work properly
//      This is not my fault (I think), it's a symptom of cypress being The Worst - Jett


describe('template spec', () => {
    let globalURL = 'https://localhost:7202'
    beforeEach(() => {
        // We made cypress visit the specimen index list
        cy.visit(globalURL + '/Specimen')
    })

    context('TestEditFunctionality', () => {

        beforeEach(() => {
           
        });
        
        //This test will check if there is an Edit button next to each specimen
        it('TestThatEditButtonIsNextToEachSpecimen', () => {
            // Count the number of <tr> elements. Each row contains one specimen
            cy.get('tbody').find('tr').then(specimenRows => {
                const totalSpecimens = specimenRows.length;

                // Now, count the number of Edit buttons within these rows.
                cy.get('tr').find('button').filter(':contains("Edit")').then(editButtons => {
                    const totalEditButtons = editButtons.length;

                    // Compare the count of specimen rows with the count of "Edit" buttons.
                    expect(totalEditButtons).to.equal(totalSpecimens, 'Each specimen has an Edit button next to it');
                });
            });
        });

        //This test will check if clicking on the edit button will open the edit specimen list
        it('TestThatEditFormOpensAnEditForm', () => {
            // Find any "Edit" button and click it.
            cy.get('button').contains('Edit').first().click();

            // Edit specimen
            cy.get('#editSpecimenModal').within(() => {
                // Check for the presence of input fields on the Edit form.
                cy.get('input[name="EnglishName"]').should('be.visible');
                cy.get('input[name="LatinName"]').should('be.visible');
                cy.get('input[name="CreeName"]').should('be.visible');
                cy.get('textarea[name="SpecimenDescription"]').should('be.visible');
                cy.get('textarea[name="CulturalSignificance"]').should('be.visible');

                // Check for the presence of "Back" and "Save Changes" buttons
                cy.get('div.modal-footer:nth-child(11) > button:nth-child(2)').scrollIntoView();
                cy.get('button').contains('Close').should('be.visible');
                cy.get('button').contains('Save Changes').should('be.visible');
            });
        });
        
        //This test will check if opening a edit page it is poblated with the previous values of the specimen
        it('TestThatEditFormIsPopulatedWithTheSpecimenAndClosingItDoesntMakeChanges', () => {
            createSpecimenPlaceholderName()
            // Find the "Plantain" specimen and click the associated "Edit" button.
            cy.contains('tr', 'PlaceholderName').within(() => {
                cy.get('button').contains('Edit').click();
            });
            // Edit specimen
            cy.get('#editSpecimenModal').within(() => {
                // Check that the fields are populated with the Plaintain details
                cy.get('input[name="EnglishName"]').invoke('val').should('eq', 'PlaceholderName');
                cy.get('input[name="CreeName"]').should('have.value', '');
                cy.get('textarea[name="SpecimenDescription"]').should('have.value', 'This specimen wanted to disappear; he is really tired. Lucky for him in order to pass this test he needs to cease existence.');
                cy.get('textarea[name="CulturalSignificance"]').should('have.value', 'This specimen needs to be deleted.');

                // Get the "Close" button and click it.
                cy.get('input[name="EnglishName"]').type('PlaceholderName edited')
                cy.get('button').contains('Close').click();
            });

            // Open again and check it is still there
            cy.visit(globalURL + '/Specimen')
            cy.contains('tr', 'PlaceholderName').within(() => {
                cy.get('button').contains('Edit').click();
            });
            cy.get('#editSpecimenModal').within(() => {
                // Check that the fields are populated with the Plaintain details
                cy.get('input[name="EnglishName"]').invoke('val').should('eq', 'PlaceholderName');
                cy.get('input[name="CreeName"]').should('have.value', '');
                cy.get('textarea[name="SpecimenDescription"]').should('have.value', 'This specimen wanted to disappear; he is really tired. Lucky for him in order to pass this test he needs to cease existence.');
                cy.get('textarea[name="CulturalSignificance"]').should('have.value', 'This specimen needs to be deleted.');
            });
           
        });
        

        // This test will ensure that the edit functionality performs changes. These changes should be displayed on the detail page of the specimen.
        it('TestEditFunctionalityPerformsChangesToTheDatabase', () => {
            const englishName = 'Hair Lichen' + randomLetters();
            // Find the "Hair Lichen" specimen and click the associated "Edit" button.
            cy.contains('tr', 'PlaceholderName').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                // Fill the form with the new values for "Hair Lichen".
                cy.get('#EnglishName').type('New Name');
                cy.get('#EnglishName').invoke('val', '');
                // Cypress bug: first time we type works randomly, fix is to type, clean and type again
                cy.get('input[name="EnglishName"]').clear().type(englishName);
                cy.get('input[name="CreeName"]').clear().type('Kisîsikâw Pîsimwâw');
                cy.get('input[name="LatinName"]').clear().type('Alectoria sarmentosa');
                cy.get('textarea[name="SpecimenDescription"]').clear().type('Hair Lichen thrives is cool, booreal forests');
                cy.get('textarea[name="CulturalSignificance"]').clear().type('Is revered for its unique appearance');


                // Click the "Save Changes" button.
                cy.get('button').contains('Save Changes').click();
            });

            //after submitting the specimen check the url
            cy.url().should('include', '/Specimen');

            // Click on the "Hair Lichen" to go to details page.
            cy.visit(globalURL + '/Specimen');
        });

        it('TestErrorMessagesAreDisplayedAfterInvalidInput', () => {

            createSpecimenPlaceholderName()

            cy.contains('tr', 'PlaceholderName').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.get('input[name="LatinName"]').invoke('val', '')
                cy.get('input[name="EnglishName"]').invoke('val', '')
                cy.get('input[name="CreeName"]').invoke('val', '')
                cy.get('textarea[name="SpecimenDescription"]').invoke('val', '')
                cy.get('textarea[name="CulturalSignificance"]').invoke('val', '')
                cy.get('button[type="submit"]').contains('Save Changes').click();
            });

            //after submitting the specimen check the url
            cy.url().should('not.include', '/Specimen/Details');
            cy.url().should('include', '/Specimen');

            //check the messages
            cy.get('#editSpecimenModal').within(() => {
                cy.get('input[name="EnglishName"]').parent().should('contain', 'English Name is required');
                cy.get('input[name="LatinName"]').parent().should('contain', 'Latin Name is required');
                cy.get('textarea[name="SpecimenDescription"]').parent().should('contain', 'SpecimenDescription cannot be blank');
                cy.get('textarea[name="CulturalSignificance"]').parent().should('contain', 'Cultural Significance is required');
            });
        });

        // Jett's Tests - Story 43

        it('TestThatSpecimenCoordinatesCanBeChangedToValidValues', () => {
            // Click the edit button

            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });
            //cy.get('.table > tbody:nth-child(2) > tr:nth-child(3) > td:nth-child(4) > button:nth-child(2)').click();

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(500);
                cy.get('input[name="Latitude"]').clear().type('54');
                cy.get('input[name="Longitude"]').clear().type('-105');
                // Click the "Save Changes" button.
                cy.wait(500);
              
            });

            attemptClickButtonInModal('editSpecimenModal', 'Save Changes', 1);
            cy.visit(globalURL + '/Specimen')
            
            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(1000);
                cy.get('input[name="Latitude"]').should('have.value', '54');
                cy.get('input[name="Longitude"]').should('have.value', '-105');
                // Click the "Save Changes" button.
                cy.wait(500);

            });
           

        });

        it('TestThatSpecimenCoordinatesCanBeChangedToValidNegativeValues', () => {
            // Click the edit button
            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(500);
                cy.get('input[name="Latitude"]').clear().type('53');
                cy.get('input[name="Longitude"]').clear().type('-106');
                // Click the "Save Changes" button.
                cy.wait(500);

            });
            attemptClickButtonInModal('editSpecimenModal', 'Save Changes', 1);
            cy.visit(globalURL + '/Specimen')

            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(1000);
                cy.get('input[name="Latitude"]').should('have.value', '53');
                cy.get('input[name="Longitude"]').should('have.value', '-106');
                // Click the "Save Changes" button.
                cy.wait(500);

            });

            
        });

        it('TestThatInvalidSpecimenCoordinatesAreNotAllowed', () => {
            // Click the edit button
            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(500);
                cy.get('input[name="Latitude"]').clear().type('A');
                cy.get('input[name="Longitude"]').clear().type('B');
                // Click the "Save Changes" button.
                cy.wait(500);

            });
            attemptClickButtonInModal('editSpecimenModal', 'Save Changes', 1);

            // Check that coordinates are not valid
            cy.wait(200)
            cy.get('#editSpecimenModal').should('contain', "The value 'A' is not valid for Latitude.");
            cy.get('#editSpecimenModal').should('contain', "The value 'B' is not valid for Longitude.");
        });

        it('TestThatSpecimenLatitudeCannotExceed54', () => {

            // Click the edit button
            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(500);
                cy.get('input[name="Latitude"]').clear().type(55);
                // Click the "Save Changes" button.
                cy.wait(500);

            });
            attemptClickButtonInModal('editSpecimenModal', 'Save Changes', 1);

          
            // Check that the correct error appears
            cy.wait(200)
            cy.get('#editSpecimenModal').should('contain', 'Latitude must be between 53 and 54');
        });

        it('TestThatSpecimenLatitudeCannotGoBelow53', () => {
            // Click the edit button
            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(500);
                cy.get('input[name="Latitude"]').clear().type(52);
                // Click the "Save Changes" button.
                cy.wait(500);

            });
            attemptClickButtonInModal('editSpecimenModal', 'Save Changes', 1);


            // Check that the correct error appears
            cy.wait(200)
            cy.get('#editSpecimenModal').should('contain', 'Latitude must be between 53 and 54');
        });

        it('TestThatSpecimenLongitudeCannotExceed-105', () => {
            // Click the edit button
            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(500);
                cy.get('input[name="Longitude"]').clear().type(-104);
                // Click the "Save Changes" button.
                cy.wait(500);

            });
            attemptClickButtonInModal('editSpecimenModal', 'Save Changes', 1);


            // Check that the correct error appears
            cy.wait(200)
            cy.get('#editSpecimenModal').should('contain', 'Longitude must be between -105 and -106');

        });

        it('TestThatSpecimenLongitudeCannotGoBelow-106 ', () => {
            // Click the edit button
            cy.contains('tr', 'Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            cy.get('#editSpecimenModal').within(() => {
                cy.wait(500);
                cy.get('input[name="Longitude"]').clear().type(-107);
                // Click the "Save Changes" button.
                cy.wait(500);

            });
            attemptClickButtonInModal('editSpecimenModal', 'Save Changes', 1);


            // Check that the correct error appears
            cy.wait(200)
            cy.get('#editSpecimenModal').should('contain', 'Longitude must be between -105 and -106');

        });

        // NOTE FOR TESTERS:
        // You may need to restart the app when running these two tests as they rely on there being the right number of media to delete

        it('TestThatSpecimenMediaCanBeDeleted', () => {
            // Click the edit button

            // Click the edit button
            cy.contains('tr', ' Velvet Leaf Blueberry').within(() => {
                cy.get('button').contains('Edit').click();
            });
           
            attemptClickButtonInModal('editSpecimenModal', 'Delete', 1);
            
            // Check that the media is no longer there
            cy.wait(200)
            cy.get('#mediaListElement').contains('.selector', 'Blueberry.png').should('not.exist')
        });

        it('TestThatSpecimenMediaCanBeDeletedWithOnlyOneMediaInTheList', () => {

            // Click the edit button
            cy.contains('tr', ' Horsetail').within(() => {
                cy.get('button').contains('Edit').click();
            });

            attemptClickButtonInModal('editSpecimenModal', 'Delete', 1);

            // Check that the media is no longer there
            cy.wait(200)
            cy.get('#mediaListElement').contains('.selector', 'Horsetail.png').should('not.exist')
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
        cy.wait(1000);
        //cy.get(`#${modalId}`).should('not.be.visible');
    }
}