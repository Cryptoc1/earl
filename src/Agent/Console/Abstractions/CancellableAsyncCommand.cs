using System.Runtime.InteropServices;
using Spectre.Console.Cli;

namespace Earl.Agent.Console.Abstractions;

/// <summary>  Base class for an asynchronous command with no settings that supports cancellation. </summary>
public abstract class CancellableAsyncCommand : AsyncCommand
{
    /// <summary> Executes the command. </summary>
    /// <param name="context"> The command context. </param>
    /// <param name="cancellation"> A token that cancels the command. </param>
    /// <returns> An integer indicating whether or not the command executed successfully. </returns>
    public abstract Task<int> ExecuteAsync( CommandContext context, CancellationToken cancellation );

    /// <inheritdoc/>
    public sealed override async Task<int> ExecuteAsync( CommandContext context )
    {
        using var cancellationSource = new CancellationTokenSource();

        using var sigInt = PosixSignalRegistration.Create( PosixSignal.SIGINT, onSignal );
        using var sigQuit = PosixSignalRegistration.Create( PosixSignal.SIGQUIT, onSignal );
        using var sigTerm = PosixSignalRegistration.Create( PosixSignal.SIGTERM, onSignal );

        var cancellable = ExecuteAsync( context, cancellationSource.Token );
        return await cancellable;

        void onSignal( PosixSignalContext context )
        {
            context.Cancel = true;
            cancellationSource.Cancel();
        }
    }
}