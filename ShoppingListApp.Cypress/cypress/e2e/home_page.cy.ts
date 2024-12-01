describe('Home Page Tests', () => {
    beforeEach(() => {
        // Visit the root URL
        cy.visit('http://localhost:5000/');
    });

    it('should display the welcome title', () => {
        // Verify the welcome title is visible
        cy.get('#Welcome-Title h1').should('be.visible').and('contain.text', 'Welcome in Shopping List App!');
    });

    it('should display navigation buttons', () => {
        // Verify both buttons are visible and have correct text
        cy.get('#List-View-Nav-Button button').eq(0).should('be.visible').and('contain.text', 'View My Lists');
        cy.get('#List-View-Nav-Button button').eq(1).should('be.visible').and('contain.text', 'Create New Shopping List');
    });

    it('should navigate to ShoppingListsViewer when the first button is clicked', () => {
        // Click the "View My Lists" button
        cy.get('#List-View-Nav-Button button').eq(0).click();

        // Verify URL contains the navigation path
        cy.url().should('include', '/ShoppingListsViewer');
    });

    it('should navigate to ShoppingListCreator when the second button is clicked', () => {
        // Click the "Create New Shopping List" button
        cy.get('#List-View-Nav-Button button').eq(1).click();

        // Verify URL contains the navigation path
        cy.url().should('include', '/ShoppingListCreator');
    });
});
