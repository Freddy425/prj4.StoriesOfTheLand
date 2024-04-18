describe('Test Map Page for Stories of the Land Application', () => {
    let globalURL = 'https://localhost:7202'
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.visit(globalURL + '/Specimen/Map')
    })

    /**
     * LOG IN TO MICROSOFT BEFORE RUNNING
     * BECAUSE ADMIN
     */

    context('AllUTMTests', () => {


        it('TestUTMtoLatLong', () => {

            // Check if map exist before starting
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1) //same location
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');


         //edit the first specimen and check the values match what they are supposed to be
            cy.get('.edit-btn:first').click();
            cy.get('#locationEditModal').should('be.visible')
            cy.wait(1000);
            cy.get("#latInput").should('have.value', 53.87971);
            cy.get("#longInput").should('have.value', -105.40012);
            cy.wait(1000);

            //switches to UTM and checks that the values were converted correctly
            cy.get('#utm').click();
            cy.get("#latInput").should('have.value', 473696.82733);
            cy.get("#longInput").should('have.value', 5970212.46315);

            //clears the values and enters valid UTM coordinates
            cy.get('#latInput').clear();
            cy.wait(1000);
            cy.get('#latInput').type('473697');
            cy.wait(1000);
            cy.get('#longInput').clear();
            cy.get('#longInput').type('5970212');

            //clicks the saved button
            cy.get('#locationEditModal .btn-primary').click();
            //the modal should not be visible
            cy.get('#locationEditModal').should('not.be.visible');

            //go back into the first marker edit button
            cy.get('.edit-btn:first').click();
            //checks that the values were properly converted to Lat and Long when saved
            cy.get("#latInput").should('have.value', 53.87971);
            cy.get("#longInput").should('have.value', -105.40012);
        })


        it('TestInvalidUTMLowerBoundary', () => {
            // Check if map exist before starting
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1) //same location
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');

             //edit the first specimen and check the values match what they are supposed to be
            cy.get('.edit-btn:first').click();
            cy.get('#locationEditModal').should('be.visible')
            cy.get("#latInput").should('have.value', 53.87971);
            cy.get("#longInput").should('have.value', -105.40012);

            //switches to UTM and checks that the values were converted correctly
            cy.get('#utm').click();
            cy.get("#latInput").should('have.value', 473696.82733);
            cy.get("#longInput").should('have.value', 5970212.46315);

            //clears values and enters invalid values for lower boundary
            cy.get('#latInput').clear();
            cy.wait(1000);
            cy.get('#latInput').type('434450');
            cy.wait(1000);
            cy.get('#longInput').clear();
            cy.get('#longInput').type('5072200');
            cy.get('#locationEditModal').click();

            //error message should appear stating you are out of bounds for Easting and Northing
            cy.get('#northingErrorMessage').should('be.visible')
            cy.get('#eastingErrorMessage').should('be.visible')

            //clicks the save button
            cy.get('#locationEditModal .btn-primary').click();
            //modal should pop open stating those values could not be saved
            cy.get('#locationEditModal').should('be.visible');
            cy.get('.invalid-feedback').should('be.visible');

            })


        it('TestInvalidUTMUpperBoundary', () => {
            // Check if map exist before starting
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1) //same location
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');

            //edit the first specimen and check the values match what they are supposed to be
            cy.get('.edit-btn:first').click();
            cy.get('#locationEditModal').should('be.visible')
            cy.get("#latInput").should('have.value', 53.87971);
            cy.get("#longInput").should('have.value', -105.40012);

            //switches to UTM and checks that the values were converted correctly
            cy.get('#utm').click();
            cy.get("#latInput").should('have.value', 473696.82733);
            cy.get("#longInput").should('have.value', 5970212.46315);

            //clears the values and entere invalid value above the boundary
            cy.get('#latInput').clear();
            cy.wait(1000);
            cy.get('#latInput').type('600000');
            cy.wait(1000);
            cy.get('#longInput').clear();
            cy.get('#longInput').type('6083990');
            cy.get('#locationEditModal').click();

            //error messages should appear stating you are above the Norhting and Easting boundart
            cy.get('#northingErrorMessage').should('be.visible')


            //Clicks the save button
            cy.get('#locationEditModal .btn-primary').click();
            //Modal should reopen with an error message displaying
            cy.get('#locationEditModal').should('be.visible');
            cy.get('.invalid-feedback').should('be.visible');

        })
        it('TestAdminEntersOneAndLeavesTheOtherBlank', () => {
            // Check if map exist before starting
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1) //same location
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');

            //edit the first specimen and check the values match what they are supposed to be
            cy.get('.edit-btn:first').click();
            cy.get('#locationEditModal').should('be.visible')
            cy.get("#latInput").should('have.value', 53.87971);
            cy.get("#longInput").should('have.value', -105.40012);

            //switches to UTM and checks that the values were converted correctly
            cy.get('#utm').click();
            cy.get("#latInput").should('have.value', 473696.82733);
            cy.get("#longInput").should('have.value', 5970212.46315);

            //clears the values and only enters a value for Easting
            cy.get('#latInput').clear();
            cy.wait(1000);
            cy.get('#latInput').type('473697');
            cy.wait(1000);
            cy.get('#longInput').clear();
            cy.get('#locationEditModal').click();

            //error message for empty value should appear
            cy.get('#emptyLatAndLongInput').should('be.visible')
            //the save button should be disabled
            cy.get('#locationEditModal .btn-primary').should('be.disabled');

            cy.get('#locationEditModal').should('be.visible');

        })
     })

        context('TestAllMobileTests', () => {

            it('MobileEditSpecimenLatLongWindowCentered', () => {
                // Check if map exist before starting
                cy.viewport('iphone-x')
                /*
                Click the first edit button in the specimen list. 
                check to see that modal is centered
                */
                cy.get('#locationlist').should('exist');
                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('exist');
                // Wait for the modal to become visible
                cy.get('#locationEditModal').should('be.visible');

                // Get the modal and check its width and height
                cy.get('.modal-dialog').should('have.css', 'width', '359px');
                cy.get('.modal-dialog').should('have.css', 'height', '364px');
            })



            it('MobileEditLatLongSpecimenModalFunctions', () => {
                // Check if map exist before starting
                cy.viewport('iphone-x')
                /*
                Click “Velvet Leaf Blueberry” in the specimen list, 
                in Edit Specimen modal, 
                click cancel. 
                check that modal is no longer visible
                */

                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //previous location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 136, 231)');

                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('be.visible')

                cy.get('#latInput').clear();
                cy.get('#latInput').type('53.7');
                cy.get('#longInput').clear();
                cy.get('#longInput').type('-105.5');
                // Click the cancel button
                cy.get('#locationEditModal .btn-secondary').click();

                // Assert that the modal is no longer visible
                cy.get('#locationEditModal').should('not.be.visible');

                //

                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //same location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 136, 231)');

                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('be.visible')

                cy.get('#latInput').clear();
                cy.get('#latInput').type('53.9');
                cy.get('#longInput').clear();
                cy.get('#longInput').type('-105.9');
                cy.get('#locationEditModal').click();
                cy.get('#locationEditModal .btn-primary').click();

                //MobileEditSpecimenValidSubmitClosesWindow
                cy.get('#locationEditModal').should('not.be.visible');

                //MobileCheckThatMapUpdatesOnChange
                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //new location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 136, 231)');

                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('be.visible')

                cy.get('#latInput').clear();
                cy.get('#latInput').type('3');
                cy.get('#longInput').clear();
                cy.get('#longInput').type('3');
                cy.get('#locationEditModal .btn-primary').click();
                //page reloads
                cy.get('#locationEditModal').should('be.visible');
                // Verify that the error message is visible
                cy.get('.invalid-feedback').should('be.visible');
            })


            it('MobileSpecimenRemoveLocation', () => {
                /**
                * In Location List, 
                * Velvelt leaf Blueberry
                click remove location. 
                check that modal is no longer visible
                */

                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //new location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');


                // Click the delete button for a specific location
                cy.get('.delete-btn').first().click();

                // Wait for the delete modal to become visible
                cy.get('#locationDeleteModal').should('be.visible');

                // Click the confirm delete button
                cy.get('.modal-footer .btn-danger').click();

                // Verify that the delete modal is no longer visible
                cy.get('#locationDeleteModal').should('be.hidden');

                //cy.get('#locationlistbody > tr:nth-child(1) > th:nth-child(2) > p').should('contain', 'No Marker Data');

            })

        })




        context('TestAllDesktopTests', () => {

            it('DesktopEditSpecimenLatLongWindowCentered', () => {
                // Check if map exist before starting

                /*
                Click the first edit button in the specimen list. 
                check to see that modal is centered
                */
                cy.get('#locationlist').should('exist');
                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('exist');
                // Wait for the modal to become visible
                cy.get('#locationEditModal').should('be.visible');

                // Get the modal and check its width and height
                cy.get('.modal-dialog').should('have.css', 'width', '500px');
                cy.get('.modal-dialog').should('have.css', 'height', '364px');
            })



            it('DesktopEditLatLongSpecimenModalFunctions', () => {
                // Check if map exist before starting

                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //previous location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');

                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('be.visible')

                cy.get('#latInput').clear();
                cy.get('#latInput').type('53.7');
                cy.get('#longInput').clear();
                cy.get('#longInput').type('-105.5');
                // Click the cancel button
                cy.get('#locationEditModal .btn-secondary').click();

                // Assert that the modal is no longer visible
                cy.get('#locationEditModal').should('not.be.visible');

                //

                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //same location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');

                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('be.visible')

                cy.get('#latInput').clear();
                cy.get('#latInput').type('53.9');
                cy.get('#longInput').clear();
                cy.get('#longInput').type('-105.9');
                cy.get('#locationEditModal').click();
                cy.get('#locationEditModal .btn-primary').click();

                //DesktopEditSpecimenValidSubmitClosesWindow
                cy.get('#locationEditModal').should('not.be.visible');

                //DesktopCheckThatMapUpdatesOnChange
                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //new location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');

                cy.get('.edit-btn:first').click();
                cy.get('#locationEditModal').should('be.visible')

                cy.get('#latInput').clear();
                cy.get('#latInput').type('3');
                cy.get('#longInput').clear();
                cy.get('#longInput').type('3');
                cy.get('#locationEditModal .btn-primary').click();
                //page reloads
                cy.get('#locationEditModal').should('be.visible');
                // Verify that the error message is visible
                cy.get('.invalid-feedback').should('be.visible');
            })


            it('DesktopSpecimenRemoveLocation', () => {
                /**
                * In Location List, 
                * Velvelt leaf Blueberry
                click remove location. 
                check that modal is no longer visible
                */

                cy.get('#map')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('div')
                    .find('svg')
                    .eq(1) //new location
                    .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');


                // Click the delete button for a specific location
                cy.get('.delete-btn').first().click();

                // Wait for the delete modal to become visible
                cy.get('#locationDeleteModal').should('be.visible');

                // Click the confirm delete button
                cy.get('.modal-footer .btn-danger').click();

                // Verify that the delete modal is no longer visible
                cy.get('#locationDeleteModal').should('be.hidden');

                //cy.get('#locationlistbody > tr:nth-child(1) > th:nth-child(2) > p').should('contain', 'No Marker Data');

            })

        })

    })