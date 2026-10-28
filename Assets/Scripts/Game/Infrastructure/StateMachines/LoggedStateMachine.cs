using System;
using UniState;
using UnityEngine;

namespace Game.Infrastructure.StateMachines
{
    public abstract class LoggedStateMachine : StateMachine
    {
        protected override void HandleStateChanged(StateMachineStateChangedData change)
        {
            Debug.Log($"[{GetType().Name}] {change.PreviousStateType?.Name ?? "Entry"} -> " +
                      $"{change.CurrentStateType?.Name ?? "Exit"}");
        }

        protected override void HandleError(StateMachineErrorData error)
        {
            throw new InvalidOperationException(
                $"{GetType().Name}: {error.State?.GetType().Name ?? "transition"} failed during {error.ErrorType}.",
                error.Exception);
        }
    }
}
