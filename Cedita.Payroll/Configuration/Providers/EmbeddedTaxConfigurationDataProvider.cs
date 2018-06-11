﻿// Copyright (c) Cedita Ltd. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the solution root for license information.
using Cedita.Payroll.Abstractions;
using Newtonsoft.Json;
using System.IO;

namespace Cedita.Payroll.Configuration.Providers
{
    public class EmbeddedTaxConfigurationDataProvider : ITaxConfigurationDataProvider
    {
        public TaxYearConfigurationData GetTaxYearConfigurationData(int taxYear)
        {
            var taxData = LoadConfigurationData();
            return taxData.ConfigurationData[taxYear];
        }


        private const string PayrollConfigName = "PayrollConfig";
        private static TaxConfigurationData LoadedTaxConfigurationData;
        private static object lockObj = new object();

        private TaxConfigurationData LoadConfigurationData()
        {
            if (LoadedTaxConfigurationData == null)
            {
                lock (lockObj) {
                    if (LoadedTaxConfigurationData == null) {
                        var asm = typeof(EmbeddedTaxConfigurationDataProvider).Assembly;
                        using (var configStream = asm.GetManifestResourceStream(PayrollConfigName))
                        using (var textReader = new StreamReader(configStream))
                        {
                            var jsonConfig = textReader.ReadToEnd();
                            LoadedTaxConfigurationData = JsonConvert.DeserializeObject<TaxConfigurationData>(jsonConfig);
                        }
                    }
                }
            }

            return LoadedTaxConfigurationData;
        }
    }
}