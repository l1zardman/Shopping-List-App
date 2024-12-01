namespace ShoppingListApp.E2ESystemTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ShoppingListAppE2ETests : PageTest {
    
    [Test]
    public async Task Homepage_RendersCorrectly() {
        await Page.GotoAsync("http://localhost:5000");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Home"));
        
        var buttons = Page.Locator("#List-View-Nav-Button button");

        // Verify that exactly two buttons are rendered
        Assert.That(2, Is.EqualTo(await buttons.CountAsync()));
        Assert.That("View My Lists", Is.EqualTo(await buttons.Nth(0).TextContentAsync()));
        Assert.That("Create New Shopping List", Is.EqualTo(await buttons.Nth(1).TextContentAsync()));
        Thread.Sleep(1000);
    }

    [Test]
    public async Task ClickingViewMyListsButton_RendersCorrectly() {
        await Page.GotoAsync("http://localhost:5000");

        var buttons = Page.Locator("#List-View-Nav-Button button");
        await buttons.Nth(0).ClickAsync();

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("ShoppingListsViewer"));

        // Verify shopping lists rendered
        var shoppingLists = Page.Locator("#shopping-lists-list li");
        await Expect(shoppingLists).ToHaveCountAsync(2);
        
        // Check if text is rendered correctly
        var list1 = shoppingLists.Nth(0);
        var list1Name = await list1.Locator("#shopping-list-name-date").TextContentAsync();
        
        Assert.That(list1Name, Is.EqualTo("Deafult list 1 - 5/24/2023"));
        
        var list2 = shoppingLists.Nth(1);
        var list2Name = await list2.Locator("#shopping-list-name-date").TextContentAsync();
        
        Assert.That(list2Name, Is.EqualTo("Deafult list 2 - 6/30/2023"));
        
        Thread.Sleep(1000);
    }

    [Test]
    public async Task ShoppingListViewer_DetailsButtonClick_RendersLIstContents() {
        
        var page = await Browser.NewPageAsync();
        
        await page.GotoAsync("http://localhost:5000/ShoppingListsViewer");
        
        var shoppingLists = page.Locator("#shopping-lists-list li");
        
        var list2 = shoppingLists.Nth(1);
        
        var list2DetailsBtn = list2.Locator("#view-details-btn");
        
        await list2DetailsBtn.ClickAsync();
        
        var detailsDiv = page.Locator("#details-view-div div");
        
        var list2Details = detailsDiv.Locator("#shopping-list-details ul");
        
        // var list2DetailsList = list2Details.Locator("#shopping-list-details-row");
        //
        await Expect(list2Details).ToHaveCountAsync(9);
        
        Thread.Sleep(1000);
    }
}