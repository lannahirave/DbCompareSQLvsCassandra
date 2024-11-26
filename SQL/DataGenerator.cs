using Bogus;
using SQL.Ents;

namespace SQL;

public class DataGenerator
{
    public List<Product> Products { get; private set; }
    public List<Store> Stores { get; private set; }
    public List<Sale> Sales { get; private set; }
    public List<SaleItem> SaleItems { get; private set; }

    private readonly int _numberOfProducts;
    private readonly int _numberOfStores;
    private readonly int _numberOfSales;
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;

    public DataGenerator(
        int numberOfProducts,
        int numberOfStores,
        int numberOfSales,
        DateTime startDate,
        DateTime endDate,
        int seed = 8675309)
    {
        _numberOfProducts = numberOfProducts;
        _numberOfStores = numberOfStores;
        _numberOfSales = numberOfSales;
        _startDate = startDate;
        _endDate = endDate;

        // Initialize lists
        Products = new List<Product>();
        Stores = new List<Store>();
        Sales = new List<Sale>();
        SaleItems = new List<SaleItem>();

        Randomizer.Seed = new Random(seed);
            
        // Generate data
        GenerateData();
    }

    private void GenerateData()
    {
        // Generate Products
        Products = GenerateProducts(_numberOfProducts);

        // Generate Stores
        Stores = GenerateStores(_numberOfStores);

        // Generate Sales
        Sales = GenerateSales(_numberOfSales, Stores, _startDate, _endDate);

        // Generate SaleItems
        SaleItems = GenerateSaleItems(Sales, Products);
    }

    private List<Product> GenerateProducts(int count)
    {
        var productId = 1;
        var faker = new Faker<Product>()
            .RuleFor(p => p.ProductId, f => productId++)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(1, 100)));

        var products = faker.Generate(count);

        return products;
    }

    private List<Store> GenerateStores(int count)
    {
        var storeId = 1;
        var faker = new Faker<Store>()
            .RuleFor(s => s.StoreId, f => storeId++)
            .RuleFor(s => s.Name, f => f.Company.CompanyName());

        var stores = faker.Generate(count);

        return stores;
    }

    private List<Sale> GenerateSales(int count, List<Store> stores, DateTime startDate, DateTime endDate)
    {
        var saleId = 1;
        var saleFaker = new Faker<Sale>()
            .RuleFor(s => s.SaleId, f => saleId++)
            .RuleFor(s => s.StoreId, f => f.PickRandom(stores).StoreId)
            .RuleFor(s => s.SaleDate, f => f.Date.Between(startDate, endDate));

        var sales = saleFaker.Generate(count);

        return sales;
    }

    private List<SaleItem> GenerateSaleItems(List<Sale> sales, List<Product> products)
    {
        var saleItemId = 1;
        var saleItems = new List<SaleItem>();
        var faker = new Faker();

        foreach (var sale in sales)
        {
            var numberOfItems = faker.Random.Int(1, 5); 
            for (var i = 0; i < numberOfItems; i++)
            {
                var product = faker.PickRandom(products);
                var quantity = faker.Random.Int(1, 10);
                var unitPrice = product.UnitPrice;

                var saleItem = new SaleItem
                {
                    SaleItemId = saleItemId++,
                    SaleId = sale.SaleId,
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

                saleItems.Add(saleItem);
            }
        }

        return saleItems;
    }
}