using Microsoft.EntityFrameworkCore;

namespace SQL;

public class Repository(SalesDbContext context)
{
   /// <summary>
        /// Завдання 1: Порахувати кількість проданого товару.
        /// </summary>
        public async Task<int> Task1()
        {
            // Підрахунок загальної кількості проданих товарів
            var totalQuantity = await context.SaleItems.SumAsync(x => x.Quantity);
            return totalQuantity;
        }

        /// <summary>
        /// Завдання 2: Порахувати вартість проданого товару.
        /// </summary>
        public async Task<decimal> Task2()
        {
            // Підрахунок загальної вартості проданих товарів
            var totalValue = await context.SaleItems.SumAsync(x => x.UnitPrice * x.Quantity);
            return totalValue;
        }

        /// <summary>
        /// Завдання 3: Порахувати вартість проданого товару за період.
        /// </summary>
        public async Task<decimal> Task3(DateTime startDate, DateTime endDate)
        {
            // Підрахунок вартості проданих товарів за вказаний період
            var totalValue = await context.SaleItems
                .Where(si => si.Sale.SaleDate >= startDate && si.Sale.SaleDate <= endDate)
                .SumAsync(x => x.UnitPrice * x.Quantity);
            return totalValue;
        }

        /// <summary>
        /// Завдання 4: Порахувати скільки було придбано товару A в магазині B за період C.
        /// </summary>
        public async Task<int> Task4(int productId, int storeId, DateTime startDate, DateTime endDate)
        {
            // Підрахунок кількості придбаного товару A в магазині B за період C
            var totalQuantity = await context.SaleItems
                .Where(si => si.ProductId == productId &&
                             si.Sale.StoreId == storeId &&
                             si.Sale.SaleDate >= startDate &&
                             si.Sale.SaleDate <= endDate)
                .SumAsync(x => x.Quantity);
            return totalQuantity;
        }

        /// <summary>
        /// Завдання 5: Порахувати скільки було придбано товару A в усіх магазинах за період C.
        /// </summary>
        public async Task<int> Task5(int productId, DateTime startDate, DateTime endDate)
        {
            // Підрахунок кількості придбаного товару A у всіх магазинах за період C
            var totalQuantity = await context.SaleItems
                .Where(si => si.ProductId == productId &&
                             si.Sale.SaleDate >= startDate &&
                             si.Sale.SaleDate <= endDate)
                .SumAsync(x => x.Quantity);
            return totalQuantity;
        }

        /// <summary>
        /// Завдання 6: Порахувати сумарну виручку магазинів за період C.
        /// </summary>
        public async Task<decimal> Task6(DateTime startDate, DateTime endDate)
        {
            // Підрахунок загальної виручки за вказаний період
            var totalRevenue = await context.SaleItems
                .Where(si => si.Sale.SaleDate >= startDate && si.Sale.SaleDate <= endDate)
                .SumAsync(x => x.UnitPrice * x.Quantity);
            return totalRevenue;
        }

        /// <summary>
        /// Завдання 7: Вивести топ 10 купівель товарів по два за період C.
        /// </summary>
        public async Task<List<(string Product1, string Product2, int Count)>> Task7(DateTime startDate, DateTime endDate)
        {
            // Завантаження даних у пам'ять
            var saleItems = await context.SaleItems
                .Where(si => si.Sale.SaleDate >= startDate && si.Sale.SaleDate <= endDate)
                .Select(si => new { si.SaleId, si.ProductId })
                .ToListAsync();

            // Групування продажів за SaleId
            var saleGroups = saleItems
                .GroupBy(si => si.SaleId)
                .Select(g => g.Select(si => si.ProductId).Distinct().OrderBy(id => id).ToList())
                .ToList();

            // Знаходження пар товарів
            var pairCounts = new Dictionary<(int, int), int>();

            foreach (var products in saleGroups)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    for (int j = i + 1; j < products.Count; j++)
                    {
                        var pair = (products[i], products[j]);
                        if (pairCounts.ContainsKey(pair))
                            pairCounts[pair]++;
                        else
                            pairCounts[pair] = 1;
                    }
                }
            }

            var topPairs = pairCounts
                .OrderByDescending(kv => kv.Value)
                .Take(10)
                .ToList();

            var productIds = topPairs.SelectMany(p => new[] { p.Key.Item1, p.Key.Item2 }).Distinct();

            var productNames = await context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId, p => p.Name);

            var result = topPairs.Select(p => (
                Product1: productNames[p.Key.Item1],
                Product2: productNames[p.Key.Item2],
                Count: p.Value
            )).ToList();

            return result;
        }
        
        /// <summary>
        /// Завдання 8: Вивести топ 10 купівель товарів по три за період C.
        /// </summary>
        public async Task<List<(string Product1, string Product2, string Product3, int Count)>> Task8(DateTime startDate, DateTime endDate)
{
    // Завантаження даних у пам'ять
    var saleItems = await context.SaleItems
        .Where(si => si.Sale.SaleDate >= startDate && si.Sale.SaleDate <= endDate)
        .Select(si => new { si.SaleId, si.ProductId })
        .ToListAsync();

    // Групування продажів за SaleId
    var saleGroups = saleItems
        .GroupBy(si => si.SaleId)
        .Select(g => g.Select(si => si.ProductId).Distinct().OrderBy(id => id).ToList())
        .ToList();

    // Знаходження трійок товарів
    var tripletCounts = new Dictionary<(int, int, int), int>();

    foreach (var products in saleGroups)
    {
        var combinations = GetCombinations(products, 3);
        foreach (var combo in combinations)
        {
            var key = (combo[0], combo[1], combo[2]);
            if (tripletCounts.ContainsKey(key))
                tripletCounts[key]++;
            else
                tripletCounts[key] = 1;
        }
    }

    var topTriplets = tripletCounts
        .OrderByDescending(kv => kv.Value)
        .Take(10)
        .Select(kv => new { Product1 = kv.Key.Item1, Product2 = kv.Key.Item2, Product3 = kv.Key.Item3, Count = kv.Value })
        .ToList();

    var productIds = topTriplets.SelectMany(p => new[] { p.Product1, p.Product2, p.Product3 }).Distinct();

    var productNames = await context.Products
        .Where(p => productIds.Contains(p.ProductId))
        .ToDictionaryAsync(p => p.ProductId, p => p.Name);

    var result = topTriplets.Select(p => (
        Product1: productNames[p.Product1],
        Product2: productNames[p.Product2],
        Product3: productNames[p.Product3],
        Count: p.Count
    )).ToList();

    return result;
}

/// <summary>
/// Завдання 9: Вивести топ 10 купівель товарів по чотири за період C.
/// </summary>
public async Task<List<(string Product1, string Product2, string Product3, string Product4, int Count)>> Task9(DateTime startDate, DateTime endDate)
{
    // Завантаження даних у пам'ять
    var saleItems = await context.SaleItems
        .Where(si => si.Sale.SaleDate >= startDate && si.Sale.SaleDate <= endDate)
        .Select(si => new { si.SaleId, si.ProductId })
        .ToListAsync();

    // Групування продажів за SaleId
    var saleGroups = saleItems
        .GroupBy(si => si.SaleId)
        .Select(g => g.Select(si => si.ProductId).Distinct().OrderBy(id => id).ToList())
        .ToList();

    // Знаходження четвірок товарів
    var quadrupletCounts = new Dictionary<(int, int, int, int), int>();

    foreach (var products in saleGroups)
    {
        var combinations = GetCombinations(products, 4);
        foreach (var combo in combinations)
        {
            var key = (combo[0], combo[1], combo[2], combo[3]);
            if (quadrupletCounts.ContainsKey(key))
                quadrupletCounts[key]++;
            else
                quadrupletCounts[key] = 1;
        }
    }

    var topQuadruplets = quadrupletCounts
        .OrderByDescending(kv => kv.Value)
        .Take(10)
        .Select(kv => new { Product1 = kv.Key.Item1, Product2 = kv.Key.Item2, Product3 = kv.Key.Item3, Product4 = kv.Key.Item4, Count = kv.Value })
        .ToList();

    var productIds = topQuadruplets.SelectMany(p => new[] { p.Product1, p.Product2, p.Product3, p.Product4 }).Distinct();

    var productNames = await context.Products
        .Where(p => productIds.Contains(p.ProductId))
        .ToDictionaryAsync(p => p.ProductId, p => p.Name);

    var result = topQuadruplets.Select(p => (
        Product1: productNames[p.Product1],
        Product2: productNames[p.Product2],
        Product3: productNames[p.Product3],
        Product4: productNames[p.Product4],
        Count: p.Count
    )).ToList();

    return result;
}

// Допоміжний метод для генерації комбінацій
private IEnumerable<int[]> GetCombinations(List<int> list, int length)
{
    if (length == 0)
        yield return new int[0];
    else
    {
        for (int i = 0; i < list.Count; i++)
        {
            var head = list[i];
            var tail = list.Skip(i + 1).ToList();
            foreach (var tailCombination in GetCombinations(tail, length - 1))
            {
                yield return new[] { head }.Concat(tailCombination).ToArray();
            }
        }
    }
}


}