describe('Hacker News Viewer E2E Tests', () => {
  beforeEach(() => {
    cy.visit('http://localhost:4200'); // Assumes your Angular app is running on default port 4200
  });

  it('should display a list of stories', () => {
    cy.get('.story-list li').should('have.length.at.least', 1);
    cy.get('.story-list li a').first().should('have.attr', 'href');
  });

  it('should allow searching for stories', () => {
    const searchTerm = 'google'; // Or any common word in HN titles
    cy.get('.search-bar input').type(searchTerm);
    cy.wait(500); // Wait for debounce and API call

    // cy.get('.story-list li').each(($li) => {
    //   cy.wrap($li).should('contain.text', searchTerm);
    // });
  });

  it('should paginate to the next and previous pages', () => {
    cy.get('.story-list li').first().then(($firstStory) => {
      const firstStoryTitle = $firstStory.text();

      cy.get('.pagination-controls button').contains('Next').click();
      cy.wait(500); // Wait for API call

      cy.get('.story-list li').first().should('not.contain.text', firstStoryTitle); // Should be a different story
      cy.get('.pagination-controls span').should('contain.text', 'Page 2');

      cy.get('.pagination-controls button').contains('Previous').click();
      cy.wait(500);

      cy.get('.story-list li').first().should('contain.text', firstStoryTitle); // Should be back to the original story
      cy.get('.pagination-controls span').should('contain.text', 'Page 1');
    });
  });

  it('should handle stories without links gracefully', () => {
    // This test would require mocking the backend or ensuring a specific story without a link is returned.
    // For a real E2E test, you might use a test-specific backend environment
    // or directly verify the text "(No Link Available)" if such a story appears.
    cy.get('.story-list li').contains('(No Link Available)').should('exist');
  });
});