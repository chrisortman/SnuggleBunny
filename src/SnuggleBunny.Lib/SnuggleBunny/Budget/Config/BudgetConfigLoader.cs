#region LICENSE

// The MIT License (MIT)
// 
// Copyright (c) <year> <copyright holders>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

namespace SnuggleBunny.Budget.Config
{
    using System;
    using System.Configuration;
    using System.IO;
    using Infrastructure;
    using YamlDotNet.RepresentationModel;

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
                        var categories = (YamlSequenceNode) rootNode.Children[new YamlScalarNode("categories")];
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