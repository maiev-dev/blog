namespace Front
{
    public partial class Form1 : Form
    {
        string apiKey = "0009c2c9-f060-47d1-9f6e-3054241c8167";
        public Form1()
        {
            InitializeComponent();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ApiClient apiClient = new ApiClient(new());
            apiClient.Authorize(apiKey);
            await apiClient.PostArticle(
                new Article("Header",
                            "Abstract",
                            "Text")
            );

        }
    }
}