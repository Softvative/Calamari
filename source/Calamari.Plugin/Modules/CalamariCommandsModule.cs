﻿using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Calamari.Commands;
using Calamari.Commands.Support;
using Octopus.CoreUtilities.Extensions;
using Module = Autofac.Module;

namespace Calamari.Modules
{
    /// <summary>
    /// Autofac module to register the calamari commands.
    /// Note that commands were never designed to all be instantiated. We can
    /// only register the commands that are actually being used, rather than
    /// registering them all and filtering later.
    /// </summary>
    public class CalamariCommandsModule : Module
    {
        public static string NormalCommand = "normalCommand";
        private static readonly CommandLocator CommandLocator = new CommandLocator();
        private readonly string commandName;
        private readonly string helpCommandName;
        private readonly Assembly assembly;

        public CalamariCommandsModule(string commandName, string helpCommandName, Assembly assembly)
        {
            this.commandName = commandName;
            this.helpCommandName = helpCommandName;
            this.assembly = assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterNormalCommand(builder);
            RegisterCommandAttributes(builder);
        }

        /// <summary>
        /// Register a "normal" (i.e. not help) command as an ICommand. This is a named
        /// service that is used in CalamariProgramModule.
        /// </summary>
        private Type RegisterNormalCommand(ContainerBuilder builder) =>
            CommandLocator.Find(commandName, assembly)?
                .Tee(command => builder.RegisterType(command).Named<ICommand>(NormalCommand).SingleInstance());


        private void RegisterCommandAttributes(ContainerBuilder builder)
        {
            foreach (var commandMetadata in CommandLocator.List(assembly))
            {
                builder.RegisterInstance(commandMetadata).As<ICommandMetadata>();
            }
        }
    }
}
