describe('Story 53 - Tests for Index & Map', () => {
    let globalURL = 'https://localhost:7202'
    context('Test Specimen Map', () => {
        it('TestThatMapWidthAndHeightAreRealitveOnMobileView', () => { 
        
            cy.visit(globalURL + '/Specimen/Map');
            cy.viewport('iphone-x');
            // Make sure that the width and height of the map are equal to specific values on 
            // mobile view
            cy.get('#map')
                .find('div')
                .find('div')
                .find('div')
                .find('div')
                .find('svg')
                .eq(1)
                .should('have.css', 'transform', 'matrix(1, 0, 0, 1, 136, 231)');
        })
        it('TestThatMapFunctionalityStillWorksOnMobile', () => {
                cy.visit(globalURL + '/Specimen/Map');
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


            });

    })
    context('Test Specimen Index', () => {
        it('TestThatIndexisresponsiveForMobileDevices', () => {
            cy.visit(globalURL + '/Specimen/');
            cy.get('.table').then(($table) => {
                const width = $table.width();
                const height = $table.height();
                // Check if width and height match specific values
                expect(width).to.equal(936); 
                expect(height).to.equal(725);
            });
            cy.viewport('iphone-x');
            cy.get('.table').then(($table) => {
                const width = $table.width();
                const height = $table.height();
                // Check if width and height match specific values after viewport change
                expect(width).to.equal(415.797);
                expect(height).to.equal(1145.38); 
            });
        });


        it('TestThatIndexSortsAlphabeticallyByEnglishName', () => {
        
            cy.visit(globalURL + '/Specimen');
            cy.get('#enSortAlpha').click();
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Common Yarrow'); 
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Achillea millefolium.'); 
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('wāpanīwask'); 
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Wild Rose');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Rosa Species');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('okinīwāhtik');
            });
        });
        it('TestThatIndexSortsAlphabeticallyByLatinName', () => {

            cy.visit(globalURL + '/Specimen');
            cy.get('#latinSortAlpha').click();
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Common Yarrow');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Achillea millefolium.');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('wāpanīwask');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Velvet Leaf Blueberry');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Vaccinium myrtilloides');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Idinimin');
            });
        });
        it('TestThatIndexSortsAlphabeticallyByCreeName', () => {

            cy.visit(globalURL + '/Specimen');
            cy.get('#creeSortAlpha').click();
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Horsetail');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Equisetum species');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('-');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Paper Birch');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Betula papyrifera.');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Waskway');
            });
        });
        it('TestThatIndexSortsReverseAlphabeticallyByEnglishName', () => {
            cy.visit(globalURL + '/Specimen');
            cy.get('#enSortReverse').click();
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Velvet Leaf Blueberry');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Myrtille à feuilles de velours');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Vaccinium myrtilloides');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Hair Lichen');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('-');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('NA.');
            });

        });
        it('TestThatIndexSortsReverseAlphabeticallyByLatinName', () => {

            cy.visit(globalURL + '/Specimen');
            cy.get('#latinSortReverse').click();
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Velvet Leaf Blueberry');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Vaccinium myrtilloides');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Idinimin');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Horsetail');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Equisetum species');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('-');
            });
        }); 
        it('TestThatIndexSortsReverseAlphabeticallyByCreeName', () => {

            cy.visit(globalURL + '/Specimen');
            cy.get('#creeSortAlpha').click();
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Horsetail');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Equisetum species');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('-');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Paper Birch');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Betula papyrifera.');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Waskway');
            });

        });
        it('TestThatYouCanStillSortInFrench', () => {

            cy.visit(globalURL + '/Specimen');
            cy.get('#toggleLanguage').click();
            cy.get('#creeSortAlpha').click();
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Prêle');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Equisetum species');
            });
            cy.get('#bodyOfTable > tr:nth-child(1) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('-');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(1) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('-');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(2) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Betula papyrifera.');
            });
            cy.get('#bodyOfTable > tr:nth-child(13) > th:nth-child(3) > a').invoke('text').then((text) => {
                expect(text.trim()).to.equal('Waskway');
            });
        });



        it('TestThatSearchFunctionWorksOnDesktopAndMobile', () => {
            cy.visit(globalURL + '/Specimen');

            // Type 'Wild Mint' in the search box
            cy.get('#searchContent').type('Wild Mint');

            // Click the search button
            cy.get('#searchBtn').click();

            // Check if there's one element in the tbody
            cy.get('#bodyOfTable > tr').should('have.length', 2);
        });
    })
    context('Test Various UI Changes', () => {
        it('Test That Changes to UI Were Made Succesfully', () => {

            cy.visit(globalURL);
            // Make the changes to the sponsor on mobile at the bottom of the page
            cy.viewport('iphone-x');
            cy.get('#footer > div > div > div:nth-child(1) > img').invoke('css', 'width').then((value) => {
                expect(value).to.equal('130px'); // Adjust the expected width value as needed
            });
            cy.get('#footer > div > div > div:nth-child(1) > img').invoke('css', 'height').then((value) => {
                expect(value).to.equal('67.1562px'); // Adjust the expected width value as needed
            });
        });

    })
})
