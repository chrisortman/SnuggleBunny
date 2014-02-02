using System;
using System.Configuration;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace SnuggleBunny.Budget.Config
{
    public class BudgetConfigLoader
    {
        public BudgetConfig LoadFile(string budgetConfigYml)
        {
       
            var config = new BudgetConfig();

            if (File.Exists(budgetConfigYml))
            {
                using (var file = new StreamReader(budgetConfigYml))
                {
                    var yaml = new YamlStream();
                    yaml.Load(file);

                    var rootNode = (YamlMappingNode) yaml.Documents[0].RootNode;

                    try
                    {
                        var categories = (YamlSequenceNode) rootNode.Children[new YamlScalarNode("categories")];
                        foreach (YamlMappingNode categoryNode in categories)
                        {
                            var categoryName = categoryNode.Children[new YamlScalarNode("name")].ToString();
                            var limit = categoryNode.Children[new YamlScalarNode("budget")].ToString();

                            config.DefineCategory(categoryName, Convert.ToDecimal(limit));
                        }
                    }
                    catch (InvalidCastException iEx)
                    {
                        throw new ConfigurationErrorsException(
                            "Invalid configuration file format.", iEx);

                    }

                }
            }

            return config;
        }
    }
}