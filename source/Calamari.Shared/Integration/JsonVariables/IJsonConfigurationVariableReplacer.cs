﻿using Octostache;

namespace Calamari.Integration.JsonVariables
{
    public interface IJsonConfigurationVariableReplacer
    {
        void ModifyJsonFile(string jsonFilePath, IVariables variables);
    }
}