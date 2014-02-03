using System;
using System.Configuration;
using System.IO;
using SnuggleBunny.Infrastructure;
using YamlDotNet.RepresentationModel;

namespace SnuggleBunny.Budget.Config
{
    public class BudgetConfigLoader
    {
        private BudgetConfig _config;

        public BudgetConfig LoadFile(string budgetConfigYml)
        {

            return Load(new FileDataSource(budgetConfigYml));
        }

        public BudgetConfig Load(IFileDataSource dataSource)
        {
            _config = new BudgetConfig();
            if (dataSource.Exists())
            {
                using (var file = dataSource.ReadStream())
                {
                    var rootNode = LoadYamlRootNode(file);

                    try
                    {
                        var categories = (YamlSequenceNode)rootNode.Children[new YamlScalarNode("categories")];
                        LoadCategories(categories);
                    }
                    catch (InvalidCastException iEx)
                    {
                        throw new ConfigurationErrorsException(
                            "Invalid configuration file format.", iEx);

                    }
                }
            }

            return _config;
        }

        private static YamlMappingNode LoadYamlRootNode(StreamReader file)
        {
            var yaml = new YamlStream();
            yaml.Load(file);

            var rootNode = (YamlMappingNode) yaml.Documents[0].RootNode;
            return rootNode;
        }

        private void LoadCategories(YamlSequenceNode categories)
        {
            foreach (YamlMappingNode categoryNode in categories)
            {
                var categoryName = categoryNode.Children[new YamlScalarNode("name")].ToString();
                var limit = categoryNode.Children[new YamlScalarNode("budget")].ToString();

                _config.DefineCategory(categoryName, Convert.ToDecimal(limit));
            }
        }
    }
}