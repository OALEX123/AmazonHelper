using System;
using System.Linq;
using System.Threading.Tasks;
using AmazonHelper.Business.Models;
using AmazonHelper.Business.Services;
using AmazonHelper.Common;
using AmazonHelper.Engine.Parser;
using Microsoft.Practices.Unity;
using Nito.AsyncEx;

namespace AmazonHelper.AmazonRemoteService
{
    class Program
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            BusinnessBootstrapper.RegisterTypes(container);
            return container;
        });

        static void Main(string[] args)
        {
            AsyncContext.Run(() => MainAsync(args));
        }

        static async void MainAsync(string[] args)
        {
            // ручной тест
            //var result = await AmazonParser.ParseBuyPriceAsync("B00U4YW8XE", "amazon.com");

            //var result2 = await AmazonParser.ParseBuyPriceAsync("B00NHQFA1I", "shit.com");

            try
            {
                BlMapper.RegisterMappings();

                var productService = container.Value.Resolve<IProductService>();

                // выбираем все группы (интервалы)
                var groups = await productService.GetProductGroups();

                // для каждой группы запускаем отдельный поток сканированя (чтобы они выполнялись параллельно)
                foreach (var group in groups)
                {
                    Task.Run(() => new TaskRunner(container.Value).RunScanningProcessAsync(group.Interval * 1000, group.GroupId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
            }

            Console.ReadLine();
        }
    }

    public class TaskRunner
    {
        private readonly IUnityContainer _container;

        public TaskRunner(IUnityContainer container)
        {
            _container = container;
        }
        public async Task RunScanningProcessAsync(int interval, int groupId)
        {
            int delayInterval = 5000;
            int intervalUsed = 0;

            while (true)
            {
                try
                {
                    string logMessage = string.Empty;
                    var productService = _container.Resolve<IProductService>();
                    var statsService = _container.Resolve<IStatsService>();
                    var settingsService = _container.Resolve<ICommonSettingsService>();
                    var emailService = _container.Resolve<IEmailService>();
                    var logService = _container.Resolve<ILogService>();
                    // выбираем все активные продукты для группы
                    var products = await productService.GetProductsPagedAsync(new ProductCriteria { GroupId = groupId, IsActive = true },
                        new PagingParams { PageNum = 1, PageSize = int.MaxValue });

                    var settings = await settingsService.GetCommonSettings(1);
                    var groups = await productService.GetProductGroups();
                    var group = groups.FirstOrDefault(g => g.GroupId == groupId);
                    Console.WriteLine("Parsing group {1}(interval: {0}) with {2} products", interval, group.GroupName, products.TotalCount);

                    // идем по продуктам из группы
                    foreach (var product in products.Data)
                    {
                        // перед запросов сервиса выставляем минимальную задержу 5 сек, чтобы не блочила кэптча
                        await Task.Delay(delayInterval);

                        #region one product handling
                        
                        if (product.IsActive)
                        {
                            var statsEntry = new StatsEntry
                            {
                                ProductId = product.ProductId,
                                GroupId = product.GroupId
                            };
                            // запрашиваем сервис, отсылаем асин и имя компании
                            var result = await AmazonParser.ParseBuyPriceAsync(product.Asin, settings.CompanyName);
                            switch (result)
                            {
                                case ParsePriceResult.BuyPriceBelongsToCompany:
                                    statsEntry.Result = StatsEntryResult.BuyPriceBelongsToCompany;
                                    break;
                                case ParsePriceResult.BuyPriceNotBelongsToCompany:
                                    statsEntry.Result = StatsEntryResult.BuyPriceNotBelongsToCompany;
                                    break;
                            }

                            // если пришел вменяемый результат - сохраняем статистику
                            if (statsEntry.Result == StatsEntryResult.BuyPriceBelongsToCompany ||
                                statsEntry.Result == StatsEntryResult.BuyPriceNotBelongsToCompany)
                            {
                                await statsService.SaveStatEntry(statsEntry);
                            }

                            logMessage = $"Product asin : {product.Asin}. Group: {group.GroupName}. Result : {result}.";

                            // если buy price не принадлежит компании и включено уведомление по емейл - шлем письмо
                            if (statsEntry.Result == StatsEntryResult.BuyPriceNotBelongsToCompany)
                            {
                                // если у продукта включена нотификация
                                if (product.IsNotificationEnabled)
                                {
                                    try
                                    {
                                        await emailService.NotifyBuyPriceNotBelongToCompany(settings.Email, product.Asin, settings.CompanyName);
                                        logMessage += " Email has sent successfully.";
                                    }
                                    catch (Exception)
                                    {
                                        logMessage += " Failed to send email.";
                                    }
                                }
                            }

                            await logService.SaveLogAsync(new LogEntry
                            {
                                Message = logMessage
                            });

                            Console.WriteLine(logMessage);

                            intervalUsed += delayInterval;
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.GetBaseException().Message);
                }
                finally
                {
                    await Task.Delay(intervalUsed >= interval ? delayInterval : interval - intervalUsed);
                    intervalUsed = 0;
                }
            }
        }
    }
}
