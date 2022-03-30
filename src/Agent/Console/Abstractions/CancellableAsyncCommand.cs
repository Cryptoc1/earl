using System.Runtime.InteropServices;
using Spectre.Console.Cli;

namespace Earl.Agent.Console.Abstractions;

/// <summary> Base class for an asynchronous command with no settings that supports cancellation. </summary>
public abstract class CancellableAsyncCommand : CancellableAsyncCommand<EmptyCommandSettings>
{
    /// <summary> Executes the command. </summary>
    /// <param name="context"> The command context. </param>
    /// <param name="cancellation"> A token that cancels the command. </param>
    /// <returns> An integer indicating whether or not the command executed successfully. </returns>
    public abstract Task<int> ExecuteAsync( CommandContext context, CancellationToken cancellation );

    /// <inheritdoc/>
    public sealed override Task<int> ExecuteAsync( CommandContext context, EmptyCommandSettings settings, CancellationToken cancellation )
        => ExecuteAsync( context, cancellation );
}

/// <summary> Base class for an asynchronous command with settings that supports cancellation. </summary>
/// <typeparam name="TSettings"> The type of settings. </typeparam>
public abstract class CancellableAsyncCommand<TSettings> : AsyncCommand<TSettings>
    where TSettings : CommandSettings
{
    /// <summary> Executes the command. </summary>
    /// <param name="context"> The command context. </param>
    /// <param name="settings"> The settings. </param>
    /// <param name="cancellation"> A token that cancels the command. </param>
    /// <returns> An integer indicating whether or not the command executed successfully. </returns>
    public abstract Task<int> ExecuteAsync( CommandContext context, TSettings settings, CancellationToken cancellation );

    /// <inheritdoc/>
    public sealed override async Task<int> ExecuteAsync( CommandContext context, TSettings settings )
    {
        using var cancellationSource = new CancellationTokenSource();

        using var sigInt = PosixSignalRegistration.Create( PosixSignal.SIGINT, onSignal );
        using var sigQuit = PosixSignalRegistration.Create( PosixSignal.SIGQUIT, onSignal );
        using var sigTerm = PosixSignalRegistration.Create( PosixSignal.SIGTERM, onSignal );

        var cancellable = ExecuteAsync( context, settings, cancellationSource.Token );
        return await cancellable.ConfigureAwait( false );

        void onSignal( PosixSignalContext context ) => cancellationSource.Cancel();
    }
}