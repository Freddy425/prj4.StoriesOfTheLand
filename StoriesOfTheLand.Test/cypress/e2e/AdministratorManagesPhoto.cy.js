describe('Story 48 - Admin Manages Photo Submissions', () => {
	let globalURL = 'https://localhost:7202/'
	beforeEach(() => {
		cy.visit(globalURL);
	})

	// Tested
	it('Test that Administrator can view photo submissions and their information', () => {
		// Navigate to the admin console (Need to go directly because NoAuth does no have a navigation item for this)
		cy.visit(globalURL + "Admin/SubmissionIndex");

		// Click the button for 'Learner Photo Submissions'
		//cy.get('[data-cy="learnerSubmissions"]').click();

		// Check that photo submissions are shown
		cy.get('body').should('contain', "Horsetail.png");
	});

	// Tested
	it('Test that denied submission is removed from the list', () => {
		// Navigate to the learner submissions (going directly so avoid the long load time)
		cy.visit(globalURL + "Admin/SubmissionIndex?status=false");

		// Click 'Deny' on the submission for blueberries.png (this is auto-confirmed by Cypress)
		cy.get('[data-cy="deny 1"]').click();

		// Check that blueberries.png is removed from the list
		cy.get('body').contains('.selector', 'Horsetail.png').should('not.exist')

	});

	// Tested
	it('Test that Denied submissions are not displayed on the homepage', () => {
		// Navigate to the learner submissions (going directly so avoid the long load time)
		cy.visit(globalURL);

		// Check that there is NOT an image on the homepage called blueberry.png
		cy.get('[data-cy="Horsetail.png"]').should('not.exist');

	});

	// Tested
	it('Test that approved submissions are displayed on the homepage', () => {
		// Navigate to the learner submissions (going directly so avoid the long load time)
		cy.visit(globalURL + "Admin/SubmissionIndex?status=false");

		// Click 'Approve' on the submission for wildwildMint.png (this is auto-confirmed by Cypress)
		cy.get('[data-cy="approve 2"]').click();

		// Navigate back to homepage
		cy.get('.navbar-brand').click();
		cy.wait(200)

		// Check that there is an image on the homepage called wildwildMint.png
		cy.get('[data-cy="LabTeaLeaves.png"]').should('exist');

	});

	it('Test that all submissions in the pending list can be denied', () => {
		// Navigate to the learner submissions (going directly so avoid the long load time)
		cy.visit(globalURL + "Admin/SubmissionIndex?status=false");

		// Click 'Deny' on the submission for blueberries.png (this is auto-confirmed by Cypress)
		cy.get('[data-cy="deny 3"]').click();

		// Navigate back to homepage
		cy.get('.navbar-brand').click();

		// Check that there is NOT an image on the homepage called blueberry.png
		cy.get('[data-cy="Lungwort.png"]').should('not.exist');

	});


	it('Test that all approved submissions can be removed from the list', () => {
		// Navigate to the learner submissions (going directly so avoid the long load time)
		cy.visit(globalURL + "Admin/SubmissionIndex?status=false");

		// Click the 'Approved' button to switch to approved submissions
		cy.get('[data-cy="approvedSubs"]').click();

		// Click 'Delete' on the submission for for wildwildMint.png (this is auto-confirmed by Cypress)
		cy.get('[data-cy="delete 2"]').click();

		// Check that wildwildMint.png is removed from the list
		cy.get('[data-cy="delete 2"]').should('not.exist')

		// Navigate to the homepage
		cy.get('.navbar-brand').click();

		// Check that there is NOT an image on the homepage called wildwildMint.png
		cy.get('[data-cy="LabTeaLeaves.png"]').should('not.exist');
	});




})