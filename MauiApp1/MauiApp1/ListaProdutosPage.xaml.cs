namespace MauiApp1;

public partial class ListaProdutosPage : ContentPage
{
	public ListaProdutosPage()
	{
		InitializeComponent();
		produtosListView.ItemsSource = MainPage.Produtos;
    
    }
    private void filtroCategoriaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        string categoriaSelecionada = filtroCategoriaPicker.SelectedItem?.ToString() ?? "Todas";
        if (categoriaSelecionada == "Todas")
        {
            produtosListView.ItemsSource = MainPage.Produtos.OrderBy(p => p.Validade).ToList();
        }
        else
        {
            produtosListView.ItemsSource = MainPage.Produtos.Where(p => p.Categoria == categoriaSelecionada).OrderBy(p => p.Validade).ToList();
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        produtosListView.ItemsSource = MainPage.Produtos.OrderBy(p => p.Validade).ToList();
    }

}