describe('template spec', () => {
    let globalURL = 'https://localhost:7202'
    beforeEach(() => {

        cy.visit(globalURL + '/Specimen/Details/1')
    })

    it('test modal appear and dissapears when clicking cancel as well as testing hamburger helper', () => {

        //hamgburger heloper should not be visible on regular size
        cy.get('button.btn.btn-primary').click();

        //modal should be visible
        cy.get('#mediaModal').should('be.visible');

        //click cacnel button to close the modal
        cy.get('button.btn.btn-secondary[type="submit"]').click();

        //check to make sure the modal is not visible
        cy.get('#mediaModal').should('not.be.visible');

        //test the hamburger helper appears when the screen is made smaller

        //hamgburger heloper should not be visible on regular size
        cy.get('span.navbar-toggler-icon').should('not.be.visible');

        // Adjust the screen dimensions
        cy.viewport(500, 600);

        // Hambuger helper should be vivible
        cy.get('span.navbar-toggler-icon').should('be.visible');

    });

    it('tries to upload an invalid file', () => {

        cy.get('button.btn.btn-primary').click();

        //modal should be visible
        cy.get('#mediaModal').should('be.visible');

        //Click the save button with no file entered
        cy.get('button.btn.btn-secondary[type="submit"]').click();

        cy.wait(1000);

        //error message appears when you dont enter a file
        cy.contains("File is required to add media")

        //select the invalid file and attatch it the the file input
        cy.get('#file-input').attachFile('example.json');

        //Click the save button
        cy.get('button.btn.btn-secondary[type="submit"]').click();

        cy.wait(1000);

        cy.contains('File must be of type png, jpeg, m4a, mp3')
    });


    it('Sucessfully uploads an audiofile', () => {

        cy.get('.Audio').should('have.length', 1);

        cy.get('button.btn.btn-primary').click();

        //modal should be visible
        cy.get('#mediaModal').should('be.visible');

        //select the invalid file and attatch it the the file input
        cy.get('#file-input')
            .attachFile(['Mint.m4a',]);

        //Click the save button
        cy.get('button.btn.btn-secondary[type="submit"]').click();

        cy.wait(1000);

        cy.get('.Audio').should('have.length', 2);

        cy.get('#mediaModal').should('not.be.visible');

    });

    it('Sucessfully uploads multiple files(one image one audio)', () => {

        cy.get('.carousel-item').should('have.length', 2);

        cy.get('button.btn.btn-primary').click();

        //modal should be visible
        cy.get('#mediaModal').should('be.visible');

        //select the invalid file and attatch it the the file input
        cy.get('#file-input')
            .attachFile(['LabTeaLeaves.png', 'LabTeaPlants.png']);

        //Click the save button
        cy.get('button.btn.btn-secondary[type="submit"]').click();

        cy.wait(1000);
        //check to make sure there is 4 images after adding images
        cy.get('.carousel-item').should('have.length', 4);

        cy.get('#mediaModal').should('not.be.visible');


    });

})