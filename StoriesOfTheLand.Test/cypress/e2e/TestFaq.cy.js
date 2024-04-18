
//first got to login using test user then run cypress
describe('visit website', () => {
  it('passes', () => {
      cy.visit('https://localhost:7202/Faq')
  })
})

describe('Functional tests For CRUD', () => {
    let firstTitle = '';
    let firstDescrip = "";


    let faq = { Id: 1, Title: "Can anyone go into the camp", Description: "Yes Everyone is free to  come in and learn the various plants" };

    /*
        it('view emptylist in Database', () => {
            cy.visit('http://localhost:5044/Faq')
            cy.get(`#accordionFaq > p`)
                .should('contain.text', 'No Faqs as of yet')
        })*/
    

        it('viewlistof3inDatabase', () => {
            cy.visit('https://localhost:7202/Faq')
    
            cy.fixture('FAQSEED.json').then(data => {
                // test for accordian item 1 title and description
                cy.get('#accordionFaq > div:nth-child(1) > h2 > button').should('contain.text', 'Are there edible plants around here?')
                cy.get('#collapse3 > div').should('contain.text', 'Why yes, there are many Edible plants')
                // test for accordian item 1 title and description
                cy.get('#accordionFaq > div:nth-child(2) > h2 > button').should('contain.text', 'Are there wild roses?')
                cy.get('#collapse2 > div').should('contain.text', 'Yes, there are many wild roses here around the camp')
                // test for accordian item 1 title and description
                cy.get('#accordionFaq > div:nth-child(3) > h2 > button').should('contain.text', 'Can anyone go into the camp')
                cy.get('#collapse1 > div').should('contain.text', 'Yes Everyone is free to  come in and learn the various plants')
                
                
            })
        })
    

        it('testcancelsstopsanedit', () => {

            //visit site
            cy.visit('https://localhost:7202/Faq')
            //press delete faq
            cy.get('#editFaq1').click()
            //make sure modal is visible
            cy.get("#faqModal").should("be.visible")
            cy.wait(2000)
            //click cancel
            cy.get('#cancelEdit').click()
            cy.wait(2000)
            //make sure modal is invisible
            cy.get("#faqModal").should('not.be.visible');
            //checking to make sure the values are not alterred


    })
    
        it('testEditFaqButtonEditsDatabase', () => {
            //visiting edit page of ID=1
            cy.visit('https://localhost:7202/Faq')

            cy.get('#editFaq1').click()
            cy.get("#faqModal").should("be.visible")
            //clicking save button after changing title and desription 
            cy.get('#titleInput').clear()
            cy.get('#titleInput').clear()
            cy.wait(2000)
            cy.get('#titleInput').type('Add things for testing?')
            cy.wait(2000)
            cy.get('#descInput').clear()
            cy.wait(2000)
            cy.get('#descInput').type('Add things for testing')
            cy.wait(2000)
            cy.get('#saveFaqButton').click()
            cy.wait(2000)
            
            cy.get('#accordionFaq > div:nth-child(1) > h2 > button').should('contain.text', 'Add things for testing?')
            cy.get('#collapse1 > div').should('contain.text', 'Add things for testing')
    
    
        })

   
    it('makessurecancelonCreateecancels', () => {
        //visit site
        //visit site
        cy.visit('https://localhost:7202/Faq')
        //press delete faq
        cy.get('#createFaq').click()
        //make sure modal is visible
        cy.get("#faqModal").should("be.visible")
        cy.wait(2000)
        //click cancel
        cy.get('#cancelEdit').click()
        cy.wait(2000)
        //make sure modal is invisible
        cy.get("#faqModal").should('not.be.visible');

        //checking to make sure the values are not alterred



        //press delete faq
        cy.get('#createFaq').click()
        //make sure modal is visible
        cy.get("#faqModal").should("be.visible")
        cy.wait(2000)
        //click cancel
        cy.get('#saveFaqButton').click()
        cy.wait(2000)

        cy.get('#titleError').should("be.visible")
        cy.wait(2000)
        cy.get('#titleError').should("contain.text", "Title Is Required")
        cy.wait(2000)
        cy.get('#descError').should("be.visible")
        cy.wait(2000)
        cy.get('#descError').should("contain.text", "Description Is Required")
        cy.wait(2000)
        cy.get('#titleInput').type('a')
        cy.wait(2000)
        cy.get('#descInput').type('a')
        cy.wait(2000)
        cy.get('#saveFaqButton').click()
        cy.wait(2000)
        cy.get('#titleError').should("contain.text", "Title must be atleast 5 characters long")
        cy.wait(2000)
        cy.get('#descError').should("be.visible")
        cy.wait(2000)
        
       
        cy.get('#descError').should("contain.text", "Description must be atleast 10 characters")
        cy.wait(2000)
        cy.get('#descError').should("be.visible")
        //click cancel
        cy.get('#cancelEdit').click()
        cy.wait(2000)
        //make sure modal is invisible
        cy.get("#faqModal").should('not.be.visible');
    })


     it('testCreateFaqButtonAddsToDatabase', () => {
         //visit site
         cy.visit('https://localhost:7202/Faq')
         cy.get('#createFaq').click()
         cy.get("#faqModal").should("be.visible")
         //clicking save button after changing title and desription 
         
         cy.wait(3000)
         cy.get('#titleInput').type('able to add to the top')
         cy.wait(3000)
        
         cy.wait(2000)
         cy.get('#descInput').type('just a test for me')
         cy.wait(2000)
         cy.get('#saveFaqButton').click()
         cy.wait(2000)
         
         cy.get('#accordionFaq > div:nth-child(1) > h2 > button').should('contain.text', 'able to add to the top')
         cy.get('#collapse4 > div').should('contain.text', 'just a test for me')
 
     })


    it('deletemodalcancelbuttonscancelsandclosesthemodal', () => {
        //visit site
        //visit site
        cy.visit('https://localhost:7202/Faq')
        //press delete faq
        cy.get('#deleteFaq3').click()
        //make sure modal is visible
        cy.get("#faqDeleteModal").should("be.visible")
        //click cancel
        cy.wait(2000)
        cy.get('#cancelDelete').click()
        cy.wait(2000)
        //make sure modal is invisible
        cy.get("#faqDeleteModal").should('not.be.visible');
        //checking to make sure the values are not alterred

    })
     it('testDeleteFaqButtonDeletesFromDatabase', () => {
         //visit site
         cy.visit('https://localhost:7202/Faq')
         //press delete faq
         cy.get('#deleteFaq3').click()
        
 
       //make sure modal is visible
         cy.get("#faqDeleteModal").should("be.visible")
         //press save
         cy.get("#DeleteFaqButton").click()
 
         //maksure the deleted item in now whereon the page 
 
         //make sure the 2nd in the list is now first
         cy.get('#accordionFaq > div:nth-child(1) > h2 > button').should('contain.text', "able to add to the top")
         cy.get('#collapse4 > div').should('contain.text', "just a test for me")
     })

   


})
