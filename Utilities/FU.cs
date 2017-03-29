using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Utilities
{
    public class FU
    {
        public delegate Either<S,E> ItFn<S,E>(S a);
        public delegate bool ItCheck<S,E>(Either<S,E> res);

        /// <summary>
        /// Generic type-safe while, for state iteration until a certain
        /// condition is met, checked by parameter function 'check'.
        /// </summary>
        /// <typeparam name="S">
        /// The state that is updated during the iterations.
        /// </typeparam>
        /// <typeparam name="E">
        /// The error returned if something goes wrong during one
        /// state update.
        /// </typeparam>
        /// <param name="iFn">
        /// The function that is executed each iteration, and that
        /// needs to update the state for the next iteration.
        /// </param>
        /// <param name="check">
        /// Function that checks the actual state and determines if
        /// it's a final state, or stops the execution if it's an error.
        /// </param>
        /// <param name="s">
        /// The initial state from which the iteration is going to start.
        /// </param>
        /// <returns>
        /// Either the final computed state or an Error.
        /// </returns>
        public static Either<S,E> whileS<S,E>(ItFn<S,E> iFn
                                             , ItCheck<S,E> check
                                             , S s) where S : ICloneable
        {
            S iniS = (S)s.Clone();
            Either<S,E> state = iniS;

            while (check(state))
            {
                var nextState = state.Match<Either<S, E>>(
                   Left: (st) =>
                   {
                       return iFn(st);
                   },
                   Right: (err) =>
                   {
                       return err;
                   }
                );
                state = nextState;
            }

            return state;
        }
    }
}
