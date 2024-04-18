describe('template spec', () => {
    let globalURL = 'https://localhost:7202/'

    Cypress.Commands.add('wasCalled', (stubOrSpy) => {
            expect(stubOrSpy).to.be.called;
    })

    // Test print single QR codes on specimen page

    it('Test print dialogue', () => {
        cy.visit(globalURL + 'specimen/Details/1')
        //Test Items only for print arent visible.
        cy.get('#firstImage').should('not.be.visible')
        cy.get('#scanMe').should('not.be.visible')

        //hidden in qr code div
        cy.get('#printBtn').should('not.be.visible')

        //open up qr code
        cy.get('#qrCollapseBtn').click()

        //Test Print Button
        cy.get('#printBtn').should('be.visible')
        let printStub

        //Test that the print dialogue is opened
        cy.window().then(win => {
            printStub = cy.stub(win, 'print')

            cy.get('#printBtn').click();
            cy.wasCalled(printStub)
        })
    })

    it('Test items included in print', () => {
        //Test Items included in Print
        cy.visit(globalURL + 'specimen/Details/1')
        cy.get('#englishName').should('have.class', 'd-print-block')
        cy.get('#creeName').should('have.class', 'd-print-block')
        cy.get('#firstImage').should('have.class', 'd-none d-print-flex')
        cy.get('#qrCollapse').should('have.class', 'd-print-flex')


        //Test Items not included in print
        cy.get('#latinName').should('have.class', 'd-print-none')
        cy.get('#specimenDesc').should('have.class', 'd-print-none')
        cy.get('#culturalSignif').should('have.class', 'd-print-none')
        cy.get('#printBtnDiv').should('have.class', 'd-print-none')
        cy.get('#qrCollapseToggle').should('have.class', 'd-print-none')
        cy.get('#navDiv').should('have.class', 'd-print-none')
        cy.get('#imageCarousel').should('have.class', 'd-print-none')

        //Test no Image no firstImage
        //Future Testing will require a specimen without an image.
        cy.visit(globalURL + 'specimen/Details/10')
        cy.get('#firstImage').should('not.exist');
    })

    //Test Print all QR Codes

    //test print all dialogue
    it('Test print all dialogue', () => {
        cy.visit(globalURL + 'Admin/printAll')
        //Test Items only for print arent visible.
        cy.get('#firstImage').should('not.be.visible')
        cy.get('#scanMe').should('not.be.visible')

        //Test Print Button
        cy.get('#printBtn').should('be.visible')
        let printStub

        //Test that the print dialogue is opened
        cy.window().then(win => {
            printStub = cy.stub(win, 'print')

            cy.get('#printBtn').click();
            cy.wasCalled(printStub)
        })
    })



    //Test print all QR codes
    it('Test print all qr codes',() => {
        cy.visit(globalURL + 'Admin/PrintAll')
        cy.get('#qrCollapse').filter('.d-print-flex').should('have.length', 1);
        cy.get('div[style="break-after:page"]').should('have.length', 13);
        cy.get('#englishName.d-print-block').should('have.length', 13);
        cy.get('#creeName.d-print-block').should('have.length', 11);
        cy.get('#firstImage.h-40.d-flex.align-items-center.justify-content-center.d-none.d-print-flex').should('have.length', 6);

        //Test Items not included in print
        cy.get('#latinName').should('have.class', 'd-print-none')
        cy.get('#specimenDesc').should('have.class', 'd-print-none')
        cy.get('#culturalSignif').should('have.class', 'd-print-none')
        cy.get('#printBtnDiv').should('have.class', 'd-print-none') 
        cy.get('#qrCollapseToggle').should('have.class', 'd-print-none')
        cy.get('#navDiv').should('have.class', 'd-print-none')
        cy.get('#imageCarousel').should('have.class', 'd-print-none')
    })


    //Test print one QR codes
    it('Test print one qr codes', () => {
        cy.visit(globalURL + 'Admin/PrintAll/1')
        cy.get('div[style="break-after:page"]').should('have.length', 1);
        cy.get('#englishName.d-print-block').should('have.length', 1);
        cy.get('#creeName.d-print-block').should('have.length', 1);
        cy.get('#firstImage.h-40.d-flex.align-items-center.justify-content-center.d-none.d-print-flex').should('have.length', 1);

        //Test Items not included in print
        cy.get('#latinName').should('have.class', 'd-print-none')
        cy.get('#specimenDesc').should('have.class', 'd-print-none')
        cy.get('#culturalSignif').should('have.class', 'd-print-none')
        cy.get('#printBtnDiv').should('have.class', 'd-print-none')
        cy.get('#qrCollapseToggle').should('have.class', 'd-print-none')
        cy.get('#navDiv').should('have.class', 'd-print-none')
        cy.get('#imageCarousel').should('have.class', 'd-print-none')
    })


    //Test print zero QR codes
    it('Test print zero qr codes', () => {
        cy.visit(globalURL + 'Admin/PrintAll/0')
        cy.get('div[style="break-after:page"]').should('have.length', 0);
        cy.get('#englishName.d-print-block').should('have.length', 0);
        cy.get('#creeName.d-print-block').should('have.length', 0);
        cy.get('#firstImage.h-40.d-flex.align-items-center.justify-content-center.d-none.d-print-flex').should('have.length', 0);

        cy.contains("No Specimen to print");
        
    })
}) 