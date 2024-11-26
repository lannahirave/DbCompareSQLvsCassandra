// See https://aka.ms/new-console-template for more information

using System.Text;
using SQL;

Console.OutputEncoding = Encoding.UTF8;
var context = new SalesDbContext();
context.Database.EnsureCreated();
if (!context.Products.Any())
{
    var dataGenerator = new DataGenerator(
        10,
        5,
        10000,
        new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        DateTime.UtcNow
    );
    Console.WriteLine("Seeding database...");
    context.Products.AddRange(dataGenerator.Products);
    context.Stores.AddRange(dataGenerator.Stores);
    context.Sales.AddRange(dataGenerator.Sales);
    context.SaleItems.AddRange(dataGenerator.SaleItems);
    context.SaveChanges();
    Console.WriteLine("Database seeded.");
}


var repository = new Repository(context);

// Завдання 1
var totalSoldQuantity = await repository.Task1();
Console.Write("К-сть проданого товару: ");
Console.WriteLine(totalSoldQuantity);

// Завдання 2
var totalSoldValue = await repository.Task2();
Console.Write("Вартість проданого товару: ");
Console.WriteLine(totalSoldValue);

// Завдання 3
var startDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
var endDate = DateTime.UtcNow;
var totalValueInPeriod = await repository.Task3(startDate, endDate);
Console.Write("Вартість проданого за період: ");
Console.WriteLine(totalValueInPeriod);

// Завдання 4
var productId = 1;
var storeId = 1;
var quantityInStore = await repository.Task4(productId, storeId, startDate, endDate);
Console.Write($"Кількість товару з ID {productId}, проданого в магазині з ID {storeId} за період: ");
Console.WriteLine(quantityInStore);

// Завдання 5
var quantityInAllStores = await repository.Task5(productId, startDate, endDate);
Console.Write($"Кількість товару з ID {productId}, проданого в усіх магазинах за період: ");
Console.WriteLine(quantityInAllStores);

// Завдання 6
var totalRevenue = await repository.Task6(startDate, endDate);
Console.Write("Сумарна виручка магазинів за період: ");
Console.WriteLine(totalRevenue);

// Завдання 7
var topPairs = await repository.Task7(startDate, endDate);
Console.WriteLine("Топ 10 купівель товарів по два за період:");
foreach (var pair in topPairs)
{
    Console.WriteLine($"{pair.Product1}, {pair.Product2} - {pair.Count} разів");
}

// Завдання 8
var topTriplets = await repository.Task8(startDate, endDate);
Console.WriteLine("Топ 10 купівель товарів по три за період:");
foreach (var triplet in topTriplets)
{
    Console.WriteLine($"{triplet.Product1}, {triplet.Product2}, {triplet.Product3} - {triplet.Count} разів");
}

// Завдання 9
var topQuadruplets = await repository.Task9(startDate, endDate);
Console.WriteLine("Топ 10 купівель товарів по чотири за період:");
foreach (var quadruplet in topQuadruplets)
{
    Console.WriteLine($"{quadruplet.Product1}, {quadruplet.Product2}, {quadruplet.Product3}, {quadruplet.Product4} - {quadruplet.Count} разів");
}
