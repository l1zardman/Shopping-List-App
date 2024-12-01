describe('Shopping List Creator', () => {

    beforeEach(() => {
        cy.visit('/ShoppingListCreator');
    });

    it('should display the shopping list creator page', () => {
        cy.get('h3').should('contain.text', 'Create Your New Shopping List!');
    });

    it('should show an error when trying to finish with an empty name', () => {
        cy.get('button.btn-success').click();
        cy.get('.text-danger').should('contain.text', 'List name cannot be empty.');
    });

    it('should allow adding a product', () => {
        cy.get('input[placeholder="Product Name"]').type('Apples');
        cy.get('input[placeholder="Amount"]').type('2');
        cy.get('input[placeholder="Weight (kg)"]').type('1.5');
        cy.get('button.btn-primary').click();

        cy.get('ul.list-group')
            .should('exist')
            .and('contain.text', '- Apples - Amount: 2 - Weight: 1.5 kg');
    });

    it('should show an error when adding a product with invalid data', () => {
        cy.get('input[placeholder="Product Name"]').type('A'); // Invalid name
        cy.get('input[placeholder="Amount"]').type('0'); // Invalid amount
        cy.get('input[placeholder="Weight (kg)"]').type('-1'); // Invalid weight
        cy.get('button.btn-primary').click();

        cy.get('.text-danger').should('contain.text', 'Product name must be longer than 2 letters.');
    });

    it('should allow removing a product', () => {
        // Add a product first
        cy.get('input[placeholder="Product Name"]').type('Apples');
        cy.get('input[placeholder="Amount"]').type('2');
        cy.get('input[placeholder="Weight (kg)"]').type('1.5');
        cy.get('button.btn-primary').click();

        // Remove the product
        cy.get('ul.list-group button.btn-danger').click();
        cy.get('ul.list-group').should('not.exist');
    });

    it('should show an error when finishing without products', () => {
        cy.get('input.form-control').first().type('My Shopping List'); // Add a list name
        cy.get('button.btn-success').click();
        cy.get('.text-danger').should('contain.text', 'List must have at least one product.');
    });

    it('should successfully finish and navigate to the shopping lists viewer', () => {
        // Add a list name
        cy.get('input.form-control').first().type('My Shopping List');

        // Add a product
        cy.get('input[placeholder="Product Name"]').type('Apples');
        cy.get('input[placeholder="Amount"]').type('2');
        cy.get('input[placeholder="Weight (kg)"]').type('1.5');
        cy.get('button.btn-primary').click();

        // Finish
        cy.get('button.btn-success').click();

        // Assert navigation
        cy.url().should('contain', '/ShoppingListsViewer');
    });
});
