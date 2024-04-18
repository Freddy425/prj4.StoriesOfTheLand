describe('Admin Portal Tests', () => {
    beforeEach(() => {
        // Ignore uncaught exceptions
        Cypress.on('uncaught:exception', (err, runnable) => {
            // returning false here prevents Cypress from failing the test
            return false;
        });
    });

/*    
    * Test the total amount of feedback for the specimen
    */
    it('Test That Overview button updates page with information', () => {
        // Navigate to the Home Page
        cy.visit('https://localhost:7202/admin/portal');

        // Check to see that it redirected successfully
        cy.url().should('eq', 'https://localhost:7202/admin/portal');

        cy.contains("Tech Overview").click();
        cy.get('#row1Box1Title').should('have.text', 'Screen Resolution Information');
        cy.contains("Overview").click();

        // Check to see that the text "Total" appears on the page
        cy.get('#row1Box1Title').should('have.text', 'Amount of Feedback');
        cy.get('#row1Box2Title').should('have.text', 'Total Page Views');
        cy.get('#row1Box3Title').should('have.text', 'User Device Usage');
        cy.get('#row2Box1Title').should('have.text', 'User Engagement Over Time');

        cy.get('#feedbackstats').should('be.visible');
        cy.get('#pageViewsList').should('be.visible');
        cy.get('svg[aria-label="A chart."]').should('be.visible');
        cy.get('svg[aria-label="A chart."] > rect').should('be.visible');
    });


    it('Test That Overview button updates page with information', () => {
        // Navigate to the Home Page
        cy.visit('https://localhost:7202/admin/portal');
        cy.viewport(1000,1000)
        // Check to see that it redirected successfully
        cy.url().should('eq', 'https://localhost:7202/admin/portal');

        cy.contains("Overview").click();
        cy.get('#row1Box1Title').should('have.text', 'Amount of Feedback');
        cy.contains("Tech Overview").click();

        // Check to see that the text "Total" appears on the page
        cy.get('#row1Box1Title').should('have.text', 'Screen Resolution Information');
        cy.get('#row1Box2Title').should('have.text', 'User Platform Usage');
        cy.get('#row1Box3Title').should('have.text', 'User Browser Usage');
        cy.get('#row2Box1Title').should('have.text', 'User Operating Systems');

        cy.contains('p', '1080').should('be.visible');
        //cy.get('svg[aria-label="A chart."][width="179"][height="200"]').should('be.visible');
        cy.get('svg')
            .eq(0)
            .should('have.attr', 'width', '134')
            .should('have.attr', 'height', '168')
            .should('be.visible');

        cy.get('svg')
            .eq(1)
            .should('have.attr', 'width', '166')
            .should('have.attr', 'height', '202')
            .should('be.visible');

        cy.get('svg')
            .eq(2)
            .should('have.attr', 'width', '644')
            .should('have.attr', 'height', '200')
            .should('be.visible');
    
    });
    it('Test That Real Time button updates page with information', () => {
        // Navigate to the Home Page
        cy.visit('https://localhost:7202/admin/portal');
        cy.viewport(1000, 1000)
        // Check to see that it redirected successfully
        cy.url().should('eq', 'https://localhost:7202/admin/portal');

        cy.contains("Overview").click();
        cy.get('#row1Box1Title').should('have.text', 'Amount of Feedback');
        cy.contains("Real Time").click();

        // Check to see that the text "Total" appears on the page
        cy.get('#row1Box1Title').should('have.text', 'Active Users');
        cy.get('#row1Box2Title').should('have.text', 'Active Cities');
        cy.get('#row1Box3Title').should('have.text', 'Event Count Last 30 Minutes');
        cy.get('#row2Box1Title').should('have.text', 'Users Over the Last Week');   
        cy.get('svg')
            .eq(0)
            .should('have.attr', 'width', '644')
            .should('have.attr', 'height', '200')
            .should('be.visible');
    });

    it('Test That Feedback Button Redirects Users to Feedback Page ', () => {
        cy.visit('https://localhost:7202/admin/portal');
        cy.viewport(1000, 1000);
        cy.url().should('eq', 'https://localhost:7202/admin/portal');
        cy.get('button').eq(1).click();
        cy.url().should('eq', 'https://localhost:7202/Feedback/Index');
    });
    
    it('Test That Feedback Combo Box Updates Values ', () => {
        cy.visit('https://localhost:7202/admin/portal');
        // Check to see all time feedback
        cy.get('#row1Box1Title').should('have.text', 'Amount of Feedback');
        cy.get('#totalfeedback').should('have.text', 'Total: 11');
        cy.get('#totalNewfeedback').should('have.text', 'New: 10');
        cy.get('#totalResolvedfeedback').should('have.text', 'Resolved: 0');
        cy.get('#totalPendingResponsefeedback').should('have.text', 'Pending Response: 1');
        cy.get('#totalInProgressfeedback').should('have.text', 'In Progress: 0');
        // Check to see last 24 hours
        cy.get('#feedbackstats').select('Last 24 Hours');
        cy.get('#totalfeedback').should('have.text', 'Total: 3');
        cy.get('#totalNewfeedback').should('have.text', 'New: 2');
        cy.get('#totalPendingResponsefeedback').should('have.text', 'Pending Response: 1');
        cy.get('#totalResolvedfeedback').should('have.text', 'Resolved: 0');
        cy.get('#totalInProgressfeedback').should('have.text', 'In Progress: 0');
        // Check to see last week
        cy.get('#feedbackstats').select('Last Week');
        cy.get('#totalfeedback').should('have.text', 'Total: 5');
        cy.get('#totalNewfeedback').should('have.text', 'New: 4');
        cy.get('#totalPendingResponsefeedback').should('have.text', 'Pending Response: 1');
        cy.get('#totalResolvedfeedback').should('have.text', 'Resolved: 0');
        cy.get('#totalInProgressfeedback').should('have.text', 'In Progress: 0');
        // Check to see last month
        cy.get('#feedbackstats').select('Last Month');
        cy.get('#totalfeedback').should('have.text', 'Total: 6');
        cy.get('#totalNewfeedback').should('have.text', 'New: 5');
        cy.get('#totalPendingResponsefeedback').should('have.text', 'Pending Response: 1');
        cy.get('#totalResolvedfeedback').should('have.text', 'Resolved: 0');
        cy.get('#totalInProgressfeedback').should('have.text', 'In Progress: 0');
    });
    it('Test That No Resolved Feedback All Time Appears', () => {
        cy.visit('https://localhost:7202/admin/portal');
        cy.get('#totalResolvedfeedback').should('have.text', 'Resolved: 0');
    });
    it('Test That 1 Pending Response Feedback All Time Appears', () => {
        cy.visit('https://localhost:7202/admin/portal');
        cy.get('#totalPendingResponsefeedback').should('have.text', 'Pending Response: 1');
    });

})
