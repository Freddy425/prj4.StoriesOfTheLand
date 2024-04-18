describe('template spec', () => {
    let globalURL = 'https://localhost:7202'

    beforeEach(() => {

        cy.visit(globalURL + '/Resources');
    })


    context('Delete Resource Tests', () => {

        //This test will check there is a delete button next to each resource
        it('TestThatDeleteButtonIsNextToEachResource', () => {

            // Count the number of <tr> in the page which might contain specimens.
            cy.get('tbody').find('tr').each((row) => {
                // Within each row, verify the presence of a Delete button.
                cy.wrap(row).find('button').filter(':contains("Delete")').should('have.length', 1);
            });
        });

        //This test will check a resource is not deleted if user clicks on cancel in the modal
        it('TestDeleteButtonMakesConfirmationPopUpAppearsAndClickingCancelClosesThePopUpWithNoChangesMade', () => {
            // Find the "Saskatchewan Polytechnic" resource and click the associated "Delete" button.
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
                cy.get('button').contains('Delete').click();
            });

            // Confirmation pop-up appears
            cy.get('#DeleteResourceModal').should('be.visible');


            // Attempt to click the 'Cancel' button 3 times

            attemptClickButtonInModal('DeleteResourceModal', 'Cancel', 5);
            cy.wait(100);
            // Pop-up closes
            cy.wait(500);

            cy.get('#DeleteResourceModal').should('not.be.visible');

            // "Saskatchewan Polytechnic" resource is still in the list
            cy.contains('tr', 'Saskatchewan Polytechnic').should('be.visible');
        });

        //This test will check both the english resource and french resource are deleted , a confirmation appears, and others resources are the same 
        it('TestClickingDeleteResourceInDeleteConfirmationClosesThePopUpAndBothLanguagesResourcesAreDeleted', () => {
            // Find the "Saskatchewan Polytechnic" resource and click the associated "Delete" button.
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
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

            // Verify "Saskatchewan Polytechnic" resource is no longer in the list
            cy.contains('tr', 'Saskatchewan Polytechnic').should('not.exist');

            //verify the other resource is still available

            cy.contains('tr', 'NCTR').should('exist');

            //clicks on French button
            cy.get('button').contains('Learner View').click();
                        cy.get('#toggleLanguage').click();

            cy.contains('tr', 'Polytechnique de la Saskatchewan').should('not.exist');
        });

        //This test will check on the deletion on the last resource
        it('TestDeleteLastResource', () => {

            cy.contains('tr', 'NCTR').within(() => {
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

            // Verify "NCTR" resource is no longer in the list
            cy.contains('tr', 'NCTR').should('not.exist');

            //check there is no delete button

            cy.contains('tr', 'Delete').should('not.exist');

            cy.get('button').contains('Learner View').click();
                        cy.get('#toggleLanguage').click();

            // Verify "NCTR" resource is no longer in the list
            cy.contains('tr', 'NCTR').should('not.exist');

        });
    });

    context('Create Resource Tests', () => {

        beforeEach(() => {
            cy.get('button').contains('Create Resource').click();
        });

        //This test will check you can add the first resource and the learner view is correct
        it('TestAddFirstValidResourceIsDisplayedInLearnerViewAndRedirects', () => {
            cy.get('#CreateResourceModal').should('be.visible');
            cy.get('#CreateResourceModal').within(() => {
                typeWithRetry('#ResourceTitle', 'NCTR');
                typeWithRetry('#ResourceDescription', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');
                typeWithRetry('#ResourceURL', 'https://nctr.ca/');
                cy.get('button[type="submit"]').contains('Create').click();
            });

            // Ensure the resource has been added successfully
            cy.contains('tr', 'NCTR').should('be.visible');

            // Click the Learner View button to switch views
            cy.get('#toggleLearnerView').click();

            // Ensure the Learner View is visible
            cy.get('.resource-container').should('be.visible');

            // Now, find the container with the "NCTR" resource in the Learner View
            cy.get('.resource-container').contains('NCTR').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'NCTR');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://nctr.ca/');
            });
        });


        //This test will check that certain fields cant be repeated. 
        //And then it will record a valid resource. to check we can add a value after there is one already
        it('TestUniqueParamMustBeUniqueAndAddASecondResource', () => {

            cy.get('#CreateResourceModal').should('be.visible');
            cy.get('#CreateResourceModal').within(() => {
                typeWithRetry('#ResourceTitle', 'NCTR');
                typeWithRetry('#ResourceDescription', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');
                typeWithRetry('#ResourceURL', 'https://nctr.ca/');
                cy.get('button[type="submit"]').contains('Create').click();
            });


            // Wait for the resource to be processed
            cy.wait(1000);

            cy.get('#CreateResourceModal').within(() => {

                // Extend the timeout and use a regular expression for a flexible match
                cy.get('#ResourceTitleError', { timeout: 1000 }).should('have.text', "The title must be unique");
                cy.get('#ResourceURLError', { timeout: 10000 }).should('have.text', "The URL must be unique");


                typeWithRetry('#ResourceTitle', 'Saskatchewan Polytechnic');
                typeWithRetry('#ResourceDescription', 'Saskpoly Is The Best');
                typeWithRetry('#ResourceURL', 'https://saskpolytech.ca/');
                cy.get('button[type="submit"]').contains('Create').click();

            });


            cy.wait(1000);

            // Check for the presence of CreateResource1 in the list
            cy.contains('NCTR').should('exist');

            // Check for the presence of CreateResource2 in the list
            cy.contains('Saskatchewan Polytechnic').should('exist');
        });

        // This test will check that the correct validation messages are displayed with an invalid form
        it('TestThatInvalidFormDisplaysTheValidationMessages', () => {
            // User tries to Create without filling mandatory fields
            cy.get('button[type="submit"]').contains('Create').click();


            cy.get('#CreateResourceModal').within(() => {
                // Extend the timeout and use a regular expression for a flexible match
                cy.get('#ResourceTitleError', { timeout: 1000 }).should('have.text', "Title is mandatory");
                cy.get('#ResourceURLError', { timeout: 1000 }).should('have.text', "URL is mandatory");
            });

        });


    });


    context('Edit Resource Tests', () => {

        //This test will check that there is a Edit button for each Resource
        it('TestThatEditButtonIsNextToEachResource', () => {
            // Count the number of <tr> in the page which might contain Resources.
            cy.get('tbody').find('tr').then(resourceRows => {
                const totalResources = resourceRows.length;

                // Now, count the number of Delete buttons within these rows.
                cy.get('tr').find('button').filter(':contains("Edit")').then(editButtons => {
                    const totalEditButtons = editButtons.length;

                    // Compare the count of resouce rows with the count of Edit buttons.
                    expect(totalEditButtons).to.equal(totalResources, 'Each resource has a Delete button next to it');
                });
            });
        });

        //This test will check that the modal opens and it contains the correct fields.
        it('TestThatEditFormOpens', () => {
            // Assuming the first edit button corresponds to the first resource in the list
            cy.get('.edit-btn').first().click();

            // Edit resource
            cy.get('#EditResourceModal').within(() => {
                // Check for the presence of input fields on the Edit form.
                cy.get('input[name="ResourceTitle"]').should('be.visible');
                cy.get('textarea[name="ResourceDescription"]').should('be.visible');
                cy.get('input[name="ResourceURL"]').should('be.visible');

                // Check for the presence of "Back" and "Save Changes" buttons.
                cy.get('button').contains('Close').should('be.visible');
                cy.get('button').contains('Save Changes').should('be.visible');
            });
        });


        //This test will check if opening a edit page it is poblated with the previous values of the resource
        it('TestThatEditFormIsPopulatedWithTheResourceAndClosingItDoesntMakeChanges', () => {

            // Find the original resource and click the associated "Edit" button.
            cy.contains('tr', 'NCTR ').within(() => {
                cy.get('button').contains('Edit').click();
            });
            // Edit Resource
            cy.get('#EditResourceModal').within(() => {
                // Check that the fields are populated with the Plaintain details
                cy.get('input[name="ResourceTitle"]').invoke('val').should('eq', 'NCTR');
                cy.get('textarea[name="ResourceDescription"]').should('have.value', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');
                cy.get('input[name="ResourceURL"]').should('have.value', 'https://nctr.ca/');

                // Get the "Close" button and click it.
                cy.get('input[name="ResourceTitle"]').type('PlaceholderName')
                cy.get('button').contains('Close').click();
            });

            // Open again and check it is still there
            cy.contains('tr', 'NCTR').within(() => {
                cy.get('button').contains('Edit').click();
            });
            cy.get('#EditResourceModal').within(() => {
                // Check that the title didnt change
                cy.get('input[name="ResourceTitle"]').invoke('val').should('eq', 'NCTR');
                cy.get('textarea[name="ResourceDescription"]').should('have.value', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');
                cy.get('input[name="ResourceURL"]').should('have.value', 'https://nctr.ca/');

            });

           

        });

        //This test will ensure that editing successfully make changes
        it('TestEditFunctionalityPerformsChangesToTheDatabase', () => {
            // User navigates to the Edit form of the resource
            cy.contains('tr', 'NCTR').within(() => {
                cy.get('button').contains('Edit').click();
            });

            //User edits the resource details with information from EditResource2
            cy.get('#EditResourceModal').within(() => {
                cy.wait(500);
                cy.get('input[name="ResourceTitle"]').clear().type('Indigenous Knowledge Portal');
                cy.get('textarea[name="ResourceDescription"]').clear().type('The Indigenous Knowledge Portal offers a comprehensive database of resources and materials relating to indigenous cultures, languages, and history. It supports educational and research endeavors aimed at fostering a deeper appreciation of Indigenous Peoples\' contributions to society.');
                cy.get('input[name="ResourceURL"]').clear().type('https://indigenousknowledge.example.com/');

                // User saves the changes
                cy.get('button').contains('Save Changes').click();
            });

            //check that in the table the values are correct.

            // Ensure the resource has been added successfully
            cy.contains('tr', 'Indigenous Knowledge Portal').should('be.visible');


            // User navigates to the Learner View to verify the changes
            cy.get('button').contains('Learner View').click();

            // User finds the edited resource in the container and verifies its presence
            cy.contains('.resource-container', 'Indigenous Knowledge Portal').should('be.visible');

            // Now, find the container with the "Indigenous Knowledge Portal" resource in the Learner View
            cy.get('.resource-container').contains('Indigenous Knowledge Portal').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Indigenous Knowledge Portal');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', 'The Indigenous Knowledge Portal offers a comprehensive database of resources and materials relating to indigenous cultures, languages, and history. It supports educational and research endeavors aimed at fostering a deeper appreciation of Indigenous Peoples\' contributions to society.');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://indigenousknowledge.example.com/');
            });
        });


        //This test will check that invalid messages are displayed and then edit the resource
        it('TestErrorMessagesAreDisplayedAfterInvalidInputAndThenEdit', () => {
            //User navigates to the Edit form of the resource
            cy.contains('tr', 'Indigenous Knowledge Portal').within(() => {
                cy.get('button').contains('Edit').click();
            });

            //User edits the resource details with information from EditResource3
            cy.get('#EditResourceModal').within(() => {
                cy.get('input[name="ResourceTitle"]').clear();
                cy.get('textarea[name="ResourceDescription"]').clear().type('a');
                cy.get('input[name="ResourceURL"]').clear();

                // User saves the changes
                cy.get('button').contains('Save Changes').click();
            });

            // Verify error messages
            cy.get('#EditResourceModal').within(() => {
                // Extend the timeout and use a regular expression for a flexible match
                cy.get('#ResourceTitleError', { timeout: 1000 }).filter(':visible').should('have.text', "Title is mandatory");
                cy.get('#ResourceURLError', { timeout: 1000 }).filter(':visible').should('have.text', "URL is mandatory");
                cy.get('#ResourceDescriptionError', { timeout: 1000 }).filter(':visible').should('have.text', "Description must be between 4 and 1000 characters.");

                
            });
            

            //Verify that the original value is still available
            cy.contains('tr', 'Indigenous Knowledge Portal').should('be.visible');


            // Verify error messages
            cy.get('#EditResourceModal').within(() => {
                // Extend the timeout and use a regular expression for a flexible match
                typeWithRetry('#ResourceTitle', 'NCTR');
                cy.get('textarea[name="ResourceDescription"]').clear()
                typeWithRetry('#ResourceDescription', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');
                typeWithRetry('#ResourceURL', 'https://nctr.ca/');

                // User saves the changes
                cy.get('button').contains('Save Changes').click();

            });

            cy.get('button').contains('Learner View').click();

            // Now, find the container with the "NCTR" resource in the Learner View
            cy.get('.resource-container').contains('NCTR').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'NCTR');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://nctr.ca/');
            });
        });

       
    })


    context('Manage Images Tests', () => {

        it('TestValidationErrorsAreDisplayed', () => {

            //User navigates to the Edit form of the resource
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
                cy.get('button').contains('Add Image').click();
            });


            // Verify error messages
            cy.get('#mediaModal').within(() => {
                // User saves the changes


                //select the invalid file and attatch it the the file input
                cy.get('#file-input').attachFile('example.json');

                cy.get('button').contains('Add').click();

            });

            cy.wait(1000);

            cy.contains('An error occurred while uploading the image. Please ensure the file is an image and try again.')
        });


        it('SucessfullyUploadAnImage', () => {

            //User navigates to the Edit form of the resource
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
                cy.get('button').contains('Add Image').click();
            });


            // Verify error messages
            cy.get('#mediaModal').within(() => {
                cy.get('#file-input').attachFile(['LabTeaLeaves.png', 'LabTeaPlants.png']);
                ;

                cy.get('button').contains('Add').click();

            });

            cy.wait(1000);

            cy.get('#mediaModal').should('not.be.visible');

            cy.get('button').contains('Learner View').click();


            cy.get('.resource-container').contains('Saskatchewan Polytechnic').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Saskatchewan Polytechnic');

                // Verify the image source is updated
                cy.get('.card-img-top').should('not.have.attr', 'src', 'images/default.png');
            });

            //Now the button is delete media
            //User navigates to the Edit form of the resource
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
                cy.get('button').contains('Remove Image').click();
            });
        });

        it('SucessfullyUploadAnImageWithRepeatedName', () => {

            //User navigates to the Edit form of the resource
            cy.contains('tr', 'NCTR').within(() => {
                cy.get('button').contains('Add Image').click();
            });



            cy.get('#mediaModal').within(() => {
                cy.get('#file-input').attachFile(['LabTeaLeaves.png', 'LabTeaPlants.png']);
                ;

                cy.get('button').contains('Add').click();

            });

            cy.wait(1000);

            cy.get('#mediaModal').should('not.be.visible');

            cy.get('button').contains('Learner View').click();


            cy.get('.resource-container').contains('NCTR').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'NCTR');

                // Verify the image source is updated
                cy.get('.card-img-top').should('not.have.attr', 'src', 'images/default.png');
            });

            //Now the button is delete media
            
            cy.contains('tr', 'NCTR').within(() => {
                cy.get('button').contains('Remove Image').click();
            });
        });



        it('DeleteImageAfterClickingNoAtFirst', () => {

            //User navigates to the Edit form of the resource
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
                cy.get('button').contains('Remove Image').click();
            });


            // Verify error messages
            cy.get('#DeleteMediaModal').within(() => {
                cy.wait(100);
            });
            attemptClickButtonInModal('DeleteMediaModal', 'Cancel', 5);

            cy.wait(100);

            //User navigates to the Edit form of the resource
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
                cy.get('button').contains('Remove Image').click();
            });

            cy.get('#DeleteMediaModal').within(() => {
                cy.get('button').contains('Delete Image').click();
            });
            // User navigates to the Learner View to verify the changes
            cy.get('button').contains('Learner View').click();

            cy.get('.resource-container').contains('Saskatchewan Polytechnic').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Saskatchewan Polytechnic');

                // Verify the image source is updated
                cy.get('.card-img-top').should('have.attr', 'src', 'images/default.png');
            });

            //The other resource still have the image

            cy.get('.resource-container').contains('NCTR').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'NCTR');

                // Verify the image source is updated
                cy.get('.card-img-top').should('not.have.attr', 'src', 'images/default.png');
            });


            //User navigates to the Edit form of the resource
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
                cy.get('button').contains('Add Image').click();
            });
        });

        //This test will delete the image of the last resource with an image
        it('DeleteImageOfLastResource', () => {

            //User navigates to the Edit form of the resource
            cy.contains('tr', 'NCTR').within(() => {
                cy.get('button').contains('Remove Image').click();
            });


            cy.get('#DeleteMediaModal').within(() => {
                cy.get('button').contains('Delete Image').click();
            });
            // User navigates to the Learner View to verify the changes
            cy.get('button').contains('Learner View').click();

            cy.get('.resource-container').contains('NCTR').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'NCTR');

                // Verify the image source is updated
                cy.get('.card-img-top').should('have.attr', 'src', 'images/default.png');
            });


            //Add image button is available
            cy.contains('tr', 'NCTR').within(() => {
                cy.get('button').contains('Add Image').click();
            });
        });
    })

    context('French Resource Tests', () => {


        //This test will check both the english resource and french resource are deleted , a confirmation appears, and others resources are the same 
        it('TestClickingDeleteResourceInDeleteConfirmationClosesThePopUpAndBothLanguagesResourcesAreDeleted', () => {
            // Find the "Saskatchewan Polytechnic" resource and click the associated "Delete" button.
            cy.contains('tr', 'Saskatchewan Polytechnic').within(() => {
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

            // Verify "Saskatchewan Polytechnic" resource is no longer in the list
            cy.contains('tr', 'Saskatchewan Polytechnic').should('not.exist');

            //verify the other resource is still available

            cy.contains('tr', 'NCTR').should('exist');

            //clicks on French button
            cy.get('button').contains('Learner View').click();
                        cy.get('#toggleLanguage').click();

            cy.contains('tr', 'Polytechnique de la Saskatchewan').should('not.exist');
        });

        //This test will check on the deletion on the last resource
        it('TestDeleteLastResource', () => {

            cy.contains('tr', 'NCTR').within(() => {
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

            // Verify "NCTR" resource is no longer in the list
            cy.contains('tr', 'NCTR').should('not.exist');

            //check there is no delete button

            cy.contains('tr', 'Delete').should('not.exist');

            cy.get('button').contains('Learner View').click();
                        cy.get('#toggleLanguage').click();

            // Verify "NCTR" resource is no longer in the list
            cy.contains('tr', 'NCTR').should('not.exist');

        });

        //This test will check you can add the first resource and the learner view is correct. The french counterpart is also created
        it('TestAddFirstValidResourceInEnglishAndInFrench', () => {

            cy.get('button').contains('Create Resource').click();
            cy.get('#CreateResourceModal').should('be.visible');
            cy.get('#CreateResourceModal').within(() => {
                typeWithRetry('#ResourceTitle', 'NCTR');
                typeWithRetry('#ResourceDescription', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');
                typeWithRetry('#ResourceURL', 'https://nctr.ca/');
                cy.get('button[type="submit"]').contains('Create').click();
            });

            // Ensure the resource has been added successfully
            cy.contains('tr', 'NCTR').should('be.visible');

            // Click the Learner View button to switch views
            cy.get('#toggleLearnerView').click();

            // Ensure the Learner View is visible
            cy.get('.resource-container').should('be.visible');

            // Now, find the container with the "NCTR" resource in the Learner View
            cy.get('.resource-container').contains('NCTR').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'NCTR');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://nctr.ca/');
            });


                        cy.get('#toggleLanguage').click();

            // Now, find the container with the "NCTR" resource in the Learner View
            cy.get('.resource-container').contains('NCTR').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'NCTR');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('contain.text', 'Le NCTR est un lieu d\'apprentissage et de dialogue où les vérités de l\'expérience des pensionnats seront honorées et préservées pour les générations futures.');


                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://nctr.ca/');
            });

        });


        //This test will check that certain fields cant be repeated. 
        //And then it will record a valid resource. to check we can add a value after there is one already
        it('TestUniqueParamMustBeUniqueAndAddASecondResourceInEnglishAndFrench', () => {

            cy.get('button').contains('Create Resource').click();

            cy.get('#CreateResourceModal').should('be.visible');
            cy.get('#CreateResourceModal').within(() => {
                typeWithRetry('#ResourceTitle', 'NCTR');
                typeWithRetry('#ResourceDescription', 'The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations.');
                typeWithRetry('#ResourceURL', 'https://nctr.ca/');
                cy.get('button[type="submit"]').contains('Create').click();
            });


            // Wait for the resource to be processed
            cy.wait(1000);

            cy.get('#CreateResourceModal').within(() => {

                // Extend the timeout and use a regular expression for a flexible match
                cy.get('#ResourceTitleError', { timeout: 1000 }).should('have.text', "The title must be unique");
                cy.get('#ResourceURLError', { timeout: 10000 }).should('have.text', "The URL must be unique");


                typeWithRetry('#ResourceTitle', 'Saskatchewan Polytechnic');
                typeWithRetry('#ResourceDescription', 'Saskpoly Is The Best');
                typeWithRetry('#ResourceURL', 'https://saskpolytech.ca/');
                cy.get('button[type="submit"]').contains('Create').click();

            });


            cy.wait(1000);

            // Check for the presence of CreateResource1 in the list
            cy.contains('NCTR').should('exist');

            // Check for the presence of CreateResource2 in the list
            cy.contains('Saskatchewan Polytechnic').should('exist');

            //user clicks on French



            cy.get('button').contains('Learner View').click();
                        cy.get('#toggleLanguage').click();

            // Check for the presence of CreateResource1 in the list
            cy.contains('NCTR').should('exist');

            // Check for the presence of CreateResource2 in the list
            cy.contains('École polytechnique de la Saskatchewan').should('exist');


        });


        //This test will ensure that editing successfully make changes
        it('TestEditFunctionalityPerformsChangesToTheDatabaseToBothTheEnglishAndFrench', () => {
            // User navigates to the Edit form of the resource
            cy.contains('tr', 'NCTR').within(() => {
                cy.get('button').contains('Edit').click();
            });

            //User edits the resource details with information from EditResource2
            cy.get('#EditResourceModal').within(() => {
                cy.wait(500);
                cy.get('input[name="ResourceTitle"]').clear().type('Indigenous Knowledge Portal');
                cy.get('textarea[name="ResourceDescription"]').clear().type('The Indigenous Knowledge Portal offers a comprehensive database of resources and materials relating to indigenous cultures, languages, and history. It supports educational and research endeavors aimed at fostering a deeper appreciation of Indigenous Peoples\' contributions to society.');
                cy.get('input[name="ResourceURL"]').clear().type('https://indigenousknowledge.example.com/');

                // User saves the changes
                cy.get('button').contains('Save Changes').click();
            });

            //check that in the table the values are correct.

            // Ensure the resource has been added successfully
            cy.contains('tr', 'Indigenous Knowledge Portal').should('be.visible');


            // User navigates to the Learner View to verify the changes
            cy.get('button').contains('Learner View').click();

            // User finds the edited resource in the container and verifies its presence
            cy.contains('.resource-container', 'Indigenous Knowledge Portal').should('be.visible');

            // Now, find the container with the "Indigenous Knowledge Portal" resource in the Learner View
            cy.get('.resource-container').contains('Indigenous Knowledge Portal').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Indigenous Knowledge Portal');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', 'The Indigenous Knowledge Portal offers a comprehensive database of resources and materials relating to indigenous cultures, languages, and history. It supports educational and research endeavors aimed at fostering a deeper appreciation of Indigenous Peoples\' contributions to society.');

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://indigenousknowledge.example.com/');
            });

            //now click french button

                        cy.get('#toggleLanguage').click();

            // Now, find the container with the "Portail des Savoirs Autochtones" resource in the Learner View (The french translation)
            cy.get('.resource-container').contains('Indigenous Knowledge Portal').parents('.card').within(() => {
                // Verify the title
                cy.get('.card-title a').filter(':visible').should('have.text', 'Portail des savoirs autochtones');

                // Verify the description
                cy.get('.card-text').filter(':visible').should('have.text', "Le portail de la connaissance indigène offre une base de données complète de ressources et de documents relatifs aux cultures, aux langues et à l'histoire indigènes. Il soutient les efforts d'éducation et de recherche visant à favoriser une meilleure appréciation des contributions des peuples autochtones à la société.");

                // Verify the href attribute of the link
                cy.get('.card-title a').filter(':visible').should('have.attr', 'href', 'https://indigenousknowledge.example.com/');
            });

        });
    })
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