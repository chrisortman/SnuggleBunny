using System.Configuration;
using Shouldly;
using SnuggleBunny.Budget.Config;
using Xunit;

namespace SnuggleBunny.Tests.Budget.Config
{
    public class BudgetConfigLoaderTests
    {
        public class TheLoadFromFileMethod
        {
            [Fact]
            public void CanLoadConfigFromYamlFile()
            {
                var loader = new BudgetConfigLoader();
                BudgetConfig config = loader.LoadFile(@"TestData\budget_config.yml");

                config.Categories.ContainsKey("grocery").ShouldBe(true);
                config.Categories.ContainsKey("clothing").ShouldBe(true);
                config.Categories["grocery"].Limit.ShouldBe(500M);
                config.Categories["clothing"].Limit.ShouldBe(200M);
            }

            [Fact]
            public void WontCrashIfFileDoesNotExist()
            {
                var loader = new BudgetConfigLoader();
                Should.NotThrow(() => loader.LoadFile("does_not_exist.yml"));
            }

            [Fact]
            public void ThrowsConfigurationExceptionIfFileIsMalformed()
            {
                var loader = new BudgetConfigLoader();
                Should.Throw<ConfigurationErrorsException>(() => loader.LoadFile(@"TestData\invalid_config.yml"));
            }
        }
    }
}