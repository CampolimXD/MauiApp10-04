using System.Text.Json;
using static MauiApp1.MainPage;
using Microsoft.Maui.Storage;
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
        AtualizarResumo();
        produtosListView.ItemsSource = Produtos;
        var hoje = DateTime.Today;
        var vencidos = Produtos.Where(p => p.Validade.HasValue && p.Validade.Value < hoje).ToList();
        var proximos = Produtos.Where(p => p.Validade.HasValue && p.Validade.Value <= hoje.AddDays(3) && p.Validade.Value >= hoje).ToList();
        
        if (vencidos.Any() || proximos.Any())
        {
            AlertaLabel.Text = $"Atenção: {vencidos.Count} vencido(s), { proximos.Count} prestes a vencer!";
        }
        else
        {
            AlertaLabel.Text = string.Empty;
        }
    }
    private void AtualizarResumo()
    {
        var produtos = produtosListView.ItemsSource as List<Produto>;
        int quantidade = produtos?.Count ?? 0;
        double total = produtos?.Sum(p => p.Preco) ?? 0;
        resumoLabel.Text = $"Total: {quantidade} produto(s) - Valor: R$ {total:F2}";
    }

    private async void produtosListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Produto produto)
        {
            bool confirmar = await DisplayAlert("Remover Produto", $"Deseja remover o produto \"{produto.Nome}\"?", "Sim", "Não");
            if (confirmar)
            {
                MainPage.Produtos.Remove(produto);
                ProdutoStorage.SalvarProdutos(MainPage.Produtos);
                AtualizarResumo();
                OnAppearing();
                produtosListView.ItemsSource = MainPage.Produtos.OrderBy(p => p.Validade).ToList();
            }
        }
    }
    public static class ProdutoStorage
    {
        const string ProdutosKey = "ProdutosSalvos";

        public static void SalvarProdutos(List<Produto> produtos)
        {
            string json = JsonSerializer.Serialize(produtos);
            Preferences.Set(ProdutosKey, json);
        }

        public static List<Produto> CarregarProdutos()
        {
            string json = Preferences.Get(ProdutosKey, string.Empty);
            return string.IsNullOrEmpty(json) ? new List<Produto>() :
                JsonSerializer.Deserialize<List<Produto>>(json) ?? new List<Produto>();
        }
    }
}