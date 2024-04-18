describe('Story 49 - Learner Submits Photo', () => {
    let url = 'https://localhost:7202/'
  it('Test That Submit Your Photo Button Makes Modal Appear', () => {
      // Navigate to the home page
      cy.visit(url);
      cy.wait(1000);
      // Click on the Submit Your Photo button
      cy.get('[data-cy="submitPhotoHomePage"]').click();
      cy.wait(1000);
      // Check to see that the modal appears
      cy.get('#mediaModal').should('be.visible');
      cy.wait(1000);
  })

  it('Test That Image Uploaded as Max Size Can Upload', () => {
      cy.visit(url);
      // Click on the Submit Your Photo button
      cy.get('[data-cy="submitPhotoHomePage"]').click();
      cy.wait(500);
      // Check to see that the modal appears
      cy.get('#mediaModal').should('be.visible');
      // Add a file that is the max size
      cy.get('#file-input').attachFile('Edge Case.png');
      
      // Click the submit button
      cy.get('button.btn.btn-secondary[type="submit"]').click();
      // Check to see that the image uploaded
      cy.get('#success').should('contain', "Your image was uploaded successfully")
  })
    it('Test That Image Just Below Max Size Can Upload', () => {
      cy.visit(url);
      // Click on the Submit Your Photo button
        cy.get('[data-cy="submitPhotoHomePage"]').click();
        cy.wait(500);
      // Check to see that the modal appears
      cy.get('#mediaModal').should('be.visible');
      // Add a file that is just below the max size
      cy.get('#file-input').attachFile('Lower Boundary.png');
      // Click the submit button
      cy.get('button.btn.btn-secondary[type="submit"]').click();
      // Check to see that the image uploaded
      cy.get('#success').should('contain', "Your image was uploaded successfully")
  })
    it('Test That Image Just Above Max Size Cannot Upload', () => {
        cy.visit(url);
      // Click on the Submit Your Photo button
        cy.get('[data-cy="submitPhotoHomePage"]').click();
        cy.wait(500);
      // Check to see that the modal appears
      cy.get('#mediaModal').should('be.visible');
      // Add a file that is just above the max size
      cy.get('#file-input').attachFile('Upper Boundary.png');
      // Click the submit button
      cy.get('button.btn.btn-secondary[type="submit"]').click();
      // Check for the error message
        cy.get('#LearnerImageError').should('contain', 'The file size should not exceed 1024 KB.')
  })

    it('Test That Cancels Button cancels', () => {
        cy.visit(url);
        // Click on the Submit Your Photo button
        cy.get('[data-cy="submitPhotoHomePage"]').click();
        cy.wait(500);
        // Check to see that the modal appears
        cy.get('#mediaModal').should('be.visible');
        // Click the cancel button
        cy.get('#cancel').click();
        // Check to see that the modal disappears
        cy.get('#mediaModal').should('be.visible');
  })
    it('Test That Images That Are an Invalid Format Will Not Upload and error messages appears', () => {
       cy.visit(url);
      // Click on the Submit Your Photo button
        cy.get('[data-cy="submitPhotoHomePage"]').click();
        cy.wait(500);
      // Check to see that the modal appears
      cy.get('#mediaModal').should('be.visible');
      // Add a file that is an invalid format
      cy.get('#file-input').attachFile('example.json');

      // Click the submit button
      cy.get('button.btn.btn-secondary[type="submit"]').click();
      // Check for the error message
        cy.get('#LearnerImageError').should('contain', 'File must be of type png, jpg or jpeg')
  })

})