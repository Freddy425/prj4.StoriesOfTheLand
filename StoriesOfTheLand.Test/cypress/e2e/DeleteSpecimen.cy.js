describe('template spec', () => {
    let globalURL = 'https://localhost:7202'
    beforeEach(() => {
        // We made cypress visit the specimen index list
        cy.visit(globalURL + '/Specimen')
    })

    context('TestDeleteFunctionality', () => {


        //This test will ensure that each specimen have a delete button next to it
        it('TestThatDeleteButtonIsNextToEachSpecimen', () => {

            // Count the number of <tr> in the page which might contain specimens.
            cy.get('tbody').find('tr').then(specimenRows => {
                const totalSpecimens = specimenRows.length;

                // Now, count the number of Delete buttons within these rows.
                cy.get('tr').find('button').filter(':contains("Delete")').then(deleteButtons => {
                    const totalDeleteButtons = deleteButtons.length;

                    // Compare the count of specimen rows with the count of Delete buttons.
                    expect(totalDeleteButtons).to.equal(totalSpecimens, 'Each specimen has a Delete button next to it');
                });
            });
        })

        //This test will check that after clicking delete the confirmation popup appears
        it('TestDeleteButtonMakesConfirmationPopUpAppearsAndClickingCancelClosesThePopUpWithNoChangesMade', () => {

            createSpecimenPlaceholderName()

            // Find the <tr> that contains the text "PlaceholderName". This looks for the row with the specimen we want to delete.
            cy.contains('tr', 'PlaceholderName').within(() => {
                // Click on the "Delete" button within this row.
                cy.get('button').contains('Delete').click();
            });

            // Wait for the modal to be fully open
            cy.get('#deleteSpecimenModal').should('be.visible')
                .and('contain', 'Confirm Deletion of Specimen')
                .and('contain', 'Are you sure you want to delete "PlaceholderName" specimen? This action cannot be undone');

            cy.wait(100);

            // Attempt to find and click the Cancel button in the popup three times
            let success = false;

            // Attempt to click the 'Cancel' button 3 times
            attemptClickCancel(3);
            cy.wait(100);

            cy.get('table.table').contains('PlaceholderName').scrollIntoView().click();

            // Verify the details of the "PlaceholderName" specimen to ensure they are unchanged.
            // For English Name
            cy.contains('div', 'English Name').parent().contains('PlaceholderName').should('exist');


            // For Cultural Significance
            cy.contains('div', 'Cultural Significance').parent().contains('This specimen needs to be deleted.').should('exist');

            // For Description
            cy.contains('div', 'Description').parent().contains('This specimen wanted to disappear; he is really tired. Lucky for him in order to pass this test he needs to cease existence.').should('exist');

            // Ensure the QR Code toggle is visible
            cy.contains('button', 'Show/Hide the QR code for this specimen').should('exist');

        });


        //This test checks that clicking Delete on the confirmation popup performs changes and the specimen is not available anymore
        it('TestClickingDeleteSpecimenInDeleteConfirmationClosesThePopUpAndChangesAreMade', () => {

            // Find the <tr> that contains the text "PlaceholderName". This looks for the row with the specimen we want to delete.
            cy.contains('tr', 'PlaceholderName').within(() => {
                // Click on the "Delete" button within this row.
                cy.get('button').contains('Delete').click();
            });

            // Wait for the modal to be fully open
            cy.get('#deleteSpecimenModal').should('be.visible')
                .and('contain', 'Confirm Deletion of Specimen')
                .and('contain', 'Are you sure you want to delete "PlaceholderName" specimen? This action cannot be undone');


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

            // Check that "PlaceholderName" is no longer in the page, indicating it was deleted.
            cy.contains('tr', 'PlaceholderName').should('not.exist');
        });

        //This test checks that clicking Delete on the confirmation popup deletes the clicked specimen and verifies the count
        it('TestDeleteSpecimenOnlyDeletesOneSpecimenAndDeletesTheCorrectOne', () => {

            createSpecimenPlaceholderName()

            // Get the number of <tr> elements before deletion. Each specimen is inside one <tr> element
            cy.get('tbody').find('tr').then(specimenRowsBeforeDelete => {
                const totalSpecimensBeforeDelete = specimenRowsBeforeDelete.length;

                // Find the <tr> that contains the text "PlaceholderName". This looks for the row with the specimen we want to delete.
                cy.contains('tr', 'PlaceholderName').within(() => {
                    // Click on the "Delete" button within this row.
                    cy.get('button').contains('Delete').click();
                });

                cy.wait(1000);

                // Delete specimen.
                cy.get('#deleteSpecimenModal').within(() => {
                    cy.get('button').contains('Delete Specimen').click();
                });

                // Get the number of <tr> elements after deletion.
                cy.get('tbody').find('tr').then(specimenRowsAfterDelete => {
                    const totalSpecimensAfterDelete = specimenRowsAfterDelete.length;

                    // Check that the count of specimens after deletion is one less than before.
                    expect(totalSpecimensAfterDelete).to.equal(totalSpecimensBeforeDelete - 1);

                    // Check that "PlaceholderName" is no longer on the page, indicating it has been deleted.
                    cy.contains('tr', 'PlaceholderName').should('not.exist');
                });
            });
        });

    })
})
function attemptClickCancel(times) {
    if (times > 0) {
        cy.get('body').then(body => {
            // Check if the 'Cancel' button exists within the modal
            if (body.find('#deleteSpecimenModal button:contains("Cancel")').length > 0) {
                cy.get('#deleteSpecimenModal').within(() => {
                    cy.wait(100); // Wait for potential animations or transitions
                    cy.get('button').contains('Cancel').click({ force: true });
                });
            }
        }).then(() => {
            // Recursive call to attempt the next click
            attemptClickCancel(times - 1);
        });
    }
}

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
