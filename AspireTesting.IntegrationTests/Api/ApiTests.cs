namespace AspireTesting.IntegrationTests.Api
{
    [Trait("Category", "Integration")]
    public class ApiTests(ApiFixture fixture) : IClassFixture<ApiFixture>
    {
        [Fact]
        public async Task FirstNoteIsCorrect()
        {
            var note = await fixture.ApiClient.GetNoteByIdAsync(1);

            Assert.Equal("First Note1", note.Text);
        }
    }
}
