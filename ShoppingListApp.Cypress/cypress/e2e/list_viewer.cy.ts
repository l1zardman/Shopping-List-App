// before(() => {
//     cy.request('POST', 'http://localhost:5000/api/Reset').then((response) => {
//         expect(response.status).to.eq(200);
//     });
// });
describe('ShoppingListsViewer E2E Tests', () => {
    beforeEach(() => {
        // Navigate to the ShoppingListsViewer page
        cy.visit('/ShoppingListsViewer');
    });

    it('should display all shopping lists fetched from the API', () => {
        // Verify the shopping list container is visible
        cy.get('#shopping-lists-list').should('be.visible');

        // Verify the correct number of shopping lists are displayed
        cy.get('#shopping-lists-element').then((lists) => {
            expect(lists.length).to.be.greaterThan(0); // Ensure there are shopping lists
        });

        // Verify content of the first shopping list
        cy.get('#shopping-list-name-date').first().should('contain.text', 'Deafult list');
    });

    it('should display shopping list details when "Details" is clicked', () => {
        // Click the "Details" button for the first shopping list
        cy.get('#view-details-btn').first().click();

        // Verify the details view is visible
        cy.get('#details-view-div').should('be.visible');


        // Verify the product list in the details view
        cy.get('#shopping-list-details').then((rows) => {
            expect(rows.length).to.be.greaterThan(0); // Ensure there are products in the list
        });

        // Check the content of the first product
        cy.get('#shopping-list-details-row').first().should('contain.text', 'Pasta');
    });

    it('should toggle a product’s completion status', () => {
        // Open the details view
        cy.get('#view-details-btn').first().click();

        // Toggle the checkbox for the first product
        cy.get('#shopping-list-details-row input[type="checkbox"]').first().check();

        // Wait for the backend to process the update
        cy.wait(1000);

        // Verify the product's completion status
        cy.get('#shopping-list-details-row').first().find('span').should('have.css', 'text-decoration-line', 'line-through');
    });

    it('should delete a shopping list when "Done" is clicked', () => {
        // Click the "Done" button for the first shopping list
        cy.get('#done-btn').first().click();

        // Wait for the backend to process the deletion
        cy.wait(1000);

        // Verify the shopping list is removed from the UI
        cy.get('#shopping-lists-element').then((lists) => {
            expect(lists.length).to.be.greaterThan(0); // Ensure one less shopping list
        });
    });

    it('should close the details view when "Close" is clicked', () => {
        // Open the details view
        cy.get('#view-details-btn').first().click();

        // Verify the details view is visible
        cy.get('#details-view-div').should('be.visible');

        // Click the "Close" button
        cy.get('button').contains('Close').click();

        // Verify the details view is hidden
        cy.get('#details-view-div').should('not.exist');
    });
});
