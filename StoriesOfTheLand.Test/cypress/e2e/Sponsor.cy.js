describe('template spec', () => {
    let globalURL = 'https://localhost:7202'

    beforeEach(() => {

        cy.visit(globalURL + '/Specimen');
    })


    it('testAddSponsor', () => {

        cy.visit('https://localhost:7202/Sponsor')
        cy.wait(100);
        cy.get('#createSponsor').click()
        cy.get("#SponsorModal").should("be.visible")
        // testing cancel doesn't create anything'
        cy.get('#SponsorName').type('Google')
        cy.wait(100)
        cy.get('#SponsorURL').type('https://www.google.com')



        cy.wait(100)

        // testing cancel doesn't create anything'
        cy.get('#cancelButton').click();
        cy.wait(100)
        cy.get("#SponsorModal").should('not.be.visible');
        cy.wait(100)
        cy.get("body > div > main > table > tbody > tr:nth-child(3) > td:nth-child(2) > a")
            .invoke('attr', 'href')
            .then(href => {
                cy.visit(href);
            });

        cy.wait(100)
        cy.url(' 	https://www.pagc.sk.ca')
    })



    it('testaddingsponsor', () => {


        cy.visit('https://localhost:7202/Sponsor')

        cy.get('#createSponsor').click()
        cy.get("#SponsorModal").should("be.visible")
        cy.wait(100)
        cy.get('#SponsorName').type('Google')
        cy.wait(200)
        cy.get('#SponsorURL').type('htps://www.google.com')
        cy.wait(200)
        cy.get('#saveSponsorButton').click()
        cy.wait(100)
        cy.get('#URLError').should('contain.text', 'Must Be a Valid URL')
        cy.wait(200)
        cy.get('#SponsorName').clear()
        cy.wait(200)
        cy.get('#SponsorName').type('Google')
        cy.wait(200)
        cy.get('#SponsorURL').clear()
        cy.wait(200)
        cy.get('#SponsorURL').type('https://www.google.com/')
        cy.wait(200)
        cy.get('#saveSponsorButton').click()
        cy.wait(100)
        cy.get('#createSponsor').click()
        cy.wait(200)
        cy.get('#SponsorName').clear()
        cy.wait(200)
        cy.get('#SponsorName').type('bing')
        cy.wait(200)
        cy.get('#SponsorURL').clear()
        cy.wait(200)
        cy.get('#SponsorURL').type('https://www.bing.com/')
        cy.wait(200)
        cy.get('#saveSponsorButton').click()


        cy.wait(200)

        cy.get('.table > tbody:nth-child(2) > tr:nth-child(4) > td:nth-child(2) > a:nth-child(1)')
            .should('contain.text', 'https://www.google.com/')



        cy.wait(100)

        cy.get('body > div > main > table > tbody > tr:nth-child(4) > td:nth-child(3) > div > button.btn.btn-primary.image-btn.mt-2.ms-1.mb-4').click();



        //modal should be visible
        cy.get('#mediaModal').should('be.visible');

        //Click the save button with no file entered



        //select the invalid file and attatch it the the file input
        cy.get('#file-input').attachFile('default1.png');

        //Click the save button
        cy.get('#imageUploadForm > div.text-center.mt-5 > button:nth-child(1)').click();
        cy.wait(500)

        cy.get('body > div > main > table > tbody > tr:nth-child(5) > td:nth-child(3) > div > button.btn.btn-primary.image-btn.mt-2.ms-1.mb-4').click();
        cy.wait(200)


        //modal should be visible
        cy.get('#mediaModal').should('be.visible');

        //Click the save button with no file entered
        cy.get('#imageUploadForm > div.text-center.mt-5 > button:nth-child(1)').click();
        cy.wait(200)

        //select the invalid file and attatch it the the file input
        cy.get('#file-input').attachFile('default1.png');
        cy.wait(200)
        //Click the save button
        cy.get('#imageUploadForm > div.text-center.mt-5 > button:nth-child(1)').click();
        cy.wait(200)


        cy.visit('https://localhost:7202/')
        cy.wait(500)


    })

    it('AdminDeletsASponosr', () => {

        cy.visit('https://localhost:7202/Sponsor')
        cy.get('#deleteSponsor5').click()
        cy.wait(400)
        cy.get('#cancelDelete').click()
        cy.wait(200)
        cy.get('body > div > main > table > tbody > tr:nth-child(4) > td:nth-child(2) > a').should('contain.text', 'https://www.google.com/')
        cy.wait(600)
        cy.get('#deleteSponsor4').click()
        cy.wait(400)
        cy.get('#DeleteSponsorButton').click()
        cy.wait(200)
        cy.get('body > div > main > table > tbody > tr:nth-child(4) > td:nth-child(2) > a').should('contain.text', 'https://www.bing.com/')
        cy.wait(200)






    })


})



  