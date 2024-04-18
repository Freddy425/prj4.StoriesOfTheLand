describe('Test Map Page for Stories of the Land Application', () => {
    let globalURL = 'https://localhost:7202'
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.visit(globalURL + '/Specimen/Map')
    })

    context('TestAllMobileTests', () => {

        it('Test That the map button redirects the user to the map page', () => {
            cy.viewport('iphone-x');
            //Click clicks the navbar item with the ID of mapnav
            cy.get('#mobilenav').click();
            cy.get('#mapnav').click();
            cy.get('#map').should('exist');
            cy.get('#map').children('div').should('exist');
        })

        it('mapsizemobile', () => {
            cy.viewport('iphone-8')
            cy.get('#map').should('exist')
            cy.get('#map').then(el => el[0].getBoundingClientRect()).should('have.property', 'width', 290)
            cy.get('#map').then(el => el[0].getBoundingClientRect()).should('have.property', 'height', 500)
        })

        it('TestThatPlotPointisAboveAndToTheRightOfAnotherPlotPointForPrecisionOnMobile  ', () => {
            // Check if map exist before starting
            cy.viewport('iphone-x')
            // Point 1
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1)
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 136, 231)');
            // Point 2
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(4)
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 109, 254)');
        })

        // Info bubble tests

        it('Test that Info Bubble shows correct text and image and can be closed', () => {
            cy.viewport('iphone-x');

            // Check if the map exists
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > canvas:nth-child(1)').should('exist');

            // Click on the pin for Labrador Tea
            cy.get('.H_zoom').click();
            cy.get('.H_zoom').click();
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > svg:nth-child(7)').click();

            // Check that there is an image on the info bubble
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > svg:nth-child(7)').should('exist');
            cy.get('[data-cy="specImg"]').should('exist');

            // Check that the image on the info bubble is the FIRST image for labrador tea
            cy.get('[data-cy="specImg"]').should('have.attr', 'src').should('include', 'LabradorTea.png')

            // Click on the 'X' button on the info bubble
            cy.get('.H_ib_close > svg:nth-child(1)').click();

            // Check that the info bubble is closed
            cy.get('.H_ib').should('have.css', 'display', 'none')

            // Check that specimen with no image displays placeholder
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > svg:nth-child(8)').click();
            cy.get('[data-cy="specPlc"]').should('have.attr', 'src').should('include', 'placeholder')

        })

    })

    context('AllDesktoptests', () => {
        it('Test That the map button redirects the user to the map page', () => {
            //Click clicks the navbar item with the ID of mapnav
            cy.visit(globalURL);
            cy.get('#mapnav').click();
            cy.get('#map').should('exist');
            cy.get('#map').children('div').should('exist');
        })
        // This tests that the user can zoom out on desktop view 
        it('TestThatUserCanZoomOutOfMapDesktop', () => {

            // Before clicking, ensure that the map exists. 
            cy.get('#map').should('exist');
            // Double click several times to zoom in on the map
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            // Test to ensure that the right amount of values are there for g elements
            //cy.get('#map').find('g').should('have.length', 10);
            // Now, zoom out 
            cy.get('#map').rightclick(); cy.get('#map').rightclick(); cy.get('#map').rightclick(); cy.get('#map').rightclick(); cy.get('#map').rightclick(); cy.get('#map').rightclick(); cy.get('#map').rightclick(); cy.get('#map').rightclick();
            // now that there are 3 more images, the total should be eight since there are two other images on the canvas being the HERE logo and terms and conditions 
            //cy.get('#map').find('g').should('have.length', 14);
        });

        // This test is for zoom in on desktop view
        it('TestThatUserCanZoomInonMapDesktop', () => {
            // Before clicking, ensure that the map exists. 
            cy.get('#map').should('exist');
            // Double click several times to zoom in on the map
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            cy.get('#map').dblclick();
            // There is a G element inside of each SVG
            // This includes the here logo in the bottom right corner AND the terms of use image in the 
            // bottom right corner. 
            //cy.get('#map').find('g').should('have.length', 10);
        });
        it('mapsize', () => {


            cy.get('#map').should('exist')
            cy.get('#map').then(el => el[0].getBoundingClientRect()).should('have.property', 'width', 444)
            cy.get('#map').then(el => el[0].getBoundingClientRect()).should('have.property', 'height', 500)

        })
        //testing that map div exist
        it('TestThatSpecimenMapExists', () => {

            // Checking if map exist
            cy.get('#map').should('exist');
        })
        it('TestThatPlotPointisAboveAndToTheRightOfAnotherPlotPointForPrecision  ', () => {
            // Check if map exist before starting
            cy.viewport(1000, 1000)
            // Point 1
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1)
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');
            // Point 2
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(4)
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 186, 254)');
        });
        it('TestThatMapContainsPointOfSpecimen ', () => {
            // Check if map exist before starting
            cy.viewport(1000, 1000)
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1)
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 213, 231)');
        })

        // Info bubble tests

        it('Test that Info Bubble shows correct text and image and can be closed', () => {

            // Check if the map exists
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > canvas:nth-child(1)').should('exist');

            // Click on the pin for Labrador Tea
            cy.get('.H_zoom').click();
            cy.get('.H_zoom').click();
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > svg:nth-child(7)').click();

            // Check that there is an image on the info bubble
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > svg:nth-child(7)').should('exist');
            cy.get('[data-cy="specImg"]').should('exist');

            // Check that the image on the info bubble is the FIRST image for labrador tea
            cy.get('[data-cy="specImg"]').should('have.attr', 'src').should('include', 'LabradorTea.png')

            // Click on the 'X' button on the info bubble
            cy.get('.H_ib_close > svg:nth-child(1)').click();

            // Check that the info bubble is closed
            cy.get('.H_ib').should('have.css', 'display', 'none')

            // Check that specimen with no image displays placeholder
            cy.get('#map > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > svg:nth-child(8)').click();
            cy.get('[data-cy="specPlc"]').should('have.attr', 'src').should('include', 'placeholder')

        })
    })
})
