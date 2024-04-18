describe('Test Feedback Form page for Stories of the Land', () => {

  let localhost = 'https://localhost:7202/'

  // Set default timeout to WAYYY lower
  Cypress.config('defaultCommandTimeout', 100);

  it('check valid feedback can be submitted', () => {
    cy.viewport('iphone-6');
    cy.visit(localhost)
    cy.get('.navbar-toggler').click();
    cy.get('li.nav-item:nth-child(2) > a:nth-child(1)').click();
      cy.contains('a', 'Velvet Leaf Blueberry').click()
    cy.get('.btn-link').click();
    cy.get('form').should('exist');
    cy.get('div.form-group:nth-child(1) > label:nth-child(1)').type('Testy McTesterson');
    cy.get('div.form-group:nth-child(2) > label:nth-child(1)').type('TestyM@gmail.com');
    cy.get('div.form-group:nth-child(3) > label:nth-child(1)').type('Feedback about Wild Mint specimen');
    cy.get('div.form-group:nth-child(5) > label:nth-child(1)').type('This is just a test of the details field in the Feedback object.');
    cy.get('input.btn.btn-primary').click();
    cy.url().should('include', '/Specimen/Details/1'); // This will probably have to change to reflect a successful submit
    cy.get('.alert').should('contain.text', 'Success! Thank you for submitting feedback.');
  })

  it('check name field shows an error message on invalid input ', () => {
    cy.viewport('iphone-6');
    cy.visit(localhost)
    cy.get('.navbar-toggler').click();
    cy.get('li.nav-item:nth-child(2) > a:nth-child(1)').click();
      cy.contains('a', 'Velvet Leaf Blueberry').click()
    cy.get('.btn-link').click();
    cy.get('form').should('exist');
    cy.get('div.form-group:nth-child(2) > label:nth-child(1)').type('TestyM@gmail.com');
    cy.get('div.form-group:nth-child(3) > label:nth-child(1)').type('Feedback about Wild Mint specimen');
    cy.get('div.form-group:nth-child(5) > label:nth-child(1)').type('This is just a test of the details field in the Feedback object.');
    cy.get('input.btn.btn-primary').click();
    cy.get('.field-validation-error').should('have.text', 'Please leave a name');
  })


  it('check mail field shows an error message on invalid input ', () => {
    cy.viewport('iphone-6');
    cy.visit(localhost)
    cy.get('.navbar-toggler').click();
    cy.get('li.nav-item:nth-child(2) > a:nth-child(1)').click();
      cy.contains('a', 'Velvet Leaf Blueberry').click()
    cy.get('.btn-link').click();
    cy.get('form').should('exist');
    cy.get('div.form-group:nth-child(1) > label:nth-child(1)').type('Testy McTesterson');
    cy.get('div.form-group:nth-child(2) > label:nth-child(1)').type('Facebook.com');
    cy.get('div.form-group:nth-child(3) > label:nth-child(1)').type('Feedback about Wild Mint specimen');
    cy.get('div.form-group:nth-child(5) > label:nth-child(1)').type('This is just a test of the details field in the Feedback object.');
    cy.get('input.btn.btn-primary').click();
    cy.url().should('include', '/Feedback/Create');
  })

  it('check details field shows an error message on invalid input ', () => {
    cy.viewport('iphone-6');
    cy.visit(localhost)
    cy.get('.navbar-toggler').click();
    cy.get('li.nav-item:nth-child(2) > a:nth-child(1)').click();
      cy.contains('a', 'Velvet Leaf Blueberry').click()
    cy.get('.btn-link').click();
    cy.get('form').should('exist');
    cy.get('div.form-group:nth-child(1) > label:nth-child(1)').type('Testy McTesterson');
    cy.get('div.form-group:nth-child(2) > label:nth-child(1)').type('TestyM@gmail.com');
    cy.get('div.form-group:nth-child(3) > label:nth-child(1)').type('Feedback about Wild Mint specimen');
    cy.get('div.form-group:nth-child(5) > label:nth-child(1)').type('CCCCCCCCCCCCCCCCCCCCCCCCCCCCC');
    cy.get('input.btn.btn-primary').click();
    cy.get('.field-validation-error').should('have.text', 'Details length must be between 30 and 2000');
  })

  it('check navigation menu can be opened from the feedback form ', () => {
    cy.viewport('iphone-6');
    cy.visit(localhost)
    cy.get('.navbar-toggler').click();
    cy.get('li.nav-item:nth-child(2) > a:nth-child(1)').click();
      cy.contains('a', 'Velvet Leaf Blueberry').click()
    cy.get('.btn-link').click();
    cy.get('form').should('exist');
    cy.get('.navbar-toggler').click();
    cy.get('li.nav-item:nth-child(1) > a:nth-child(1)').click();
  })

})