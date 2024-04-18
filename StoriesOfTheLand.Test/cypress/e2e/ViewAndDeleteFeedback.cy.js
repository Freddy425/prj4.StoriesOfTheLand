describe('template spec', () => {
    let globalURL = 'https://localhost:7202'
  

    it('Test going to feedback page and all values and buttons shows', () => {
        cy.visit(globalURL + '/Feedback/Index');
        

        //checks all the values that should be on the page are on the page
        cy.get('Button.accordion-button').eq(0).contains("Feb.-13-2024 Wild Mint Billy Bob Specimen page is missing an image")
        cy.get('Button.accordion-button').eq(1).contains("Feb.-11-2024 Velvet Leaf Blueberry Sally Joe Misspelled name on page")
        cy.get('Button.accordion-button').eq(2).contains("Apr.-02-2023 null Jonh Smith Error on a page")

        //check delete button should be visible
        cy.get('.d-flex button.btn-danger').should('be.visible');

        //check repsond button is visible
        cy.get('.d-flex button.btn-primary').should('be.visible');
    });

    it('Test changing status of feeback entry to "Resolved"', () => {
        cy.visit(globalURL + '/Feedback/Index');
        cy.viewport(1200, 860);
        //opens all the accordians
        cy.get('Button.accordion-button').eq(1).click()
        cy.get('Button.accordion-button').eq(2).click()
        cy.get('Button.accordion-button').eq(0).click()

        cy.window().scrollTo('bottom');
        //set the status of the last feebdack to resolved
        cy.get('.status-dropdown').eq(0).select('2');
        //click the save button to reload the page
        cy.get('.btn.btn-primary').eq(2).click();


    });

   

    it('Test delete a feedback entry', () => {
        cy.visit(globalURL + '/Feedback/Index');
        //checks that there are currently 5 feedback entries on the page
        cy.get('.accordion-item').should('have.length', 11);

        //clicks the delete button on the first feedback
       cy.get('.btn.btn-danger').first().invoke('attr', 'data-bs-target').then((target) => {
            cy.get('.btn.btn-danger').first().click()
            //checks to make sure the delte confirmation modal pops up   
            cy.get(target).should('be.visible');

            //clicks the delete confirm button
            cy.get(target + ' button[type="submit"]').first().click();

            
           //after pressing the delete button the modal should not be visible 
           //cy.get(target).should('not.be.visible');

           //now the feedback list should be one less
           cy.get(target).should('not.exist');
       })



       
    });

    it('Test changing status of feeback entry to "Resolved"', () => {
        cy.visit(globalURL + '/Feedback/Index');
        cy.viewport(1200, 860);

        cy.get('Button.accordion-button').eq(0).click()

        //checks to see that the value of the combo box is currently "New"
        cy.get('.status-dropdown').eq(0).invoke('val').should('eq', '0');

        //click on the combo box
        cy.get('.status-dropdown').eq(0).select('2');
        cy.get('.btn.btn-primary').eq(2).click();

        //check to see that the opions contain "Resolved" and clikc on that
        cy.get('.status-dropdown').eq(0).invoke('val').should('eq', '2');

        //reload the page and check to make sure its still resovled
    });
    


    
})